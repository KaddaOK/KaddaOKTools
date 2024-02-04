namespace KaddaOK.Library.Ytmm
{
    public partial class RzProjectLine
    {
        public RzProjectLine()
        {
            Items = new List<RzLineItem>();
        }
    }

    public partial class RzLineItem
    {
        public RzLineItem()
        {
            GraffitiLayers = new List<GraffitiLayer>();
        }
    }

    public partial class GraffitiObj
    {
        public GraffitiObj()
        {
            Points = new List<GraffitiPoint>();
        }
    }
    public partial class RzProject
    {
        public RzProject()
        {
            nTimelineLeftBarWidth = 220;
            nTimeLineNameFontHeight = 11;
            dScaleFactorForPlay = 1;

            Lines = new List<RzProjectLine>();
        }

        public RzProjectLine BackgroundLine => GetLine(RzLinesSpec.Background, "Background");

        public RzProjectLine KaraokeLine => GetLine(RzLinesSpec.Text0, "Karaoke");

        public RzProjectLine Text1Line => GetLine(RzLinesSpec.Text1, "Text 1");

        public RzProjectLine Text2Line => GetLine(RzLinesSpec.Text2, "Text 2");

        public RzProjectLine VideoLine => GetLine(RzLinesSpec.Video, "Video");

        public RzProjectLine VideoOverlayLine => GetLine(RzLinesSpec.Overlay0, "Video Overlay");

        public RzProjectLine VideoSoundLine => GetLine(RzLinesSpec.Audio0, "Video Sound", true);

        public RzProjectLine OriginalAudioLine => GetLine(RzLinesSpec.Audio1, "Original Audio", true);

        public RzProjectLine NoVocalsAudioLine => GetLine(RzLinesSpec.Audio2, "No-Vocals Audio");

        public RzProjectLine VocalsOnlyAudioLine => GetLine(RzLinesSpec.Audio3, "Vocals-Only", true);

        public RzProjectLine GraffitiLine => GetLine(RzLinesSpec.Graffiti, "Progress Bars");

        public RzProjectLine GetLine(RzLinesSpec lineType, string lineName, bool isDisabled = false)
        {
            var line = Lines.SingleOrDefault(l => l.eLine == (int)lineType);
            if (line == null)
            {
                line = new RzProjectLine
                {
                    eLine = (int)lineType,
                    szLineName = lineName,
                    bAlwaysInFrontOfPrevLines = 1,
                    bDisable = (byte)(isDisabled ? 1 : 0)
                };
                Lines.Add(line);
            }

            return line;
        }
    }
}
