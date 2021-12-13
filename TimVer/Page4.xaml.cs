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

        cbxMode.SelectedIndex = UserSettings.Setting.DarkMode;

        switch (UserSettings.Setting.SizeZoom)
        {
            case 0.85:
                cbxSize.SelectedIndex = 0;
                break;
            case 0.90:
                cbxSize.SelectedIndex = 1;
                break;
            case 0.95:
                cbxSize.SelectedIndex = 2;
                break;
            case 1.0:
                cbxSize.SelectedIndex = 3;
                break;
            case 1.05:
                cbxSize.SelectedIndex = 4;
                break;
        }

        cbxPage.SelectedIndex = UserSettings.Setting.InitialPage - 1;
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

    #region ComboBox events
    private void SizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox)
        {
            switch (((ComboBoxItem)comboBox.SelectedItem).Tag.ToString())
            {
                case "1":
                    UserSettings.Setting.SizeZoom = 0.85;
                    break;
                case "2":
                    UserSettings.Setting.SizeZoom = 0.90;
                    break;
                case "3":
                    UserSettings.Setting.SizeZoom = 0.95;
                    break;
                case "4":
                    UserSettings.Setting.SizeZoom = 1.0;
                    break;
                case "5":
                    UserSettings.Setting.SizeZoom = 1.05;
                    break;
                default:
                    UserSettings.Setting.SizeZoom = 1.0;
                    break;
            }
        }
    }

    private void CbxPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox)
        {
            UserSettings.Setting.InitialPage = Convert.ToInt32(((ComboBoxItem)comboBox.SelectedItem).Tag);
        }
    }

    private void CbxMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox)
        {
            UserSettings.Setting.DarkMode = Convert.ToInt32(((ComboBoxItem)comboBox.SelectedItem).Tag);
        }
    }
    #endregion ComboBox events
}
