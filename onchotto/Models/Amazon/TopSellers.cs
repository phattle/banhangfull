using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    [XmlRoot]
    public class TopSellers
    {
        [XmlElement("TopSeller")]
        public TopSeller[] TopSeller { get; set; }
    }
}
