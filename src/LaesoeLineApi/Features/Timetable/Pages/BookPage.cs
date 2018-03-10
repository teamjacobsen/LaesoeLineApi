using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LaesoeLineApi.Features.Timetable.Pages
{
    public class BookPage : IPage
    {
        public string Url { get; } = "https://booking.laesoe-line.dk/dk/book/obo-2018/Rejsedetaljer/";
        public IWebDriver Driver { get; private set; }

        private static readonly By LoadingSpinnerSelector = By.ClassName("cw-loading-mask-spinner");

        // Booking Details
        private IWebElement LocalVehicleOneWayRadio => Driver.FindVisibleElement(By.Id("cw-bookingflow-OBO18ENK"));

        private IWebElement CrossingSelect => Driver.FindVisibleElement(By.Id("j1_route-j1_route"));
        private const string DepartureCalendarCssSelector = "input.cw-journeysearch-calendar-1";
        private IWebElement PassengersSelect => Driver.FindVisibleElement(By.Id("cw-journeysearch-pax-1-500"));
        private IWebElement VehicleSelect => Driver.FindVisibleElement(By.Name("cw_journeysearch_j1_vehicles[0][ctg]"));

        private IWebElement DetailsNextButton => Driver.FindVisibleElement(By.CssSelector("button.cw-action-next"));

        // Departure Select
        private static readonly By DepartureTableSelector = By.ClassName("choosejourney-departures");
        private IEnumerable<IWebElement> DepartureRows => Driver.FindVisibleElements(By.ClassName("cw-choosejourney-row-day"));

        private IWebElement LaterDeparturesLink => Driver.FindVisibleElement(By.CssSelector("span[data-cw-action=\"laterDepartures\"]"));

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

        public IDictionary<DateTime, (DateTime Departure, bool IsAvailable)[]> GetDepartures(Crossing crossing, Vehicle vehicle, DateTime date, int days)
        {
            var result = new Dictionary<DateTime, (DateTime Departure, bool Available)[]>();

            // Booking Details
            LocalVehicleOneWayRadio.Click();

            Driver.WaitForElementToDisappear(LoadingSpinnerSelector);

            CrossingSelect.SelectByIndex((int)crossing);
            Driver.SetValueWithScript(DepartureCalendarCssSelector, date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            PassengersSelect.SelectByValue("1");
            if (!VehicleValues.TryGetValue(vehicle, out var vehicleValue))
            {
                return null;
            }
            VehicleSelect.SelectByValue(vehicleValue);

            DetailsNextButton.Click();

            // Depearture Select
            Driver.WaitForElementToAppear(DepartureTableSelector);
            foreach (var day in Enumerable.Range(0, days).Select(x => date.AddDays(x)))
            {
                if (day != date)
                {
                    LaterDeparturesLink.Click();
                    Driver.WaitForElementToDisappear(LoadingSpinnerSelector);
                }

                var dayDepartures = new List<(DateTime Departure, bool Available)>();
                foreach (var row in DepartureRows)
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
    }
}
