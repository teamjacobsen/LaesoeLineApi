using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookSeasonPassDepartureSelectPage : DepartureSelectPageBase
    {
        private readonly IBrowserSession _session;
        private readonly ILogger<BookSeasonPassDepartureSelectPage> _logger;

        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/aarskort-2018/chooseJourney/";

        public BookSeasonPassDepartureSelectPage(IBrowserSession session, ILogger<BookSeasonPassDepartureSelectPage> logger)
            : base(session, logger)
        {
            _session = session;
            _logger = logger;
        }
    }
}
