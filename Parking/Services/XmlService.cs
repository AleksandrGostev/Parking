using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using Parking.Models.Home;

namespace Parking.Services
{
    public static class XmlService
    {
        
        public static XmlParking GetPlaces()
        {
            var doc = XDocument.Load(Config.XmlParkingPath);
            var serializer = new XmlSerializer(typeof(XmlParking));
            return (XmlParking)serializer.Deserialize(doc.CreateReader());
        }

        public static void Checkout(int number, int row)
        {
            var doc = XDocument.Load(Config.XmlParkingPath);
            var element = doc.Descendants("place")
                .Where(
                    x => x.Element("number").Value == number.ToString() && x.Element("row").Value == row.ToString());
            var isClient = Boolean.Parse(element.Elements().FirstOrDefault(x => x.Name == "client").Value);
            var parkedDate = DateTime.Parse(element.Elements().FirstOrDefault(x => x.Name == "time").Value);
            var differnce = DateTime.Now.Subtract(parkedDate);
            var hours = differnce.Hours;
            if (differnce.Minutes > 0)
            {
                hours += 1;
            }
            var totalPrice = 0M;
            if (!isClient)
            {
                totalPrice += (hours*Config.NormalPrice);
                var xElement = doc.Element("parking");
                var totalMoney = Decimal.Parse(xElement.Attribute("total-money-normal").Value);
                xElement.Attribute("total-money-normal").Value = (totalMoney+totalPrice).ToString();
                doc.Save(Config.XmlParkingPath);
            }            
        }
    }
}