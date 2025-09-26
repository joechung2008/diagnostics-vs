using System;
using System.Collections.Generic;
using System.Linq;
using DiagnosticsVS.Core.Models;

namespace DiagnosticsVS.Core.Extensions
{
    public static class ExtensionCollectionExtensions
    {
        public static bool IsExtensionsEmpty(this IReadOnlyDictionary<string, ExtensionBase> extensions)
        {
            return extensions == null || extensions.Count == 0;
        }

        public static IEnumerable<string> SortExtensionsByName(this IReadOnlyDictionary<string, ExtensionBase> extensions)
        {
            if (extensions == null)
            {
                return Enumerable.Empty<string>();
            }

            // Order keys by the ExtensionInfo.extensionName when available; otherwise use empty string.
            // This mirrors the TypeScript logic where non-ExtensionInfo entries are treated as equal (return 0).
            return extensions.Keys.OrderBy(k =>
            {
                if (extensions.TryGetValue(k, out var ext) && ext is ExtensionInfo info)
                {
                    return info.ExtensionName ?? string.Empty;
                }

                return string.Empty;
            }, StringComparer.CurrentCulture);
        }

        public static IReadOnlyList<string> PrepareExtensionChoices(this IReadOnlyDictionary<string, ExtensionBase> extensions)
        {
            if (extensions.IsExtensionsEmpty())
            {
                return Array.Empty<string>();
            }

            return SortExtensionsByName(extensions).ToList();
        }
    }
}
