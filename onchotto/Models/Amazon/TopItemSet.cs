using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    [XmlRoot]
    public class TopItemSet
    {
        public string Type { get; set; }
        [XmlElement("TopItem")]
        public TopItem[] TopItem { get; set; }
    }
}
