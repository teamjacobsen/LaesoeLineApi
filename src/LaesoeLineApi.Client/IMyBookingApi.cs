using Refit;
using System.Threading.Tasks;

namespace LaesoeLineApi
{
    public interface IMyBookingApi
    {
        [Delete("/MyBooking/Bookings/{bookingNumber}")]
        Task CancelAsync(string bookingNumber, string bookingPassword);
    }
}