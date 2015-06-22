using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Parking.Models.Home
{
    [XmlRoot("parking"), XmlType("parking")]
    public class XmlParking
    {
        [XmlElement("place")]
        public List<Place> Places { get; set; }
    }

    public class Place
    {
        [XmlElement("number")]
        public int Number { get; set; }
        [XmlElement("row")]
        public int Row { get; set; }
        [XmlElement("time")]
        public DateTime Time { get; set; }
        [XmlElement("available")]
        public bool IsAvailable { get; set; }
        [XmlElement("client")]
        public bool IsClient { get; set; }
        [XmlElement("clientId")]
        public string ClientId { get; set; }
    }
}