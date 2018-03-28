using System;
using System.Collections.Generic;

namespace LaesoeLineApi.Features.Timetable
{
    public class DepartureInfo
    {
        public DateTime Departure { get; set; }
        public IDictionary<Vehicle, VehicleAvailabilityInfo> Availability { get; set; }
    }
}
