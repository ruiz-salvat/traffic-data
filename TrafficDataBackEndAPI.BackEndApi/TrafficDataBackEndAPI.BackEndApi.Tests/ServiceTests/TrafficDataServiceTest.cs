using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficDataBackEndAPI.BackEndApi.Data;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using TrafficDataBackEndAPI.BackEndApi.Models;
using TrafficDataBackEndAPI.BackEndApi.Services;
using TrafficDataBackEndAPI.BackEndApi.Tests.Mocks;
using Xunit;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.ServiceTests
{
    public class TrafficDataServiceTest : IDisposable
    {
        private readonly Context context;
        private readonly ITrafficDataService trafficDataService;
        private readonly IMeasurementPointService measurementPointService;

        public TrafficDataServiceTest()
        {
            context = TestDBInitializer.CreateContext();
            TestDBInitializer.Initialize(context);

            measurementPointService = new MeasurementPointService(context, MockMapper.CreateMapper());
            trafficDataService = new TrafficDataService(context, measurementPointService);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        private int PopulateTable()
        {
            List<TrafficData> trafficData = (List<TrafficData>)MockFactory.CreateTrafficDataList();
            trafficDataService.AddTrafficData(trafficData);
            return trafficData.Count;
        }

        [Fact]
        public async Task GetAllTrafficData_TableSize_Equal()
        {
            int size = PopulateTable();

            List<TrafficData> trafficData = (List<TrafficData>) await trafficDataService.GetAllTrafficData();

            Assert.Equal(size, trafficData.Count);
        }

        [Fact]
        public async Task GetTrafficDataByMeasurementPointId_ValidId_NotNull()
        {
            await measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());
            await trafficDataService.AddTrafficDataRow(MockFactory.CreateTrafficDataRow());

            Assert.NotNull(trafficDataService.GetTrafficDataByMeasurementPointId(Constants.ValidId));
        }

        [Fact]
        public async Task GetTrafficDataByMeasurementPointId_InvalidId_ThrowsException()
        {
            await measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());
            await trafficDataService.AddTrafficDataRow(MockFactory.CreateTrafficDataRow());

            await Assert.ThrowsAsync<InvalidOperationException>(() => trafficDataService.GetTrafficDataByMeasurementPointId(Constants.InvalidId));
        }

        [Fact]
        public async Task GetTrafficDataTimeInterval_ValidDates_TableSizeEqual()
        { 
            int size = PopulateTable();

            List<TrafficData> trafficData = (List<TrafficData>) await trafficDataService.GetTrafficDataTimeInterval(Constants.StartDatetime, Constants.EndDatetime);

            Assert.Equal(size, trafficData.Count);
        }

        [Fact]
        public async Task GetTrafficDataTimeInterval_InvalidDates_ThrowsException()
        {
            PopulateTable();

            await Assert.ThrowsAsync<Exception>(() => trafficDataService.GetTrafficDataTimeInterval(Constants.EndDatetime, Constants.StartDatetime));
        }

        [Fact]
        public async Task GetTrafficDataTimeIntervalByMeasurementPointId_ValidInputs_TableSizeEqual()
        {
            await measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());
            int size = PopulateTable();

            List<TrafficData> trafficData = (List<TrafficData>) await trafficDataService.GetTrafficDataTimeIntervalByMeasurementPointId(Constants.ValidId, Constants.StartDatetime, Constants.EndDatetime);

            Assert.Equal(size, trafficData.Count);
        }

        [Fact]
        public async Task GetTrafficDataTimeIntervalByMeasurementPointId_InvalidId_ThrowsException()
        {
            await measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());
            int size = PopulateTable();

            await Assert.ThrowsAsync<InvalidOperationException>(() => trafficDataService.GetTrafficDataTimeIntervalByMeasurementPointId(Constants.InvalidId, Constants.StartDatetime, Constants.EndDatetime));
        }

        [Fact]
        public async Task GetTrafficDataTimeIntervalByMeasurementPointId_InvalidDates_ThrowsException()
        {
            await measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());
            int size = PopulateTable();

            await Assert.ThrowsAsync<Exception>(() => trafficDataService.GetTrafficDataTimeIntervalByMeasurementPointId(Constants.ValidId, Constants.EndDatetime, Constants.StartDatetime));
        }

        [Fact]
        public async Task AddTrafficDataRow_ValidId_EqualIds()
        {
            TrafficData trafficData = await trafficDataService.AddTrafficDataRow(MockFactory.CreateTrafficDataRow());

            Assert.Equal(Constants.ValidId, trafficData.Id);
        }

        [Fact]
        public async Task AddTrafficData_TableSize_Equal()
        { 
            int size = PopulateTable(); // AddTrafficData method is in PopulateTable

            List<TrafficData> trafficData = (List<TrafficData>) await trafficDataService.GetAllTrafficData();

            Assert.Equal(size, trafficData.Count);
        }

        [Fact]
        public async Task UpdateTrafficDataRow_ValidId_FlowEquals()
        {
            await trafficDataService.AddTrafficDataRow(MockFactory.CreateTrafficDataRow());
            int newFlow = 1;
            
            TrafficData trafficData = MockFactory.CreateTrafficDataRow();
            trafficData.Id = Constants.ValidId;
            trafficData.Flow = newFlow;
            await trafficDataService.UpdateTrafficDataRow(trafficData);

            Assert.Equal(newFlow, ((List<TrafficData>)await trafficDataService.GetAllTrafficData()).Where(t => t.Id == Constants.ValidId).SingleOrDefault().Flow);
        }

        [Fact]
        public async Task UpdateTrafficDataRow_InvalidId_ThrowsException()
        {
            await trafficDataService.AddTrafficDataRow(MockFactory.CreateTrafficDataRow());

            TrafficData trafficData = MockFactory.CreateTrafficDataRow();
            trafficData.Id = Constants.InvalidId;

            await Assert.ThrowsAsync<ArgumentNullException>(() => trafficDataService.UpdateTrafficDataRow(trafficData));
        }

        [Fact]
        public async Task UpdateTrafficData_TableSize_Equal()
        {
            int size = PopulateTable();

            Assert.Equal(size, ((List<TrafficData>) await trafficDataService.UpdateTrafficData(MockFactory.CreateTrafficDataListWithIds())).Count);
        }

        [Fact]
        public async Task UpdateTrafficData_OneInvalidId_ThrowsException()
        {
            PopulateTable();

            List<TrafficData> trafficData = (List<TrafficData>)MockFactory.CreateTrafficDataList();
            trafficData[0].Id = Constants.InvalidId;

            await Assert.ThrowsAsync<ArgumentNullException>(() => trafficDataService.UpdateTrafficData(trafficData));
        }

        [Fact]
        public async Task DeleteTrafficDataRow_ValidId_Null()
        {
            await trafficDataService.AddTrafficDataRow(MockFactory.CreateTrafficDataRow());

            await trafficDataService.DeleteTrafficDataRow(Constants.ValidId);

            Assert.Empty(await trafficDataService.GetAllTrafficData());
        }

        [Fact]
        public async Task DeleteTrafficDataRow_InvalidId_ThrowsException()
        {
            await trafficDataService.AddTrafficDataRow(MockFactory.CreateTrafficDataRow());

            await Assert.ThrowsAsync<ArgumentNullException>(() => trafficDataService.DeleteTrafficDataRow(Constants.InvalidId));
        }

        [Fact]
        public async Task DeleteTrafficData_ValidIds_Null()
        {
            int size = PopulateTable();

            await trafficDataService.DeleteTrafficData(MockFactory.CreateIdList(size));

            Assert.Empty(await trafficDataService.GetAllTrafficData());
        }

        [Fact]
        public async Task DeleteTrafficData_InvalidIds_ThrowsException()
        {
            int size = PopulateTable();

            List<int> idList = (List<int>)MockFactory.CreateIdList(size);
            idList[0] = Constants.InvalidId;

            await trafficDataService.DeleteTrafficData(idList);

            Assert.NotEmpty(await trafficDataService.GetAllTrafficData());
        }

        [Fact]
        public async Task DeleteTrafficData_ValidDateTime_Empty()
        {
            PopulateTable();

            await trafficDataService.DeleteTrafficData(DateTime.Now);

            Assert.Empty(await trafficDataService.GetAllTrafficData());
        }
    }
}