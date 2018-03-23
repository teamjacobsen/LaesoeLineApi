using LaesoeLineApi.Features.MyBooking.Pages;
using LaesoeLineApi.Selenium;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.MyBooking
{
    [Route("[controller]")]
    public class MyBookingController : ControllerBase
    {
        private readonly IWebDriverFactory _webDriverFactory;

        public MyBookingController(IWebDriverFactory webDriverFactory)
        {
            _webDriverFactory = webDriverFactory;
        }

        [HttpDelete("Bookings/{bookingNumber}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Cancel(string bookingNumber, [Required] string bookingPassword)
        {
            using (var driver = await _webDriverFactory.CreateAsync())
            {
                var myBookingPage = await driver.GoToAsync<MyBookingPage>();
                await myBookingPage.LoginAsync(bookingNumber, bookingPassword);

                var bookingConfirmationPage = await driver.GoToAsync<BookingConfirmationPage>();
                await bookingConfirmationPage.CancelAsync();

                return NoContent();
            }
        }
    }
}
