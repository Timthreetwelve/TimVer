// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace TimVer;

public partial class Page5 : Page
{
    public Page5()
    {
        InitializeComponent();
    }

    private void Page_Loaded(object sender, System.EventArgs e)
    {
        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
        if (versionInfo != null)
        {
            tbName.Text = versionInfo.ProductName;
            tbVersion.Text = versionInfo.FileVersion.Remove(versionInfo.FileVersion.LastIndexOf("."));
            tbCopyright.Text = versionInfo.LegalCopyright.Replace("Copyright ", "");
            tbCommit.Text = Properties.Resources.CurrentCommit;
        }
    }

    private void OnNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process p = new();
        p.StartInfo.FileName = e.Uri.AbsoluteUri;
        p.StartInfo.UseShellExecute = true;
        p.Start();
        e.Handled = true;
    }

    private void BtnReadme_Click(object sender, RoutedEventArgs e)
    {
        string dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        TextFileViewer.ViewTextFile(Path.Combine(dir, "ReadMe.txt"));
    }
}
