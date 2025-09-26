using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DiagnosticsVS.Core.Models
{
    public class ExtensionInfo : ExtensionBase
    {
        [JsonProperty("extensionName")]
        public string ExtensionName { get; set; }

        [JsonProperty("manageSdpEnabled")]
        public bool ManageSdpEnabled { get; set; }

        [JsonProperty("config")]
        public Dictionary<string, string> Config { get; set; }

        [JsonProperty("stageDefinition")]
        public Dictionary<string, string[]> StageDefinition { get; set; }

        public ExtensionInfo() { }
    }
}
