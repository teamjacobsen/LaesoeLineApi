using OpenQA.Selenium;

namespace LaesoeLineApi.Features.AgentBooking.Pages
{
    public class LoginPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/customerLogin/";
        public IWebDriver Driver { get; private set; }

        private static readonly By UsernameSelector = By.Id("username");
        private IWebElement Username => Driver.FindVisibleElement(UsernameSelector);
        private IWebElement Password => Driver.FindVisibleElement(By.Id("password"));
        private IWebElement Submit => Driver.FindVisibleElement(By.CssSelector("button[type=submit]"));

        public LoginPage(IWebDriver driver)
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
    }
}
