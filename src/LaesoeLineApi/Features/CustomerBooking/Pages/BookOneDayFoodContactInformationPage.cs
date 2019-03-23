using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayFoodContactInformationPage : ContactInformationPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/endagsmad/customerInfo/";

        public BookOneDayFoodContactInformationPage(IBrowserSession session, ILogger<BookOneDayFoodContactInformationPage> logger)
            : base(session, logger)
        {
        }
    }
}
