using DiagnosticsVS.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace DiagnosticsVS.Core.Rendering
{
    public static partial class HtmlRenderer
    {
        public static string RenderConfigTable(Dictionary<string, string> config, string title)
        {
            if (config == null || config.Count == 0)
            {
                return "";
            }

            var sb = new StringBuilder();
            sb.AppendLine($"<h2>{Escape(title)}</h2>");
            sb.AppendLine("<table>");
            sb.AppendLine("  <thead>");
            sb.AppendLine("    <tr>");
            sb.AppendLine("      <th>Key</th>");
            sb.AppendLine("      <th>Value</th>");
            sb.AppendLine("    </tr>");
            sb.AppendLine("  </thead>");
            sb.AppendLine("  <tbody>");

            foreach (var kv in config)
            {
                sb.AppendLine("    <tr>");
                sb.AppendLine($"      <td>{Escape(kv.Key)}</td>");
                sb.AppendLine($"      <td>{Escape(kv.Value)}</td>");
                sb.AppendLine("    </tr>");
            }

            sb.AppendLine("  </tbody>");
            sb.AppendLine("</table>");

            return sb.ToString();
        }

        public static string RenderStageDefinitionTable(Dictionary<string, string[]> stageDefinition, string title)
        {
            if (stageDefinition == null || stageDefinition.Count == 0)
            {
                return "";
            }

            var sb = new StringBuilder();
            sb.AppendLine($"<h2>{Escape(title)}</h2>");
            sb.AppendLine("<table>");
            sb.AppendLine("  <thead>");
            sb.AppendLine("    <tr>");
            sb.AppendLine("      <th>Key</th>");
            sb.AppendLine("      <th>Values</th>");
            sb.AppendLine("    </tr>");
            sb.AppendLine("  </thead>");
            sb.AppendLine("  <tbody>");

            foreach (var kv in stageDefinition)
            {
                var joined = kv.Value != null ? string.Join(", ", kv.Value.Select(Escape)) : string.Empty;
                sb.AppendLine("    <tr>");
                sb.AppendLine($"      <td>{Escape(kv.Key)}</td>");
                sb.AppendLine($"      <td>{joined}</td>");
                sb.AppendLine("    </tr>");
            }

            sb.AppendLine("  </tbody>");
            sb.AppendLine("</table>");

            return sb.ToString();
        }

        public static string RenderExtensionContent(ExtensionBase extension)
        {
            if (extension is ExtensionInfo info)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"<h1>{Escape(info.ExtensionName)}</h1>");
                sb.AppendLine($"<p><strong>Manage SDP Enabled:</strong> {(info.ManageSdpEnabled ? "Yes" : "No")}</p>");
                sb.AppendLine(RenderConfigTable(info.Config, "Configuration"));
                sb.AppendLine(RenderStageDefinitionTable(info.StageDefinition, "Stage Definitions"));

                return sb.ToString();
            }

            if (extension is ExtensionError err)
            {
                var last = err.LastError ?? new ExtensionLastError { ErrorMessage = "Unknown", Time = string.Empty };
                var errorHtml = new StringBuilder();
                errorHtml.AppendLine("<h1>Extension Error</h1>");
                errorHtml.AppendLine($"<p><strong>Error:</strong> {Escape(last.ErrorMessage)}</p>");
                errorHtml.AppendLine($"<p><strong>Time:</strong> {Escape(last.Time)}</p>");
                return errorHtml.ToString();
            }

            return "";
        }

        public static string RenderFullHtml(string content)
        {
            return RenderFullHtml(content, null);
        }

        public static string RenderFullHtml(string content, Dictionary<string, string> themeColors)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("  <meta charset=\"UTF-8\">");
            sb.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");

            // Add theme variables if provided (before Tailwind CSS)
            if (themeColors != null && themeColors.Count > 0)
            {
                sb.AppendLine("  <style>");
                sb.AppendLine("    :root {");
                foreach (var kv in themeColors)
                {
                    sb.AppendLine($"      --{kv.Key}: {kv.Value};");
                }
                sb.AppendLine("    }");
                sb.AppendLine("  </style>");
            }

            sb.AppendLine("  <style text=\"text/css\">");
            sb.AppendLine(GetTailwindCss());
            sb.AppendLine(" </style>");
            sb.AppendLine("  <title>Extension Details</title>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("  <div class=\"container mx-auto p-4 prose\">");
            sb.AppendLine(content);
            sb.AppendLine("  </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }

        private static string Escape(string input) => WebUtility.HtmlEncode(input ?? string.Empty);
    }
}
