using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YourProjectName.Domain.WeatherForecast;

namespace YourProjectName.Application.Infrastructure.Persistance;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    //Add DbSets here
    DbSet<WeatherForecastAggregate> Forecasts { get; }
}
