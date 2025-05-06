using Microsoft.EntityFrameworkCore;
using YourProjectName.Domain.WeatherForecast;

namespace YourProjectName.Infrastructure.Persistence.Repository;
internal class WeatherForecastRepository(ApplicationDbContext applicationDbContext) : IWeatherForecastRepository
{
    public async Task<List<WeatherForecastAggregate>> GetWeatherForecasts(int? temperatureRangeMin, int? temperatureRangeMax)
    {
        var query = applicationDbContext.Forecasts.AsNoTracking();

        if (temperatureRangeMin.HasValue)
        {
            query = query.Where(x => x.TemperatureC >= temperatureRangeMin.Value);
        }

        if (temperatureRangeMax.HasValue)
        {
            query = query.Where(x => x.TemperatureC <= temperatureRangeMax.Value);
        }

        return await query.ToListAsync();
    }
}
