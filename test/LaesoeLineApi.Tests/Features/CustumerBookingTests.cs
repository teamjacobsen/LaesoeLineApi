using System;
using System.Threading.Tasks;
using Xunit;

namespace LaesoeLineApi.Tests.Features
{
    public class CustumerBookingTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public CustumerBookingTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task BookSeasonPassOneWay()
        {
            // Given
            _fixture.Api.SetAuthorization(_fixture.CustomerUsername, _fixture.CustomerPassword);

            // When
            var booking = await _fixture.Api.CustomerBooking.BookSeasonPassOneWay(new CustomerBookingBookOneWay()
            {
                Journey = new CustomerBookingJourney()
                {
                    Crossing = Crossing.LaesoeFrederikshavn,
                    Departure = DateTime.UtcNow.AddDays(20).Date.AddHours(6),
                    Passengers = 1,
                    Vehicle = VehicleType.Car
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
            _fixture.Api.SetAuthorization(_fixture.CustomerUsername, _fixture.CustomerPassword);

            // When
            var booking = await _fixture.Api.CustomerBooking.BookSeasonPassRoundTrip(new CustomerBookingBookRoundTrip()
            {
                Outbound = new CustomerBookingJourney()
                {
                    Crossing = Crossing.LaesoeFrederikshavn,
                    Departure = DateTime.UtcNow.AddDays(20).Date.AddHours(6),
                    Passengers = 1,
                    Vehicle = VehicleType.Car
                },
                Return = new CustomerBookingJourney()
                {
                    Crossing = Crossing.FrederikshavnLaesoe,
                    Departure = DateTime.UtcNow.AddDays(20).Date.AddHours(16).AddMinutes(50),
                    Passengers = 1,
                    Vehicle = VehicleType.Car
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
                _fixture.Api.SetAuthorization(_fixture.CustomerUsername, _fixture.CustomerPassword);

                await _fixture.Api.CustomerBooking.CancelAsync(bookingNumber);
            }
            catch (Exception)
            {
                Assert.True(false, $"The booking {bookingNumber} was not cancelled");
            }
        }
    }
}
