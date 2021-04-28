using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrafficDataBackEndAPI.BackEndApi.Data;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using TrafficDataBackEndAPI.BackEndApi.Models;

namespace TrafficDataBackEndAPI.BackEndApi.Services
{
    public class TrafficDataService:ITrafficDataService
    {
        private const string datetimeExceptionMessage = "The start date cannot be greater than the end date";
        private readonly Context context;
        private readonly IMeasurementPointService measurementPointService;

        public TrafficDataService(Context context, IMeasurementPointService measurementPointService)
        {
            this.context = context;
            this.measurementPointService = measurementPointService;
        }

        public async Task<IEnumerable<TrafficData>> GetAllTrafficData()
        {
            return await context.TrafficData.ToListAsync();
        }

        // Id of MeasurementPoint
        public async Task<IEnumerable<TrafficData>> GetTrafficDataByMeasurementPointId(int id) 
        {
            MeasurementPoint measurementPoint = await measurementPointService.GetMeasurementPoint(id);  
            return measurementPoint.TrafficData;
        }

        public async Task<IEnumerable<TrafficData>> GetTrafficDataTimeInterval(DateTime startDateTime, DateTime endDateTime)
        {
            if (startDateTime >= endDateTime)
                throw new Exception(datetimeExceptionMessage);
            
            IEnumerable<TrafficData> trafficData = await GetAllTrafficData();
            IEnumerable<TrafficData> trafficDataTimeInterval = trafficData
                .Where(t => t.DateTime >= startDateTime)
                .Where(t => t.DateTime <= endDateTime)
                .ToList();
            return trafficDataTimeInterval;
        }

        // Id of MeasurementPoint
        public async Task<IEnumerable<TrafficData>> GetTrafficDataTimeIntervalByMeasurementPointId(int id, DateTime startDateTime, DateTime endDateTime) 
        {
            if (startDateTime > endDateTime)
                throw new Exception(datetimeExceptionMessage);

            MeasurementPoint measurementPoint = await measurementPointService.GetMeasurementPoint(id);
            IEnumerable<TrafficData> trafficData = measurementPoint.TrafficData;
            IEnumerable<TrafficData> selectedTrafficData = trafficData
                .Where(t => t.DateTime >= startDateTime)
                .Where(t => t.DateTime <= endDateTime)
                .ToList();
            return selectedTrafficData;
        }

        public async Task<TrafficData> AddTrafficDataRow(TrafficData trafficDataRow)
        {
            await context.TrafficData.AddAsync(trafficDataRow);
            await context.SaveChangesAsync();
            return trafficDataRow;
        }

        public async Task<IEnumerable<TrafficData>> AddTrafficData(IEnumerable<TrafficData> trafficData)
        {
            await context.TrafficData.AddRangeAsync(trafficData);
            await context.SaveChangesAsync();
            return trafficData;
        }

        public async Task<TrafficData> UpdateTrafficDataRow(TrafficData trafficDataRow)
        {
            IEnumerable<TrafficData> trafficData = await GetAllTrafficData();
            TrafficData trafficDataRowDB = trafficData.Where(t => t.Id == trafficDataRow.Id).SingleOrDefault();

            if (trafficDataRowDB == null)
                throw new ArgumentNullException("Traffic Data Row is Null");

            trafficDataRowDB.DateTime = trafficDataRow.DateTime;
            trafficDataRowDB.Flow = trafficDataRow.Flow;
            trafficDataRowDB.Speed = trafficDataRow.Speed;

            await context.SaveChangesAsync();
            return trafficDataRowDB;
        }

        public async Task<IEnumerable<TrafficData>> UpdateTrafficData(IEnumerable<TrafficData> trafficData)
        {
            IEnumerable<TrafficData> trafficDataDB = await GetAllTrafficData();
            foreach (var trafficDataRow in trafficData)
            {
                TrafficData trafficDataRowDB = trafficDataDB.Where(t => t.Id == trafficDataRow.Id).SingleOrDefault();

                if (trafficDataRowDB == null)
                    throw new ArgumentNullException();
                
                trafficDataRowDB.DateTime = trafficDataRow.DateTime;
                trafficDataRowDB.Flow = trafficDataRow.Flow;
                trafficDataRowDB.Speed = trafficDataRow.Speed;
            }
            await context.SaveChangesAsync();
            return trafficDataDB;
        }

        public async Task DeleteTrafficDataRow(int id)
        {
            IEnumerable<TrafficData> trafficData = await GetAllTrafficData();
            TrafficData trafficDataRowDB = trafficData.Where(t => t.Id == id).SingleOrDefault();

            if (trafficDataRowDB == null)
                throw new ArgumentNullException();
            
            context.TrafficData.Remove(trafficDataRowDB);
            await context.SaveChangesAsync();
        }

        public async Task DeleteTrafficData(IEnumerable<int> ids)
        {
            IEnumerable<TrafficData> trafficData = await GetAllTrafficData();
            foreach (var id in ids)
            {
                TrafficData trafficDataRow = trafficData.Where(t => t.Id == id).SingleOrDefault();

                if (trafficDataRow != null)
                    context.Remove(trafficDataRow);

            }
            await context.SaveChangesAsync();
        }

        // This method deletes all the traffic data previous to the specified date time
        public async Task DeleteTrafficData(DateTime endDateTime)
        {
            IEnumerable<TrafficData> trafficData = await GetAllTrafficData();
            IEnumerable<TrafficData> selectedTrafficData = trafficData.Where(t => t.DateTime < endDateTime);
            context.RemoveRange(selectedTrafficData);
            await context.SaveChangesAsync();
        }
    }
}