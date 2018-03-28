using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.Timetable.Pages
{
    public class DepartureSelectPage : DepartureSelectPageBase
    {
        private readonly IBrowserSession _session;
        private readonly ILogger<DepartureSelectPage> _logger;

        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/obo-2018/chooseJourney/";

        public DepartureSelectPage(IBrowserSession session, ILogger<DepartureSelectPage> logger)
            : base(session, logger)
        {
            _session = session;
            _logger = logger;
        }

        public async Task<IDictionary<DateTime, (DateTime Departure, VehicleAvailabilityInfo Availability)[]>> GetDeparturesAsync(DateTime date, int days, CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<DateTime, (DateTime Departure, VehicleAvailabilityInfo Availability)[]>();

            foreach (var day in Enumerable.Range(0, days).Select(x => date.AddDays(x)))
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (day != date)
                {
                    await GoToNextDay();
                }

                var departures = await GetDeparturesAsync();

                if (departures == null)
                {
                    break;
                }

                result[day] = departures.ToArray();
            }

            return result;
        }

        private async Task<IList<(DateTime Departure, VehicleAvailabilityInfo Availability)>> GetDeparturesAsync()
        {
            var departureTableIsPresent = await WaitForDepartureTableAsync();

            if (!departureTableIsPresent)
            {
                // Ran to the end of the public timetable
                return null;
            }

            var result = new List<(DateTime Departure, VehicleAvailabilityInfo Availability)>();
            var departureRows = await _session.InvokeAsync(driver => driver.FindElements(DepartureRows).Where(x => x.IsDisplayed()).ToArray());
            foreach (var row in departureRows)
            {
                var dateString = row.FindElement(DateCell).Text.Trim();
                var date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                var timeString = row.FindElement(DepartsCell).Text.Trim();
                var time = TimeSpan.ParseExact(timeString, @"hh\.mm", CultureInfo.InvariantCulture);

                var soldOut = row.FindElements(SoldOutCell).Count > 0;

                result.Add((date.Add(time), new VehicleAvailabilityInfo() { IsAvailable = !soldOut }));
            }

            return result;
        }

        private Task<bool> WaitForDepartureTableAsync()
        {
            return ExecuteWithRetry(async () =>
            {
                try
                {
                    await _session.WaitForElementToAppearAsync(DepartureTable);

                    return true;
                }
                catch (WebDriverTimeoutException)
                {
                }

                await _session.WaitForElementToAppearAsync(NoDeparturesErrorListItem);

                return false;
            });
        }

        private async Task GoToNextDay()
        {
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    await _session.InvokeOnElementAsync(LaterDeparturesLink, x => x.Click());

                    await _session.WaitForElementToDisappearAsync(LoadingSpinner);
                    await _session.WaitForInteractiveReadyStateAsync();

                    return;
                }
                catch (UnhandledAlertException)
                {
                    // At random the site shows an alert with "Hov, der skete en fejl". The alert happens after approximately 20 seconds.
                    // In this case we have not moved to the next date, so we need to retry the "later departures" click.

                    await _session.InvokeAsync(driver => driver.SwitchTo().Alert().Accept());
                }
            }

            throw new ApiException(ApiStatus.GatewayTimeout);
        }

        private static readonly By NoDeparturesErrorListItem = By.ClassName("exception1");

        public static readonly By DepartureRows = By.ClassName("cw-choosejourney-row-day");

        public static readonly By DateCell = By.ClassName("date");
        public static readonly By DepartsCell = By.ClassName("departs");
        public static readonly By SoldOutCell = By.ClassName("cw-choosejourney-inactive");

        public static readonly By LaterDeparturesLink = By.CssSelector("span[data-cw-action=\"laterDepartures\"]");
    }
}