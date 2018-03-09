using OpenQA.Selenium;

namespace LaesoeLineApi.Features.Agent.Pages
{
    public class BookingConfirmationPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/bookingConfirmation/";
        public IWebDriver Driver { get; private set; }

        private static readonly By CancelBookingButtonSelector = By.ClassName("cw-action-cancelBooking");
        private IWebElement CancelBookingButton => Driver.FindVisibleElement(CancelBookingButtonSelector);
        private static readonly By CancelPopupMessageDivSelector = By.CssSelector("div.fancybox-inner > div.cancel-booking-message");
        private IWebElement ConfirmCancelButton => Driver.FindVisibleElement(By.CssSelector("div.fancybox-dialog-buttons > button:last-child"));

        public BookingConfirmationPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void Cancel()
        {
            // Wait for the cancel button to show
            Driver.WaitForElementToAppear(CancelBookingButtonSelector);

            CancelBookingButton.Click();
            Driver.WaitForElementToAppear(CancelPopupMessageDivSelector);
            ConfirmCancelButton.Click();
            Driver.WaitForTitleContains("Booking annulleret");
        }
    }
}
