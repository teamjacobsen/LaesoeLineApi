using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaesoeLineApi.Features.CustomerBooking.Models
{
    public class Journey
    {
        [Required]
        public Crossing? Crossing { get; set; }

        [Required]
        public DateTime Departure { get; set; }

        [Range(1, int.MaxValue)]
        public int Passengers { get; set; } = 1;

        public Vehicle Vehicle { get; set; } = Vehicle.Car;
    }
}
