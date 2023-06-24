// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Views;
/// <summary>
/// Interaction logic for DrivesPage.xaml
/// </summary>
public partial class DrivesPage : UserControl
{
    public static DrivesPage Instance { get; set; }
    public DrivesPage()
    {
        InitializeComponent();
        Instance = this;
        TabControl1.SelectedIndex = TempSettings.Setting.DriveSelectedTab;
    }

    #region Set active tab
    private void TabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TempSettings.Setting.DriveSelectedTab = TabControl1.SelectedIndex;
    }
    #endregion Set active tab
}
