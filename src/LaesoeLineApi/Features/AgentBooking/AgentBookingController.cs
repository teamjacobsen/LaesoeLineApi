using LaesoeLineApi.Features.AgentBooking.Models;
using LaesoeLineApi.Features.AgentBooking.Pages;
using LaesoeLineApi.Selenium;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.AgentBooking
{
    [Authorize]
    [Route("[controller]")]
    public class AgentBookingController : ControllerBase
    {
        private readonly IWebDriverFactory _webDriverFactory;

        public AgentBookingController(IWebDriverFactory webDriverFactory)
        {
            _webDriverFactory = webDriverFactory;
        }

        [HttpPost("Book/It/RoundTrip")]
        [ProducesResponseType(typeof(BookSuccessResult), 200)]
        [ProducesResponseType(typeof(BookErrorResult), 422)]
        public async Task<IActionResult> BookItRoundTrip([Required] [FromBody] BookRoundTrip command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var driver = await _webDriverFactory.CreateAsync())
            {
                var loginPage = await driver.GoToAsync<LoginPage>();
                await loginPage.LoginAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, User.FindFirst(ClaimTypes.Authentication).Value);

                var bookPage = await driver.GoToAsync<BookPage>();
                var status = await bookPage.BookItRoundTripAsync(command.Customer, command.Outbound, command.Return);
                var bookResult = new BookSuccessResult
                {
                    BookingNumber = bookPage.BookingNumber,
                    BookingPassword = bookPage.BookingPassword,
                    TotalPrice = bookPage.Price
                };

                if (status == BookStatus.Success)
                {
                    return Ok(bookResult);
                }
                else
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, new BookErrorResult(status));
                }
            }
        }
    }
}
