using Refit;
using System.Threading.Tasks;

namespace LaesoeLineApi
{
    public interface IAgentBookingApi
    {
        [Post("/AgentBooking/Book/It/RoundTrip")]
        Task<BookResult> BookItRoundTripAsync(AgentBooking.AgentBookingBookRoundTrip command);
    }
}
