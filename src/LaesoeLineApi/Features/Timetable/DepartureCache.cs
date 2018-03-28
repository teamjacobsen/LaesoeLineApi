using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.Timetable
{
    public class DepartureCache
    {
        private readonly IDistributedCache _cache;

        private static readonly DistributedCacheEntryOptions EntryOptions = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        };

        public DepartureCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<DepartureInfo[]> GetDeparturesAsync(Crossing crossing, DateTime date, int days, bool allowIncompleteDays, CancellationToken cancellationToken = default)
        {
            var hits = await Task.WhenAll(
                Enumerable.Range(0, days)
                    .Select(x => date.AddDays(x))
                    .Select(x => _cache.GetStringAsync(ComputeKey(crossing, x), cancellationToken)));

            if (!allowIncompleteDays && hits.Any(x => x == null))
            {
                return null;
            }

            var entries = hits.Where(x => x != null).Select(json => JsonConvert.DeserializeObject<Entry>(json)).OrderBy(x => x.Date);

            var departures = entries.SelectMany(x => x.Departures).OrderBy(x => x.Departure).ToArray();

            var vehicles = (Vehicle[])Enum.GetValues(typeof(Vehicle));
            var expectedNumberOfVehicles = vehicles.Count(x => x.GetAttribute().IncludeInAvailability);
            if (departures.Any(x => x.Availability.Count != expectedNumberOfVehicles))
            {
                return null;
            }

            return departures;
        }

        public Task SetDeparturesAsync(Crossing crossing, DateTime date, DepartureInfo[] departures, CancellationToken cancellationToken = default)
        {
            var json = JsonConvert.SerializeObject(new Entry()
            {
                Date = date,
                Departures = departures
            });

            return _cache.SetStringAsync(ComputeKey(crossing, date), json, EntryOptions, cancellationToken);
        }

        private static string ComputeKey(Crossing crossing, DateTime date) => $"crossings:{crossing}:departures:{date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";

        private class Entry
        {
            public DateTime Date { get; set; }
            public DepartureInfo[] Departures { get; set; }
        }
    }
}
