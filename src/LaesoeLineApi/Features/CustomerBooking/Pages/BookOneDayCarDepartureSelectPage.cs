using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayCarDepartureSelectPage : DepartureSelectPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/golftur/chooseJourney/";

        public BookOneDayCarDepartureSelectPage(IBrowserSession session, ILogger<BookOneDayDepartureSelectPage> logger)
            : base(session, logger)
        {
        }
    }
}
