using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.AgentBooking.Pages
{
    public class BookConfirmationPage : BookingConfirmationPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/bookingConfirmation/";

        public BookConfirmationPage(IBrowserSession session, ILogger<BookConfirmationPage> logger)
            : base(session, logger)
        {
        }
    }
}
