using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    public class RelatedItems
    {
        public string Relationship { get; set; }

        public string RelationshipType { get; set; }

        public byte RelatedItemCount { get; set; }

        public byte RelatedItemPageCount { get; set; }

        public byte RelatedItemPage { get; set; }

        [XmlElement("RelatedItem")]
        public RelatedItemsRelatedItem[] RelatedItem { get; set; }
    }

    public class RelatedItemsRelatedItem
    {
        public RelatedItemsRelatedItemItem Item { get; set; }
    }

    public class RelatedItemsRelatedItemItem
    {
        public string ASIN { get; set; }
    }
}
