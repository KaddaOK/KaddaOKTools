using System.Collections.ObjectModel;

namespace KaddaOK.Library
{
    public interface ILineSplitter
    {
        void SplitLineAt(ObservableCollection<LyricLine>? allLines, LyricWord wordToSplit, bool splitBefore);
        void DeleteWord(ObservableCollection<LyricLine> allLines, LyricWord wordToDelete);
    }

    public class LineSplitter : ILineSplitter
    {
        public void SplitLineAt(ObservableCollection<LyricLine>? allLines, LyricWord wordToSplit, bool splitBefore)
        {
            var originalLine = allLines?.SingleOrDefault(l => l.Words != null && l.Words.Contains(wordToSplit));
            if (originalLine != null)
            {
                var originalLineIndex = allLines!.IndexOf(originalLine);
                if (originalLine.Words != null)
                {
                    var originalWordIndex = originalLine.Words.IndexOf(wordToSplit);

                    var newLine = new LyricLine
                    {
                        IsSelected = originalLine.IsSelected,
                        Words = new ObservableCollection<LyricWord>(originalLine.Words.Skip(originalWordIndex + (splitBefore ? 0 : 1)))
                    };

                    originalLine.Words = new ObservableCollection<LyricWord>(originalLine.Words.Take(splitBefore ? originalWordIndex : originalWordIndex + 1));
                    originalLine.Words.Last().Text = originalLine.Words.Last().Text?.TrimEnd();
                    allLines.Insert(originalLineIndex+1, newLine);
                }
            }
        }

        public void DeleteWord(ObservableCollection<LyricLine> allLines, LyricWord wordToDelete)
        {
            var originalLine = allLines.SingleOrDefault(l => l.Words != null && l.Words.Contains(wordToDelete));
            if (originalLine?.Words != null)
            {
                var indexOf = originalLine.Words.IndexOf(wordToDelete);
                if (indexOf > 0)
                {
                    originalLine.Words[indexOf - 1].Text = originalLine?.Words[indexOf - 1]?.Text?.TrimEnd();
                }
                originalLine?.Words.Remove(wordToDelete);

            }
        }
    }
}
