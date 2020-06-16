using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace TimVer
{
    /// <summary>
    ///  Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        #region Events
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void OnNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

            string version = versionInfo.FileVersion;
            string copyright = versionInfo.LegalCopyright;
            string product = versionInfo.ProductName;

            this.tbVersion.Text = version.Remove(version.LastIndexOf("."));
            this.tbCopyright.Text = copyright.Replace("Copyright ", "");
            this.Title = $"About {product}";
            this.Topmost = true;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
            this.Close();
            _ = Process.Start(@".\ReadMe.txt");
        }
        #endregion Events
    }
}