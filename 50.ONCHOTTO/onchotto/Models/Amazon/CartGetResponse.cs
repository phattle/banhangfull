using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    [XmlRoot]
    public class CartGetResponse : AmazonResponse
    {
        public Cart Cart { get; set; }
    }
}