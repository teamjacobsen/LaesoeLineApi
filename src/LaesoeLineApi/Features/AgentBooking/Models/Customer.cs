using System.ComponentModel.DataAnnotations;

namespace LaesoeLineApi.Features.AgentBooking
{
    public class Customer
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Phone number prefixed with country code, e.g. +4599999999
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }
    }
}
