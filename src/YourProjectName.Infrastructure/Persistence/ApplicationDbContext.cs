using System.Reflection;
using Microsoft.EntityFrameworkCore;
using YourProjectName.Application.Infrastructure.Persistance;
using YourProjectName.Domain.WeatherForecast;

namespace YourProjectName.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        //Set your DB sets here, put them in the IApplicationDbContext interface first
        public DbSet<WeatherForecastAggregate> Forecasts { get; set; }
    }
}
