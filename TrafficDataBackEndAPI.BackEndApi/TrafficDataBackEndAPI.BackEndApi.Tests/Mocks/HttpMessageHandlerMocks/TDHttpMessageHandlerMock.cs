using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.Mocks.HttpMessageHandlerMocks
{
    public class TDHttpMessageHandlerMock : HttpMessageHandlerMock
    {
        private const string confSection = "FilePaths:TrafficDataCompressedFilePath";

        public TDHttpMessageHandlerMock(IConfiguration configuration) : base(configuration)
        {
        }

        public override HttpMessageHandler GetHttpClientMock(HttpStatusCode httpStatusCode)
        {
            return GetHttpClientMockByConf(httpStatusCode, confSection);
        }
    }
}
