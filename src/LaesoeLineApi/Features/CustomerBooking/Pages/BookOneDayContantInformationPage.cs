using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayContantInformationPage : ContactInformationPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/endagspakke/customerInfo/";

        public BookOneDayContantInformationPage(IBrowserSession session, ILogger<BookOneDayContantInformationPage> logger)
            : base(session, logger)
        {
        }
    }
}
