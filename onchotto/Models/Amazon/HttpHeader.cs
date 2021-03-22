using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    public class HttpHeader
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Value { get; set; }
    }
}
