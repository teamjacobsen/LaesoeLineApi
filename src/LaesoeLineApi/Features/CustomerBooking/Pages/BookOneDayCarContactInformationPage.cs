using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayCarContactInformationPage : ContactInformationPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/golftur/customerInfo/";

        public BookOneDayCarContactInformationPage(IBrowserSession session, ILogger<BookOneDayContactInformationPage> logger)
            : base(session, logger)
        {
        }
    }
}
