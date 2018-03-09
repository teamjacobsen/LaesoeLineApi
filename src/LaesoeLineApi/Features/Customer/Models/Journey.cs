using System;
using System.ComponentModel.DataAnnotations;

namespace LaesoeLineApi.Features.Customer.Models
{
    public class Journey
    {
        [Required]
        public Crossing? Crossing { get; set; }

        [Required]
        public DateTime Departure { get; set; }

        public int Passengers { get; set; } = 1;

        public Vehicle Vehicle { get; set; } = Vehicle.Car;
    }
}
