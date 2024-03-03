using System.Collections.ObjectModel;

namespace KaddaOK.Library
{
    public interface IWordMerger
    {
        void MergeWord(ObservableCollection<LyricLine>? allLines, LyricWord wordToMerge, bool withWordBefore);
    }

    public class WordMerger : IWordMerger
    {
        public void MergeWord(ObservableCollection<LyricLine>? allLines, LyricWord wordToMerge, bool withWordBefore)
        {
            var originalLine = allLines?.SingleOrDefault(l => l.Words != null && l.Words.Contains(wordToMerge));
            if (originalLine != null)
            {
                var originalLineIndex = allLines!.IndexOf(originalLine);
                if (originalLine.Words != null)
                {
                    var originalWordIndex = originalLine.Words.IndexOf(wordToMerge);
                    if ((withWordBefore && originalWordIndex < 1)
                        || (!withWordBefore && originalWordIndex == originalLine.Words.Count - 1))
                    {
                        return; // I don't wanna.  TODO: I guess this would move the word to the line in question?
                    }

                    if (withWordBefore)
                    {
                        var prevWordIndex = originalWordIndex - 1;
                        var prevWord = originalLine.Words[prevWordIndex];
                        var newWord = new LyricWord
                        {
                            StartSecond = prevWord.StartSecond,
                            EndSecond = wordToMerge.EndSecond,
                            Text = $"{prevWord.Text.TrimEnd()}{wordToMerge.Text.TrimStart()}"
                        };
                        originalLine.Words = new ObservableCollection<LyricWord>(
                            originalLine.Words.Take(prevWordIndex).Concat(new []{newWord}).Concat(originalLine.Words.Skip(originalWordIndex + 1)));
                    }
                    else
                    {
                        var nextWordIndex = originalWordIndex + 1;
                        var nextWord = originalLine.Words[nextWordIndex];
                        var newWord = new LyricWord
                        {
                            StartSecond = wordToMerge.StartSecond,
                            EndSecond = nextWord.EndSecond,
                            Text = $"{wordToMerge.Text.TrimEnd()}{nextWord.Text.TrimStart()}"
                        };
                        originalLine.Words = new ObservableCollection<LyricWord>(
                            originalLine.Words.Take(originalWordIndex).Concat(new[] { newWord }).Concat(originalLine.Words.Skip(nextWordIndex + 1)));
                    }
                }
            }
        }
    }
}
