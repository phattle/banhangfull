using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    public class VariationDimensions
    {
        [XmlElement("VariationDimension")]
        public string[] VariationDimension { get; set; }
    }
}
