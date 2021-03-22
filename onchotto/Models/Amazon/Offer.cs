﻿using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    public class Offer
    {
        public Merchant Merchant { get; set; }
        public OfferAttributes OfferAttributes { get; set; }
        [XmlElement("OfferListing")]
        public OfferListing[] OfferListing { get; set; }
        public LoyaltyPoints LoyaltyPoints { get; set; }
        public Promotion[] Promotions { get; set; }
    }
}
