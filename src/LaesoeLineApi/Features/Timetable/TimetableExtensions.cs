using LaesoeLineApi.Features.Timetable;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TimetableExtensions
    {
        public static IServiceCollection AddTimetableFeature(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton<CrawlDeparturesProcessor>()
                .AddSingleton<DepartureCache>()
                .AddSingleton<IHostedService, CrawlDeparturesHostedService>()
                .Configure<TimetableOptions>(configuration);
        }
    }
}
