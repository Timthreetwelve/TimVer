// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

/// <summary>
/// Displays Computer information
/// </summary>
public partial class Page2 : UserControl
{
    public Page2()
    {
        InitializeComponent();
    }

    #region Copy to clipboard
    private void BtnCopy_Click(object sender, RoutedEventArgs e)
    {
        CopyToClipboard();
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
