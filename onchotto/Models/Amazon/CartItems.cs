using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    public class CartItems
    {
        public Price SubTotal { get; set; }
        [XmlElement("CartItem")]
        public CartItem[] CartItem { get; set; }
    }
}
