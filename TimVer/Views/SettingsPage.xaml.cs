// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using NLog.Fluent;

namespace TimVer.Views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        #region NLog Instance
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        #endregion NLog Instance

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void HistOnStart_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded && !RegRun.RegRunEntry("TimVer"))
            {
                string result = RegRun.AddRegEntry("TimVer", AppInfo.AppPath + " /hide");
                if (result == "OK")
                {
                    _log.Info(@"TimVer added to HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    _ = new MDCustMsgBox(
                        "TimVer was added to Windows startup",
                        "TimVer",
                        ButtonType.Ok,
                        true,
                        true,
                        Application.Current.MainWindow,
                        false).ShowDialog();
                }
                else
                {
                    _log.Info($"TimVer add to startup failed: {result}");
                    _ = new MDCustMsgBox(
                        "Failed to add TimVer to Windows startup.\n\nSee log file for additional info.",
                        "TimVer",
                        ButtonType.Ok,
                        true,
                        true,
                        Application.Current.MainWindow,
                        true).ShowDialog();
                }
            }
        }

        private void HistOnStart_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                string result = RegRun.RemoveRegEntry("TimVer");
                if (result == "OK")
                {
                    _log.Info(@"TimVer removed from HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    _ = new MDCustMsgBox(
                        "TimVer was removed from Windows startup",
                        "TimVer",
                        ButtonType.Ok,
                        true,
                        true,
                        Application.Current.MainWindow,
                        false).ShowDialog();
                }
                else
                {
                    _log.Info($"Attempt to remove startup entry failed: {result}");
                    _ = new MDCustMsgBox(
                        "Failed to remove TimVer from Windows startup.\n\nSee log file for additional info.",
                        "TimVer",
                        ButtonType.Ok,
                        true,
                        true,
                        Application.Current.MainWindow,
                        true).ShowDialog();
                }
            }
        }
    }
}
