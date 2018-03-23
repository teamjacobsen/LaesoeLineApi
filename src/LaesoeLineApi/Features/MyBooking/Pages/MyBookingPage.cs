using OpenQA.Selenium;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.MyBooking.Pages
{
    public class MyBookingPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/my-booking/";
        public IWebDriver Driver { get; private set; }

        private static readonly By BookingNumberText = By.Id("cw-login-booking-code");
        private static readonly By BookingPasswordText = By.Id("cw-login-booking-password");
        private static readonly By SubmitButton = By.CssSelector("button[type=submit]");

        public MyBookingPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public async Task LoginAsync(string bookingNumber, string bookingPassword)
        {
            await Driver.FindVisibleElementAsync(BookingNumberText).ThenSendKeys(bookingNumber);
            await Driver.FindVisibleElementAsync(BookingPasswordText).ThenSendKeys(bookingPassword);
            await Driver.FindVisibleElementAsync(SubmitButton).ThenClick();
        }
    }
}