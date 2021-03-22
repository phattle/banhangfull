using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    public class DecimalWithUnits
    {
        [XmlAttribute]
        public string Units { get; set; }
        [XmlText]
        public decimal Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Value, this.Units);
        }
    }
}
