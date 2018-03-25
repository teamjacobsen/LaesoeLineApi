using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaesoeLineApi.Converters
{
    public class VehicleValueConverter : JsonConverter
    {
        private static readonly Dictionary<string, Vehicle> _valueToVehicleMap = ((Vehicle[])Enum.GetValues(typeof(Vehicle))).ToDictionary(x => x.GetAttribute().Value, x => x, StringComparer.OrdinalIgnoreCase);

        public override bool CanConvert(Type objectType) => objectType == typeof(Vehicle);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vehicle = (Vehicle)value;
            var attribute = vehicle.GetAttribute();

            writer.WriteValue(vehicle.GetAttribute().Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return Vehicle.None;
            }
            else if (reader.TokenType != JsonToken.String)
            {
                throw new NotSupportedException();
            }

            if (!_valueToVehicleMap.TryGetValue((string)reader.Value, out var vehicle))
            {
                throw new JsonReaderException("Invalid vehicle");
            }

            return vehicle;
        }
    }
}
