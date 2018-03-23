using OpenQA.Selenium;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.AgentBooking.Pages
{
    public class LoginPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/customerLogin/";
        public IWebDriver Driver { get; private set; }

        private static readonly By UsernameText = By.Id("username");
        private static readonly By PasswordText = By.Id("password");
        private static readonly By SubmitButton = By.CssSelector("button[type=submit]");

        public LoginPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public async Task LoginAsync(string username, string password)
        {
            await Driver.FindVisibleElementAsync(UsernameText).ThenSendKeys(username);
            await Driver.FindVisibleElementAsync(PasswordText).ThenSendKeys(password);
            await Driver.FindVisibleElementAsync(SubmitButton).ThenClick();
        }
    }
}
