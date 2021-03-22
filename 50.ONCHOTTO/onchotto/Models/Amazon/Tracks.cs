﻿using System.Xml.Serialization;

namespace OnChotto.Models.Amazon
{
    [XmlRoot]
    public class Tracks
    {
        public TracksDisc Disc { get; set; }
    }

    public class TracksDisc
    {
        [XmlElement("Track")]
        public TracksDiscTrack[] Track { get; set; }

        [XmlAttribute]
        public byte Number { get; set; }
    }

    public class TracksDiscTrack
    {
        [XmlAttribute]
        public byte Number { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
