// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

/// <summary>
/// Displays Windows information page
/// </summary>
public partial class Page1 : Page
{
    public Page1()
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
        _ = builder.AppendLine("WINDOWS INFORMATION");
        _ = builder.Append("Product Name   = ").AppendLine(CombinedInfo.ProdName);
        _ = builder.Append("Version        = ").AppendLine(CombinedInfo.Version);
        _ = builder.Append("Build          = ").AppendLine(CombinedInfo.Build);
        _ = builder.Append("Architecture   = ").AppendLine(CombinedInfo.Arch);
        _ = builder.Append("Build Branch   = ").AppendLine(CombinedInfo.BuildBranch);
        _ = builder.Append("Edition ID     = ").AppendLine(CombinedInfo.EditionID);
        _ = builder.Append("Installed on   = ").AppendLine(CombinedInfo.InstallDate);
        _ = builder.Append("Windows Folder = ").AppendLine(CombinedInfo.WindowsFolder);
        _ = builder.Append("Temp Folder    = ").AppendLine(CombinedInfo.TempFolder);
        if (UserSettings.Setting.ShowUser)
        {
            _ = builder.Append("Registered to  = ").AppendLine(CombinedInfo.RegUser);
        }
        Clipboard.SetText(builder.ToString());
    }
    #endregion Copy to clipboard
}
