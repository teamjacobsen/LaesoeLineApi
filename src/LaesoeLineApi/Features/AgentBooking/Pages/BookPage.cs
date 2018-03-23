using LaesoeLineApi.Features.AgentBooking.Models;
using OpenQA.Selenium;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.AgentBooking.Pages
{
    public class BookPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/Rejsedetaljer/";
        public IWebDriver Driver { get; private set; }
        public string BookingNumber { get; set; }
        public string BookingPassword { get; set; }
        public decimal Price { get; set; }

        public BookPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public async Task<BookStatus> BookItRoundTripAsync(Customer customer, Journey outbound, Journey @return)
        {
            // Booking Details
            await Driver.FindVisibleElementAsync(BookingDetails.ItRoundTripRadio).ThenClick();

            //await Driver.TryWaitForElementToAppearAsync(BookingDetails.LoadingSpinner, timeout: TimeSpan.FromMilliseconds(100));
            await Driver.WaitForElementToDisappearAsync(BookingDetails.LoadingSpinner);

            await Driver.FindVisibleElementAsync(BookingDetails.CopyDetailsCheckbox).ThenClick();

            // Outbound
            await Driver.FindVisibleSelectElementAsync(BookingDetails.OutboundCrossingSelect).ThenSelectByIndex((int)outbound.Crossing);

            Driver.SetValueWithScript(BookingDetails.OutboundDepartureCalendar, outbound.Departure.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            await Driver.FindVisibleSelectElementAsync(BookingDetails.OutboundPassengersSelect).ThenSelectByValue(outbound.VehiclePassengers.ToString());
            await Driver.FindVisibleSelectElementAsync(BookingDetails.OutboundAdultsSelect).ThenSelectByValue(outbound.Adults.ToString());
            await Driver.FindVisibleSelectElementAsync(BookingDetails.OutboundChildrenSelect).ThenSelectByValue(outbound.Children.ToString());
            await Driver.FindVisibleSelectElementAsync(BookingDetails.OutboundSeniorsSelect).ThenSelectByValue(outbound.Seniors.ToString());
            await Driver.FindVisibleSelectElementAsync(BookingDetails.OutboundInfantsSelect).ThenSelectByValue(outbound.Infants.ToString());

            var outboundVehicleValue = outbound.Vehicle.GetAttribute().OptionValue;
            if (outboundVehicleValue == null)
            {
                return BookStatus.VehicleNotFound;
            }
            await Driver.FindVisibleSelectElementAsync(BookingDetails.OutboundVehicleSelect).ThenSelectByValue(outboundVehicleValue);

            // Return
            await Driver.FindVisibleSelectElementAsync(BookingDetails.ReturnCrossingSelect).ThenSelectByIndex((int)@return.Crossing);

            Driver.SetValueWithScript(BookingDetails.ReturnDepartureCalendar, @return.Departure.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            await Driver.FindVisibleSelectElementAsync(BookingDetails.ReturnPassengersSelect).ThenSelectByValue(@return.VehiclePassengers.ToString());
            await Driver.FindVisibleSelectElementAsync(BookingDetails.ReturnAdultsSelect).ThenSelectByValue(@return.Adults.ToString());
            await Driver.FindVisibleSelectElementAsync(BookingDetails.ReturnChildrenSelect).ThenSelectByValue(@return.Children.ToString());
            await Driver.FindVisibleSelectElementAsync(BookingDetails.ReturnSeniorsSelect).ThenSelectByValue(@return.Seniors.ToString());
            await Driver.FindVisibleSelectElementAsync(BookingDetails.ReturnInfantsSelect).ThenSelectByValue(@return.Infants.ToString());

            var returnVehicleValue = @return.Vehicle.GetAttribute().OptionValue;
            if (returnVehicleValue == null)
            {
                return BookStatus.VehicleNotFound;
            }
            await Driver.FindVisibleSelectElementAsync(BookingDetails.ReturnVehicleSelect).ThenSelectByValue(returnVehicleValue);

            await Driver.FindVisibleElementAsync(BookingDetails.NextButton).ThenClick();

            // Departure Select
            await Driver.WaitForElementToAppearAsync(DepartureSelect.DepartureTable);
            var outboundDepartureRadio = await Driver.TryFindVisibleElementAsync(DepartureSelect.OutboundDepartureRadio(outbound.Departure.Value));
            if (outboundDepartureRadio == null)
            {
                return BookStatus.OutboundDepartureNotFound;
            }
            outboundDepartureRadio.Click();

            await Driver.WaitForElementToAppearAsync(DepartureSelect.DepartureTable, 2);
            var returnDepartureRadio = await Driver.TryFindVisibleElementAsync(DepartureSelect.ReturnDepartureRadio(@return.Departure.Value));
            if (returnDepartureRadio == null)
            {
                return BookStatus.ReturnDepartureNotFound;
            }
            returnDepartureRadio.Click();

            await Driver.FindVisibleElementAsync(DepartureSelect.NextButton).ThenClick();

            // Contact Information
            var lastNameNext = await Driver.FindVisibleElementAsync(ContactInformation.LastNameText);
            lastNameNext.Clear();
            lastNameNext.SendKeys(customer.Name);

            var mobileText = await Driver.FindVisibleElementAsync(ContactInformation.MobileText);
            mobileText.Clear();
            mobileText.SendKeys(customer.PhoneNumber);

            var emailText = await Driver.FindVisibleElementAsync(ContactInformation.EmailText);
            emailText.Clear();
            emailText.SendKeys(customer.Email);

            await Driver.FindVisibleElementAsync(ContactInformation.TermsCheckbox).ThenClick();

            await Driver.FindVisibleElementAsync(ContactInformation.NextButton).ThenClick();

            // Confirmation
            var bookingNumberDiv = await Driver.FindVisibleElementAsync(Confirmation.BookingNumberDiv);
            var bookingPasswordDiv = await Driver.FindVisibleElementAsync(Confirmation.BookingPasswordDiv);

            BookingNumber = bookingNumberDiv.Text;
            BookingPassword = bookingPasswordDiv.Text;

            var totalPriceSpan = Driver.FindElements(Confirmation.TotalPriceSpans).First();
            Price = decimal.Parse(totalPriceSpan.Text.Replace(" DKK", string.Empty).Replace(',', '.'), CultureInfo.InvariantCulture);

            return BookStatus.Success;
        }

        private static class BookingDetails
        {
            public static readonly By ItRoundTripRadio = By.Id("cw-bookingflow-IT");

            public static readonly By LoadingSpinner = By.ClassName("cw-loading-mask-spinner");

            public static readonly By CopyDetailsCheckbox = By.Name("journeysearch-passengers-copy");

            public static readonly By OutboundCrossingSelect = By.Id("j1_route-j1_route");
            public const string OutboundDepartureCalendar = "input.cw-journeysearch-calendar-1";
            public static readonly By OutboundPassengersSelect = By.Id("cw-journeysearch-pax-1-500");
            public static readonly By OutboundAdultsSelect = By.Id("cw-journeysearch-pax-1-1");
            public static readonly By OutboundChildrenSelect = By.Id("cw-journeysearch-pax-1-3");
            public static readonly By OutboundSeniorsSelect = By.Id("cw-journeysearch-pax-1-4");
            public static readonly By OutboundInfantsSelect = By.Id("cw-journeysearch-pax-1-2");
            public static readonly By OutboundVehicleSelect = By.Name("cw_journeysearch_j1_vehicles[0][ctg]");

            public static readonly By ReturnCrossingSelect = By.Id("j2_route-j2_route");
            public const string ReturnDepartureCalendar = "input.cw-journeysearch-calendar-2";
            public static readonly By ReturnPassengersSelect = By.Id("cw-journeysearch-pax-2-500");
            public static readonly By ReturnAdultsSelect = By.Id("cw-journeysearch-pax-2-1");
            public static readonly By ReturnChildrenSelect = By.Id("cw-journeysearch-pax-2-3");
            public static readonly By ReturnSeniorsSelect = By.Id("cw-journeysearch-pax-2-4");
            public static readonly By ReturnInfantsSelect = By.Id("cw-journeysearch-pax-2-2");
            public static readonly By ReturnVehicleSelect = By.Name("cw_journeysearch_j2_vehicles[0][ctg]");

            public static readonly By NextButton = By.CssSelector("button.cw-action-next");
        }

        private static class DepartureSelect
        {
            public static readonly By DepartureTable = By.ClassName("choosejourney-departures");
            public static By OutboundDepartureRadio(DateTime departure) => By.CssSelector($"input[name=\"cw_choosejourney_j1_departures\"][data-cw-departure-datetime=\"{departure.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}\"]");
            public static By ReturnDepartureRadio(DateTime departure) => By.CssSelector($"input[name=\"cw_choosejourney_j2_departures\"][data-cw-departure-datetime=\"{departure.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}\"]");
            public static readonly By NextButton = By.CssSelector("button.cw-action-next");
        }

        private static class ContactInformation
        {
            public static readonly By LastNameText = By.Id("lastName");
            public static readonly By MobileText = By.Id("mobile");
            public static readonly By EmailText = By.Id("email");
            public static readonly By TermsCheckbox = By.Id("acceptTerms");
            public static readonly By NextButton = By.CssSelector("button.cw-action-next");
        }

        private static class Confirmation
        {
            public static readonly By BookingNumberDiv = By.CssSelector("div.cw-booking-code");
            public static readonly By BookingPasswordDiv = By.ClassName("cw-booking-pwd");
            public static readonly By TotalPriceSpans = By.ClassName("total-label-price");
        }
    }
}