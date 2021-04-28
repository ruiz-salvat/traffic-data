using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using TrafficDataBackEndAPI.BackEndApi.DTO;

namespace TrafficDataBackEndAPI.BackEndApi.Extraction.Parsers
{
    public class MeasurementPointsParser
    {
        private const string measurementSitesTag = "measurementSiteRecord";
        private const string locationTag = "locationForDisplay";
        private const string latitudeTag = "latitude";
        private const string longitudeTag = "longitude";
        private readonly IConfiguration configuration;
        private readonly double maxLat;
        private readonly double minLat;
        private readonly double maxLon;
        private readonly double minLon;

        public MeasurementPointsParser(IConfiguration configuration)
        {
            this.configuration = configuration;
            maxLat = double.Parse(configuration.GetSection("CoordinateLimits:MaxLat").Value);
            minLat = double.Parse(configuration.GetSection("CoordinateLimits:MinLat").Value);
            maxLon = double.Parse(configuration.GetSection("CoordinateLimits:MaxLon").Value);
            minLon = double.Parse(configuration.GetSection("CoordinateLimits:MinLon").Value);
        }

        public IEnumerable<MeasurementPoint> ParseMeasurementPoints()
        {
            List<MeasurementPoint> measurementPoints = new List<MeasurementPoint>();
            XDocument xDocument = XDocument.Load(configuration.GetSection("FilePaths:MeasurementPointXMLFilePath").Value);
            
            foreach (var xElement in xDocument.Descendants())
            {
                if (xElement.Name.LocalName.Equals(measurementSitesTag))
                {
                    MeasurementPoint measurementPoint = new MeasurementPoint();
                    measurementPoint.Reference = xElement.FirstAttribute.Value;
                    
                    foreach (var desc in xElement.Descendants())
                    {
                        if (desc.Name.LocalName.Equals(locationTag))
                        {
                            IEnumerable<XElement> latLonElements = desc.Descendants();
                            try
                            {
                                string latStr = latLonElements.Where(e => e.Name.LocalName.Equals(latitudeTag)).SingleOrDefault().Value;
                                string lonStr = latLonElements.Where(e => e.Name.LocalName.Equals(longitudeTag)).SingleOrDefault().Value;
                                measurementPoint.Latitude = double.Parse(latStr);
                                measurementPoint.Longitude = double.Parse(lonStr);
                                if (
                                    measurementPoint.Latitude < maxLat &&
                                    measurementPoint.Latitude > minLat &&
                                    measurementPoint.Longitude < maxLon &&
                                    measurementPoint.Longitude > minLon
                                ) 
                                {
                                    measurementPoints.Add(measurementPoint);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.StackTrace);
                            }
                            
                            break;
                        }
                    }                    
                }
            }
            
            return measurementPoints;
        }

        public IEnumerable<MeasurementPoint> ParseMeasurementPoints(string fileDestination)
        {
            List<MeasurementPoint> measurementPoints = new List<MeasurementPoint>();
            XDocument xDocument = XDocument.Load(fileDestination);

            foreach (var xElement in xDocument.Descendants())
            {
                if (xElement.Name.LocalName.Equals(measurementSitesTag))
                {
                    MeasurementPoint measurementPoint = new MeasurementPoint();
                    measurementPoint.Reference = xElement.FirstAttribute.Value;

                    foreach (var desc in xElement.Descendants())
                    {
                        if (desc.Name.LocalName.Equals(locationTag))
                        {
                            IEnumerable<XElement> latLonElements = desc.Descendants();
                            try
                            {
                                string latStr = latLonElements.Where(e => e.Name.LocalName.Equals(latitudeTag)).SingleOrDefault().Value;
                                string lonStr = latLonElements.Where(e => e.Name.LocalName.Equals(longitudeTag)).SingleOrDefault().Value;
                                measurementPoint.Latitude = double.Parse(latStr);
                                measurementPoint.Longitude = double.Parse(lonStr);
                                if (
                                    measurementPoint.Latitude < maxLat &&
                                    measurementPoint.Latitude > minLat &&
                                    measurementPoint.Longitude < maxLon &&
                                    measurementPoint.Longitude > minLon
                                )
                                {
                                    measurementPoints.Add(measurementPoint);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.StackTrace);
                            }

                            break;
                        }
                    }
                }
            }

            return measurementPoints;
        }
    }
}