using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features
{
    public abstract class BookingDetailsPageBase : PageBase
    {
        private readonly IBrowserSession _session;

        public BookingDetailsPageBase(IBrowserSession session, ILogger logger)
            : base(session, logger)
        {
            _session = session;
        }

        protected async Task PopulateBookingFlowRadioAsync(By by)
        {
            await _session.InvokeOnElementAsync(by, x => x.Click());

            await _session.WaitForElementToDisappearAsync(LoadingSpinner);
            await _session.WaitForInteractiveReadyStateAsync();
        }

        protected Task PopulateCopyDetails(bool value)
        {
            return _session.InvokeOnElementAsync(CopyDetailsCheckbox, x =>
            {
                if (x.Enabled != value)
                {
                    x.Click();
                }
            });
        }

        protected async Task PopulateOutboundAsync(Crossing crossing, DateTime departure, string vehicleOptionValue, int passengers)
        {
            if (vehicleOptionValue == null)
            {
                throw new ApiException(ApiStatus.VehicleNotFound);
            }

            await _session.InvokeOnSelectElementAsync(OutboundCrossingSelect, x => x.SelectByIndex((int)crossing));

            await _session.SetValueWithScriptAsync(OutboundDepartureCalendar, departure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            await _session.InvokeOnSelectElementAsync(OutboundPassengersSelect, x => x.SelectByValue(passengers.ToString()));

            await _session.InvokeOnSelectElementAsync(OutboundVehicleSelect, x => x.SelectByValue(vehicleOptionValue));
        }

        protected async Task PopulateOutboundAsync(Crossing crossing, DateTime departure, int adults, int children, int seniors, int infants)
        {
            await _session.InvokeOnSelectElementAsync(OutboundCrossingSelect, x => x.SelectByIndex((int)crossing));

            await _session.SetValueWithScriptAsync(OutboundDepartureCalendar, departure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            await _session.InvokeOnSelectElementAsync(OutboundAdultsSelect, x => x.SelectByValue(adults.ToString()));
            await _session.InvokeOnSelectElementAsync(OutboundChildrenSelect, x => x.SelectByValue(children.ToString()));
            await _session.InvokeOnSelectElementAsync(OutboundSeniorsSelect, x => x.SelectByValue(seniors.ToString()));
            await _session.InvokeOnSelectElementAsync(OutboundInfantsSelect, x => x.SelectByValue(infants.ToString()));
        }

        protected async Task PopulateReturnAsync(Crossing crossing, DateTime departure, string vehicleOptionValue, int passengers)
        {
            if (vehicleOptionValue == null)
            {
                throw new ApiException(ApiStatus.VehicleNotFound);
            }

            await _session.InvokeOnSelectElementAsync(ReturnCrossingSelect, x => x.SelectByIndex((int)crossing));

            await _session.SetValueWithScriptAsync(ReturnDepartureCalendar, departure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            await _session.InvokeOnSelectElementAsync(ReturnPassengersSelect, x => x.SelectByValue(passengers.ToString()));

            await _session.InvokeOnSelectElementAsync(ReturnVehicleSelect, x => x.SelectByValue(vehicleOptionValue));
        }

        protected async Task PopulateReturnAsync(Crossing crossing, DateTime departure, int adults, int children, int seniors, int infants)
        {
            await _session.InvokeOnSelectElementAsync(ReturnCrossingSelect, x => x.SelectByIndex((int)crossing));

            await _session.SetValueWithScriptAsync(ReturnDepartureCalendar, departure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            await _session.InvokeOnSelectElementAsync(ReturnAdultsSelect, x => x.SelectByValue(adults.ToString()));
            await _session.InvokeOnSelectElementAsync(ReturnChildrenSelect, x => x.SelectByValue(children.ToString()));
            await _session.InvokeOnSelectElementAsync(ReturnSeniorsSelect, x => x.SelectByValue(seniors.ToString()));
            await _session.InvokeOnSelectElementAsync(ReturnInfantsSelect, x => x.SelectByValue(infants.ToString()));
        }

        protected async Task GoToNextStepAsync()
        {
            await _session.InvokeOnElementAsync(NextButton, x => x.Click());

            await _session.WaitForInteractiveReadyStateAsync();
            await _session.InvokeAsync(x => Thread.Sleep(500));
        }

        protected static readonly By LoadingSpinner = By.ClassName("cw-loading-mask-spinner");

        protected static readonly By LocalVehicleOneWayRadio = By.Id("cw-bookingflow-OBO18ENK");
        protected static readonly By SeasonPassOneWayRadio = By.Id("cw-bookingflow-ÅRS18ENK");
        protected static readonly By SeasonPassRoundTripRadio = By.Id("cw-bookingflow-ÅRSRET18");
        protected static readonly By SeasonPassLocalOneWayRadio = By.Id("cw-bookingflow-AAKOE18");
        protected static readonly By SeasonPassLocalRoundTripRadio = By.Id("cw-bookingflow-AAKOR18");
        protected static readonly By ItRoundTripRadio = By.Id("cw-bookingflow-IT");

        protected static readonly By CopyDetailsCheckbox = By.Name("journeysearch-passengers-copy");

        protected static readonly By OutboundCrossingSelect = By.Id("j1_route-j1_route");
        protected const string OutboundDepartureCalendar = "input.cw-journeysearch-calendar-1";
        protected static readonly By OutboundPassengersSelect = By.Id("cw-journeysearch-pax-1-500");
        protected static readonly By OutboundAdultsSelect = By.Id("cw-journeysearch-pax-1-1");
        protected static readonly By OutboundChildrenSelect = By.Id("cw-journeysearch-pax-1-3");
        protected static readonly By OutboundSeniorsSelect = By.Id("cw-journeysearch-pax-1-4");
        protected static readonly By OutboundInfantsSelect = By.Id("cw-journeysearch-pax-1-2");
        protected static readonly By OutboundVehicleSelect = By.Name("cw_journeysearch_j1_vehicles[0][ctg]");

        protected static readonly By ReturnCrossingSelect = By.Id("j2_route-j2_route");
        protected const string ReturnDepartureCalendar = "input.cw-journeysearch-calendar-2";
        protected static readonly By ReturnPassengersSelect = By.Id("cw-journeysearch-pax-2-500");
        protected static readonly By ReturnAdultsSelect = By.Id("cw-journeysearch-pax-2-1");
        protected static readonly By ReturnChildrenSelect = By.Id("cw-journeysearch-pax-2-3");
        protected static readonly By ReturnSeniorsSelect = By.Id("cw-journeysearch-pax-2-4");
        protected static readonly By ReturnInfantsSelect = By.Id("cw-journeysearch-pax-2-2");
        protected static readonly By ReturnVehicleSelect = By.Name("cw_journeysearch_j2_vehicles[0][ctg]");

        protected static readonly By NextButton = By.CssSelector("button.cw-action-next");
    }
}
