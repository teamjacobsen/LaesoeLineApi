using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LaesoeLineApi.Converters
{
    public class VehicleDictionaryKeyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var dictionaryInterface = objectType.GetInterfaces().SingleOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>));

            if (dictionaryInterface == null)
            {
                return false;
            }

            var TKey = dictionaryInterface.GetGenericArguments()[0];

            return TKey == typeof(Vehicle);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();

            var dictionary = (IDictionary)value;

            foreach (var key in dictionary.Keys)
            {
                var vehicle = (Vehicle)key;

                writer.WritePropertyName(vehicle.GetAttribute().Value ?? vehicle.ToString());
                writer.WriteValue(dictionary[key]);
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
