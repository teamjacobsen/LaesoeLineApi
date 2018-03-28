using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features
{
    public abstract class DepartureSelectPageBase : PageBase
    {
        private readonly IBrowserSession _session;

        public DepartureSelectPageBase(IBrowserSession session, ILogger logger)
            : base(session, logger)
        {
            _session = session;
        }

        public virtual Task SelectDepartureAsync(DateTime departure)
        {
            return ExecuteWithRetry(async () =>
            {
                await PopulateDepartureAsync(departure);

                await GoToNextStepAsync();
            });
        }

        public virtual Task SelectDeparturesAsync(DateTime outbound, DateTime @return)
        {
            return ExecuteWithRetry(async () =>
            {
                await PopulateDepartureAsync(outbound, @return);

                await GoToNextStepAsync();
            });
        }

        protected async Task PopulateDepartureAsync(DateTime departure)
        {
            await _session.WaitForElementToAppearAsync(DepartureTable, minCount: 1);

            if (!await _session.TryInvokeOnElementAsync(OutboundDepartureRadio(departure), x => x.Click()))
            {
                throw new ApiException(ApiStatus.DepartureNotFound);
            }
        }

        protected async Task PopulateDepartureAsync(DateTime outbound, DateTime @return)
        {
            await _session.WaitForElementToAppearAsync(DepartureTable, minCount: 1);

            if (!await _session.TryInvokeOnElementAsync(OutboundDepartureRadio(outbound), x => x.Click()))
            {
                throw new ApiException(ApiStatus.OutboundDepartureNotFound);
            }

            await _session.WaitForElementToDisappearAsync(LoadingSpinner);
            await _session.WaitForElementToAppearAsync(DepartureTable, minCount: 2);

            if (!await _session.TryInvokeOnElementAsync(ReturnDepartureRadio(@return), x => x.Click()))
            {
                throw new ApiException(ApiStatus.ReturnDepartureNotFound);
            }
        }

        protected async Task GoToNextStepAsync()
        {
            await _session.InvokeOnElementAsync(NextButton, x => x.Click());

            await _session.WaitForInteractiveReadyStateAsync();
            await _session.InvokeAsync(x => Thread.Sleep(500));
        }

        protected static readonly By LoadingSpinner = By.ClassName("cw-loading-mask-spinner");

        protected static readonly By DepartureTable = By.ClassName("choosejourney-departures");
        protected static By OutboundDepartureRadio(DateTime departure) => By.CssSelector($"input[name=\"cw_choosejourney_j1_departures\"][data-cw-departure-datetime=\"{departure.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}\"]");
        protected static By ReturnDepartureRadio(DateTime departure) => By.CssSelector($"input[name=\"cw_choosejourney_j2_departures\"][data-cw-departure-datetime=\"{departure.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}\"]");
        protected static readonly By NextButton = By.CssSelector("button.cw-action-next");
    }
}
