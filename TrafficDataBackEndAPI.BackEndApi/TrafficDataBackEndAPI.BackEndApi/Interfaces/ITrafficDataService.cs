using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficDataBackEndAPI.BackEndApi.Models;

namespace TrafficDataBackEndAPI.BackEndApi.Interfaces
{
    public interface ITrafficDataService
    {
        public Task<IEnumerable<TrafficData>> GetAllTrafficData();
        public Task<IEnumerable<TrafficData>> GetTrafficDataByMeasurementPointId(int id);
        public Task<IEnumerable<TrafficData>> GetTrafficDataTimeInterval(DateTime startDateTime, DateTime endDateTime);
        public Task<IEnumerable<TrafficData>> GetTrafficDataTimeIntervalByMeasurementPointId(int id, DateTime startDateTime, DateTime endDateTime);
        public Task<TrafficData> AddTrafficDataRow(TrafficData trafficDataRow);
        public Task<IEnumerable<TrafficData>> AddTrafficData(IEnumerable<TrafficData> trafficData);
        public Task<TrafficData> UpdateTrafficDataRow(TrafficData trafficDataRow);
        public Task<IEnumerable<TrafficData>> UpdateTrafficData(IEnumerable<TrafficData> trafficData);
        public Task DeleteTrafficDataRow(int id);
        public Task DeleteTrafficData(IEnumerable<int> ids);
        public Task DeleteTrafficData(DateTime endDateTime);
    }
}