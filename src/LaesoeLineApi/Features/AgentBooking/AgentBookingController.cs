using LaesoeLineApi.Features.AgentBooking.Pages;
using LaesoeLineApi.Selenium;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.AgentBooking
{
    [Authorize]
    [Route("[controller]")]
    public class AgentBookingController : ControllerBase
    {
        private readonly IBrowserSessionFactory _browserSessionFactory;

        public AgentBookingController(IBrowserSessionFactory browserSessionFactory)
        {
            _browserSessionFactory = browserSessionFactory;
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

            using (var session = _browserSessionFactory.CreateSession())
            {
                var loginPage = await session.GoToAsync<LoginPage>();
                await loginPage.LoginAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, User.FindFirst(ClaimTypes.Authentication).Value);

                var bookingDetailsPage = await session.GoToAsync<BookDetailsPage>();
                await bookingDetailsPage.EnterDetailsAsync(command.Outbound, command.Return);

                var departureSelectPage = await session.GoToAsync<BookDepartureSelectPage>();
                await departureSelectPage.SelectDeparturesAsync(command.Outbound.Departure.Value, command.Return.Departure.Value);

                var contactInformationPage = await session.GoToAsync<BookContantInformationPage>();
                await contactInformationPage.EnterInformationAndCheckTermsAsync(command.Customer.Name, command.Customer.PhoneNumber, command.Customer.Email);

                var bookingConfiguration = await session.GoToAsync<BookConfirmationPage>();
                var details = await bookingConfiguration.GetBookingDetailsAsync();

                var result = new BookSuccessResult
                {
                    BookingNumber = details.BookingNumber,
                    BookingPassword = details.BookingPassword,
                    TotalPrice = details.TotalPrice
                };

                return Ok(result);
            }
        }
    }
}
