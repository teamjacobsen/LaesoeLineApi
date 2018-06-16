using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookSeasonPassDepartureSelectPage : DepartureSelectPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/aarskort-2018/chooseJourney/";

        public BookSeasonPassDepartureSelectPage(IBrowserSession session, ILogger<BookSeasonPassDepartureSelectPage> logger)
            : base(session, logger)
        {
        }
    }
}
