using System;

namespace TrafficDataBackEndAPI.BackEndApi.DTO
{
    public class TrafficData
    {
        public int Id {get; set;}
        public DateTime DateTime {get; set;}
        public int Flow {get; set;}
        public double Speed {get; set;}
        public int MeasurementPointId {get; set;}
    }
}