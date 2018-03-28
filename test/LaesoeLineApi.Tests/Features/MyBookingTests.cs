using System.Threading.Tasks;
using Xunit;

namespace LaesoeLineApi.Tests.Features
{
    public class MyBookingTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public MyBookingTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        //[Fact]
        [Fact(Skip = "No reservation to cancel")]
        public async Task Cancel()
        {
            // Given

            // When
            await _fixture.Api.MyBooking.CancelAsync("311349", "KCAD");

            // Then
        }
    }
}
