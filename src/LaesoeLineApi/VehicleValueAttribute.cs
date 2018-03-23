using System;

namespace LaesoeLineApi
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VehicleValueAttribute : Attribute
    {
        public string Value { get; set; }
        public string OptionValue { get; private set; }
        public string SeasonPassOptionValue { get; private set; }
        public bool IncludeInAvailability { get; private set; }

        public VehicleValueAttribute(string value, string optionValue, string seasonPassOptionValue = null, bool includeInAvailability = false)
        {
            Value = value;
            OptionValue = optionValue;
            SeasonPassOptionValue = seasonPassOptionValue;
            IncludeInAvailability = includeInAvailability;
        }
    }
}
