using System;

namespace ExampleClient.Api
{
    public class AgentBookingJourney
    {
        public Crossing Crossing { get; set; }
        public DateTime? Departure { get; set; }
        public int? Seniors { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public int? Infants { get; set; }
        public int? VehiclePassengers { get; set; }
        public string Vehicle { get; set; }
    }
}
