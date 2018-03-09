using System.ComponentModel.DataAnnotations;

namespace LaesoeLineApi.Features.Customer.Models
{
    public class BookOneWay
    {
        [Required]
        public Journey Journey { get; set; }

        public bool Local { get; set; }
    }
}