using System.Threading.Tasks;
using Extract.TrafficData.Test.Mocks;
using Microsoft.AspNetCore.Mvc;
using TrafficDataBackEndAPI.BackEndApi.Controllers;
using TrafficDataBackEndAPI.BackEndApi.Extraction;
using TrafficDataBackEndAPI.BackEndApi.Extraction.Pattern;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using TrafficDataBackEndAPI.BackEndApi.Tests.Mocks;
using TrafficDataBackEndAPI.BackEndApi.Tests.Mocks.ServiceMocks;
using Xunit;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.ControllerTests
{
    public class DataExtractionControllerTest
    {
        private readonly DataExtractionController dataExtractionController;
        private readonly IMetadataService metadataService;

        public DataExtractionControllerTest()
        {
            IMeasurementPointService measurementPointService = MockMeasurementPointService.GetMeasurementPointServiceAsync();
            ITrafficDataService trafficDataService = MockTrafficDataService.GetTrafficDataServiceAsync();
            DataExtractor dataExtractor = new DataExtractor(ConfigurationMock.GetMockConfiguration(), measurementPointService, MockMapper.CreateMapper());
            metadataService = MockMetadataService.GetMetadataService();
            DataEngine dataEngine = new DataEngine(ConfigurationMock.GetMockConfiguration(), dataExtractor, metadataService, trafficDataService);
            dataExtractionController = new DataExtractionController(dataEngine);
        }

        [Fact]
        public void GetState_State_Ok()
        { 
            IActionResult result = dataExtractionController.GetState();
            
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task RetrieveMeasurementPointsAsync_Ok()
        {
            IActionResult result = await dataExtractionController.RetrieveMeasurementPointsAsync();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DoAction_NullJObject_BadRequestAsync()
        {
            IActionResult result = await dataExtractionController.DoActionAsync(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DoAction_BadInputFormat_BadRequestAsync()
        {
            IActionResult result = await dataExtractionController.DoActionAsync("Bad input format");

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}