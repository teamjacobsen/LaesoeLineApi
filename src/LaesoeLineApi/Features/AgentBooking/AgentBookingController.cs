using LaesoeLineApi.Features.AgentBooking.Models;
using LaesoeLineApi.Features.AgentBooking.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.AgentBooking
{
    [Route("[controller]")]
    public class AgentBookingController : ControllerBase
    {
        private readonly IWebDriver _webDriver;

        public AgentBookingController(IWebDriver webDriver)
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
    }
}
