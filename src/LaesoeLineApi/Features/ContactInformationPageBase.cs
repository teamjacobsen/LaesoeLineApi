using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System.Threading;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features
{
    public abstract class ContactInformationPageBase : PageBase
    {
        private readonly IBrowserSession _session;

        public ContactInformationPageBase(IBrowserSession session, ILogger logger)
            : base(session, logger)
        {
            _session = session;
        }

        public virtual Task CheckTermsAsync()
        {
            return ExecuteWithRetry(async () =>
            {
                await PopulateTermsAsync();

                await GoToNextStepAsync();
            });
        }

        public virtual Task EnterInformationAndCheckTermsAsync(string name, string phoneNumber, string email)
        {
            return ExecuteWithRetry(async () =>
            {
                await PopulateInformationAsync(name, phoneNumber, email);

                await PopulateTermsAsync();

                await GoToNextStepAsync();
            });
        }

        protected async Task PopulateInformationAsync(string name, string phoneNumber, string email)
        {
            await _session.InvokeOnElementAsync(LastNameText, x =>
            {
                x.Clear();
                x.SendKeys(name);
            });


            await _session.InvokeOnElementAsync(MobileText, x =>
            {
                x.Clear();
                x.SendKeys(phoneNumber);
            });

            await _session.InvokeOnElementAsync(EmailText, x =>
            {
                x.Clear();
                x.SendKeys(email);
            });
        }

        protected Task PopulateTermsAsync()
        {
            return _session.InvokeOnElementAsync(TermsCheckbox, x => x.Click());
        }

        protected async Task GoToNextStepAsync()
        {
            await _session.InvokeOnElementAsync(NextButton, x => x.Click());

            await _session.WaitForInteractiveReadyStateAsync();
            await _session.InvokeAsync(x => Thread.Sleep(500));
        }

        protected static readonly By LastNameText = By.Id("lastName");
        protected static readonly By MobileText = By.Id("mobile");
        protected static readonly By EmailText = By.Id("email");
        protected static readonly By TermsCheckbox = By.Id("acceptTerms");

        protected static readonly By NextButton = By.CssSelector("button.cw-action-next");
    }
}
