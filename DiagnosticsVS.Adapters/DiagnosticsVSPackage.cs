using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace DiagnosticsVS.Adapters
{
    // Add ProvideToolWindow attribute to register the tool window.
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(DiagnosticsVSPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(DiagnosticsToolWindow))]
    public sealed class DiagnosticsVSPackage : AsyncPackage
    {
        public const string PackageGuidString = "119A918A-FFDB-48CE-BBC7-A232F37F3BC2";

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await DiagnosticsCommand.InitializeAsync(this);
        }
    }
}