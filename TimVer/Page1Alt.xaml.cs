// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

/// <summary>
/// Displays Windows information page
/// </summary>
public partial class Page1Alt : UserControl
{
    public Page1Alt()
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
        Clipboard.SetText(builder.ToString());
    }
    #endregion Copy to clipboard
}
