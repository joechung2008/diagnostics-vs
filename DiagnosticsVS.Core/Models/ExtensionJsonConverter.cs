using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiagnosticsVS.Core.Models
{
    // Converter that inspects the JSON shape to determine concrete subtype.
    // If JSON contains "extensionName" it is treated as ExtensionInfo.
    // If JSON contains "lastError" it is treated as ExtensionError.
    public class ExtensionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(ExtensionBase).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token.Type == JTokenType.Null) return null;

            if (token.Type != JTokenType.Object)
            {
                throw new JsonSerializationException($"Unexpected JSON token when deserializing ExtensionBase: {token.Type}");
            }

            var obj = (JObject)token;

            // Case-sensitive checks: only exact JSON property names are considered.
            bool hasExtensionName = obj.ContainsKey("extensionName");
            bool hasLastError = obj.ContainsKey("lastError");

            ExtensionBase result;

            if (hasExtensionName)
            {
                result = new ExtensionInfo();
            }
            else if (hasLastError)
            {
                result = new ExtensionError();
            }
            else
            {
                // Could not determine concrete type; prefer to throw so caller can handle unexpected shapes.
                throw new JsonSerializationException("Cannot determine Extension type from JSON: missing 'extensionName' or 'lastError' properties.");
            }

            serializer.Populate(obj.CreateReader(), result);
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Serialize the concrete object without adding any discriminator.
            var jo = JObject.FromObject(value, serializer);
            jo.WriteTo(writer);
        }
    }
}
