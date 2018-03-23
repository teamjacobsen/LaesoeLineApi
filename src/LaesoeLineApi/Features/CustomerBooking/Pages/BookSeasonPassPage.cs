using LaesoeLineApi.Features.CustomerBooking.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookSeasonPassPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/book/aarskort-2018/Rejsedetaljer/";
        public IWebDriver Driver { get; private set; }
        public string BookingNumber { get; set; }
        public string BookingPassword { get; set; }

        public BookSeasonPassPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public async Task<BookStatus> BookOneWayAsync(Journey journey, bool local)
        {
            // Booking Details
            if (local)
            {
                await Driver.FindVisibleElementAsync(BookingDetails.SeasonPassLocalOneWayRadio).ThenClick();
            }
            else
            {
                await Driver.FindVisibleElementAsync(BookingDetails.SeasonPassOneWayRadio).ThenClick();
            }

            await Driver.WaitForElementToDisappearAsync(BookingDetails.LoadingSpinner);

            await Driver.FindVisibleSelectElementAsync(BookingDetails.OutboundCrossingSelect).ThenSelectByIndex((int)journey.Crossing);

            Driver.SetValueWithScript(BookingDetails.OutboundDepartureCalendar, journey.Departure.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            await Driver.FindVisibleSelectElementAsync(BookingDetails.OutboundPassengersSelect).ThenSelectByValue(journey.Passengers.ToString());

            var vehicleValue = journey.Vehicle.GetAttribute().SeasonPassOptionValue;
            if (vehicleValue == null)
            {
                return BookStatus.VehicleNotFound;
            }
            await Driver.FindVisibleSelectElementAsync(BookingDetails.OutboundVehicleSelect).ThenSelectByValue(vehicleValue);

            await Driver.FindVisibleElementAsync(BookingDetails.NextButton).ThenClick();

            // Depearture Select
            await Driver.WaitForElementToAppearAsync(DepartureSelect.DepartureTable);
            var departureRadio = await Driver.TryFindVisibleElementAsync(DepartureSelect.OutboundDepartureRadio(journey.Departure.Value));
            if (departureRadio == null)
            {
                return BookStatus.DepartureNotFound;
            }
            departureRadio.Click();

            await Driver.FindVisibleElementAsync(DepartureSelect.NextButton).ThenClick();

            // Contact Information
            await Driver.FindVisibleElementAsync(ContactInformation.TermsCheckbox).ThenClick();

            await Driver.FindVisibleElementAsync(ContactInformation.NextButton).ThenClick();

            // Confirmation
            var bookingNumberDiv = await Driver.FindVisibleElementAsync(Confirmation.BookingNumberDiv);
            var bookingPasswordDiv = await Driver.FindVisibleElementAsync(Confirmation.BookingPasswordDiv);
            BookingNumber = bookingNumberDiv.Text;
            BookingPassword = bookingPasswordDiv.Text;

            return BookStatus.Success;
        }

        public async Task<BookStatus> BookRoundTripAsync(Journey outbound, Journey @return, bool local)
        {
            // Booking Details
            if (local)
            {
                await Driver.FindVisibleElementAsync(BookingDetails.SeasonPassLocalRoundTripRadio).ThenClick();
            }
            else
            {
                await Driver.FindVisibleElementAsync(BookingDetails.SeasonPassRoundTripRadio).ThenClick();
            }

            //await Driver.TryWaitForElementToAppearAsync(BookingDetails.LoadingSpinner, timeout: TimeSpan.FromMilliseconds(100));
            await Driver.WaitForElementToDisappearAsync(BookingDetails.LoadingSpinner);

            await Driver.FindVisibleElementAsync(BookingDetails.CopyDetailsCheckbox).ThenClick();

            // Outbound
            await Driver.FindVisibleSelectElementAsync(BookingDetails.OutboundCrossingSelect).ThenSelectByIndex((int)outbound.Crossing);

            Driver.SetValueWithScript(BookingDetails.OutboundDepartureCalendar, outbound.Departure.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            await Driver.FindVisibleSelectElementAsync(BookingDetails.OutboundPassengersSelect).ThenSelectByValue(outbound.Passengers.ToString());

            var outboundVehicleValue = outbound.Vehicle.GetAttribute().SeasonPassOptionValue;
            if (outboundVehicleValue == null)
            {
                return BookStatus.VehicleNotFound;
            }
            await Driver.FindVisibleSelectElementAsync(BookingDetails.OutboundVehicleSelect).ThenSelectByValue(outboundVehicleValue);

            // Return
            await Driver.FindVisibleSelectElementAsync(BookingDetails.ReturnCrossingSelect).ThenSelectByIndex((int)@return.Crossing);

            Driver.SetValueWithScript(BookingDetails.ReturnDepartureCalendar, @return.Departure.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            await Driver.FindVisibleSelectElementAsync(BookingDetails.ReturnPassengersSelect).ThenSelectByValue(@return.Passengers.ToString());

            var returnVehicleValue = @return.Vehicle.GetAttribute().SeasonPassOptionValue;
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
            await Driver.FindVisibleElementAsync(ContactInformation.TermsCheckbox).ThenClick();

            await Driver.FindVisibleElementAsync(ContactInformation.NextButton).ThenClick();

            // Confirmation
            var bookingNumberDiv = await Driver.FindVisibleElementAsync(Confirmation.BookingNumberDiv);
            var bookingPasswordDiv = await Driver.FindVisibleElementAsync(Confirmation.BookingPasswordDiv);
            BookingNumber = bookingNumberDiv.Text;
            BookingPassword = bookingPasswordDiv.Text;

            return BookStatus.Success;
        }

        private static class BookingDetails
        {
            public static readonly By SeasonPassOneWayRadio = By.Id("cw-bookingflow-ÅRS18ENK");
            public static readonly By SeasonPassRoundTripRadio = By.Id("cw-bookingflow-ÅRSRET18");
            public static readonly By SeasonPassLocalOneWayRadio = By.Id("cw-bookingflow-AAKOE18");
            public static readonly By SeasonPassLocalRoundTripRadio = By.Id("cw-bookingflow-AAKOR18");

            public static readonly By LoadingSpinner = By.ClassName("cw-loading-mask-spinner");

            public static readonly By CopyDetailsCheckbox = By.Name("journeysearch-passengers-copy");

            public static readonly By OutboundCrossingSelect = By.Id("j1_route-j1_route");
            public const string OutboundDepartureCalendar = "input.cw-journeysearch-calendar-1";
            public static readonly By OutboundPassengersSelect = By.Id("cw-journeysearch-pax-1-500");
            public static readonly By OutboundVehicleSelect = By.Name("cw_journeysearch_j1_vehicles[0][ctg]");

            public static readonly By ReturnCrossingSelect = By.Id("j2_route-j2_route");
            public const string ReturnDepartureCalendar = "input.cw-journeysearch-calendar-2";
            public static readonly By ReturnPassengersSelect = By.Id("cw-journeysearch-pax-2-500");
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
            public static readonly By TermsCheckbox = By.Id("acceptTerms");
            public static readonly By NextButton = By.CssSelector("button.cw-action-next");
        }

        private static class Confirmation
        {
            public static readonly By BookingNumberDiv = By.CssSelector("div.cw-booking-code");
            public static readonly By BookingPasswordDiv = By.ClassName("cw-booking-pwd");
        }
    }
}
