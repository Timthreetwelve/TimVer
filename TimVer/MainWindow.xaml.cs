// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region using directives
global using Microsoft.Win32;
global using System;
global using System.ComponentModel;
global using System.Diagnostics;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using System.Text;
global using System.Windows;
global using System.Windows.Controls;
global using NLog;
global using NLog.Targets;
global using TKUtils;
global using System.Windows.Navigation;
global using System.Collections.Generic;
global using System.Globalization;
global using CsvHelper;
global using CsvHelper.Configuration;
global using System.Windows.Input;
#endregion using directives

namespace TimVer;

public partial class MainWindow : Window
{
    #region NLog Instance
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
    #endregion NLog Instance

    public MainWindow()
    {
        UserSettings.Init(UserSettings.AppFolder, UserSettings.DefaultFilename, true);

        InitializeComponent();

        ReadSettings();

        MainFrame.Content = new Page1();

        Page3.WriteHistory();

        ProcessCommandLine();
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

        // Window position
        Top = UserSettings.Setting.WindowTop;
        Left = UserSettings.Setting.WindowLeft;
        Height = UserSettings.Setting.WindowHeight;
        Width = UserSettings.Setting.WindowWidth;
        Topmost = UserSettings.Setting.KeepOnTop;
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
    private void BtnClick_Page1(object sender, RoutedEventArgs e)
    {
        Mouse.OverrideCursor = Cursors.Wait;
        Stopwatch sw = Stopwatch.StartNew();
        MainFrame.Content = new Page1();
        sw.Stop();
        log.Debug($"Time to load Windows info: {sw.ElapsedMilliseconds} ms.");
        Mouse.OverrideCursor = null;
    }
    private void BtnClick_Page2(object sender, RoutedEventArgs e)
    {
        Mouse.OverrideCursor = Cursors.Wait;
        Stopwatch sw = Stopwatch.StartNew();
        MainFrame.Content = new Page2();
        sw.Stop();
        log.Debug($"Time to load Computer info: {sw.ElapsedMilliseconds} ms.");
        Mouse.OverrideCursor = null;
    }
    private void BtnClick_Page3(object sender, RoutedEventArgs e)
    {
        Mouse.OverrideCursor = Cursors.Wait;
        Stopwatch sw = Stopwatch.StartNew();
        MainFrame.Content = new Page3();
        sw.Stop();
        log.Debug($"Time to load History: {sw.ElapsedMilliseconds} ms.");
        Mouse.OverrideCursor = null;
    }
    private void BtnClick_Page4(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new Page4();
    }
    private void BtnClick_Page5(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new Page5();
    }
    #endregion Page Navigation

    #region Helper Methods

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
        }
        log.Debug($"Setting change: {e.PropertyName} New Value: {newValue}");
    }
    #endregion Setting change

    #endregion Helper Methods

    #region Window closing
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        // Shut down NLog
        log.Info("TimVer is shutting down.");
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
