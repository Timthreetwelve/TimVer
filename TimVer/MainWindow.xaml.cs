#region using directives
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TimVer.ViewModels;
#endregion

namespace TimVer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ReadSettings();

            DataContext = new Page1ViewModel();
        }

        #region Events
        // Menu events
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            CopyToClipboard();
        }

        private void Page1_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new Page1ViewModel();
        }

        private void Page2_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new Page2ViewModel();
        }

        private void Page3_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PrevInfoViewModel();
        }

        private void GridSmaller_Click(object sender, RoutedEventArgs e)
        {
            GridSmaller();
        }

        private void GridLarger_Click(object sender, RoutedEventArgs e)
        {
            GridLarger();
        }

        private void GridReset_Click(object sender, RoutedEventArgs e)
        {
            GridSizeReset();
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
            TextFileViewer.ViewTextFile(@".\ReadMe.txt");
        }

        // Keyboard events
        private void Window_Keydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }

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
                DataContext = new PrevInfoViewModel();
            }

            if (e.Key == Key.C && (e.KeyboardDevice.Modifiers == ModifierKeys.Control))
            {
                CopyToClipboard();
            }

            if (e.Key == Key.NumPad0)
            {
                GridSizeReset();
            }

            if (e.Key == Key.Add)
            {
                GridLarger();
            }

            if (e.Key == Key.Subtract)
            {
                GridSmaller();
            }
        }

        // Mouse wheel
        private void Grid1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
            {
                if (Keyboard.Modifiers != ModifierKeys.Control)
                    return;

                if (e.Delta > 0)
                {
                    GridLarger();
                }
                else if (e.Delta < 0)
                {
                    GridSmaller();
                }
                e.Handled = true;
        }

        // Window events
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Page1ViewModel page1 = new Page1ViewModel();

            // save the property settings
            Properties.Settings.Default.PrevBuild = page1.Build;
            Properties.Settings.Default.PrevBranch= page1.BuildBranch;
            Properties.Settings.Default.PrevVersion = page1.Version;
            Properties.Settings.Default.LastRun = DateTime.Now;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region Helper methods
        private void ReadSettings()
        {
            if (Properties.Settings.Default.SettingsUpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.SettingsUpgradeRequired = false;
                Properties.Settings.Default.Save();
                // CleanupPrevSettings must be called AFTER settings Upgrade and Save
                CleanUp.CleanupPrevSettings();
                Debug.WriteLine("*** SettingsUpgradeRequired");
            }

            // Set grid zoom
            double curZoom = Properties.Settings.Default.Zoom;
            if (curZoom < 0.5)
            {
                curZoom = 0.5;
            }
            Grid1.LayoutTransform = new ScaleTransform(curZoom, curZoom);

            WindowTitleVersion();
        }

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
            Title = $"Tim's Winver - v{titleVer}";
        }

        private void GridSizeReset()
        {
            Properties.Settings.Default.Zoom = 1.0;
            Grid1.LayoutTransform = new ScaleTransform(1, 1);
        }

        private void GridSmaller()
        {
            double curZoom = Properties.Settings.Default.Zoom;
            if (curZoom > 0.75)
            {
                curZoom -= .05;
                Properties.Settings.Default.Zoom = Math.Round(curZoom, 2);
            }
            Grid1.LayoutTransform = new ScaleTransform(curZoom, curZoom);
        }

        private void GridLarger()
        {
            double curZoom = Properties.Settings.Default.Zoom;
            if (curZoom < 2.0)
            {
                curZoom += .05;
                Properties.Settings.Default.Zoom = Math.Round(curZoom, 2);
            }
            Grid1.LayoutTransform = new ScaleTransform(curZoom, curZoom);
        }
        #endregion Helper methods
    }
}