using System;
using System.Threading.Tasks;
using AutoMapper;
using Extract.TrafficData.Test.Mocks;
using Microsoft.Extensions.Configuration;
using TrafficDataBackEndAPI.BackEndApi.Data;
using TrafficDataBackEndAPI.BackEndApi.Extraction;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using TrafficDataBackEndAPI.BackEndApi.Services;
using TrafficDataBackEndAPI.BackEndApi.Tests.Mocks;
using Xunit;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.ExtractionTests
{
    public abstract class DataExtractorTest : IDisposable
    {
        protected readonly IConfiguration configuration;
        protected readonly Context context;
        protected readonly IMeasurementPointService measurementPointService;
        protected readonly IMapper mapper;
        protected DataExtractor dataExtractor;

        public DataExtractorTest()
        {
            configuration = ConfigurationMock.GetMockConfiguration();
            context = TestDBInitializer.CreateContext();
            TestDBInitializer.Initialize(context);
            mapper = MockMapper.CreateMapper();
            // The services cannot be mocked
            measurementPointService = new MeasurementPointService(context, mapper);
            dataExtractor = new DataExtractor(configuration, measurementPointService, mapper);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    
        [Fact]
        public async Task DataExtractor_MeasurementPoints_NotEmpty()
        {
            await dataExtractor.ExtractMeasurementPointsAsync();

            Assert.NotEmpty(await measurementPointService.GetMeasurementPoints());
        }
    
        [Fact]
        public async Task DataExtractor_TrafficData_NotEmptyAsync()
        {
            await dataExtractor.ExtractMeasurementPointsAsync();

            await dataExtractor.ExtractTrafficDataAsync();

            Assert.NotEmpty(measurementPointService.GetMeasurementPoint(Constants.ValidId).Result.TrafficData);
        }
    }
}