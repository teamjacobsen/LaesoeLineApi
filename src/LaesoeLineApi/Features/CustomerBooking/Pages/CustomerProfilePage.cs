using OpenQA.Selenium;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class CustomerProfilePage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/customer-profile/";
        public IWebDriver Driver { get; private set; }

        // Login
        private static readonly By UsernameSelector = By.Id("cw-login-customer-customerCode");
        private IWebElement Username => Driver.FindVisibleElement(UsernameSelector);
        private IWebElement Password => Driver.FindVisibleElement(By.Id("cw-login-customer-password"));
        private IWebElement Submit => Driver.FindVisibleElement(By.CssSelector("button[type=submit]"));

        private static readonly By BookingsTable = By.ClassName("cw-bookings");

        // Cancel
        private IWebElement CancelLink(string bookingNumber) => Driver.FindVisibleElement(By.CssSelector($"a.cw-booking-cancel[data-cw-action-link*=\"{bookingNumber}\""));
        private static readonly By CancelPopupMessageDivSelector = By.CssSelector("div.fancybox-inner > div.cancel-booking-message");
        private IWebElement ConfirmCancelButton => Driver.FindVisibleElement(By.CssSelector("div.fancybox-dialog-buttons > button:last-child"));

        public CustomerProfilePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void Login(string username, string password)
        {
            // Wait for login to show
            Driver.WaitForElementToAppear(UsernameSelector);

            Username.SendKeys(username);
            Password.SendKeys(password);
            Submit.Click();
        }

        public bool Cancel(string bookingNumber)
        {
            // Wait for login to actually redirect to the customer profile page
            Driver.WaitForElementToAppear(BookingsTable);

            var cancelLink = CancelLink(bookingNumber);
            if (cancelLink == null)
            {
                return false;
            }
            cancelLink.Click();

            Driver.WaitForElementToAppear(CancelPopupMessageDivSelector);
            ConfirmCancelButton.Click();
            Driver.WaitForTitleContains("Booking annulleret");

            return true;
        }
    }
}
