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

        ProcessCommandline();
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
        _log.Debug($"{AppInfo.AppName} Build date: {BuildInfo.BuildDateUtc.ToUniversalTime():f} (UTC)");
        _log.Debug($"{AppInfo.AppName} Commit ID: {BuildInfo.CommitIDString} ");

        // Log the .NET version, app framework and OS platform
        string version = Environment.Version.ToString();
        _log.Debug($".NET version: {AppInfo.RuntimeVersion.Replace(".NET", "")}  ({version})");
        _log.Debug(AppInfo.Framework);
        _log.Debug(AppInfo.OsPlatform);

        // Window position
        MainWindowHelpers.SetWindowPosition();

        // Light or dark
        MainWindowUIHelpers.SetBaseTheme(UserSettings.Setting.UITheme);

        // Primary accent color
        MainWindowUIHelpers.SetPrimaryColor(UserSettings.Setting.PrimaryColor);

        // UI size
        double size = MainWindowUIHelpers.UIScale(UserSettings.Setting.UISize);
        MainGrid.LayoutTransform = new ScaleTransform(size, size);

        // Check registry for run
        UserSettings.Setting.HistoryOnBoot = RegRun.RegRunEntry("TimVer");

        // Settings change event
        UserSettings.Setting.PropertyChanged += SettingChange.UserSettingChanged;

        // Initial page viewed
        NavigateToPage(UserSettings.Setting.InitialPage);
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

    #region Navigation
    /// <summary>
    /// Navigates to the requested dialog or page
    /// </summary>
    private void NavigateToPage(NavPage page)
    {
        NavListBox.SelectedIndex = (int)page;
        _ = NavListBox.Focus();
        switch (page)
        {
            case NavPage.WindowsInfo:
                DataContext = new WindowsInfoPage();
                PageTitle.Content = "Windows Information";
                break;
            case NavPage.ComputerInfo:
                DataContext = new ComputerInfoPage();
                PageTitle.Content = "Computer Information";
                break;
            case NavPage.History:
                DataContext = new HistoryPage();
                PageTitle.Content = "History";
                break;
            case NavPage.Environment:
                DataContext = new EnvVarPage();
                PageTitle.Content = "Environment Variables";
                break;
            case NavPage.Settings:
                DataContext = new SettingsPage();
                PageTitle.Content = "Settings";
                break;
            case NavPage.About:
                DataContext = new AboutPage();
                PageTitle.Content = "About";
                break;
            case NavPage.Exit:
                Application.Current.Shutdown();
                break;
        }
    }

    private void NavListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (IsLoaded)
        {
            NavigateToPage((NavPage)NavListBox.SelectedIndex);
        }
    }
    #endregion

    #region Process the command line
    /// <summary>
    /// Parse any command line options
    /// </summary>
    private void ProcessCommandline()
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
            EnvVarViewModel.GetEnvironmentVariables();
            SettingsViewModel.ParseInitialPage();
            NavigateToPage(UserSettings.Setting.InitialPage);
        }
    }
    #endregion Process the command line

    #region Keyboard Events
    /// <summary>
    /// Keyboard events for window
    /// </summary>
    private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
    {
        // No Ctrl or Shift
        switch (e.Key)
        {
            case Key.F1:
                {
                    NavigateToPage(NavPage.About);
                    break;
                }
        }
        // CTRL key combos
        if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
        {
            switch (e.Key)
            {
                case Key.OemComma:
                    {
                        NavigateToPage(NavPage.Settings);
                        break;
                    }
                case Key.C:
                    {
                        if (MainContent.Content != null)
                        {
                            Commands.CopyToClipboard(MainContent);
                        }
                        break;
                    }
                case Key.M:
                    {
                        switch (UserSettings.Setting.UITheme)
                        {
                            case ThemeType.Light:
                                UserSettings.Setting.UITheme = ThemeType.Dark;
                                break;
                            case ThemeType.Dark:
                                UserSettings.Setting.UITheme = ThemeType.Darker;
                                break;
                            case ThemeType.Darker:
                                UserSettings.Setting.UITheme = ThemeType.System;
                                break;
                            case ThemeType.System:
                                UserSettings.Setting.UITheme = ThemeType.Light;
                                break;
                        }
                        SnackbarMsg.ClearAndQueueMessage($"Theme set to {UserSettings.Setting.UITheme}");
                        break;
                    }
                case Key.N:
                    if (e.Key == Key.N)
                    {
                        if ((int)UserSettings.Setting.PrimaryColor >= (int)AccentColor.BlueGray)
                        {
                            UserSettings.Setting.PrimaryColor = 0;
                        }
                        else
                        {
                            UserSettings.Setting.PrimaryColor++;
                        }
                        SnackbarMsg.ClearAndQueueMessage($"Accent color set to {UserSettings.Setting.PrimaryColor}");
                    }
                    break;
                case Key.Add:
                    {
                        EverythingLarger();
                        SnackbarMsg.ClearAndQueueMessage($"Size set to {UserSettings.Setting.UISize}");
                        break;
                    }
                case Key.Subtract:
                    {
                        EverythingSmaller();
                        SnackbarMsg.ClearAndQueueMessage($"Size set to {UserSettings.Setting.UISize}");
                        break;
                    }
            }
        }
    }
    #endregion Keyboard Events

    #region Smaller/Larger
    /// <summary>
    /// Decreases the size of the UI
    /// </summary>
    public void EverythingSmaller()
    {
        int size = (int)UserSettings.Setting.UISize;
        if (size > 0)
        {
            size--;
            UserSettings.Setting.UISize = (MySize)size;
            double newSize = MainWindowUIHelpers.UIScale((MySize)size);
            MainGrid.LayoutTransform = new ScaleTransform(newSize, newSize);
        }
    }

    /// <summary>
    /// Increases the size of the UI
    /// </summary>
    public void EverythingLarger()
    {
        int size = (int)UserSettings.Setting.UISize;
        if (size < (int)MySize.Largest)
        {
            size++;
            UserSettings.Setting.UISize = (MySize)size;
            double newSize = MainWindowUIHelpers.UIScale((MySize)size);
            MainGrid.LayoutTransform = new ScaleTransform(newSize, newSize);
        }
    }
    #endregion Smaller/Larger
}
