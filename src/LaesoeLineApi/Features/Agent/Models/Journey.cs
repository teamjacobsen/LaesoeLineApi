using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaesoeLineApi.Features.Agent.Models
{
    public class Journey : IValidatableObject
    {
        [Required]
        public Crossing? Crossing { get; set; }

        [Required]
        public DateTime Departure { get; set; }

        public int Seniors { get; set; }

        public int Adults { get; set; }

        public int Children { get; set; }

        public int Infants { get; set; }

        public int VehiclePassengers { get; set; }

        public Vehicle Vehicle { get; set; } = Vehicle.None;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Vehicle == Vehicle.None)
            {
                if (Seniors + Adults + Children + Infants == 0)
                {
                    yield return new ValidationResult("At least one passenger is required when there is not vehicle");
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
