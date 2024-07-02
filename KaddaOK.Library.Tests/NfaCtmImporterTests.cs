using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaddaOK.Library.Tests
{
    public class NfaCtmImporterTests
    {
        private List<string> sampleCtmLines = new List<string>
        {
            "resampledInput 1 0.00 10.56 <b>",
            "resampledInput 1 10.56 0.08 ▁And",
            "resampledInput 1 10.64 0.48 <b>",
            "resampledInput 1 11.12 0.08 ▁after",
            "resampledInput 1 11.20 0.64 <b>",
            "resampledInput 1 11.84 0.08 ▁the",
            "resampledInput 1 11.92 0.40 <b>",
            "resampledInput 1 12.32 0.08 ▁sto",
            "resampledInput 1 12.40 0.24 <b>",
            "resampledInput 1 12.64 0.08 r",
            "resampledInput 1 12.72 0.08 m",
            "resampledInput 1 12.80 3.44 <b>",
            "resampledInput 1 16.24 0.08 ▁I",
            "resampledInput 1 16.32 0.08 <b>",
            "resampledInput 1 16.40 0.08 ▁run",
            "resampledInput 1 16.48 0.48 <b>",
            "resampledInput 1 16.96 0.16 ▁and",
            "resampledInput 1 17.12 0.16 <b>",
            "resampledInput 1 17.28 0.08 ▁run",
            "resampledInput 1 17.36 0.40 <b>",
            "resampledInput 1 17.76 0.08 ▁as",
            "resampledInput 1 17.84 0.32 <b>",
            "resampledInput 1 18.16 0.08 ▁the",
            "resampledInput 1 18.24 0.08 <b>",
            "resampledInput 1 18.32 0.08 ▁ra",
            "resampledInput 1 18.40 0.08 <b>",
            "resampledInput 1 18.48 0.08 in",
            "resampledInput 1 18.56 0.16 <b>",
            "resampledInput 1 18.72 0.08 s",
            "resampledInput 1 18.80 0.16 <b>",
            "resampledInput 1 18.96 0.08 ▁come",
            "resampledInput 1 19.04 0.40 <b>"
        };

        [Fact]
        public void ShouldParseCorrectly()
        {
            var importer = new NfaCtmImporter();
            var inputLyrics = new List<string>
            {
                "And after the storm",
                "I run and run as the rains come"
            };
            var resultingLines = importer.ImportNfaCtmAndLyrics(sampleCtmLines, inputLyrics);

            Assert.Equal(2, resultingLines.Count);

            var firstLine = resultingLines[0];
            Assert.Equal(6, firstLine.Words.Count);
            var firstLineFirstWord = firstLine.Words.First();
            Assert.Equal("And ", firstLineFirstWord.Text);
            Assert.Equal(10.56, firstLineFirstWord.StartSecond);
            Assert.Equal(10.64, firstLineFirstWord.EndSecond);
            var firstLineLastWord = firstLine.Words.Last();
            Assert.Equal("m", firstLineLastWord.Text);
            Assert.Equal(12.72, firstLineLastWord.StartSecond);
            Assert.Equal(12.80, firstLineLastWord.EndSecond);

            var secondLine = resultingLines[1];
            Assert.Equal(10, secondLine.Words.Count);
            var secondLineFirstWord = secondLine.Words.First();
            Assert.Equal("I ", secondLineFirstWord.Text);
            Assert.Equal(16.24, secondLineFirstWord.StartSecond);
            Assert.Equal(16.32, secondLineFirstWord.EndSecond);
            var secondLineLastWord = secondLine.Words.Last();
            Assert.Equal("come", secondLineLastWord.Text);
            Assert.Equal(18.8, secondLineLastWord.StartSecond);
            Assert.Equal(19.04, secondLineLastWord.EndSecond);
        }

        [Fact]
        public void ShouldOutputRemainingIfNoMoreLyrics()
        {
            var importer = new NfaCtmImporter();
            var inputLyrics = new List<string>
            {
                "And after the storm",
                "I run"
            };
            var resultingLines = importer.ImportNfaCtmAndLyrics(sampleCtmLines, inputLyrics);

            Assert.Equal(2, resultingLines.Count);

            var firstLine = resultingLines[0];
            Assert.Equal(6, firstLine.Words.Count);
            var firstLineFirstWord = firstLine.Words.First();
            Assert.Equal("And ", firstLineFirstWord.Text);
            Assert.Equal(10.56, firstLineFirstWord.StartSecond);
            Assert.Equal(10.64, firstLineFirstWord.EndSecond);
            var firstLineLastWord = firstLine.Words.Last();
            Assert.Equal("m", firstLineLastWord.Text);
            Assert.Equal(12.72, firstLineLastWord.StartSecond);
            Assert.Equal(12.80, firstLineLastWord.EndSecond);

            var secondLine = resultingLines[1];
            Assert.Equal(10, secondLine.Words.Count);
            var secondLineFirstWord = secondLine.Words.First();
            Assert.Equal("I ", secondLineFirstWord.Text);
            Assert.Equal(16.24, secondLineFirstWord.StartSecond);
            Assert.Equal(16.32, secondLineFirstWord.EndSecond);
            var secondLineLastWord = secondLine.Words.Last();
            Assert.Equal("come", secondLineLastWord.Text);
            Assert.Equal(18.8, secondLineLastWord.StartSecond);
            Assert.Equal(19.04, secondLineLastWord.EndSecond);
        }

        [Fact]
        public void ShouldOutputRemainingIfTextDifference()
        {
            var importer = new NfaCtmImporter();
            var inputLyrics = new List<string>
            {
                "And after the storm,",
                "I run, and run, as the rains come"
            };
            var resultingLines = importer.ImportNfaCtmAndLyrics(sampleCtmLines, inputLyrics);

            Assert.Single(resultingLines);

            var firstLine = resultingLines[0];
            Assert.Equal(16, firstLine.Words.Count);
            var firstLineFirstWord = firstLine.Words.First();
            Assert.Equal("And ", firstLineFirstWord.Text);
            Assert.Equal(10.56, firstLineFirstWord.StartSecond);
            Assert.Equal(10.64, firstLineFirstWord.EndSecond);
            var firstLineLastWord = firstLine.Words.Last();
            Assert.Equal("come", firstLineLastWord.Text);
            Assert.Equal(18.8, firstLineLastWord.StartSecond);
            Assert.Equal(19.04, firstLineLastWord.EndSecond);
        }

        [Fact]
        public void ShouldOutputAllOnOneLineIfNoInput()
        {
            var importer = new NfaCtmImporter();

            var resultingLines = importer.ImportNfaCtmAndLyrics(sampleCtmLines, null);

            Assert.Single(resultingLines);

            var firstLine = resultingLines[0];
            Assert.Equal(16, firstLine.Words.Count);
            var firstLineFirstWord = firstLine.Words.First();
            Assert.Equal("And ", firstLineFirstWord.Text);
            Assert.Equal(10.56, firstLineFirstWord.StartSecond);
            Assert.Equal(10.64, firstLineFirstWord.EndSecond);
            var firstLineLastWord = firstLine.Words.Last();
            Assert.Equal("come", firstLineLastWord.Text);
            Assert.Equal(18.8, firstLineLastWord.StartSecond);
            Assert.Equal(19.04, firstLineLastWord.EndSecond);
        }
    }
}
