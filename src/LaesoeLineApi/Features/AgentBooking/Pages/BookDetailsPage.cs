﻿using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.AgentBooking.Pages
{
    public class BookDetailsPage : BookingDetailsPageBase
    {
        private readonly IBrowserSession _session;
        private readonly ILogger<BookDetailsPage> _logger;

        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/it/Rejsedetaljer/";

        public BookDetailsPage(IBrowserSession session, ILogger<BookDetailsPage> logger)
            : base(session, logger)
        {
            _session = session;
            _logger = logger;
        }

        public Task EnterDetailsAsync(Journey journey)
        {
            return ExecuteWithRetry(async () =>
            {
                await PopulateBookingFlowRadioAsync(GetBookingFlow());

                if (journey.Vehicle == Vehicle.None)
                {
                    await PopulateOutboundAsync(journey.Crossing.Value, journey.Departure.Value, journey.Adults ?? 0, journey.Children ?? 0, journey.Seniors ?? 0, journey.Infants ?? 0);
                }
                else
                {
                    await PopulateOutboundAsync(journey.Crossing.Value, journey.Departure.Value, journey.Vehicle.GetAttribute().OptionValue, journey.VehiclePassengers.Value);
                }

                await GoToNextStepAsync();
            });
        }

        public Task EnterDetailsAsync(Journey outbound, Journey @return)
        {
            return ExecuteWithRetry(async () =>
            {
                await PopulateBookingFlowRadioAsync(GetBookingFlow());

                await PopulateCopyDetails(false);

                if (outbound.Vehicle == Vehicle.None)
                {
                    await PopulateOutboundAsync(outbound.Crossing.Value, outbound.Departure.Value, outbound.Adults ?? 0, outbound.Children ?? 0, outbound.Seniors ?? 0, outbound.Infants ?? 0);
                }
                else
                {
                    await PopulateOutboundAsync(outbound.Crossing.Value, outbound.Departure.Value, outbound.Vehicle.GetAttribute().OptionValue, outbound.VehiclePassengers.Value);
                }

                if (@return.Vehicle == Vehicle.None)
                {
                    await PopulateReturnAsync(@return.Crossing.Value, @return.Departure.Value, @return.Adults ?? 0, @return.Children ?? 0, @return.Seniors ?? 0, @return.Infants ?? 0);
                }
                else
                {
                    await PopulateReturnAsync(@return.Crossing.Value, @return.Departure.Value, @return.Vehicle.GetAttribute().OptionValue, @return.VehiclePassengers.Value);
                }

                await GoToNextStepAsync();
            });
        }

        private static By GetBookingFlow() => ItRoundTripRadio;
    }
}