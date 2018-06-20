using LaesoeLineApi.CustomerBooking;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LaesoeLineApi.Tests.Features
{
    public class CustumerBookingOneDayTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly TestFixture.Credentials _credentials;

        public CustumerBookingOneDayTests(TestFixture fixture)
        {
            _fixture = fixture;
            _credentials = fixture.GetCredentials("OneDay");
        }

        [Fact]
        public async Task Book()
        {
            // Given
            _fixture.Api.SetAuthorization(_credentials.Username, _credentials.Password);

            // When
            var booking = await _fixture.Api.CustomerBooking.BookOneDayAsync(new CustomerBookingBookOneDay()
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
                    Departure = DateTime.UtcNow.AddDays(21).Date.AddHours(6),
                    Vehicle = Vehicle.None,
                    Adults = 1
                },
                Return = new Journey()
                {
                    Crossing = Crossing.FrederikshavnLaesoe,
                    Departure = DateTime.UtcNow.AddDays(21).Date.AddHours(16).AddMinutes(50),
                    Vehicle = Vehicle.None,
                    Adults = 1
                }
            });

            // Then
            Assert.NotNull(booking.BookingNumber);
            Assert.NotNull(booking.BookingPassword);

            await CancelOrFailAsync(booking.BookingNumber, booking.BookingPassword);
        }

        //[Fact]
        [Fact(Skip = "No reservation to cancel")]
        public Task Cancel()
        {
            return CancelOrFailAsync("339780", "VPCA");
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
