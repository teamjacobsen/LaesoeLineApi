using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayFoodContantInformationPage : ContactInformationPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/endagsmad/customerInfo/";

        public BookOneDayFoodContantInformationPage(IBrowserSession session, ILogger<BookOneDayFoodContantInformationPage> logger)
            : base(session, logger)
        {
        }
    }
}
