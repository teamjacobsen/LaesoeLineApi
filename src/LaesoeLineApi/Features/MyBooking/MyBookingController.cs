using LaesoeLineApi.Features.MyBooking.Pages;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.MyBooking
{
    [Route("[controller]")]
    public class MyBookingController : ControllerBase
    {
        private readonly IWebDriver _webDriver;

        public MyBookingController(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        [HttpDelete("Bookings/{bookingNumber}")]
        public async Task<IActionResult> Cancel(string bookingNumber, string bookingPassword)
        {
            await Task.Factory.StartNew(() =>
            {
                var myBookingPage = _webDriver.GoTo<MyBookingPage>();
                myBookingPage.Login(bookingNumber, bookingPassword);

                var bookingConfirmationPage = _webDriver.GoTo<BookingConfirmationPage>();
                bookingConfirmationPage.Cancel();
            });

            return NoContent();
        }
    }
}
