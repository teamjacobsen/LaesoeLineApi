using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaesoeLineApi.Features.AgentBooking.Models
{
    public class Journey : IValidatableObject
    {
        [Required]
        public Crossing? Crossing { get; set; }

        [Required]
        public DateTime? Departure { get; set; }

        [Range(0, 9)]
        public int? Seniors { get; set; }

        [Range(0, 9)]
        public int? Adults { get; set; }

        [Range(0, 9)]
        public int? Children { get; set; }

        [Range(0, 9)]
        public int? Infants { get; set; }

        [Range(0, 9)]
        public int? VehiclePassengers { get; set; }

        public Vehicle Vehicle { get; set; } = Vehicle.None;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Vehicle == Vehicle.None)
            {
                if (Seniors + Adults + Children + Infants == 0)
                {
                    yield return new ValidationResult("At least one passenger is required when no vehicle is specified");
                }

                if (VehiclePassengers > 0)
                {
                    yield return new ValidationResult("No vehicle passengers are allowed");
                }
            }
            else
            {
                if (Seniors + Adults + Children + Infants > 0)
                {
                    yield return new ValidationResult("Only vehicle passengers are allowed");
                }

                if (VehiclePassengers == 0)
                {
                    yield return new ValidationResult("At least one vehicle passenger is required");
                }
            }
        }
    }
}
