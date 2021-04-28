using System;
using System.Linq;
using System.Threading.Tasks;
using TrafficDataBackEndAPI.BackEndApi.Data;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using TrafficDataBackEndAPI.BackEndApi.Models;
using TrafficDataBackEndAPI.BackEndApi.Util;

namespace TrafficDataBackEndAPI.BackEndApi.Services
{
    public class MetadataService:IMetadataService
    {
        private readonly Context context;
        // There is just one row of metadata in the database with Id 1
        private const int metadataId = 1;

        public MetadataService(Context context)
        {
            this.context = context;
        }

        public Metadata GetMetadata()
        {
            return context.Metadata.Where(m => m.Id == metadataId).SingleOrDefault();
        }

        public async Task<bool> UpdateMetadata(int state)
        {
            if (state < State.MeasurementPointsNotRetrieved || state > State.Aborting)
                throw new Exception("Invalid state");

            try 
            {
                Metadata metadataFromDB = context.Metadata.Where(m => m.Id == metadataId).Single();
                metadataFromDB.State = state;
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}