using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    [XmlRoot]
    public class TopSeller
    {
        public string ASIN { get; set; }
        public string Title { get; set; }
    }
}
