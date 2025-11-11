using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Utilities
{
    public static class JsonService
    {
        private static readonly JsonSerializerSettings Settings;

        static JsonService()
        {
            Settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include
            };

            Settings.Converters.Add(new Vector3Converter());
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        public static object Deserialize(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type, Settings);
        }

        private class Vector3Converter : JsonConverter<Vector3>
        {
            public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x"); writer.WriteValue(value.x);
                writer.WritePropertyName("y"); writer.WriteValue(value.y);
                writer.WritePropertyName("z"); writer.WriteValue(value.z);
                writer.WriteEndObject();
            }

            public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                float x = 0, y = 0, z = 0;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string prop = (string)reader.Value;
                        reader.Read();

                        switch (prop)
                        {
                            case "x": x = Convert.ToSingle(reader.Value); break;
                            case "y": y = Convert.ToSingle(reader.Value); break;
                            case "z": z = Convert.ToSingle(reader.Value); break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Vector3(x, y, z);
            }
        }
    }
}
