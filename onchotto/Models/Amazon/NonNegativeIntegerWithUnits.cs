using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    public class NonNegativeIntegerWithUnits
    {
        public string Units { get; set; }
        [XmlText(DataType = "nonNegativeInteger")]
        public string Value { get; set; }
    }
}
