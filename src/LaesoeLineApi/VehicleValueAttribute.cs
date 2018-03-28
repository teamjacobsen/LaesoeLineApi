using System;

namespace LaesoeLineApi
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VehicleValueAttribute : Attribute
    {
        public string OptionValue { get; private set; }
        public string SeasonPassOptionValue { get; private set; }
        public bool IncludeInAvailability { get; private set; }

        public VehicleValueAttribute(string optionValue, string seasonPassOptionValue = null, bool includeInAvailability = false)
        {
            OptionValue = optionValue;
            SeasonPassOptionValue = seasonPassOptionValue;
            IncludeInAvailability = includeInAvailability;
        }
    }
}
