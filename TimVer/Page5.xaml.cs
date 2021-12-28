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
        _ = await TextFileViewer.ViewTextFile(Path.Combine(dir, "License.txt")).ConfigureAwait(false);
    }

    private async void BtnReadme_Click(object sender, RoutedEventArgs e)
    {
        string dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        _ = await TextFileViewer.ViewTextFile(Path.Combine(dir, "ReadMe.txt")).ConfigureAwait(false);
    }

    private void BtnExit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
    #endregion Other Click events

    #region Get log file name
    public static string TempLogFile
    {
        get
        {
            // Ask NLog what the file name is
            using (FileTarget target = LogManager.Configuration.FindTargetByName("logFile") as FileTarget)
            {
                if (target != null)
                {
                    LogEventInfo logEventInfo = new() { TimeStamp = DateTime.Now };
                    return target.FileName.Render(logEventInfo);
                }
            }
            return null;
        }
    }
    #endregion Get log file name

    private async void Log_Click(object sender, RoutedEventArgs e)
    {
        _ = await TextFileViewer.ViewTextFile(TempLogFile).ConfigureAwait(false);
    }
}
