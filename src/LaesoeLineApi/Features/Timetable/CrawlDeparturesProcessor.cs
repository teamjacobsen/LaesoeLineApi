using LaesoeLineApi.Features.Timetable.Pages;
using LaesoeLineApi.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.Timetable
{
    public class CrawlDeparturesProcessor
    {
        private readonly IBrowserSessionFactory _browserSessionFactory;
        private readonly DepartureCache _cache;

        public CrawlDeparturesProcessor(IBrowserSessionFactory browserSessionFactory, DepartureCache cache)
        {
            _browserSessionFactory = browserSessionFactory;
            _cache = cache;
        }

        public async Task SyncDeparturesAsync(Crossing crossing, DateTime date, int days, CancellationToken cancellationToken = default)
        {
            var vehicleResults = new Dictionary<DateTime, DepartureInfo[]>();

            var vehicles = ((Vehicle[])Enum.GetValues(typeof(Vehicle))).Where(x => x.GetAttribute().IncludeInAvailability);

            await Task.WhenAll(vehicles.Select(async vehicle =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var session = _browserSessionFactory.CreateSession())
                {
                    var bookingDetails = await session.GoToAsync<BookingDetailsPage>();
                    await bookingDetails.EnterDetailsAsync(crossing, vehicle, date);

                    var departureSelect = await session.GoToAsync<DepartureSelectPage>();
                    var departures = await departureSelect.GetDeparturesAsync(date, days, cancellationToken);

                    lock (vehicleResults)
                    {
                        PatchAvailability(vehicleResults, vehicle, departures);
                    }
                }
            }));

            await Task.WhenAll(vehicleResults.Select(x => _cache.SetDeparturesAsync(crossing, x.Key, x.Value, cancellationToken)));
        }

        private void PatchAvailability(IDictionary<DateTime, DepartureInfo[]> master, Vehicle vehicle, IDictionary<DateTime, (DateTime Departure, VehicleAvailabilityInfo Availability)[]> patch)
        {
            if (master.Count == 0)
            {
                foreach (var pair in patch)
                {
                    master[pair.Key] = pair.Value.Select(x => new DepartureInfo()
                    {
                        Departure = x.Departure,
                        Availability = new Dictionary<Vehicle, VehicleAvailabilityInfo>()
                        {
                            { vehicle, x.Availability }
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
