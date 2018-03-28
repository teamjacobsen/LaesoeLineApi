using Refit;
using System.Threading.Tasks;

namespace LaesoeLineApi
{
    public interface ICustomerBookingApi
    {
        [Post("/CustomerBooking/Book/SeasonPass/OneWay")]
        Task<BookResult> BookSeasonPassOneWayAsync(CustomerBooking.CustomerBookingBookOneWay command);

        [Post("/CustomerBooking/Book/SeasonPass/RoundTrip")]
        Task<BookResult> BookSeasonPassRoundTripAsync(CustomerBooking.CustomerBookingBookRoundTrip command);

        [Delete("/CustomerBooking/Bookings/{bookingNumber}")]
        Task CancelAsync(string bookingNumber);
    }
}