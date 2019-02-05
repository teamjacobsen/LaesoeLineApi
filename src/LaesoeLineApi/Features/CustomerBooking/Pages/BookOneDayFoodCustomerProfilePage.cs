using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayFoodCustomerProfilePage : CustomerProfilePage
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/endagsmad/customerLogin";

        public BookOneDayFoodCustomerProfilePage(IBrowserSession session, ILogger<CustomerProfilePage> logger) : base(session, logger)
        {
        }
    }
}
