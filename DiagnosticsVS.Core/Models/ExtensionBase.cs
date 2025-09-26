using Newtonsoft.Json;

namespace DiagnosticsVS.Core.Models
{
    [JsonConverter(typeof(ExtensionJsonConverter))]
    public abstract class ExtensionBase { }
}
