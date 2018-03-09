using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaesoeLineApi.Features.Customer.Models
{
    public class BookRoundTrip : IValidatableObject
    {
        [Required]
        public Journey Outbound { get; set; }

        [Required]
        public Journey Return { get; set; }

        public bool Local { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Outbound?.Crossing == Return?.Crossing)
            {
                yield return new ValidationResult("The outbound and return crossing must be different");
            }
        }
    }
}
