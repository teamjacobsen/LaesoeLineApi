using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LaesoeLineApi.Converters
{
    public class CamelCaseEnumDictionaryKeyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (!objectType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
            {
                return false;
            }

            var TKey = objectType.GetGenericArguments()[0];

            return TKey.IsEnum;
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
                writer.WritePropertyName(ToCamelCase(key.ToString()));
                writer.WriteValue(dictionary[key]);
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private static string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
            {
                return s;
            }

            var chars = s.ToCharArray();

            chars[0] = char.ToLowerInvariant(chars[0]);

            return new string(chars);
        }
    }
}
