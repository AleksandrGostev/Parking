using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Parking
{
    public static class Config
    {
        public static string XmlParkingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            System.Configuration.ConfigurationManager.AppSettings["XmlParkingPath"]);

        public static decimal ClientPrice = 30M;
        public static decimal NormalPrice = 0.5M;
    }
}