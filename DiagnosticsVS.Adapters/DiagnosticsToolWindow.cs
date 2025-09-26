using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace DiagnosticsVS.Adapters
{
    [Guid("C3B5298E-7D4A-4A2A-9F3E-1A2B3C4D5E6F")]
    public class DiagnosticsToolWindow : ToolWindowPane
    {
        public DiagnosticsToolWindow() : base(null)
        {
            this.Caption = "Azure Portal Extensions";

            // Use the XAML user control as the tool window content.
            this.Content = new DiagnosticsUserControl();
        }
    }
}
