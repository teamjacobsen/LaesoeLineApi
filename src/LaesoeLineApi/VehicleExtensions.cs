using System;
using System.Linq;

namespace LaesoeLineApi
{
    public static class VehicleExtensions
    {
        public static string ToCamelCase(this Vehicle vehicle)
        {
            var chars = vehicle.ToString().ToCharArray();

            chars[0] = char.ToLowerInvariant(chars[0]);

            return new string(chars);
        }

        public static VehicleValueAttribute GetAttribute(this Vehicle vehicle)
        {
            var attribute = vehicle.GetType().GetMember(vehicle.ToString())[0].GetCustomAttributes(typeof(VehicleValueAttribute), false).Cast<VehicleValueAttribute>().SingleOrDefault();

            if (attribute == null)
            {
                throw new NotImplementedException($"The VehicleValue attribute was not found for the vehicle '{vehicle}'");
            }

            return attribute;
        }
    }
}
