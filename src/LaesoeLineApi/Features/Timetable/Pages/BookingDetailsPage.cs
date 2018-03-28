using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.Timetable.Pages
{
    public class BookingDetailsPage : BookingDetailsPageBase
    {
        private readonly IBrowserSession _session;
        private readonly ILogger<BookingDetailsPage> _logger;

        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/obo-2018/Rejsedetaljer/";

        public BookingDetailsPage(IBrowserSession session, ILogger<BookingDetailsPage> logger)
            : base(session, logger)
        {
            _session = session;
            _logger = logger;
        }

        public Task EnterDetailsAsync(Crossing crossing, Vehicle vehicle, DateTime date)
        {
            return ExecuteWithRetry(async () =>
            {
                await PopulateBookingFlowRadioAsync(LocalVehicleOneWayRadio);

                await PopulateOutboundAsync(crossing, date, vehicle.GetAttribute().OptionValue, 1);

                await GoToNextStepAsync();
            });
        }
    }
}