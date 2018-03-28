using LaesoeLineApi.Features.MyBooking.Pages;
using LaesoeLineApi.Selenium;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.MyBooking
{
    [Route("[controller]")]
    public class MyBookingController : ControllerBase
    {
        private readonly IBrowserSessionFactory _browserSessionFactory;

        public MyBookingController(IBrowserSessionFactory browserSessionFactory)
        {
            _browserSessionFactory = browserSessionFactory;
        }

        [HttpDelete("Bookings/{bookingNumber}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Cancel(string bookingNumber, [Required] string bookingPassword)
        {
            using (var driver = _browserSessionFactory.CreateSession())
            {
                var myBookingPage = await driver.GoToAsync<MyBookingPage>();
                await myBookingPage.LoginAsync(bookingNumber, bookingPassword);
                await myBookingPage.CancelAsync();

                return NoContent();
            }
        }
    }
}
