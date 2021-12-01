// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region Using directives
using System.Text;
using System.Windows;
using System.Windows.Controls;
#endregion Using directives

namespace TimVer;

public partial class Page2 : Page
{
    public Page2()
    {
        InitializeComponent();

        // workaround for a weird binding error that I couldn't resolve
        tbDiskDrives.Text = InfoVM.DiskDrives;
    }

    #region Copy to clipboard
    private void BtnCopy_Click(object sender, RoutedEventArgs e)
    {
        CopyToClipboard();
    }

    private void CopyToClipboard()
    {
        StringBuilder builder = new();
        _ = builder.AppendLine("COMPUTER INFORMATION");
        _ = builder.Append("Manufacturer    = ").AppendLine(InfoVM.Manufacturer);
        _ = builder.Append("Model           = ").AppendLine(InfoVM.Model);
        _ = builder.Append("Machine Name    = ").AppendLine(InfoVM.MachName);
        _ = builder.Append("Last Rebooted   = ").AppendLine(InfoVM.LastBoot);
        _ = builder.Append("CPU             = ").AppendLine(InfoVM.ProcName);
        _ = builder.Append("Total Cores     = ").AppendLine(InfoVM.ProcCores);
        _ = builder.Append("Architecture    = ").AppendLine(InfoVM.ProcArch);
        _ = builder.Append("Physical Memory = ").AppendLine(InfoVM.TotalMemory);
        if (UserSettings.Setting.ShowDrives)
        {
            _ = builder.Append("Disk Drives     = ").AppendLine(InfoVM.DiskDrives);
        }
        Clipboard.SetText(builder.ToString());
    }
    #endregion Copy to clipboard
}
