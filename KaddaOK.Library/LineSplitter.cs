using System.Collections.ObjectModel;

namespace KaddaOK.Library
{
    public interface ILineSplitter
    {
        void SplitLineAt<TList, TItem>(ObservableCollection<TList>? allLines, TItem wordToSplit, bool splitBefore)
            where TItem : LyricWord
            where TList : ILyricLine<TItem>, new();
        void DeleteWord<TList, TItem>(ObservableCollection<TList> allLines, TItem wordToDelete)
            where TItem : LyricWord
            where TList : ILyricLine<TItem>, new();
    }

    public class LineSplitter : ILineSplitter
    {
        public void SplitLineAt<TList, TItem>(ObservableCollection<TList>? allLines, TItem wordToSplit, bool splitBefore)
            where TItem : LyricWord
            where TList : ILyricLine<TItem>, new()
        {
            var originalLine = allLines.SingleOrDefault(l => l.Words != null && l.Words.Contains(wordToSplit));
            if (originalLine != null)
            {
                var originalLineIndex = allLines!.IndexOf(originalLine);
                if (originalLine.Words != null)
                {
                    var originalWordIndex = originalLine.Words.IndexOf(wordToSplit);
                    if (originalWordIndex == 0 && splitBefore)
                    {
                        // can't split before this because it's already the first word; do nothing
                        return;
                    }

                    var newLine = new TList
                    {
                        Words = new ObservableCollection<TItem>(originalLine.Words.Skip(originalWordIndex + (splitBefore ? 0 : 1)))
                    };
                    if (newLine is LyricLine newLyricLine && originalLine is LyricLine oldLyricLine)
                    {
                        newLyricLine.IsSelected = oldLyricLine.IsSelected;
                    }

                    originalLine.Words = new ObservableCollection<TItem>(originalLine.Words.Take(splitBefore ? originalWordIndex : originalWordIndex + 1));
                    originalLine.Words.Last().Text = originalLine.Words.Last().Text?.TrimEnd();
                    allLines.Insert(originalLineIndex+1, newLine);
                }
            }
        }

        public void DeleteWord<TList, TItem>(ObservableCollection<TList> allLines, TItem wordToDelete)
            where TItem : LyricWord
            where TList : ILyricLine<TItem>, new()
        {
            var originalLine = allLines.SingleOrDefault(l => l.Words != null && l.Words.Contains(wordToDelete));
            if (originalLine?.Words != null)
            {
                var indexOf = originalLine.Words.IndexOf(wordToDelete);
                if (indexOf > 0)
                {
                    // TODO: wait what why? Investigate what the rationale for this trimming was, because it looks wrong as I go by it here rn
                    originalLine.Words[indexOf - 1].Text = originalLine?.Words[indexOf - 1]?.Text?.TrimEnd();
                }
                originalLine?.Words.Remove(wordToDelete);

            }
        }
    }
}
