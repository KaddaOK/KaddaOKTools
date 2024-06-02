using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using KaddaOK.Library;
using System.Windows.Input;
using Avalonia.Controls.Notifications;
using Avalonia.Platform.Storage;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.Services;
using KaddaOK.AvaloniaApp.Views;

namespace KaddaOK.AvaloniaApp.ViewModels
{
    public partial class RecognizeViewModel : DrawsFullLengthVocalsBase
    {
        private ObservableCollection<string>? _logContents;
        public ObservableCollection<string>? LogContents
        {
            get => _logContents;
            set => SetProperty(ref _logContents, value);
        }

        private bool? _hasEverBeenStarted;
        public bool? HasEverBeenStarted
        {
            get => _hasEverBeenStarted;
            set => SetProperty(ref _hasEverBeenStarted, value);
        }

        private KaddaOKSettings recognizeSettings = null!;
        public KaddaOKSettings RecognizeSettings
        {
            get => recognizeSettings;
            set => SetProperty(ref recognizeSettings, value);
        }

        private bool _gettingFile;
        public bool GettingFile
        {
            get => _gettingFile;
            set => SetProperty(ref _gettingFile, value);
        }

        public IAzureRecognizer AzureRecognizer { get; }
        private IKaddaOKSettingsPersistor SettingsPersistor { get; }
        private INfaCtmImporter NfaCtmImporter { get; }
        public WindowNotificationManager? NotificationManager { get; set; }
        public RecognizeViewModel(IAzureRecognizer recognizer, IKaddaOKSettingsPersistor settingsPersistor, INfaCtmImporter cfmImporter, KaraokeProcess karaokeProcess) : base(karaokeProcess)
        {
            LogContents = new ObservableCollection<string>();
            AzureRecognizer = recognizer;
            SettingsPersistor = settingsPersistor;
            NfaCtmImporter = cfmImporter;
            FullLengthVocalsDraw = new WaveformDraw
            {
                DesiredPeakHeight = 200
            };
            HasEverBeenStarted = false;
            Dispatcher.UIThread.Invoke(() => FullLengthVocalsDraw.RedrawWaveform(CurrentProcess!.VocalsAudioStream));
            RecognizeSettings = SettingsPersistor.LoadState();
            RecognizeSettings.RecognitionLanguage ??= new SupportedLanguage { Bcp47 = "en-US", DisplayName = "English (United States)" };
        }

        public ICommand RecognizeCommand => new RelayCommandWithReason((parameter) =>
        {
            Dispatcher.UIThread.Invoke(Recognize);
        }, CanStartRecognizing);

        internal bool CanStartRecognizing(object? parameter, IReportReasonCantExecute reporter)
        {
            if (CurrentProcess!.VocalsAudioFilePath == null)
            {
                reporter.ReasonCantExecute = "Wave file selection is required.";
                return false;
            }

            if (!CurrentProcess!.KnownOriginalLyrics?.SeparatorCleansedLines?.Any() ?? true)
            {
                reporter.ReasonCantExecute = "Lyrics must be supplied for a quality result.";
                return false;
            }

            if (CurrentProcess!.RecognitionIsRunning)
            {
                reporter.ReasonCantExecute = "The recognition process is already running.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(RecognizeSettings?.AzureSpeechKey))
            {
                reporter.ReasonCantExecute = "Speech Service Key is required.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(RecognizeSettings?.AzureSpeechRegion))
            {
                reporter.ReasonCantExecute = "Speech Service Location/Region is required.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(RecognizeSettings?.RecognitionLanguage?.Bcp47))
            {
                reporter.ReasonCantExecute = "Language selection is required.";
                return false;
            }

            return true;
        }

        private void PossibilitiesDetected(LinePossibilities possibilities)
        {
            CurrentProcess!.DetectedLinePossibilities!.Add(possibilities);

        }

        async Task Recognize()
        {
            HasEverBeenStarted = true;

            //await FullLengthVocalsDraw.RedrawWaveform(CurrentProcess.VocalsAudioStream);

            SettingsPersistor.SaveState(RecognizeSettings);

            CurrentProcess!.DetectedLinePossibilities = new ObservableCollection<LinePossibilities>();

            CurrentProcess!.RecognitionIsRunning = true;
            await AzureRecognizer.Recognize(
                RecognizeSettings!.AzureSpeechKey!,
                RecognizeSettings!.AzureSpeechRegion!,
                RecognizeSettings!.RecognitionLanguage?.Bcp47 ?? "en-US",
                CurrentProcess.VocalsAudioStream!,
                CurrentProcess!.KnownOriginalLyrics!,
                PossibilitiesDetected,
                AppendLogLine);
            CurrentProcess!.RecognitionIsRunning = false;
            CurrentProcess!.NarrowingStepCompletenessChanged();
        }

        [RelayCommand]
        protected async Task CancelRecognition()
        {
            await AzureRecognizer.CancelRecognition(AppendLogLine);
        }

        private void AppendLogLine(string line)
        {
            Debug.WriteLine(line);
            LogContents!.Add(line);
        }

        [RelayCommand]
        public void GoToNextStep(object? parameter)
        {
            CurrentProcess!.SelectedTabIndex = (int)TabIndexes.Narrow;
        }

        [RelayCommand]
        private void LinkToAzureSpeechService()
        {
            UrlOpener.OpenUrl("https://azure.microsoft.com/en-us/products/ai-services/speech-to-text");
        }
    }
}
