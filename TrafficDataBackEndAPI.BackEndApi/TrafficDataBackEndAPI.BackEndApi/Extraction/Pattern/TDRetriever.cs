using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using TrafficDataBackEndAPI.BackEndApi.Util;

namespace TrafficDataBackEndAPI.BackEndApi.Extraction.Pattern
{
    public class TDRetriever
    {
        private readonly int refreshingTime;
        private readonly int retentionInDays;
        private readonly IMetadataService metadataService;
        private readonly DataExtractor dataExtractor;
        private readonly ITrafficDataService trafficDataService;
        private static bool active;

        public TDRetriever(IConfiguration configuration, DataExtractor dataExtractor, IMetadataService metadataService, ITrafficDataService trafficDataService)
        {
            this.dataExtractor = dataExtractor;
            this.metadataService = metadataService;
            this.trafficDataService = trafficDataService;
            refreshingTime = Int32.Parse(configuration.GetSection("TimeLimits:RefreshingTime").Value);
            retentionInDays = Int32.Parse(configuration.GetSection("TimeLimits:RetentionInDays").Value);
        }

        public async Task RunAsync()
        {
            TDRetriever.active = true;
            while (TDRetriever.active)
            {
                Console.WriteLine("-> Starting traffic data extraction");

                await metadataService.UpdateMetadata(State.ExtractingTrafficData);

                await dataExtractor.ExtractTrafficDataAsync();

                Console.WriteLine("-> Traffic data extraction done");

                Console.WriteLine("-> Deleting obsolete data");

                await trafficDataService.DeleteTrafficData(DateTime.Now.AddDays(-retentionInDays));

                Console.WriteLine("-> Obsolete data deleted");

                if (TDRetriever.active)
                {
                    await metadataService.UpdateMetadata(State.Sleeping); 
                    Thread.Sleep(refreshingTime);
                }
            }
            await metadataService.UpdateMetadata(State.Aborted);
            Console.WriteLine("-> Process aborted");
        }

        public static void Abort()
        {
            TDRetriever.active = false;
            Console.WriteLine("-> Aborting process...");
        }
    }
}