// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region using directives
using Microsoft.Win32;
using NLog;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TimVer.ViewModels;
using TKUtils;
#endregion using directives

namespace TimVer
{
    public partial class MainWindow : Window
    {
        #region NLog Instance
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        #endregion NLog Instance

        private readonly MySettings settings = MySettings.Read();

        public MainWindow()
        {
            InitializeComponent();

            ReadSettings();

            DataContext = new Page1ViewModel();

            HistoryViewModel.WriteHistory();

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

            // Startup message in the temp file
            log.Info($"{AppInfo.AppName} {AppInfo.TitleVersion} is starting up.");

            Top = settings.WindowTop;
            Left = settings.WindowLeft;

            double curZoom = settings.Zoom;
            if (curZoom < 0.75)
            {
                curZoom = 0.75;
            }
            Grid1.LayoutTransform = new ScaleTransform(curZoom, curZoom);

            WindowTitleVersion();

            CheckRegEntry();
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

        #region Menu Events
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            CopyToClipboard();
        }

        private void AddRegistry(object sender, RoutedEventArgs e)
        {
            AddRegEntry();
        }

        private void RemoveResgistry(object sender, RoutedEventArgs e)
        {
            RemoveRegEntry();
        }

        private void Page1_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new Page1ViewModel();
        }

        private void Page2_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new Page2ViewModel();
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new HistoryViewModel();
        }
        private void GridSmaller_Click(object sender, RoutedEventArgs e)
        {
            ZoomSmaller();
        }

        private void GridLarger_Click(object sender, RoutedEventArgs e)
        {
            ZoomLarger();
        }

        private void GridReset_Click(object sender, RoutedEventArgs e)
        {
            ZoomReset();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            About about = new About
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            _ = about.ShowDialog();
        }

        private void ReadMe_Click(object sender, RoutedEventArgs e)
        {
            string dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            TextFileViewer.ViewTextFile(Path.Combine(dir, "ReadMe.txt"));
        }
        #endregion Menu Events

        #region Keyboard Events
        private void Window_Keydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D1 && (e.KeyboardDevice.Modifiers == ModifierKeys.Control))
            {
                DataContext = new Page1ViewModel();
            }

            if (e.Key == Key.D2 && (e.KeyboardDevice.Modifiers == ModifierKeys.Control))
            {
                DataContext = new Page2ViewModel();
            }

            if (e.Key == Key.D3 && (e.KeyboardDevice.Modifiers == ModifierKeys.Control))
            {
                DataContext = new HistoryViewModel();
            }

            if (e.Key == Key.C && (e.KeyboardDevice.Modifiers == ModifierKeys.Control))
            {
                CopyToClipboard();
            }

            if (e.Key == Key.NumPad0)
            {
                ZoomReset();
            }

            if (e.Key == Key.Add)
            {
                ZoomLarger();
            }

            if (e.Key == Key.Subtract)
            {
                ZoomSmaller();
            }
        }
        #endregion Keyboard Events

        #region Mouse Wheel Events
        private void Grid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            if (e.Delta > 0)
            {
                ZoomLarger();
            }
            else if (e.Delta < 0)
            {
                ZoomSmaller();
            }
            e.Handled = true;
        }
        #endregion Mouse Wheel Events

        #region Window Events
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // save the settings
            settings.WindowLeft = Left;
            settings.WindowTop = Top;
            MySettings.Save(settings);

            // Shut down NLog
            log.Info("TimVer is shutting down.");
            LogManager.Shutdown();
        }
        #endregion

        #region Helper methods
        private void CopyToClipboard()
        {
            Page1ViewModel page1 = new Page1ViewModel();
            Page2ViewModel page2 = new Page2ViewModel();

            StringBuilder builder = new StringBuilder();
            _ = builder.Append("Product Name  = ").AppendLine(page1.ProdName);
            _ = builder.Append("Version       = ").AppendLine(page1.Version);
            _ = builder.Append("Build         = ").AppendLine(page1.Build);
            _ = builder.Append("Architecture  = ").AppendLine(page1.Arch);
            _ = builder.Append("Installed on  = ").AppendLine(page1.InstallDate);
            _ = builder.Append("Build Branch  = ").AppendLine(page1.BuildBranch);
            _ = builder.Append("Machine Name  = ").AppendLine(page2.MachName);
            _ = builder.Append("Last Reboot   = ").AppendLine(page2.LastBoot);
            _ = builder.Append("Boot Device   = ").AppendLine(page2.BootDevice);
            _ = builder.Append("System Device = ").AppendLine(page2.SystemDevice);
            _ = builder.Append("Manufacturer  = ").AppendLine(page2.Manufacturer);
            _ = builder.Append("Model         = ").AppendLine(page2.Model);
            Clipboard.SetText(builder.ToString());
        }

        public void WindowTitleVersion()
        {
            // Get the assembly version
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            // Remove the release (last) node
            string titleVer = version.ToString().Remove(version.ToString().LastIndexOf("."));

            // Set the windows title
            Title = $"Tim's Winver - {titleVer}";
        }
        #endregion Helper methods

        #region Registry methods
        public void CheckRegEntry()
        {
            const string regpath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(regpath, true))
            {
                var timver = key.GetValue("TimVer");
                if (timver != null)
                {
                    mnuAddReg.IsChecked = true;
                    log.Debug($"Registry key HKCU\\{regpath}\\TimVer was found.");
                }
                else
                {
                    log.Debug($"Registry key HKCU\\{regpath}\\TimVer was not found.");
                    mnuAddReg.IsChecked = false;
                }
            }
        }

        public void AddRegEntry()
        {
            if (IsLoaded)
            {
                const string regpath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                string loc = AppInfo.AppPath;
                Debug.WriteLine("AddRegEntry fired");
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(regpath, true))
                    {
                        {
                            key.SetValue("TimVer", loc + " /hide");
                            log.Info($"Registry key HKCU\\{regpath}\\Timver has been added.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex, $"Unsuccessful attempt to update Registry.");
                    _ = MessageBox.Show($"Unsuccessful attempt to update Registry.\n{ex}",
                        "TimVer Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        public void RemoveRegEntry()
        {
            if (IsLoaded)
            {
                const string regpath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(regpath, true))
                    {
                        key.DeleteValue("TimVer", false);
                        log.Info($"Registry key HKCU\\{regpath}\\Timver has been removed.");
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex, $"Unsuccessful attempt to update Registry.");
                    _ = MessageBox.Show($"Unsuccessful attempt to update Registry.\n{ex}",
                                        "TimVer Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                }
            }
        } 
        #endregion Registry methods

        #region Zoom
        private void ZoomReset()
        {
            settings.Zoom = 1.0;
            Grid1.LayoutTransform = new ScaleTransform(1, 1);
        }

        private void ZoomSmaller()
        {
            double curZoom = settings.Zoom;
            if (curZoom > 0.75)
            {
                curZoom -= .050;
                settings.Zoom = Math.Round(curZoom, 2);
            }
            Grid1.LayoutTransform = new ScaleTransform(curZoom, curZoom);
        }

        private void ZoomLarger()
        {
            double curZoom = settings.Zoom;
            if (curZoom < 2.0)
            {
                curZoom += .05;
                settings.Zoom = Math.Round(curZoom, 2);
            }
            Grid1.LayoutTransform = new ScaleTransform(curZoom, curZoom);
        }
        #endregion Zoom

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
    }
}