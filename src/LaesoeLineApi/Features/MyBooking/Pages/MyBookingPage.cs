using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.MyBooking.Pages
{
    public class MyBookingPage : PageBase
    {
        private readonly IBrowserSession _session;
        private readonly ILogger<MyBookingPage> _logger;

        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/my-booking/";

        public MyBookingPage(IBrowserSession session, ILogger<MyBookingPage> logger)
            : base(session, logger)
        {
            _session = session;
            _logger = logger;
        }

        public Task LoginAsync(string bookingNumber, string bookingPassword)
        {
            return ExecuteWithRetry(async () =>
            {
                if (await _session.InvokeAsync(x => x.FindElements(CancelBookingButton).Count > 0))
                {
                    // Already logged in
                    return;
                }

                await _session.InvokeOnElementAsync(BookingNumberText, x => x.SendKeys(bookingNumber));
                await _session.InvokeOnElementAsync(BookingPasswordText, x => x.SendKeys(bookingPassword));
                await _session.InvokeOnElementAsync(SubmitButton, x => x.Click());

                await _session.WaitForElementToAppearAsync(CancelBookingButton);
            });
        }

        public async Task CancelAsync()
        {
            try
            {
                await _session.InvokeOnElementAsync(CancelBookingButton, x => x.Click());
                await _session.WaitForElementToAppearAsync(CancelPopupMessageDiv);
                await _session.InvokeOnElementAsync(ConfirmButton, x => x.Click());

                await _session.WaitForAsync(driver => driver.Title.Contains("Booking annulleret"));
            }
            catch (WebDriverTimeoutException)
            {
                throw new ApiException(ApiStatus.GatewayTimeout);
            }
        }

        private static readonly By BookingNumberText = By.Id("cw-login-booking-code");
        private static readonly By BookingPasswordText = By.Id("cw-login-booking-password");
        private static readonly By SubmitButton = By.CssSelector("button[type=submit]");

        private static readonly By CancelBookingButton = By.ClassName("cw-action-cancelBooking");
        private static readonly By CancelPopupMessageDiv = By.CssSelector("div.fancybox-inner > div.cancel-booking-message");
        private static readonly By ConfirmButton = By.CssSelector("div.fancybox-dialog-buttons > button:last-child");
    }
}