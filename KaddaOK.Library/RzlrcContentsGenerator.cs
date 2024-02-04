using System.Globalization;
using KaddaOK.Library.Ytmm;
using System.Xml.Serialization;
using SkiaSharp;
using System.Text;

namespace KaddaOK.Library
{
    public interface IRzlrcContentsGenerator
    {
        string GenerateRzlrcFileContents(IEnumerable<LyricLine> processedResults, string mediaFilePath, string? lyricsFilePath,
            SKColor backgroundColor, SKColor unsungTextColor, SKColor unsungOutlineColor, SKColor sungTextColor, SKColor sungOutlineColor);

        string InterpolateFileContents(List<RzlrcLyric>? originalFileContents,
            RzlrcLyric? selectedPage,
            IList<LyricLine>? processedResults);
    }
    public class RzlrcContentsGenerator : IRzlrcContentsGenerator
    {
        public static uint? GetBGRUintColorFromSKColor(SKColor? skcolor) =>
            skcolor == null
                ? null
                : uint.Parse($"{skcolor.Value.Blue:X2}{skcolor.Value.Green:X2}{skcolor.Value.Red:X2}",
                    NumberStyles.HexNumber);

        public string GenerateRzlrcFileContents(IEnumerable<LyricLine> processedResults, string mediaFilePath, string? lyricsFilePath, 
            SKColor backgroundColor, SKColor unsungTextColor, SKColor unsungOutlineColor, SKColor sungTextColor, SKColor sungOutlineColor)
        {
            var ytmmLyric = FromDetectedLyrics(processedResults, mediaFilePath, lyricsFilePath, 
                backgroundColor, unsungTextColor, unsungOutlineColor, sungTextColor, sungOutlineColor);

            return SerializeToXmlString(new List<RzlrcLyric>{ytmmLyric});
        }

        private static string SerializeToXmlString(List<RzlrcLyric> ytmmLyrics)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var ser = new XmlSerializer(typeof(List<RzlrcLyric>));

            var originalStringWriter = new StringWriter();
            ser.Serialize(originalStringWriter, ytmmLyrics, ns);
            var result = originalStringWriter.ToString();
            return result.Replace(" ThreeDMixColor", " 3DMixColor").Replace("<ArrayOfLyric>", "").Replace("</ArrayOfLyric>","");
        }

        public static RzlrcLyric FromDetectedLyrics(IEnumerable<LyricLine> lyrics, string? mediaFilePath, string? lyricsFilePath,
            SKColor? backgroundColor, SKColor? unsungTextColor, SKColor? unsungOutlineColor, SKColor? sungTextColor, SKColor? sungOutlineColor)
        {
            // many of these constants are random trash and should be reviewed. They work good enough so far...
            var fromScratch = new RzlrcLyric
            {
                xArea = 20,
                yArea = 20,
                wArea = 408,
                hArea = 220,
                wRefArea = 856,
                hRefArea = 480,
                bLoopActionAppear = 1,
                BKColor = GetBGRUintColorFromSKColor(backgroundColor) ?? 0,
                clrOverlayFont = GetBGRUintColorFromSKColor(sungTextColor) ?? 16777215,
                clrOverlayEdge = GetBGRUintColorFromSKColor(sungOutlineColor) ?? 0,
                clrOverlayShade = 4259584,
                clrOverlayShine = 5082312,
                eAlignType = 8,
                nMarginLeft = 20,
                nMarginRight = 20,
                nMarginBottom = 20,
                bUseMarginLeft = 1,
                nLineSpace = 5,
                nAppearIndex = 6,
                nDisappearIndex = 1,
                dDisappearDur = 0.1M,
                nShowMode = 2,
                LayerName = "Lyrics",
                EnableMusic = 1,
                LLnLineInterval = 10,
                BKVEnd = 1,
                BKCropH = 1080,

                Text = lyricsFilePath ?? "",
                fontlist = new LyricFontlist
                {
                    bloop = 1,
                    font = new LyricFontlistFont
                    {
                        clrFont = GetBGRUintColorFromSKColor(unsungTextColor) ?? 0xFFFFFF,
                        clrOutline = GetBGRUintColorFromSKColor(unsungOutlineColor) ?? 0x408080,
                        clrShining = 0xFFFFFF,
                        nOutlineFactor = 4,
                        nShadeFactor = 4,
                        nShineFactor = 2,
                        xDelProjection = 6,
                        yDelProjection = 6,
                        dTransProjection = 0.4M,
                        lfFaceName = "Arial",
                        lfHeight = 30
                    }
                },
                fontoptions = new LyricFontoptions
                {
                    fontname = "Arial",
                    lfHeight = 100,
                    clrFont = GetBGRUintColorFromSKColor(unsungTextColor) ?? 0xFFFFFF,
                    clrOutline = GetBGRUintColorFromSKColor(unsungOutlineColor) ?? 0x408080,
                    clrShade = 0xC0C0C0,
                    clrShining = 0x808080,
                    efe = 2,
                    nOutlineFactor = 8,
                    nShadeFactor = 4,
                    nShineFactor = 8,
                    clrGradient = 255,
                    clrHatch = 255,
                    xDelProjection = 6,
                    yDelProjection = 6,
                    dTransProjection = 0.4M
                },
                layervalidtime = new LyricLayervalidtime[] { },
                mediafile = mediaFilePath ?? "",
                dAudioDuration = (decimal)(lyrics?.Max(m => m.EndSecond) ?? 0D),
                item = lyrics?.Select(l => new LyricItem
                {
                    dStartTime = (decimal?)l.StartSecond ?? 0M,
                    dEndTime = (decimal?)l.EndSecond ?? 0M,
                    text = BuildInnerTextFromWords(l.Words)
                }).ToArray()
            };

            // dDuration??

            return fromScratch;
        }

        public static string BuildInnerTextFromWords(IList<LyricWord>? words)
        {
            var innerTextBuilder = new StringBuilder();
            if (words != null)
            {
                for (var i = 0; i < words.Count; i++)
                {
                    var word = words[i];
                    innerTextBuilder.Append(word.Text);
                    if (word != words.Last())
                    {
                        var nextEntry = Math.Round(words[i + 1].StartSecond, 2);
                        var difference = Math.Round(nextEntry - word.EndSecond, 2);
                        var restText = difference > 0.01 ? $"+{difference}" : null;
                        innerTextBuilder.Append((string?)$"<{Math.Round(word.EndSecond, 2)}{restText}>");
                    }
                }
            }

            var innertext = innerTextBuilder.ToString();
            return innertext;
        }

        public string InterpolateFileContents(List<RzlrcLyric>? originalFileContents, 
                                                RzlrcLyric? selectedPage,
                                                IList<LyricLine>? processedResults)
        {
            if (processedResults == null)
            {
                throw new ArgumentException("No chosen lines provided!");
            }
            if (originalFileContents == null)
            {
                throw new ArgumentException("RZLRC file to interpolate was not provided!");
            }
            if (selectedPage == null)
            {
                throw new ArgumentException("RZLRC page to interpolate was not provided!");
            }
            if (!originalFileContents.Contains(selectedPage))
            {
                throw new ArgumentException("RZLRC page to interpolate was not a member of the provided original contents!");
            }

            if (processedResults.Any(p =>
                    p.OriginalRzlrcItem != null && !selectedPage.item!.Contains(p.OriginalRzlrcItem)))
            {
                throw new ArgumentException(
                    "RZLRC page to interpolate was not the origin of one or more chosen items!");
            }

            // ok, everything is still wired up by ref
            foreach (var line in processedResults)
            {
                LyricItem itemToSet;
                if (line.OriginalRzlrcItem != null)
                {
                    itemToSet = line.OriginalRzlrcItem;
                }
                else
                {
                    // find closest item to clone
                    var closestItem = selectedPage.item!
                                          .Where(i => i.dStartTime > (decimal)line.StartSecond)
                                          .OrderBy(i => i.dStartTime)
                                          .FirstOrDefault()
                                      ?? selectedPage.item!.OrderBy(i => i.dStartTime).LastOrDefault();
                    itemToSet = Clone(closestItem) ?? new LyricItem();
                    selectedPage.item = selectedPage.item!.Append(itemToSet).ToArray();
                }
                itemToSet.dStartTime = ((decimal)line.StartSecond!);
                itemToSet.dEndTime = ((decimal)line.EndSecond!);
                itemToSet.text = BuildInnerTextFromWords(line.Words);
            }

            selectedPage.item = selectedPage.item?.OrderBy(i => i.dStartTime).ToArray();

            return SerializeToXmlString(originalFileContents);
        }

        private LyricItem? Clone(LyricItem? original)
        {
            if (original == null) return null;
            var ser = new XmlSerializer(typeof(LyricItem));

            var originalStringWriter = new StringWriter();
            ser.Serialize(originalStringWriter, original);
            var stringReader = new StringReader(originalStringWriter.ToString());
            return ser.Deserialize(stringReader) as LyricItem;
        }
    }
}
