using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrafficDataBackEndAPI.BackEndApi.Data;

namespace TrafficDataBackEndAPI.BackEndApi.Tests
{
    public class TestDBInitializer
    {
        public static void Initialize(Context context)
        {
            context.Database.EnsureCreated();
        }

        public static Context CreateContext()
        {
            Context context = new Context(CreateNewEntityFrameworkContextOptions());
            context.Database.EnsureCreated();
            return context;
        }

        private static DbContextOptions<Context> CreateNewEntityFrameworkContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<Context>();
            builder.UseInMemoryDatabase(typeof(TestDBInitializer).Name)
                .EnableSensitiveDataLogging()
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}