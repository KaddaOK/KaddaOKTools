using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace KaddaOK.Library.Tests
{
    public class KbpContentsGeneratorTests
    {
        public class ColorTo3DigitHex
        {
            [Theory]
            [InlineData("000", 0)]
            [InlineData("111", 9)]
            [InlineData("111", 16)]
            [InlineData("111", 23)]
            [InlineData("222", 26)]
            [InlineData("222", 34)]
            [InlineData("444", 64)]
            [InlineData("555", 77)]
            [InlineData("ABC", 178, 179, 196)]
            [InlineData("EEE", 238)]
            [InlineData("FFF", 247)]
            [InlineData("FFF", 255)]
            public void ShouldRoundCorrectly(string expectedResult, byte originalRed, int? originalGreen = null, int? originalBlue = null)
            {
                var result = KbpContentsGenerator.ColorTo3DigitHex(new SKColor(originalRed, (byte?)originalGreen ?? originalRed, (byte?)originalBlue ?? (byte?)originalGreen ?? originalRed));
                Assert.Equal(expectedResult, result);
            }
        }

        public class PageFromLyricLines
        {
            [Fact]
            public void ShouldNotCrashWithEmptyLines()
            {
                var ser = new KbpSerializer();
                var gen = new KbpContentsGenerator(ser);
                var result = gen.PageFromLyricLines(new List<LyricLine>
                {
                    new LyricLine
                    {
                        Words = new ObservableCollection<LyricWord>()
                    },
                    new LyricLine
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new LyricWord
                            {
                                Text = "Test",
                                StartSecond = 5, 
                                EndSecond = 10
                            }
                        },
                    },
                    new LyricLine
                    {
                        Words = new ObservableCollection<LyricWord>()
                    },
                }.ToArray());
                Assert.NotNull(result);
                // it should actually still have 3 lines
                Assert.Equal(3, result.Lines.Count);
                var firstLine = result.Lines[0];
                Assert.Empty(firstLine.Words);
                var secondLine = result.Lines[1];
                var singleWord = Assert.Single(secondLine.Words);
                Assert.Equal("Test", singleWord.Text);
                Assert.Equal(500, singleWord.StartTicks);
                Assert.Equal(1000, singleWord.EndTicks);
            }
        }

        public class ArrangeIntoPages
        {
            [Fact]
            public void ShouldUseLinesPerPageIfNoPageBreaks()
            {
                var ser = new KbpSerializer();
                var gen = new KbpContentsGenerator(ser);
                var lines = Enumerable.Range(0, 10).Select(i => new LyricLine { Words = new ObservableCollection<LyricWord>
                {
                        new LyricWord { Text = $"Line {i}", StartSecond = i, EndSecond = i + 1 }
                }}).ToArray();
                var result = gen.ArrangeIntoPages(lines, 3);
                Assert.Equal(4, result.Count); // 3 pages of 3 lines, and 1 page of 1 line
                Assert.Equal("Line 0", result[0][0].Text);
                Assert.Equal("Line 1", result[0][1].Text);
                Assert.Equal("Line 2", result[0][2].Text);
                Assert.Equal("Line 3", result[1][0].Text);
                Assert.Equal("Line 4", result[1][1].Text);
                Assert.Equal("Line 5", result[1][2].Text);
                Assert.Equal("Line 6", result[2][0].Text);
                Assert.Equal("Line 7", result[2][1].Text);
                Assert.Equal("Line 8", result[2][2].Text);
                Assert.Equal("Line 9", result[3][0].Text);
            }

            [Fact]
            public void ShouldUsePageBreaksIfAnyHavePageIndexes()
            {
                var ser = new KbpSerializer();
                var gen = new KbpContentsGenerator(ser);
                var lines = Enumerable.Range(0, 10).Select(i => new LyricLine { Words = new ObservableCollection<LyricWord>
                {
                        new LyricWord { Text = $"Line {i}", StartSecond = i, EndSecond = i + 1 }
                }, PageIndex = i >= 3 ? (int?)1 : 0 }).ToArray();
                var result = gen.ArrangeIntoPages(lines, 3);
                Assert.Equal(2, result.Count); // based on 3 lines per page, it should be 4 pages, but with a page index on line 3, it should be one page of 3 lines then 1 page of 7 lines
                Assert.Equal("Line 0", result[0][0].Text);
                Assert.Equal("Line 1", result[0][1].Text);
                Assert.Equal("Line 2", result[0][2].Text);
                Assert.Equal("Line 3", result[1][0].Text);
                Assert.Equal("Line 4", result[1][1].Text);
                Assert.Equal("Line 5", result[1][2].Text);
                Assert.Equal("Line 6", result[1][3].Text);
                Assert.Equal("Line 7", result[1][4].Text);
                Assert.Equal("Line 8", result[1][5].Text);
                Assert.Equal("Line 9", result[1][6].Text); 
            }
        }
    }
}
