using LaesoeLineApi.Features.AgentBooking.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LaesoeLineApi.Features.AgentBooking.Pages
{
    public class BookPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/Rejsedetaljer/";
        public IWebDriver Driver { get; private set; }
        public string BookingNumber { get; set; }
        public string BookingPassword { get; set; }
        public decimal Price { get; set; }

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
        private IWebElement OutboundDepartureRadio(DateTime departure) => Driver.FindVisibleElement(By.CssSelector($"input[name=\"cw_choosejourney_j1_departures\"][data-cw-departure-datetime=\"{departure.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}\"]"));
        private IWebElement ReturnDepartureRadio(DateTime departure) => Driver.FindVisibleElement(By.CssSelector($"input[name=\"cw_choosejourney_j2_departures\"][data-cw-departure-datetime=\"{departure.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}\"]"));
        private IWebElement DepartureNextButton => Driver.FindVisibleElement(By.CssSelector("button.cw-action-next"));

        // Contact Information
        private IWebElement LastNameText => Driver.FindVisibleElement(By.Id("lastName"));
        private IWebElement MobileText => Driver.FindVisibleElement(By.Id("mobile"));
        private IWebElement EmailText => Driver.FindVisibleElement(By.Id("email"));
        private IWebElement TermsCheckbox => Driver.FindVisibleElement(By.Id("acceptTerms"));
        private IWebElement ContactNextButton => Driver.FindVisibleElement(By.CssSelector("button.cw-action-next"));

        // Confirmation
        private static readonly By BookingNumberDivSelector = By.CssSelector("div.cw-booking-code");
        private IWebElement BookingNumberDiv => Driver.FindVisibleElement(BookingNumberDivSelector);
        private IWebElement BookingPasswordDiv => Driver.FindVisibleElement(By.ClassName("cw-booking-pwd"));
        private IWebElement TotalPriceSpan => Driver.FindVisibleElements(By.ClassName("total-label-price")).First();

        private static readonly Dictionary<Vehicle, string> VehicleValues = new Dictionary<Vehicle, string>()
        {
            { Vehicle.None, string.Empty },
            { Vehicle.Car, "19" },
            { Vehicle.Van, "21" }
        };

        public BookPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public BookStatus BookItRoundTrip(Customer guest, Journey outbound, Journey @return)
        {
            // Booking Details
            ItRoundTripRadio.Click();

            Driver.WaitForElementToDisappear(LoadingSpinnerSelector);

            CopyDetailsCheckbox.SetChecked(false);

            OutboundCrossingSelect.SelectByIndex((int)outbound.Crossing);
            Driver.SetValueWithScript(OutboundDepartureCalendarCssSelector, outbound.Departure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            OutboundPassengersSelect.SelectByValue(outbound.VehiclePassengers.ToString());
            OutboundAdultsSelect.SelectByValue(outbound.Adults.ToString());
            OutboundChildrenSelect.SelectByValue(outbound.Children.ToString());
            OutboundSeniorsSelect.SelectByValue(outbound.Seniors.ToString());
            OutboundInfantsSelect.SelectByValue(outbound.Infants.ToString());
            if (!VehicleValues.TryGetValue(outbound.Vehicle, out var outboundVehicleValue))
            {
                return BookStatus.VehicleNotFound;
            }
            OutboundVehicleSelect.SelectByValue(outboundVehicleValue);

            ReturnCrossingSelect.SelectByIndex((int)@return.Crossing);
            Driver.SetValueWithScript(ReturnDepartureCalendarCssSelector, @return.Departure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            ReturnPassengersSelect.SelectByValue(@return.VehiclePassengers.ToString());
            ReturnAdultsSelect.SelectByValue(@return.Adults.ToString());
            ReturnChildrenSelect.SelectByValue(@return.Children.ToString());
            ReturnSeniorsSelect.SelectByValue(@return.Seniors.ToString());
            ReturnInfantsSelect.SelectByValue(@return.Infants.ToString());
            if (!VehicleValues.TryGetValue(@return.Vehicle, out var returnVehicleValue))
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
            LastNameText.Clear();
            LastNameText.SendKeys(guest.Name);
            MobileText.Clear();
            MobileText.SendKeys(guest.PhoneNumber);
            EmailText.Clear();
            EmailText.SendKeys(guest.Email);
            TermsCheckbox.Click();

            ContactNextButton.Click();

            // Confirmation
            Driver.WaitForElementToAppear(BookingNumberDivSelector);
            BookingNumber = BookingNumberDiv.Text;
            BookingPassword = BookingPasswordDiv.Text;
            Price = decimal.Parse(TotalPriceSpan.Text.Replace(" DKK", string.Empty).Replace(',', '.'), CultureInfo.InvariantCulture);

            return BookStatus.Success;
        }
    }
}
