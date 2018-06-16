using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.AgentBooking.Pages
{
    public class BookContantInformationPage : ContactInformationPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/customerInfo/";

        public BookContantInformationPage(IBrowserSession session, ILogger<BookContantInformationPage> logger)
            : base(session, logger)
        {
        }
    }
}
