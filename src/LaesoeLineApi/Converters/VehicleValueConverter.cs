using Newtonsoft.Json;
using System;

namespace LaesoeLineApi.Converters
{
    public class VehicleValueConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Vehicle);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vehicle = (Vehicle)value;

            writer.WriteValue(vehicle.ToCamelCase());
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

            return Enum.Parse(typeof(Vehicle), (string)reader.Value, true);
        }
    }
}
