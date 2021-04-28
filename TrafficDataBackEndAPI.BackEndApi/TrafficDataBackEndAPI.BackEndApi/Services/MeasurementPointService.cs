using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrafficDataBackEndAPI.BackEndApi.Data;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using TrafficDataBackEndAPI.BackEndApi.Models;

namespace TrafficDataBackEndAPI.BackEndApi.Services 
{
    public class MeasurementPointService:IMeasurementPointService
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public MeasurementPointService(Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<MeasurementPoint>> GetMeasurementPoints()
        {
            return await context.MeasurementPoints
                .Include(m => m.TrafficData)
                .ToListAsync();
        }

        public IEnumerable<MeasurementPoint> GetMeasurementPointsSync()
        {
            return (IEnumerable<MeasurementPoint>)context.MeasurementPoints
                .Include(m => m.TrafficData)
                .ToList();
        }

        public async Task<MeasurementPoint> GetMeasurementPoint(int id)
        {
            IEnumerable<MeasurementPoint> measurementPoints = await GetMeasurementPoints();
            return measurementPoints.Where(m => m.Id == id).Single();
        }

        public MeasurementPoint GetMeasurementPointSync(int id)
        {
            IEnumerable<MeasurementPoint> measurementPoints = GetMeasurementPointsSync();
            return measurementPoints.Where(m => m.Id == id).Single();
        }

        public async Task<MeasurementPoint> AddMeasurementPoint(MeasurementPoint measurementPoint)
        {
            await context.MeasurementPoints.AddAsync(measurementPoint);
            await context.SaveChangesAsync();
            return measurementPoint;
        }

        // This method is specific for the data extraction
        // Checks if a measurement point already exists in the database
        public IEnumerable<MeasurementPoint> AddMeasurementPoints(IEnumerable<MeasurementPoint> measurementPoints)
        {
            foreach (var measurementPoint in measurementPoints)
            {
                if (GetMeasurementPointsSync().Where(m => m.Reference == measurementPoint.Reference).Count() == 0)
                {
                    context.Add(measurementPoint);
                }
            }
            context.SaveChanges();
            return measurementPoints;
        }

        public async Task<MeasurementPoint> UpdateMeasurementPoint(MeasurementPoint measurementPoint)
        {
            MeasurementPoint measurementPointDB = await GetMeasurementPoint(measurementPoint.Id);

            measurementPointDB.Reference = measurementPoint.Reference;
            measurementPointDB.Latitude = measurementPoint.Latitude;
            measurementPointDB.Longitude = measurementPoint.Longitude;

            await context.SaveChangesAsync();
            return measurementPointDB;
        }

        // The following method is used to add traffic data to the database updating the measurement points
        // The updates consist of appending traffic data rows to the traffic data list of each of the updated measurement points
        public IEnumerable<MeasurementPoint> UpdateMeasurementPointList_AddTrafficData(IEnumerable<DTO.TrafficData> trafficDataDTO)
        {
            foreach (var trafficDataRowDTO in trafficDataDTO)
            {
                MeasurementPoint measurementPoint = GetMeasurementPointsSync()
                    .Where(m => m.Id == trafficDataRowDTO.MeasurementPointId)
                    .SingleOrDefault();
                
                if (measurementPoint != null)
                {
                    ((List<TrafficData>)measurementPoint.TrafficData).Add(
                        mapper.Map<TrafficData>(trafficDataRowDTO)
                    );
                }
            }
            
            context.SaveChanges();
            return GetMeasurementPointsSync();
        }

        public async Task DeleteMeasurementPoint(int id)
        {
            MeasurementPoint measurementPoint = await GetMeasurementPoint(id);
            context.MeasurementPoints.Remove(measurementPoint);
            await context.SaveChangesAsync();
        }
    }
}