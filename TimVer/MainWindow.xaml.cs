using Microsoft.Win32;
using System;
using System.Management;
using System.Windows;


namespace TimVer
{
    public partial class MainWindow : Window
    {
        private string relID;
        private string bldUbr;
        private string prodName;
        private string osArch;
        private string instDate;
        private string lastBoot;
        private string bldBrnch;
        private string bullet = " \u25AA ";

        public MainWindow()
        {
            
            GetRegistryInfo();
            GetWmiInfo();
            InitializeComponent();

            txtblkProduct.Text = prodName;
            txtblkVersion.Text = relID;
            txtblkBuild.Text = bldUbr;
            txtblkOsArch.Text = osArch;
            txtblkInstall.Text = instDate;
            txtblkReboot.Text = lastBoot;
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
                        string curBld = key.GetValue("CurrentBuild").ToString();
                        string ubr = key.GetValue("UBR").ToString();
                        bldUbr = string.Format($"{curBld}{bullet}{ubr}");
                        prodName = key.GetValue("ProductName").ToString();
                        //regOwn = key.GetValue("RegisteredOwner").ToString();
                        bldBrnch = key.GetValue("BuildLab").ToString();
                    }
                }
            }
            catch (Exception)
            {
                //react appropriately
            }
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
                    DateTime lb = System.Management.ManagementDateTimeConverter.ToDateTime(mo.GetPropertyValue("LastBootUpTime").ToString());
                    osArch = mo.GetPropertyValue("OSArchitecture").ToString();
                    instDate = id.ToString();
                    lastBoot = lb.ToString();
                }
            }
            catch (Exception)
            {
                //react appropriately   OSArchitecture
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}