using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.Timetable.Pages
{
    public class BookPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/book/obo-2018/Rejsedetaljer/";
        public IWebDriver Driver { get; private set; }

        public BookPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public async Task<IDictionary<DateTime, (DateTime Departure, bool IsAvailable)[]>> GetDeparturesAsync(Crossing crossing, Vehicle vehicle, DateTime date, int days)
        {
            var result = new Dictionary<DateTime, (DateTime Departure, bool Available)[]>();

            // Booking Details
            await Driver.FindVisibleElementAsync(BookingDetails.LocalVehicleOneWayRadio).ThenClick();

            //await Driver.TryWaitForElementToAppearAsync(LoadingSpinner, timeout: TimeSpan.FromMilliseconds(100));
            await Driver.WaitForElementToDisappearAsync(LoadingSpinner);

            await Driver.FindVisibleSelectElementAsync(BookingDetails.CrossingSelect).ThenSelectByIndex((int)crossing);

            Driver.SetValueWithScript(BookingDetails.DepartureCalendar, date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            await Driver.FindVisibleSelectElementAsync(BookingDetails.PassengersSelect).ThenSelectByValue("1");

            var vehicleValue = vehicle.GetAttribute().OptionValue;
            if (vehicleValue == null)
            {
                return null;
            }
            await Driver.FindVisibleSelectElementAsync(BookingDetails.VehicleSelect).ThenSelectByValue(vehicleValue);

            await Driver.FindVisibleElementAsync(BookingDetails.NextButton).ThenClick();

            // Depearture Select
            await Driver.WaitForElementToAppearAsync(DepartureSelect.DepartureTable);

            foreach (var day in Enumerable.Range(0, days).Select(x => date.AddDays(x)))
            {
                if (day != date)
                {
                    await Driver.FindVisibleElementAsync(DepartureSelect.LaterDeparturesLink).ThenClick();

                    //await Driver.TryWaitForElementToAppearAsync(LoadingSpinner, timeout: TimeSpan.FromMilliseconds(100));
                    await Driver.WaitForElementToDisappearAsync(LoadingSpinner);
                }

                var departureTables = Driver.FindElements(DepartureSelect.DepartureTable).Where(x => x.Displayed);

                if (!departureTables.Any())
                {
                    // Ran to the end of the public timetable
                    break;
                }

                var dayDepartures = new List<(DateTime Departure, bool Available)>();
                foreach (var row in Driver.FindElements(DepartureSelect.DepartureRows).Where(x => x.Displayed))
                {
                    var timeString = row.FindElement(By.ClassName("departs")).Text.Trim();
                    var time = TimeSpan.ParseExact(timeString, @"hh\.mm", CultureInfo.InvariantCulture);

                    var soldOut = row.FindElements(By.ClassName("cw-choosejourney-inactive")).Count > 0;

                    dayDepartures.Add((day.Add(time), !soldOut));
                }

                result[day] = dayDepartures.ToArray();
            }

            return result;
        }

        private static readonly By LoadingSpinner = By.ClassName("cw-loading-mask-spinner");

        // Booking Details
        private static class BookingDetails
        {
            public static readonly By LocalVehicleOneWayRadio = By.Id("cw-bookingflow-OBO18ENK");
            public static readonly By CrossingSelect = By.Id("j1_route-j1_route");
            public static readonly string DepartureCalendar = "input.cw-journeysearch-calendar-1";
            public static readonly By PassengersSelect = By.Id("cw-journeysearch-pax-1-500");
            public static readonly By VehicleSelect = By.Name("cw_journeysearch_j1_vehicles[0][ctg]");
            public static readonly By NextButton = By.CssSelector("button.cw-action-next");
        }

        // Departure Select
        private static class DepartureSelect
        {
            public static readonly By DepartureTable = By.ClassName("choosejourney-departures");
            public static readonly By DepartureRows = By.ClassName("cw-choosejourney-row-day");
            public static readonly By LaterDeparturesLink = By.CssSelector("span[data-cw-action=\"laterDepartures\"]");
        }
    }
}