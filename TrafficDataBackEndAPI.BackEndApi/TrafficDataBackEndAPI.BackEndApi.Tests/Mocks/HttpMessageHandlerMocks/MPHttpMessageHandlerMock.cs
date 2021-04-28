using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.Mocks.HttpMessageHandlerMocks
{
    public class MPHttpMessageHandlerMock : HttpMessageHandlerMock
    {
        private const string confSection = "FilePaths:MeasurementPointCompressedFilePath";

        public MPHttpMessageHandlerMock(IConfiguration configuration) : base(configuration)
        { 
        }

        public override HttpMessageHandler GetHttpClientMock(HttpStatusCode httpStatusCode)
        {
            return GetHttpClientMockByConf(httpStatusCode, confSection);
        }
    }
}
