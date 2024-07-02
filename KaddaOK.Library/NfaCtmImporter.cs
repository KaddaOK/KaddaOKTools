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
            var lines = new List<LyricLine>();
            originalLyrics ??= new List<string>();

            // constants that could change with CTM formatting versions:
            var expectedPartsCount = 5;
            var onsetTimeIndex = 2;
            var tokenLengthIndex = 3;
            var tokenContentIndex = 4;
            var breakContent = "<b>";

            var currentExpectedLineIndex = 0;
            var currentLine = new LyricLine();

            var noMoreOriginalLyrics = !originalLyrics.Any();

            for (var i = 0; i < ctmLines.Count(); i++)
            {
                // we expect each row of a CTM to be expectedPartsCount space-separated values
                var parts = ctmLines[i].Split(" ");
                if (parts.Count() == expectedPartsCount)
                {
                    // parse start, length, and rawContent out of the current CTM row
                    var start = Decimal.Parse(parts[onsetTimeIndex]);
                    var length = Decimal.Parse(parts[tokenLengthIndex]);
                    var rawContent = parts[tokenContentIndex];

                    // It seems like almost every row in the CTM has its own `<b>` row following it, which if we
                    // actually honored as breaks, would be quite choppy. From experimenting with it a bit when I
                    // first wrote this importer, it seemed like I got results that were closer to what I'd expect
                    // if we let the *following* syllable absorb that extra time instead of adding it to the
                    // *preceding* one (but this should be played around with and verified a lot more, and honestly
                    // ideally we should accept a parameter to choose from a few diffrent modes of `<b>` handling
                    // so people can experiment themselves, because I could be very wrong and it might give much
                    // better results handled some other way... )
                    // So we skip those as primary objects of the loop, and instead reach back for
                    // them by `index--` when we're processing a row that does have actual text.
                    // TODO: re-examine and/or make configurable the processing of `<b>` rows
                    if (rawContent.Trim() != breakContent)
                    {
                        // Underscores is what CTM (at least, NeMo Forced Aligner in English) does with spaces
                        // (which we'll flip to the other side of words after we're finished parsing)
                        var cleanedContent = rawContent.Replace("▁", " ");

                        var startTime = start;
                        // If this isn't the first syllable in the line, we should check to see if the previous 
                        // row was a `<b>` and if so steal its startTime instead of using what this row claims.
                        // (we don't do it when this is the first syllable of a line, because that would pull 
                        // in the gap between lines.  And that's another problem with not having the original
                        // lines of lyrics to go off of. Maybe we should re-examine this algorithm's assumptions
                        // about the accuracy of `<b>`'s, as this may turn out real bad in some cases...)
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

                        // anyway, let's add the current syllable to the line we're constructing
                        currentLine.Words.Add(new LyricWord
                        {
                            StartSecond = (double)startTime, 
                            EndSecond = (double)(start + length), 
                            Text = cleanedContent
                        });

                        // If we've now constructed the full line (and there are any more of the line-break lyrics left) then we
                        // save the current line and start a new one.
                        // If we're out of line-broken lyrics, or never received any to begin with, then everything remaining 
                        // in the CTM will continue to get added to the same line, which we'll add after the whole loop.
                        if (!noMoreOriginalLyrics 
                            && currentLine.Text?.Trim().ToLowerInvariant() == originalLyrics[currentExpectedLineIndex].Trim().ToLowerInvariant())
                        {
                            currentExpectedLineIndex++;
                            lines.Add(currentLine);
                            if (currentExpectedLineIndex <= originalLyrics.Count() - 1)
                            {
                                currentLine = new LyricLine();
                            }
                            else
                            {
                                noMoreOriginalLyrics = true;
                            }
                        }
                    }
                }
            }

            // If we still have a line we haven't added to the parsed list yet (either because we ran out of lyrics, or 
            // the lyrics stopped matching up at some point) here's where we make sure we include it
            if (currentLine.Words.Any() && !lines.Contains(currentLine))
            {
                lines.Add(currentLine);
            }

            // As mentioned above, this process results in spaces in front of the words, and everything else expects them 
            // to be on the end of words, so let's go through and flip that around.
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
