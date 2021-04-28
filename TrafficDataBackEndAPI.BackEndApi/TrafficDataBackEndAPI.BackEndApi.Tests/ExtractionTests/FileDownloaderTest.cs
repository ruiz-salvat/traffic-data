using Extract.TrafficData.Test.Mocks;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using TrafficDataBackEndAPI.BackEndApi.Extraction;
using TrafficDataBackEndAPI.BackEndApi.Tests.Mocks.HttpMessageHandlerMocks;
using Xunit;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.ExtractionTests
{
    public class FileDownloaderTest
    {
        private readonly MPHttpMessageHandlerMock mpHttpMessageHandler;
        private readonly TDHttpMessageHandlerMock tdHttpMessageHandlerMock;

        public FileDownloaderTest()
        {
            mpHttpMessageHandler = new MPHttpMessageHandlerMock(ConfigurationMock.GetMockConfiguration());
            tdHttpMessageHandlerMock = new TDHttpMessageHandlerMock(ConfigurationMock.GetMockConfiguration());
        }

        [Fact]
        public void DownloadMeasurementPointsFileAsync_MeasurementPoints_OkAsync()
        {
            for (int i = 1; i <= Constants.NumberOfRetries; ++i)
            {
                FileDownloader fileDownloader = new FileDownloader(ConfigurationMock.GetMockConfiguration(),
                    new HttpClient(mpHttpMessageHandler.GetHttpClientMock(HttpStatusCode.OK)));

                var result = fileDownloader.DownloadMeasurementPointsFileAsync();

                if (result.Result)
                    Assert.True(result.Result);
                else
                    Thread.Sleep(Constants.DelayOnRetry);
            }
        }

        [Fact]
        public void DownloadTrafficDataFileAsync_TrafficData_OkAsync()
        {
            for (int i = 1; i <= Constants.NumberOfRetries; ++i)
            {
                try
                {
                    FileDownloader fileDownloader = new FileDownloader(ConfigurationMock.GetMockConfiguration(),
                        new HttpClient(tdHttpMessageHandlerMock.GetHttpClientMock(HttpStatusCode.OK)));

                    var result = fileDownloader.DownloadTrafficDataFileAsync();

                    Assert.True(result.Result);
                }
                catch (IOException e) when (i <= Constants.NumberOfRetries)
                {
                    Console.WriteLine(e.StackTrace);
                    Thread.Sleep(Constants.DelayOnRetry);
                }
            }
        }
    }
}