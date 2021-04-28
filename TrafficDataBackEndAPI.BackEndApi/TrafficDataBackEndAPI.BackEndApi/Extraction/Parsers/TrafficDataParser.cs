using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using TrafficDataBackEndAPI.BackEndApi.DTO;

namespace TrafficDataBackEndAPI.BackEndApi.Extraction.Parsers
{
    public class TrafficDataParser
    {
        private const string siteMeasurementsTag = "siteMeasurements";
        private const string referenceTag = "measurementSiteReference";
        private const string measurementTimeTag = "measurementTimeDefault";
        private const string basicTag = "basicData";
        private const string flowTag = "TrafficFlow";
        private const string speedTag = "TrafficSpeed";
        private const string flowValueTag = "vehicleFlowRate";
        private const string speedValueTag = "speed";
        private readonly IConfiguration configuration;

        public TrafficDataParser(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<TrafficData> ParseTrafficData(Dictionary<string, int> refIds) // Returns DTO's !!
        {
            List<TrafficData> trafficData = new List<TrafficData>();
            
            XDocument xDocument = XDocument.Load(configuration.GetSection("FilePaths:TrafficDataXMLFilePath").Value);
            IEnumerable<XElement> xElements = xDocument.Descendants();

            foreach(var xElement in xElements)
            {
                if (xElement.Name.LocalName.Equals(siteMeasurementsTag))
                {
                    TrafficData trafficDataRow = new TrafficData();
                    int flow = 0;
                    double speed = 0;
                    int speedCount = 0;
                     
                    foreach (var desc in xElement.Descendants())
                    {
                        if (desc.Name.LocalName.Equals(referenceTag))
                        {
                            if (refIds.ContainsKey(desc.FirstAttribute.Value))
                                trafficDataRow.MeasurementPointId = refIds[desc.FirstAttribute.Value];
                        }
                        else if (desc.Name.LocalName.Equals(measurementTimeTag))
                            trafficDataRow.DateTime = DateTime.Parse(desc.Value);
                        else if (desc.Name.LocalName.Equals(basicTag))
                        {
                            string type = desc.FirstAttribute.Value;
                            foreach (var valueDesc in desc.Descendants())
                            {
                                if (type.Equals(flowTag) && valueDesc.Name.LocalName.Equals(flowValueTag) && !valueDesc.Value.Contains("true"))
                                {
                                    flow = flow + int.Parse(valueDesc.Value);
                                }
                                else if (type.Equals(speedTag) && valueDesc.Name.LocalName.Equals(speedValueTag) && !valueDesc.Value.Contains("true"))
                                {
                                    if (double.Parse(valueDesc.Value) > 0)
                                    {
                                        speed = speed + double.Parse(valueDesc.Value);
                                        speedCount++;
                                    }
                                }
                            }
                        }    
                    }
                    
                    if (trafficDataRow.MeasurementPointId != 0) // if the measurement point matches the reference id from the XML file
                    {
                        trafficDataRow.Flow = flow; // Sum of measured flow
                        if (speedCount > 0)
                            trafficDataRow.Speed = speed / speedCount; // Mean of measured speeds
                        else
                            trafficDataRow.Speed = -1; // Invalid value if no vehicles where detected
                        trafficData.Add(trafficDataRow); 
                    }
                }
            }

            return trafficData;
        }

        public IEnumerable<TrafficData> ParseTrafficData(Dictionary<string, int> refIds, string fileDestination) // Returns DTO's !!
        {
            List<TrafficData> trafficData = new List<TrafficData>();

            XDocument xDocument = XDocument.Load(fileDestination);
            IEnumerable<XElement> xElements = xDocument.Descendants();

            foreach (var xElement in xElements)
            {
                if (xElement.Name.LocalName.Equals(siteMeasurementsTag))
                {
                    TrafficData trafficDataRow = new TrafficData();
                    int flow = 0;
                    double speed = 0;
                    int speedCount = 0;

                    foreach (var desc in xElement.Descendants())
                    {
                        if (desc.Name.LocalName.Equals(referenceTag))
                        {
                            if (refIds.ContainsKey(desc.FirstAttribute.Value))
                                trafficDataRow.MeasurementPointId = refIds[desc.FirstAttribute.Value];
                        }
                        else if (desc.Name.LocalName.Equals(measurementTimeTag))
                            trafficDataRow.DateTime = DateTime.Parse(desc.Value);
                        else if (desc.Name.LocalName.Equals(basicTag))
                        {
                            string type = desc.FirstAttribute.Value;
                            foreach (var valueDesc in desc.Descendants())
                            {
                                if (type.Equals(flowTag) && valueDesc.Name.LocalName.Equals(flowValueTag) && !valueDesc.Value.Contains("true"))
                                {
                                    flow = flow + int.Parse(valueDesc.Value);
                                }
                                else if (type.Equals(speedTag) && valueDesc.Name.LocalName.Equals(speedValueTag) && !valueDesc.Value.Contains("true"))
                                {
                                    if (double.Parse(valueDesc.Value) > 0)
                                    {
                                        speed = speed + double.Parse(valueDesc.Value);
                                        speedCount++;
                                    }
                                }
                            }
                        }
                    }

                    if (trafficDataRow.MeasurementPointId != 0) // if the measurement point matches the reference id from the XML file
                    {
                        trafficDataRow.Flow = flow; // Sum of measured flow
                        if (speedCount > 0)
                            trafficDataRow.Speed = speed / speedCount; // Mean of measured speeds
                        else
                            trafficDataRow.Speed = -1; // Invalid value if no vehicles where detected
                        trafficData.Add(trafficDataRow);
                    }
                }
            }

            return trafficData;
        }
    }
}