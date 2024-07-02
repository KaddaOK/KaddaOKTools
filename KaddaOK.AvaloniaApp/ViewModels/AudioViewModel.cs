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
using KaddaOK.AvaloniaApp.Views;
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
        public void GoToNextStep(object? parameter)
        {
            switch (CurrentProcess.KaraokeSource)
            {
                case InitialKaraokeSource.CtmImport:
                case InitialKaraokeSource.KbpImport:
                case InitialKaraokeSource.RzlrcImport:
                    CurrentProcess.SelectedTabIndex = (int)TabIndexes.Edit;
                    break;
                default:
                    CurrentProcess.SelectedTabIndex = (int)TabIndexes.Lyrics;
                    break;
            }
        }

        [RelayCommand]
        private void LinkToUVR()
        {
            UrlOpener.OpenUrl("https://github.com/Anjok07/ultimatevocalremovergui");
        }
    }
}
