using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookSeasonPassCustomerProfilePage : CustomerProfilePage
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/aarskort-2018/customerLogin";

        public BookSeasonPassCustomerProfilePage(IBrowserSession session, ILogger<CustomerProfilePage> logger) : base(session, logger)
        {
        }
    }
}
