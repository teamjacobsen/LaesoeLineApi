using System;
using System.ComponentModel.DataAnnotations;

namespace LaesoeLineApi.Features.Agent.Models
{
    public class Journey
    {
        [Required]
        public Crossing? Crossing { get; set; }

        [Required]
        public DateTime Departure { get; set; }

        public int Seniors { get; set; }

        public int Adults { get; set; } = 1;

        public int Children { get; set; }

        public int Infants { get; set; }

        public int VehiclePassengers { get; set; }

        public Vehicle Vehicle { get; set; } = Vehicle.None;
    }
}
