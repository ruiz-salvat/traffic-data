using System;

namespace TrafficDataBackEndAPI.BackEndApi.Models {
    public class TrafficData
    {
        public int Id {get; set;}
        public DateTime DateTime {get; set;}
        public int Flow {get; set;}
        public double Speed {get; set;}

        public override string ToString()
        {
            return "{" + DateTime + ", " + Flow + ", " + Speed + "}";
        }
    }
}