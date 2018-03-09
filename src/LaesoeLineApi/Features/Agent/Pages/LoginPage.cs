using OpenQA.Selenium;

namespace LaesoeLineApi.Features.Agent.Pages
{
    public class LoginPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/customerLogin/";
        public IWebDriver Driver { get; private set; }

        private IWebElement Username => Driver.FindVisibleElement(By.Id("username"));
        private IWebElement Password => Driver.FindVisibleElement(By.Id("password"));
        private IWebElement Submit => Driver.FindVisibleElement(By.CssSelector("button[type=submit]"));

        public LoginPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void Login(string username, string password)
        {
            Username.SendKeys(username);
            Password.SendKeys(password);
            Submit.Click();
        }
    }
}
