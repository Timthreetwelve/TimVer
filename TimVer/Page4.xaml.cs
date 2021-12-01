// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region using directives
using System;
using System.Windows;
using System.Windows.Controls;
using NLog;
using NLog.Targets;
using TKUtils;
#endregion using directives

namespace TimVer;

public partial class Page4 : Page
{
    #region NLog Instance
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
    #endregion NLog Instance

    public Page4()
    {
        InitializeComponent();
    }

    #region Check registry on page load
    private void Page_Loaded(object sender, EventArgs e)
    {
        cbxHistOnStart.IsChecked = RegRun.RegRunEntry("TimVer");
    }
    #endregion Check registry on page load

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
        TextFileViewer.ViewTextFile(GetTempLogFile());
    }
    #endregion Button events

    #region Get log file name
    public static string GetTempLogFile()
    {
        // Ask NLog what the file name is
        FileTarget target = LogManager.Configuration.FindTargetByName("logFile") as FileTarget;
        LogEventInfo logEventInfo = new() { TimeStamp = DateTime.Now };
        return target.FileName.Render(logEventInfo);
    }
    #endregion Get log file name
}
