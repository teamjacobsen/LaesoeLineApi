using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookSeasonPassContantInformationPage : ContactInformationPageBase
    {
        private readonly IBrowserSession _session;
        private readonly ILogger<BookSeasonPassContantInformationPage> _logger;

        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/aarskort-2018/customerInfo/";

        public BookSeasonPassContantInformationPage(IBrowserSession session, ILogger<BookSeasonPassContantInformationPage> logger)
            : base(session, logger)
        {
            _session = session;
            _logger = logger;
        }
    }
}
