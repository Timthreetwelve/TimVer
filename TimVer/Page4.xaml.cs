// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

/// <summary>
/// Display the Options page
/// </summary>
public partial class Page4 : UserControl
{
    #region NLog Instance
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
    #endregion NLog Instance

    public Page4()
    {
        InitializeComponent();
    }

    #region Page loaded events
    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        tbHistOnStart.IsChecked = RegRun.RegRunEntry("TimVer");
    }
    #endregion Page loaded events

    #region Button events
    private async void BtnLogFile_Click(object sender, RoutedEventArgs e)
    {
        _ = await TextFileViewer.ViewTextFile(NLHelpers.GetLogfileName()).ConfigureAwait(true);
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
    #endregion Button events

    #region History on Windows startup
    private async void TbHistOnStart_CheckedAsync(object sender, RoutedEventArgs e)
    {
        if (IsLoaded && !RegRun.RegRunEntry("TimVer"))
        {
            string result = RegRun.AddRegEntry("TimVer", AppInfo.AppPath + " /hide");
            if (result == "OK")
            {
                log.Info(@"TimVer added to HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                OkDialog ok = new();
                ok.Message = "TimVer was added to Windows startup";
                _ = await DialogHost.Show(ok, "dh1").ConfigureAwait(true);
            }
            else
            {
                log.Info($"TimVer add to startup failed: {result}");
                ErrorDialog ed = new();
                ed.Message = "Failed to add TimVer to Windows startup.\n\nSee log file for additional info.";
                _ = await DialogHost.Show(ed, "dh1").ConfigureAwait(true);
            }
        }
    }

    private async void TbHistOnStart_Unchecked(object sender, RoutedEventArgs e)
    {
        if (IsLoaded)
        {
            string result = RegRun.RemoveRegEntry("TimVer");
            if (result == "OK")
            {
                log.Info(@"TimVer removed from HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                OkDialog ok = new();
                ok.Message = "TimVer was removed from Windows startup";
                _ = await DialogHost.Show(ok, "dh1").ConfigureAwait(true);
            }
            else
            {
                log.Info($"Attempt to remove startup entry failed: {result}");
                ErrorDialog ed = new();
                ed.Message = "Failed to remove TimVer from Windows startup.\n\nSee log file for additional info.";
                _ = await DialogHost.Show(ed, "dh1").ConfigureAwait(true);
            }
        }
    }
    #endregion History on Windows startup
}
