using System;

namespace LaesoeLineApi
{
    public class CustomerBookingJourney
    {
        public Crossing Crossing { get; set; }
        public DateTime Departure { get; set; }
        public int Passengers { get; set; } = 1;
        public string Vehicle { get; set; } = VehicleType.Car;
    }
}
