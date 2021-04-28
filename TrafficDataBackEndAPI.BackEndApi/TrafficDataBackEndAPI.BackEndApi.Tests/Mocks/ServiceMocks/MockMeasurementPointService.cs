using System;
using Moq;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.Mocks.ServiceMocks
{
    public class MockMeasurementPointService
    {
        public static IMeasurementPointService GetMeasurementPointServiceAsync()
        {
            Mock<IMeasurementPointService> mockMeasurementPointService = new Mock<IMeasurementPointService>();

            // Just included the services used by the controller
            mockMeasurementPointService.Setup(s => s.GetMeasurementPoints()).Returns(MockFactory.CreateMeasurementPointListAsync());
            mockMeasurementPointService.Setup(s => s.GetMeasurementPoint(Constants.ValidId)).Returns(MockFactory.CreateMeasurementPointAsync());
            mockMeasurementPointService.Setup(s => s.GetMeasurementPoint(Constants.InvalidId)).Throws(new ArgumentNullException());
            mockMeasurementPointService.Setup(s => s.AddMeasurementPoint(MockFactory.CreateMeasurementPoint())).Returns(MockFactory.CreateMeasurementPointAsync());
            mockMeasurementPointService.Setup(s => s.AddMeasurementPoint(null)).Throws(new ArgumentNullException());
            mockMeasurementPointService.Setup(s => s.UpdateMeasurementPoint(MockFactory.CreateMeasurementPoint())).Returns(MockFactory.CreateMeasurementPointAsync());
            mockMeasurementPointService.Setup(s => s.UpdateMeasurementPoint(null)).Throws(new ArgumentNullException());
            mockMeasurementPointService.Setup(s => s.DeleteMeasurementPoint(Constants.ValidId));
            mockMeasurementPointService.Setup(s => s.DeleteMeasurementPoint(Constants.InvalidId)).Throws(new ArgumentNullException());

            return mockMeasurementPointService.Object;
        }
    }
}
