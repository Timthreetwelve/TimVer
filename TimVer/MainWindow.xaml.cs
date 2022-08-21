// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

public partial class MainWindow
{
    #region NLog Instance
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
    #endregion NLog Instance

    #region Stopwatch
    private readonly Stopwatch stopwatch = new();
    #endregion Stopwatch

    public MainWindow()
    {
        InitializeSettings();

        InitializeComponent();

        ReadSettings();

        ProcessCommandLine();
    }

    #region Settings
    private void InitializeSettings()
    {
        stopwatch.Start();

        UserSettings.Init(UserSettings.AppFolder, UserSettings.DefaultFilename, true);
    }

    private void ReadSettings()
    {
        // Set NLog configuration
        NLHelpers.NLogConfig();

        // Unhandled exception handler
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        // Put the version number in the title bar
        Title = $"TimVer - {AppInfo.TitleVersion}";

        // Startup message in the temp file
        log.Info($"{AppInfo.AppName} ({AppInfo.AppProduct}) {AppInfo.AppVersion} is starting up");
        log.Info($"{AppInfo.AppCopyright}");
        log.Debug($"{AppInfo.AppName} Build date: {BuildInfo.BuildDateString} UTC");
        log.Debug($"{AppInfo.AppName} Commit ID: {BuildInfo.CommitIDString}");

        // Log the .NET version, app framework and OS platform
        string version = Environment.Version.ToString();
        log.Debug($".NET version: {AppInfo.RuntimeVersion.Replace(".NET", "")}  ({version})");
        log.Debug($"Framework Version: {AppInfo.Framework}");
        log.Debug($"Operating System: {AppInfo.OsPlatform}");

        // Window position
        Top = UserSettings.Setting.WindowTop;
        Left = UserSettings.Setting.WindowLeft;
        Height = UserSettings.Setting.WindowHeight;
        Width = UserSettings.Setting.WindowWidth;
        Topmost = UserSettings.Setting.KeepOnTop;

        // Light or dark
        SetBaseTheme(UserSettings.Setting.DarkMode);

        // Primary color
        SetPrimaryColor(UserSettings.Setting.PrimaryColor);

        // UI size
        double size = UIScale(UserSettings.Setting.UISize);
        MainGrid.LayoutTransform = new ScaleTransform(size, size);

        // Initial page viewed
        SetInitialView(UserSettings.Setting.InitialPage);

        // Settings change event
        UserSettings.Setting.PropertyChanged += UserSettingChanged;

        // Update history file (if needed)
        Page3.WriteHistory();
    }

    #endregion Settings

    #region Setting change
    private void UserSettingChanged(object sender, PropertyChangedEventArgs e)
    {
        PropertyInfo prop = sender.GetType().GetProperty(e.PropertyName);
        object newValue = prop?.GetValue(sender, null);
        log.Debug($"Setting change: {e.PropertyName} New Value: {newValue}");

        switch (e.PropertyName)
        {
            case nameof(UserSettings.Setting.KeepOnTop):
                Topmost = (bool)newValue;
                break;

            case nameof(UserSettings.Setting.IncludeDebug):
                NLHelpers.SetLogLevel((bool)newValue);
                break;

            case nameof(UserSettings.Setting.DarkMode):
                SetBaseTheme((int)newValue);
                break;

            case nameof(UserSettings.Setting.UISize):
                int size = (int)newValue;
                double newSize = UIScale(size);
                MainGrid.LayoutTransform = new ScaleTransform(newSize, newSize);
                break;

            case nameof(UserSettings.Setting.PrimaryColor):
                SetPrimaryColor((int)newValue);
                break;
        }
    }
    #endregion Setting change

    #region Set initial view
    private void SetInitialView(int page)
    {
        Stopwatch sw;
        switch (page)
        {
            case 0:
                sw = Stopwatch.StartNew();
                tabWinInfo.Content = UserSettings.Setting.Page1Alt ? new Page1Alt() : new Page1();
                sw.Stop();
                log.Debug($"Windows information loaded in {sw.Elapsed.TotalMilliseconds:N2} ms");
                break;
            case 1:
                sw = Stopwatch.StartNew();
                tabCompInfo.Content = new Page2();
                sw.Stop();
                log.Debug($"Computer information loaded in {sw.Elapsed.TotalMilliseconds:N2} ms");
                _ = tabCompInfo.Focus();
                break;
            case 2:
                sw = Stopwatch.StartNew();
                tabHistory.Content = new Page3();
                sw.Stop();
                log.Debug($"History loaded in {sw.Elapsed.TotalMilliseconds:N2} ms");
                _ = tabHistory.Focus();
                break;
            case 4:
                _ = tabSettings.Focus();
                break;
            case 5:
                _ = tabAbout.Focus();
                break;
            default:
                _ = tabWinInfo.Focus();
                break;
        }
    }
    #endregion Set initial view

    #region Tab selection changed
    private void TcMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is TabControl && IsLoaded)
        {
            if (tabWinInfo.IsSelected)
            {
                Stopwatch sw = Stopwatch.StartNew();
                tabWinInfo.Content = UserSettings.Setting.Page1Alt ? new Page1Alt() : new Page1();
                sw.Stop();
                log.Debug($"Windows information loaded in {sw.Elapsed.TotalMilliseconds:N2} ms");
            }

            if (tabCompInfo.IsSelected)
            {
                Stopwatch sw = Stopwatch.StartNew();
                tabCompInfo.Content = new Page2();
                sw.Stop();
                log.Debug($"Computer information loaded in {sw.Elapsed.TotalMilliseconds:N2} ms");
            }

            if (tabHistory.IsSelected)
            {
                Stopwatch sw = Stopwatch.StartNew();
                tabHistory.Content = new Page3();
                sw.Stop();
                log.Debug($"History loaded in {sw.Elapsed.TotalMilliseconds:N2} ms");
            }
        }
    }
    #endregion Tab selection changed

    #region Process command line args
    private void ProcessCommandLine()
    {
        log.Debug($"Startup time: {stopwatch.ElapsedMilliseconds} ms.");

        // If count is less that two, bail out
        string[] args = Environment.GetCommandLineArgs();
        if (args.Length < 2)
        {
            return;
        }

        foreach (string item in args)
        {
            if (item.Replace("-", "").Replace("/", "").Equals("hide", StringComparison.OrdinalIgnoreCase))
            {
                // hide the window
                Visibility = Visibility.Hidden;
                log.Info("Command line argument 'hide' was found. Shutting down.");
                Application.Current.Shutdown();
            }
        }
    }
    #endregion Process command line args

    #region Window closing
    private void Window_Closing(object sender, CancelEventArgs e)
    {
        stopwatch.Stop();
        log.Info($"{AppInfo.AppName} is shutting down.  Elapsed time: {stopwatch.Elapsed:h\\:mm\\:ss\\.ff}");

        // Shut down NLog
        LogManager.Shutdown();

        // Save settings
        UserSettings.Setting.WindowLeft = Math.Floor(Left);
        UserSettings.Setting.WindowTop = Math.Floor(Top);
        UserSettings.Setting.WindowWidth = Math.Floor(Width);
        UserSettings.Setting.WindowHeight = Math.Floor(Height);
        UserSettings.SaveSettings();
    }
    #endregion Window closing

    #region Unhandled Exception Handler
    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        log.Error("Unhandled Exception");
        Exception e = (Exception)args.ExceptionObject;
        log.Error(e.Message);
        if (e.InnerException != null)
        {
            log.Error(e.InnerException.ToString());
        }
        log.Error(e.StackTrace);
    }
    #endregion Unhandled Exception Handler

    #region Smaller/Larger
    private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control)
            return;

        if (e.Delta > 0)
        {
            EverythingLarger();
        }
        else if (e.Delta < 0)
        {
            EverythingSmaller();
        }
    }

    public void EverythingSmaller()
    {
        int size = UserSettings.Setting.UISize;
        if (size > 0)
        {
            size--;
            UserSettings.Setting.UISize = size;
            double newSize = UIScale(size);
            MainGrid.LayoutTransform = new ScaleTransform(newSize, newSize);
        }
    }

    public void EverythingLarger()
    {
        int size = UserSettings.Setting.UISize;
        if (size < 4)
        {
            size++;
            UserSettings.Setting.UISize = size;
            double newSize = UIScale(size);
            MainGrid.LayoutTransform = new ScaleTransform(newSize, newSize);
        }
    }
    #endregion Smaller/Larger

    #region Set light or dark theme
    private static void SetBaseTheme(int mode)
    {
        //Retrieve the app's existing theme
        PaletteHelper paletteHelper = new();
        ITheme theme = paletteHelper.GetTheme();

        switch (mode)
        {
            case 0:
                theme.SetBaseTheme(Theme.Light);
                break;
            case 1:
                theme.SetBaseTheme(Theme.Dark);
                break;
            case 2:
                if (GetSystemTheme().Equals("light", StringComparison.OrdinalIgnoreCase))
                {
                    theme.SetBaseTheme(Theme.Light);
                }
                else
                {
                    theme.SetBaseTheme(Theme.Dark);
                }
                break;
            default:
                theme.SetBaseTheme(Theme.Light);
                break;
        }

        //Change the app's current theme
        paletteHelper.SetTheme(theme);
    }

    private static string GetSystemTheme()
    {
        BaseTheme? sysTheme = Theme.GetSystemTheme();
        if (sysTheme != null)
        {
            return sysTheme.ToString();
        }
        return string.Empty;
    }
    #endregion Set light or dark theme

    #region Set primary color
    private void SetPrimaryColor(int color)
    {
        PaletteHelper paletteHelper = new PaletteHelper();
        ITheme theme = paletteHelper.GetTheme();

        PrimaryColor primary;
        switch (color)
        {
            case 0:
                primary = PrimaryColor.Red;
                break;
            case 1:
                primary = PrimaryColor.Pink;
                break;
            case 2:
                primary = PrimaryColor.Purple;
                break;
            case 3:
                primary = PrimaryColor.DeepPurple;
                break;
            case 4:
                primary = PrimaryColor.Indigo;
                break;
            case 5:
                primary = PrimaryColor.Blue;
                break;
            case 6:
                primary = PrimaryColor.LightBlue;
                break;
            case 7:
                primary = PrimaryColor.Cyan;
                break;
            case 8:
                primary = PrimaryColor.Teal;
                break;
            case 9:
                primary = PrimaryColor.Green;
                break;
            case 10:
                primary = PrimaryColor.LightGreen;
                break;
            case 11:
                primary = PrimaryColor.Lime;
                break;
            case 12:
                primary = PrimaryColor.Yellow;
                break;
            case 13:
                primary = PrimaryColor.Amber;
                break;
            case 14:
                primary = PrimaryColor.Orange;
                break;
            case 15:
                primary = PrimaryColor.DeepOrange;
                break;
            case 16:
                primary = PrimaryColor.Brown;
                break;
            case 17:
                primary = PrimaryColor.Grey;
                break;
            case 18:
                primary = PrimaryColor.BlueGrey;
                break;
            default:
                primary = PrimaryColor.Blue;
                break;
        }
        Color primaryColor = SwatchHelper.Lookup[(MaterialDesignColor)primary];
        theme.SetPrimaryColor(primaryColor);
        paletteHelper.SetTheme(theme);
    }
    #endregion Set primary color

    #region UI scale converter
    private static double UIScale(int size)
    {
        switch (size)
        {
            case 0:
                return 0.90;
            case 1:
                return 0.95;
            case 2:
                return 1.0;
            case 3:
                return 1.05;
            case 4:
                return 1.1;
            default:
                return 1.0;
        }
    }
    #endregion UI scale converter

    #region Dialog closing
    private void DialogClosing(object sender, DialogClosingEventArgs e)
    {
        if (!(bool)e.Parameter)
        {
            return;
        }
    }
    #endregion Dialog closing

    #region Keyboard events
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
        {
            if (e.Key == Key.M)
            {
                switch (UserSettings.Setting.DarkMode)
                {
                    case 0:
                        UserSettings.Setting.DarkMode = 1;
                        break;
                    case 1:
                        UserSettings.Setting.DarkMode = 2;
                        break;
                    case 2:
                        UserSettings.Setting.DarkMode = 0;
                        break;
                }
            }
            if (e.Key == Key.Add)
            {
                EverythingLarger();
            }
            if (e.Key == Key.Subtract)
            {
                EverythingSmaller();
            }
            if (e.Key == Key.OemComma)
            {
                _ = tabSettings.Focus();
            }
        }
        if (e.Key == Key.F1)
        {
            _ = tabAbout.Focus();
        }
    }
    #endregion Keyboard events

    #region Double click window to resize
    /// <summary>
    /// Double click the window to set optimal width
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        SizeToContent = SizeToContent.WidthAndHeight;
        _ = Task.Delay(50);
        SizeToContent = SizeToContent.Manual;
    }
    #endregion Double click window to resize
}
