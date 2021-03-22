using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    [XmlRoot]
    public class BrowseNodeLookupResponse : AmazonResponse
    {
        public BrowseNodes BrowseNodes { get; set; }
    }
}
