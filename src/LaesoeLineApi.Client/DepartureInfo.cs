using System;
using System.Collections.Generic;

namespace LaesoeLineApi
{
    public class DepartureInfo
    {
        public DateTime Departure { get; set; }
        public Dictionary<string, bool> Availability { get; set; }
    }
}
