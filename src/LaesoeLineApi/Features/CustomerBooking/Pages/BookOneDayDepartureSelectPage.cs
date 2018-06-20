using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayDepartureSelectPage : DepartureSelectPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/endagspakke/chooseJourney/";

        public BookOneDayDepartureSelectPage(IBrowserSession session, ILogger<BookOneDayDepartureSelectPage> logger)
            : base(session, logger)
        {
        }
    }
}
