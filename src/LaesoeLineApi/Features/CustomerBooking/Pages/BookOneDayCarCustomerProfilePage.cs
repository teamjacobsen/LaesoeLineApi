using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayCarCustomerProfilePage : CustomerProfilePage
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/golftur/customerLogin";

        public BookOneDayCarCustomerProfilePage(IBrowserSession session, ILogger<CustomerProfilePage> logger) : base(session, logger)
        {
        }
    }
}
