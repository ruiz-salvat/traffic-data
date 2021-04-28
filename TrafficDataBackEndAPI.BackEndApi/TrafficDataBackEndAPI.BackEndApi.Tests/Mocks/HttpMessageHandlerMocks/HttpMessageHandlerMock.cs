using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.Mocks.HttpMessageHandlerMocks
{
    public abstract class HttpMessageHandlerMock
    {
        private IConfiguration configuration;

        protected HttpMessageHandlerMock(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public abstract HttpMessageHandler GetHttpClientMock(HttpStatusCode httpStatusCode);

        protected HttpMessageHandler GetHttpClientMockByConf(HttpStatusCode httpStatusCode, string confSection)
        {
            Mock<HttpMessageHandler> messageHandlerMock = new Mock<HttpMessageHandler>();

            messageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(CreateHttpResponseMessage(httpStatusCode, configuration.GetSection(confSection).Value));

            return messageHandlerMock.Object;
        }

        private HttpResponseMessage CreateHttpResponseMessage(HttpStatusCode httpStatusCode, string filePath)
        {
            Byte[] byteArray;
            FileStream fileStream = new FileInfo(filePath).OpenRead();
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                byteArray = ms.ToArray();
            }


            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(byteArray),
                StatusCode = httpStatusCode
            };

            return responseMessage;
        }
    }
}
