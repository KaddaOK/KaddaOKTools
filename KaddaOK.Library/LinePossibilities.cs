using System.Collections.ObjectModel;

namespace KaddaOK.Library
{
    public class LinePossibilities : ObservableBase, IAudioSpan
    {
        public LinePossibilities(IEnumerable<LyricLine> lyrics)
        {
            Lyrics = new ObservableCollection<LyricLine>(lyrics);
            foreach (var lyric in Lyrics)
            {
                lyric.InPossibilities = this;
            }
        }
        public double StartSecond => Lyrics?.Select(w => w.StartSecond).DefaultIfEmpty().Min() ?? 0;
        public double EndSecond => Lyrics?.Select(w => w.EndSecond).DefaultIfEmpty().Max() ?? 0;

        private ObservableCollection<LyricLine> lyrics = null!;
        public ObservableCollection<LyricLine> Lyrics
        {
            get => lyrics;
            set
            {
                if (SetProperty(ref lyrics, value))
                {
                    RaisePropertyChanged(nameof(StartSecond));
                    RaisePropertyChanged(nameof(EndSecond));
                };
            }
        }

        public bool HasSelected => SelectedLyric != null;

        private LyricLine? selectedLyric;
        public LyricLine? SelectedLyric
        {
            get => selectedLyric;
            set
            {
                if (SetProperty(ref selectedLyric, value))
                {
                    RaisePropertyChanged(nameof(HasSelected));
                }
            }
        }

        private bool isIgnored;
        public bool IsIgnored
        {
            get => isIgnored;
            set => SetProperty(ref isIgnored, value);
        }
    }
}
