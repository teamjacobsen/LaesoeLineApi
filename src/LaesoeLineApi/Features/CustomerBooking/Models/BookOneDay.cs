﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaesoeLineApi.Features.CustomerBooking
{
    public class BookOneDay : IValidatableObject
    {
        [Required]
        public Customer Customer { get; set; }

        [Required]
        public Journey Outbound { get; set; }

        [Required]
        public Journey Return { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Outbound?.Crossing == Return?.Crossing)
            {
                yield return new ValidationResult("The outbound and return crossing must be different");
            }
        }
    }
}
