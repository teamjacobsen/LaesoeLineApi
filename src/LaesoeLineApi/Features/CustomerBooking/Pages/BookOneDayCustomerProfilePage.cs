using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayCustomerProfilePage : CustomerProfilePage
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/endagspakke/customerLogin";

        public BookOneDayCustomerProfilePage(IBrowserSession session, ILogger<CustomerProfilePage> logger) : base(session, logger)
        {
        }
    }
}
