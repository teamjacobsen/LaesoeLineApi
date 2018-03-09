using LaesoeLineApi.Features.Customer.Models;
using LaesoeLineApi.Features.Customer.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.Customer
{
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IWebDriver _webDriver;

        public CustomerController(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        [HttpPost("Book/SeasonPass/OneWay")]
        public async Task<IActionResult> BookSeasonPassOneWay([Required] [FromBody] BookOneWay command)
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
                var customerProfilePage = _webDriver.GoTo<CustomerProfilePage>();
                customerProfilePage.Login(credentials.Username, credentials.Password);

                var bookSeasonPassPage = _webDriver.GoTo<BookSeasonPassPage>();
                var result = bookSeasonPassPage.BookOneWay(command.Journey, command.Local);
                bookingNumber = bookSeasonPassPage.BookingNumber;

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

        [HttpPost("Book/SeasonPass/RoundTrip")]
        public async Task<IActionResult> BookSeasonPassRoundTrip([Required] [FromBody] BookRoundTrip command)
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
                var customerProfilePage = _webDriver.GoTo<CustomerProfilePage>();
                customerProfilePage.Login(credentials.Username, credentials.Password);

                var bookSeasonPassPage = _webDriver.GoTo<BookSeasonPassPage>();
                var result = bookSeasonPassPage.BookRoundTrip(command.Outbound, command.Return, command.Local);

                bookingNumber = bookSeasonPassPage.BookingNumber;

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

        [HttpDelete("Bookings/{bookingNumber}")]
        public async Task<IActionResult> Cancel(string bookingNumber)
        {
            var credentials = Request.GetCredentials();

            if (credentials == null)
            {
                return Unauthorized();
            }

            var found = await Task.Factory.StartNew(() =>
            {
                var customerProfilePage = _webDriver.GoTo<CustomerProfilePage>();

                customerProfilePage.Login(credentials.Username, credentials.Password);

                return customerProfilePage.Cancel(bookingNumber);
            });

            if (!found)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
