using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parking.Models.Home
{
    public class PlaceModalViewModel
    {
        public bool IsAvailable { get; set; }
        public bool IsClient { get; set; }
        public DateTime Time { get; set; }
        public TimeSpan TimeElapsed { get; set; }
        public int Number { get; set; }
        public int Row { get; set; }
        public string ClientId { get; set; }
        public bool IsReserved { get; set; }
    }
}