// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Class for methods used by the MainWindow and perhaps other classes.
/// </summary>
internal static class MainWindowHelpers
{
    #region Startup
    internal static void TimVerStartUp()
    {
        EventHandlers();

        MainWindowUIHelpers.ApplyUISettings();

        if (CommandLineHelpers.ProcessCommandLine())
        {
            _mainWindow!.Visibility = Visibility.Hidden;
            if (UserSettings.Setting!.KeepHistory)
            {
                HistoryHelpers.WriteHistory();
            }
            else
            {
                _log.Warn(GetStringResource("MsgText_HideButNoHistory"));
            }
            Application.Current.Shutdown();
        }
        TempSettings.Setting!.RunAccessPermitted = RegistryHelpers.RegRunAccessPermitted();
    }
    #endregion Startup

    #region MainWindow Instance
    private static readonly MainWindow? _mainWindow = Application.Current.MainWindow as MainWindow;
    #endregion MainWindow Instance

    #region StopWatch
    public static readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    #endregion StopWatch

    #region Set and Save MainWindow position and size
    /// <summary>
    /// Sets the MainWindow position and size.
    /// </summary>
    public static void SetWindowPosition()
    {
        Window mainWindow = Application.Current.MainWindow;
        mainWindow.Height = UserSettings.Setting!.WindowHeight;
        mainWindow.Left = UserSettings.Setting!.WindowLeft;
        mainWindow.Top = UserSettings.Setting!.WindowTop;
        mainWindow.Width = UserSettings.Setting!.WindowWidth;

        if (UserSettings.Setting.StartCentered)
        {
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }

    /// <summary>
    /// Saves the MainWindow position and size.
    /// </summary>
    public static void SaveWindowPosition()
    {
        Window mainWindow = Application.Current.MainWindow;
        UserSettings.Setting!.WindowHeight = Math.Floor(mainWindow.Height);
        UserSettings.Setting!.WindowLeft = Math.Floor(mainWindow.Left);
        UserSettings.Setting!.WindowTop = Math.Floor(mainWindow.Top);
        UserSettings.Setting!.WindowWidth = Math.Floor(mainWindow.Width);
    }
    #endregion Set and Save MainWindow position and size

    #region Window title
    /// <summary>
    /// Puts the version number in the title bar as well as Administrator if running elevated
    /// </summary>
    public static string WindowTitleVersionAdmin()
    {
        // Set the windows title
        return AppInfo.IsAdmin
            ? $"{AppInfo.AppProduct}  {AppInfo.AppProductVersion} - ({GetStringResource("MsgText_WindowTitleAdministrator")})"
            : $"{AppInfo.AppProduct}  {AppInfo.AppProductVersion}";
    }
    #endregion Window title

    #region Event handlers
    /// <summary>
    /// Event handlers.
    /// </summary>
    internal static void EventHandlers()
    {
        // Settings change events
        UserSettings.Setting!.PropertyChanged += SettingChange.UserSettingChanged!;
        TempSettings.Setting!.PropertyChanged += SettingChange.TempSettingChanged!;

        // Window closing event
        _mainWindow.Closing += MainWindow_Closing!;
    }
    #endregion Event handlers

    #region Window Events
    private static void MainWindow_Closing(object sender, CancelEventArgs e)
    {
        // Clear any remaining messages
        Snackbar snackbar = MainWindowUIHelpers.FindChild<Snackbar>(Application.Current.MainWindow, "SnackBar1");
        snackbar.MessageQueue!.Clear();

        // Stop the _stopwatch and record elapsed time
        _stopwatch.Stop();
        _log.Info($"{AppInfo.AppName} {GetStringResource("MsgText_ApplicationShutdown")}.  " +
                         $"{GetStringResource("MsgText_ElapsedTime")}: {_stopwatch.Elapsed:h\\:mm\\:ss\\.ff}");

        // Shut down NLog
        LogManager.Shutdown();

        // Save settings
        SaveWindowPosition();
        ConfigHelpers.SaveSettings();
    }
    #endregion Window Events

    #region Write startup messages to the log
    /// <summary>
    /// Initializes NLog and writes startup messages to the log.
    /// </summary>
    internal static void LogStartup()
    {
        // Log the version, build date and commit id
        _log.Info($"{AppInfo.AppName} ({AppInfo.AppProduct}) {AppInfo.AppProductVersion} {GetStringResource("MsgText_ApplicationStarting")}");
        _log.Info($"{AppInfo.AppName} {AppInfo.AppCopyright}");
        _log.Debug($"{AppInfo.AppName} was started from {AppInfo.AppPath}");
        _log.Debug($"{AppInfo.AppName} Build date: {BuildInfo.BuildDateStringUtc}");
        _log.Debug($"{AppInfo.AppName} Commit ID: {BuildInfo.CommitIDString}");
        if (AppInfo.IsAdmin)
        {
            _log.Debug($"{AppInfo.AppName} is running as Administrator");
        }

        // Log the .NET version and OS platform
        _log.Debug($"Operating System version: {AppInfo.OsPlatform}");
        _log.Debug($".NET version: {AppInfo.RuntimeVersion.Replace(".NET", "")}");
    }
    #endregion Write startup messages to the log

    #region Toggle History item in navigation list
    /// <summary>
    /// Toggle the History item in the navigation ListBox.
    /// </summary>
    /// <remarks>
    /// Called during startup and by change to UserSettings.Setting.KeepHistory
    /// </remarks>
    public static void ToggleHistory()
    {
        ListBox NavBox = MainWindowUIHelpers.FindChild<ListBox>(Application.Current.MainWindow, "NavigationListBox");
        if (!UserSettings.Setting!.KeepHistory)
        {
            NavigationViewModel.PopulateNoHistoryList();
            NavBox.ItemsSource = NavigationViewModel.NavigationListNoHistory;
            NavBox.Items.Refresh();
            _log.Debug("Keep History option is false.");
        }
        else
        {
            NavBox.ItemsSource = NavigationViewModel.NavigationViewModelTypes;
            NavBox.Items.Refresh();
            HistoryViewModel.WriteHistory();
        }
    }
    #endregion Toggle History item in navigation list
}
