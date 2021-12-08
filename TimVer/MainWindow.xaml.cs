// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

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
global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Input;
global using System.Windows.Navigation;
global using CsvHelper;
global using CsvHelper.Configuration;
global using Microsoft.Win32;
global using NLog;
global using NLog.Targets;
global using TKUtils;
using System.Windows.Media;
using AdonisUI;
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
        stopwatch.Start();

        UserSettings.Init(UserSettings.AppFolder, UserSettings.DefaultFilename, true);

        InitializeComponent();

        ReadSettings();

        Page3.WriteHistory();

        ProcessCommandLine();

        LoadNavigationBar(0);

        log.Debug($"Startup time: {stopwatch.ElapsedMilliseconds} ms.");
    }

    #region Settings
    private void ReadSettings()
    {
        // Change the log file filename when debugging
        string env = Debugger.IsAttached ? "debug" : "temp";
        GlobalDiagnosticsContext.Set("TempOrDebug", env);

        // Unhandled exception handler
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        // Settings change event
        UserSettings.Setting.PropertyChanged += UserSettingChanged;

        // Put the version number in the title bar
        Title = $"TimVer - {AppInfo.TitleVersion}";

        // Startup message in the temp file
        log.Info($"{AppInfo.AppName} {AppInfo.TitleVersion} is starting up.");

        // NLog logging level
        LogManager.Configuration.Variables["logLev"] = UserSettings.Setting.IncludeDebug ? "Debug" : "Info";
        LogManager.ReconfigExistingLoggers();

        // .NET version
        string runtimeVer = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription.Replace(".NET", "");
        string version = Environment.Version.ToString();
        log.Debug($".NET version: {runtimeVer}    {version}.");

        // Light or dark
        switch (UserSettings.Setting.DarkMode)
        {
            case 0:
                UseLightMode();
                break;
            case 1:
                UseDarkMode();
                break;
            case 2:
                UseSysMode();
                break;
            default:
                UseLightMode();
                break;
        }

        // Window position
        Top = UserSettings.Setting.WindowTop;
        Left = UserSettings.Setting.WindowLeft;
        Height = UserSettings.Setting.WindowHeight;
        Width = UserSettings.Setting.WindowWidth;
        Topmost = UserSettings.Setting.KeepOnTop;

        // Zoom
        double curZoom = UserSettings.Setting.SizeZoom;
        MainGrid.LayoutTransform = new ScaleTransform(curZoom, curZoom);
    }

    #endregion Settings

    #region Process command line args
    private void ProcessCommandLine()
    {
        // If count is less that two, bail out
        string[] args = Environment.GetCommandLineArgs();
        if (args.Length < 2)
            return;

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

    #region Page Navigation
    private void LoadNavigationBar(int page)
    {
        _ = lbNavigation.Items.Add("Windows Info");
        _ = lbNavigation.Items.Add("Computer Info");
        _ = lbNavigation.Items.Add("History");
        _ = lbNavigation.Items.Add("Options");
        _ = lbNavigation.Items.Add("About");
        lbNavigation.SelectedIndex = page;
    }

    private void LbNavigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (lbNavigation.SelectedIndex)
        {
            case 0:
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    Stopwatch sw = Stopwatch.StartNew();
                    MainFrame.Content = new Page1();
                    sw.Stop();
                    log.Debug($"Time to load Windows info: {sw.ElapsedMilliseconds} ms.");
                    Mouse.OverrideCursor = null;
                }
                break;
            case 1:
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    Stopwatch sw = Stopwatch.StartNew();
                    MainFrame.Content = new Page2();
                    sw.Stop();
                    log.Debug($"Time to load Computer info: {sw.ElapsedMilliseconds} ms.");
                    Mouse.OverrideCursor = null;
                }
                break;
            case 2:
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    Stopwatch sw = Stopwatch.StartNew();
                    MainFrame.Content = new Page3();
                    sw.Stop();
                    log.Debug($"Time to load History: {sw.ElapsedMilliseconds} ms.");
                    Mouse.OverrideCursor = null;
                }
                break;
            case 3:
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    MainFrame.Content = new Page4();
                    sw.Stop();
                    log.Debug($"Time to load Options: {sw.ElapsedMilliseconds} ms.");
                }
                break;
            case 4:
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    MainFrame.Content = new Page5();
                    sw.Stop();
                    log.Debug($"Time to load About: {sw.ElapsedMilliseconds} ms.");
                }
                break;
            default:
                break;
        }
    }
    #endregion Page Navigation

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
                switch (newValue)
                {
                    case 0:
                        UseLightMode();
                        break;
                    case 1:
                        UseDarkMode();
                        break;
                    case 2:
                        UseSysMode();
                        break;
                    default:
                        UseLightMode();
                        break;
                }
                break;
            case "SizeZoom":
                double curZoom = (double)newValue;
                MainGrid.LayoutTransform = new ScaleTransform(curZoom, curZoom);
                break;
        }
        log.Debug($"Setting change: {e.PropertyName} New Value: {newValue}");
    }
    #endregion Setting change

    #region Light/Dark/System mode
    private static void UseDarkMode()
    {
        ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.DarkColorScheme);
    }

    private static void UseLightMode()
    {
        ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.LightColorScheme);
    }

    private static void UseSysMode()
    {
        const string regpath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        using RegistryKey key = Registry.CurrentUser.OpenSubKey(regpath, true);
        object darkPref = key.GetValue("AppsUseLightTheme");
        if (darkPref != null)
        {
            if (darkPref.ToString() == "0")
            {
                UseDarkMode();
            }
            else
            {
                UseLightMode();
            }
        }
    }
    #endregion Light/Dark/System mode

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
        double curZoom = UserSettings.Setting.SizeZoom;
        if (curZoom > 0.85)
        {
            curZoom -= .05;
            UserSettings.Setting.SizeZoom = Math.Round(curZoom, 2);
        }

        MainGrid.LayoutTransform = new ScaleTransform(curZoom, curZoom);
    }

    public void EverythingLarger()
    {
        double curZoom = UserSettings.Setting.SizeZoom;
        if (curZoom < 1.05)
        {
            curZoom += .05;
            UserSettings.Setting.SizeZoom = Math.Round(curZoom, 2);
        }

        MainGrid.LayoutTransform = new ScaleTransform(curZoom, curZoom);
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
        UserSettings.Setting.WindowLeft = Left;
        UserSettings.Setting.WindowTop = Top;
        UserSettings.Setting.WindowWidth = Width;
        UserSettings.Setting.WindowHeight = Height;
        UserSettings.SaveSettings();
    }
    #endregion Window closing
}
