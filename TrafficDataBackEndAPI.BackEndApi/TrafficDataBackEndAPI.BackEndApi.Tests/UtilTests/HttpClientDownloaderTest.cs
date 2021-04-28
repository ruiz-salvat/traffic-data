using Extract.TrafficData.Test.Mocks;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TrafficDataBackEndAPI.BackEndApi.Tests.Mocks.HttpMessageHandlerMocks;
using TrafficDataBackEndAPI.BackEndApi.Util;
using Xunit;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.UtilTests
{
    public class HttpClientDownloaderTest
    {
        private HttpClientDownloader httpClientDownloaderTD;
        private HttpClientDownloader httpClientDownloaderMP;
        private IConfiguration configuration;

        public HttpClientDownloaderTest()
        {
            MPHttpMessageHandlerMock mpHttpMessageHandlerMock = new MPHttpMessageHandlerMock(ConfigurationMock.GetMockConfiguration());
            TDHttpMessageHandlerMock tdHttpMessageHandlerMock = new TDHttpMessageHandlerMock(ConfigurationMock.GetMockConfiguration());

            configuration = ConfigurationMock.GetMockConfiguration();
            httpClientDownloaderTD = new HttpClientDownloader(new HttpClient(
                tdHttpMessageHandlerMock.GetHttpClientMock(HttpStatusCode.OK)));
            httpClientDownloaderMP = new HttpClientDownloader(new HttpClient(
                mpHttpMessageHandlerMock.GetHttpClientMock(HttpStatusCode.OK)));
        }

        [Fact]
        public async Task DownloadFile_TrafficDataFile_ValidInputs_True()
        {
            for (int i = 1; i <= Constants.NumberOfRetries; ++i)
            {
                var result = await httpClientDownloaderTD.DownloadFile(
                    configuration.GetSection("URLs:TrafficDataURL").Value, configuration.GetSection("FilePaths:TrafficDataCompressedFilePath").Value);
                    
                if (result)    
                    Assert.True(result);
                else 
                    Thread.Sleep(Constants.DelayOnRetry);
            }
        }

        [Fact]
        public async Task DownloadFile_TrafficDataFile_InvalidURL_False()
        {
            for (int i = 1; i <= Constants.NumberOfRetries; ++i)
            {
                var result = await httpClientDownloaderTD.DownloadFile(
                    "Invalid URL", configuration.GetSection("FilePaths:TrafficDataCompressedFilePath").Value);

                if (result)
                    Assert.False(result);
                else
                    Thread.Sleep(Constants.DelayOnRetry);                
            }
        }

        [Fact]
        public async Task DownloadFile_MeasurementPointsFile_ValidInputs_True()
        {
            for (int i = 1; i <= Constants.NumberOfRetries; ++i)
            {
                var result = await httpClientDownloaderMP.DownloadFile(
                    configuration.GetSection("URLs:MeasurementPointsURL").Value, configuration.GetSection("FilePaths:MeasurementPointCompressedFilePath").Value);

                if (result)
                    Assert.True(result);
                else
                    Thread.Sleep(Constants.DelayOnRetry);
            }
        }

        [Fact]
        public async Task DownloadFile_MeasurementPointsFile_InvalidURL_False()
        {
            for (int i = 1; i <= Constants.NumberOfRetries; ++i)
            {
                var result = await httpClientDownloaderMP.DownloadFile(
                    "Invalid URL", configuration.GetSection("FilePaths:MeasurementPointCompressedFilePath").Value);

                if (result)
                    Assert.False(result);
                else
                    Thread.Sleep(Constants.DelayOnRetry);
            }
        }
    }
}
