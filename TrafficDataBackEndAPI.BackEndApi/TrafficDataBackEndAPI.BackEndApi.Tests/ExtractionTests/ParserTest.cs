using System.Collections.Generic;
using TrafficDataBackEndAPI.BackEndApi.DTO;
using Xunit;
using TrafficDataBackEndAPI.BackEndApi.Data;
using AutoMapper;
using TrafficDataBackEndAPI.BackEndApi.Tests.Mocks;
using TrafficDataBackEndAPI.BackEndApi.Extraction.Parsers;
using Extract.TrafficData.Test.Mocks;
using System.Threading;
using System.IO;
using System;
using TrafficDataBackEndAPI.BackEndApi.Extraction;
using System.Net.Http;
using System.Net;
using TrafficDataBackEndAPI.BackEndApi.Tests.Mocks.HttpMessageHandlerMocks;
using System.Threading.Tasks;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.ExtractionTests
{
    public class ParserTest
    {
        private readonly MeasurementPointsParser measurementPointParser;
        private readonly TrafficDataParser trafficDataParser;
        private readonly Context context;
        private readonly IMapper mapper;
        private readonly MPHttpMessageHandlerMock mpHttpMessageHandler;
        private FileDownloader fileDownloader;

        public ParserTest()
        {
            context = TestDBInitializer.CreateContext();
            TestDBInitializer.Initialize(context);
            mapper = MockMapper.CreateMapper();
            var config = ConfigurationMock.GetMockConfiguration();
            measurementPointParser = new MeasurementPointsParser(config);
            trafficDataParser = new TrafficDataParser(config);
            mpHttpMessageHandler = new MPHttpMessageHandlerMock(ConfigurationMock.GetMockConfiguration());
            fileDownloader = new FileDownloader(ConfigurationMock.GetMockConfiguration(), new HttpClient());
        }

        private void populateContextWithMeasurementPoints()
        {
            IEnumerable<MeasurementPoint> measurementPoints = measurementPointParser.ParseMeasurementPoints();
            IEnumerable<Models.MeasurementPoint> measurementPointsModels = mapper.Map<IEnumerable<Models.MeasurementPoint>>(measurementPoints);
            context.MeasurementPoints.AddRange(measurementPointsModels);
            context.SaveChangesAsync();
        }

        private Dictionary<string, int> createDictionaryRefId()
        {
            Dictionary<string, int> refIds = new Dictionary<string, int>();
            populateContextWithMeasurementPoints();
            foreach (var measurementPoint in context.MeasurementPoints)
            {
                refIds[measurementPoint.Reference] = measurementPoint.Id;
            }
            return refIds;
        }

        /*
        Remarks 
        * The parser objects return lists of DTOs which have to be later mapped with the models when inserting data to the database
        * The traffic data parser needs a dictionary of the references of the measurement points matched with their IDs
        */

        [Fact]
        public async Task ParseMeasurementPoints_List_NotEmptyAsync()
        {
            await fileDownloader.DownloadMeasurementPointsFileAsync("Extraction/Files/parse_test_measurement.xml.gz");

            List<MeasurementPoint> measurementPoints = (List<MeasurementPoint>)measurementPointParser.ParseMeasurementPoints("Extraction/Files/parse_test_measurement.xml");

            Assert.NotEmpty(measurementPoints);       
        }

        [Fact]
        public async Task ParseTrafficData_List_NotEmptyAsync()
        {
            await fileDownloader.DownloadTrafficDataFileAsync("Extraction/Files/parse_test_trafficspeed.xml.gz");

            Dictionary<string, int> refIds = createDictionaryRefId();

            List<TrafficData> trafficData = (List<TrafficData>)trafficDataParser.ParseTrafficData(refIds, "Extraction/Files/parse_test_trafficspeed.xml");

            Assert.NotEmpty(trafficData);   
        }
    }
}