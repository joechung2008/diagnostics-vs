using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiagnosticsVS.Core.Models
{
    public class Diagnostics
    {
        [JsonProperty("extensions")]
        public Dictionary<string, ExtensionBase> Extensions { get; set; } = new Dictionary<string, ExtensionBase>();
    }
}
