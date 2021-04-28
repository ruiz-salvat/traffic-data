using System.Collections.Generic;

namespace TrafficDataBackEndAPI.BackEndApi.DTO
{
    public class MeasurementPoint
    {
        public int Id {get; set;}
        public string Reference {get; set;}
        public double Latitude {get; set;}
        public double Longitude {get; set;}
    }
}