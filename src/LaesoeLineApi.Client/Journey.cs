using System;

namespace LaesoeLineApi
{
    public class Journey
    {
        public Crossing Crossing { get; set; }
        public DateTime? Departure { get; set; }
        public int? Seniors { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public int? Infants { get; set; }
        public int? VehiclePassengers { get; set; }
        public Vehicle? Vehicle { get; set; }
    }
}
