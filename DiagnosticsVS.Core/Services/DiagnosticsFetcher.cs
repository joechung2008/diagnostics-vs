using DiagnosticsVS.Core.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiagnosticsVS.Core.Services
{
    public static class DiagnosticsFetcher
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Fetch diagnostics from the Azure environment's diagnostics endpoint.
        /// Throws an Exception when the HTTP request fails or the response cannot be parsed.
        /// </summary>
        public static Task<Diagnostics> FetchDiagnosticsAsync(AzureEnvironment environment)
        {
            var url = environment.GetDiagnosticsApiUrl();
            return FetchDiagnosticsAsync(url);
        }

        /// <summary>
        /// Fetch diagnostics from the provided URL.
        /// Throws an Exception when the HTTP request fails or the response cannot be parsed.
        /// </summary>
        public static async Task<Diagnostics> FetchDiagnosticsAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL must be provided", nameof(url));
            }

            try
            {
                using var response = await _httpClient.GetAsync(url).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new HttpRequestException($"HTTP {(int)response.StatusCode} ({response.ReasonPhrase}) while fetching diagnostics from {url}. Response body: {responseBody}");
                }

                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                try
                {
                    var diagnostics = JsonConvert.DeserializeObject<Diagnostics>(json);
                    if (diagnostics == null)
                    {
                        throw new JsonException("Deserialized diagnostics is null.");
                    }

                    return diagnostics;
                }
                catch (JsonException jex)
                {
                    throw new Exception($"Failed to deserialize diagnostics JSON from {url}: {jex.Message}", jex);
                }
            }
            catch (HttpRequestException hrex)
            {
                throw new Exception($"HTTP request failed for {url}: {hrex.Message}", hrex);
            }
            catch (TaskCanceledException tce)
            {
                throw new Exception($"HTTP request timed out for {url}: {tce.Message}", tce);
            }
        }
    }
}
