using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    public class OperationRequest
    {
        public string RequestId { get; set; }
        public float RequestProcessingTime { get; set; }
        [XmlArrayItem("Header")]
        public HttpHeader[] HTTPHeaders { get; set; }
        [XmlArrayItem("Argument")]
        public Argument[] Arguments { get; set; }
        [XmlArrayItem("Error")]
        public AmazonError[] Errors { get; set; }
    }
}
