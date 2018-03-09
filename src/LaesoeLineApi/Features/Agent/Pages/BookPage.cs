using LaesoeLineApi.Features.Agent.Models;
using OpenQA.Selenium;
using System.Globalization;

namespace LaesoeLineApi.Features.Agent.Pages
{
    public class BookPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/Rejsedetaljer/";
        public IWebDriver Driver { get; private set; }
        public string BookingNumber { get; set; }

        // Booking Details
        private IWebElement ItRoundTripRadio => Driver.FindVisibleElement(By.Id("cw-bookingflow-IT"));

        private static readonly By LoadingSpinnerSelector = By.ClassName("cw-loading-mask-spinner");

        private IWebElement CopyDetailsCheckbox => Driver.FindVisibleElement(By.Name("journeysearch-passengers-copy"));

        private IWebElement OutboundCrossingSelect => Driver.FindVisibleElement(By.Id("j1_route-j1_route"));
        private const string OutboundDepartureCalendarCssSelector = "input.cw-journeysearch-calendar-1";
        private IWebElement OutboundPassengersSelect => Driver.FindVisibleElement(By.Id("cw-journeysearch-pax-1-500"));
        private IWebElement OutboundAdultsSelect => Driver.FindVisibleElement(By.Id("cw-journeysearch-pax-1-1"));
        private IWebElement OutboundChildrenSelect => Driver.FindVisibleElement(By.Id("cw-journeysearch-pax-1-3"));
        private IWebElement OutboundSeniorsSelect => Driver.FindVisibleElement(By.Id("cw-journeysearch-pax-1-4"));
        private IWebElement OutboundInfantsSelect => Driver.FindVisibleElement(By.Id("cw-journeysearch-pax-1-2"));
        private IWebElement OutboundVehicleSelect => Driver.FindVisibleElement(By.Name("cw_journeysearch_j1_vehicles[0][ctg]"));

        private IWebElement ReturnCrossingSelect => Driver.FindVisibleElement(By.Id("j2_route-j2_route"));
        private const string ReturnDepartureCalendarCssSelector = "input.cw-journeysearch-calendar-2";
        private IWebElement ReturnPassengersSelect => Driver.FindVisibleElement(By.Id("cw-journeysearch-pax-2-500"));
        private IWebElement ReturnAdultsSelect => Driver.FindVisibleElement(By.Id("cw-journeysearch-pax-2-1"));
        private IWebElement ReturnChildrenSelect => Driver.FindVisibleElement(By.Id("cw-journeysearch-pax-2-3"));
        private IWebElement ReturnSeniorsSelect => Driver.FindVisibleElement(By.Id("cw-journeysearch-pax-2-4"));
        private IWebElement ReturnInfantsSelect => Driver.FindVisibleElement(By.Id("cw-journeysearch-pax-2-2"));
        private IWebElement ReturnVehicleSelect => Driver.FindVisibleElement(By.Name("cw_journeysearch_j2_vehicles[0][ctg]"));

        private IWebElement DetailsNextButton => Driver.FindVisibleElement(By.CssSelector("button.cw-action-next"));

        // Departure Select
        private static readonly By DepartureTableSelector = By.ClassName("choosejourney-departures");

        public BookPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public BookStatus BookItRoundTrip(Journey outbound, Journey @return)
        {
            // Booking Details
            ItRoundTripRadio.Click();

            Driver.WaitForElementToDisappear(LoadingSpinnerSelector);

            CopyDetailsCheckbox.SetChecked(false);

            OutboundCrossingSelect.SelectByIndex((int)outbound.Crossing);
            Driver.SetValueWithScript(OutboundDepartureCalendarCssSelector, outbound.Departure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            OutboundPassengersSelect.SelectByValue(outbound.Passengers.ToString());
            OutboundAdultsSelect.SelectByValue(outbound.Adults.ToString());
            OutboundChildrenSelect.SelectByValue(outbound.Children.ToString());
            OutboundSeniorsSelect.SelectByValue(outbound.Seniors.ToString());
            OutboundInfantsSelect.SelectByValue(outbound.Infants.ToString());
            if (!TryGetVehicleValue(outbound.Vehicle, out var outboundVehicleValue))
            {
                return BookStatus.VehicleNotFound;
            }
            OutboundVehicleSelect.SelectByValue(outboundVehicleValue);

            ReturnCrossingSelect.SelectByIndex((int)@return.Crossing);
            Driver.SetValueWithScript(ReturnDepartureCalendarCssSelector, @return.Departure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            ReturnPassengersSelect.SelectByValue(@return.Passengers.ToString());
            ReturnAdultsSelect.SelectByValue(@return.Adults.ToString());
            ReturnChildrenSelect.SelectByValue(@return.Children.ToString());
            ReturnSeniorsSelect.SelectByValue(@return.Seniors.ToString());
            ReturnInfantsSelect.SelectByValue(@return.Infants.ToString());
            if (!TryGetVehicleValue(@return.Vehicle, out var returnVehicleValue))
            {
                return BookStatus.VehicleNotFound;
            }
            ReturnVehicleSelect.SelectByValue(returnVehicleValue);

            DetailsNextButton.Click();

            // Departure Select
            Driver.WaitForElementToAppear(DepartureTableSelector);

            return BookStatus.Success;
        }

        private bool TryGetVehicleValue(Vehicle vehicle, out string value)
        {
            switch (vehicle)
            {
                case Vehicle.Car:
                    value = "19";
                    return true;
                default:
                    value = default;
                    return false;
            }
        }
    }
}
