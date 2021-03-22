using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    public class Request
    {
        public string IsValid { get; set; }
        [XmlArrayItem("Error")]
        public AmazonError[] Errors { get; set; }
    }
}
