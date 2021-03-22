using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    [XmlRoot]
    public class TopItem
    {
        public string ASIN { get; set; }
        public string Title { get; set; }
        public string DetailPageURL { get; set; }
        public string ProductGroup { get; set; }
        [XmlElement("Actor")]
        public string[] Actor { get; set; }
    }
}
