using Newtonsoft.Json;

namespace DiagnosticsVS.Core.Models
{
    public class ExtensionError : ExtensionBase
    {
        [JsonProperty("lastError")]
        public ExtensionLastError LastError { get; set; }
    }

    public class ExtensionLastError
    {
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }
    }
}
