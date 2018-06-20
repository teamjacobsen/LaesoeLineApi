using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookOneDayDetailsPage : BookingDetailsPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/endagspakke/Rejsedetaljer/";

        public BookOneDayDetailsPage(IBrowserSession session, ILogger<BookOneDayDetailsPage> logger)
            : base(session, logger)
        {
        }

        public Task EnterDetailsAsync(Journey outbound, Journey @return)
        {
            if (outbound.Vehicle != Vehicle.None || @return.Vehicle != Vehicle.None)
            {
                throw new ApiException(ApiStatus.VehicleNotValid);
            }

            return ExecuteWithRetry(async () =>
            {
                await PopulateCopyDetails(false);

                await PopulateOutboundCrossingAsync(outbound.Crossing.Value);
                await PopulateOutboundDepartureCalendarAsync(outbound.Departure.Value);
                await PopulateOutboundAdultsSelectAsync(outbound.Adults ?? 0);
                await PopulateOutboundChildrenSelectAsync(outbound.Children ?? 0);

                await PopulateReturnCrossingAsync(@return.Crossing.Value);
                await PopulateReturnDepartureCalendarAsync(@return.Departure.Value);
                await PopulateReturnAdultsSelectAsync(@return.Adults ?? 0);
                await PopulateReturnChildrenSelectAsync(@return.Children ?? 0);

                await GoToNextStepAsync();
            });
        }
    }
}
