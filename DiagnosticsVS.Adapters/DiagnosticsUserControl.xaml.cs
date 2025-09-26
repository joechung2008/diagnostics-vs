using DiagnosticsVS.Core.Models;
using DiagnosticsVS.Core.Rendering;
using DiagnosticsVS.Core.Services;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DiagnosticsVS.Adapters
{
    public partial class DiagnosticsUserControl : UserControl
    {
        // Keep the currently fetched extensions so callers can look up by extension key.
        private IReadOnlyDictionary<string, ExtensionBase> _currentExtensions;

        public DiagnosticsUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            EnsureCoreWebView2Async().FireAndForget();

            EnvironmentSelector.ItemsSource = Enum.GetValues(typeof(AzureEnvironment))
                .Cast<AzureEnvironment>()
                .Select(env => new { Key = env.ToString(), Value = env.GetDiagnosticsApiUrl() })
                .ToList();
        }

        private void EnvironmentSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EnvironmentSelector.SelectedValue is string url && !string.IsNullOrWhiteSpace(url))
            {
                FetchDiagnosticsAndPopulateExtensionsAsync(url).FireAndForget();
            }
            else
            {
                ExtensionSelector.ItemsSource = null;
                ExtensionSelector.IsEnabled = false;
                _currentExtensions = null;
            }
        }

        private void ExtensionSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedValue = ExtensionSelector.SelectedValue as string;

            ExtensionBase extension = null;
            if (!string.IsNullOrWhiteSpace(selectedValue))
            {
                _currentExtensions.TryGetValue(selectedValue, out extension);
            }

            if (extension != null)
            {
                DisplayExtensionAsync(extension).FireAndForget();
            }
            else
            {
                ExtensionDisplay.Visibility = Visibility.Hidden;
            }
        }

        private async Task DisplayExtensionAsync(ExtensionBase extension)
        {
            try
            {
                ExtensionDisplay.Visibility = Visibility.Visible;
             
                var content = HtmlRenderer.RenderExtensionContent(extension);
                var html = HtmlRenderer.RenderFullHtml(content);
                await ExtensionDisplay.EnsureCoreWebView2Async(null);
                ExtensionDisplay.NavigateToString(html);
            }
            catch (Exception ex)
            {
                ExtensionDisplay.Visibility = Visibility.Hidden;
            
                MessageBox.Show(
                    messageBoxText: $"Failed to render extension: {ex.Message}",
                    caption: "Rendering Error",
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Error
                );
            }
        }

        private async Task EnsureCoreWebView2Async()
        {
            await ExtensionDisplay.EnsureCoreWebView2Async(null);
        }

        private async Task FetchDiagnosticsAndPopulateExtensionsAsync(string url)
        {
            try
            {
                var diagnostics = await DiagnosticsFetcher.FetchDiagnosticsAsync(url);

                _currentExtensions = diagnostics?.Extensions;

                ExtensionSelector.ItemsSource = _currentExtensions?
                    .Select(kvp => new { kvp.Key, Value = kvp.Key })
                    .OrderBy(kvp => kvp.Key)
                    .ToList();
            }
            catch (Exception ex)
            {
                _currentExtensions = null;

                ExtensionSelector.ItemsSource = null;

                MessageBox.Show(
                    messageBoxText: $"Failed to fetch diagnostics: {ex.Message}",
                    caption: "Diagnostics Fetch Error",
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Error
                );
            }
        }
    }
}
