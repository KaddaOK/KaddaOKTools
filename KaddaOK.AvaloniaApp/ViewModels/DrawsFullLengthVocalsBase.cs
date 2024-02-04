using KaddaOK.AvaloniaApp.Models;

namespace KaddaOK.AvaloniaApp.ViewModels
{
    public partial class DrawsFullLengthVocalsBase : ViewModelBase
    {
        private WaveformDraw _fullLengthVocalsDraw = null!;
        public WaveformDraw FullLengthVocalsDraw
        {
            get => _fullLengthVocalsDraw;
            set => SetProperty(ref _fullLengthVocalsDraw, value);
        }

        public DrawsFullLengthVocalsBase(KaraokeProcess karaokeProcess) : base(karaokeProcess)
        {
            FullLengthVocalsDraw = new WaveformDraw
            {
                Drawing = false
            };
        }
    }
}
