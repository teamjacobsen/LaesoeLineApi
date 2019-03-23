using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookSeasonPassContactInformationPage : ContactInformationPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/aarskort-2018/customerInfo/";

        public BookSeasonPassContactInformationPage(IBrowserSession session, ILogger<BookSeasonPassContactInformationPage> logger)
            : base(session, logger)
        {
        }
    }
}
