using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features
{
    public abstract class BookingConfirmationPageBase : PageBase
    {
        private readonly IBrowserSession _session;

        public BookingConfirmationPageBase(IBrowserSession session, ILogger logger)
            : base(session, logger)
        {
            _session = session;
        }

        public virtual Task<(string BookingNumber, string BookingPassword, decimal TotalPrice)> GetBookingDetailsAsync()
        {
            return ExecuteWithRetry(() => ReadBookingDetailsAsync());
        }

        protected async Task<(string BookingNumber, string BookingPassword, decimal TotalPrice)> ReadBookingDetailsAsync()
        {
            string bookingNumber = null;
            string bookingPassword = null;
            decimal totalPrice = 0;

            await _session.InvokeOnElementAsync(BookingNumberDiv, x => bookingNumber = x.Text);

            await _session.InvokeOnElementAsync(BookingPasswordDiv, x => bookingPassword = x.Text);

            await _session.InvokeAsync(driver =>
            {
                var element = driver.FindElements(TotalPriceSpans).First();

                totalPrice = decimal.Parse(element.Text.Replace(" DKK", string.Empty).Replace(".", string.Empty).Replace(',', '.'), CultureInfo.InvariantCulture);
            });

            return (bookingNumber, bookingPassword, totalPrice);
        }

        protected static readonly By BookingNumberDiv = By.CssSelector("div.cw-booking-code");
        protected static readonly By BookingPasswordDiv = By.ClassName("cw-booking-pwd");
        protected static readonly By TotalPriceSpans = By.ClassName("total-label-price");
    }
}
