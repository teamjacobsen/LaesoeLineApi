using LaesoeLineApi;
using LaesoeLineApi.CustomerBooking;
using LaesoeLineApi.Timetable;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ExampleClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var api = new LaesoeLineApiClient("http://localhost:51059/");
            api.SetAuthorization(args[0], args[1]);

            var departures = await api.Timetable.GetDeparturesAsync(Crossing.LaesoeFrederikshavn, days: 3);

            foreach (var departure in departures)
            {
                Console.WriteLine(FormatDeparture(departure));
            }

            var booking = await api.CustomerBooking.BookSeasonPassOneWayAsync(new CustomerBookingBookOneWay()
            {
                Journey = new Journey()
                {
                    Crossing = Crossing.LaesoeFrederikshavn,
                    Departure = new DateTime(2018, 5, 2, 6, 0, 0),
                    Vehicle = Vehicle.Car,
                    VehiclePassengers = 1
                }
            });

            await api.CustomerBooking.CancelAsync(booking.BookingNumber);
        }

        private static string FormatDeparture(DepartureInfo departureInfo)
        {
            var builder = new StringBuilder();

            builder.AppendFormat("{0} {1}: ", departureInfo.Departure.ToShortDateString(), departureInfo.Departure.ToShortTimeString());

            foreach (var availability in departureInfo.Availability)
            {
                if (availability.Value.IsAvailable)
                {
                    builder.AppendFormat("\t{0} (available)", availability.Key);
                }
                else
                {
                    builder.AppendFormat("\t{0} (sold out)", availability.Key);
                }
            }

            return builder.ToString();
        }
    }
}
