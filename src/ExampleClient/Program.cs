using ExampleClient.Api;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ExampleClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:51059/", UriKind.Absolute)
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{args[0]}:{args[1]}")));

            var api = new LaesoeLineApi(client);
            var departures = await api.Timetable.GetDepartures(Crossing.LaesoeFrederikshavn, days: 3);

            foreach (var departure in departures)
            {
                Console.WriteLine(departure);
            }

            var booking = await api.CustomerBooking.BookSeasonPassOneWay(new CustomerBookingBookOneWay()
            {
                Journey = new CustomerBookingJourney()
                {
                    Crossing = Crossing.LaesoeFrederikshavn,
                    Departure = new DateTime(2018, 5, 2, 6, 0, 0),
                    Passengers = 1,
                    Vehicle = VehicleType.Car
                }
            });

            await api.CustomerBooking.CancelAsync(booking.BookingNumber);

            Console.ReadLine();
        }
    }
}
