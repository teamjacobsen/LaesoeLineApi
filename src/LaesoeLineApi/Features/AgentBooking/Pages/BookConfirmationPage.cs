using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.AgentBooking.Pages
{
    public class BookConfirmationPage : BookingConfirmationPageBase
    {
        private readonly IBrowserSession _session;
        private readonly ILogger<BookConfirmationPage> _logger;

        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/bookingConfirmation/";

        public BookConfirmationPage(IBrowserSession session, ILogger<BookConfirmationPage> logger)
            : base(session, logger)
        {
            _session = session;
            _logger = logger;
        }
    }
}
