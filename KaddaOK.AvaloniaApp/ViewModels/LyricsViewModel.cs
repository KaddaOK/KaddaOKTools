using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using KaddaOK.Library;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KaddaOK.AvaloniaApp.Models;
using Avalonia.Controls.Notifications;
using KaddaOK.AvaloniaApp.Views;

namespace KaddaOK.AvaloniaApp.ViewModels
{
    public partial class LyricsViewModel : ViewModelBase
    {
        private string? _lyricEditorText;
        public string? LyricEditorText
        {
            get => _lyricEditorText;
            set => SetProperty(ref _lyricEditorText, value);
        }

        private string? _phrasesText;
        public string? PhrasesText
        {
            get => _phrasesText;
            set => SetProperty(ref _phrasesText, value);
        }

        private bool _gettingFile;
        public bool GettingFile
        {
            get => _gettingFile;
            set => SetProperty(ref _gettingFile, value);
        }

        public WindowNotificationManager? NotificationManager { get; set; }

        [RelayCommand]
        protected async Task SelectLyricsFile()
        {
            GettingFile = true;
            if (App.MainWindow == null)
            {
                throw new InvalidOperationException(
                    "Couldn't find the reference to MainWindow in order to show a dialog");
            }

            var options = new FilePickerOpenOptions
            {
                AllowMultiple = false,
                Title = "Select a text file containing the lyrics",
                FileTypeFilter = new FilePickerFileType[] { new ("Text file")
                {
                    Patterns = new[] { "*.txt" }, MimeTypes = new[] { "text/plain" }
                } }
            };
            try
            {
                var results = await App.MainWindow.StorageProvider.OpenFilePickerAsync(options);
                var result = results?.FirstOrDefault();
                if (result != null)
                {
                    CurrentProcess!.LyricsFilePath = result.TryGetLocalPath();
                    // TODO: the data here flows a really wacky way, should maybe rethink
                    var knownLyrics = KnownOriginalLyrics.FromFilePath(CurrentProcess!.LyricsFilePath);
                    if (knownLyrics.UncleansedLines != null)
                    {
                        LyricEditorText = string.Join(Environment.NewLine, knownLyrics.UncleansedLines);
                    }
                }
                GettingFile = false;
            }
            catch (Exception e)
            {
                // The user canceled or something went wrong
                if (NotificationManager != null)
                {
                    NotificationManager.Position = NotificationPosition.BottomRight;
                    NotificationManager.Show(new Notification("Error", $"An error occurred: {e.Message}", NotificationType.Error, TimeSpan.Zero));
                }
                GettingFile = false;
            }
        }

        [RelayCommand]
        public void GoToNextStep(object? parameter)
        {
            switch (CurrentProcess.KaraokeSource)
            {
                case InitialKaraokeSource.ManualSync:
                    SetUpManualAlignment();
                    CurrentProcess.SelectedTabIndex = (int)TabIndexes.ManualAlign;
                    break;
                case InitialKaraokeSource.CtmImport:
                    DoCtmImport();
                    CurrentProcess.SelectedTabIndex = (int)TabIndexes.Edit;
                    break;
                case InitialKaraokeSource.AzureSpeechService:
                    CurrentProcess.SelectedTabIndex = (int)TabIndexes.Recognize;
                    break;
            }
        }

        public void SetUpManualAlignment()
        {
            // TODO: warn the user if CurrentProcess.ManualTimingLines is not null and any have manual start and end set
            var maxSeconds = (CurrentProcess.UnseparatedAudioStream ?? CurrentProcess.VocalsAudioStream)?.TotalTime.TotalSeconds;
            CurrentProcess.ManualTimingLines = new ObservableCollection<ManualTimingLine>
                (
                    CurrentProcess.KnownOriginalLyrics?.UncleansedLines?
                    .Select(l => new ManualTimingLine
                    (
                        LyricWord.GetLyricWordsAcrossTime(l, maxSeconds ?? 0, maxSeconds ?? 0)
                            .Select(TimingWord.FromLyricWord)))
                    ?? new ManualTimingLine[]{}
            );
            CurrentProcess.ManualTimingQueue =
                new ObservableQueue<TimingWord>(CurrentProcess.ManualTimingLines.SelectMany(t => t.Words));
            CurrentProcess.ManualTimingQueue.Peek().IsNext = true;
        }

        public void DoCtmImport()
        {
            var ctmFilePath = CurrentProcess.ImportedKaraokeSourceFilePath;
            if (ctmFilePath == null || !Path.GetExtension(ctmFilePath)
                    .EndsWith("ctm", StringComparison.InvariantCultureIgnoreCase))
            {
                if (NotificationManager != null)
                {
                    NotificationManager.Position = NotificationPosition.BottomRight;
                    NotificationManager.Show(new Notification("Error", "There is no CTM file selected for import!", NotificationType.Error, TimeSpan.Zero));
                }
            }
            var ctmLines = File.ReadAllLines(ctmFilePath).ToList();
            var lyricLines =
                NfaCtmImporter.ImportNfaCtmAndLyrics(ctmLines, CurrentProcess.KnownOriginalLyrics?.SeparatorCleansedLines);

            CurrentProcess.ChosenLines =
                new ObservableCollection<LyricLine>(lyricLines);
            CurrentProcess.RaiseChosenLinesChanged();
            CurrentProcess.NarrowingStepCompletenessChanged();
            CurrentProcess.CanExportFactorsChanged();
        }

        private readonly INfaCtmImporter NfaCtmImporter;
        public LyricsViewModel(KaraokeProcess karaokeProcess, INfaCtmImporter nfaCtmImporter) : base(karaokeProcess)
        {
            NfaCtmImporter = nfaCtmImporter;
        }
    }
}
