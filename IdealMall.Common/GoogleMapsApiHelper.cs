using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace IdealMall.Common
{
    public class GoogleMapsApiHelper
    {
        public static XElement GetGeocodingSearchResults(string address)
        {

            var url = String.Format("http://maps.google.com/maps/api/geocode/xml?address={0}&sensor=false", address);

            var results = XElement.Load(url);

            var status = results.Element("status").Value;
            if (status != "OK" && status != "ZERO_RESULTS")
                throw new ApplicationException("There was an error with Google's Geocoding Service: " + status);

            return results;
        }

        public static List<string> GetLatLng(string address, string radius)
        {
            var latLng = new LatLng();
            List<string> lstPostCode = new List<string>();
            try
            {
                var results = GoogleMapsApiHelper.GetGeocodingSearchResults(address + ",UK");

                var resultCount = results.Elements("result").Count();

                if (resultCount > 0)
                {
                    latLng.Latitude =
                        Convert.ToDecimal (
                            results.Element("result")
                                .Element("geometry")
                                .Element("location")
                                .Element("lat")
                                .Value);
                    latLng.Longitude =
                        Convert.ToDecimal (
                            results.Element("result")
                                .Element("geometry")
                                .Element("location")
                                .Element("lng")
                                .Value);


                    var url = String.Format("http://maps.google.com/maps/api/place/nearbysearch/xml?location={0}&radius={1}&sensor=false", latLng.Latitude + "," + latLng.Longitude, radius);
                    XmlDocument doc = new XmlDocument();
                    doc.Load(url);

                    XmlNode element = doc.SelectSingleNode("//GeocodeResponse/status");
                    if (element.InnerText == "ZERO_RESULTS")
                    {
                        //  return ("No data available for the specified location");
                    }
                    else
                    {

                        element = doc.SelectSingleNode("//GeocodeResponse/result/formatted_address");

                        string longname = "";
                        string shortname = "";
                        string typename = "";
                        bool fHit = false;


                        XmlNodeList xnList = doc.SelectNodes("//GeocodeResponse/result/address_component");

                        foreach (XmlNode xn in xnList)
                        {

                            longname = xn["long_name"].InnerText;
                            shortname = xn["short_name"].InnerText;
                            typename = xn["type"].InnerText;


                            fHit = true;
                            switch (typename)
                            {
                                //Add whatever you are looking for below
                                case "postal_code":
                                    {
                                        lstPostCode.Add(longname);
                                        break;
                                    }

                                default:
                                    fHit = false;
                                    break;
                            }

                        }
                    }
                }
            }

            catch (Exception)
            {
                latLng.Latitude = 0;
                latLng.Longitude = 0;
            }
            return lstPostCode;
        }


        public static LatLng GetLatLng(string address)
        {
            var latLng = new LatLng();

            try
            {

                var results = GoogleMapsApiHelper.GetGeocodingSearchResults(address + ",UK");

                var resultCount = results.Elements("result").Count();

                if (resultCount > 0)
                {
                    latLng.Latitude =
                        Convert.ToDecimal(
                            results.Element("result")
                                .Element("geometry")
                                .Element("location")
                                .Element("lat")
                                .Value);
                    latLng.Longitude =
                        Convert.ToDecimal(
                            results.Element("result")
                                .Element("geometry")
                                .Element("location")
                                .Element("lng")
                                .Value);
                }
            }
            catch (Exception)
            {
                latLng.Latitude = 0;
                latLng.Longitude = 0;
            }
            return latLng;
        }

    }
}