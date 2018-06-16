using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.AgentBooking.Pages
{
    public class BookDepartureSelectPage : DepartureSelectPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/chooseJourney/";

        public BookDepartureSelectPage(IBrowserSession session, ILogger<BookDepartureSelectPage> logger)
            : base(session, logger)
        {
        }
    }
}
