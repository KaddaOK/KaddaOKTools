using Microsoft.CognitiveServices.Speech;
using System.Collections.ObjectModel;
using KaddaOK.Library.Kbs;
using KaddaOK.Library.Ytmm;

namespace KaddaOK.Library
{
    public class LyricLine : ObservableBase, IAudioSpan
    {
        public string? Text => string.Concat(Words.Select(w => w.Text));

        #region persistence from KBP import only

        private int? pageIndex;
        public int? PageIndex
        {
            get => pageIndex;
            set => SetProperty(ref pageIndex, value);
        }

        private short? kbpStyleIndex;
        public short? KbpStyleIndex
        {
            get => kbpStyleIndex;
            set => SetProperty(ref kbpStyleIndex, value);
        }

        private bool kbpIsFixedText;
        public bool KbpIsFixedText
        {
            get => kbpIsFixedText;
            set => SetProperty(ref kbpIsFixedText, value);
        }

        private KbpLine.HorizontalAlignmentType? kbpAlignmentType;
        public KbpLine.HorizontalAlignmentType? KbpAlignmentType
        {
            get => kbpAlignmentType;
            set => SetProperty(ref kbpAlignmentType, value);
        }

        #endregion

        private ObservableCollection<LyricWord> words;
        public ObservableCollection<LyricWord> Words
        {
            get => words;
            set
            {
                SetProperty(ref words, value);
                RaisePropertyChanged(nameof(StartSecond));
                RaisePropertyChanged(nameof(EndSecond));
            }
        }

        public double StartSecond
        {
            get
            {
                if (Words.Any())
                {
                    return Math.Round(Words.Select(w => w.StartSecond).Min(), 2);
                }

                return 0;
            }
        }

        public double EndSecond
        {
            get
            {
                if (Words != null && Words.Any())
                {
                    return Math.Round(Words.Select(w => w.EndSecond).Max(), 2);
                }

                return 0;
            }
        }

        private bool? isSelected;
        public bool? IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        private LinePossibilities? inPossibilities;
        public LinePossibilities? InPossibilities
        {
            get => inPossibilities;
            set => SetProperty(ref inPossibilities, value);
        }

        public LyricLine(DetailedSpeechRecognitionResult fromAzure)
        {
            words = new ObservableCollection<LyricWord>(
                fromAzure.Words?.Select(w => new LyricWord(w)) ?? Array.Empty<LyricWord>());

            // azure doesn't have syllables, the best we get is words, but to support syllables we 
            // need to move spaces inside the individual word entries
            foreach (var word in Words)
            {
                if (word != Words.Last())
                {
                    word.Text += " ";
                }
            }
        }

        public LyricItem? OriginalRzlrcItem { get; }
        public LyricLine(LyricItem rzlrcItem)
        {
            OriginalRzlrcItem = rzlrcItem;
            words = new ObservableCollection<LyricWord>();
        }

        public LyricLine()
        {
            words = new ObservableCollection<LyricWord>();
        }

        public static void MoveSpacesToEndsOfWords(IList<LyricWord> words)
        {
            for (int i = 0; i < words.Count; i++)
            {
                var thisWord = words[i];
                var previousWord = i == 0 ? null : words[i - 1];
                if (thisWord.Text?.StartsWith(" ") ?? false)
                {
                    thisWord.Text = thisWord.Text.TrimStart();
                    if (previousWord != null && (!previousWord.Text?.EndsWith(" ") ?? true))
                    {
                        previousWord.Text = $"{previousWord.Text} ";
                    }
                }
            }
        }
    }
}
