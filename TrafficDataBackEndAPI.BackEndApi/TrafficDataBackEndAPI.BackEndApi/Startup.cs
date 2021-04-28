using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrafficDataBackEndAPI.BackEndApi.Data;
using TrafficDataBackEndAPI.BackEndApi.Extraction;
using TrafficDataBackEndAPI.BackEndApi.Extraction.Pattern;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;
using TrafficDataBackEndAPI.BackEndApi.Services;

namespace TrafficDataBackEndAPI.BackEndApi
{
    public class Startup
    {
        public IConfiguration configuration {get;}

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(configuration);
            string connectionString = configuration.GetSection("ConnectionStrings:PostGresConnectionString").Value;
            
            services.AddDbContext<Context>(options => options.UseNpgsql(connectionString), ServiceLifetime.Transient);
            services.Configure<InitializationOptions>(configuration.GetSection("Initialization"));

            services.AddLogging();
            services.AddScoped<DbInitializer>();
            services.AddScoped<DataExtractor>();
            services.AddScoped<DataEngine>();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddScoped<IMeasurementPointService, MeasurementPointService>();
            services.AddScoped<ITrafficDataService, TrafficDataService>();
            services.AddScoped<IMetadataService, MetadataService>();

            services.AddCors(options => {
                options.AddPolicy("AllowSpecificOrigin", policy => {
                    policy.WithOrigins(configuration.GetSection("Web:SchemeAndHost").Value)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowSpecificOrigin");

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}