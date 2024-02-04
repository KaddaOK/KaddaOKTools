using KaddaOK.Library.Kbs;
using static KaddaOK.Library.Kbs.HeaderV2;
using static KaddaOK.Library.Kbs.KbpStyle;
using System.Text.RegularExpressions;
using static KaddaOK.Library.Kbs.KbpLine;

namespace KaddaOK.Library
{
    public interface IKbpSerializer
    {
        string Serialize(KbpFile kbpFile);
        KbpFile Deserialize(string kbpFileContents);
    }

    public class KbpSerializer : IKbpSerializer
    {
        public const string PageBreak = "-----------------------------";

        // TODO: useful validation errors for anything this code doesn't interpret correctly
        public KbpFile Deserialize(string kbpFileContents)
        {
            var pages = kbpFileContents.Split(PageBreak).Select(s => s.Trim()).ToList();

            var headerText = pages.SingleOrDefault(p => p.StartsWith("HEADERV2"));
            if (string.IsNullOrWhiteSpace(headerText))
            {
                throw new InvalidOperationException("The selected file has no KBP header.");
            }
            var pageTexts = pages.Where(p => p.StartsWith("PAGEV2")).ToList();
            if (!pageTexts.Any())
            {
                throw new InvalidOperationException("The selected file has no pages.");
            }

            return new KbpFile
            {
                Header = ParseHeader(headerText),
                Pages = ParsePages(pageTexts)
            };
        }

        public string Serialize(KbpFile kbpFile)
        {
            var pages = new List<string>();
            if (kbpFile.Header != null)
            {
                pages.Add(kbpFile.Header.ToString());
            };
            if (kbpFile.Pages != null)
            {
                pages.AddRange(kbpFile.Pages.Select(p => p.ToString()));
            }
            
            return string.Join(PageBreak + Environment.NewLine, pages);
        }

        private List<PageV2> ParsePages(List<string> pageTexts)
        {
            return pageTexts.Select(p => ParsePage(p)).ToList();
        }

        private static int GetAlphaIndex(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Trim().Length != 1)
            {
                throw new ArgumentException($"Input '{value}' was not 1 letter.");
            }
            var upper = value.Trim().ToUpperInvariant().First();
            if (upper < 'A' || upper > 'Z')
            {
                throw new ArgumentOutOfRangeException($"Letter '{value}' not between A and Z.");
            }

            return upper - 'A';
        }

        private PageV2 ParsePage(string pageText)
        {
            var page = new PageV2();

            var pageLines = pageText.Split(Environment.NewLine).Select(s => s.Trim()).Where(h => !string.IsNullOrWhiteSpace(h) && !h.StartsWith("'")).ToList();

            var lineIndex = 0;
            // sanity check
            if (pageLines[lineIndex] != "PAGEV2")
            {
                throw new InvalidOperationException("ParsePage did not receive PAGEV2 data");
            }
            lineIndex++;

            // pages will only have a 'FX/' line if their Display or Removal settings aren't Line-by-Line 
            if (pageLines[lineIndex].StartsWith("FX"))
            {
                var fxItems = pageLines[lineIndex].Split("/");
                page.PageRemoval = fxItems[1];
                page.PageDisplay = fxItems[2];
                lineIndex++;
            }

            KbpLine? currentKbpLine = null;
            while (lineIndex < pageLines.Count)
            {
                var lineItems = pageLines[lineIndex].Split("/");

                if (Regex.IsMatch(pageLines[lineIndex], "^[CLR]\\/[A-Za-z]\\/"))
                {
                    currentKbpLine = new KbpLine
                    {
                        Alignment = Enum.Parse<HorizontalAlignmentType>(lineItems[0]),
                        StyleIndex = (short)GetAlphaIndex(lineItems[1]),
                        IsFixedText = char.IsLower(lineItems[1]?.FirstOrDefault() ?? 'A'),
                        DisplayStartTicks = int.Parse(lineItems[2]),
                        DisplayEndTicks = int.Parse(lineItems[3]),
                        Across = short.Parse(lineItems[4]),
                        Down = short.Parse(lineItems[5]),
                        Rotation = short.Parse(lineItems[6])
                    };
                    if (currentKbpLine.DisplayEndTicks > 0) // don't even bother if it can't display
                    {
                        page.Lines.Add(currentKbpLine);
                    }
                }
                else
                {
                    var word = new KbpWord
                    {
                        Text = lineItems[0],
                        StartTicks = int.Parse(lineItems[1]),
                        EndTicks = int.Parse(lineItems[2]),
                        BeatDelay = short.Parse(lineItems[3])
                    };
                    if (word.EndTicks > 0)
                    {
                        if (currentKbpLine == null)
                        {
                            throw new InvalidOperationException("Found word without line");
                        }

                        currentKbpLine.Words.Add(word);
                    }
                }

                lineIndex++;
            }
            return page;
        }

        private HeaderV2 ParseHeader(string headerText)
        {
            var header = new HeaderV2();

            var headerLines = headerText.Split(Environment.NewLine).Select(s => s.Trim()).Where(h => !string.IsNullOrWhiteSpace(h) && !h.StartsWith("'")).ToList();

            // sanity check
            if (headerLines[0] != "HEADERV2")
            {
                throw new InvalidOperationException("ParseHeader did not receive HEADERV2 data");
            }

            // we expect the first line to define the color palette
            var paletteLine = headerLines[1];
            if (!Regex.IsMatch(paletteLine, "([0-9a-fA-F]{3},?){16}"))
            {
                throw new ArgumentException($"Expected first line of header to be palette; got '{paletteLine}'");
            }
            header.PaletteColors = paletteLine.Split(",").Select(s => KbpPaletteColor.From3DigitHexString(s)).ToList();

            var lineIndex = 2;
            while (headerLines[lineIndex].StartsWith("Style"))
            {
                var styleLine = headerLines[lineIndex];
                if (styleLine.StartsWith("StyleEnd"))
                {
                    lineIndex++;
                }
                else
                {
                    var style1Items = styleLine.Split(",");
                    var style2Items = headerLines[lineIndex + 1].Split(",");
                    var style3Items = headerLines[lineIndex + 2].Split(",");
                    var style = new KbpStyle
                    {
                        Number = byte.Parse(style1Items[0].Substring(5, 2)),
                        Name = style1Items[1],
                        TextColorPaletteIndex = byte.Parse(style1Items[2]),
                        OutlineColorPaletteIndex = byte.Parse(style1Items[3]),
                        TextWipeColorPaletteIndex = byte.Parse(style1Items[4]),
                        OutlineWipeColorPaletteIndex = byte.Parse(style1Items[5]),
                        FontName = style2Items[0],
                        FontSize = byte.Parse(style2Items[1]),
                        FontStyle = style2Items[2],
                        FontCharset = style2Items[3],
                        OutlineLeft = byte.Parse(style3Items[0]),
                        OutlineRight = byte.Parse(style3Items[1]),
                        OutlineTop = byte.Parse(style3Items[2]),
                        OutlineBottom = byte.Parse(style3Items[3]),
                        ShadowAcross = byte.Parse(style3Items[4]),
                        ShadowDown = byte.Parse(style3Items[5]),
                        Wiping = (WipingType)int.Parse(style3Items[6]),
                        Uppercase = Enum.Parse<KbpStyle.CaseType>(style3Items[7])
                    };

                    header.Styles.Add(style);
                    lineIndex += 3;
                }
            }
            var margins = headerLines[lineIndex].Split(",");
            header.MarginLeft = short.Parse(margins[0]);
            header.MarginRight = short.Parse(margins[1]);
            header.MarginTop = short.Parse(margins[2]);
            header.LineSpacing = short.Parse(margins[3]);
            lineIndex++;

            var borderAndDetail = headerLines[lineIndex].Split(",");
            header.BorderColorPaletteIndex = byte.Parse(borderAndDetail[0]);
            header.Detail = (DetailLevel)int.Parse(borderAndDetail[1]);
            lineIndex++;

            (header.Status, lineIndex) = GetHeaderMetadataIfValue(headerLines, lineIndex);
            (header.Title, lineIndex) = GetHeaderMetadataIfValue(headerLines, lineIndex);
            (header.Artist, lineIndex) = GetHeaderMetadataIfValue(headerLines, lineIndex);
            (header.Audio, lineIndex) = GetHeaderMetadataIfValue(headerLines, lineIndex);
            (header.BuildFile, lineIndex) = GetHeaderMetadataIfValue(headerLines, lineIndex);
            (header.Intro, lineIndex) = GetHeaderMetadataIfValue(headerLines, lineIndex);
            (header.Outro, lineIndex) = GetHeaderMetadataIfValue(headerLines, lineIndex);
            (header.Comments, lineIndex) = GetHeaderMetadataIfValue(headerLines, lineIndex);
            while (lineIndex < headerLines.Count)
            {
                header.Comments += Environment.NewLine + headerLines[lineIndex];
                lineIndex++;
            }
            return header;
        }

        private (string? value, int lineIndex) GetHeaderMetadataIfValue(List<string> headerLines, int lineIndex)
        {
            var line = headerLines[lineIndex];
            string? value = null;
            if (line.Length > 10)
            {
                value = line.Substring(10);
            }
            lineIndex++;
            return (value, lineIndex);
        }
    }
}
