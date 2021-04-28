using System.Threading.Tasks;
using Moq;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using TrafficDataBackEndAPI.BackEndApi.Models;
using TrafficDataBackEndAPI.BackEndApi.Util;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.Mocks.ServiceMocks
{
    public class MockMetadataService
    {
        public static IMetadataService GetMetadataService()
        {
            Mock<IMetadataService> mockMetadataService = new Mock<IMetadataService>();

            mockMetadataService.Setup(s => s.GetMetadata()).Returns(new Metadata{State = State.ExtractingTrafficData});
            mockMetadataService.Setup(s => s.UpdateMetadata(State.MeasurementPointsNotRetrieved)).Returns(new Task<bool>(() => true));
            mockMetadataService.Setup(s => s.UpdateMetadata(State.MeasurementPointsRetrieved)).Returns(new Task<bool>(() => true));
            mockMetadataService.Setup(s => s.UpdateMetadata(State.ExtractingTrafficData)).Returns(new Task<bool>(() => true));
            mockMetadataService.Setup(s => s.UpdateMetadata(State.Sleeping)).Returns(new Task<bool>(() => true));
            mockMetadataService.Setup(s => s.UpdateMetadata(State.Aborted)).Returns(new Task<bool>(() => true));
            mockMetadataService.Setup(s => s.UpdateMetadata(State.Resumed)).Returns(new Task<bool>(() => true));
            mockMetadataService.Setup(s => s.UpdateMetadata(State.Aborting)).Returns(new Task<bool>(() => true));

            return mockMetadataService.Object;
        }
    }
}