using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    [XmlRoot]
    public class BrowseNodes
    {
        public BaseBrowseNodeLookupRequest Request { get; set; }
        public BrowseNode BrowseNode { get; set; }
    }
}
