using System.Collections.Generic;
using TrafficDataBackEndAPI.BackEndApi.Data;

namespace TrafficDataBackEndAPI.BackEndApi.Models 
{
    public class MeasurementPoint
    {
        public MeasurementPoint()
        {
            TrafficData = new List<TrafficData>();
        }

        public int Id {get; set;}
        public string Reference {get; set;}
        public double Latitude {get; set;}
        public double Longitude {get; set;}
        public IEnumerable<TrafficData> TrafficData {get; set;}

        public override bool Equals(object obj)
        {
            MeasurementPoint measurementPoint = (MeasurementPoint)obj;

            if (measurementPoint == null)
                return false;
            else 
                return measurementPoint.Id.Equals(measurementPoint.Id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "{" + Id + ": " + Reference + ", " + Latitude + ", " + Longitude + "}";
        }
    }
}