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
        private static readonly TimeZoneInfo EuropeCopenhagen = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

        public CrawlDeparturesHostedService(CrawlDeparturesProcessor processor, IOptions<TimetableOptions> options)
        {
            _processor = processor;
            _options = options;
        }

        //  Runs when webserver starts
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            var isFirstRun = true;
            while (!cancellationToken.IsCancellationRequested)
            {
                var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, EuropeCopenhagen);

                int days = GetDays(now, isFirstRun);
                isFirstRun = false;

                foreach (var crossing in (Crossing[])Enum.GetValues(typeof(Crossing)))
                {
                    await _processor.SyncDeparturesAsync(crossing, now.Date, days, cancellationToken);
                }

                await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
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