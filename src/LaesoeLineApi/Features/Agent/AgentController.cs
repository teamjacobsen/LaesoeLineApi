using LaesoeLineApi.Features.Agent.Models;
using LaesoeLineApi.Features.Agent.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.Agent
{
    [Route("[controller]")]
    public class AgentController : ControllerBase
    {
        private readonly IWebDriver _webDriver;

        public AgentController(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        [HttpPost("Book/It/RoundTrip")]
        public async Task<IActionResult> BookItRoundTrip([Required] [FromBody] BookRoundTrip command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var credentials = Request.GetCredentials();

            if (credentials == null)
            {
                return Unauthorized();
            }

            var bookResult = new BookResult();
            var status = await Task.Factory.StartNew(() =>
            {
                var loginPage = _webDriver.GoTo<LoginPage>();
                loginPage.Login(credentials.Username, credentials.Password);

                var bookPage = _webDriver.GoTo<BookPage>();
                var result = bookPage.BookItRoundTrip(command.Customer, command.Outbound, command.Return);
                bookResult.BookingNumber = bookPage.BookingNumber;
                bookResult.BookingPassword = bookPage.BookingPassword;
                bookResult.Price = bookPage.Price;

                return result;
            });

            if (status == BookStatus.Success)
            {
                return Ok(bookResult);
            }
            else
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, status);
            }
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
