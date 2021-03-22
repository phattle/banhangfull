using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    [XmlRoot]
    public class BrowseNodeLookupRequest
    {
        public string BrowseNodeId { get; set; }
        public string ResponseGroup { get; set; }
    }
}
