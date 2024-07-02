using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Avalonia.Platform.Storage;
using Avalonia.Skia;
using CommunityToolkit.Mvvm.Input;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.Services;
using KaddaOK.Library;
using KaddaOK.Library.Ytmm;

namespace KaddaOK.AvaloniaApp.ViewModels
{
    public partial class ExportViewModel : ViewModelBase
    {
        private IRzlrcContentsGenerator RzlrcContentsGenerator { get; }
        private IKbpContentsGenerator KbpContentsGenerator { get; }
        private IRzProjectGenerator RzProjectGenerator { get; }
        private IRzProjectSerializer RzProjectSerializer { get; }
        private IAudioFileLengthChecker LengthChecker { get; }

        public WindowNotificationManager? NotificationManager { get; set; }

        public ExportViewModel(KaraokeProcess karaokeProcess,
            IRzlrcContentsGenerator rzlrcContentsGenerator,
            IKbpContentsGenerator kbpContentsGenerator,
            IRzProjectGenerator rzProjectGenerator,
            IRzProjectSerializer rzProjectSerializer,
            IAudioFileLengthChecker lengthChecker) : base(karaokeProcess)
        {
            RzlrcContentsGenerator = rzlrcContentsGenerator;
            KbpContentsGenerator = kbpContentsGenerator;
            RzProjectGenerator = rzProjectGenerator;
            RzProjectSerializer = rzProjectSerializer;
            LengthChecker = lengthChecker;
        }

        [RelayCommand]
        protected async Task SelectRzTemplate()
        {
            if (App.MainWindow == null)
            {
                throw new InvalidOperationException(
                    "Couldn't find the reference to MainWindow in order to show a dialog");
            }

            var options = new FilePickerOpenOptions
            {
                AllowMultiple = false,
                Title = "Select a project file to use as a template",
                FileTypeFilter = new FilePickerFileType[] { new ("YTMM Project (.rzmmpj)")
                {
                    Patterns = new[] { "*.rzmmpj" }
                } }
            };

            var results = await App.MainWindow.StorageProvider.OpenFilePickerAsync(options);
            var result = results.FirstOrDefault();
            if (result != null)
            {
                CurrentProcess.RzProjectTemplatePath = result.TryGetLocalPath();
            }
        }

        [RelayCommand]
        protected async Task SelectRzVideo()
        {
            if (App.MainWindow == null)
            {
                throw new InvalidOperationException(
                    "Couldn't find the reference to MainWindow in order to show a dialog");
            }

            var options = new FilePickerOpenOptions
            {
                AllowMultiple = false,
                Title = "Select a video file to include",
                FileTypeFilter = new FilePickerFileType[] { new ("Video files")
                {
                    Patterns = new[] {"*.mp4", "*.m4v", "*.avi", "*.wmv", "*.mov", "*.flv", "*.mpg", "*.webm" }
                } }
            };

            var results = await App.MainWindow.StorageProvider.OpenFilePickerAsync(options);
            var result = results.FirstOrDefault();
            var localPath = result?.TryGetLocalPath();
            if (localPath != null)
            {
                var videoLength = LengthChecker.CheckAudioLength(localPath);
                if (videoLength is { TotalSeconds: > 1 })
                {
                    CurrentProcess!.RzVideoPath = localPath;
                    CurrentProcess!.RzVideoLength = videoLength.Value.TotalSeconds;
                }
                else
                {
                    CurrentProcess!.UseRzVideo = false;
                    // TODO: tell the user we couldn't read their file
                }
            }
        }

        [RelayCommand]
        protected async Task ExportToRzlrc()
        {
            string contents;
            if (CurrentProcess.KaraokeSource == InitialKaraokeSource.RzlrcImport)
            {
                contents = RzlrcContentsGenerator.InterpolateFileContents(
                    CurrentProcess.OriginalImportedRzlrcFile,
                    CurrentProcess.OriginalImportedRzlrcPage,
                    CurrentProcess.ChosenLines); // TODO: lock down the colors or involve them
            }
            else
            {
                contents = RzlrcContentsGenerator.GenerateRzlrcFileContents(
                    CurrentProcess.ChosenLines!,
                    (CurrentProcess.UnseparatedAudioFilePath ?? CurrentProcess!.InstrumentalAudioFilePath ?? CurrentProcess!.VocalsAudioFilePath)!,
                    CurrentProcess.LyricsFilePath,
                    CurrentProcess.BackgroundColor.ToSKColor(),
                    CurrentProcess.UnsungTextColor.ToSKColor(),
                    CurrentProcess.UnsungOutlineColor.ToSKColor(),
                    CurrentProcess.SungTextColor.ToSKColor(),
                    CurrentProcess.SungOutlineColor.ToSKColor());
            }

            if (App.MainWindow == null)
            {
                throw new InvalidOperationException(
                    "Couldn't find the reference to MainWindow in order to show a dialog");
            }

            var options = new FilePickerSaveOptions
            {
                Title = "Export to YTMM",
                DefaultExtension = ".rzlrc",
                FileTypeChoices = new FilePickerFileType[] { new ("YTMM Lyrics File")
                {
                    Patterns = new[] { "*.rzlrc" }, MimeTypes = new[] { "application/xml" }
                } }
            };
            try
            {
                var file = await App.MainWindow.StorageProvider.SaveFilePickerAsync(options);
                if (file != null)
                {
                    await using var stream = await file.OpenWriteAsync();
                    // UTF-16 LE BOM
                    Encoding encoding = new UnicodeEncoding(false, true);

                    using var streamWriter = new StreamWriter(stream, encoding)
                    {
                        AutoFlush = true
                    };
                    await streamWriter.WriteAsync(new StringBuilder(contents));

                    var pathToLaunch = file.Path.ToString();
                    if (CurrentProcess.GenerateRzProject)
                    {
                        var lyricPath = file.Path.LocalPath;
                        var projectPath = Path.Combine(
                                Path.GetDirectoryName(lyricPath) ?? "",
                                Path.GetFileNameWithoutExtension(lyricPath) + ".rzmmpj");

                        RzProject? template = null;
                        if (CurrentProcess.UseRzProjectTemplate &&
                            !string.IsNullOrWhiteSpace(CurrentProcess.RzProjectTemplatePath))
                        {
                            template = RzProjectSerializer.Deserialize(
                                File.ReadAllText(CurrentProcess.RzProjectTemplatePath));
                        }

                        var generatedProject = RzProjectGenerator.GenerateProject(lyricPath,
                            (decimal)(CurrentProcess.VocalsAudioStream ?? CurrentProcess.UnseparatedAudioStream)!.TotalTime.TotalSeconds,
                            CurrentProcess.VocalsAudioFilePath,
                            CurrentProcess.UnseparatedAudioFilePath,
                            CurrentProcess.InstrumentalAudioFilePath,
                            CurrentProcess.UseRzVideo ? CurrentProcess.RzVideoPath : null,
                            CurrentProcess.RzVideoAsOverlay,
                            CurrentProcess.UseRzVideo ? (decimal)CurrentProcess.RzVideoLength : null,
                            CurrentProcess.BackgroundColor.ToSKColor(),
                            template);
                        if (CurrentProcess.InsertProgressBars && CurrentProcess.ChosenLines != null)
                        {
                            RzProjectGenerator.InsertProgressBars(
                                generatedProject,
                                CurrentProcess.ChosenLines.ToList(),
                                CurrentProcess.ProgressBarGapLength,
                                CurrentProcess.ProgressBarWidth,
                                CurrentProcess.ProgressBarHeight,
                                CurrentProcess.ProgressBarYPosition,
                                CurrentProcess.ProgressBarFillColor.ToSKColor(),
                                CurrentProcess.ProgressBarOutlineColor.ToSKColor());
                        }

                        var projectContents = RzProjectSerializer.Serialize(generatedProject);
                        using var writer = new StreamWriter(projectPath, false, encoding);
                        await writer.WriteAsync(projectContents);

                        pathToLaunch = projectPath;
                    }

                    if (CurrentProcess.LaunchResult && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        using var proc = new Process { StartInfo = { UseShellExecute = true, FileName = pathToLaunch } };
                        proc.Start();
                    }
                }
            }
            catch (Exception e)
            {
                // The user canceled or something went wrong
                if (NotificationManager != null)
                {
                    NotificationManager.Position = NotificationPosition.BottomRight;
                    NotificationManager.Show(new Notification("Error", $"An error occurred: {e.Message}", NotificationType.Error, TimeSpan.Zero));
                }
            }
        }

        [RelayCommand]
        protected async Task ExportToKbp()
        {
            var contents = KbpContentsGenerator.GenerateKbpFileContents(
                (CurrentProcess.UnseparatedAudioFilePath ?? CurrentProcess.VocalsAudioFilePath)!,
                CurrentProcess.ChosenLines!,
                CurrentProcess.BackgroundColor.ToSKColor(),
                CurrentProcess.UnsungTextColor.ToSKColor(), CurrentProcess.UnsungOutlineColor.ToSKColor(),
                CurrentProcess.SungTextColor.ToSKColor(), CurrentProcess.SungOutlineColor.ToSKColor(),
                customHeader: CurrentProcess.KaraokeSource == InitialKaraokeSource.KbpImport
                            ? CurrentProcess.OriginalImportedKbpFile?.Header?.ToString()
                            : null);

            if (App.MainWindow == null)
            {
                throw new InvalidOperationException(
                    "Couldn't find the reference to MainWindow in order to show a dialog");
            }

            var options = new FilePickerSaveOptions
            {
                Title = "Export to KBS",
                DefaultExtension = ".kbp",
                FileTypeChoices = new FilePickerFileType[] { new ("KBS Lyrics File")
                {
                    Patterns = new[] { "*.kbp" }, MimeTypes = new[] { "application/flatfile" }
                } }
            };
            try
            {
                var file = await App.MainWindow.StorageProvider.SaveFilePickerAsync(options);
                if (file != null)
                {
                    await using var stream = await file.OpenWriteAsync();
                    using var streamWriter = new StreamWriter(stream)
                    {
                        AutoFlush = true
                    };
                    await streamWriter.WriteAsync(new StringBuilder(contents));

                    if (CurrentProcess.LaunchResult && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        using var proc = new Process { StartInfo = { UseShellExecute = true, FileName = file.Path.ToString() } };
                        proc.Start();
                    }
                }
            }
            catch (Exception e)
            {
                // The user canceled or something went wrong
                if (NotificationManager != null)
                {
                    NotificationManager.Position = NotificationPosition.BottomRight;
                    NotificationManager.Show(new Notification("Error", $"An error occurred: {e.Message}", NotificationType.Error, TimeSpan.Zero));
                }
            }
        }

        [RelayCommand]
        private void LinkToYTMM()
        {
            UrlOpener.OpenUrl("https://www.makeyoutubevideo.com/");
        }

        [RelayCommand]
        private void LinkToKBS()
        {
            UrlOpener.OpenUrl("https://www.karaokebuilder.com/kbstudio.php");
        }
    }
}
