using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using TrafficDataBackEndAPI.BackEndApi.Extraction.Parsers;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using TrafficDataBackEndAPI.BackEndApi.Models;

namespace TrafficDataBackEndAPI.BackEndApi.Extraction
{
    public class DataExtractor
    {
        private const string fileDownloaderError = "Error downloading the file";
        private readonly IMeasurementPointService measurementPointService;
        private readonly FileDownloader fileDownloader;
        private readonly MeasurementPointsParser measurementPointsParser;
        private readonly TrafficDataParser trafficDataParser;
        private readonly IMapper mapper;

        public DataExtractor(IConfiguration configuration, IMeasurementPointService measurementPointService, IMapper mapper)
        {
            this.measurementPointService = measurementPointService;
            fileDownloader = new FileDownloader(configuration, new HttpClient());
            measurementPointsParser = new MeasurementPointsParser(configuration);
            trafficDataParser = new TrafficDataParser(configuration);
            this.mapper = mapper;
        }

        // Extracts the measurement points and stores them in the database
        // Then it creates a dictionary which matches the references with the Ids of the measurement points
        public async Task ExtractMeasurementPointsAsync()
        {
            if (await fileDownloader.DownloadMeasurementPointsFileAsync())
            {
                IEnumerable<DTO.MeasurementPoint> measurementPointDTOs = measurementPointsParser.ParseMeasurementPoints();
                IEnumerable<MeasurementPoint> measurementPoints = mapper.Map<IEnumerable<MeasurementPoint>>(measurementPointDTOs);
                measurementPointService.AddMeasurementPoints(measurementPoints);
            }
        }

        // Extracts the traffic data and stores it in the database
        public async Task ExtractTrafficDataAsync()
        {
            if (await fileDownloader.DownloadTrafficDataFileAsync())
            {
                IEnumerable<DTO.TrafficData> trafficDataDTO = trafficDataParser.ParseTrafficData(DictRefIds());
                measurementPointService.UpdateMeasurementPointList_AddTrafficData(trafficDataDTO);
            }
            else
            {
                throw new Exception(fileDownloaderError);
            }
        }

        private Dictionary<string, int> DictRefIds()
        {
            IEnumerable<MeasurementPoint> measurementPoints = measurementPointService.GetMeasurementPointsSync();

            Dictionary<string, int> refIds = new Dictionary<string, int>();
            foreach (var measurementPoint in measurementPoints)
            {
                refIds[measurementPoint.Reference] = measurementPoint.Id;
            }
            
            return refIds;
        } 
    }
}