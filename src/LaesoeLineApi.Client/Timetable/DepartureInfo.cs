using System;
using System.Collections.Generic;

namespace LaesoeLineApi.Timetable
{
    public class DepartureInfo
    {
        public DateTime Departure { get; set; }
        public Dictionary<Vehicle, VehicleAvailabilityInfo> Availability { get; set; }
    }
}
