using LaesoeLineApi.AgentBooking;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LaesoeLineApi.Tests.Features
{
    public class AgentBookingItTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly TestFixture.Credentials _crendentials;

        public AgentBookingItTests(TestFixture fixture)
        {
            _fixture = fixture;
            _crendentials = fixture.GetCredentials("It");
        }

        [Fact]
        public async Task BookItRoundTrip_Passengers()
        {
            // Given
            _fixture.Api.SetAuthorization(_crendentials.Username, _crendentials.Password);

            // When
            var booking = await _fixture.Api.AgentBooking.BookItRoundTripAsync(new AgentBookingBookRoundTrip()
            {
                Customer = new Customer()
                {
                    FirstName = "Læsø Pakkerejser",
                    LastName = "Rasmus Jacobsen",
                    Email = "rmja@laesoe.org",
                    PhoneNumber = "+4520285909"
                },
                Outbound = new Journey()
                {
                    Crossing = Crossing.LaesoeFrederikshavn,
                    Departure = DateTime.UtcNow.AddDays(20).Date.AddHours(6),
                    Adults = 1
                },
                Return = new Journey()
                {
                    Crossing = Crossing.FrederikshavnLaesoe,
                    Departure = DateTime.UtcNow.AddDays(20).Date.AddHours(16).AddMinutes(50),
                    Adults = 1
                }
            });

            // Then
            Assert.NotNull(booking.BookingNumber);
            Assert.NotNull(booking.BookingPassword);
            Assert.True(booking.TotalPrice > 0);

            await CancelOrFailAsync(booking.BookingNumber, booking.BookingPassword);
        }

        [Fact]
        public async Task BookItRoundTrip_Vehicle()
        {
            // Given
            _fixture.Api.SetAuthorization(_crendentials.Username, _crendentials.Password);

            // When
            var booking = await _fixture.Api.AgentBooking.BookItRoundTripAsync(new AgentBookingBookRoundTrip()
            {
                Customer = new Customer()
                {
                    FirstName = "Læsø Pakkerejser",
                    LastName = "Rasmus Jacobsen",
                    Email = "rmja@laesoe.org",
                    PhoneNumber = "+4520285909"
                },
                Outbound = new Journey()
                {
                    Crossing = Crossing.LaesoeFrederikshavn,
                    Departure = DateTime.UtcNow.AddDays(20).Date.AddHours(6),
                    Vehicle = Vehicle.Car,
                    VehiclePassengers = 1
                },
                Return = new Journey()
                {
                    Crossing = Crossing.FrederikshavnLaesoe,
                    Departure = DateTime.UtcNow.AddDays(20).Date.AddHours(16).AddMinutes(50),
                    Vehicle = Vehicle.Car,
                    VehiclePassengers = 1
                }
            });

            // Then
            Assert.NotNull(booking.BookingNumber);
            Assert.NotNull(booking.BookingPassword);
            Assert.True(booking.TotalPrice > 0);

            await CancelOrFailAsync(booking.BookingNumber, booking.BookingPassword);
        }

        private async Task CancelOrFailAsync(string bookingNumber, string bookingPassword)
        {
            try
            {
                await _fixture.Api.MyBooking.CancelAsync(bookingNumber, bookingPassword);
            }
            catch (Exception)
            {
                Assert.True(false, $"The booking {bookingNumber}/{bookingPassword} was not cancelled");
            }
        }
    }
}
