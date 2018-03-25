using OpenQA.Selenium;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class CustomerProfilePage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/customer-profile/";
        public IWebDriver Driver { get; private set; }

        // Login
        private static class Login
        {
            public static readonly By Username = By.Id("cw-login-customer-customerCode");
            public static readonly By Password = By.Id("cw-login-customer-password");
            public static readonly By Submit = By.CssSelector("button[type=submit]");

            public static readonly By CustomerLogoutButton = By.ClassName("cw-do-customerlogout");
        }

        // Cancel
        private static class Cancel
        {
            public static readonly By BookingsTable = By.ClassName("cw-bookings");

            public static By CancelLink(string bookingNumber) => By.CssSelector($"a.cw-booking-cancel[data-cw-action-link*=\"{bookingNumber}\"");
            public static readonly By CancelPopupMessageDiv = By.CssSelector("div.fancybox-inner > div.cancel-booking-message");
            public static readonly By ConfirmButton = By.CssSelector("div.fancybox-dialog-buttons > button:last-child");
        }

        public CustomerProfilePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public async Task LoginAsync(string username, string password)
        {
            await Driver.FindVisibleElementAsync(Login.Username).ThenSendKeys(username);
            await Driver.FindVisibleElementAsync(Login.Password).ThenSendKeys(password);
            await Driver.FindVisibleElementAsync(Login.Submit).ThenClick();

            await Driver.WaitForElementToAppearAsync(Login.CustomerLogoutButton);
        }

        public async Task<bool> CancelAsync(string bookingNumber)
        {
            // Wait for login to actually redirect to the customer profile page
            await Driver.WaitForElementToAppearAsync(Cancel.BookingsTable);

            var cancelLink = await Driver.FindVisibleElementAsync(Cancel.CancelLink(bookingNumber));
            if (cancelLink == null)
            {
                return false;
            }
            cancelLink.Click();

            await Driver.WaitForElementToAppearAsync(Cancel.CancelPopupMessageDiv);
            await Driver.FindVisibleElementAsync(Cancel.ConfirmButton).ThenClick();
            await Driver.WaitForTitleContainsAsync("Booking annulleret");

            return true;
        }
    }
}
