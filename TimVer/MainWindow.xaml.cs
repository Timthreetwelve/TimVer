﻿// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region using directives
global using System;
global using System.Collections.Generic;
global using System.ComponentModel;
global using System.Diagnostics;
global using System.Globalization;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using System.Text;
global using System.Threading.Tasks;
global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Input;
global using System.Windows.Navigation;
global using CsvHelper;
global using CsvHelper.Configuration;
global using Microsoft.Win32;
global using NLog;
global using NLog.Targets;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
#endregion using directives

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
        // Change the log file filename when debugging
        string env = Debugger.IsAttached ? "debug" : "temp";
        GlobalDiagnosticsContext.Set("TempOrDebug", env);

        // Unhandled exception handler
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        // Put the version number in the title bar
        Title = $"TimVer - {AppInfo.TitleVersion}";

        // Startup message in the temp file
        log.Info($"{AppInfo.AppName} {AppInfo.TitleVersion} ({AppInfo.AppVersion}) is starting up.");

        // NLog logging level
        LogManager.Configuration.Variables["logLev"] = UserSettings.Setting.IncludeDebug ? "Debug" : "Info";
        LogManager.ReconfigExistingLoggers();

        // .NET version, app framework and OS platform
        string version = Environment.Version.ToString();
        log.Debug($".NET version: {AppInfo.RuntimeVersion.Replace(".NET", "")}  ({version})");
        log.Debug(AppInfo.Framework);
        log.Debug(AppInfo.OsPlatform);

        // Window position
        Top = UserSettings.Setting.WindowTop;
        Left = UserSettings.Setting.WindowLeft;
        Height = UserSettings.Setting.WindowHeight;
        Width = UserSettings.Setting.WindowWidth;
        Topmost = UserSettings.Setting.KeepOnTop;

        // Light or dark
        SetBaseTheme(UserSettings.Setting.DarkMode);

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

    #region Set initial view
    private void SetInitialView(int page)
    {
        Stopwatch sw;
        switch (page)
        {
            case 0:
                sw = Stopwatch.StartNew();
                tabWinInfo.Content = new Page1();
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

    #region Setting change
    private void UserSettingChanged(object sender, PropertyChangedEventArgs e)
    {
        PropertyInfo prop = sender.GetType().GetProperty(e.PropertyName);
        var newValue = prop?.GetValue(sender, null);
        switch (e.PropertyName)
        {
            case "KeepOnTop":
                Topmost = (bool)newValue;
                break;

            case "IncludeDebug":
                if ((bool)newValue)
                {
                    LogManager.Configuration.Variables["logLev"] = "Debug";
                }
                else
                {
                    LogManager.Configuration.Variables["logLev"] = "Info";
                }
                LogManager.ReconfigExistingLoggers();
                break;

            case "DarkMode":
                SetBaseTheme((int)newValue);
                break;

            case "UISize":
                int size = (int)newValue;
                double newSize = UIScale(size);
                MainGrid.LayoutTransform = new ScaleTransform(newSize, newSize);
                break;
        }
        log.Debug($"Setting change: {e.PropertyName} New Value: {newValue}");
    }
    #endregion Setting change

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

    #region Window closing
    private void Window_Closing(object sender, CancelEventArgs e)
    {
        stopwatch.Stop();
        log.Info($"{AppInfo.AppName} is shutting down.  Elapsed time: {stopwatch.Elapsed:g}");

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

    private void TcMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is TabControl && IsLoaded)
        {
            if (tabWinInfo.IsSelected)
            {
                Stopwatch sw = Stopwatch.StartNew();
                tabWinInfo.Content = new Page1();
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

    #region Dialog closing
    private void DialogClosing(object sender, DialogClosingEventArgs e)
    {
        if (!(bool)e.Parameter)
        {
            return;
        }
    }
    #endregion Dialog closing
}
