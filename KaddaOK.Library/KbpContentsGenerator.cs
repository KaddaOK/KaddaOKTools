using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaddaOK.Library.Kbs;
using SkiaSharp;

namespace KaddaOK.Library
{
    public interface IKbpContentsGenerator
    {
        string GenerateKbpFileContents(string mediaFilePath, IEnumerable<LyricLine> processedResults,
            SKColor backgroundColor, SKColor unsungTextColor, SKColor unsungOutlineColor, SKColor sungTextColor, SKColor sungOutlineColor,
            int linesPerPage = 4, string? customHeader = null);

        //string ColorTo3DigitHex(SKColor skcolor);
    }
    public class KbpContentsGenerator : IKbpContentsGenerator
    {
        private readonly IKbpSerializer _serializer;
        public KbpContentsGenerator(IKbpSerializer serializer) 
        {
            _serializer = serializer;
        }

        // TODO: none of this makes use of the original import!
        public string GenerateKbpFileContents(string mediaFilePath, IEnumerable<LyricLine> processedResults,
            SKColor backgroundColor, SKColor unsungTextColor, SKColor unsungOutlineColor, SKColor sungTextColor, SKColor sungOutlineColor, 
            int linesPerPage = 4,
            string? customHeader = null)
        {
            var header = DefaultHeader();

            // TODO: this palette assignment is invalid for imports; they may have switched palette assignments
            header.PaletteColors[0] = KbpPaletteColorFromSKColor(backgroundColor);
            header.PaletteColors[1] = KbpPaletteColorFromSKColor(unsungTextColor);
            header.PaletteColors[2] = KbpPaletteColorFromSKColor(unsungOutlineColor);
            header.PaletteColors[3] = KbpPaletteColorFromSKColor(sungTextColor);
            header.PaletteColors[4] = KbpPaletteColorFromSKColor(sungOutlineColor);

            header.Audio = mediaFilePath;

            var pages = GetKbpPages(processedResults, linesPerPage);

            var file = new KbpFile
            {
                Header = header,
                Pages = pages
            };

            return _serializer.Serialize(file);

            /*var sb = new StringBuilder();
            sb.AppendLine(customHeader ?? KbpHeader(mediaFilePath, 
                ColorTo3DigitHex(backgroundColor),
                ColorTo3DigitHex(unsungTextColor),
                ColorTo3DigitHex(unsungOutlineColor),
                ColorTo3DigitHex(sungTextColor),
                ColorTo3DigitHex(sungOutlineColor)));
            var pages = GetKbpPageStrings(processedResults, linesPerPage);
            var pagesText = string.Join("", pages);
            sb.AppendLine(pagesText);
            return sb.ToString();*/
        }

        public KbpPaletteColor KbpPaletteColorFromSKColor(SKColor color)
        {
            return new KbpPaletteColor
            {
                Red = color.Red,
                Green = color.Green,
                Blue = color.Blue
            };
        }

        public HeaderV2 DefaultHeader()
        {
            return new HeaderV2
            {
                PaletteColors = new List<KbpPaletteColor>
                {
                    KbpPaletteColor.From3DigitHexString("055"),
                    KbpPaletteColor.From3DigitHexString("FFF"),
                    KbpPaletteColor.From3DigitHexString("000"),
                    KbpPaletteColor.From3DigitHexString("E70"),
                    KbpPaletteColor.From3DigitHexString("940"),
                    KbpPaletteColor.From3DigitHexString("CFF"),
                    KbpPaletteColor.From3DigitHexString("033"),
                    KbpPaletteColor.From3DigitHexString("0DD"),
                    KbpPaletteColor.From3DigitHexString("077"),
                    KbpPaletteColor.From3DigitHexString("FCF"),
                    KbpPaletteColor.From3DigitHexString("303"),
                    KbpPaletteColor.From3DigitHexString("F3F"),
                    KbpPaletteColor.From3DigitHexString("818"),
                    KbpPaletteColor.From3DigitHexString("000"),
                    KbpPaletteColor.From3DigitHexString("FFF"),
                    KbpPaletteColor.From3DigitHexString("000")
                },
                Styles = new List<KbpStyle>
                {
                    new()
                    {
                        Number = 0,
                        Name = "Default",
                        TextColorPaletteIndex = 1,
                        OutlineColorPaletteIndex = 2,
                        TextWipeColorPaletteIndex = 3,
                        OutlineWipeColorPaletteIndex = 4,
                        FontName = "Arial",
                        FontSize = 12,
                        FontStyle = "B",
                        FontCharset = "0",
                        OutlineLeft = 2,
                        OutlineRight = 2,
                        OutlineBottom = 2,
                        OutlineTop = 2,
                        ShadowAcross = 0,
                        ShadowDown = 0,
                        Wiping = KbpStyle.WipingType.WipeText,
                        Uppercase = KbpStyle.CaseType.L
                    },
                    new()
                    {
                        Number = 1,
                        Name = "Male",
                        TextColorPaletteIndex = 5,
                        OutlineColorPaletteIndex = 6,
                        TextWipeColorPaletteIndex = 7,
                        OutlineWipeColorPaletteIndex = 8,
                        FontName = "Arial",
                        FontSize = 12,
                        FontStyle = "B",
                        FontCharset = "0",
                        OutlineLeft = 2,
                        OutlineRight = 2,
                        OutlineBottom = 2,
                        OutlineTop = 2,
                        ShadowAcross = 0,
                        ShadowDown = 0,
                        Wiping = KbpStyle.WipingType.WipeText,
                        Uppercase = KbpStyle.CaseType.L
                    },
                    new()
                    {
                        Number = 2,
                        Name = "Female",
                        TextColorPaletteIndex = 9,
                        OutlineColorPaletteIndex = 10,
                        TextWipeColorPaletteIndex = 11,
                        OutlineWipeColorPaletteIndex = 12,
                        FontName = "Arial",
                        FontSize = 12,
                        FontStyle = "B",
                        FontCharset = "0",
                        OutlineLeft = 2,
                        OutlineRight = 2,
                        OutlineBottom = 2,
                        OutlineTop = 2,
                        ShadowAcross = 0,
                        ShadowDown = 0,
                        Wiping = KbpStyle.WipingType.WipeText,
                        Uppercase = KbpStyle.CaseType.L
                    },
                    new()
                    {
                        Number = 3,
                        Name = "Other",
                        TextColorPaletteIndex = 4,
                        OutlineColorPaletteIndex = 8,
                        TextWipeColorPaletteIndex = 12,
                        OutlineWipeColorPaletteIndex = 14,
                        FontName = "Arial",
                        FontSize = 12,
                        FontStyle = "B",
                        FontCharset = "0",
                        OutlineLeft = 2,
                        OutlineRight = 2,
                        OutlineBottom = 2,
                        OutlineTop = 2,
                        ShadowAcross = 0,
                        ShadowDown = 0,
                        Wiping = KbpStyle.WipingType.WipeText,
                        Uppercase = KbpStyle.CaseType.L
                    }
                },
                MarginLeft = 2,
                MarginRight = 2,
                MarginTop = 7,
                LineSpacing = 12,
                BorderColorPaletteIndex = 0,
                Detail = HeaderV2.DetailLevel.Good,
                Comments = $"Exported by Kadda OK Tools"
            };
        }

        public string KbpHeader(string audioPath, string bg3hex, string unsungText3hex, string unsungOutline3hex, string sungText3hex, string sungOutline3hex)
        {
            return
           $@"-----------------------------
KARAOKE BUILDER STUDIO
www.KaraokeBuilder.com

-----------------------------
HEADERV2

'--- Template Information ---

'Palette Colours (0-15)
  {bg3hex},{unsungText3hex},{unsungOutline3hex},{sungText3hex},{sungOutline3hex},CFF,033,0DD,077,FCF,303,F3F,818,000,FFF,000

'Styles (00-19)
'  Number,Name
'  Colour: Text,Outline,Text Wipe,Outline Wipe
'  Font  : Name,Size,Style,Charset
'  Other : Outline*4,Shadow*2,Wiping,Uppercase

  Style00,Default,1,2,3,4
    Arial,12,B,0
    2,2,2,2,0,0,0,L

  Style01,Male,5,6,7,8
    Arial,12,B,0
    2,2,2,2,0,0,0,L

  Style02,Female,9,10,11,12
    Arial,12,B,0
    2,2,2,2,0,0,0,L

  Style03,Other,4,8,12,14
    Arial,12,B,0
    2,2,2,2,0,0,0,L

  StyleEnd

'Margins : L,R,T,Line Spacing
  2,2,7,12

'Other: Border Colour,Detail Level
  0,2

'--- Track Information ---

Status    1
Title     
Artist    
Audio     {audioPath}
BuildFile 
Intro     
Outro     

Comments  Created with Karaoke Builder Studio
          www.KaraokeBuilder.com

-----------------------------";
        }

        private bool SanityCheckPageChunking(List<LyricLine[]> pages)
        {
            for (var i = 0; i < pages.Count; i++)
            {
                if (i > 0)
                {
                    var startSecond = pages[i].Min(m => m.StartSecond);
                    var previousEndSecond = pages[i - 1].Max(m => m.EndSecond);
                    if (previousEndSecond > startSecond)
                    {
                        return false;
                    }
                }

                if (i < pages.Count - 1)
                {
                    var endSecond = pages[i].Max(m => m.EndSecond);
                    var nextStartSecond = pages[i + 1].Min(m => m.StartSecond);
                    if (endSecond > nextStartSecond)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public PageV2 PageFromLyricLines(LyricLine[] lines)
        {
            var page = new PageV2
            {
                Lines = lines.Select(KbpLineFromLyricLine).ToList()
            };
            // TODO: align start ticks depending on page preferences?
            return page;
        }

        public KbpLine KbpLineFromLyricLine(LyricLine lyricLine)
        {
            var kbpLine = new KbpLine // TODO: this should be stored from an import and regurgitated
            {
                Alignment = lyricLine.KbpAlignmentType ?? KbpLine.HorizontalAlignmentType.C,
                // Across
                // DisplayEndTicks
                // DisplayStartTicks
                // Down
                IsFixedText = lyricLine.KbpIsFixedText,
                // Rotation
                StyleIndex = lyricLine.KbpStyleIndex ?? 0,
                Words = lyricLine.Words?.Select(s => new KbpWord
                {
                    Text = s.Text ?? "",
                    StartTicks = (int)(s.StartSecond * 100),
                    EndTicks = (int)(s.EndSecond * 100)
                }).ToList() ?? new List<KbpWord>()
            };

            // set some sane defaults, which we may have to mess with
            kbpLine.DisplayStartTicks = Math.Max(kbpLine.Words.Min(w => w.StartTicks) - 300, 0);
            kbpLine.DisplayEndTicks = Math.Max(kbpLine.Words.Max(w => w.EndTicks) + 50, 0);

            return kbpLine;
        }

        public List<PageV2> GetKbpPages(IEnumerable<LyricLine> processedResults, int linesPerPage = 4)
        {
            var pages = ArrangeIntoPages(processedResults, linesPerPage);
            var kbpPages = pages.Select(PageFromLyricLines).ToList();
            // TODO: align start ticks depending on page preferences?
            return kbpPages;
        }

        public List<string> GetKbpPageStrings(IEnumerable<LyricLine> processedResults, int linesPerPage = 4)
        {
            var pages = ArrangeIntoPages(processedResults, linesPerPage);

            var maxValueSec = pages.Max(p => p.Max(m => m.EndSecond));
            var kbpPages = new List<string>();
            for (int pageIndex = 0; pageIndex < pages.Count; pageIndex++)
            {
                var pageBuilder = new StringBuilder();
                pageBuilder.AppendLine("PAGEV2");
                var page = pages[pageIndex].ToList();
                var previousPage = pageIndex > 0 ? pages[pageIndex - 1].ToList() : null;
                var nextPage = pageIndex < pages.Count - 1 ? pages[pageIndex + 1].ToList() : null;
                var pageMinStartSecond = page.Min(m => m.StartSecond) - 3;
                var pageNaturalStartTicks = (int)((pageMinStartSecond) * 100);

                for (int lineIndex = 0; lineIndex < page.Count(); lineIndex++)
                {
                    var line = page[lineIndex];

                    var previousLineEndSec = previousPage != null && previousPage.Count >= lineIndex - 1 
                        ? (previousPage[lineIndex].EndSecond) + 0.5 
                        : 0;

                    var lineStartTicks = Math.Max((int)previousLineEndSec * 100, pageNaturalStartTicks);

                    var nextLineStartSec = nextPage != null && nextPage.Count > lineIndex
                    ? (nextPage[lineIndex].StartSecond) - 3
                    : maxValueSec;

                    var lineEndSec = Math.Min(nextLineStartSec, line.EndSecond) + 0.5;
                    var lineEndTicks = (int)(lineEndSec * 100);
                    var styleCode = KbpLine.GetStyleCode(line.KbpStyleIndex ?? 0, line.KbpIsFixedText);
                    var lineText = $"{line.KbpAlignmentType?.ToString() ?? "C"}/{styleCode}/{lineStartTicks}/{lineEndTicks}/0/0/0";

                    pageBuilder.AppendLine(lineText);
                    if (line.Words != null)
                    {
                        foreach (var word in line.Words)
                        {
                            var paddedWord = $"{word.Text}/".PadRight(15);
                            pageBuilder.AppendLine(
                                $"{paddedWord}{(int)(word.StartSecond * 100)}/{(int)(word.EndSecond * 100)}/0");
                        }
                    }

                    pageBuilder.AppendLine();
                }
                pageBuilder.AppendLine("-----------------------------");
                kbpPages.Add(pageBuilder.ToString());
            }

            return kbpPages;
        }

        private List<LyricLine[]> ArrangeIntoPages(IEnumerable<LyricLine> processedResults, int linesPerPage)
        {
            var pages = processedResults.Chunk(linesPerPage).ToList();
            if (processedResults.Any(p => p.PageIndex != null))
            {
                var indexedPages = processedResults.GroupBy(p => p.PageIndex).OrderBy(k => k.Key).Select(v => v.ToArray())
                    .ToList();
                if (SanityCheckPageChunking(indexedPages))
                {
                    pages = indexedPages;
                }
                else
                {
                    // uh oh, some pages are out of order; try to sort them...
                    indexedPages = indexedPages.OrderBy(o => o.Min(m => m.StartSecond)).ToList();
                    if (SanityCheckPageChunking(indexedPages))
                    {
                        pages = indexedPages;
                    }
                    // otherwise, better to just leave it at the ignorantly linesPerPage-chunked
                }
            }

            return pages;
        }

        private static string RoundToRepeatHex(byte input)
        {
            byte roundedInput = (byte)((Math.Round(input / 17M)) * 17);
            return roundedInput.ToString("X2").Substring(0, 1);
        }

        // TODO: move this, because it's now used just by the KBP classes themselves
        public static string ColorTo3DigitHex(SKColor skcolor)
        {
            return ColorTo3DigitHex(skcolor.Red, skcolor.Green, skcolor.Blue);
        }

        public static string ColorTo3DigitHex(byte red, byte green, byte blue)
        {
            return
                $"{RoundToRepeatHex(red)}{RoundToRepeatHex(green)}{RoundToRepeatHex(blue)}";
        }
    }
}
