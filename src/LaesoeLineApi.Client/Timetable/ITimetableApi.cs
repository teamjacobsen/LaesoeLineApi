using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaesoeLineApi
{
    public interface ITimetableApi
    {
        [Get("/Timetable/Crossings/{crossing}/Departures")]
        Task<List<Timetable.DepartureInfo>> GetDeparturesAsync(Crossing crossing, DateTime? date = null, int? days = null);
    }
}
