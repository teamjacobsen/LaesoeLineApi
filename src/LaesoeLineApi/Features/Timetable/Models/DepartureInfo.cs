using System;
using System.Collections.Generic;

namespace LaesoeLineApi.Features.Timetable.Models
{
    public class DepartureInfo
    {
        public DateTime Departure { get; set; }
        public IDictionary<Vehicle, bool> Availability { get; set; }
    }
}
