using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace KaddaOK.Library.Kbs
{
    public class KbpFile
    {
        public HeaderV2? Header { get; set; }
        public List<PageV2>? Pages { get; set; }
    }

    public class HeaderV2
    {
        // there are 16 of these in 3-digit hex
        public List<KbpPaletteColor> PaletteColors { get; set; } = new List<KbpPaletteColor>();

        // there can be up to 20 of these
        public List<KbpStyle> Styles { get; set; } = new List<KbpStyle>();

        public short MarginLeft { get; set; }
        public short MarginRight { get; set; }
        public short MarginTop { get; set; }
        public short LineSpacing { get; set; }

        public byte BorderColorPaletteIndex { get; set; }

        // default is 2 I think
        public DetailLevel Detail { get; set; }

        public string? Status { get; set; }
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public string? Audio { get; set; }
        public string? BuildFile { get; set; }
        public string? Intro { get; set; }
        public string? Outro { get; set; }

        public string? Comments { get; set; }

        public enum DetailLevel
        {
            Finest = 1,
            Good = 2,
            Medium = 3,
            Fast = 4,
            WordByWord = 12
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("HEADERV2");
            sb.AppendLine($"  {string.Join(",", PaletteColors.Select(s => s.ToString()))}");

            foreach (var style in Styles)
            {
                sb.AppendLine(style.ToString());
            }

            sb.AppendLine("  StyleEnd");
            sb.AppendLine($"  {MarginLeft},{MarginRight},{MarginTop},{LineSpacing}");
            sb.AppendLine($"  {BorderColorPaletteIndex},{(int)Detail}");

            sb.AppendLine("Status    1"); // I still don't know what this means
            sb.AppendLine($"Title     {Title}");
            sb.AppendLine($"Artist    {Artist}");
            sb.AppendLine($"Audio     {Audio}");
            sb.AppendLine($"BuildFile {BuildFile}");
            sb.AppendLine($"Intro     {Intro}");
            sb.AppendLine($"Outro     {Outro}");
            if (!string.IsNullOrWhiteSpace(Comments))
            {
                var commentLines = Comments.Split(Environment.NewLine);
                var nonFirstIndent = "          ";
                for (int i = 0; i < commentLines.Length; i++)
                {
                    if (i > 0 && !commentLines[i].StartsWith(nonFirstIndent))
                    {
                        commentLines[i] = nonFirstIndent + commentLines[i];
                    }
                }
                sb.AppendLine($"Comments  {string.Join(Environment.NewLine, commentLines)}");
            }

            return sb.ToString();
        }
    }

    public class KbpPaletteColor
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public override string ToString()
        {
            // TODO: move this into here?
            return KbpContentsGenerator.ColorTo3DigitHex(Red, Green, Blue);
        }

        public static KbpPaletteColor From3DigitHexString(string hexStringColor)
        {
            // eh, no need to reinvent the wheel, just let Skia do it
            var skcolor = SKColor.Parse(hexStringColor);
            return new KbpPaletteColor
            {
                Red = skcolor.Red,
                Green = skcolor.Green,
                Blue = skcolor.Blue
            };
        }
    }

    public class KbpStyle
    {
        public byte Number { get; set; }

        public string? Name { get; set; }

        public byte TextColorPaletteIndex { get; set; }
        public byte OutlineColorPaletteIndex { get; set; }
        public byte TextWipeColorPaletteIndex { get; set; }
        public byte OutlineWipeColorPaletteIndex { get; set; }

        public string? FontName { get; set; }
        public byte FontSize { get; set; }

        // "B", "I", "S" and "U" in combination
        public string? FontStyle { get; set; }

        public string? FontCharset { get; set; }


        public byte OutlineLeft { get; set; }
        public byte OutlineRight { get; set; }
        public byte OutlineTop { get; set; }
        public byte OutlineBottom { get; set; }

        public byte ShadowAcross { get; set; }
        public byte ShadowDown { get; set; }

        public WipingType Wiping { get; set; }

        // "L" for false and "U" for true
        public CaseType Uppercase { get; set; }

        public enum WipingType
        {
            WipeText = 0,
            WipeOutline = 1,
            Both = 2
        }

        public enum CaseType
        {
            L,
            U
        }

        public override string ToString()
        {
            var line1 =
                $"  Style{Number:00},{Name},{TextColorPaletteIndex},{OutlineColorPaletteIndex},{TextWipeColorPaletteIndex},{OutlineWipeColorPaletteIndex}";
            var line2 = $"    {FontName},{FontSize},{FontStyle},{FontCharset}";
            var line3 =
                $"    {OutlineLeft},{OutlineRight},{OutlineTop},{OutlineBottom},{ShadowAcross},{ShadowDown},{(int)Wiping},{Uppercase}";
            return string.Join(Environment.NewLine, line1, line2, line3);
        }
    }

    public class PageV2
    {
        // pages will only have a 'FX/' line if their Display or Removal settings aren't Line-by-Line
        public string? PageRemoval { get; set; }
        public string? PageDisplay { get; set; }


        public List<KbpLine> Lines { get; set; } = new List<KbpLine>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("PAGEV2");
            if (!string.IsNullOrWhiteSpace(PageRemoval) || !string.IsNullOrWhiteSpace(PageDisplay))
            {
                sb.AppendLine($"FX/{PageRemoval}/{PageDisplay}");
                sb.AppendLine();
            }

            foreach (var line in Lines)
            {
                sb.AppendLine(line.ToString());
            }
            
            return sb.ToString();
        }
    }

    public class KbpLine
    {
        public HorizontalAlignmentType Alignment { get; set; }

        public short StyleIndex { get; set; }
        public bool IsFixedText { get; set; }

        public int DisplayStartTicks { get; set; }
        public int DisplayEndTicks { get; set; }

        public short Across { get; set; }
        public short Down { get; set; }
        public short Rotation { get; set; }

        public List<KbpWord> Words { get; set; } = new List<KbpWord>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{Alignment}/{GetStyleCode(StyleIndex, IsFixedText)}/{DisplayStartTicks}/{DisplayEndTicks}/{Across}/{Down}/{Rotation}");
            foreach (var word in Words)
            {
                sb.AppendLine(word.ToString());
            }
            
            return sb.ToString();
        }

        public enum HorizontalAlignmentType
        {
            L,
            C,
            R
        }

        // TODO: either move this, or more likely, change the code that consumes it to use KbpLine
        public static string GetStyleCode(short value, bool isFixedText)
        {
            if (value < 0 || value > 25)
            {
                throw new ArgumentOutOfRangeException($"Input '{value}' does not correspond to an alphabetical letter.");
            }

            return ((char)((isFixedText ? 'a' : 'A') + value)).ToString();
        }
    }

    public class KbpWord
    {
        public string? Text { get; set; }
        public int StartTicks { get; set; }
        public int EndTicks { get; set; }

        /// <remarks>
        /// I don't really understand what this is or does, tbh.
        /// </remarks>
        public int BeatDelay { get; set; }

        public override string ToString()
        {
            return $"{(Text + "/"),-15}{StartTicks}/{EndTicks}/{BeatDelay}";
        }
    }
}
