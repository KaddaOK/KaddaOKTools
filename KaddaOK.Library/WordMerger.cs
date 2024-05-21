using System.Collections.ObjectModel;

namespace KaddaOK.Library
{
    public interface IWordMerger
    {
        (LyricLine? resultingLine, LyricWord? resultingWord) MergeWord(ObservableCollection<LyricLine>? allLines, LyricWord wordToMerge, bool withWordBefore);
    }

    public class WordMerger : IWordMerger
    {
        public (LyricLine? resultingLine, LyricWord? resultingWord) MergeWord(ObservableCollection<LyricLine>? allLines, LyricWord wordToMerge, bool withWordBefore)
        {
            var originalLine = allLines?.SingleOrDefault(l => l.Words != null && l.Words.Contains(wordToMerge));
            if (originalLine == null)
            {
                return (null, null);
            }

            var originalLineIndex = allLines!.IndexOf(originalLine);

            if (originalLine.Words == null)
            {
                return (null, null);
            }

            var originalWordIndex = originalLine.Words.IndexOf(wordToMerge);
            if ((withWordBefore && originalWordIndex < 1)
                || (!withWordBefore && originalWordIndex == originalLine.Words.Count - 1))
            {
                return (null, null); // I don't wanna.  TODO: I guess this would move the word to the previous line...
            }

            // either we're merging with the word with the previous index or the next index
            var firstWordIndex = withWordBefore ? originalWordIndex - 1 : originalWordIndex;
            var secondWordIndex = withWordBefore ? originalWordIndex : originalWordIndex + 1;
            var firstWord = originalLine.Words[firstWordIndex];
            var secondWord = originalLine.Words[secondWordIndex];

            var replacementWord = new LyricWord
            {
                StartSecond = firstWord.StartSecond,
                EndSecond = secondWord.EndSecond,
                Text = $"{firstWord.Text.TrimEnd()}{secondWord.Text.TrimStart()}"
            };

            originalLine.Words = new ObservableCollection<LyricWord>(
                originalLine.Words.Take(firstWordIndex).Concat(new[] { replacementWord })
                    .Concat(originalLine.Words.Skip(secondWordIndex + 1)));

            return (originalLine, replacementWord);
        }
    }
}
