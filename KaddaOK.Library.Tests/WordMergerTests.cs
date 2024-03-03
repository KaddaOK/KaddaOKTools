using System.Collections.ObjectModel;

namespace KaddaOK.Library.Tests
{
    public class WordMergerTests
    {
        public class TestHarness
        {
            public WordMerger Merger { get; }

            public TestHarness()
            {
                Merger = new WordMerger();
            }

            public static LyricLine FromWords(int startSecond, params string[] words)
            {
                var line = new LyricLine
                {
                    Words = new ObservableCollection<LyricWord>()
                };
                int currentSecond = startSecond;
                foreach (var word in words)
                {
                    line.Words.Add(new LyricWord
                    {
                        StartSecond = currentSecond,
                        EndSecond = currentSecond + 1,
                        Text = word + (Array.IndexOf(words, word) == words.Length - 1 ? "" : " ") 
                    });
                    currentSecond++;
                }
                return line;
            }

            public ObservableCollection<LyricLine> AllLines = new(
                new List<LyricLine>
                {
                    FromWords(5, "never", "gonna", "give", "you", "up"),
                    FromWords(15, "never", "gonna", "let", "you", "down"),
                    FromWords(20, "never", "gonna", "run", "around", "and", "desert", "you"),
                    FromWords(30, "neffer", "gonna", "make", "you", "cry"),
                    FromWords(35, "never", "goingta", "say", "goodbye"),
                    FromWords(40, "never", "gonna", "tell", "a", "lie", "and", "hurt", "you"),
                });
        }

        public class MergeWord
        {
            [Theory]
            [InlineData(true, "run ", 2, "never gonnarun around and desert you", 6, 1, "gonnarun ", 21, 23)]
            [InlineData(false, "around ", 2, "never gonna run aroundand desert you", 6, 3, "aroundand ", 23, 25)]
            [InlineData(true, "around ", 2, "never gonna runaround and desert you", 6, 2, "runaround ", 22, 24)]
            [InlineData(false, "desert ", 2, "never gonna run around and desertyou", 6, 5, "desertyou", 25, 27)]
            [InlineData(true, "neffer ", 3, "neffer gonna make you cry", 5, 0, "neffer ", 30, 31)]
            [InlineData(true, "goingta ", 4, "nevergoingta say goodbye", 3, 0, "nevergoingta ", 35, 37)]
            [InlineData(false, "cry ", 3, "neffer gonna make you cry", 5, 4, "cry", 34, 35)]
            public void ShouldMergeCorrectly(bool before, string matchWordText, int checkLineIndex, string expectedLineText, int expectedWordCount, int checkWordIndex, string expectedWordText, double expectedStart, double expectedEnd)
            {
                var harness = new TestHarness();

                var around = harness.AllLines.SelectMany(a => a.Words!).SingleOrDefault(w => w.Text == matchWordText);

                harness.Merger.MergeWord(harness.AllLines, around!, before);

                Assert.Equal(6, harness.AllLines.Count);
                var existingLine = harness.AllLines[checkLineIndex];
                Assert.Equal(expectedLineText, existingLine.Text);
                Assert.Equal(expectedWordCount, existingLine.Words.Count);
                var newWord = existingLine.Words[checkWordIndex];
                Assert.Equal(expectedWordText, newWord.Text);
                Assert.Equal(expectedStart, newWord.StartSecond);
                Assert.Equal(expectedEnd, newWord.EndSecond);
            }
        }
    }
}