using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.AgentBooking.Pages
{
    public class LoginPage : PageBase
    {
        private readonly IBrowserSession _session;
        private readonly ILogger<LoginPage> _logger;

        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/customerLogin/";

        public LoginPage(IBrowserSession session, ILogger<LoginPage> logger)
            : base(session, logger)
        {
            _session = session;
            _logger = logger;
        }

        public Task LoginAsync(string username, string password)
        {
            return ExecuteWithRetry(async () =>
            {
                if (await _session.InvokeAsync(x => x.FindElements(BookingDetailsContainerDiv).Count > 0))
                {
                    // Already logged in
                    return;
                }

                await _session.InvokeOnElementAsync(UsernameText, x => x.SendKeys(username));
                await _session.InvokeOnElementAsync(PasswordText, x => x.SendKeys(password));
                await _session.InvokeOnElementAsync(SubmitButton, x => x.Click());

                await _session.WaitForElementToAppearAsync(BookingDetailsContainerDiv);
            });
        }

        private static readonly By UsernameText = By.Id("username");
        private static readonly By PasswordText = By.Id("password");
        private static readonly By SubmitButton = By.CssSelector("button[type=submit]");

        private static readonly By BookingDetailsContainerDiv = By.ClassName("cw-component-journeysearch-content");
    }
}
