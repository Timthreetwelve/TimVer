#region using directives
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
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

            if (e.Key == Key.D1 && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                DataContext = new Page1ViewModel();
            }

            if (e.Key == Key.D2 && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                DataContext = new Page2ViewModel();
            }

            if (e.Key == Key.D3 && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                DataContext = new PrevInfoViewModel();
            }

            if (e.Key == Key.C && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                CopyToClipboard();
            }

            if (e.Key == Key.Add && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && FontSize < 17)
            {
                FontSize += 1;
                Properties.Settings.Default.FontSize = FontSize;
            }
            if (e.Key == Key.Subtract && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && FontSize > 12)
            {
                FontSize -= 1;
                Properties.Settings.Default.FontSize = FontSize;
            }
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

            WindowTitleVersion();
        }

        private void CopyToClipboard()
        {
            Page1ViewModel page1 = new Page1ViewModel();
            Page2ViewModel page2 = new Page2ViewModel();

            StringBuilder builder = new StringBuilder();
            _ = builder.AppendLine($"Product Name  = {page1.ProdName}");
            _ = builder.AppendLine($"Version       = {page1.Version}");
            _ = builder.AppendLine($"Build         = {page1.Build}");
            _ = builder.AppendLine($"Architecture  = {page1.Arch}");
            _ = builder.AppendLine($"Installed on  = {page1.InstallDate}");
            _ = builder.AppendLine($"Build Branch  = {page1.BuildBranch}");
            _ = builder.AppendLine($"Machine Name  = {page2.MachName}");
            _ = builder.AppendLine($"Last Reboot   = {page2.LastBoot}");
            _ = builder.AppendLine($"Boot Device   = {page2.BootDevice}");
            _ = builder.AppendLine($"System Device = {page2.SystemDevice}");
            _ = builder.AppendLine($"Manufacturer  = {page2.Manufacturer}");
            _ = builder.AppendLine($"Model         = {page2.Model}");
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
        #endregion


    }
}