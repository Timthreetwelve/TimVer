// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region using directives
using NLog;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using TKUtils;
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

        log.Debug(InfoVM.DiskDrives);
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

        // .NET version
        string runtimeVer = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription.Replace(".NET","");
        string version = Environment.Version.ToString();
        log.Info($".NET version: {runtimeVer}    {version}.");

        // Window position
        Top = UserSettings.Setting.WindowTop;
        Left = UserSettings.Setting.WindowLeft;
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
        MainFrame.Content = new Page1();
    }
    private void BtnClick_Page2(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new Page2();
    }
    private void BtnClick_Page3(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new Page3();
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
        }
        log.Debug($"***Setting change: {e.PropertyName} New Value: {newValue}");
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
        UserSettings.SaveSettings();
    }
    #endregion Window closing
}
