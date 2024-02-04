using System.Xml.Serialization;

namespace KaddaOK.Library.Ytmm
{
    public class FontBase
    {
        [XmlAttribute]
        public byte bBold { get; set; }

        [XmlAttribute]
        public byte bVertical { get; set; }

        [XmlAttribute]
        public uint clrFont { get; set; }

        [XmlAttribute]
        public uint clrOutline { get; set; }

        [XmlAttribute]
        public uint clrShade { get; set; }

        [XmlAttribute]
        public uint clrShining { get; set; }

        [XmlAttribute]
        public short efe { get; set; }

        [XmlAttribute]
        public short nOutlineFactor { get; set; }

        [XmlAttribute]
        public short nShadeFactor { get; set; }

        [XmlAttribute]
        public short nShineFactor { get; set; }

        [XmlAttribute]
        public short nRotateAngle { get; set; }

        [XmlAttribute]
        public uint clrGradient { get; set; }

        [XmlAttribute]
        public uint clrHatch { get; set; }

        [XmlAttribute]
        public byte eDataType { get; set; }

        [XmlAttribute]
        public byte eGradientType { get; set; }

        [XmlAttribute]
        public short nHatchIndex { get; set; }

        [XmlAttribute]
        public byte bEnableProjection { get; set; }

        [XmlAttribute]
        public short xDelProjection { get; set; }

        [XmlAttribute]
        public short yDelProjection { get; set; }

        [XmlAttribute]
        public decimal dTransProjection { get; set; }

        [XmlAttribute]
        public byte bUseInsertColors { get; set; }

        [XmlAttribute]
        public uint ics00 { get; set; }

        [XmlAttribute]
        public uint ics01 { get; set; }

        [XmlAttribute]
        public uint ics02 { get; set; }

        [XmlAttribute]
        public uint ics03 { get; set; }

        [XmlAttribute]
        public uint ics04 { get; set; }

        [XmlAttribute]
        public uint ics05 { get; set; }

        [XmlAttribute]
        public uint ics06 { get; set; }

        [XmlAttribute]
        public uint ics07 { get; set; }

        [XmlAttribute]
        public uint ics08 { get; set; }

        [XmlAttribute]
        public uint ics09 { get; set; }
    }
}
