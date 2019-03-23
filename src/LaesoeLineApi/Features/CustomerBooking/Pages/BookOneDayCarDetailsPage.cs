using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayCarDetailsPage : BookingDetailsPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/golftur/Rejsedetaljer/";

        public BookOneDayCarDetailsPage(IBrowserSession session, ILogger<BookOneDayFoodDetailsPage> logger)
            : base(session, logger)
        {
        }

        public Task EnterDetailsAsync(Journey outbound, Journey @return)
        {
            if (outbound.Vehicle != Vehicle.OneDayCar || @return.Vehicle != Vehicle.OneDayCar)
            {
                throw new ApiException(ApiStatus.VehicleNotValid);
            }

            return ExecuteWithRetry(async () =>
            {
                await PopulateCopyDetails(false);

                await PopulateOutboundAsync(outbound.Crossing.Value, outbound.Departure.Value, outbound.Vehicle.GetAttribute().OptionValue, outbound.VehiclePassengers.Value);

                await PopulateReturnAsync(@return.Crossing.Value, @return.Departure.Value, @return.Vehicle.GetAttribute().OptionValue, @return.VehiclePassengers.Value);

                await GoToNextStepAsync();
            });
        }
    }
}
