using System.Xml.Serialization;

namespace KaddaOK.Library.Ytmm
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName="project", Namespace = "", IsNullable = false)]
    public partial class RzProject
    {        
        public RzEncoderSettings? EncoderSettings { get; set; }

        [XmlElement("line")]
        public List<RzProjectLine> Lines { get; set; }

        [XmlAttribute]
        public int fps { get; set; }

        [XmlAttribute]
        public uint clrBackground { get; set; }

        [XmlAttribute]
        public int nTimeLineNameFontHeight { get; set; }

        [XmlAttribute]
        public int nTimelineLeftBarWidth { get; set; }

        [XmlAttribute]
        public int CH { get; set; }

        [XmlAttribute]
        public int BH { get; set; }

        [XmlAttribute]
        public int VH { get; set; }

        [XmlAttribute]
        public int O0H { get; set; }

        [XmlAttribute]
        public int O1H { get; set; }

        [XmlAttribute]
        public int O2H { get; set; }

        [XmlAttribute]
        public int O3H { get; set; }

        [XmlAttribute]
        public int O4H { get; set; }

        [XmlAttribute]
        public int O5H { get; set; }

        [XmlAttribute]
        public int O6H { get; set; }

        [XmlAttribute]
        public int O7H { get; set; }

        [XmlAttribute]
        public int O8H { get; set; }

        [XmlAttribute]
        public int O9H { get; set; }

        [XmlAttribute]
        public int O10H { get; set; }

        [XmlAttribute]
        public int O11H { get; set; }

        [XmlAttribute]
        public int O12H { get; set; }

        [XmlAttribute]
        public int O13H { get; set; }

        [XmlAttribute]
        public int O14H { get; set; }

        [XmlAttribute]
        public int O15H { get; set; }

        [XmlAttribute]
        public int A0H { get; set; }

        [XmlAttribute]
        public int A1H { get; set; }

        [XmlAttribute]
        public int A2H { get; set; }

        [XmlAttribute]
        public int A3H { get; set; }

        [XmlAttribute]
        public int T0H { get; set; }

        [XmlAttribute]
        public int T1H { get; set; }

        [XmlAttribute]
        public int T2H { get; set; }

        [XmlAttribute]
        public int T3H { get; set; }

        [XmlAttribute]
        public int T4H { get; set; }

        [XmlAttribute]
        public int T5H { get; set; }

        [XmlAttribute]
        public int T6H { get; set; }

        [XmlAttribute]
        public int T7H { get; set; }

        [XmlAttribute]
        public int T8H { get; set; }
        [XmlAttribute]
        public int T9H { get; set; }

        [XmlAttribute]
        public int T10H { get; set; }

        [XmlAttribute]
        public int T11H { get; set; }

        [XmlAttribute]
        public int SH { get; set; }

        [XmlAttribute]
        public int GH { get; set; }

        [XmlAttribute]
        public decimal dScaleFactorForPlay { get; set; }

        [XmlAttribute]
        public byte b3D { get; set; }

        [XmlAttribute]
        public byte bEnableFog { get; set; }

        [XmlAttribute]
        public byte bEnableLighting { get; set; }

        [XmlAttribute]
        public uint clrFog { get; set; }

        [XmlAttribute]
        public uint clrLightAmbient { get; set; }

        [XmlAttribute]
        public uint clrLightDiffuse { get; set; }

        [XmlAttribute]
        public int vensettingIndex { get; set; }

        [XmlAttribute]
        public int count { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public partial class RzEncoderSettings
    {
        [XmlAttribute]
        public byte bAudioSimulateSurround { get; set; }

        [XmlAttribute]
        public byte bFlipSourceVideo { get; set; }

        [XmlAttribute]
        public byte bForceSourceResized { get; set; }

        [XmlAttribute]
        public int eTargetSizeMode { get; set; }

        [XmlAttribute]
        public int eVResolution { get; set; }

        [XmlAttribute]
        public int eVType { get; set; }

        [XmlAttribute]
        public int lBitRate { get; set; }

        [XmlAttribute]
        public int lFileSize { get; set; }

        [XmlAttribute]
        public int lFormat { get; set; }

        [XmlAttribute]
        public int lFrameHeight { get; set; }

        [XmlAttribute]
        public int lFrameWidth { get; set; }

        [XmlAttribute]
        public int lOverlayQuality { get; set; }

        [XmlAttribute]
        public int lQuality { get; set; }

        [XmlAttribute]
        public int lWMAEncoder { get; set; }

        [XmlAttribute]
        public int lWMAFormat { get; set; }

        [XmlAttribute]
        public int lWMVEncoder { get; set; }

        [XmlAttribute]
        public int nAudioBitrate { get; set; }

        [XmlAttribute]
        public int nAudioChannels { get; set; }

        [XmlAttribute]
        public int nAudioSamplerate { get; set; }

        [XmlAttribute]
        public int nIndex { get; set; }

        [XmlAttribute]
        public int nMp4ProfileIndex { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public partial class RzProjectLine
    {
        private int subtitlebottommarginField;
        private bool subtitlebottommarginFieldSpecified;

        [XmlElement("item")]
        public List<RzLineItem> Items { get; set; }

        [XmlAttribute]
        public int eLine { get; set; }

        [XmlAttribute]
        public byte bAsReferenceLineForTemplate { get; set; }

        [XmlAttribute]
        public byte bAutoAdjustDurationEqualToReferenceLineForTemplate { get; set; }

        [XmlAttribute]
        public string? szLineName { get; set; }

        [XmlAttribute]
        public byte bUse3DGroupTemplate { get; set; }

        [XmlAttribute]
        public byte bDisable { get; set; }

        [XmlAttribute]
        public byte b3DGroup { get; set; }

        [XmlAttribute]
        public int ax3DGroup { get; set; }

        [XmlAttribute]
        public int ay3DGroup { get; set; }

        [XmlAttribute]
        public int az3DGroup { get; set; }

        [XmlAttribute]
        public int x3DGroup { get; set; }

        [XmlAttribute]
        public int y3DGroup { get; set; }

        [XmlAttribute]
        public int z3DGroup { get; set; }

        [XmlAttribute]
        public int enableLighting3DGroup { get; set; }

        [XmlAttribute]
        public uint clrLightAmbient3DGroup { get; set; }

        [XmlAttribute]
        public uint clrLightDiffuse3DGroup { get; set; }

        [XmlAttribute]
        public byte bAlwaysInFrontOfPrevLines { get; set; }

        [XmlAttribute]
        public int subtitlebottommargin
        {
            get => this.subtitlebottommarginField;
            set
            {
                this.subtitlebottommarginField = value;
                subtitlebottommarginSpecified = true;
            }
        }

        [XmlIgnore]
        public bool subtitlebottommarginSpecified
        {
            get => this.subtitlebottommarginFieldSpecified;
            set => this.subtitlebottommarginFieldSpecified = value;
        }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public partial class RzLineItem
    {
        private List<GraffitiLayer>? graffitilayerField;
        private CustomMotion? custommotionField;
        private VuMeter? vumeterField;
        private LineItemColorblk? colorblkField;
        private ThreeDGroupSettings? _3DGroupSettingsField;
        private string? fontnameField;
        private string? textField;
        private object? fontcolorimageField;
        private string? sourceField;
        private decimal dXParticleDelField;
        private bool dXParticleDelFieldSpecified;
        private decimal dYParticleDelField;
        private bool dYParticleDelFieldSpecified;
        private byte bFillBlurToBlankWhenKeepRatioField;
        private bool bFillBlurToBlankWhenKeepRatioFieldSpecified;
        private byte bParticleEmissionDensityControlledByAudioField;
        private bool bParticleEmissionDensityControlledByAudioFieldSpecified;
        private byte rotateAny2DOverlaybEnableField;
        private bool rotateAny2DOverlaybEnableFieldSpecified;
        private byte rotateAny2DOverlaybFlipField;
        private bool rotateAny2DOverlaybFlipFieldSpecified;
        private byte rotateAny2DOverlaybReverseField;
        private bool rotateAny2DOverlaybReverseFieldSpecified;
        private int rotateAny2DOverlaylRotationAngleField;
        private bool rotateAny2DOverlaylRotationAngleFieldSpecified;
        private int channelOptionField;
        private bool channelOptionFieldSpecified;
        private byte bUseStartKeyFrameFor3DGroupField;
        private bool bUseStartKeyFrameFor3DGroupFieldSpecified;
        private decimal dStartTimeOfKeyFrameFor3DGroupField;
        private bool dStartTimeOfKeyFrameFor3DGroupFieldSpecified;
        private int nAudioPitchField;
        private bool nAudioPitchFieldSpecified;
        private byte bVocalCutField;
        private bool bVocalCutFieldSpecified;
        private decimal dAudioDelayField;
        private bool dAudioDelayFieldSpecified;
        private byte bEnableCameraField;
        private bool bEnableCameraFieldSpecified;
        private byte bReversePlaybackField;
        private bool bReversePlaybackFieldSpecified;
        private byte bEmptyFrameField;
        private bool bEmptyFrameFieldSpecified;
        private byte bUseTextTemplateField;
        private bool bUseTextTemplateFieldSpecified;
        private decimal dTypographyTransparenceField;
        private bool dTypographyTransparenceFieldSpecified;
        private byte bEnableLineWrapField;
        private bool bEnableLineWrapFieldSpecified;
        private int nMaxCharsPerLineForLineWrapField;
        private bool nMaxCharsPerLineForLineWrapFieldSpecified;
        private decimal dxAngleField;
        private bool dxAngleFieldSpecified;
        private decimal dxScaleField;
        private bool dxScaleFieldSpecified;
        private decimal dyAngleField;
        private bool dyAngleFieldSpecified;
        private decimal dyScaleField;
        private bool dyScaleFieldSpecified;
        private decimal dzAngleField;
        private bool dzAngleFieldSpecified;
        private decimal dzScaleField;
        private bool dzScaleFieldSpecified;
        private decimal n3DepthField;
        private bool n3DepthFieldSpecified;
        private decimal dzDelPosField;
        private bool dzDelPosFieldSpecified;
        private byte bSubtitleFileField;
        private bool bSubtitleFileFieldSpecified;
        private int nSubtitleChapterIndexField;
        private bool nSubtitleChapterIndexFieldSpecified;
        private byte bDisableAudioField;
        private bool bDisableAudioFieldSpecified;
        private byte bHadAudioField;
        private bool bHadAudioFieldSpecified;
        private decimal dASelfDurationField;
        private bool dASelfDurationFieldSpecified;
        private decimal dASelfEndTimeField;
        private bool dASelfEndTimeFieldSpecified;
        private decimal dASelfStartTimeField;
        private bool dASelfStartTimeFieldSpecified;
        private decimal dEditorEndTimeField;
        private bool dEditorEndTimeFieldSpecified;
        private decimal dEditorStartTimeField;
        private bool dEditorStartTimeFieldSpecified;
        private decimal dVSelfDurationField;
        private bool dVSelfDurationFieldSpecified;
        private decimal dVSelfEndTimeField;
        private bool dVSelfEndTimeFieldSpecified;
        private decimal dVSelfStartTimeField;
        private bool dVSelfStartTimeFieldSpecified;
        private decimal dTransitionDurationField;
        private bool dTransitionDurationFieldSpecified;
        private decimal dPlaybackRateField;
        private bool dPlaybackRateFieldSpecified;
        private decimal dVoiceScaleField;
        private bool dVoiceScaleFieldSpecified;
        private int eImageFillModeField;
        private bool eImageFillModeFieldSpecified;
        private int eMediaTypeField;
        private bool eMediaTypeFieldSpecified;
        private int eMediaTypeLineField;
        private bool eMediaTypeLineFieldSpecified;
        private int nFileNameWidthField;
        private bool nFileNameWidthFieldSpecified;
        private int nIndexField;
        private bool nIndexFieldSpecified;
        private int nIndexVEffectField;
        private bool nIndexVEffectFieldSpecified;
        private int nIndexVTranEffectField;
        private bool nIndexVTranEffectFieldSpecified;
        private int nVideoWidthField;
        private bool nVideoWidthFieldSpecified;
        private int nVideoHeightField;
        private bool nVideoHeightFieldSpecified;
        private decimal dwselptrinmainappField;
        private bool dwselptrinmainappFieldSpecified;
        private int nCountCaptureImageForEffect_EndField;
        private bool nCountCaptureImageForEffect_EndFieldSpecified;
        private int nIndexVTranEffect3DField;
        private bool nIndexVTranEffect3DFieldSpecified;
        private decimal overlaydTransparentField;
        private bool overlaydTransparentFieldSpecified;
        private int overlaynHeightField;
        private bool overlaynHeightFieldSpecified;
        private int overlaynWidthField;
        private bool overlaynWidthFieldSpecified;
        private int overlayXField;
        private bool overlayXFieldSpecified;
        private int overlayYField;
        private bool overlayYFieldSpecified;
        private byte bEnableAudioEffectField;
        private bool bEnableAudioEffectFieldSpecified;
        private decimal dAudioFadeInDurationField;
        private bool dAudioFadeInDurationFieldSpecified;
        private decimal dAudioFadeOutDurationField;
        private bool dAudioFadeOutDurationFieldSpecified;
        private decimal dEditStartTimeField;
        private bool dEditStartTimeFieldSpecified;
        private decimal dEditEndTimeField;
        private bool dEditEndTimeFieldSpecified;
        private int actionToMediaIndexField;
        private bool actionToMediaIndexFieldSpecified;
        private decimal actiondTransparentEndField;
        private bool actiondTransparentEndFieldSpecified;
        private decimal actiondTransparentStartField;
        private bool actiondTransparentStartFieldSpecified;
        private int actionrcEnd_leftField;
        private bool actionrcEnd_leftFieldSpecified;
        private int actionrcEnd_topField;
        private bool actionrcEnd_topFieldSpecified;
        private int actionrcEnd_rightField;
        private bool actionrcEnd_rightFieldSpecified;
        private int actionrcEnd_bottomField;
        private bool actionrcEnd_bottomFieldSpecified;
        private int actionrcStart_leftField;
        private bool actionrcStart_leftFieldSpecified;
        private int actionrcStart_topField;
        private bool actionrcStart_topFieldSpecified;
        private int actionrcStart_rightField;
        private bool actionrcStart_rightFieldSpecified;
        private int actionrcStart_bottomField;
        private bool actionrcStart_bottomFieldSpecified;
        private int actionulfHeightEndField;
        private bool actionulfHeightEndFieldSpecified;
        private int actionulfHeightStartField;
        private bool actionulfHeightStartFieldSpecified;
        private int nAppearTypeField;
        private bool nAppearTypeFieldSpecified;
        private int nDisappearTypeField;
        private bool nDisappearTypeFieldSpecified;
        private decimal appearDurField;
        private bool appearDurFieldSpecified;
        private decimal disappearDurField;
        private bool disappearDurFieldSpecified;
        private byte bFillInDurationField;
        private bool bFillInDurationFieldSpecified;
        private byte bCustomizeActionField;
        private bool bCustomizeActionFieldSpecified;
        private int nLoopCustomCountField;
        private bool nLoopCustomCountFieldSpecified;
        private byte bDoneInBlockField;
        private bool bDoneInBlockFieldSpecified;
        private byte bKeepOriginalSurroundingOfBlockField;
        private bool bKeepOriginalSurroundingOfBlockFieldSpecified;
        private int nClarityStartField;
        private bool nClarityStartFieldSpecified;
        private int nClarityEndField;
        private bool nClarityEndFieldSpecified;
        private int shake_0Field;
        private bool shake_0FieldSpecified;
        private int shake_1Field;
        private bool shake_1FieldSpecified;
        private int shake_2Field;
        private bool shake_2FieldSpecified;
        private int shake_3Field;
        private bool shake_3FieldSpecified;
        private int shake_4Field;
        private bool shake_4FieldSpecified;
        private int shake_5Field;
        private bool shake_5FieldSpecified;
        private int shake_6Field;
        private bool shake_6FieldSpecified;
        private int shake_7Field;
        private bool shake_7FieldSpecified;
        private int shake_8Field;
        private bool shake_8FieldSpecified;
        private int shake_9Field;
        private bool shake_9FieldSpecified;
        private int shake_10Field;
        private bool shake_10FieldSpecified;
        private int screenAligneAlignField;
        private bool screenAligneAlignFieldSpecified;
        private int screenAlignnBottomMarginField;
        private bool screenAlignnBottomMarginFieldSpecified;
        private int screenAlignnLeftMarginField;
        private bool screenAlignnLeftMarginFieldSpecified;
        private int screenAlignnRightMarginField;
        private bool screenAlignnRightMarginFieldSpecified;
        private int screenAlignnTopMarginField;
        private bool screenAlignnTopMarginFieldSpecified;
        private int nAlignField;
        private bool nAlignFieldSpecified;
        private uint titleclrTextField;
        private bool titleclrTextFieldSpecified;
        private int titledTranparentField;
        private bool titledTranparentFieldSpecified;
        private int titlelfCharSetField;
        private bool titlelfCharSetFieldSpecified;
        private int titlelfClipPrecisionField;
        private bool titlelfClipPrecisionFieldSpecified;
        private int titlelfEscapementField;
        private bool titlelfEscapementFieldSpecified;
        private int titlelfHeightField;
        private bool titlelfHeightFieldSpecified;
        private int titlelfItalicField;
        private bool titlelfItalicFieldSpecified;
        private int titlelfOrientationField;
        private bool titlelfOrientationFieldSpecified;
        private int titlelfOutPrecisionField;
        private bool titlelfOutPrecisionFieldSpecified;
        private int titlelfPitchAndFamilyField;
        private bool titlelfPitchAndFamilyFieldSpecified;
        private int titlelfQualityField;
        private bool titlelfQualityFieldSpecified;
        private int titlelfStrikeOutField;
        private bool titlelfStrikeOutFieldSpecified;
        private int titlelfUnderlineField;
        private bool titlelfUnderlineFieldSpecified;
        private int titlelfWeightField;
        private bool titlelfWeightFieldSpecified;
        private int titlelfWidthField;
        private bool titlelfWidthFieldSpecified;
        private int titleXField;
        private bool titleXFieldSpecified;
        private decimal titlexposScaleField;
        private bool titlexposScaleFieldSpecified;
        private int titleYField;
        private bool titleYFieldSpecified;
        private decimal titleyposScaleField;
        private bool titleyposScaleFieldSpecified;
        private byte bBoldField;
        private bool bBoldFieldSpecified;
        private byte bVerticalField;
        private bool bVerticalFieldSpecified;
        private uint clrFontField;
        private bool clrFontFieldSpecified;
        private uint clrOutlineField;
        private bool clrOutlineFieldSpecified;
        private uint clrShadeField;
        private bool clrShadeFieldSpecified;
        private uint clrShiningField;
        private bool clrShiningFieldSpecified;
        private int efeField;
        private bool efeFieldSpecified;
        private int nOutlineFactorField;
        private bool nOutlineFactorFieldSpecified;
        private int nShadeFactorField;
        private bool nShadeFactorFieldSpecified;
        private int nShineFactorField;
        private bool nShineFactorFieldSpecified;
        private int nRotateAngleField;
        private bool nRotateAngleFieldSpecified;
        private uint clrGradientField;
        private bool clrGradientFieldSpecified;
        private uint clrHatchField;
        private bool clrHatchFieldSpecified;
        private int eDataTypeField;
        private bool eDataTypeFieldSpecified;
        private int eGradientTypeField;
        private bool eGradientTypeFieldSpecified;
        private int nHatchIndexField;
        private bool nHatchIndexFieldSpecified;
        private byte bEnableProjectionField;
        private bool bEnableProjectionFieldSpecified;
        private int xDelProjectionField;
        private bool xDelProjectionFieldSpecified;
        private int yDelProjectionField;
        private bool yDelProjectionFieldSpecified;
        private decimal dTransProjectionField;
        private bool dTransProjectionFieldSpecified;
        private byte bUseInsertColorsField;
        private bool bUseInsertColorsFieldSpecified;
        private uint ics00Field;
        private bool ics00FieldSpecified;
        private uint ics01Field;
        private bool ics01FieldSpecified;
        private int ics02Field;
        private bool ics02FieldSpecified;
        private int ics03Field;
        private bool ics03FieldSpecified;
        private int ics04Field;
        private bool ics04FieldSpecified;
        private int ics05Field;
        private bool ics05FieldSpecified;
        private int ics06Field;
        private bool ics06FieldSpecified;
        private int ics07Field;
        private bool ics07FieldSpecified;
        private int ics08Field;
        private bool ics08FieldSpecified;
        private int ics09Field;
        private bool ics09FieldSpecified;

        [XmlElement("graffitilayer")]
        public List<GraffitiLayer>? GraffitiLayers
        {
            get => this.graffitilayerField;
            set => this.graffitilayerField = value;
        }

        public CustomMotion? custommotion
        {
            get => this.custommotionField;
            set => this.custommotionField = value;
        }

        public VuMeter? vumeter
        {
            get => this.vumeterField;
            set => this.vumeterField = value;
        }

        public LineItemColorblk? colorblk
        {
            get => this.colorblkField;
            set => this.colorblkField = value;
        }

        public ThreeDGroupSettings? _3DGroupSettings
        {
            get => this._3DGroupSettingsField;
            set => this._3DGroupSettingsField = value;
        }

        public string? fontname
        {
            get => this.fontnameField;
            set => this.fontnameField = value;
        }

        public string? text
        {
            get => this.textField;
            set => this.textField = value;
        }

        public object? fontcolorimage
        {
            get => this.fontcolorimageField;
            set => this.fontcolorimageField = value;
        }

        public string? source
        {
            get => this.sourceField;
            set => this.sourceField = value;
        }

        [XmlAttribute]
        public int Shake_0
        {
            get => this.shake_0Field;
            set
            {
                this.shake_0Field = value;
                Shake_0Specified = true;
            }
        }

        [XmlIgnore]
        public bool Shake_0Specified
        {
            get => this.shake_0FieldSpecified;
            set => this.shake_0FieldSpecified = value;
        }

        [XmlAttribute]
        public int Shake_1
        {
            get => this.shake_1Field;
            set
            {
                this.shake_1Field = value;
                Shake_1Specified = true;
            }
        }

        [XmlIgnore]
        public bool Shake_1Specified
        {
            get => this.shake_1FieldSpecified;
            set => this.shake_1FieldSpecified = value;
        }

        [XmlAttribute]
        public int Shake_2
        {
            get => this.shake_2Field;
            set
            {
                this.shake_2Field = value;
                Shake_2Specified = true;
            }
        }

        [XmlIgnore]
        public bool Shake_2Specified
        {
            get => this.shake_2FieldSpecified;
            set => this.shake_2FieldSpecified = value;
        }

        [XmlAttribute]
        public int Shake_3
        {
            get => this.shake_3Field;
            set
            {
                this.shake_3Field = value;
                Shake_3Specified = true;
            }
        }

        [XmlIgnore]
        public bool Shake_3Specified
        {
            get => this.shake_3FieldSpecified;
            set => this.shake_3FieldSpecified = value;
        }

        [XmlAttribute]
        public int Shake_4
        {
            get => this.shake_4Field;
            set
            {
                this.shake_4Field = value;
                Shake_4Specified = true;
            }
        }

        [XmlIgnore]
        public bool Shake_4Specified
        {
            get => this.shake_4FieldSpecified;
            set => this.shake_4FieldSpecified = value;
        }

        [XmlAttribute]
        public int Shake_5
        {
            get => this.shake_5Field;
            set
            {
                this.shake_5Field = value;
                Shake_5Specified = true;
            }
        }

        [XmlIgnore]
        public bool Shake_5Specified
        {
            get => this.shake_5FieldSpecified;
            set => this.shake_5FieldSpecified = value;
        }

        [XmlAttribute]
        public int Shake_6
        {
            get => this.shake_6Field;
            set
            {
                this.shake_6Field = value;
                Shake_6Specified = true;
            }
        }

        [XmlIgnore]
        public bool Shake_6Specified
        {
            get => this.shake_6FieldSpecified;
            set => this.shake_6FieldSpecified = value;
        }

        [XmlAttribute]
        public int Shake_7
        {
            get => this.shake_7Field;
            set
            {
                this.shake_7Field = value;
                Shake_7Specified = true;
            }
        }

        [XmlIgnore]
        public bool Shake_7Specified
        {
            get => this.shake_7FieldSpecified;
            set => this.shake_7FieldSpecified = value;
        }

        [XmlAttribute]
        public int Shake_8
        {
            get => this.shake_8Field;
            set
            {
                this.shake_8Field = value;
                Shake_8Specified = true;
            }
        }

        [XmlIgnore]
        public bool Shake_8Specified
        {
            get => this.shake_8FieldSpecified;
            set => this.shake_8FieldSpecified = value;
        }

        [XmlAttribute]
        public int Shake_9
        {
            get => this.shake_9Field;
            set
            {
                this.shake_9Field = value;
                Shake_9Specified = true;
            }
        }

        [XmlIgnore]
        public bool Shake_9Specified
        {
            get => this.shake_9FieldSpecified;
            set => this.shake_9FieldSpecified = value;
        }

        [XmlAttribute]
        public int Shake_10
        {
            get => this.shake_10Field;
            set
            {
                this.shake_10Field = value;
                Shake_10Specified = true;
            }
        }

        [XmlIgnore]
        public bool Shake_10Specified
        {
            get => this.shake_10FieldSpecified;
            set => this.shake_10FieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dEditStartTime
        {
            get => this.dEditStartTimeField;
            set
            {
                this.dEditStartTimeField = value;
                dEditStartTimeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dEditStartTimeSpecified
        {
            get => this.dEditStartTimeFieldSpecified;
            set => this.dEditStartTimeFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dEditEndTime
        {
            get => this.dEditEndTimeField;
            set
            {
                this.dEditEndTimeField = value;
                dEditEndTimeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dEditEndTimeSpecified
        {
            get => this.dEditEndTimeFieldSpecified;
            set => this.dEditEndTimeFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dXParticleDel
        {
            get => this.dXParticleDelField;
            set
            {
                this.dXParticleDelField = value;
                dXParticleDelSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dXParticleDelSpecified
        {
            get => dXParticleDelFieldSpecified;
            set => dXParticleDelFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dYParticleDel
        {
            get => this.dYParticleDelField;
            set
            {
                this.dYParticleDelField = value;
                dYParticleDelSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dYParticleDelSpecified
        {
            get => dYParticleDelFieldSpecified;
            set => dYParticleDelFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bFillBlurToBlankWhenKeepRatio
        {
            get => this.bFillBlurToBlankWhenKeepRatioField;
            set
            {
                this.bFillBlurToBlankWhenKeepRatioField = value;
                bFillBlurToBlankWhenKeepRatioSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bFillBlurToBlankWhenKeepRatioSpecified
        {
            get => bFillBlurToBlankWhenKeepRatioFieldSpecified;
            set => bFillBlurToBlankWhenKeepRatioFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bParticleEmissionDensityControlledByAudio
        {
            get => this.bParticleEmissionDensityControlledByAudioField;
            set
            {
                this.bParticleEmissionDensityControlledByAudioField = value;
                bParticleEmissionDensityControlledByAudioSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bParticleEmissionDensityControlledByAudioSpecified
        {
            get => bParticleEmissionDensityControlledByAudioFieldSpecified;
            set => bParticleEmissionDensityControlledByAudioFieldSpecified = value;
        }

        [XmlAttribute]
        public byte RotateAny2DOverlaybEnable
        {
            get => this.rotateAny2DOverlaybEnableField;
            set
            {
                this.rotateAny2DOverlaybEnableField = value;
                RotateAny2DOverlaybEnableSpecified = true;
            }
        }

        [XmlIgnore]
        public bool RotateAny2DOverlaybEnableSpecified
        {
            get => this.rotateAny2DOverlaybEnableFieldSpecified;
            set => this.rotateAny2DOverlaybEnableFieldSpecified = value;
        }

        [XmlAttribute]
        public byte RotateAny2DOverlaybFlip
        {
            get => this.rotateAny2DOverlaybFlipField;
            set
            {
                this.rotateAny2DOverlaybFlipField = value;
                RotateAny2DOverlaybFlipSpecified = true;
            }
        }

        [XmlIgnore]
        public bool RotateAny2DOverlaybFlipSpecified
        {
            get => this.rotateAny2DOverlaybFlipFieldSpecified;
            set => this.rotateAny2DOverlaybFlipFieldSpecified = value;
        }

        [XmlAttribute]
        public byte RotateAny2DOverlaybReverse
        {
            get => this.rotateAny2DOverlaybReverseField;
            set
            {
                this.rotateAny2DOverlaybReverseField = value;
                RotateAny2DOverlaybReverseSpecified = true;
            }
        }

        [XmlIgnore]
        public bool RotateAny2DOverlaybReverseSpecified
        {
            get => this.rotateAny2DOverlaybReverseFieldSpecified;
            set => this.rotateAny2DOverlaybReverseFieldSpecified = value;
        }

        [XmlAttribute]
        public int RotateAny2DOverlaylRotationAngle
        {
            get => this.rotateAny2DOverlaylRotationAngleField;
            set
            {
                this.rotateAny2DOverlaylRotationAngleField = value;
                RotateAny2DOverlaylRotationAngleSpecified = true;
            }
        }

        [XmlIgnore]
        public bool RotateAny2DOverlaylRotationAngleSpecified
        {
            get => this.rotateAny2DOverlaylRotationAngleFieldSpecified;
            set => this.rotateAny2DOverlaylRotationAngleFieldSpecified = value;
        }

        [XmlAttribute]
        public int ChannelOption
        {
            get => this.channelOptionField;
            set
            {
                this.channelOptionField = value;
                ChannelOptionSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ChannelOptionSpecified
        {
            get => channelOptionFieldSpecified;
            set => channelOptionFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bUseStartKeyFrameFor3DGroup
        {
            get => this.bUseStartKeyFrameFor3DGroupField;
            set
            {
                this.bUseStartKeyFrameFor3DGroupField = value;
                bUseStartKeyFrameFor3DGroupSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bUseStartKeyFrameFor3DGroupSpecified
        {
            get => bUseStartKeyFrameFor3DGroupFieldSpecified;
            set => bUseStartKeyFrameFor3DGroupFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dStartTimeOfKeyFrameFor3DGroup
        {
            get => this.dStartTimeOfKeyFrameFor3DGroupField;
            set
            {
                this.dStartTimeOfKeyFrameFor3DGroupField = value;
                dStartTimeOfKeyFrameFor3DGroupSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dStartTimeOfKeyFrameFor3DGroupSpecified
        {
            get => dStartTimeOfKeyFrameFor3DGroupFieldSpecified;
            set => dStartTimeOfKeyFrameFor3DGroupFieldSpecified = value;
        }

        [XmlAttribute]
        public int nAudioPitch
        {
            get => this.nAudioPitchField;
            set
            {
                this.nAudioPitchField = value;
                nAudioPitchSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nAudioPitchSpecified
        {
            get => nAudioPitchFieldSpecified;
            set => nAudioPitchFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bVocalCut
        {
            get => this.bVocalCutField;
            set
            {
                this.bVocalCutField = value;
                bVocalCutSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bVocalCutSpecified
        {
            get => bVocalCutFieldSpecified;
            set => bVocalCutFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dAudioDelay
        {
            get => this.dAudioDelayField;
            set
            {
                this.dAudioDelayField = value;
                dAudioDelaySpecified = true;
            }
        }

        [XmlIgnore]
        public bool dAudioDelaySpecified
        {
            get => dAudioDelayFieldSpecified;
            set => dAudioDelayFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bEnableCamera
        {
            get => this.bEnableCameraField;
            set
            {
                this.bEnableCameraField = value;
                bEnableCameraSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bEnableCameraSpecified
        {
            get => bEnableCameraFieldSpecified;
            set => bEnableCameraFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bReversePlayback
        {
            get => this.bReversePlaybackField;
            set
            {
                this.bReversePlaybackField = value;
                bReversePlaybackSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bReversePlaybackSpecified
        {
            get => bReversePlaybackFieldSpecified;
            set => bReversePlaybackFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bEmptyFrame
        {
            get => this.bEmptyFrameField;
            set
            {
                this.bEmptyFrameField = value;
                bEmptyFrameSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bEmptyFrameSpecified
        {
            get => bEmptyFrameFieldSpecified;
            set => bEmptyFrameFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bUseTextTemplate
        {
            get => this.bUseTextTemplateField;
            set
            {
                this.bUseTextTemplateField = value;
                bUseTextTemplateSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bUseTextTemplateSpecified
        {
            get => bUseTextTemplateFieldSpecified;
            set => bUseTextTemplateFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dTypographyTransparence
        {
            get => this.dTypographyTransparenceField;
            set
            {
                this.dTypographyTransparenceField = value;
                dTypographyTransparenceSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dTypographyTransparenceSpecified
        {
            get => dTypographyTransparenceFieldSpecified;
            set => dTypographyTransparenceFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bEnableLineWrap
        {
            get => this.bEnableLineWrapField;
            set
            {
                this.bEnableLineWrapField = value;
                bEnableLineWrapSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bEnableLineWrapSpecified
        {
            get => bEnableLineWrapFieldSpecified;
            set => bEnableLineWrapFieldSpecified = value;
        }

        [XmlAttribute]
        public int nMaxCharsPerLineForLineWrap
        {
            get => this.nMaxCharsPerLineForLineWrapField;
            set
            {
                this.nMaxCharsPerLineForLineWrapField = value;
                nMaxCharsPerLineForLineWrapSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nMaxCharsPerLineForLineWrapSpecified
        {
            get => nMaxCharsPerLineForLineWrapFieldSpecified;
            set => nMaxCharsPerLineForLineWrapFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dxAngle
        {
            get => this.dxAngleField;
            set
            {
                this.dxAngleField = value;
                dxAngleSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dxAngleSpecified
        {
            get => this.dxAngleFieldSpecified;
            set => this.dxAngleFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dxScale
        {
            get => this.dxScaleField;
            set
            {
                this.dxScaleField = value;
                dxScaleSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dxScaleSpecified
        {
            get => this.dxScaleFieldSpecified;
            set => this.dxScaleFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dyAngle
        {
            get => this.dyAngleField;
            set
            {
                this.dyAngleField = value;
                dyAngleSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dyAngleSpecified
        {
            get => this.dyAngleFieldSpecified;
            set => this.dyAngleFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dyScale
        {
            get => this.dyScaleField;
            set
            {
                this.dyScaleField = value;
                dyScaleSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dyScaleSpecified
        {
            get => this.dyScaleFieldSpecified;
            set => this.dyScaleFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dzAngle
        {
            get => this.dzAngleField;
            set
            {
                this.dzAngleField = value;
                dzAngleSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dzAngleSpecified
        {
            get => this.dzAngleFieldSpecified;
            set => this.dzAngleFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dzScale
        {
            get => this.dzScaleField;
            set
            {
                this.dzScaleField = value;
                dzScaleSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dzScaleSpecified
        {
            get => this.dzScaleFieldSpecified;
            set => this.dzScaleFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal n3Depth
        {
            get => this.n3DepthField;
            set
            {
                this.n3DepthField = value;
                n3DepthSpecified = true;
            }
        }

        [XmlIgnore]
        public bool n3DepthSpecified
        {
            get => this.n3DepthFieldSpecified;
            set => this.n3DepthFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dzDelPos
        {
            get => this.dzDelPosField;
            set
            {
                this.dzDelPosField = value;
                dzDelPosSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dzDelPosSpecified
        {
            get => this.dzDelPosFieldSpecified;
            set => this.dzDelPosFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bSubtitleFile
        {
            get => this.bSubtitleFileField;
            set
            {
                this.bSubtitleFileField = value;
                bSubtitleFileSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bSubtitleFileSpecified
        {
            get => bSubtitleFileFieldSpecified;
            set => bSubtitleFileFieldSpecified = value;
        }

        [XmlAttribute]
        public int nSubtitleChapterIndex
        {
            get => this.nSubtitleChapterIndexField;
            set
            {
                this.nSubtitleChapterIndexField = value;
                nSubtitleChapterIndexSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nSubtitleChapterIndexSpecified
        {
            get => nSubtitleChapterIndexFieldSpecified;
            set => nSubtitleChapterIndexFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bEnableAudioEffect
        {
            get => this.bEnableAudioEffectField;
            set
            {
                this.bEnableAudioEffectField = value;
                bEnableAudioEffectSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bEnableAudioEffectSpecified
        {
            get => this.bEnableAudioEffectFieldSpecified;
            set => this.bEnableAudioEffectFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dAudioFadeInDuration
        {
            get => this.dAudioFadeInDurationField;
            set
            {
                this.dAudioFadeInDurationField = value;
                dAudioFadeInDurationSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dAudioFadeInDurationSpecified
        {
            get => this.dAudioFadeInDurationFieldSpecified;
            set => this.dAudioFadeInDurationFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dAudioFadeOutDuration
        {
            get => this.dAudioFadeOutDurationField;
            set
            {
                this.dAudioFadeOutDurationField = value;
                dAudioFadeOutDurationSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dAudioFadeOutDurationSpecified
        {
            get => this.dAudioFadeOutDurationFieldSpecified;
            set => this.dAudioFadeOutDurationFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bDisableAudio
        {
            get => this.bDisableAudioField;
            set
            {
                this.bDisableAudioField = value;
                bDisableAudioSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bDisableAudioSpecified
        {
            get => bDisableAudioFieldSpecified;
            set => bDisableAudioFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bHadAudio
        {
            get => this.bHadAudioField;
            set
            {
                this.bHadAudioField = value;
                bHadAudioSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bHadAudioSpecified
        {
            get => bHadAudioFieldSpecified;
            set => bHadAudioFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dASelfDuration
        {
            get => this.dASelfDurationField;
            set
            {
                this.dASelfDurationField = value;
                dASelfDurationSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dASelfDurationSpecified
        {
            get => dASelfDurationFieldSpecified;
            set => dASelfDurationFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dASelfEndTime
        {
            get => this.dASelfEndTimeField;
            set
            {
                this.dASelfEndTimeField = value;
                dASelfEndTimeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dASelfEndTimeSpecified
        {
            get => dASelfEndTimeFieldSpecified;
            set => dASelfEndTimeFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dASelfStartTime
        {
            get => this.dASelfStartTimeField;
            set
            {
                this.dASelfStartTimeField = value;
                dASelfStartTimeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dASelfStartTimeSpecified
        {
            get => dASelfStartTimeFieldSpecified;
            set => dASelfStartTimeFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dEditorEndTime
        {
            get => this.dEditorEndTimeField;
            set
            {
                this.dEditorEndTimeField = value;
                dEditorEndTimeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dEditorEndTimeSpecified
        {
            get => dEditorEndTimeFieldSpecified;
            set => dEditorEndTimeFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dEditorStartTime
        {
            get => this.dEditorStartTimeField;
            set
            {
                this.dEditorStartTimeField = value;
                dEditorStartTimeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dEditorStartTimeSpecified
        {
            get => dEditorStartTimeFieldSpecified;
            set => dEditorStartTimeFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dVSelfDuration
        {
            get => this.dVSelfDurationField;
            set
            {
                this.dVSelfDurationField = value;
                dVSelfDurationSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dVSelfDurationSpecified
        {
            get => dVSelfDurationFieldSpecified;
            set => dVSelfDurationFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dVSelfEndTime
        {
            get => this.dVSelfEndTimeField;
            set
            {
                this.dVSelfEndTimeField = value;
                dVSelfEndTimeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dVSelfEndTimeSpecified
        {
            get => dVSelfEndTimeFieldSpecified;
            set => dVSelfEndTimeFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dVSelfStartTime
        {
            get => this.dVSelfStartTimeField;
            set
            {
                this.dVSelfStartTimeField = value;
                dVSelfStartTimeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dVSelfStartTimeSpecified
        {
            get => dVSelfStartTimeFieldSpecified;
            set => dVSelfStartTimeFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dTransitionDuration
        {
            get => this.dTransitionDurationField;
            set
            {
                this.dTransitionDurationField = value;
                dTransitionDurationSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dTransitionDurationSpecified
        {
            get => dTransitionDurationFieldSpecified;
            set => dTransitionDurationFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dPlaybackRate
        {
            get => this.dPlaybackRateField;
            set
            {
                this.dPlaybackRateField = value;
                dPlaybackRateSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dPlaybackRateSpecified
        {
            get => dPlaybackRateFieldSpecified;
            set => dPlaybackRateFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dVoiceScale
        {
            get => this.dVoiceScaleField;
            set
            {
                this.dVoiceScaleField = value;
                dVoiceScaleSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dVoiceScaleSpecified
        {
            get => dVoiceScaleFieldSpecified;
            set => dVoiceScaleFieldSpecified = value;
        }

        [XmlAttribute]
        public int eImageFillMode
        {
            get => this.eImageFillModeField;
            set
            {
                this.eImageFillModeField = value;
                eImageFillModeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool eImageFillModeSpecified
        {
            get => eImageFillModeFieldSpecified;
            set => eImageFillModeFieldSpecified = value;
        }

        [XmlAttribute]
        public int eMediaType
        {
            get => this.eMediaTypeField;
            set
            {
                this.eMediaTypeField = value;
                eMediaTypeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool eMediaTypeSpecified
        {
            get => eMediaTypeFieldSpecified;
            set => eMediaTypeFieldSpecified = value;
        }

        [XmlAttribute]
        public int eMediaTypeLine
        {
            get => this.eMediaTypeLineField;
            set
            {
                this.eMediaTypeLineField = value;
                eMediaTypeLineSpecified = true;
            }
        }

        [XmlIgnore]
        public bool eMediaTypeLineSpecified
        {
            get => eMediaTypeLineFieldSpecified;
            set => eMediaTypeLineFieldSpecified = value;
        }

        [XmlAttribute]
        public int nFileNameWidth
        {
            get => this.nFileNameWidthField;
            set
            {
                this.nFileNameWidthField = value;
                nFileNameWidthSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nFileNameWidthSpecified
        {
            get => nFileNameWidthFieldSpecified;
            set => nFileNameWidthFieldSpecified = value;
        }

        [XmlAttribute]
        public int nIndex
        {
            get => this.nIndexField;
            set
            {
                this.nIndexField = value;
                nIndexFieldSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nIndexSpecified
        {
            get => nIndexFieldSpecified;
            set => nIndexFieldSpecified = value;
        }

        [XmlAttribute]
        public int nIndexVEffect
        {
            get => this.nIndexVEffectField;
            set
            {
                this.nIndexVEffectField = value;
                nIndexVEffectSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nIndexVEffectSpecified
        {
            get => nIndexVEffectFieldSpecified;
            set => nIndexVEffectFieldSpecified = value;
        }


        [XmlAttribute]
        public int nIndexVTranEffect
        {
            get => this.nIndexVTranEffectField;
            set
            {
                this.nIndexVTranEffectField = value;
                nIndexVTranEffectSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nIndexVTranEffectSpecified
        {
            get => nIndexVTranEffectFieldSpecified;
            set => nIndexVTranEffectFieldSpecified = value;
        }

        [XmlAttribute]
        public int nVideoWidth
        {
            get => this.nVideoWidthField;
            set
            {
                this.nVideoWidthField = value;
                nVideoWidthSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nVideoWidthSpecified
        {
            get => nVideoWidthFieldSpecified;
            set => nVideoWidthFieldSpecified = value;
        }

        [XmlAttribute]
        public int nVideoHeight
        {
            get => this.nVideoHeightField;
            set
            {
                this.nVideoHeightField = value;
                nVideoHeightSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nVideoHeightSpecified
        {
            get => nVideoHeightFieldSpecified;
            set => nVideoHeightFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dwselptrinmainapp
        {
            get => this.dwselptrinmainappField;
            set
            {
                this.dwselptrinmainappField = value;
                dwselptrinmainappSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dwselptrinmainappSpecified
        {
            get => dwselptrinmainappFieldSpecified;
            set => dwselptrinmainappFieldSpecified = value;
        }

        [XmlAttribute]
        public int nCountCaptureImageForEffect_End
        {
            get => this.nCountCaptureImageForEffect_EndField;
            set
            {
                this.nCountCaptureImageForEffect_EndField = value;
                nCountCaptureImageForEffect_EndSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nCountCaptureImageForEffect_EndSpecified
        {
            get => nCountCaptureImageForEffect_EndFieldSpecified;
            set => nCountCaptureImageForEffect_EndFieldSpecified = value;
        }

        [XmlAttribute]
        public int nIndexVTranEffect3D
        {
            get => this.nIndexVTranEffect3DField;
            set
            {
                this.nIndexVTranEffect3DField = value;
                nIndexVTranEffect3DSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nIndexVTranEffect3DSpecified
        {
            get => nIndexVTranEffect3DFieldSpecified;
            set => nIndexVTranEffect3DFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal OverlaydTransparent
        {
            get => this.overlaydTransparentField;
            set
            {
                this.overlaydTransparentField = value;
                OverlaydTransparentSpecified = true;
            }
        }

        [XmlIgnore]
        public bool OverlaydTransparentSpecified
        {
            get => this.overlaydTransparentFieldSpecified;
            set => this.overlaydTransparentFieldSpecified = value;
        }

        [XmlAttribute]
        public int OverlaynHeight
        {
            get => this.overlaynHeightField;
            set
            {
                this.overlaynHeightField = value;
                OverlaynHeightSpecified = true;
            }
        }

        [XmlIgnore]
        public bool OverlaynHeightSpecified
        {
            get => this.overlaynHeightFieldSpecified;
            set => this.overlaynHeightFieldSpecified = value;
        }

        [XmlAttribute]
        public int OverlaynWidth
        {
            get => this.overlaynWidthField;
            set
            {
                this.overlaynWidthField = value;
                OverlaynWidthSpecified = true;
            }
        }

        [XmlIgnore]
        public bool OverlaynWidthSpecified
        {
            get => this.overlaynWidthFieldSpecified;
            set => this.overlaynWidthFieldSpecified = value;
        }

        [XmlAttribute]
        public int OverlayX
        {
            get => this.overlayXField;
            set
            {
                this.overlayXField = value;
                OverlayXSpecified = true;
            }
        }

        [XmlIgnore]
        public bool OverlayXSpecified
        {
            get => this.overlayXFieldSpecified;
            set => this.overlayXFieldSpecified = value;
        }

        [XmlAttribute]
        public int OverlayY
        {
            get => this.overlayYField;
            set
            {
                this.overlayYField = value;
                OverlayYSpecified = true;
            }
        }

        [XmlIgnore]
        public bool OverlayYSpecified
        {
            get => this.overlayYFieldSpecified;
            set => this.overlayYFieldSpecified = value;
        }

        [XmlAttribute]
        public int actionToMediaIndex
        {
            get => this.actionToMediaIndexField;
            set
            {
                this.actionToMediaIndexField = value;
                actionToMediaIndexSpecified = true;
            }
        }

        [XmlIgnore]
        public bool actionToMediaIndexSpecified
        {
            get => this.actionToMediaIndexFieldSpecified;
            set => this.actionToMediaIndexFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal ActiondTransparentEnd
        {
            get => this.actiondTransparentEndField;
            set
            {
                this.actiondTransparentEndField = value;
                ActiondTransparentEndSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ActiondTransparentEndSpecified
        {
            get => this.actiondTransparentEndFieldSpecified;
            set => this.actiondTransparentEndFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal ActiondTransparentStart
        {
            get => this.actiondTransparentStartField;
            set
            {
                this.actiondTransparentStartField = value;
                ActiondTransparentStartSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ActiondTransparentStartSpecified
        {
            get => this.actiondTransparentStartFieldSpecified;
            set => this.actiondTransparentStartFieldSpecified = value;
        }

        [XmlAttribute]
        public int ActionrcEnd_left
        {
            get => this.actionrcEnd_leftField;
            set
            {
                this.actionrcEnd_leftField = value;
                ActionrcEnd_leftSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ActionrcEnd_leftSpecified
        {
            get => this.actionrcEnd_leftFieldSpecified;
            set => this.actionrcEnd_leftFieldSpecified = value;
        }

        [XmlAttribute]
        public int ActionrcEnd_top
        {
            get => this.actionrcEnd_topField;
            set
            {
                this.actionrcEnd_topField = value;
                ActionrcEnd_topSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ActionrcEnd_topSpecified
        {
            get => this.actionrcEnd_topFieldSpecified;
            set => this.actionrcEnd_topFieldSpecified = value;
        }

        [XmlAttribute]
        public int ActionrcEnd_right
        {
            get => this.actionrcEnd_rightField;
            set
            {
                this.actionrcEnd_rightField = value;
                ActionrcEnd_rightSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ActionrcEnd_rightSpecified
        {
            get => this.actionrcEnd_rightFieldSpecified;
            set => this.actionrcEnd_rightFieldSpecified = value;
        }

        [XmlAttribute]
        public int ActionrcEnd_bottom
        {
            get => this.actionrcEnd_bottomField;
            set
            {
                this.actionrcEnd_bottomField = value;
                ActionrcEnd_bottomSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ActionrcEnd_bottomSpecified
        {
            get => this.actionrcEnd_bottomFieldSpecified;
            set => this.actionrcEnd_bottomFieldSpecified = value;
        }

        [XmlAttribute]
        public int ActionrcStart_left
        {
            get => this.actionrcStart_leftField;
            set
            {
                this.actionrcStart_leftField = value;
                ActionrcStart_leftSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ActionrcStart_leftSpecified
        {
            get => this.actionrcStart_leftFieldSpecified;
            set => this.actionrcStart_leftFieldSpecified = value;
        }

        [XmlAttribute]
        public int ActionrcStart_top
        {
            get => this.actionrcStart_topField;
            set
            {
                this.actionrcStart_topField = value;
                ActionrcStart_topSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ActionrcStart_topSpecified
        {
            get => this.actionrcStart_topFieldSpecified;
            set => this.actionrcStart_topFieldSpecified = value;
        }

        [XmlAttribute]
        public int ActionrcStart_right
        {
            get => this.actionrcStart_rightField;
            set
            {
                this.actionrcStart_rightField = value;
                ActionrcStart_rightSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ActionrcStart_rightSpecified
        {
            get => this.actionrcStart_rightFieldSpecified;
            set => this.actionrcStart_rightFieldSpecified = value;
        }

        [XmlAttribute]
        public int ActionrcStart_bottom
        {
            get => this.actionrcStart_bottomField;
            set
            {
                this.actionrcStart_bottomField = value;
                ActionrcStart_bottomSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ActionrcStart_bottomSpecified
        {
            get => this.actionrcStart_bottomFieldSpecified;
            set => this.actionrcStart_bottomFieldSpecified = value;
        }

        [XmlAttribute]
        public int ActionulfHeightEnd
        {
            get => this.actionulfHeightEndField;
            set
            {
                this.actionulfHeightEndField = value;
                ActionulfHeightEndSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ActionulfHeightEndSpecified
        {
            get => this.actionulfHeightEndFieldSpecified;
            set => this.actionulfHeightEndFieldSpecified = value;
        }

        [XmlAttribute]
        public int ActionulfHeightStart
        {
            get => this.actionulfHeightStartField;
            set
            {
                this.actionulfHeightStartField = value;
                ActionulfHeightStartSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ActionulfHeightStartSpecified
        {
            get => this.actionulfHeightStartFieldSpecified;
            set => this.actionulfHeightStartFieldSpecified = value;
        }

        [XmlAttribute]
        public int nAppearType
        {
            get => this.nAppearTypeField;
            set
            {
                this.nAppearTypeField = value;
                nAppearTypeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nAppearTypeSpecified
        {
            get => this.nAppearTypeFieldSpecified;
            set => this.nAppearTypeFieldSpecified = value;
        }

        [XmlAttribute]
        public int nDisappearType
        {
            get => this.nDisappearTypeField;
            set
            {
                this.nDisappearTypeField = value;
                nDisappearTypeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nDisappearTypeSpecified
        {
            get => this.nDisappearTypeFieldSpecified;
            set => this.nDisappearTypeFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal AppearDur
        {
            get => this.appearDurField;
            set
            {
                this.appearDurField = value;
                AppearDurSpecified = true;
            }
        }

        [XmlIgnore]
        public bool AppearDurSpecified
        {
            get => this.appearDurFieldSpecified;
            set => this.appearDurFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal DisappearDur
        {
            get => this.disappearDurField;
            set
            {
                this.disappearDurField = value;
                DisappearDurSpecified = true;
            }
        }

        [XmlIgnore]
        public bool DisappearDurSpecified
        {
            get => this.disappearDurFieldSpecified;
            set => this.disappearDurFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bFillInDuration
        {
            get => this.bFillInDurationField;
            set
            {
                this.bFillInDurationField = value;
                bFillInDurationSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bFillInDurationSpecified
        {
            get => this.bFillInDurationFieldSpecified;
            set => this.bFillInDurationFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bCustomizeAction
        {
            get => this.bCustomizeActionField;
            set
            {
                this.bCustomizeActionField = value;
                bCustomizeActionSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bCustomizeActionSpecified
        {
            get => this.bCustomizeActionFieldSpecified;
            set => this.bCustomizeActionFieldSpecified = value;
        }

        [XmlAttribute]
        public int nLoopCustomCount
        {
            get => this.nLoopCustomCountField;
            set
            {
                this.nLoopCustomCountField = value;
                nLoopCustomCountSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nLoopCustomCountSpecified
        {
            get => this.nLoopCustomCountFieldSpecified;
            set => this.nLoopCustomCountFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bDoneInBlock
        {
            get => this.bDoneInBlockField;
            set
            {
                this.bDoneInBlockField = value;
                bDoneInBlockSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bDoneInBlockSpecified
        {
            get => this.bDoneInBlockFieldSpecified;
            set => this.bDoneInBlockFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bKeepOriginalSurroundingOfBlock
        {
            get => this.bKeepOriginalSurroundingOfBlockField;
            set
            {
                this.bKeepOriginalSurroundingOfBlockField = value;
                bKeepOriginalSurroundingOfBlockSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bKeepOriginalSurroundingOfBlockSpecified
        {
            get => this.bKeepOriginalSurroundingOfBlockFieldSpecified;
            set => this.bKeepOriginalSurroundingOfBlockFieldSpecified = value;
        }

        [XmlAttribute]
        public int nClarityStart
        {
            get => this.nClarityStartField;
            set
            {
                this.nClarityStartField = value;
                nClarityStartSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nClarityStartSpecified
        {
            get => this.nClarityStartFieldSpecified;
            set => this.nClarityStartFieldSpecified = value;
        }

        [XmlAttribute]
        public int nClarityEnd
        {
            get => this.nClarityEndField;
            set
            {
                this.nClarityEndField = value;
                nClarityEndSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nClarityEndSpecified
        {
            get => this.nClarityEndFieldSpecified;
            set => this.nClarityEndFieldSpecified = value;
        }

        [XmlAttribute]
        public int ScreenAligneAlign
        {
            get => this.screenAligneAlignField;
            set
            {
                this.screenAligneAlignField = value;
                ScreenAligneAlignSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ScreenAligneAlignSpecified
        {
            get => this.screenAligneAlignFieldSpecified;
            set => this.screenAligneAlignFieldSpecified = value;
        }

        [XmlAttribute]
        public int ScreenAlignnBottomMargin
        {
            get => this.screenAlignnBottomMarginField;
            set
            {
                this.screenAlignnBottomMarginField = value;
                ScreenAlignnBottomMarginSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ScreenAlignnBottomMarginSpecified
        {
            get => this.screenAlignnBottomMarginFieldSpecified;
            set => this.screenAlignnBottomMarginFieldSpecified = value;
        }

        [XmlAttribute]
        public int ScreenAlignnLeftMargin
        {
            get => this.screenAlignnLeftMarginField;
            set
            {
                this.screenAlignnLeftMarginField = value;
                ScreenAlignnLeftMarginSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ScreenAlignnLeftMarginSpecified
        {
            get => this.screenAlignnLeftMarginFieldSpecified;
            set => this.screenAlignnLeftMarginFieldSpecified = value;
        }

        [XmlAttribute]
        public int ScreenAlignnRightMargin
        {
            get => this.screenAlignnRightMarginField;
            set
            {
                this.screenAlignnRightMarginField = value;
                ScreenAlignnRightMarginSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ScreenAlignnRightMarginSpecified
        {
            get => this.screenAlignnRightMarginFieldSpecified;
            set => this.screenAlignnRightMarginFieldSpecified = value;
        }

        [XmlAttribute]
        public int ScreenAlignnTopMargin
        {
            get => this.screenAlignnTopMarginField;
            set
            {
                this.screenAlignnTopMarginField = value;
                ScreenAlignnTopMarginSpecified = true;
            }
        }

        [XmlIgnore]
        public bool ScreenAlignnTopMarginSpecified
        {
            get => this.screenAlignnTopMarginFieldSpecified;
            set => this.screenAlignnTopMarginFieldSpecified = value;
        }

        [XmlAttribute]
        public int nAlign
        {
            get => this.nAlignField;
            set
            {
                this.nAlignField = value;
                nAlignSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nAlignSpecified
        {
            get => this.nAlignFieldSpecified;
            set => this.nAlignFieldSpecified = value;
        }

        [XmlAttribute]
        public uint TitleclrText
        {
            get => this.titleclrTextField;
            set
            {
                this.titleclrTextField = value;
                TitleclrTextSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitleclrTextSpecified
        {
            get => this.titleclrTextFieldSpecified;
            set => this.titleclrTextFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitledTranparent
        {
            get => this.titledTranparentField;
            set
            {
                this.titledTranparentField = value;
                TitledTranparentSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitledTranparentSpecified
        {
            get => this.titledTranparentFieldSpecified;
            set => this.titledTranparentFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitlelfCharSet
        {
            get => this.titlelfCharSetField;
            set
            {
                this.titlelfCharSetField = value;
                TitlelfCharSetSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlelfCharSetSpecified
        {
            get => this.titlelfCharSetFieldSpecified;
            set => this.titlelfCharSetFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitlelfClipPrecision
        {
            get => this.titlelfClipPrecisionField;
            set
            {
                this.titlelfClipPrecisionField = value;
                TitlelfClipPrecisionSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlelfClipPrecisionSpecified
        {
            get => this.titlelfClipPrecisionFieldSpecified;
            set => this.titlelfClipPrecisionFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitlelfEscapement
        {
            get => this.titlelfEscapementField;
            set
            {
                this.titlelfEscapementField = value;
                TitlelfEscapementSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlelfEscapementSpecified
        {
            get => this.titlelfEscapementFieldSpecified;
            set => this.titlelfEscapementFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitlelfHeight
        {
            get => this.titlelfHeightField;
            set
            {
                this.titlelfHeightField = value;
                TitlelfHeightSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlelfHeightSpecified
        {
            get => this.titlelfHeightFieldSpecified;
            set => this.titlelfHeightFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitlelfItalic
        {
            get => this.titlelfItalicField;
            set
            {
                this.titlelfItalicField = value;
                TitlelfItalicSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlelfItalicSpecified
        {
            get => this.titlelfItalicFieldSpecified;
            set => this.titlelfItalicFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitlelfOrientation
        {
            get => this.titlelfOrientationField;
            set
            {
                this.titlelfOrientationField = value;
                TitlelfOrientationSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlelfOrientationSpecified
        {
            get => this.titlelfOrientationFieldSpecified;
            set => this.titlelfOrientationFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitlelfOutPrecision
        {
            get => this.titlelfOutPrecisionField;
            set
            {
                this.titlelfOutPrecisionField = value;
                TitlelfOutPrecisionSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlelfOutPrecisionSpecified
        {
            get => this.titlelfOutPrecisionFieldSpecified;
            set => this.titlelfOutPrecisionFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitlelfPitchAndFamily
        {
            get => this.titlelfPitchAndFamilyField;
            set
            {
                this.titlelfPitchAndFamilyField = value;
                TitlelfPitchAndFamilySpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlelfPitchAndFamilySpecified
        {
            get => this.titlelfPitchAndFamilyFieldSpecified;
            set => this.titlelfPitchAndFamilyFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitlelfQuality
        {
            get => this.titlelfQualityField;
            set
            {
                this.titlelfQualityField = value;
                TitlelfQualitySpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlelfQualitySpecified
        {
            get => this.titlelfQualityFieldSpecified;
            set => this.titlelfQualityFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitlelfStrikeOut
        {
            get => this.titlelfStrikeOutField;
            set
            {
                this.titlelfStrikeOutField = value;
                TitlelfStrikeOutSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlelfStrikeOutSpecified
        {
            get => this.titlelfStrikeOutFieldSpecified;
            set => this.titlelfStrikeOutFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitlelfUnderline
        {
            get => this.titlelfUnderlineField;
            set
            {
                this.titlelfUnderlineField = value;
                TitlelfUnderlineSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlelfUnderlineSpecified
        {
            get => this.titlelfUnderlineFieldSpecified;
            set => this.titlelfUnderlineFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitlelfWeight
        {
            get => this.titlelfWeightField;
            set
            {
                this.titlelfWeightField = value;
                TitlelfWeightSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlelfWeightSpecified
        {
            get => this.titlelfWeightFieldSpecified;
            set => this.titlelfWeightFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitlelfWidth
        {
            get => this.titlelfWidthField;
            set
            {
                this.titlelfWidthField = value;
                TitlelfWidthSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlelfWidthSpecified
        {
            get => this.titlelfWidthFieldSpecified;
            set => this.titlelfWidthFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitleX
        {
            get => this.titleXField;
            set
            {
                this.titleXField = value;
                TitleXSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitleXSpecified
        {
            get => this.titleXFieldSpecified;
            set => this.titleXFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal TitlexposScale
        {
            get => this.titlexposScaleField;
            set
            {
                this.titlexposScaleField = value;
                TitlexposScaleSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitlexposScaleSpecified
        {
            get => this.titlexposScaleFieldSpecified;
            set => this.titlexposScaleFieldSpecified = value;
        }

        [XmlAttribute]
        public int TitleY
        {
            get => this.titleYField;
            set
            {
                this.titleYField = value;
                TitleYSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitleYSpecified
        {
            get => this.titleYFieldSpecified;
            set => this.titleYFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal TitleyposScale
        {
            get => this.titleyposScaleField;
            set
            {
                this.titleyposScaleField = value;
                TitleyposScaleSpecified = true;
            }
        }

        [XmlIgnore]
        public bool TitleyposScaleSpecified
        {
            get => this.titleyposScaleFieldSpecified;
            set => this.titleyposScaleFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bBold
        {
            get => this.bBoldField;
            set
            {
                this.bBoldField = value;
                bBoldSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bBoldSpecified
        {
            get => this.bBoldFieldSpecified;
            set => this.bBoldFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bVertical
        {
            get => this.bVerticalField;
            set
            {
                this.bVerticalField = value;
                bVerticalSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bVerticalSpecified
        {
            get => this.bVerticalFieldSpecified;
            set => this.bVerticalFieldSpecified = value;
        }

        [XmlAttribute]
        public uint clrFont
        {
            get => this.clrFontField;
            set
            {
                this.clrFontField = value;
                clrFontSpecified = true;
            }
        }

        [XmlIgnore]
        public bool clrFontSpecified
        {
            get => this.clrFontFieldSpecified;
            set => this.clrFontFieldSpecified = value;
        }

        [XmlAttribute]
        public uint clrOutline
        {
            get => this.clrOutlineField;
            set
            {
                this.clrOutlineField = value;
                clrOutlineSpecified = true;
            }
        }

        [XmlIgnore]
        public bool clrOutlineSpecified
        {
            get => this.clrOutlineFieldSpecified;
            set => this.clrOutlineFieldSpecified = value;
        }

        [XmlAttribute]
        public uint clrShade
        {
            get => this.clrShadeField;
            set
            {
                this.clrShadeField = value;
                clrShadeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool clrShadeSpecified
        {
            get => this.clrShadeFieldSpecified;
            set => this.clrShadeFieldSpecified = value;
        }

        [XmlAttribute]
        public uint clrShining
        {
            get => this.clrShiningField;
            set
            {
                this.clrShiningField = value;
                clrShiningSpecified = true;
            }
        }

        [XmlIgnore]
        public bool clrShiningSpecified
        {
            get => this.clrShiningFieldSpecified;
            set => this.clrShiningFieldSpecified = value;
        }

        [XmlAttribute]
        public int efe
        {
            get => this.efeField;
            set
            {
                this.efeField = value;
                efeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool efeSpecified
        {
            get => this.efeFieldSpecified;
            set => this.efeFieldSpecified = value;
        }

        [XmlAttribute]
        public int nOutlineFactor
        {
            get => this.nOutlineFactorField;
            set
            {
                this.nOutlineFactorField = value;
                nOutlineFactorSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nOutlineFactorSpecified
        {
            get => this.nOutlineFactorFieldSpecified;
            set => this.nOutlineFactorFieldSpecified = value;
        }

        [XmlAttribute]
        public int nShadeFactor
        {
            get => this.nShadeFactorField;
            set
            {
                this.nShadeFactorField = value;
                nShadeFactorSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nShadeFactorSpecified
        {
            get => this.nShadeFactorFieldSpecified;
            set => this.nShadeFactorFieldSpecified = value;
        }

        [XmlAttribute]
        public int nShineFactor
        {
            get => this.nShineFactorField;
            set
            {
                this.nShineFactorField = value;
                nShineFactorSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nShineFactorSpecified
        {
            get => this.nShineFactorFieldSpecified;
            set => this.nShineFactorFieldSpecified = value;
        }

        [XmlAttribute]
        public int nRotateAngle
        {
            get => this.nRotateAngleField;
            set
            {
                this.nRotateAngleField = value;
                nRotateAngleSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nRotateAngleSpecified
        {
            get => this.nRotateAngleFieldSpecified;
            set => this.nRotateAngleFieldSpecified = value;
        }

        [XmlAttribute]
        public uint clrGradient
        {
            get => this.clrGradientField;
            set
            {
                this.clrGradientField = value;
                clrGradientSpecified = true;
            }
        }

        [XmlIgnore]
        public bool clrGradientSpecified
        {
            get => this.clrGradientFieldSpecified;
            set => this.clrGradientFieldSpecified = value;
        }

        [XmlAttribute]
        public uint clrHatch
        {
            get => this.clrHatchField;
            set
            {
                this.clrHatchField = value;
                clrHatchSpecified = true;
            }
        }

        [XmlIgnore]
        public bool clrHatchSpecified
        {
            get => this.clrHatchFieldSpecified;
            set => this.clrHatchFieldSpecified = value;
        }

        [XmlAttribute]
        public int eDataType
        {
            get => this.eDataTypeField;
            set
            {
                this.eDataTypeField = value;
                eDataTypeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool eDataTypeSpecified
        {
            get => this.eDataTypeFieldSpecified;
            set => this.eDataTypeFieldSpecified = value;
        }

        [XmlAttribute]
        public int eGradientType
        {
            get => this.eGradientTypeField;
            set
            {
                this.eGradientTypeField = value;
                eGradientTypeSpecified = true;
            }
        }

        [XmlIgnore]
        public bool eGradientTypeSpecified
        {
            get => this.eGradientTypeFieldSpecified;
            set => this.eGradientTypeFieldSpecified = value;
        }

        [XmlAttribute]
        public int nHatchIndex
        {
            get => this.nHatchIndexField;
            set
            {
                this.nHatchIndexField = value;
                nHatchIndexSpecified = true;
            }
        }

        [XmlIgnore]
        public bool nHatchIndexSpecified
        {
            get => this.nHatchIndexFieldSpecified;
            set => this.nHatchIndexFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bEnableProjection
        {
            get => this.bEnableProjectionField;
            set
            {
                this.bEnableProjectionField = value;
                bEnableProjectionSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bEnableProjectionSpecified
        {
            get => this.bEnableProjectionFieldSpecified;
            set => this.bEnableProjectionFieldSpecified = value;
        }

        [XmlAttribute]
        public int xDelProjection
        {
            get => this.xDelProjectionField;
            set
            {
                this.xDelProjectionField = value;
                xDelProjectionSpecified = true;
            }
        }

        [XmlIgnore]
        public bool xDelProjectionSpecified
        {
            get => this.xDelProjectionFieldSpecified;
            set => this.xDelProjectionFieldSpecified = value;
        }

        [XmlAttribute]
        public int yDelProjection
        {
            get => this.yDelProjectionField;
            set
            {
                this.yDelProjectionField = value;
                yDelProjectionSpecified = true;
            }
        }

        [XmlIgnore]
        public bool yDelProjectionSpecified
        {
            get => this.yDelProjectionFieldSpecified;
            set => this.yDelProjectionFieldSpecified = value;
        }

        [XmlAttribute]
        public decimal dTransProjection
        {
            get => this.dTransProjectionField;
            set
            {
                this.dTransProjectionField = value;
                dTransProjectionSpecified = true;
            }
        }

        [XmlIgnore]
        public bool dTransProjectionSpecified
        {
            get => this.dTransProjectionFieldSpecified;
            set => this.dTransProjectionFieldSpecified = value;
        }

        [XmlAttribute]
        public byte bUseInsertColors
        {
            get => this.bUseInsertColorsField;
            set
            {
                this.bUseInsertColorsField = value;
                bUseInsertColorsSpecified = true;
            }
        }

        [XmlIgnore]
        public bool bUseInsertColorsSpecified
        {
            get => this.bUseInsertColorsFieldSpecified;
            set => this.bUseInsertColorsFieldSpecified = value;
        }

        [XmlAttribute]
        public uint ics00
        {
            get => this.ics00Field;
            set
            {
                this.ics00Field = value;
                ics00Specified = true;
            }
        }

        [XmlIgnore]
        public bool ics00Specified
        {
            get => this.ics00FieldSpecified;
            set => this.ics00FieldSpecified = value;
        }

        [XmlAttribute]
        public uint ics01
        {
            get => this.ics01Field;
            set
            {
                this.ics01Field = value;
                ics01Specified = true;
            }
        }

        [XmlIgnore]
        public bool ics01Specified
        {
            get => this.ics01FieldSpecified;
            set => this.ics01FieldSpecified = value;
        }

        [XmlAttribute]
        public int ics02
        {
            get => this.ics02Field;
            set
            {
                this.ics02Field = value;
                ics02Specified = true;
            }
        }

        [XmlIgnore]
        public bool ics02Specified
        {
            get => this.ics02FieldSpecified;
            set => this.ics02FieldSpecified = value;
        }

        [XmlAttribute]
        public int ics03
        {
            get => this.ics03Field;
            set
            {
                this.ics03Field = value;
                ics03Specified = true;
            }
        }

        [XmlIgnore]
        public bool ics03Specified
        {
            get => this.ics03FieldSpecified;
            set => this.ics03FieldSpecified = value;
        }

        [XmlAttribute]
        public int ics04
        {
            get => this.ics04Field;
            set
            {
                this.ics04Field = value;
                ics04Specified = true;
            }
        }

        [XmlIgnore]
        public bool ics04Specified
        {
            get => this.ics04FieldSpecified;
            set => this.ics04FieldSpecified = value;
        }

        [XmlAttribute]
        public int ics05
        {
            get => this.ics05Field;
            set
            {
                this.ics05Field = value;
                ics05Specified = true;
            }
        }

        [XmlIgnore]
        public bool ics05Specified
        {
            get => this.ics05FieldSpecified;
            set => this.ics05FieldSpecified = value;
        }

        [XmlAttribute]
        public int ics06
        {
            get => this.ics06Field;
            set
            {
                this.ics06Field = value;
                ics06Specified = true;
            }
        }

        [XmlIgnore]
        public bool ics06Specified
        {
            get => this.ics06FieldSpecified;
            set => this.ics06FieldSpecified = value;
        }

        [XmlAttribute]
        public int ics07
        {
            get => this.ics07Field;
            set
            {
                this.ics07Field = value;
                ics07Specified = true;
            }
        }

        [XmlIgnore]
        public bool ics07Specified
        {
            get => this.ics07FieldSpecified;
            set => this.ics07FieldSpecified = value;
        }

        [XmlAttribute]
        public int ics08
        {
            get => this.ics08Field;
            set
            {
                this.ics08Field = value;
                ics08Specified = true;
            }
        }

        [XmlIgnore]
        public bool ics08Specified
        {
            get => this.ics08FieldSpecified;
            set => this.ics08FieldSpecified = value;
        }

        [XmlAttribute]
        public int ics09
        {
            get => this.ics09Field;
            set
            {
                this.ics09Field = value;
                ics09Specified = true;
            }
        }

        [XmlIgnore]
        public bool ics09Specified
        {
            get => this.ics09FieldSpecified;
            set => this.ics09FieldSpecified = value;
        }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public partial class GraffitiLayer
    {
        public GraffitiObj? obj { get; set; }

        [XmlAttribute]
        public byte bVisible { get; set; }

        [XmlAttribute]
        public decimal dDelayTime { get; set; }

        [XmlAttribute]
        public decimal dDrawTime { get; set; }

        [XmlAttribute]
        public string? lpszName { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public partial class GraffitiObj
    {
        [XmlElement("point")]
        public List<GraffitiPoint> Points { get; set; }

        [XmlAttribute]
        public byte bEraser { get; set; }

        [XmlAttribute]
        public uint clrPen { get; set; }

        [XmlAttribute]
        public decimal dTransparent { get; set; }

        [XmlAttribute]
        public int eType { get; set; }

        [XmlAttribute]
        public int nAngle { get; set; }

        [XmlAttribute]
        public int nEraserRadius { get; set; }

        [XmlAttribute]
        public int nPenWidth { get; set; }

        [XmlAttribute]
        public int nShapeType { get; set; }

        [XmlAttribute]
        public uint clrOutline { get; set; }

        [XmlAttribute]
        public int nOutlineWidth { get; set; }

        [XmlAttribute]
        public int nOutlineType { get; set; }

        [XmlAttribute]
        public byte bSmooth { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public partial class GraffitiPoint
    {
        [XmlAttribute]
        public int X { get; set; }

        [XmlAttribute]
        public int Y { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public partial class CustomMotion
    {
        public string? name { get; set; }

        [XmlElement("keypoint")]
        public CustomMotionKeypoint[]? keypoint { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public partial class CustomMotionKeypoint
    {
        [XmlAttribute]
        public decimal dDurationPercent { get; set; }

        [XmlAttribute]
        public int X { get; set; }

        [XmlAttribute]
        public int Y { get; set; }

        [XmlAttribute]
        public int CX { get; set; }

        [XmlAttribute]
        public int CY { get; set; }

        [XmlAttribute]
        public decimal nSizePercent { get; set; }

        [XmlAttribute]
        public decimal nTransPercent { get; set; }

        [XmlAttribute]
        public int nAngle { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public partial class VuMeter
    {
        [XmlAttribute]
        public decimal number1 { get; set; }

        [XmlAttribute]
        public decimal number2 { get; set; }

        [XmlAttribute]
        public decimal number3 { get; set; }

        [XmlAttribute]
        public decimal number4 { get; set; }

        [XmlAttribute]
        public decimal number5 { get; set; }

        [XmlAttribute]
        public decimal number6 { get; set; }

        [XmlAttribute]
        public decimal number7 { get; set; }

        [XmlAttribute]
        public decimal number8 { get; set; }

        [XmlAttribute]
        public decimal number9 { get; set; }

        [XmlAttribute]
        public decimal number10 { get; set; }

        [XmlAttribute]
        public decimal number11 { get; set; }

        [XmlAttribute]
        public decimal number12 { get; set; }

        [XmlAttribute]
        public decimal number13 { get; set; }

        [XmlAttribute]
        public decimal number14 { get; set; }

        [XmlAttribute]
        public decimal number15 { get; set; }

        [XmlAttribute]
        public decimal number16 { get; set; }

        [XmlAttribute]
        public decimal number17 { get; set; }

        [XmlAttribute]
        public decimal number18 { get; set; }

        [XmlAttribute]
        public decimal number19 { get; set; }

        [XmlAttribute]
        public decimal number20 { get; set; }

        [XmlAttribute]
        public decimal number21 { get; set; }

        [XmlAttribute]
        public decimal number22 { get; set; }

        [XmlAttribute]
        public decimal number23 { get; set; }

        [XmlAttribute]
        public decimal number24 { get; set; }

        [XmlAttribute]
        public decimal number25 { get; set; }

        [XmlAttribute]
        public decimal number26 { get; set; }

        [XmlAttribute]
        public decimal number27 { get; set; }

        [XmlAttribute]
        public decimal number28 { get; set; }

        [XmlAttribute]
        public decimal number29 { get; set; }

        [XmlAttribute]
        public decimal number30 { get; set; }

        [XmlAttribute]
        public decimal number31 { get; set; }

        [XmlAttribute]
        public decimal number32 { get; set; }

        [XmlAttribute]
        public decimal number33 { get; set; }

        [XmlAttribute]
        public decimal number34 { get; set; }

        [XmlAttribute]
        public decimal number35 { get; set; }

        [XmlAttribute]
        public decimal number36 { get; set; }

        [XmlAttribute]
        public decimal number37 { get; set; }

        [XmlAttribute]
        public decimal number38 { get; set; }

        [XmlAttribute]
        public decimal number39 { get; set; }

        [XmlAttribute]
        public decimal number40 { get; set; }

        [XmlAttribute]
        public decimal number41 { get; set; }

        [XmlAttribute]
        public decimal number42 { get; set; }

        [XmlAttribute]
        public decimal number43 { get; set; }

        [XmlAttribute]
        public decimal number44 { get; set; }

        [XmlAttribute]
        public decimal ics00 { get; set; }

        [XmlAttribute]
        public decimal ics01 { get; set; }

        [XmlAttribute]
        public decimal ics02 { get; set; }

        [XmlAttribute]
        public decimal ics03 { get; set; }

        [XmlAttribute]
        public decimal ics04 { get; set; }

        [XmlAttribute]
        public decimal ics05 { get; set; }

        [XmlAttribute]
        public decimal ics06 { get; set; }

        [XmlAttribute]
        public decimal ics07 { get; set; }

        [XmlAttribute]
        public decimal ics08 { get; set; }

        [XmlAttribute]
        public decimal ics09 { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public partial class LineItemColorblk
    {
        [XmlAttribute]
        public uint clr { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public partial class ThreeDGroupSettings
    {
        [XmlAttribute]
        public decimal dDepth { get; set; }

        [XmlAttribute]
        public int ax { get; set; }

        [XmlAttribute]
        public int ay { get; set; }

        [XmlAttribute]
        public int az { get; set; }

        [XmlAttribute]
        public int x { get; set; }

        [XmlAttribute]
        public int y { get; set; }

        [XmlAttribute]
        public int z { get; set; }

        [XmlAttribute]
        public int nDelHY { get; set; }

        [XmlAttribute]
        public int nDelWX { get; set; }

        [XmlAttribute]
        public int nHeight { get; set; }

        [XmlAttribute]
        public int nMotionReferencePos { get; set; }

        [XmlAttribute]
        public int nVisible { get; set; }

        [XmlAttribute]
        public int nWidth { get; set; }

        [XmlAttribute]
        public byte bBottom { get; set; }

        [XmlAttribute]
        public byte bEnable { get; set; }

        [XmlAttribute]
        public byte bLeft { get; set; }

        [XmlAttribute]
        public byte bRight { get; set; }

        [XmlAttribute]
        public byte bTop { get; set; }

        [XmlAttribute]
        public int nMaxTransparency { get; set; }

        [XmlAttribute]
        public int nVisiblePart { get; set; }
    }

}