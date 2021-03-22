using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    [XmlRoot]
    public class CartClearResponse : AmazonResponse
    {
        public Cart Cart { get; set; }
    }
}
