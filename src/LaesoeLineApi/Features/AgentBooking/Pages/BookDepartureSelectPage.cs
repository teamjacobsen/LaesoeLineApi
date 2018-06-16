using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.AgentBooking.Pages
{
    public class BookDepartureSelectPage : DepartureSelectPageBase
    {
        private readonly IBrowserSession _session;
        private readonly ILogger<BookDepartureSelectPage> _logger;

        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/chooseJourney/";

        public BookDepartureSelectPage(IBrowserSession session, ILogger<BookDepartureSelectPage> logger)
            : base(session, logger)
        {
            _session = session;
            _logger = logger;
        }
    }
}
