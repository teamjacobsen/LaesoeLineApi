using System.ComponentModel.DataAnnotations;

namespace LaesoeLineApi.Features.CustomerBooking
{
    public class BookOneWay
    {
        [Required]
        public Journey Journey { get; set; }

        public bool Local { get; set; }
    }
}