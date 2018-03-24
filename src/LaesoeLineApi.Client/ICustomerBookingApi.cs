﻿using Refit;
using System.Threading.Tasks;

namespace LaesoeLineApi
{
    public interface ICustomerBookingApi
    {
        [Post("/CustomerBooking/Book/SeasonPass/OneWay")]
        Task<CustomerBookingBookSuccessResult> BookSeasonPassOneWay(CustomerBookingBookOneWay command);

        [Post("/CustomerBooking/Book/SeasonPass/RoundTrip")]
        Task<CustomerBookingBookSuccessResult> BookSeasonPassRoundTrip(CustomerBookingBookRoundTrip command);

        [Delete("/CustomerBooking/Bookings/{bookingNumber}")]
        Task CancelAsync(string bookingNumber);
    }
}