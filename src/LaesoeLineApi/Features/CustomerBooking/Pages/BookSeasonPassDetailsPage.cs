using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookSeasonPassDetailsPage : BookingDetailsPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/aarskort-2018/Rejsedetaljer/";

        public BookSeasonPassDetailsPage(IBrowserSession session, ILogger<BookSeasonPassDetailsPage> logger)
            : base(session, logger)
        {
        }

        public Task EnterDetailsAsync(Journey journey, bool local)
        {
            return ExecuteWithRetry(async () =>
            {
                await PopulateBookingFlowRadioAsync(GetOneWayBookingFlow(local));

                await PopulateOutboundAsync(journey.Crossing.Value, journey.Departure.Value, journey.Vehicle.GetAttribute().SeasonPassOptionValue, journey.VehiclePassengers.Value);

                await GoToNextStepAsync();
            });
        }

        public Task EnterDetailsAsync(Journey outbound, Journey @return, bool local)
        {
            return ExecuteWithRetry(async () =>
            {
                await PopulateBookingFlowRadioAsync(GetRoundTripBookingFlow(local));

                await PopulateCopyDetails(false);

                await PopulateOutboundAsync(outbound.Crossing.Value, outbound.Departure.Value, outbound.Vehicle.GetAttribute().SeasonPassOptionValue, outbound.VehiclePassengers.Value);

                await PopulateReturnAsync(@return.Crossing.Value, @return.Departure.Value, @return.Vehicle.GetAttribute().SeasonPassOptionValue, @return.VehiclePassengers.Value);

                await GoToNextStepAsync();
            });
        }

        private static By GetOneWayBookingFlow(bool local) => local ? SeasonPassLocalOneWayRadio : SeasonPassOneWayRadio;
        private static By GetRoundTripBookingFlow(bool local) => local ? SeasonPassLocalRoundTripRadio : SeasonPassRoundTripRadio;
    }
}
