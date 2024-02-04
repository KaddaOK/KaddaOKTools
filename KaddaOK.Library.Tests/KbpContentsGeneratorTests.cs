using System;
using System.Collections.Generic;
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
    }
}
