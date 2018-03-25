using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.Timetable
{
    public class CrawlDeparturesHostedService : HostedService
    {
        private readonly CrawlDeparturesProcessor _processor;
        private readonly IOptions<TimetableOptions> _options;
        private readonly ILogger<CrawlDeparturesHostedService> _logger;

        public CrawlDeparturesHostedService(CrawlDeparturesProcessor processor, IOptions<TimetableOptions> options, ILogger<CrawlDeparturesHostedService> logger)
        {
            _processor = processor;
            _options = options;
            _logger = logger;
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            var crossings = (Crossing[])Enum.GetValues(typeof(Crossing));

            var isFirstRun = true;
            while (!cancellationToken.IsCancellationRequested)
            {
                var now = LaesoeTime.Now;
                var nowDate = now.Date;

                int days = GetDays(now, isFirstRun);
                isFirstRun = false;

                try
                {
                    foreach (var crossing in crossings)
                    {
                        await _processor.SyncDeparturesAsync(crossing, nowDate, days, cancellationToken);

                        _logger.LogInformation("Successfully synchronized departures for {Crossing} from {Date} for {Days} days", crossing, nowDate, days);
                    }
                }
                catch (TaskCanceledException)
                {
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Departure synchronization failed from {Date} for {Days} days", nowDate, days);
                }

                try
                {
                    await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
                }
                catch (TaskCanceledException)
                {
                }
            }
        }

        private int GetDays(DateTime now, bool isFirstRun)
        {
            if (isFirstRun)
            {
                return _options.Value.InitialCrawlDays;
            }
            else if (now.Hour == 0)
            {
                return _options.Value.DailyCrawlDays;
            }
            else
            {
                return _options.Value.HourlyCrawlDays;
            }
        }
    }
}