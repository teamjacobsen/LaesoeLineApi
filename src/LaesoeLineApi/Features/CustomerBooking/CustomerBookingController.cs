using LaesoeLineApi.Features.CustomerBooking.Models;
using LaesoeLineApi.Features.CustomerBooking.Pages;
using LaesoeLineApi.Selenium;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.CustomerBooking
{
    [Authorize]
    [Route("[controller]")]
    public class CustomerBookingController : ControllerBase
    {
        private readonly IWebDriverFactory _webDriverFactory;

        public CustomerBookingController(IWebDriverFactory webDriverFactory)
        {
            _webDriverFactory = webDriverFactory;
        }

        [HttpPost("Book/SeasonPass/OneWay")]
        [ProducesResponseType(typeof(BookSuccessResult), 200)]
        [ProducesResponseType(typeof(BookErrorResult), 422)]
        public async Task<IActionResult> BookSeasonPassOneWay([Required] [FromBody] BookOneWay command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var driver = await _webDriverFactory.CreateAsync())
            {
                var customerProfilePage = await driver.GoToAsync<CustomerProfilePage>();
                await customerProfilePage.LoginAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, User.FindFirst(ClaimTypes.Authentication).Value);

                var bookSeasonPassPage = await driver.GoToAsync<BookSeasonPassPage>();
                var status = await bookSeasonPassPage.BookOneWayAsync(command.Journey, command.Local);
                var bookResult = new BookSuccessResult
                {
                    BookingNumber = bookSeasonPassPage.BookingNumber,
                    BookingPassword = bookSeasonPassPage.BookingPassword
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

        [HttpPost("Book/SeasonPass/RoundTrip")]
        [ProducesResponseType(typeof(BookSuccessResult), 200)]
        [ProducesResponseType(typeof(BookErrorResult), 422)]
        public async Task<IActionResult> BookSeasonPassRoundTrip([Required] [FromBody] BookRoundTrip command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var driver = await _webDriverFactory.CreateAsync())
            {
                var customerProfilePage = await driver.GoToAsync<CustomerProfilePage>();
                await customerProfilePage.LoginAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, User.FindFirst(ClaimTypes.Authentication).Value);

                var bookSeasonPassPage = await driver.GoToAsync<BookSeasonPassPage>();
                var status = await bookSeasonPassPage.BookRoundTripAsync(command.Outbound, command.Return, command.Local);
                var bookResult = new BookSuccessResult
                {
                    BookingNumber = bookSeasonPassPage.BookingNumber,
                    BookingPassword = bookSeasonPassPage.BookingPassword
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

        [HttpDelete("Bookings/{bookingNumber}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Cancel(string bookingNumber)
        {
            using (var driver = await _webDriverFactory.CreateAsync())
            {
                var customerProfilePage = await driver.GoToAsync<CustomerProfilePage>();

                await customerProfilePage.LoginAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, User.FindFirst(ClaimTypes.Authentication).Value);

                var found = await customerProfilePage.CancelAsync(bookingNumber);

                if (!found)
                {
                    return NotFound();
                }

                return NoContent();
            }
        }
    }
}
