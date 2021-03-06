﻿using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;

namespace LaesoeLineApi.Features.CustomerBooking.Pages
{
    public class BookSeasonPassConfirmationPage : BookingConfirmationPageBase
    {
        public override string Url { get; } = "https://booking.laesoe-line.dk/dk/book/aarskort-2018/bookingConfirmation/";

        public BookSeasonPassConfirmationPage(IBrowserSession session, ILogger<BookSeasonPassConfirmationPage> logger)
            : base(session, logger)
        {
        }
    }
}
