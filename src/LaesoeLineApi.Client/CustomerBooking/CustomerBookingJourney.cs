using System;

namespace LaesoeLineApi.CustomerBooking
{
    public class CustomerBookingJourney
    {
        public Crossing Crossing { get; set; }
        public DateTime Departure { get; set; }
        public int Passengers { get; set; } = 1;
        public Vehicle Vehicle { get; set; } = Vehicle.Car;
    }
}
