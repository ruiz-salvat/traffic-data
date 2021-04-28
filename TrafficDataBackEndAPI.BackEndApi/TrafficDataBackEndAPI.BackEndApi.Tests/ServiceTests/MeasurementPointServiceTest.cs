using TrafficDataBackEndAPI.BackEndApi.Services;
using TrafficDataBackEndAPI.BackEndApi.Data;
using TrafficDataBackEndAPI.BackEndApi.Tests.Mocks;
using Xunit;
using System;
using System.Collections.Generic;
using TrafficDataBackEndAPI.BackEndApi.Models;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using System.Threading.Tasks;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.ServiceTests
{
    public class MeasurementPointServiceTest:IDisposable
    {
        private readonly IMeasurementPointService measurementPointService;
        private readonly Context context;

        public MeasurementPointServiceTest()
        {
            context = TestDBInitializer.CreateContext();
            TestDBInitializer.Initialize(context);
            measurementPointService = new MeasurementPointService(context, MockMapper.CreateMapper());
        }

        public void Dispose()
        {
            context.Dispose();
        }

        private int PopulateTable()
        {
            List<MeasurementPoint> measurementPoints = (List<MeasurementPoint>)MockFactory.CreateMeasurementPointList();

            foreach (var measurementPoint in measurementPoints)
            {
                measurementPointService.AddMeasurementPoint(measurementPoint);
            }

            return measurementPoints.Count;
        }

        [Fact]
        public async Task GetMeasurementPoint_ValidId_NotNull()
        {
            await measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());
            
            Assert.NotNull(measurementPointService.GetMeasurementPoint(Constants.ValidId));
        }

        [Fact]
        public async Task GetMeasurementPoint_InvalidId_ThrowsException()
        {
            await measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());

            await Assert.ThrowsAsync<InvalidOperationException>(() => measurementPointService.GetMeasurementPoint(Constants.InvalidId));
        }

        [Fact]
        public void GetMeasurementPointSync_ValidId_NotNull()
        {
            measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());
            
            Assert.NotNull(measurementPointService.GetMeasurementPointSync(Constants.ValidId));
        }

        [Fact]
        public void GetMeasurementPointSync_InvalidId_ThrowsException()
        {
            measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());

            Assert.Throws<InvalidOperationException>(() => measurementPointService.GetMeasurementPointSync(Constants.InvalidId));
        }

        [Fact]
        public async Task GetMeasurementPoints_TableSize_EqualAsync()
        {
            int size = PopulateTable();

            List<MeasurementPoint> measurementPoints = (List<MeasurementPoint>)await measurementPointService.GetMeasurementPoints();

            Assert.Equal(size, measurementPoints.Count);
        }

        [Fact]
        public void GetMeasurementPointsSync_TableSize_Equal()
        {
            int size = PopulateTable();

            List<MeasurementPoint> measurementPoints = (List<MeasurementPoint>)measurementPointService.GetMeasurementPointsSync();

            Assert.Equal(size, measurementPoints.Count);
        }

        [Fact]
        public async Task AddMeasurementPoint_ValidId_EqualAsync()
        {
            MeasurementPoint measurementPoint = await measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());

            Assert.Equal(Constants.ValidId, measurementPoint.Id);
        }

        [Fact]
        public async Task AddMeasurementPoints_TableSize_Equal()
        {
            List<MeasurementPoint> measurementPoints = (List<MeasurementPoint>)await MockFactory.CreateMeasurementPointListAsync();

            List<MeasurementPoint> measurementPointsDB = (List<MeasurementPoint>)measurementPointService.AddMeasurementPoints(measurementPoints);

            Assert.Equal(measurementPoints.Count, measurementPointsDB.Count);
        }

        [Fact]
        public async Task UpdateMeasuretementPoint_ValidId_ReferenceEqualAsync()
        {
            string newReference = "test_reference";
            await measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());

            MeasurementPoint measurementPoint = MockFactory.CreateMeasurementPoint();
            measurementPoint.Id = Constants.ValidId;
            measurementPoint.Reference = newReference;

            await measurementPointService.UpdateMeasurementPoint(measurementPoint);

            MeasurementPoint measurementPointFromService = await measurementPointService.GetMeasurementPoint(Constants.ValidId);
            Assert.Equal(newReference, measurementPointFromService.Reference);
        }

        [Fact]
        public void UpdateMeasurementPoint_InvalidId_ThrowsException()
        {
            measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());

            MeasurementPoint measurementPoint = MockFactory.CreateMeasurementPoint();
            measurementPoint.Id = Constants.InvalidId;

            Assert.ThrowsAsync<ArgumentNullException>(() => measurementPointService.UpdateMeasurementPoint(measurementPoint));
        }

        [Fact]
        public void UpdateMeasurementPointList_AddTrafficData_TDListNotEqual()
        {
            measurementPointService.AddMeasurementPoints((List<MeasurementPoint>)MockFactory.CreateMeasurementPointList());

            IEnumerable<DTO.TrafficData> trafficDataDtos = MockFactory.CreateTrafficDataDtoList();
            List<MeasurementPoint> measurementPointsFromUpdate = (List<MeasurementPoint>)measurementPointService.UpdateMeasurementPointList_AddTrafficData(trafficDataDtos);

            // First element tested as an example
            Assert.NotEqual(measurementPointsFromUpdate[0].TrafficData, ((List<MeasurementPoint>)MockFactory.CreateMeasurementPointList())[0].TrafficData);
        }

        [Fact]
        public void DeleteMeasurementPoint_ValidId_ThrowsException()
        {
            measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());

            measurementPointService.DeleteMeasurementPoint(Constants.ValidId);

            Assert.ThrowsAsync<ArgumentNullException>(() => measurementPointService.GetMeasurementPoint(Constants.ValidId));
        }

        [Fact]
        public void DeleteMeasurementPoint_InvalidId_ThrowsException()
        {
            measurementPointService.AddMeasurementPoint(MockFactory.CreateMeasurementPoint());

            Assert.ThrowsAsync<ArgumentNullException>(() => measurementPointService.DeleteMeasurementPoint(Constants.InvalidId));
        }
    }
}