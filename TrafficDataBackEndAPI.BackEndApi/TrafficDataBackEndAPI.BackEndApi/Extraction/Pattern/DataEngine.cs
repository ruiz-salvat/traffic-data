using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using TrafficDataBackEndAPI.BackEndApi.Util;

namespace TrafficDataBackEndAPI.BackEndApi.Extraction.Pattern
{
    // This class manages the data updates to the database
    public class DataEngine
    {
        private readonly DataExtractor dataExtractor;
        private readonly IMetadataService metadataService;
        private TDRetriever tdRetriever;

        public DataEngine(IConfiguration configuration, DataExtractor dataExtractor, IMetadataService metadataService, ITrafficDataService trafficDataService)
        {
            this.dataExtractor = dataExtractor;
            this.metadataService = metadataService;
            tdRetriever = new TDRetriever(configuration, dataExtractor, metadataService, trafficDataService);
        }

        public async Task RunAsync()
        {
            await tdRetriever.RunAsync();
        }

        public async Task InsertMeasurementPointsToDBAsync()
        {
            Console.WriteLine("-> Starting measurement points extraction");
            await dataExtractor.ExtractMeasurementPointsAsync();
            metadataService.UpdateMetadata(State.MeasurementPointsRetrieved).RunSynchronously();
        }

        public int GetState()
        {
            return metadataService.GetMetadata().State;
        }

        public async Task SetStateAsync(int state)
        {
            await metadataService.UpdateMetadata(state);
            if (state.Equals(State.Aborting))
                TDRetriever.Abort();
        }
    }
}