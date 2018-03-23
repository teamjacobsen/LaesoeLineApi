using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleClient.Api
{
    public class DepartureInfo
    {
        public DateTime Departure { get; set; }
        public Dictionary<string, bool> Availability { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("{0} {1}: ", Departure.ToShortDateString(), Departure.ToShortTimeString());

            foreach (var availability in Availability)
            {
                if (availability.Value)
                {
                    builder.AppendFormat("\t{0} (available)", availability.Key);
                }
                else
                {
                    builder.AppendFormat("\t{0} (sold out)", availability.Key);
                }
            }

            return builder.ToString();
        }
    }
}
