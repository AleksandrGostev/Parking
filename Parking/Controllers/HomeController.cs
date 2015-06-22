using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Parking.Models.Home;
using Parking.Services;

namespace Parking.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var parking = XmlService.GetPlaces();
            var model = new IndexViewModel()
            {
                Places = parking.Places
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult GetInfo(int number, int row, bool reserved)
        {
            var parking = XmlService.GetPlaces();
            var model = new PlaceModalViewModel()
            {
                Number = number,
                Row = row
            };
            if (parking.Places.Any(x => x.Number == number && x.Row == row))
            {
                var place = parking.Places.FirstOrDefault(x => x.Number == number && x.Row == row);
                model.IsAvailable = place.IsAvailable;
                model.IsClient = place.IsClient;
                model.Time = place.Time;
                model.TimeElapsed = DateTime.Now.Subtract(model.Time);
                model.ClientId = place.ClientId;
            }
            else
            {
                model.IsAvailable = true;
                model.IsReserved = reserved;
            }
            return PartialView("_PlaceModal", model);
        }

        [HttpPost]
        public bool TakePlace(int number, int row, bool isClient = false)
        {
            try
            {
                var doc = XDocument.Load(Config.XmlParkingPath);
                var newPlace = new XElement("place", new XElement("number", number), new XElement("row", row),
                    new XElement("available", false), new XElement("client", isClient),
                    new XElement("time", DateTime.Now));
                var xElement = doc.Element("parking");
                if (xElement != null)
                {
                    if (isClient)
                    {
                        var totalClients = Int32.Parse(xElement.Attribute("total-clients").Value);
                        xElement.Attribute("total-clients").Value = (++totalClients).ToString();
                        newPlace.Add(new XElement("client", true));
                    }
                    else
                    {
                        var totalNormal = Int32.Parse(xElement.Attribute("total-normal").Value);
                        xElement.Attribute("total-normal").Value = (++totalNormal).ToString();
                    }
                    xElement.Add(newPlace);
                }

                doc.Save(Config.XmlParkingPath);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool LeavePlace(int number, int row)
        {
            try
            {
                XmlService.Checkout(number, row);
                var doc = XDocument.Load(Config.XmlParkingPath);
                doc.Descendants("place")
                    .Where(
                        x => x.Element("number").Value == number.ToString() && x.Element("row").Value == row.ToString())
                    .Remove();
                doc.Save(Config.XmlParkingPath);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        [HttpPost]
        public ActionResult Stats()
        {
            var doc = XDocument.Load(Config.XmlParkingPath);
            var parking = doc.Element("parking");
            if (parking == null)
            {
                throw new Exception("missing xml file");
            }
            var totalClients = Int32.Parse(parking.Attribute("total-clients").Value);
            var totalNormal = Int32.Parse(parking.Attribute("total-normal").Value);
            var totalPriceNormal = Decimal.Parse(parking.Attribute("total-money-normal").Value);
            var model = new StatsViewModel()
            {
                TotalClients = totalClients,
                TotalNormal= totalNormal,
                TotalPrice = (totalClients * Config.ClientPrice) + totalPriceNormal
            };

            return PartialView("_StatsModal", model);
        }
    }
}