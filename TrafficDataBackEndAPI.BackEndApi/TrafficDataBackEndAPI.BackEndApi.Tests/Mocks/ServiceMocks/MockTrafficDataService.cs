using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using Moq;
using System;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.Mocks.ServiceMocks
{
    public class MockTrafficDataService
    {
        public static ITrafficDataService GetTrafficDataServiceAsync()
        {
            Mock<ITrafficDataService> mockTrafficDataService = new Mock<ITrafficDataService>();

            mockTrafficDataService.Setup(s => s.GetAllTrafficData()).Returns(MockFactory.CreateTrafficDataListAsync());
            mockTrafficDataService.Setup(s => s.GetTrafficDataByMeasurementPointId(Constants.ValidId)).Returns(MockFactory.CreateTrafficDataListAsync());
            mockTrafficDataService.Setup(s => s.GetTrafficDataByMeasurementPointId(Constants.InvalidId)).Throws(new Exception());
            mockTrafficDataService.Setup(s => s.GetTrafficDataTimeInterval(Constants.StartDatetime, Constants.EndDatetime)).Returns(MockFactory.CreateTrafficDataListAsync());
            mockTrafficDataService.Setup(s => s.GetTrafficDataTimeInterval(Constants.EndDatetime, Constants.StartDatetime)).Throws(new Exception());
            mockTrafficDataService.Setup(s => s.GetTrafficDataTimeIntervalByMeasurementPointId(Constants.ValidId, Constants.StartDatetime, Constants.EndDatetime))
                .Returns(MockFactory.CreateTrafficDataListAsync());
            mockTrafficDataService.Setup(s => s.GetTrafficDataTimeIntervalByMeasurementPointId(Constants.InvalidId, Constants.StartDatetime, Constants.EndDatetime))
                .Throws(new Exception());
            mockTrafficDataService.Setup(s => s.GetTrafficDataTimeIntervalByMeasurementPointId(Constants.ValidId, Constants.EndDatetime, Constants.StartDatetime))
                .Throws(new Exception());
            mockTrafficDataService.Setup(s => s.GetTrafficDataTimeIntervalByMeasurementPointId(Constants.ValidId, Constants.MockDatetime, Constants.MockDatetime))
                .Returns(MockFactory.CreateTrafficDataListSingleRowAsync());
            mockTrafficDataService.Setup(s => s.AddTrafficData(MockFactory.CreateTrafficDataList())).Returns(MockFactory.CreateTrafficDataListAsync());
            mockTrafficDataService.Setup(s => s.AddTrafficData(null)).Throws(new ArgumentNullException());
            mockTrafficDataService.Setup(s => s.AddTrafficDataRow(MockFactory.CreateTrafficDataRow())).Returns(MockFactory.CreateTrafficDataRowAsync());
            mockTrafficDataService.Setup(s => s.AddTrafficDataRow(null)).Throws(new ArgumentNullException());
            mockTrafficDataService.Setup(s => s.UpdateTrafficData(MockFactory.CreateTrafficDataList())).Returns(MockFactory.CreateTrafficDataListAsync());
            mockTrafficDataService.Setup(s => s.UpdateTrafficData(null)).Throws(new ArgumentNullException());
            mockTrafficDataService.Setup(s => s.UpdateTrafficDataRow(MockFactory.CreateTrafficDataRow())).Returns(MockFactory.CreateTrafficDataRowAsync());
            mockTrafficDataService.Setup(s => s.UpdateTrafficDataRow(null)).Throws(new ArgumentNullException());
            mockTrafficDataService.Setup(s => s.DeleteTrafficData(MockFactory.CreateIdList(5)));
            mockTrafficDataService.Setup(s => s.DeleteTrafficData(null)).Throws(new ArgumentNullException());
            mockTrafficDataService.Setup(s => s.DeleteTrafficDataRow(Constants.ValidId));
            mockTrafficDataService.Setup(s => s.DeleteTrafficDataRow(Constants.InvalidId)).Throws(new Exception());
            mockTrafficDataService.Setup(s => s.DeleteTrafficData(DateTime.Now));

            return mockTrafficDataService.Object;
        }
    }
}