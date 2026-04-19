// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

/// <summary>
/// Logical drives collection
/// </summary>
internal sealed partial class DriveInfoViewModel : ObservableObject
{
    #region Logical disk drives
    public static ObservableCollection<LogicalDrives> LogicalDrivesList
    {
        get
        {
            if (field?.Count > 0)
            {
                return field;
            }
            field = new ObservableCollection<LogicalDrives>(DiskDriveHelpers.GetLogicalDriveInfo()!);
            return field;
        }
    }
    #endregion Logical disk drives

    #region Physical disk drives
    /// <summary>
    /// Physical drives collection
    /// </summary>
    public static ObservableCollection<PhysicalDrives> PhysicalDrivesList
    {
        get
        {
            if (field?.Count > 0)
            {
                return field;
            }
            field = new ObservableCollection<PhysicalDrives>(DiskDriveHelpers.GetPhysicalDriveInfo());
            return field;
        }
    }
    #endregion Physical disk drives

    #region Refresh drives command
    /// <summary>
    /// Relay command for freshing drives collections
    /// </summary>
    [RelayCommand]
    private static void RefreshDrives()
    {
        LogicalDrivesList.Clear();
        DrivesPage.Instance!.LDrivesDataGrid.ItemsSource = LogicalDrivesList;

        if (UserSettings.Setting!.GetPhysicalDrives)
        {
            PhysicalDrivesList.Clear();
            DrivesPage.Instance.PDisksDataGrid.ItemsSource = PhysicalDrivesList;
            DrivesPage.Instance.PDisksDataGrid.SelectedIndex = 0;
        }
        SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_DriveInfoRefreshed"));
    }
    #endregion Refresh drives command
}
