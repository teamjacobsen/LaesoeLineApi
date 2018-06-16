using LaesoeLineApi.CustomerBooking;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LaesoeLineApi.Tests.Features
{
    public class CustumerBookingSeasonPassTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly TestFixture.Credentials _credentials;

        public CustumerBookingSeasonPassTests(TestFixture fixture)
        {
            _fixture = fixture;
            _credentials = fixture.GetCredentials("SeasonPass");
        }

        [Fact]
        public async Task BookSeasonPassOneWay()
        {
            // Given
            _fixture.Api.SetAuthorization(_credentials.Username, _credentials.Password);

            // When
            var booking = await _fixture.Api.CustomerBooking.BookSeasonPassOneWayAsync(new CustomerBookingBookOneWay()
            {
                Journey = new Journey()
                {
                    Crossing = Crossing.LaesoeFrederikshavn,
                    Departure = DateTime.UtcNow.AddDays(20).Date.AddHours(6),
                    Vehicle = Vehicle.Car,
                    VehiclePassengers = 1
                },
                Local = true
            });

            // Then
            Assert.NotNull(booking.BookingNumber);
            Assert.NotNull(booking.BookingPassword);

            await CancelOrFailAsync(booking.BookingNumber);
        }

        [Fact]
        public async Task BookSeasonPassRoundTrip()
        {
            // Given
            _fixture.Api.SetAuthorization(_credentials.Username, _credentials.Password);

            // When
            var booking = await _fixture.Api.CustomerBooking.BookSeasonPassRoundTripAsync(new CustomerBookingBookRoundTrip()
            {
                Outbound = new Journey()
                {
                    Crossing = Crossing.LaesoeFrederikshavn,
                    Departure = DateTime.UtcNow.AddDays(21).Date.AddHours(6),
                    Vehicle = Vehicle.Car,
                    VehiclePassengers = 1
                },
                Return = new Journey()
                {
                    Crossing = Crossing.FrederikshavnLaesoe,
                    Departure = DateTime.UtcNow.AddDays(21).Date.AddHours(16).AddMinutes(50),
                    Vehicle = Vehicle.Car,
                    VehiclePassengers = 1
                },
                Local = true
            });

            // Then
            Assert.NotNull(booking.BookingNumber);
            Assert.NotNull(booking.BookingPassword);

            await CancelOrFailAsync(booking.BookingNumber);
        }

        //[Fact]
        [Fact(Skip = "No reservation to cancel")]
        public Task Cancel()
        {
            return CancelOrFailAsync("309847");
        }

        private async Task CancelOrFailAsync(string bookingNumber)
        {
            try
            {
                _fixture.Api.SetAuthorization(_credentials.Username, _credentials.Password);

                await _fixture.Api.CustomerBooking.CancelAsync(bookingNumber);
            }
            catch (Exception)
            {
                Assert.True(false, $"The booking {bookingNumber} was not cancelled");
            }
        }
    }
}
