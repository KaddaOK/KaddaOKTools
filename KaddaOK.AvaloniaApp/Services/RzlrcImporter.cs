using System;
using System.IO;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.Library.Ytmm;
using System.Xml.Serialization;
using Avalonia.Media;
using System.Collections.ObjectModel;
using System.Linq;
using KaddaOK.Library;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace KaddaOK.AvaloniaApp.Services
{
    public interface IRzlrcImporter
    {
        RzlrcLyrics ImportRzlrc(string rzlrcPath);
        Task LoadRzlrcPageIntoKaraokeProcessAsync(KaraokeProcess karaokeProcess, RzlrcLyric rzlrcPageToLoad, string originalPath);
    }

    public class RzlrcImporter : Importer, IRzlrcImporter
    {
        public RzlrcImporter(IAudioFromFile audioFileReader, IMinMaxFloatWaveStreamSampler sampler) : base(audioFileReader, sampler)
        {
        }

        private Color GetFromBGRUint(uint uintColor)
        {
            var colorHex = uintColor.ToString("X6");

            var blue = byte.Parse(colorHex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            var green = byte.Parse(colorHex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            var red = byte.Parse(colorHex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            return Color.FromRgb(red, green, blue);
        }

        private string MakeRzlrcContentsDeserializable(string originalContents)
        {
            return originalContents
                       .Insert(39, "<ArrayOfLyric>") // after xml declaration, add open wrapper tag for multiple lyrics layers
                       .Replace("3DMixColor", "ThreeDMixColor") // 3DMixColor and 3DMixColorOption are not valid XML attributes, but rzlrc don't care
                   + "</ArrayOfLyric>"; // close wrapper
        }

        public RzlrcLyrics ImportRzlrc(string rzlrcPath)
        {
            var xmlStringContents = File.ReadAllText(rzlrcPath);

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var ser = new XmlSerializer(typeof(RzlrcLyrics));

            var stringReader = new StringReader(MakeRzlrcContentsDeserializable(xmlStringContents));
            var rzlrcLyrics = ser.Deserialize(stringReader) as RzlrcLyrics;

            if (rzlrcLyrics == null || rzlrcLyrics.Count == 0)
            {
                // why?  idk.  do we care?  idk.
                throw new ArgumentException("Could not import RZLRC file.");
            }

            return rzlrcLyrics;
        }

        public async Task LoadRzlrcPageIntoKaraokeProcessAsync(KaraokeProcess karaokeProcess, RzlrcLyric rzlrcPageToLoad, string originalPath)
        {
            karaokeProcess.ImportedKaraokeSourceFilePath = originalPath;
            karaokeProcess.ExportToFilePath = originalPath;
            karaokeProcess.KaraokeSource = InitialKaraokeSource.RzlrcImport;
            karaokeProcess.BackgroundColor = GetFromBGRUint(rzlrcPageToLoad.BKColor);
            karaokeProcess.SungTextColor = GetFromBGRUint(rzlrcPageToLoad.clrOverlayFont);
            karaokeProcess.SungOutlineColor = GetFromBGRUint(rzlrcPageToLoad.clrOverlayEdge);
            karaokeProcess.UnsungTextColor = GetFromBGRUint(rzlrcPageToLoad.fontoptions.clrFont);
            karaokeProcess.UnsungOutlineColor = GetFromBGRUint(rzlrcPageToLoad.fontoptions.clrOutline);
            karaokeProcess.LyricsFilePath = rzlrcPageToLoad.Text;
            karaokeProcess.ChosenLines =
                new ObservableCollection<LyricLine>(rzlrcPageToLoad.item?.Select(GetFromRzlrcItem) ??
                                                    Array.Empty<LyricLine>());

            await LoadAudioIntoProcess(karaokeProcess, rzlrcPageToLoad.mediafile);

            karaokeProcess.RaiseChosenLinesChanged();
            karaokeProcess.CanExportFactorsChanged();
        }

        private LyricLine GetFromRzlrcItem(LyricItem rzlrcItem)
        {
            var instructionArray = rzlrcItem.text.Split('<', '>');

            var timeWithDelayRegex = new Regex(@"^(?'time'[0-9]+.?[0-9]*)(?:\+(?'delay'[0-9\.]+))?$");
            var lyricLine = new LyricLine(rzlrcItem)
            {
                IsSelected = true,
                Words = new ObservableCollection<LyricWord>()
            };
            var lineCanonicalText = new StringBuilder();
            LyricWord? currentWord = null;
            var nextWordStartTime = (double)rzlrcItem.dStartTime;
            foreach (var item in instructionArray)
            {
                var match = timeWithDelayRegex.Match(item);
                if (match.Success)
                {
                    // this is time
                    var time = double.Parse(match.Groups["time"].Value);
                    if (currentWord == null)
                    {
                        // TODO: show warning, because that's unexpected
                    }
                    else
                    {
                        currentWord.EndSecond = time;
                        lyricLine.Words.Add(currentWord);
                    }
                    
                    if (match.Groups.Count > 2 && match.Groups["delay"].Success)
                    {
                        var delay = double.Parse(match.Groups["delay"].Value);
                        nextWordStartTime = time + delay;
                    }
                    else
                    {
                        nextWordStartTime = time;
                    }
                }
                else
                {
                    // this is text
                    lineCanonicalText.Append(item);
                    currentWord = new LyricWord
                    {
                        StartSecond = nextWordStartTime,
                        Text = item
                    };
                }
            }

            if (currentWord == null)
            {
                throw new InvalidOperationException();
            }
            currentWord.EndSecond = (double)rzlrcItem.dEndTime;
            lyricLine.Words.Add(currentWord);
            LyricLine.MoveSpacesToEndsOfWords(lyricLine.Words);
            return lyricLine;
        }
    }
}
