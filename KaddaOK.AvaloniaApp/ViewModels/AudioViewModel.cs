using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.Services;
using KaddaOK.Library.Ytmm;

namespace KaddaOK.AvaloniaApp.ViewModels
{
    public partial class AudioViewModel : DrawsFullLengthVocalsBase
    {
        private bool _gettingFile;
        public bool GettingFile
        {
            get => _gettingFile;
            set => SetProperty(ref _gettingFile, value);
        }

        private WaveformDraw? unseparatedAudioDrawnWaveform;
        public WaveformDraw? UnseparatedAudioDrawnWaveform
        {
            get => unseparatedAudioDrawnWaveform;
            set => SetProperty(ref unseparatedAudioDrawnWaveform, value);
        }

        private WaveformDraw? instrumentalAudioDrawnWaveform;
        public WaveformDraw? InstrumentalAudioDrawnWaveform
        {
            get => instrumentalAudioDrawnWaveform;
            set => SetProperty(ref instrumentalAudioDrawnWaveform, value);
        }

        private IRzlrcImporter RzlrcImporter { get; }
        private IKbpImporter KbpImporter { get; }

        public WindowNotificationManager? NotificationManager { get; set; }

        public AudioViewModel(KaraokeProcess karaokeProcess, IRzlrcImporter rzlrcImporter, IKbpImporter kbpImporter) : base(karaokeProcess)
        {
            //ClearVocalsOnlyAudio();
            RzlrcImporter = rzlrcImporter;
            KbpImporter = kbpImporter;
            //ClearUnseparatedAudio();
        }

        [RelayCommand]
        protected void ClearVocalsOnlyAudio()
        {
            GettingFile = false;
            FullLengthVocalsDraw = new WaveformDraw
            {
                DesiredPeakHeight = 150
            };
            CurrentProcess!.VocalsAudioFilePath = null;
            CurrentProcess!.VocalsAudioStream = null;
        }

        [RelayCommand]
        protected void ClearUnseparatedAudio()
        {
            if (UnseparatedAudioDrawnWaveform == null)
            {
                UnseparatedAudioDrawnWaveform = new WaveformDraw
                {
                    DesiredPeakHeight = 100
                };
            }

            UnseparatedAudioDrawnWaveform.CurrentImageSource = null;
            CurrentProcess!.UnseparatedAudioFilePath = null;
            CurrentProcess!.UnseparatedAudioStream = null;
        }

        [RelayCommand]
        protected void ClearInstrumentalAudio()
        {
            if (InstrumentalAudioDrawnWaveform == null)
            {
                InstrumentalAudioDrawnWaveform = new WaveformDraw
                {
                    DesiredPeakHeight = 100
                };
            }

            InstrumentalAudioDrawnWaveform.CurrentImageSource = null;
            CurrentProcess!.InstrumentalAudioFilePath = null;
            CurrentProcess!.InstrumentalAudioStream = null;
        }

        [RelayCommand]
        protected void ClearAll()
        {
            // TODO: show confirm
            CurrentProcess?.ClearAudioAndDownstream();
        }

        [RelayCommand]
        protected async Task Import()
        {
            if (App.MainWindow == null)
            {
                throw new InvalidOperationException(
                    "Couldn't find the reference to MainWindow in order to show a dialog");
            }

            var options = new FilePickerOpenOptions
            {
                AllowMultiple = false,
                Title = "Select an existing file to import",
                FileTypeFilter = new FilePickerFileType[] { new ("Karaoke lyrics file")
                {
                    Patterns = new[] { "*.rzlrc", "*.kbp" }
                } }
            };
            try
            {
                Dispatcher.UIThread.Invoke(() => { GettingFile = true; });
                var results = await App.MainWindow.StorageProvider.OpenFilePickerAsync(options);
                var result = results?.FirstOrDefault();
                if (result != null)
                {
                    var previousFilePath = CurrentProcess!.ExistingKaraokeImportFilePath;
                    var existingKaraokeImportFilePath = result.TryGetLocalPath();

                    if (!string.IsNullOrWhiteSpace(existingKaraokeImportFilePath))
                    {
                        var extension = System.IO.Path.GetExtension(existingKaraokeImportFilePath);
                        switch (extension)
                        {
                            case ".rzlrc":
                                var imported = RzlrcImporter.ImportRzlrc(existingKaraokeImportFilePath);

                                // TODO: if (imported == null || imported.Count == 0) // inform and cancel
                                RzlrcLyric? selectedLayer = null;
                                if (imported.Count > 1) // select one
                                {
                                    var dialogResult = await DialogHost.Show(imported, "AudioViewDialogHost");
                                    selectedLayer = dialogResult as RzlrcLyric;
                                }
                                CurrentProcess.ClearAudioAndDownstream();
                                CurrentProcess.OriginalImportedRzlrcFile = imported;
                                CurrentProcess.OriginalImportedRzlrcPage = selectedLayer ?? imported.First();
                                await RzlrcImporter.LoadRzlrcPageIntoKaraokeProcessAsync(CurrentProcess, CurrentProcess.OriginalImportedRzlrcPage, existingKaraokeImportFilePath);
                                break;
                            case ".kbp":
                                CurrentProcess.ClearAudioAndDownstream();
                                await KbpImporter.ImportKbpAsync(CurrentProcess, existingKaraokeImportFilePath);
                                break;
                            default:
                                // TODO: inform better!
                                throw new InvalidOperationException($"Import of {extension} files not yet supported.");
                        }
                    }
                    CurrentProcess!.SelectedTabIndex = 4;
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
            CurrentProcess!.SelectedTabIndex = 1;
        }

        [RelayCommand]
        private void LinkToUVR()
        {
            UrlOpener.OpenUrl("https://github.com/Anjok07/ultimatevocalremovergui");
        }
    }
}
