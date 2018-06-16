using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class CustomerProfilePage : IPage
    {
        private readonly IBrowserSession _session;
        private readonly ILogger<CustomerProfilePage> _logger;

        public string Url { get; } = "https://booking.laesoe-line.dk/dk/customer-profile/";

        public CustomerProfilePage(IBrowserSession session, ILogger<CustomerProfilePage> logger)
        {
            _session = session;
            _logger = logger;
        }

        public async Task LoginAsync(string username, string password)
        {
            var ok = false;

            for (var i = 0; i < 5; i++)
            {
                try
                {
                    if (await _session.InvokeAsync(x => x.FindElements(Login.CustomerLogoutButton).Count > 0))
                    {
                        ok = true;
                        break;
                    }

                    await _session.InvokeOnElementAsync(Login.Username, x => x.SendKeys(username));
                    await _session.InvokeOnElementAsync(Login.Password, x => x.SendKeys(password));
                    await _session.InvokeOnElementAsync(Login.Submit, x => x.Click());

                    await _session.WaitForAnyElementToAppearAsync(Login.CustomerLogoutButton, Login.AgentLogoutButton);

                    ok = true;
                    break;
                }
                catch (WebDriverTimeoutException e)
                {
                    _logger.LogWarning(e, "Timeout");

                    await _session.GoToAsync(Url);
                }
            }

            if (!ok)
            {
                throw new ApiException(ApiStatus.GatewayTimeout);
            }
        }

        public async Task<bool> CancelAsync(string bookingNumber)
        {
            // Wait for login to actually redirect to the customer profile page
            await _session.WaitForElementToAppearAsync(Cancel.BookingsTable);

            var clicked = await _session.TryInvokeOnElementAsync(Cancel.CancelLink(bookingNumber), x => x.Click());

            if (!clicked)
            {
                return false;
            }

            await _session.WaitForElementToAppearAsync(Cancel.CancelPopupMessageDiv);
            await _session.InvokeOnElementAsync(Cancel.ConfirmButton, x => x.Click());

            await _session.WaitForAsync(driver => driver.Title.Contains("Booking annulleret"));

            return true;
        }

        private static class Login
        {
            public static readonly By Username = By.Id("cw-login-customer-customerCode");
            public static readonly By Password = By.Id("cw-login-customer-password");
            public static readonly By Submit = By.CssSelector("button[type=submit]");

            public static readonly By CustomerLogoutButton = By.ClassName("cw-do-customerlogout");
            public static readonly By AgentLogoutButton = By.ClassName("cw-do-agentlogout");
        }

        private static class Cancel
        {
            public static readonly By BookingsTable = By.ClassName("cw-bookings");

            public static By CancelLink(string bookingNumber) => By.CssSelector($"a.cw-booking-cancel[data-cw-action-link*=\"{bookingNumber}\"");
            public static readonly By CancelPopupMessageDiv = By.CssSelector("div.fancybox-inner > div.cancel-booking-message");
            public static readonly By ConfirmButton = By.CssSelector("div.fancybox-dialog-buttons > button:last-child");
        }
    }
}
