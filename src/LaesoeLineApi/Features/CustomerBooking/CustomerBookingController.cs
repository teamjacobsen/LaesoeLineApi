using LaesoeLineApi.Features.CustomerBooking.Pages;
using LaesoeLineApi.Selenium;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.CustomerBooking
{
    [Authorize]
    [Route("[controller]")]
    public class CustomerBookingController : ControllerBase
    {
        private readonly IBrowserSessionFactory _browserSessionFactory;

        public CustomerBookingController(IBrowserSessionFactory browserSessionFactory)
        {
            _browserSessionFactory = browserSessionFactory;
        }

        [HttpPost("Book/OneDay")]
        [ProducesResponseType(typeof(BookSuccessResult), 200)]
        [ProducesResponseType(typeof(BookErrorResult), 422)]
        public async Task<IActionResult> BookOneDay([Required] [FromBody] BookOneDay command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var session = _browserSessionFactory.CreateSession())
            {
                var customerProfilePage = await session.GoToAsync<BookOneDayCustomerProfilePage>();
                await customerProfilePage.LoginAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, User.FindFirst(ClaimTypes.Authentication).Value);

                var bookingDetailsPage = await session.GoToAsync<BookOneDayDetailsPage>();
                await bookingDetailsPage.EnterDetailsAsync(command.Outbound, command.Return);

                var departureSelectPage = await session.GoToAsync<BookOneDayDepartureSelectPage>();
                await departureSelectPage.SelectDeparturesAsync(command.Outbound.Departure.Value, command.Return.Departure.Value);

                var contactInformationPage = await session.GoToAsync<BookOneDayContactInformationPage>();
                await contactInformationPage.EnterInformationAndCheckTermsAsync(command.Customer.FirstName, command.Customer.LastName, command.Customer.PhoneNumber, command.Customer.Email);

                var bookingConfiguration = await session.GoToAsync<BookOneDayConfirmationPage>();
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

        [HttpPost("Book/OneDayCar")]
        [ProducesResponseType(typeof(BookSuccessResult), 200)]
        [ProducesResponseType(typeof(BookErrorResult), 422)]
        public async Task<IActionResult> BookOneDayCar([Required] [FromBody] BookOneDay command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var session = _browserSessionFactory.CreateSession())
            {
                var customerProfilePage = await session.GoToAsync<BookOneDayCarCustomerProfilePage>();
                await customerProfilePage.LoginAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, User.FindFirst(ClaimTypes.Authentication).Value);

                var bookingDetailsPage = await session.GoToAsync<BookOneDayCarDetailsPage>();
                await bookingDetailsPage.EnterDetailsAsync(command.Outbound, command.Return);

                var departureSelectPage = await session.GoToAsync<BookOneDayCarDepartureSelectPage>();
                await departureSelectPage.SelectDeparturesAsync(command.Outbound.Departure.Value, command.Return.Departure.Value);

                var contactInformationPage = await session.GoToAsync<BookOneDayCarContactInformationPage>();
                await contactInformationPage.EnterInformationAndCheckTermsAsync(command.Customer.FirstName, command.Customer.LastName, command.Customer.PhoneNumber, command.Customer.Email);

                var bookingConfiguration = await session.GoToAsync<BookOneDayCarConfirmationPage>();
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

        [HttpPost("Book/OneDayFood")]
        [ProducesResponseType(typeof(BookSuccessResult), 200)]
        [ProducesResponseType(typeof(BookErrorResult), 422)]
        public async Task<IActionResult> BookOneDayFood([Required] [FromBody] BookOneDay command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var session = _browserSessionFactory.CreateSession())
            {
                var customerProfilePage = await session.GoToAsync<BookOneDayFoodCustomerProfilePage>();
                await customerProfilePage.LoginAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, User.FindFirst(ClaimTypes.Authentication).Value);

                var bookingDetailsPage = await session.GoToAsync<BookOneDayFoodDetailsPage>();
                await bookingDetailsPage.EnterDetailsAsync(command.Outbound, command.Return);

                var departureSelectPage = await session.GoToAsync<BookOneDayFoodDepartureSelectPage>();
                await departureSelectPage.SelectDeparturesAsync(command.Outbound.Departure.Value, command.Return.Departure.Value);

                var contactInformationPage = await session.GoToAsync<BookOneDayFoodContactInformationPage>();
                await contactInformationPage.EnterInformationAndCheckTermsAsync(command.Customer.FirstName, command.Customer.LastName, command.Customer.PhoneNumber, command.Customer.Email);

                var bookingConfiguration = await session.GoToAsync<BookOneDayFoodConfirmationPage>();
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

        [HttpPost("Book/SeasonPass/OneWay")]
        [ProducesResponseType(typeof(BookSuccessResult), 200)]
        [ProducesResponseType(typeof(BookErrorResult), 422)]
        public async Task<IActionResult> BookSeasonPassOneWay([Required] [FromBody] BookOneWay command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var session = _browserSessionFactory.CreateSession())
            {
                var customerProfilePage = await session.GoToAsync<BookSeasonPassCustomerProfilePage>();
                await customerProfilePage.LoginAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, User.FindFirst(ClaimTypes.Authentication).Value);

                var bookingDetailsPage = await session.GoToAsync<BookSeasonPassDetailsPage>();
                await bookingDetailsPage.EnterDetailsAsync(command.Journey, command.Local);

                var departureSelectPage = await session.GoToAsync<BookSeasonPassDepartureSelectPage>();
                await departureSelectPage.SelectDepartureAsync(command.Journey.Departure.Value);

                var contactInformationPage = await session.GoToAsync<BookSeasonPassContactInformationPage>();
                await contactInformationPage.CheckTermsAsync();

                var bookingConfiguration = await session.GoToAsync<BookSeasonPassConfirmationPage>();
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

        [HttpPost("Book/SeasonPass/RoundTrip")]
        [ProducesResponseType(typeof(BookSuccessResult), 200)]
        [ProducesResponseType(typeof(BookErrorResult), 422)]
        public async Task<IActionResult> BookSeasonPassRoundTrip([Required] [FromBody] BookRoundTrip command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var session = _browserSessionFactory.CreateSession())
            {
                var customerProfilePage = await session.GoToAsync<BookSeasonPassCustomerProfilePage>();
                await customerProfilePage.LoginAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, User.FindFirst(ClaimTypes.Authentication).Value);

                var bookingDetailsPage = await session.GoToAsync<BookSeasonPassDetailsPage>();
                await bookingDetailsPage.EnterDetailsAsync(command.Outbound, command.Return, command.Local);

                var departureSelectPage = await session.GoToAsync<BookSeasonPassDepartureSelectPage>();
                await departureSelectPage.SelectDeparturesAsync(command.Outbound.Departure.Value, command.Return.Departure.Value);

                var contactInformationPage = await session.GoToAsync<BookSeasonPassContactInformationPage>();
                await contactInformationPage.CheckTermsAsync();

                var bookingConfiguration = await session.GoToAsync<BookSeasonPassConfirmationPage>();
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

        [HttpDelete("Bookings/{bookingNumber}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Cancel(string bookingNumber)
        {
            using (var session = _browserSessionFactory.CreateSession())
            {
                var customerProfilePage = await session.GoToAsync<CustomerProfilePage>();
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
