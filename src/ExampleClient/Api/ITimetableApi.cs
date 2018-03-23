using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleClient.Api
{
    public interface ITimetableApi
    {
        [Get("/Timetable/Crossings/{crossing}/Departures")]
        Task<List<DepartureInfo>> GetDepartures(Crossing crossing, DateTime? date = null, int? days = null);
    }
}
