// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Class for methods used by the MainWindow and perhaps other classes.
/// </summary>
internal static class MainWindowHelpers
{
    private static bool UpdateHistoryOnly { get; set; }

    #region Startup
    internal static void TimVerStartUp()
    {
        EventHandlers();

        CheckCommandLine();
    }
    #endregion Startup

    #region MainWindow Instance
    private static readonly MainWindow? _mainWindow = Application.Current.MainWindow as MainWindow;
    #endregion MainWindow Instance

    #region StopWatch
    private static readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    #endregion StopWatch

    #region Apply UI settings
    /// <summary>
    /// Single method called during startup to apply UI settings.
    /// </summary>
    private static void ApplyUISettings()
    {
        // Make the window visible
        _mainWindow!.Visibility = Visibility.Visible;

        // Put version number in window title
        _mainWindow.Title = WindowTitleVersionAdmin();

        // Window position
        SetWindowPosition();

        // Light or dark theme
        SetBaseTheme(UserSettings.Setting!.UITheme);

        // Primary accent color
        SetPrimaryColor(UserSettings.Setting.PrimaryColor);

        // UI size
        UIScale(UserSettings.Setting.UISize);
    }
    #endregion Apply UI settings

    #region Set and Save MainWindow position and size
    /// <summary>
    /// Sets the MainWindow position and size.
    /// </summary>
    private static void SetWindowPosition()
    {
        Window? mainWindow = Application.Current.MainWindow;
        mainWindow!.Height = UserSettings.Setting!.WindowHeight;
        mainWindow.Left = UserSettings.Setting.WindowLeft;
        mainWindow.Top = UserSettings.Setting.WindowTop;
        mainWindow.Width = UserSettings.Setting.WindowWidth;

        if (UserSettings.Setting.StartCentered)
        {
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        // The following will ensure that the window appears on screen
        if (mainWindow.Top < SystemParameters.VirtualScreenTop)
        {
            mainWindow.Top = SystemParameters.VirtualScreenTop;
        }

        if (mainWindow.Left < SystemParameters.VirtualScreenLeft)
        {
            mainWindow.Left = SystemParameters.VirtualScreenLeft;
        }

        if (mainWindow.Left + mainWindow.Width > SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth)
        {
            mainWindow.Left = SystemParameters.VirtualScreenWidth + SystemParameters.VirtualScreenLeft - mainWindow.Width;
        }

        if (mainWindow.Top + mainWindow.Height > SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight)
        {
            mainWindow.Top = SystemParameters.VirtualScreenHeight + SystemParameters.VirtualScreenTop - mainWindow.Height;
        }
    }

    /// <summary>
    /// Saves the MainWindow position and size.
    /// </summary>
    private static void SaveWindowPosition()
    {
        Window? mainWindow = Application.Current.MainWindow;
        UserSettings.Setting!.WindowHeight = Math.Floor(mainWindow!.Height);
        UserSettings.Setting.WindowLeft = Math.Floor(mainWindow.Left);
        UserSettings.Setting.WindowTop = Math.Floor(mainWindow.Top);
        UserSettings.Setting.WindowWidth = Math.Floor(mainWindow.Width);
    }
    #endregion Set and Save MainWindow position and size

    #region Window title
    /// <summary>
    /// Puts the version number in the title bar as well as Administrator if running elevated
    /// </summary>
    private static string WindowTitleVersionAdmin()
    {
        // Set the windows title
        return AppInfo.IsAdmin
               ? $"{AppInfo.AppProduct}  {BuildInfo.VersionString} - ({GetStringResource("MsgText_WindowTitleAdministrator")})"
               : $"{AppInfo.AppProduct}  {BuildInfo.VersionString}";
    }
    #endregion Window title

    #region Event handlers
    /// <summary>
    /// Event handlers.
    /// </summary>
    private static void EventHandlers()
    {
        // Settings change events
        UserSettings.Setting!.PropertyChanged += SettingChange.UserSettingChanged!;
        TempSettings.Setting!.PropertyChanged += SettingChange.TempSettingChanged!;

        // Window closing event
        _mainWindow!.Closing += MainWindow_Closing!;
    }
    #endregion Event handlers

    #region Window Events
    private static void MainWindow_Closing(object sender, CancelEventArgs e)
    {
        // Stop the _stopwatch and record elapsed time
        _stopwatch.Stop();
        _log.Info($"{AppInfo.AppName} {GetStringResource("MsgText_ApplicationShutdown")}.  " +
                         $"{GetStringResource("MsgText_ElapsedTime")}: {_stopwatch.Elapsed:h\\:mm\\:ss\\.ff}");

        // Shut down NLog
        LogManager.Shutdown();

        if (!UpdateHistoryOnly)
        {
            // Clear any remaining messages
            Snackbar snackbar = _mainWindow!.SnackBar1;
            if (snackbar is not null)
            {
                snackbar.MessageQueue!.Clear();
            }

            // Save settings
            SaveWindowPosition();
            ConfigHelpers.SaveSettings();
        }
    }
    #endregion Window Events

    #region Log Startup messages
    /// <summary>
    /// Writes startup messages to the log.
    /// </summary>
    internal static void LogStartup()
    {
        // Log the version, commit date and commit id
        _log.Info($"{AppInfo.AppName} ({AppInfo.AppProduct}) {BuildInfo.VersionString} {GetStringResource("MsgText_ApplicationStarting")}");
        _log.Info($"{AppInfo.AppName} {AppInfo.AppCopyright}");
        _log.Debug($"{AppInfo.AppName} was started from {PathHelpers.AnonymizePath(AppInfo.AppPath)}");
        _log.Debug($"{AppInfo.AppName} Commit date: {BuildInfo.CommitDateStringUtc} - {BuildInfo.CommitDateStringLocal}");
        _log.Debug($"{AppInfo.AppName} Commit ID: {BuildInfo.CommitIDString}");
        _log.Debug($"{AppInfo.AppName} Process ID: {AppInfo.AppProcessID}");
        if (!string.IsNullOrEmpty(BuildInfo.Prerelease))
        {
            _log.Warn($"{AppInfo.AppName} is a prerelease version: {BuildInfo.Prerelease}");
        }
        if (AppInfo.IsAdmin)
        {
            _log.Debug($"{AppInfo.AppName} is running as Administrator");
        }
        if (CommandLineHelpers.CommandLineParserError is not null)
        {
            _log.Warn(CommandLineHelpers.CommandLineParserError);
        }

        // Log the .NET version and OS platform
        _log.Debug($"Operating System version: {AppInfo.OsPlatform}");
        _log.Debug($".NET version: {AppInfo.RuntimeVersion.Replace(".NET", "")}");
    }
    #endregion Log Startup messages

    #region Set theme
    /// <summary>
    /// Gets the current theme
    /// </summary>
    /// <returns>Dark or Light</returns>
    private static string GetSystemTheme()
    {
        BaseTheme? sysTheme = Theme.GetSystemTheme();
        return sysTheme != null ? sysTheme.ToString()! : string.Empty;
    }

    /// <summary>
    /// Sets the theme
    /// </summary>
    /// <param name="mode">A value in the ThemeType enum</param>
    internal static void SetBaseTheme(ThemeType mode)
    {
        //Retrieve the app's existing theme
        PaletteHelper paletteHelper = new();
        Theme theme = paletteHelper.GetTheme();

        if (mode == ThemeType.System)
        {
            var systemTheme = GetSystemTheme();
            mode = systemTheme.Equals("light", StringComparison.OrdinalIgnoreCase)
                ? UserSettings.Setting!.SystemLightTheme
                : UserSettings.Setting!.SystemDarkTheme;

            // Guard against invalid config values (e.g., System) so we always apply a concrete theme.
            if (mode == ThemeType.System)
            {
                mode = systemTheme.Equals("light", StringComparison.OrdinalIgnoreCase) ? ThemeType.Light : ThemeType.Darker;
            }

#if DEBUG
            // For testing: log or expose the selected theme for verification
            Debug.WriteLine($"System theme detected: {systemTheme}, applied: {mode}");
#endif
        }

        switch (mode)
        {
            case ThemeType.Light: // Light
                theme.SetBaseTheme(BaseTheme.Light);
                theme.Background = Colors.WhiteSmoke;
                break;
            case ThemeType.LightGray: // Pale Graphite
                theme.SetBaseTheme(BaseTheme.Light);
                theme.Background = (Color)ColorConverter.ConvertFromString("#FFD3D3D3");
                theme.Foreground = (Color)ColorConverter.ConvertFromString("#EE111111");
                theme.Cards.Background = (Color)ColorConverter.ConvertFromString("#FFE0E0E0");
                theme.DataGrids.Selected = (Color)ColorConverter.ConvertFromString("#FFC0C0C0");
                theme.Separators.Background = (Color)ColorConverter.ConvertFromString("#FFA9A9A9");
                break;
            case ThemeType.Dark: // Material Dark
                theme.SetBaseTheme(BaseTheme.Dark);
                break;
            case ThemeType.Darker: // Darker
                theme.SetBaseTheme(BaseTheme.Dark);
                theme.Cards.Background = (Color)ColorConverter.ConvertFromString("#FF141414");
                theme.Background = (Color)ColorConverter.ConvertFromString("#FF202020");
                theme.Foreground = (Color)ColorConverter.ConvertFromString("#E5F0F0F0");
                theme.DataGrids.Selected = (Color)ColorConverter.ConvertFromString("#FF303030");
                theme.Separators.Background = (Color)ColorConverter.ConvertFromString("#FF444951");
                break;
            case ThemeType.DarkBlue: // Midnight Blue
                theme.SetBaseTheme(BaseTheme.Dark);
                theme.Background = (Color)ColorConverter.ConvertFromString("#FF000F25");
                theme.Cards.Background = (Color)ColorConverter.ConvertFromString("#FF011636");
                theme.DataGrids.Selected = (Color)ColorConverter.ConvertFromString("#FF274470");
                theme.Foreground = (Color)ColorConverter.ConvertFromString("#FFD3D3E3");
                theme.Separators.Background = (Color)ColorConverter.ConvertFromString("#FF003C85");
                theme.ToolTips.Background = (Color)ColorConverter.ConvertFromString("#FF63afff");
                break;
            default:
                theme.SetBaseTheme(BaseTheme.Light);
                break;
        }

        //Change the app's current theme
        paletteHelper.SetTheme(theme);
    }
    #endregion Set theme

    #region Set accent color
    /// <summary>
    /// Sets the MDIX primary accent color
    /// </summary>
    /// <param name="color">One of the 18 MDIX color values plus Black and White</param>
    internal static void SetPrimaryColor(AccentColor color)
    {
        PaletteHelper paletteHelper = new();
        Theme theme = paletteHelper.GetTheme();
        PrimaryColor primary = color switch
        {
            AccentColor.Red => PrimaryColor.Red,
            AccentColor.Pink => PrimaryColor.Pink,
            AccentColor.Purple => PrimaryColor.Purple,
            AccentColor.DeepPurple => PrimaryColor.DeepPurple,
            AccentColor.Indigo => PrimaryColor.Indigo,
            AccentColor.Blue => PrimaryColor.Blue,
            AccentColor.LightBlue => PrimaryColor.LightBlue,
            AccentColor.Cyan => PrimaryColor.Cyan,
            AccentColor.Teal => PrimaryColor.Teal,
            AccentColor.Green => PrimaryColor.Green,
            AccentColor.LightGreen => PrimaryColor.LightGreen,
            AccentColor.Lime => PrimaryColor.Lime,
            AccentColor.Yellow => PrimaryColor.Yellow,
            AccentColor.Amber => PrimaryColor.Amber,
            AccentColor.Orange => PrimaryColor.Orange,
            AccentColor.DeepOrange => PrimaryColor.DeepOrange,
            AccentColor.Brown => PrimaryColor.Brown,
            AccentColor.Gray => PrimaryColor.Grey,
            AccentColor.BlueGray => PrimaryColor.BlueGrey,
            _ => PrimaryColor.Blue,
        };
        if (color == AccentColor.Black)
        {
            theme.SetPrimaryColor(Colors.Black);
        }
        else if (color == AccentColor.White)
        {
            theme.SetPrimaryColor(Colors.White);
        }
        else
        {
            Color primaryColor = SwatchHelper.Lookup[(MaterialDesignColor)primary];
            theme.SetPrimaryColor(primaryColor);
        }
        paletteHelper.SetTheme(theme);
    }
    #endregion Set accent color

    #region Set UI size
    /// <summary>
    /// Sets the value for UI scaling
    /// </summary>
    /// <param name="size">One of 7 values</param>
    /// <returns>Scaling multiplier</returns>
    internal static void UIScale(MySize size)
    {
        double newSize = size switch
        {
            MySize.Smallest => 0.8,
            MySize.Smaller => 0.9,
            MySize.Small => 0.95,
            MySize.Default => 1.0,
            MySize.Large => 1.05,
            MySize.Larger => 1.1,
            MySize.Largest => 1.2,
            _ => 1.0,
        };
        _mainWindow!.MainGrid.LayoutTransform = new ScaleTransform(newSize, newSize);
    }

    /// <summary>
    /// Decreases the size of the UI
    /// </summary>
    public static void EverythingSmaller()
    {
        MySize size = UserSettings.Setting!.UISize;
        if (size > 0)
        {
            size--;
            UserSettings.Setting.UISize = size;
            UIScale(UserSettings.Setting.UISize);
        }
    }

    /// <summary>
    /// Increases the size of the UI
    /// </summary>
    public static void EverythingLarger()
    {
        MySize size = UserSettings.Setting!.UISize;
        if (size < MySize.Largest)
        {
            size++;
            UserSettings.Setting.UISize = size;
            UIScale(UserSettings.Setting.UISize);
        }
    }
    #endregion Set UI size

    #region Find a parent of a control
    /// <summary>
    /// Finds the Parent of the given item in the visual tree.
    /// </summary>
    /// <typeparam name="T">The type of the queried item.</typeparam>
    /// <param name="child">x:Name or Name of child.</param>
    /// <returns>The parent object.</returns>
    public static T FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        //get parent item
        DependencyObject? parentObject = VisualTreeHelper.GetParent(child);

        //we've reached the end of the tree
        if (parentObject == null)
            return null!;

        //check if the parent matches the type we're looking for
        if (parentObject is T parent)
            return parent;
        else
            return FindParent<T>(parentObject);
    }
    #endregion Find a parent of a control

    #region Change mouse pointer
    /// <summary>
    /// Change to the wait mouse cursor.
    /// </summary>
    public static void MainWindowWaitPointer()
    {
        _mainWindow!.Cursor = Cursors.Wait;
    }

    /// <summary>
    /// Change to the normal mouse cursor.
    /// </summary>
    public static void MainWindowNormalPointer()
    {
        _mainWindow!.Cursor = Cursors.Arrow;
    }
    #endregion Change mouse pointer

    #region Check command line
    /// <summary>
    /// Write build history and exit or start UI based on command line/
    /// </summary>
    private static void CheckCommandLine()
    {
        CommandLineHelpers.CommandLineArgs parsedArgs = CommandLineHelpers.ProcessCommandLine();
        if (parsedArgs == CommandLineHelpers.CommandLineArgs.Hide)
        {
            UpdateHistoryOnly = true;
            if (UserSettings.Setting!.KeepHistory)
            {
                _log.Debug("Command line argument \"hide\" specified. Will update build history log if needed. ");
                HistoryHelpers.WriteHistory();
            }
            else
            {
                _log.Warn(GetStringResource("MsgText_HideButNoHistory"));
            }
            Application.Current.Shutdown();
        }

        ApplyUISettings();
        TempSettings.Setting!.RunAccessPermitted = RegistryHelpers.RegRunAccessPermitted();
    }
    #endregion Check command line
}
