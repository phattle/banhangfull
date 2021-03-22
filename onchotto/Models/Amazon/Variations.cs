using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    public class Variations
    {
        public int TotalVariations { get; set; }
        public int TotalVariationPages { get; set; }
        public VariationDimensions VariationDimensions { get; set; }
        [XmlElement("Item")]
        public Item[] Item { get; set; }
    }
}
