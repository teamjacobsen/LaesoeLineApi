using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayContactInformationPage : ContactInformationPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/endagspakke/customerInfo/";

        public BookOneDayContactInformationPage(IBrowserSession session, ILogger<BookOneDayContactInformationPage> logger)
            : base(session, logger)
        {
        }
    }
}
