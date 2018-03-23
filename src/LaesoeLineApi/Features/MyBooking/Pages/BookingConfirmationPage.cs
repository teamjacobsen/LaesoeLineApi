using OpenQA.Selenium;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.MyBooking.Pages
{
    public class BookingConfirmationPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/bookingConfirmation/";
        public IWebDriver Driver { get; private set; }

        private static readonly By CancelBookingButton = By.ClassName("cw-action-cancelBooking");
        private static readonly By CancelPopupMessageDiv = By.CssSelector("div.fancybox-inner > div.cancel-booking-message");
        private static readonly By ConfirmButton = By.CssSelector("div.fancybox-dialog-buttons > button:last-child");

        public BookingConfirmationPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public async Task CancelAsync()
        {
            await Driver.FindVisibleElementAsync(CancelBookingButton).ThenClick();
            await Driver.WaitForElementToAppearAsync(CancelPopupMessageDiv);
            await Driver.FindVisibleElementAsync(ConfirmButton).ThenClick();
            await Driver.WaitForTitleContainsAsync("Booking annulleret");
        }
    }
}
