using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficDataBackEndAPI.BackEndApi.Models;

namespace TrafficDataBackEndAPI.BackEndApi.Interfaces
{
    public interface IMeasurementPointService
    {
        public Task<IEnumerable<MeasurementPoint>> GetMeasurementPoints();
        public IEnumerable<MeasurementPoint> GetMeasurementPointsSync();
        public Task<MeasurementPoint> GetMeasurementPoint(int id);
        public MeasurementPoint GetMeasurementPointSync(int id);
        public Task<MeasurementPoint> AddMeasurementPoint(MeasurementPoint measurementPoint);
        public IEnumerable<MeasurementPoint> AddMeasurementPoints(IEnumerable<MeasurementPoint> measurementPoints);
        public Task<MeasurementPoint> UpdateMeasurementPoint(MeasurementPoint measurementPoint);
        public IEnumerable<MeasurementPoint> UpdateMeasurementPointList_AddTrafficData(IEnumerable<DTO.TrafficData> trafficDataDTO);
        public Task DeleteMeasurementPoint(int id);
    }
}