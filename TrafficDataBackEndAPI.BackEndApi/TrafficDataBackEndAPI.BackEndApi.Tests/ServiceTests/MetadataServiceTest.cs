using System;
using System.Threading.Tasks;
using TrafficDataBackEndAPI.BackEndApi.Data;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using TrafficDataBackEndAPI.BackEndApi.Models;
using TrafficDataBackEndAPI.BackEndApi.Services;
using TrafficDataBackEndAPI.BackEndApi.Util;
using Xunit;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.ServiceTests
{
    public class MetadataServiceTest : IDisposable
    {
        private Context context;
        private IMetadataService metadataService;

        public MetadataServiceTest()
        {
            context = TestDBInitializer.CreateContext();
            TestDBInitializer.Initialize(context);
            metadataService = new MetadataService(context);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        [Fact]
        public void GetMetadata_State_Equal()
        {
            context.Metadata.Add(new Metadata{State = State.ExtractingTrafficData});
            context.SaveChanges();

            Assert.Equal(metadataService.GetMetadata().State, State.ExtractingTrafficData);
        }

        [Fact]
        public async Task UpdateMetadata_ValidState_EqualAsync()
        {
            context.Metadata.Add(new Metadata { State = State.ExtractingTrafficData });
            context.SaveChanges();

            await metadataService.UpdateMetadata(State.Aborted);

            Assert.Equal(metadataService.GetMetadata().State, State.Aborted);
        }

        [Fact]
        public async Task UpdateMetadata_InvalidState_ThrowExceptionAsync()
        {
            context.Metadata.Add(new Metadata { State = State.ExtractingTrafficData });
            context.SaveChanges();

            await Assert.ThrowsAsync<Exception>(() => metadataService.UpdateMetadata(Constants.InvalidState));
        }

        [Fact]
        public async Task UpdateMetadata_EmptyTable_ThrowExceptionAsync()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => metadataService.UpdateMetadata(State.ExtractingTrafficData));
        }
    }
}