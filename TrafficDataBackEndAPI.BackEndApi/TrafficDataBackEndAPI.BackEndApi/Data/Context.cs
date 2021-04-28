using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrafficDataBackEndAPI.BackEndApi.Models;

namespace TrafficDataBackEndAPI.BackEndApi.Data {
    public class Context : DbContext
    {
        public DbSet<MeasurementPoint> MeasurementPoints {get; set;}
        public DbSet<TrafficData> TrafficData {get; set;}
        public DbSet<Metadata> Metadata {get; set;}

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relations
            modelBuilder.Entity<MeasurementPoint>()
                .HasMany(m => m.TrafficData)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // Required attributes for MeasurementPoint
            modelBuilder.Entity<MeasurementPoint>().Property(m => m.Reference).IsRequired(true);
            modelBuilder.Entity<MeasurementPoint>().Property(m => m.Latitude).IsRequired(true);
            modelBuilder.Entity<MeasurementPoint>().Property(m => m.Longitude).IsRequired(true);

            // Required attributes for TrafficData
            modelBuilder.Entity<TrafficData>().Property(t => t.DateTime).HasColumnType("timestamp");
            modelBuilder.Entity<TrafficData>().Property(t => t.DateTime).IsRequired(true);

            // Required attributes for Metadata
            modelBuilder.Entity<Metadata>().Property(m => m.State).IsRequired(true);
        }
    }
}