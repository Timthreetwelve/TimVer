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
        if (TabControl1.SelectedIndex != -1)
        {
            TempSettings.Setting.DriveSelectedTab = TabControl1.SelectedIndex;
        }
    }
    #endregion Set active tab

    #region Grid splitter drag completed event
    private void DetailsSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
    {
        UserSettings.Setting.DetailsHeight = Math.Floor(DetailsRow.Height.Value);
    }
    #endregion Grid splitter drag completed event
}
