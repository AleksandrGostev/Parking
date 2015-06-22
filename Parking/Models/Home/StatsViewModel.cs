using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace Parking.Models.Home
{
    public class StatsViewModel
    {
        public int TotalClients { get; set; }
        public int TotalNormal { get; set; }
        public decimal TotalPrice { get; set; }
    }
}