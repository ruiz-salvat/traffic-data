using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TrafficDataBackEndAPI.BackEndApi.Data
{
    public class DbInitializer
    {
        private readonly Context context;
        private readonly InitializationOptions options;

        public DbInitializer(Context context, IOptionsMonitor<InitializationOptions> subOptionsAccessor)
        {
            this.context = context;
            options = subOptionsAccessor.CurrentValue;
        }

        public virtual void Initialize()
        {
            UpdateSchema();

            // Seed the database
            if (options.SeedDatabase)
            {
                DbSeeder dbSeeder = new DbSeeder(context);
                dbSeeder.Seed();
            }

            if (options.SeedDatabase)
                context.SaveChanges();
        }

        public virtual void UpdateSchema()
        {
            if (context.Database.IsInMemory())
                context.Database.EnsureCreated();
            else 
                context.Database.Migrate();
        }
    }
}