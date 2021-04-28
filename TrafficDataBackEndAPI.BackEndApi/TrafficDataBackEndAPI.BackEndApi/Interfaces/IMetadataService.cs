using System.Threading.Tasks;
using TrafficDataBackEndAPI.BackEndApi.Models;

namespace TrafficDataBackEndAPI.BackEndApi.Interfaces
{
    public interface IMetadataService
    {
        public Metadata GetMetadata();
        public Task<bool> UpdateMetadata(int state);
    }
}