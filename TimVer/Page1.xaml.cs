// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region Using directives
using System.Text;
using System.Windows;
using System.Windows.Controls;
#endregion Using directives

namespace TimVer;

public partial class Page1 : Page
{
    public Page1()
    {
        InitializeComponent();
        //DataContext = new InfoVM();
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
        _ = builder.Append("Product Name   = ").AppendLine(InfoVM.ProdName);
        _ = builder.Append("Version        = ").AppendLine(InfoVM.Version);
        _ = builder.Append("Build          = ").AppendLine(InfoVM.Build);
        _ = builder.Append("Architecture   = ").AppendLine(InfoVM.Arch);
        _ = builder.Append("Build Branch   = ").AppendLine(InfoVM.BuildBranch);
        _ = builder.Append("Edition ID     = ").AppendLine(InfoVM.EditionID);
        _ = builder.Append("Installed on   = ").AppendLine(InfoVM.InstallDate);
        _ = builder.Append("Windows Folder = ").AppendLine(InfoVM.WindowsFolder);
        _ = builder.Append("Temp Folder    = ").AppendLine(InfoVM.TempFolder);
        if (UserSettings.Setting.ShowUser)
        {
            _ = builder.Append("Registered to  = ").AppendLine(InfoVM.RegUser);
        }
        Clipboard.SetText(builder.ToString());
    }
    #endregion Copy to clipboard
}
