using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.AgentBooking.Pages
{
    public class BookContantInformationPage : ContactInformationPageBase
    {
        private readonly IBrowserSession _session;
        private readonly ILogger<BookContantInformationPage> _logger;

        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/customerInfo/";

        public BookContantInformationPage(IBrowserSession session, ILogger<BookContantInformationPage> logger)
            : base(session, logger)
        {
            _session = session;
            _logger = logger;
        }
    }
}
