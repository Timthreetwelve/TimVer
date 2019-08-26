using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Management;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Media;


namespace TimVer
{
    /// <summary>
    /// Todo: Comments
    /// </summary>
    public partial class MainWindow : Window
    {
        private string relID;
        private string bldUbr;
        private string prodName;
        private string osArch;
        private string instDate;
        private string lastBoot;
        private string bldBrnch;
        private string regOwn;
        private string bootDev;
        private string systemDev;
        private string edID;
        private string machineName;
        private string curBld;
        private string ubr;
        private readonly string bullet = " \u25AA ";


        public MainWindow()
        {
            UpgradeSettings();

            GetRegistryInfo();

            GetWmiInfo();

            GetEnvironment();

            InitializeComponent();

            txtblkProduct.Text = prodName;
            txtblkVersion.Text = relID;
            txtblkBuild.Text = bldUbr;
            txtblkOsArch.Text = osArch;
            txtblkInstall.Text = instDate;
            txtblkReboot.Text = lastBoot;
            txtblkRegOwn.Text = regOwn;
            txtblkBldBranch.Text = bldBrnch;
            txtblkEditionID.Text = edID;
            txtblkBootDev.Text = bootDev;
            txtblkSystemDev.Text = systemDev;
            txtblkMachineName.Text = machineName;

            WindowTitleVersion();
        }

        public void GetRegistryInfo()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion"))
                {
                    if (key != null)
                    {
                        relID = key.GetValue("ReleaseID").ToString();
                        curBld = key.GetValue("CurrentBuild").ToString();
                        ubr = key.GetValue("UBR").ToString();
                        bldUbr = string.Format($"{curBld}{bullet}{ubr}");
                        prodName = key.GetValue("ProductName").ToString();
                        bldBrnch = key.GetValue("BuildBranch").ToString();
                        //bldLab = key.GetValue("BuildLab").ToString();
                        regOwn = key.GetValue("RegisteredOwner").ToString();
                        edID = key.GetValue("EditionID").ToString();
                    }
                }
            }
            catch (Exception)
            {
                //react appropriately
            }
        }

        public void GetEnvironment()
        {
            machineName = Environment.MachineName;
        }

        public void GetWmiInfo()
        {
            try
            {
                ManagementObjectSearcher objOSDetails = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectCollection osDetailsCollection = objOSDetails.Get();

                foreach (ManagementObject mo in osDetailsCollection)
                {
                    DateTime id = System.Management.ManagementDateTimeConverter.ToDateTime(mo.GetPropertyValue("InstallDate").ToString());
                    instDate = id.ToString();
                    DateTime lb = System.Management.ManagementDateTimeConverter.ToDateTime(mo.GetPropertyValue("LastBootUpTime").ToString());
                    lastBoot = lb.ToString();
                    osArch = mo.GetPropertyValue("OSArchitecture").ToString();

                    bootDev = mo.GetPropertyValue("BootDevice").ToString();
                    systemDev = mo.GetPropertyValue("SystemDevice").ToString();
                }

                objOSDetails.Dispose();
            }
            catch (Exception)
            {
                //react appropriately   
            }

        }

        /// <summary>
        /// Add version number to the window title
        /// </summary>
        public void WindowTitleVersion()
        {
            // Get the assembly version
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            // Remove the release (last) node
            string titleVer = version.ToString().Remove(version.ToString().LastIndexOf("."));

            // Set the windows title
            this.Title = "Tim's Winver - v" + titleVer;
        }

        /// <summary>
        /// Press Escape to exit
        /// </summary>
        private void Window_Keydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }

            if (e.Key == Key.C && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                CopyToClipboard();
            }
        }
        /// <summary>
        /// Exit when Exit button clicked
        /// </summary>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            CopyToClipboard();
        }

        // Save the window position at shutdown so that it will start in the same location
        // Triggered by  Closing="Window_Closing"  in the MainWindow.xaml
        // WindowTop and WindowLeft are saved in user.config
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // save the property settings
            Properties.Settings.Default.LastRun = DateTime.Now;
            Properties.Settings.Default.Save();
        }

        private void UpgradeSettings()
        {
            if (Properties.Settings.Default.SettingsUpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.SettingsUpgradeRequired = false;
                Properties.Settings.Default.Save();
            }
        }

        private void CopyToClipboard()
        {
            
            string equals = " = ";
            string newline = Environment.NewLine;

            StringBuilder builder = new StringBuilder();
            builder.Append("Product Name");
            builder.Append(equals);
            builder.Append(prodName);
            builder.Append(newline);
            builder.Append("Version");
            builder.Append(equals);
            builder.Append(relID);
            builder.Append(newline);
            builder.Append("Build");
            builder.Append(equals);
            builder.Append(curBld + "." + ubr);
            builder.Append(newline);
            builder.Append("Architecture");
            builder.Append(equals);
            builder.Append(osArch);
            builder.Append(newline);
            builder.Append("Installed on");
            builder.Append(equals);
            builder.Append(instDate);
            builder.Append(newline);
            builder.Append("Last Reboot");
            builder.Append(equals);
            builder.Append(lastBoot);
            builder.Append(newline);
            builder.Append("Build Branch");
            builder.Append(equals);
            builder.Append(bldBrnch);
            builder.Append(newline);
            builder.Append("Edition ID");
            builder.Append(equals);
            builder.Append(edID);
            builder.Append(newline);
            builder.Append("Boot Device");
            builder.Append(equals);
            builder.Append(bootDev);
            builder.Append(newline);
            builder.Append("System Device");
            builder.Append(equals);
            builder.Append(systemDev);
            builder.Append(newline);
            builder.Append("Machine Name");
            builder.Append(equals);
            builder.Append(machineName);
            builder.Append(newline);
            builder.Append("Registered to");
            builder.Append(equals);
            builder.Append(regOwn);
            builder.Append(newline);
            Clipboard.SetText(builder.ToString());

            if(Properties.Settings.Default.BeepOnCopy)
            {
                //Console.Beep(1000, 50);
                try
                {
                    using (SoundPlayer soundPlayer = new SoundPlayer(Properties.Resources.Pop))
                    {
                        soundPlayer.Play();
                    }
                }
                catch (Exception) { }
            }
        }
    }
}