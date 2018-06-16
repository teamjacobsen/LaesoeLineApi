using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayFoodConfirmationPage : BookingConfirmationPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/endagsmad/bookingConfirmation/";

        public BookOneDayFoodConfirmationPage(IBrowserSession session, ILogger<BookOneDayFoodConfirmationPage> logger)
            : base(session, logger)
        {
        }
    }
}
