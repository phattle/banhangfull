using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    public class ItemAttributesCreator
    {
        [XmlAttribute]
        public string Role { get; set; }
        [XmlText]
        public string Name { get; set; }
    }
}
