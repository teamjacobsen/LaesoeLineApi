﻿namespace LaesoeLineApi.CustomerBooking
{
    public class CustomerBookingBookRoundTrip
    {
        public Journey Outbound { get; set; } = new Journey();
        public Journey Return { get; set; } = new Journey();
        public bool Local { get; set; }
    }
}
