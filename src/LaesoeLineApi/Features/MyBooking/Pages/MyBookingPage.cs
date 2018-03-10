using OpenQA.Selenium;

namespace LaesoeLineApi.Features.MyBooking.Pages
{
    public class MyBookingPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/my-booking/";
        public IWebDriver Driver { get; private set; }

        private static readonly By BookingNumberSelector = By.Id("cw-login-booking-code");
        private IWebElement BookingNumber => Driver.FindVisibleElement(BookingNumberSelector);
        private IWebElement BookingPassword => Driver.FindVisibleElement(By.Id("cw-login-booking-password"));
        private IWebElement Submit => Driver.FindVisibleElement(By.CssSelector("button[type=submit]"));

        public MyBookingPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void Login(string bookingNumber, string bookingPassword)
        {
            // Wait for login to show
            Driver.WaitForElementToAppear(BookingNumberSelector);

            BookingNumber.SendKeys(bookingNumber);
            BookingPassword.SendKeys(bookingPassword);
            Submit.Click();
        }
    }
}