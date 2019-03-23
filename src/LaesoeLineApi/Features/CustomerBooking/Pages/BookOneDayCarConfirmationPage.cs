using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayCarConfirmationPage : BookingConfirmationPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/golftur/bookingConfirmation/";

        public BookOneDayCarConfirmationPage(IBrowserSession session, ILogger<BookOneDayConfirmationPage> logger)
            : base(session, logger)
        {
        }
    }
}
