using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayFoodDepartureSelectPage : DepartureSelectPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/endagsmad/chooseJourney/";

        public BookOneDayFoodDepartureSelectPage(IBrowserSession session, ILogger<BookOneDayFoodDepartureSelectPage> logger)
            : base(session, logger)
        {
        }
    }
}
