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

            string bookingNumber = null;
            var status = await Task.Factory.StartNew(() =>
            {
                var loginPage = _webDriver.GoTo<LoginPage>();
                loginPage.Login(credentials.Username, credentials.Password);

                var bookPage = _webDriver.GoTo<BookPage>();
                var result = bookPage.BookItRoundTrip(command.Outbound, command.Return);
                bookingNumber = bookPage.BookingNumber;

                return result;
            });

            if (status == BookStatus.Success)
            {
                var result = new BookResult()
                {
                    BookingNumber = bookingNumber
                };

                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, status);
            }
        }
    }
}
