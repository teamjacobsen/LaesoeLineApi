using LaesoeLineApi.Features.Timetable.Models;
using LaesoeLineApi.Features.Timetable.Pages;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.Timetable
{
    public class CrawlDeparturesProcessor
    {
        private readonly IServiceProvider _services;
        private readonly DepartureCache _cache;

        public CrawlDeparturesProcessor(IServiceProvider services, DepartureCache cache)
        {
            _services = services;
            _cache = cache;
        }

        public Task SyncDeparturesAsync(Crossing crossing, DateTime date, int days, CancellationToken cancellationToken = default)
        {
            var vehicles = new[] { Vehicle.Car };

            var vehicleResults = new Dictionary<DateTime, DepartureInfo[]>();

            Parallel.ForEach(vehicles, vehicle =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var driver = _services.GetRequiredService<IWebDriver>())
                {
                    var bookPage = driver.GoTo<BookPage>();

                    var departures = bookPage.GetDepartures(crossing, vehicle, date, days);

                    lock (vehicleResults)
                    {
                        PatchAvailability(vehicleResults, vehicle, departures);
                    }
                }
            });

            return Task.WhenAll(vehicleResults.Select(x => _cache.SetDeparturesAsync(crossing, x.Key, x.Value, cancellationToken)));
        }

        private void PatchAvailability(IDictionary<DateTime, DepartureInfo[]> master, Vehicle vehicle, IDictionary<DateTime, (DateTime Departure, bool IsAvailable)[]> patch)
        {
            if (master.Count == 0)
            {
                foreach (var pair in patch)
                {
                    master[pair.Key] = pair.Value.Select(x => new DepartureInfo()
                    {
                        Departure = x.Departure,
                        Availability = new Dictionary<Vehicle, bool>()
                        {
                            { vehicle, x.IsAvailable }
                        }
                    }).ToArray();
                }
            }
            else
            {
                foreach (var pair in patch)
                {
                    foreach (var (departure, isAvailable) in pair.Value)
                    {
                        master[pair.Key].Single(x => x.Departure == departure).Availability[vehicle] = isAvailable;
                    }
                }
            }
        }
    }
}
