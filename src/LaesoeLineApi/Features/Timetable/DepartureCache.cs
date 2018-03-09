﻿using LaesoeLineApi.Features.Timetable.Models;
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

        public async Task<DepartureInfo[]> GetDeparturesAsync(Crossing crossing, DateTime date, int days = 1, CancellationToken cancellationToken = default)
        {
            var hits = await Task.WhenAll(
                Enumerable.Range(0, days)
                    .Select(x => date.AddDays(x))
                    .Select(x => _cache.GetStringAsync(ComputeKey(crossing, x), cancellationToken)));

            if (hits.Any(x => x == null))
            {
                return null;
            }

            var entries = hits.Select(json => JsonConvert.DeserializeObject<Entry>(json)).OrderBy(x => x.Date);

            return entries.SelectMany(x => x.Departures).ToArray();
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
