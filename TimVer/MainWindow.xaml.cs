// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region Stopwatch
    private readonly Stopwatch _stopwatch = new();
    #endregion Stopwatch

    #region NLog Instance
    private static readonly Logger _log = LogManager.GetLogger("logTemp");
    #endregion NLog Instance

    public MainWindow()
    {
        ConfigHelpers.InitializeSettings();

        InitializeComponent();

        _stopwatch.Start();

        ReadSettings();

        ProcessCommandLine();
    }

    #region Settings
    public void ReadSettings()
    {
        // Set NLog configuration
        NLogHelpers.NLogConfig(false);

        // Unhandled exception handler
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        // Put version number in window title
        WindowTitleVersionAdmin();

        // Log the version, build date and commit id
        _log.Info($"{AppInfo.AppName} ({AppInfo.AppProduct}) {AppInfo.AppVersion} is starting up");
        _log.Info($"{AppInfo.AppName} {AppInfo.AppCopyright}");
        _log.Debug($"{AppInfo.AppName} Build date: {BuildInfo.BuildDateString} (UTC)");
        _log.Debug($"{AppInfo.AppName} Commit ID: {BuildInfo.CommitIDString} ");

        // Log the .NET version and OS platform
        _log.Debug($"Operating System version: {AppInfo.OsPlatform}");
        _log.Debug($".NET version: {AppInfo.RuntimeVersion.Replace(".NET", "")}");

        // Window position
        MainWindowHelpers.SetWindowPosition();

        // Light or dark
        MainWindowUIHelpers.SetBaseTheme(UserSettings.Setting.UITheme);

        // Primary accent color
        MainWindowUIHelpers.SetPrimaryColor(UserSettings.Setting.PrimaryColor);

        // UI size
        MainWindowUIHelpers.UIScale(UserSettings.Setting.UISize);

        // Check registry for run
        UserSettings.Setting.HistoryOnBoot = RegRun.RegRunEntry("TimVer");

        // Settings change event
        UserSettings.Setting.PropertyChanged += SettingChange.UserSettingChanged;
    }
    #endregion Settings

    #region Window Events
    private void Window_Closing(object sender, CancelEventArgs e)
    {
        // Clear any remaining messages
        SnackBar1.MessageQueue.Clear();

        // Stop the _stopwatch and record elapsed time
        _stopwatch.Stop();
        _log.Info($"{AppInfo.AppName} is shutting down.  Elapsed time: {_stopwatch.Elapsed:h\\:mm\\:ss\\.ff}");

        // Shut down NLog
        LogManager.Shutdown();

        // Save settings
        MainWindowHelpers.SaveWindowPosition();
        ConfigHelpers.SaveSettings();
    }
    #endregion Window Events

    #region Window Title
    /// <summary>
    /// Puts the version number in the title bar as well as Administrator if running elevated
    /// </summary>
    public void WindowTitleVersionAdmin()
    {
        // Set the windows title
        if (IsAdministrator())
        {
            Title = AppInfo.AppName + " - " + AppInfo.TitleVersion + " - (Administrator)";
        }
        else
        {
            Title = AppInfo.AppName + " - " + AppInfo.TitleVersion;
        }
    }
    #endregion Window Title

    #region Running as Administrator?
    /// <summary>
    /// Determines if running as administrator (elevated)
    /// </summary>
    /// <returns>True if running elevated</returns>
    public static bool IsAdministrator()
    {
        return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
    }
    #endregion Running as Administrator?

    #region Unhandled Exception Handler
    /// <summary>
    /// Handles any exceptions that weren't caught by a try-catch statement
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        _log.Error("Unhandled Exception");
        Exception e = (Exception)args.ExceptionObject;
        _log.Error(e.Message);
        if (e.InnerException != null)
        {
            _log.Error(e.InnerException.ToString());
        }
        _log.Error(e.StackTrace);

        _ = MessageBox.Show("An error has occurred. See the _log file",
            "ERROR",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
    #endregion Unhandled Exception Handler

    #region Process the command line
    /// <summary>
    /// Parse any command line options
    /// </summary>
    private void ProcessCommandLine()
    {
        // Since this is not a console app, get the command line args
        string[] args = Environment.GetCommandLineArgs();

        // Parser settings
        Parser parser = new(s =>
        {
            s.CaseSensitive = false;
            s.IgnoreUnknownArguments = true;
        });

        // parses the command line. result object will hold the arguments
        ParserResult<CommandLineOptions> result = parser.ParseArguments<CommandLineOptions>(args);

        // Check options
        if (result?.Value.Hide == true)
        {
            _log.Debug("Argument \"hide\" specified.");
            Visibility = Visibility.Hidden;
            HistoryViewModel.WriteHistory();
            Application.Current.Shutdown();
        }
        else
        {
            HistoryViewModel.WriteHistory();
            //EnvVarViewModel.GetEnvironmentVariables();
            SettingsViewModel.ParseInitialPage();
            //NavigateToPage(UserSettings.Setting.InitialPage);
        }
    }
    #endregion Process the command line
}
