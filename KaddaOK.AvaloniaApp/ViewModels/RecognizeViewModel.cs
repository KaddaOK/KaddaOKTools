using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using KaddaOK.Library;
using System.Windows.Input;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.Services;

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

        public IAzureRecognizer AzureRecognizer { get; }
        private IKaddaOKSettingsPersistor SettingsPersistor { get; }
        public RecognizeViewModel(IAzureRecognizer recognizer, IKaddaOKSettingsPersistor settingsPersistor, KaraokeProcess karaokeProcess) : base(karaokeProcess)
        {
            LogContents = new ObservableCollection<string>();
            AzureRecognizer = recognizer;
            SettingsPersistor = settingsPersistor;
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

            if (!CurrentProcess!.KnownOriginalLyrics?.Lyrics?.Any() ?? true)
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
            CurrentProcess!.SelectedTabIndex = 3;
        }

        [RelayCommand]
        private void LinkToAzureSpeechService()
        {
            UrlOpener.OpenUrl("https://azure.microsoft.com/en-us/products/ai-services/speech-to-text");
        }

        public List<SupportedLanguage> SupportedLanguages =
        //public List<(string bcp47, string displayName)> SupportedLanguages =
            new()
            {
                /*("af-ZA", "Afrikaans (South Africa)"),
                ("am-ET", "Amharic (Ethiopia)"),
                ("ar-AE", "Arabic (United Arab Emirates)"),
                ("ar-BH", "Arabic (Bahrain)"),
                ("ar-DZ", "Arabic (Algeria)"),
                ("ar-EG", "Arabic (Egypt)"),
                ("ar-IL", "Arabic (Israel)"),
                ("ar-IQ", "Arabic (Iraq)"),
                ("ar-JO", "Arabic (Jordan)"),
                ("ar-KW", "Arabic (Kuwait)"),
                ("ar-LB", "Arabic (Lebanon)"),
                ("ar-LY", "Arabic (Libya)"),
                ("ar-MA", "Arabic (Morocco)"),
                ("ar-OM", "Arabic (Oman)"),
                ("ar-PS", "Arabic (Palestinian Authority)"),
                ("ar-QA", "Arabic (Qatar)"),
                ("ar-SA", "Arabic (Saudi Arabia)"),
                ("ar-SY", "Arabic (Syria)"),
                ("ar-TN", "Arabic (Tunisia)"),
                ("ar-YE", "Arabic (Yemen)"),
                ("az-AZ", "Azerbaijani (Latin, Azerbaijan)"),
                ("bg-BG", "Bulgarian (Bulgaria)"),
                ("bn-IN", "Bengali (India)"),
                ("bs-BA", "Bosnian (Bosnia and Herzegovina)"),
                ("ca-ES", "Catalan"),
                ("cs-CZ", "Czech (Czechia)"),
                ("cy-GB", "Welsh (United Kingdom)"),
                ("da-DK", "Danish (Denmark)"),
                ("de-AT", "German (Austria)"),
                ("de-CH", "German (Switzerland)"),
                ("de-DE", "German (Germany)"),
                ("el-GR", "Greek (Greece)"),
                ("en-AU", "English (Australia)"),
                ("en-CA", "English (Canada)"),
                ("en-GB", "English (United Kingdom)"),
                ("en-GH", "English (Ghana)"),
                ("en-HK", "English (Hong Kong SAR)"),
                ("en-IE", "English (Ireland)"),
                ("en-IN", "English (India)"),
                ("en-KE", "English (Kenya)"),
                ("en-NG", "English (Nigeria)"),
                ("en-NZ", "English (New Zealand)"),
                ("en-PH", "English (Philippines)"),
                ("en-SG", "English (Singapore)"),
                ("en-TZ", "English (Tanzania)"),
                ("en-US", "English (United States)"),
                ("en-ZA", "English (South Africa)"),
                ("es-AR", "Spanish (Argentina)"),
                ("es-BO", "Spanish (Bolivia)"),
                ("es-CL", "Spanish (Chile)"),
                ("es-CO", "Spanish (Colombia)"),
                ("es-CR", "Spanish (Costa Rica)"),
                ("es-CU", "Spanish (Cuba)"),
                ("es-DO", "Spanish (Dominican Republic)"),
                ("es-EC", "Spanish (Ecuador)"),
                ("es-ES", "Spanish (Spain)"),
                ("es-GQ", "Spanish (Equatorial Guinea)"),
                ("es-GT", "Spanish (Guatemala)"),
                ("es-HN", "Spanish (Honduras)"),
                ("es-MX", "Spanish (Mexico)"),
                ("es-NI", "Spanish (Nicaragua)"),
                ("es-PA", "Spanish (Panama)"),
                ("es-PE", "Spanish (Peru)"),
                ("es-PR", "Spanish (Puerto Rico)"),
                ("es-PY", "Spanish (Paraguay)"),
                ("es-SV", "Spanish (El Salvador)"),
                ("es-US", "Spanish (United States)"),
                ("es-UY", "Spanish (Uruguay)"),
                ("es-VE", "Spanish (Venezuela)"),
                ("et-EE", "Estonian (Estonia)"),
                ("eu-ES", "Basque"),
                ("fa-IR", "Persian (Iran)"),
                ("fi-FI", "Finnish (Finland)"),
                ("fil-PH", "Filipino (Philippines)"),
                ("fr-BE", "French (Belgium)"),
                ("fr-CA", "French (Canada)"),
                ("fr-CH", "French (Switzerland)"),
                ("fr-FR", "French (France)"),
                ("ga-IE", "Irish (Ireland)"),
                ("gl-ES", "Galician"),
                ("gu-IN", "Gujarati (India)"),
                ("he-IL", "Hebrew (Israel)"),
                ("hi-IN", "Hindi (India)"),
                ("hr-HR", "Croatian (Croatia)"),
                ("hu-HU", "Hungarian (Hungary)"),
                ("hy-AM", "Armenian (Armenia)"),
                ("id-ID", "Indonesian (Indonesia)"),
                ("is-IS", "Icelandic (Iceland)"),
                ("it-CH", "Italian (Switzerland)"),
                ("it-IT", "Italian (Italy)"),
                ("ja-JP", "Japanese (Japan)"),
                ("jv-ID", "Javanese (Latin, Indonesia)"),
                ("ka-GE", "Georgian (Georgia)"),
                ("kk-KZ", "Kazakh (Kazakhstan)"),
                ("km-KH", "Khmer (Cambodia)"),
                ("kn-IN", "Kannada (India)"),
                ("ko-KR", "Korean (Korea)"),
                ("lo-LA", "Lao (Laos)"),
                ("lt-LT", "Lithuanian (Lithuania)"),
                ("lv-LV", "Latvian (Latvia)"),
                ("mk-MK", "Macedonian (North Macedonia)"),
                ("ml-IN", "Malayalam (India)"),
                ("mn-MN", "Mongolian (Mongolia)"),
                ("mr-IN", "Marathi (India)"),
                ("ms-MY", "Malay (Malaysia)"),
                ("mt-MT", "Maltese (Malta)"),
                ("my-MM", "Burmese (Myanmar)"),
                ("nb-NO", "Norwegian Bokmål (Norway)"),
                ("ne-NP", "Nepali (Nepal)"),
                ("nl-BE", "Dutch (Belgium)"),
                ("nl-NL", "Dutch (Netherlands)"),
                ("pa-IN", "Punjabi (India)"),
                ("pl-PL", "Polish (Poland)"),
                ("ps-AF", "Pashto (Afghanistan)"),
                ("pt-BR", "Portuguese (Brazil)"),
                ("pt-PT", "Portuguese (Portugal)"),
                ("ro-RO", "Romanian (Romania)"),
                ("ru-RU", "Russian (Russia)"),
                ("si-LK", "Sinhala (Sri Lanka)"),
                ("sk-SK", "Slovak (Slovakia)"),
                ("sl-SI", "Slovenian (Slovenia)"),
                ("so-SO", "Somali (Somalia)"),
                ("sq-AL", "Albanian (Albania)"),
                ("sr-RS", "Serbian (Cyrillic, Serbia)"),
                ("sv-SE", "Swedish (Sweden)"),
                ("sw-KE", "Swahili (Kenya)"),
                ("sw-TZ", "Swahili (Tanzania)"),
                ("ta-IN", "Tamil (India)"),
                ("te-IN", "Telugu (India)"),
                ("th-TH", "Thai (Thailand)"),
                ("tr-TR", "Turkish (Türkiye)"),
                ("uk-UA", "Ukrainian (Ukraine)"),
                ("ur-IN", "Urdu (India)"),
                ("uz-UZ", "Uzbek (Latin, Uzbekistan)"),
                ("vi-VN", "Vietnamese (Vietnam)"),
                ("wuu-CN", "Chinese (Wu, Simplified)"),
                ("yue-CN", "Chinese (Cantonese, Simplified)"),
                ("zh-CN", "Chinese (Mandarin, Simplified)"),
                ("zh-CN-shandong", "Chinese (Jilu Mandarin, Simplified)"),
                ("zh-CN-sichuan", "Chinese (Southwestern Mandarin, Simplified)"),
                ("zh-HK", "Chinese (Cantonese, Traditional)"),
                ("zh-TW", "Chinese (Taiwanese Mandarin, Traditional)"),
                ("zu-ZA", "Zulu (South Africa)"),*/
            };
    }
}
