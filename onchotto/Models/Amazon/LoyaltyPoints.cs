using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    public class LoyaltyPoints
    {
        [XmlElement(DataType = "nonNegativeInteger")]
        public string Points { get; set; }
        public Price TypicalRedemptionValue { get; set; }
    }
}
