using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    public class Argument
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Value { get; set; }
    }
}
