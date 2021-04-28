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
    public class MeasurementPointControllerTest
    {

        private readonly MeasurementPointController measurementPointController;
        private readonly IMeasurementPointService measurementPointService;
        private readonly IMapper mapper;

        public MeasurementPointControllerTest()
        {
            measurementPointService = MockMeasurementPointService.GetMeasurementPointServiceAsync();
            mapper = MockMapper.CreateMapper();
            var logger = new MockLogger<MeasurementPointController>().CreateLogger();
            measurementPointController = new MeasurementPointController(measurementPointService, mapper, logger);
        }

        [Fact]
        public async Task GetMeasurementPoints_OkAsync()
        {
            IActionResult result = await measurementPointController.GetMeasurementPoints();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetMeasurementPoint_ValidId_OkAsync()
        {
            IActionResult result = await measurementPointController.GetMeasurementPoint(Constants.ValidId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetMeasurementPoint_InvalidId_BadRequestAsync()
        {
            IActionResult result = await measurementPointController.GetMeasurementPoint(Constants.InvalidId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddMeasurementPoint_NewMeasurementPointDto_OkAsync()
        {
            IActionResult result = await measurementPointController.AddMeasurementPoint(MockFactory.CreateMeasurementPointDto());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddMeasurementPoint_NullDto_BadRequestAsync()
        {
            IActionResult result = await measurementPointController.AddMeasurementPoint(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateMeasurementPoint_NewMeasurementPointDto_OkAsync()
        {
            DTO.MeasurementPoint measurementPointDto = MockFactory.CreateMeasurementPointDto();
            measurementPointDto.Id = Constants.ValidId;

            IActionResult result = await measurementPointController.UpdateMeasurementPoint(measurementPointDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateMeasurementPoint_NullDto_BadRequestAsync()
        {
            IActionResult result = await measurementPointController.UpdateMeasurementPoint(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteMeasurementPoint_ValidId_OkAsync()
        {
            IActionResult result = await measurementPointController.DeleteMeasurementPoint(Constants.ValidId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteMeasurementPoint_InvalidId_BadRequestAsync()
        {
            IActionResult result = await measurementPointController.DeleteMeasurementPoint(Constants.InvalidId);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
