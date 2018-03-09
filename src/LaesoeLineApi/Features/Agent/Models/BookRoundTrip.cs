using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaesoeLineApi.Features.Agent.Models
{
    public class BookRoundTrip : IValidatableObject
    {
        [Required]
        public Guest Customer { get; set; }

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
