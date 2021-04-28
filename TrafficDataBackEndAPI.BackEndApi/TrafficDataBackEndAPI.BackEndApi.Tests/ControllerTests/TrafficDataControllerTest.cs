using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TrafficDataBackEndAPI.BackEndApi.Controllers;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using TrafficDataBackEndAPI.BackEndApi.Tests.Mocks;
using TrafficDataBackEndAPI.BackEndApi.Tests.Mocks.ServiceMocks;
using Xunit;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.ControllerTests
{
    public class TrafficDataControllerTest
    {
        private readonly TrafficDataController trafficDataController;
        private readonly ITrafficDataService trafficDataService;
        private readonly IMeasurementPointService measurementPointService;
        private readonly IMapper mapper;

        public TrafficDataControllerTest()
        {
            trafficDataService = MockTrafficDataService.GetTrafficDataServiceAsync();
            measurementPointService = MockMeasurementPointService.GetMeasurementPointServiceAsync();
            mapper = MockMapper.CreateMapper();
            var logger = new MockLogger<TrafficDataController>().CreateLogger();
            trafficDataController = new TrafficDataController(trafficDataService, measurementPointService, mapper, logger);
        }

        [Fact]
        public async Task GetAllTrafficData_OkAsync()
        {
            IActionResult result = await trafficDataController.GetAllTrafficData();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetTrafficDataByMeasurementPointId_ValidId_OkAsync()
        {
            IActionResult result = await trafficDataController.GetTrafficDataByMeasurementPointId(Constants.ValidId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetTrafficDataByMeasurementPointId_InvalidId_NotFound()
        {
            IActionResult result = await trafficDataController.GetTrafficDataByMeasurementPointId(Constants.InvalidId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetTrafficDataTimeInterval_ValidDates_Ok()
        {
            IActionResult result = await trafficDataController.GetTrafficDataTimeInterval(Constants.StartDatetimeString, Constants.EndDatetimeString);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllTrafficDataTimeInterval_InvalidDates_NotFound()
        {
            IActionResult result = await trafficDataController.GetTrafficDataTimeInterval(Constants.EndDatetimeString, Constants.StartDatetimeString);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetTrafficDataTimeIntervalByMeasurementPointId_ValidInputs_Ok()
        {
            IActionResult result = await trafficDataController
                .GetTrafficDataTimeIntervalByMeasurementPointId(Constants.ValidId, Constants.StartDatetimeString, Constants.EndDatetimeString);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetTrafficDataTimeIntervalByMeasurementPointId_InvalidId_NotFound()
        {
            IActionResult result = await trafficDataController
                .GetTrafficDataTimeIntervalByMeasurementPointId(Constants.InvalidId, Constants.StartDatetimeString, Constants.EndDatetimeString);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetTrafficDataTimeIntervalByMeasurementPointId_InvalidDates_NotFound()
        {
            IActionResult result = await trafficDataController
                .GetTrafficDataTimeIntervalByMeasurementPointId(Constants.ValidId, Constants.EndDatetimeString, Constants.StartDatetimeString);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddTrafficDataEndPoint_OkRow_Ok()
        {
            IActionResult result = await trafficDataController.AddTrafficDataEndPoint(MockFactory.CreateTrafficDataRowStringDto());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddTrafficDataEndPoint_OkList_Ok()
        {
            IActionResult result = await trafficDataController.AddTrafficDataEndPoint(MockFactory.CreateTrafficDataListStringDto());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddTrafficDataEndPoint_BadInputFormat_BadRequest()
        {
            IActionResult result = await trafficDataController.AddTrafficDataEndPoint("This is a bad input format");

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTrafficDataEndPoint_OkRow_Ok()
        {
            IActionResult result = await trafficDataController.UpdateTrafficDataEndPoint(MockFactory.CreateTrafficDataRowStringDto());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTrafficDataEndPoint_OkList_Ok()
        {
            IActionResult result = await trafficDataController.UpdateTrafficDataEndPoint(MockFactory.CreateTrafficDataListStringDto());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTrafficDataEndPoint_BadInputFormat_BadRequest()
        {
            IActionResult result = await trafficDataController.UpdateTrafficDataEndPoint("This is a bad input format");

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTrafficDataTimeInterval_ValidDates_Ok()
        {
            IActionResult result = await trafficDataController.DeleteTrafficDataTimeInterval(Constants.StartDatetimeString, Constants.EndDatetimeString);

            Assert.IsType<OkResult>(result); 
        }

        [Fact]
        public async Task DeleteTrafficDataTimeInterval_InvalidDates_BadRequest()
        {
            IActionResult result = await trafficDataController.DeleteTrafficDataTimeInterval(Constants.EndDatetimeString, Constants.StartDatetimeString);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTrafficDataTimeIntervalByMeasurementPointId_ValidInputs_Ok()
        {
            IActionResult result = await trafficDataController.DeleteTrafficDataTimeIntervalByMeasurementPointId(Constants.ValidId, Constants.StartDatetimeString, Constants.EndDatetimeString);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteTrafficDataTimeIntervalByMeasurementPointId_InvalidId_NotFound()
        {
            IActionResult result = await trafficDataController.DeleteTrafficDataTimeIntervalByMeasurementPointId(Constants.InvalidId, Constants.StartDatetimeString, Constants.EndDatetimeString);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTrafficDataTimeIntervalByMeasurementPointId_InvalidDates_NotFound()
        {
            IActionResult result = await trafficDataController.DeleteTrafficDataTimeIntervalByMeasurementPointId(Constants.ValidId, Constants.EndDatetimeString, Constants.StartDatetimeString);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
