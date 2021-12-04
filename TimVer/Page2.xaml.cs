// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

/// <summary>
/// Displays Computer information
/// </summary>
public partial class Page2 : Page
{
    public Page2()
    {
        InitializeComponent();

        // workaround for a weird binding error that I couldn't resolve
        tbDiskDrives.Text = CombinedInfo.DiskDrives;
    }

    #region Copy to clipboard
    private void BtnCopy_Click(object sender, RoutedEventArgs e)
    {
        Mouse.OverrideCursor = Cursors.Wait;
        CopyToClipboard();
        Mouse.OverrideCursor = null;
    }

    private static void CopyToClipboard()
    {
        StringBuilder builder = new();
        _ = builder.AppendLine("COMPUTER INFORMATION");
        _ = builder.Append("Manufacturer    = ").AppendLine(CombinedInfo.Manufacturer);
        _ = builder.Append("Model           = ").AppendLine(CombinedInfo.Model);
        _ = builder.Append("Machine Name    = ").AppendLine(CombinedInfo.MachName);
        _ = builder.Append("Last Rebooted   = ").AppendLine(CombinedInfo.LastBoot);
        _ = builder.Append("CPU             = ").AppendLine(CombinedInfo.ProcName);
        _ = builder.Append("Total Cores     = ").AppendLine(CombinedInfo.ProcCores);
        _ = builder.Append("Architecture    = ").AppendLine(CombinedInfo.ProcArch);
        _ = builder.Append("Physical Memory = ").AppendLine(CombinedInfo.TotalMemory);
        if (UserSettings.Setting.ShowDrives)
        {
            _ = builder.Append("Disk Drives     = ").AppendLine(CombinedInfo.DiskDrives);
        }
        Clipboard.SetText(builder.ToString());
    }
    #endregion Copy to clipboard
}
