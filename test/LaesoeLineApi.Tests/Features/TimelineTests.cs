using System;
using System.Threading.Tasks;
using Xunit;

namespace LaesoeLineApi.Tests.Features
{
    public class TimelineTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public TimelineTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetDepartures()
        {
            // Given
            var date = DateTime.UtcNow.AddDays(20).Date;

            // When
            var departures = await _fixture.Api.Timetable.GetDeparturesAsync(Crossing.LaesoeFrederikshavn, date, 4);

            // Then
            Assert.Contains(departures, x => x.Departure == new DateTime(date.Year, date.Month, date.Day, 6, 0, 0));
            Assert.Contains(VehicleType.Car, departures[0].Availability.Keys);
        }
    }
}
