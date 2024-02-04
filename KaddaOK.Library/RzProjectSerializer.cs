using System.Text.RegularExpressions;
using System.Xml.Serialization;
using KaddaOK.Library.Ytmm;

namespace KaddaOK.Library
{
    public interface IRzProjectSerializer
    {
        string Serialize(RzProject project);
        RzProject? Deserialize(string xmlString);
    }
    public class RzProjectSerializer : IRzProjectSerializer
    {
        public string Serialize(RzProject project)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var ser = new XmlSerializer(typeof(
                RzProject));
            var writer = new StringWriter();
            ser.Serialize(writer, project);
            var result =
                Regex.Replace(
                    writer.ToString()
                        .Replace("encoding=\"utf-", "encoding=\"UTF-")
                        .Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", ""),
                    @" number([0-9]+)=""", @" $1=""");
            return result;
        }

        public RzProject? Deserialize(string xmlString)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var ser = new XmlSerializer(typeof(RzProject));

            var regex = new Regex(@" ([0-9]+)\=\""");
            var fixedText = regex.Replace(xmlString, @" number$1=""");

            var stringReader = new StringReader((fixedText));

            return ser.Deserialize(stringReader) as RzProject;
        }
    }
}
