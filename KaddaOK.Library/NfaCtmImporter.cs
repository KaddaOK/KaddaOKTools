using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaddaOK.Library
{
    public interface INfaCtmImporter
    {
        List<LyricLine> ImportNfaCtmAndLyrics(List<string> ctmLines, List<string>? originalLyrics);
    }
    public class NfaCtmImporter : INfaCtmImporter
    {
        public List<LyricLine> ImportNfaCtmAndLyrics(List<string> ctmLines, List<string>? originalLyrics)
        {
            if (originalLyrics == null)
            {
                throw new InvalidOperationException(
                    "System is not designed to parse NFA output without access to the lyric lines that fed it, sorry");
            }
            var lines = new List<LyricLine>();

            // constants that could change with CTM formatting versions:
            var expectedPartsCount = 5;
            var onsetTimeIndex = 2;
            var tokenLengthIndex = 3;
            var tokenContentIndex = 4;
            var breakContent = "<b>";

            var currentExpectedLineIndex = 0;
            var currentLine = new LyricLine();
            for (var i = 0; i < ctmLines.Count(); i++)
            {
                if (currentExpectedLineIndex > originalLyrics.Count() - 1)
                {
                    // we must be at the end
                    break;
                }

                var parts = ctmLines[i].Split(" ");
                if (parts.Count() == expectedPartsCount)
                {
                    var start = Decimal.Parse(parts[onsetTimeIndex]);
                    var length = Decimal.Parse(parts[tokenLengthIndex]);
                    var rawContent = parts[tokenContentIndex];

                    if (rawContent.Trim() != breakContent) // this doesn't provide us useful data; we'll reach back if we want it
                    {
                        var cleanedContent = rawContent.Replace("▁", " ");
                        var startTime = start;
                        if (currentLine.Words.Any())
                        {
                            var previous = ctmLines[i - 1];
                            var previousParts = previous.Split(" ");
                            if (previousParts.Count() == expectedPartsCount && previousParts[tokenContentIndex] == breakContent)
                            {
                                // pull the start from the previous break
                                startTime = Decimal.Parse(previousParts[onsetTimeIndex]);
                            }
                        }
                        currentLine.Words.Add(new LyricWord { StartSecond = (double)startTime, EndSecond = (double)(start + length), Text = cleanedContent });

                        if (currentLine.Text?.Trim().ToLowerInvariant() == originalLyrics[currentExpectedLineIndex].Trim().ToLowerInvariant())
                        {
                            currentExpectedLineIndex++;
                            lines.Add(currentLine);
                            currentLine = new LyricLine();
                        }
                    }
                }
            }

            // this results in spaces in front of the words, and I prefer them to be behind them so let's change that
            foreach (var line in lines)
            {
                for (var i = 0; i < line.Words.Count; i++)
                {
                    var word = line.Words[i];
                    if (word.Text!.StartsWith(" "))
                    {
                        word.Text = word.Text.TrimStart();
                        if (i > 0)
                        {
                            var prevWord = line.Words[i - 1];
                            if (!prevWord.Text!.EndsWith(" "))
                            {
                                prevWord.Text += " ";
                            }
                        }
                    }
                }
            }
            return lines;
        }
    }
}
