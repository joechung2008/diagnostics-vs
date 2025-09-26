using System.Collections.Generic;

namespace DiagnosticsVS.Core.Models
{
    public enum AzureEnvironment
    {
        Public,
        Fairfax,
        Mooncake
    }

    public static class AzureEnvironmentExtensions
    {
        private static readonly IReadOnlyDictionary<AzureEnvironment, string> _diagnosticsUrls =
            new Dictionary<AzureEnvironment, string>
            {
                [AzureEnvironment.Public] = "https://hosting.portal.azure.net/api/diagnostics",
                [AzureEnvironment.Fairfax] = "https://hosting.azureportal.usgovcloudapi.net/api/diagnostics",
                [AzureEnvironment.Mooncake] = "https://hosting.azureportal.chinacloudapi.cn/api/diagnostics",
            };

        public static string GetDiagnosticsApiUrl(this AzureEnvironment environment)
        {
            return _diagnosticsUrls[environment];
        }
    }
}
