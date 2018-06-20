using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayConfirmationPage : BookingConfirmationPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/endagspakke/bookingConfirmation/";

        public BookOneDayConfirmationPage(IBrowserSession session, ILogger<BookOneDayConfirmationPage> logger)
            : base(session, logger)
        {
        }
    }
}
