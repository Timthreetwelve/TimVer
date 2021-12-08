// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

/// <summary>
/// Display the Options page
/// </summary>
public partial class Page4 : Page
{
    #region NLog Instance
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
    #endregion NLog Instance

    public Page4()
    {
        InitializeComponent();
    }

    #region Page loaded events
    private void Page_Loaded(object sender, EventArgs e)
    {
        cbxHistOnStart.IsChecked = RegRun.RegRunEntry("TimVer");

        switch (UserSettings.Setting.DarkMode)
        {
            case 0:
                rb0.IsChecked = true;
                break;
            case 1:
                rb1.IsChecked = true;
                break;
            case 2:
                rb2.IsChecked = true;
                break;
            default:
                rb0.IsChecked = true;
                break;
        }
    }
    #endregion Page loaded events

    #region History Checkbox
    private void CbxHsitory_Checked(object sender, RoutedEventArgs e)
    {
        if (IsLoaded && !RegRun.RegRunEntry("TimVer"))
        {
            string result = RegRun.AddRegEntry("TimVer", AppInfo.AppPath + " /hide");
            if (result == "OK")
            {
                log.Info(@"TimVer added to HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            }
            else
            {
                log.Info($"TimVer add to startup failed: {result}");
            }
        }
    }

    private void CbxHistory_Unchecked(object sender, RoutedEventArgs e)
    {
        if (IsLoaded)
        {
            string result = RegRun.RemoveRegEntry("TimVer");
            if (result == "OK")
            {
                log.Info(@"TimVer removed from HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            }
            else
            {
                log.Info($"Attempt to remove startup entry failed: {result}");
            }
        }
    }
    #endregion History Checkbox

    #region Button events
    private void BtnOpenLog_Click(object sender, RoutedEventArgs e)
    {
        TextFileViewer.ViewTextFile(TempLogFile);
    }
    private void RbMode_Checked(object sender, RoutedEventArgs e)
    {
        RadioButton rb = sender as RadioButton;
        UserSettings.Setting.DarkMode = Convert.ToUInt16(rb.Tag);
    }

    private void BtnSmaller_Click(object sender, RoutedEventArgs e)
    {
        double curZoom = UserSettings.Setting.SizeZoom;
        if (curZoom > 0.85)
        {
            curZoom -= .05;
            UserSettings.Setting.SizeZoom = Math.Round(curZoom, 2);
        }
    }
    private void BtnLarger_Click(object sender, RoutedEventArgs e)
    {
        double curZoom = UserSettings.Setting.SizeZoom;
        if (curZoom < 1.05)
        {
            curZoom += .05;
            UserSettings.Setting.SizeZoom = Math.Round(curZoom, 2);
        }
    }
    #endregion Button events

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

}
