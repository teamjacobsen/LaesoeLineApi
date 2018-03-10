using LaesoeLineApi.Features.Customer.Models;
using OpenQA.Selenium;
using System;
using System.Globalization;

namespace LaesoeLineApi.Features.Customer.Pages
{
    public class BookSeasonPassPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/book/aarskort-2018/Rejsedetaljer/";
        public IWebDriver Driver { get; private set; }
        public string BookingNumber { get; set; }
        public string BookingPassword { get; set; }

        // Booking Details
        private IWebElement SeasonPassOneWayRadio => Driver.FindVisibleElement(By.Id("cw-bookingflow-ÅRS18ENK"));
        private IWebElement SeasonPassRoundTripRadio => Driver.FindVisibleElement(By.Id("cw-bookingflow-ÅRSRET18"));
        private IWebElement SeasonPassLocalOneWayRadio => Driver.FindVisibleElement(By.Id("cw-bookingflow-AAKOE18"));
        private IWebElement SeasonPassLocalRoundTripRadio => Driver.FindVisibleElement(By.Id("cw-bookingflow-AAKOR18"));

        private static readonly By LoadingSpinnerSelector = By.ClassName("cw-loading-mask-spinner");

        private IWebElement CopyDetailsCheckbox => Driver.FindVisibleElement(By.Name("journeysearch-passengers-copy"));

        private IWebElement OutboundCrossingSelect => Driver.FindVisibleElement(By.Id("j1_route-j1_route"));
        private const string OutboundDepartureCalendarCssSelector = "input.cw-journeysearch-calendar-1";
        private IWebElement OutboundPassengersSelect => Driver.FindVisibleElement(By.Id("cw-journeysearch-pax-1-500"));
        private IWebElement OutboundVehicleSelect => Driver.FindVisibleElement(By.Name("cw_journeysearch_j1_vehicles[0][ctg]"));

        private IWebElement ReturnCrossingSelect => Driver.FindVisibleElement(By.Id("j2_route-j2_route"));
        private const string ReturnDepartureCalendarCssSelector = "input.cw-journeysearch-calendar-2";
        private IWebElement ReturnPassengersSelect => Driver.FindVisibleElement(By.Id("cw-journeysearch-pax-2-500"));
        private IWebElement ReturnVehicleSelect => Driver.FindVisibleElement(By.Name("cw_journeysearch_j2_vehicles[0][ctg]"));

        private IWebElement DetailsNextButton => Driver.FindVisibleElement(By.CssSelector("button.cw-action-next"));

        // Departure Select
        private static readonly By DepartureTableSelector = By.ClassName("choosejourney-departures");
        private IWebElement OutboundDepartureRadio(DateTime departure) => Driver.FindVisibleElement(By.CssSelector($"input[name=\"cw_choosejourney_j1_departures\"][data-cw-departure-datetime=\"{departure.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}\"]"));
        private IWebElement ReturnDepartureRadio(DateTime departure) => Driver.FindVisibleElement(By.CssSelector($"input[name=\"cw_choosejourney_j2_departures\"][data-cw-departure-datetime=\"{departure.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}\"]"));
        private IWebElement DepartureNextButton => Driver.FindVisibleElement(By.CssSelector("button.cw-action-next"));

        // Contact Information
        private IWebElement TermsCheckbox => Driver.FindVisibleElement(By.Id("acceptTerms"));
        private IWebElement ContactNextButton => Driver.FindVisibleElement(By.CssSelector("button.cw-action-next"));

        // Confirmation
        private static readonly By BookingNumberDivSelector = By.CssSelector("div.cw-booking-code");
        private IWebElement BookingNumberDiv => Driver.FindVisibleElement(BookingNumberDivSelector);
        private IWebElement BookingPasswordDiv => Driver.FindVisibleElement(By.ClassName("cw-booking-pwd"));

        public BookSeasonPassPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public BookStatus BookOneWay(Journey journey, bool local)
        {
            // Booking Details
            if (local)
            {
                SeasonPassLocalOneWayRadio.Click();
            }
            else
            {
                SeasonPassOneWayRadio.Click();
            }

            Driver.WaitForElementToDisappear(LoadingSpinnerSelector);

            OutboundCrossingSelect.SelectByIndex((int)journey.Crossing);
            Driver.SetValueWithScript(OutboundDepartureCalendarCssSelector, journey.Departure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            OutboundPassengersSelect.SelectByValue(journey.Passengers.ToString());
            if (!TryGetVehicleValue(journey.Vehicle, out var vehicleValue))
            {
                return BookStatus.VehicleNotFound;
            }
            OutboundVehicleSelect.SelectByValue(vehicleValue);

            DetailsNextButton.Click();

            // Depearture Select
            Driver.WaitForElementToAppear(DepartureTableSelector);
            var departureRadio = OutboundDepartureRadio(journey.Departure);
            if (departureRadio == null)
            {
                return BookStatus.DepartureNotFound;
            }
            departureRadio.Click();

            DepartureNextButton.Click();

            // Contact Information
            TermsCheckbox.Click();

            ContactNextButton.Click();

            // Confirmation
            Driver.WaitForElementToAppear(BookingNumberDivSelector);
            BookingNumber = BookingNumberDiv.Text;
            BookingPassword = BookingPasswordDiv.Text;

            return BookStatus.Success;
        }

        public BookStatus BookRoundTrip(Journey outbound, Journey @return, bool local)
        {
            // Booking Details
            if (local)
            {
                SeasonPassLocalRoundTripRadio.Click();
            }
            else
            {
                SeasonPassRoundTripRadio.Click();
            }

            Driver.WaitForElementToDisappear(LoadingSpinnerSelector);

            CopyDetailsCheckbox.SetChecked(false);

            OutboundCrossingSelect.SelectByIndex((int)outbound.Crossing);
            Driver.SetValueWithScript(OutboundDepartureCalendarCssSelector, outbound.Departure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            OutboundPassengersSelect.SelectByValue(outbound.Passengers.ToString());
            if (!TryGetVehicleValue(outbound.Vehicle, out var outboundVehicleValue))
            {
                return BookStatus.VehicleNotFound;
            }
            OutboundVehicleSelect.SelectByValue(outboundVehicleValue);

            ReturnCrossingSelect.SelectByIndex((int)@return.Crossing);
            Driver.SetValueWithScript(ReturnDepartureCalendarCssSelector, @return.Departure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            ReturnPassengersSelect.SelectByValue(@return.Passengers.ToString());
            if (!TryGetVehicleValue(@return.Vehicle, out var returnVehicleValue))
            {
                return BookStatus.VehicleNotFound;
            }
            ReturnVehicleSelect.SelectByValue(returnVehicleValue);

            DetailsNextButton.Click();

            // Departure Select
            Driver.WaitForElementToAppear(DepartureTableSelector);
            var outboundDepartureRadio = OutboundDepartureRadio(outbound.Departure);
            if (outboundDepartureRadio == null)
            {
                return BookStatus.OutboundDepartureNotFound;
            }
            outboundDepartureRadio.Click();

            Driver.WaitForElementToAppear(DepartureTableSelector, 2);
            var returnDepartureRadio = ReturnDepartureRadio(@return.Departure);
            if (returnDepartureRadio == null)
            {
                return BookStatus.ReturnDepartureNotFound;
            }
            returnDepartureRadio.Click();

            DepartureNextButton.Click();

            // Contact Information
            TermsCheckbox.Click();

            ContactNextButton.Click();

            // Confirmation
            Driver.WaitForElementToAppear(BookingNumberDivSelector);
            BookingNumber = BookingNumberDiv.Text;
            BookingPassword = BookingPasswordDiv.Text;

            return BookStatus.Success;
        }

        private bool TryGetVehicleValue(Vehicle vehicle, out string value)
        {
            switch (vehicle)
            {
                case Vehicle.Car:
                    value = "319";
                    return true;
                default:
                    value = default;
                    return false;
            }
        }
    }
}
