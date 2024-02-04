using System.Collections.ObjectModel;

namespace KaddaOK.Library.Tests
{
    public class LineSplitterTests
    {
        public class TestHarness
        {
            public LineSplitter Splitter { get; }

            public TestHarness()
            {
                Splitter = new LineSplitter();
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
                    FromWords(30, "never", "gonna", "make", "you", "cry"),
                    FromWords(35, "never", "gonna", "say", "goodbye"),
                    FromWords(40, "never", "gonna", "tell", "a", "lie", "and", "hurt", "you"),
                });
        }

        public class SplitLineAt
        {
            [Fact]
            public void ShouldSplitBefore()
            {
                var harness = new TestHarness();

                var desert = harness.AllLines.SelectMany(a => a.Words!).SingleOrDefault(w => w.Text == "desert ");

                harness.Splitter.SplitLineAt(harness.AllLines, desert!, true);

                Assert.Equal(7, harness.AllLines.Count);
                var existingLine = harness.AllLines[2];
                Assert.Equal("never gonna run around and", existingLine.Text);
                Assert.Equal(25, existingLine.EndSecond);
                var newLine = harness.AllLines[3];
                Assert.Equal("desert you", newLine.Text);
                Assert.Equal(25, newLine.StartSecond);
                Assert.Equal(27, newLine.EndSecond);
            }

            [Fact]
            public void ShouldSplitAfter()
            {
                var harness = new TestHarness();

                var lie = harness.AllLines.SelectMany(a => a.Words!).SingleOrDefault(w => w.Text == "lie ");

                harness.Splitter.SplitLineAt(harness.AllLines, lie!, false);

                Assert.Equal(7, harness.AllLines.Count);
                var existingLine = harness.AllLines[5];
                Assert.Equal("never gonna tell a lie", existingLine.Text);
                Assert.Equal(45, existingLine.EndSecond);
                var newLine = harness.AllLines[6];
                Assert.Equal("and hurt you", newLine.Text);
                Assert.Equal(45, newLine.StartSecond);
                Assert.Equal(48, newLine.EndSecond);
            }
        }

        public class DeleteWord
        {
            [Fact]
            public void ShouldDeleteWord()
            {
                var harness = new TestHarness();

                var cry = harness.AllLines.SelectMany(a => a.Words!).SingleOrDefault(w => w.Text == "cry");

                harness.Splitter.DeleteWord(harness.AllLines, cry!);

                Assert.Equal(6, harness.AllLines.Count);
                var existingLine = harness.AllLines[3];
                Assert.Equal("never gonna make you", existingLine.Text);
                Assert.Equal(34, existingLine.EndSecond);
                Assert.Equal(4, existingLine.Words!.Count);
            }
        }
    }
}