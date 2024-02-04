using System.ComponentModel;
using System.Xml.Serialization;

#pragma warning disable CS8618

namespace KaddaOK.Library.Ytmm
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(Namespace = "", TypeName = "Lyric")]
    [XmlRoot(Namespace = "", ElementName = "Lyric", IsNullable = false)]
    public partial class RzlrcLyric
    {
        public LyricLrc3dlayout lrc3dlayout { get; set; }
        public LyricFontlist fontlist { get; set; }
        public LyricParagraphs paragraphs { get; set; }
        public LyricFontoptions fontoptions { get; set; }
        public LyricLayervalidtime[] layervalidtime { get; set; }
        public string mediafile { get; set; }

        [XmlElement("item")]
        public LyricItem[]? item { get; set; }

        [XmlAttribute]
        public byte bEnableArea { get; set; }

        [XmlAttribute]
        public ushort xArea { get; set; }

        [XmlAttribute]
        public ushort yArea { get; set; }

        [XmlAttribute]
        public ushort wArea { get; set; }

        [XmlAttribute]
        public ushort hArea { get; set; }

        [XmlAttribute]
        public ushort wRefArea { get; set; }

        [XmlAttribute]
        public ushort hRefArea { get; set; }

        [XmlAttribute]
        public byte bLoopActionAppear { get; set; }

        [XmlAttribute]
        public byte bLoopActionDisappear { get; set; }

        [XmlAttribute]
        public string AppPicks { get; set; }

        [XmlAttribute]
        public string DisappPicks { get; set; }

        [XmlAttribute]
        public short n3DAppearIndex { get; set; }

        [XmlAttribute]
        public short n3DDisappearIndex { get; set; }

        [XmlAttribute]
        public byte bLoop3DActionAppear { get; set; }

        [XmlAttribute]
        public byte bLoop3DActionDisappear { get; set; }

        [XmlAttribute]
        public short wordlayout { get; set; }
        
        [XmlAttribute]
        public short ReverseBallInPara { get; set; }
        
        [XmlAttribute]
        public short nTargetResolutionRatio { get; set; }
        
        [XmlAttribute]
        public byte bFillActionDuration { get; set; }
        
        [XmlAttribute]
        public uint clrOverlayEdge { get; set; }
        
        [XmlAttribute]
        public uint clrOverlayFont { get; set; }
        
        [XmlAttribute]
        public uint clrOverlayShade { get; set; }
        
        [XmlAttribute]
        public uint clrOverlayShine { get; set; }
        
        [XmlAttribute]
        public short iocs00 { get; set; }
        
        [XmlAttribute]
        public short iocs01 { get; set; }
        
        [XmlAttribute]
        public short iocs02 { get; set; }
        
        [XmlAttribute]
        public short iocs03 { get; set; }
        
        [XmlAttribute]
        public short iocs04 { get; set; }
        
        [XmlAttribute]
        public short iocs05 { get; set; }
        
        [XmlAttribute]
        public short iocs06 { get; set; }
        
        [XmlAttribute]
        public short iocs07 { get; set; }
        
        [XmlAttribute]
        public short iocs08 { get; set; }
        
        [XmlAttribute]
        public short iocs09 { get; set; }
        
        [XmlAttribute]
        public short clrOverlayGradient { get; set; }
        
        [XmlAttribute]
        public short clrOverlayHatch { get; set; }
        
        [XmlAttribute]
        public short eAlignType { get; set; }
        
        [XmlAttribute]
        public short nMarginLeft { get; set; }
        
        [XmlAttribute]
        public short nMarginRight { get; set; }
        
        [XmlAttribute]
        public short nMarginTop { get; set; }
        
        [XmlAttribute]
        public short nMarginBottom { get; set; }
        
        [XmlAttribute]
        public byte bUseMarginLeft { get; set; }
        
        [XmlAttribute]
        public byte bUseMarginTop { get; set; }
        
        [XmlAttribute]
        public short nLineSpace { get; set; }
        
        [XmlAttribute]
        public short nAppearIndex { get; set; }
        
        [XmlAttribute]
        public short nDisappearIndex { get; set; }
        
        [XmlAttribute]
        public short nKaraOKEffectIndex { get; set; }
        
        [XmlAttribute]
        public short eKaraokeScanType { get; set; }
        
        [XmlAttribute]
        public short nTransparencyIndex { get; set; }
        
        [XmlAttribute]
        public decimal d3DSceneLayoutMaxDisappearDuration { get; set; }
        
        [XmlAttribute]
        public short n3DAlignType { get; set; }
        
        [XmlAttribute]
        public decimal d3DDepth { get; set; }
        
        [XmlAttribute]
        public short n3DTransparencyNormal { get; set; }
        
        [XmlAttribute]
        public short n3DTransparencyGone { get; set; }
        
        [XmlAttribute]
        public short n3DTransparencyBeing { get; set; }
        
        [XmlAttribute]
        public byte bEnable3DMixColorToBeing { get; set; }
        
        [XmlAttribute]
        public short ThreeDMixColor { get; set; }
        
        [XmlAttribute]
        public short ThreeDMixColorOption { get; set; }
        
        [XmlAttribute]
        public string strCurrent3DSceneLayoutPreFile { get; set; }
        
        [XmlAttribute]
        public decimal dAppearDur { get; set; }
        
        [XmlAttribute]
        public decimal dDisappearDur { get; set; }
        
        [XmlAttribute]
        public short nShowMode { get; set; }
        
        [XmlAttribute]
        public decimal dAudioDuration { get; set; }
        
        [XmlAttribute]
        public string Text { get; set; }
        
        [XmlAttribute]
        public string LayerName { get; set; }
        
        [XmlAttribute]
        public short DisableLayer { get; set; }
        
        [XmlAttribute]
        public byte bAutoReduceFontToFitScreen { get; set; }
        
        [XmlAttribute]
        public uint BKColor { get; set; }
        
        [XmlAttribute]
        public string BKImage { get; set; }
        
        [XmlAttribute]
        public short BKImageFillMode { get; set; }
        
        [XmlAttribute]
        public short IncludeBK { get; set; }
        
        [XmlAttribute]
        public short EnableMusic { get; set; }
        
        [XmlAttribute]
        public string BKVideo { get; set; }
        
        [XmlAttribute]
        public short BKVideoFillDurationMode { get; set; }
        
        [XmlAttribute]
        public decimal BKVStart { get; set; }
        
        [XmlAttribute]
        public decimal BKVEnd { get; set; }
        
        [XmlAttribute]
        public short BKCropEnable { get; set; }
        
        [XmlAttribute]
        public short BKCropX { get; set; }
        
        [XmlAttribute]
        public short BKCropY { get; set; }
        
        [XmlAttribute]
        public short BKCropW { get; set; }
        
        [XmlAttribute]
        public short BKCropH { get; set; }
        
        [XmlAttribute]
        public short BKFadeEnable { get; set; }
        
        [XmlAttribute]
        public short BKFadeIn { get; set; }
        
        [XmlAttribute]
        public short BKFadeOut { get; set; }
        
        [XmlAttribute]
        public byte bParticleEmissionDensityControlledByAudio { get; set; }
        
        [XmlAttribute]
        public short nCurrentAudioPitch { get; set; }

        [XmlAttribute]
        public short LLbEnable { get; set; }

        [XmlAttribute]
        public short LLnAlign { get; set; }

        [XmlAttribute]
        public short LLnLineInterval { get; set; }

        [XmlAttribute]
        public decimal dDuration { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class LyricLrc3dlayout
    {
        [XmlElement("item")]
        public LyricLrc3dlayoutItem[] item { get; set; }

        [XmlAttribute("CameraAppear.x")]
        public short CameraAppearx { get; set; }

        [XmlAttribute("CameraAppear.y")]
        public short CameraAppeary { get; set; }

        [XmlAttribute("CameraAppear.z")]
        public short CameraAppearz { get; set; }

        [XmlAttribute("CameraAppear.ax")]
        public short CameraAppearax { get; set; }

        [XmlAttribute("CameraAppear.ay")]
        public short CameraAppearay { get; set; }

        [XmlAttribute("CameraAppear.az")]
        public short CameraAppearaz { get; set; }

        [XmlAttribute("CameraDisappear.x")]
        public short CameraDisappearx { get; set; }

        [XmlAttribute("CameraDisappear.y")]
        public short CameraDisappeary { get; set; }

        [XmlAttribute("CameraDisappear.z")]
        public short CameraDisappearz { get; set; }

        [XmlAttribute("CameraDisappear.ax")]
        public short CameraDisappearax { get; set; }

        [XmlAttribute("CameraDisappear.ay")]
        public short CameraDisappearay { get; set; }

        [XmlAttribute("CameraDisappear.az")]
        public short CameraDisappearaz { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class LyricLrc3dlayoutItem
    {
        [XmlAttribute]
        public short nDelWX { get; set; }

        [XmlAttribute]
        public short nDelHY { get; set; }

        [XmlAttribute]
        public short X { get; set; }

        [XmlAttribute]
        public short Y { get; set; }

        [XmlAttribute]
        public short Z { get; set; }

        [XmlAttribute]
        public short AX { get; set; }

        [XmlAttribute]
        public short AY { get; set; }

        [XmlAttribute]
        public short AZ { get; set; }

        [XmlAttribute]
        public short nRotateBy { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class LyricFontlist
    {
        public LyricFontlistFont font { get; set; }

        [XmlAttribute]
        public byte bloop { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class LyricFontlistFont : FontBase
    {
        [XmlAttribute]
        public short repeat { get; set; }

        [XmlAttribute]
        public string szFontColorImage { get; set; }

        [XmlAttribute]
        public short lfCharSet { get; set; }

        [XmlAttribute]
        public short lfClipPrecision { get; set; }

        [XmlAttribute]
        public short lfEscapement { get; set; }

        [XmlAttribute]
        public string lfFaceName { get; set; }

        [XmlAttribute]
        public short lfHeight { get; set; }

        [XmlAttribute]
        public short lfItalic { get; set; }

        [XmlAttribute]
        public short lfOrientation { get; set; }

        [XmlAttribute]
        public short lfOutPrecision { get; set; }

        [XmlAttribute]
        public short lfPitchAndFamily { get; set; }

        [XmlAttribute]
        public short lfQuality { get; set; }

        [XmlAttribute]
        public short lfStrikeOut { get; set; }

        [XmlAttribute]
        public short lfUnderline { get; set; }

        [XmlAttribute]
        public short lfWeight { get; set; }

        [XmlAttribute]
        public short lfWidth { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class LyricParagraphs
    {
        [XmlElement("para")]
        public LyricParagraphsPara[] para { get; set; }

        [XmlAttribute]
        public short Effect { get; set; }

        [XmlAttribute]
        public short ScrollMode { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class LyricParagraphsPara
    {
        public Customizelyt customizelyt { get; set; }

        [XmlAttribute]
        public short nStartLine { get; set; }

        [XmlAttribute]
        public short nAlign { get; set; }

        [XmlAttribute]
        public short nAngle { get; set; }

        [XmlAttribute]
        public short nLineInterval { get; set; }

        [XmlAttribute]
        public short nInnerAlign { get; set; }

        [XmlAttribute]
        public short nScrollDirection { get; set; }

        [XmlAttribute]
        public short nGradient { get; set; }

        [XmlAttribute]
        public decimal dAppears { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class Customizelyt
    {
        [XmlElement("item")]
        public CustomizelytItem[] item { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class CustomizelytItem
    {
        [XmlAttribute]
        public byte bVerticalFont { get; set; }

        [XmlAttribute]
        public short nAngle { get; set; }

        [XmlAttribute]
        public short nHDel { get; set; }

        [XmlAttribute]
        public short nWDel { get; set; }

        [XmlAttribute]
        public short xDelToPrev { get; set; }

        [XmlAttribute]
        public short yDelToPrev { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class LyricFontoptions : FontBase
    {
        public string fontname { get; set; }

        [XmlAttribute]
        public short lfCharSet { get; set; }

        [XmlAttribute]
        public short lfClipPrecision { get; set; }

        [XmlAttribute]
        public short lfEscapement { get; set; }

        [XmlAttribute]
        public short lfHeight { get; set; }

        [XmlAttribute]
        public short lfItalic { get; set; }

        [XmlAttribute]
        public short lfOrientation { get; set; }

        [XmlAttribute]
        public short lfOutPrecision { get; set; }

        [XmlAttribute]
        public short lfPitchAndFamily { get; set; }

        [XmlAttribute]
        public short lfQuality { get; set; }

        [XmlAttribute]
        public short lfStrikeOut { get; set; }

        [XmlAttribute]
        public short lfUnderline { get; set; }

        [XmlAttribute]
        public short lfWeight { get; set; }

        [XmlAttribute]
        public short lfWidth { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class LyricLayervalidtime
    {
        [XmlAttribute]
        public byte bEnable { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class LyricItem
    {
        public string text { get; set; }

        public LyricItemFontList FontList { get; set; }

        [XmlAttribute]
        public decimal dStartTime { get; set; }

        [XmlAttribute]
        public decimal dEndTime { get; set; }

        [XmlAttribute]
        public short n3DRhythm { get; set; }

        [XmlAttribute]
        public short n3DRhythmStyle { get; set; }

        [XmlAttribute]
        public short n3DRotateX { get; set; }

        [XmlAttribute]
        public short n3DRotateY { get; set; }

        [XmlAttribute]
        public short n3DRotateZ { get; set; }

        [XmlAttribute]
        public string str3DSceneLayoutFile { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class LyricItemFontList
    {
        public LyricItemFontListFont font { get; set; }

        [XmlAttribute]
        public byte bloop { get; set; }

        [XmlAttribute]
        public byte bUseKaraokeOverlayColors { get; set; }

        [XmlAttribute]
        public uint clrFontOutline { get; set; }

        [XmlAttribute]
        public uint clrFont { get; set; }

        [XmlAttribute]
        public uint clrFontShine { get; set; }

        [XmlAttribute]
        public uint clrFontShade { get; set; }

        [XmlAttribute]
        public short clrFontGradient { get; set; }

        [XmlAttribute]
        public short clrFontHatch { get; set; }

        [XmlAttribute]
        public string strImageFont { get; set; }

        [XmlAttribute]
        public short lineocs00 { get; set; }

        [XmlAttribute]
        public short lineocs01 { get; set; }

        [XmlAttribute]
        public short lineocs02 { get; set; }

        [XmlAttribute]
        public short lineocs03 { get; set; }

        [XmlAttribute]
        public short lineocs04 { get; set; }

        [XmlAttribute]
        public short lineocs05 { get; set; }

        [XmlAttribute]
        public short lineocs06 { get; set; }

        [XmlAttribute]
        public short lineocs07 { get; set; }

        [XmlAttribute]
        public short lineocs08 { get; set; }

        [XmlAttribute]
        public short lineocs09 { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class LyricItemFontListFont : FontBase
    {
        [XmlAttribute]
        public short repeat { get; set; }

        [XmlAttribute]
        public string szFontColorImage { get; set; }

        [XmlAttribute]
        public short lfCharSet { get; set; }

        [XmlAttribute]
        public short lfClipPrecision { get; set; }

        [XmlAttribute]
        public short lfEscapement { get; set; }

        [XmlAttribute]
        public string lfFaceName { get; set; }

        [XmlAttribute]
        public short lfHeight { get; set; }

        [XmlAttribute]
        public short lfItalic { get; set; }

        [XmlAttribute]
        public short lfOrientation { get; set; }

        [XmlAttribute]
        public short lfOutPrecision { get; set; }

        [XmlAttribute]
        public short lfPitchAndFamily { get; set; }

        [XmlAttribute]
        public short lfQuality { get; set; }

        [XmlAttribute]
        public short lfStrikeOut { get; set; }

        [XmlAttribute]
        public short lfUnderline { get; set; }

        [XmlAttribute]
        public short lfWeight { get; set; }

        [XmlAttribute]
        public short lfWidth { get; set; }
    }
}
