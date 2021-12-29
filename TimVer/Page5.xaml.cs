// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

/// <summary>
/// Displays the About page
/// </summary>
public partial class Page5 : UserControl
{
    public Page5()
    {
        InitializeComponent();
    }

    #region Clicked on the GitHub link
    private void OnNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process p = new();
        p.StartInfo.FileName = e.Uri.AbsoluteUri;
        p.StartInfo.UseShellExecute = true;
        p.Start();
        e.Handled = true;
    }
    #endregion Clicked on the GitHub link

    #region Other Click events
    private async void BtnLicense_Click(object sender, RoutedEventArgs e)
    {
        string dir = AppInfo.AppDirectory;
        _ = await TextFileViewer.ViewTextFile(Path.Combine(dir, "License.txt")).ConfigureAwait(true);
    }

    private async void BtnReadme_Click(object sender, RoutedEventArgs e)
    {
        string dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        _ = await TextFileViewer.ViewTextFile(Path.Combine(dir, "ReadMe.txt")).ConfigureAwait(true);
    }

    private void BtnExit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private async void BtnLog_Click(object sender, RoutedEventArgs e)
    {
        _ = await TextFileViewer.ViewTextFile(NLHelpers.GetLogfileName()).ConfigureAwait(true);
    }
    #endregion Other Click events
}
