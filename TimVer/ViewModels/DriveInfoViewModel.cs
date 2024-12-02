// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

/// <summary>
/// Logical drives collection
/// </summary>
internal sealed partial class DriveInfoViewModel : ObservableObject
{
    #region Logical disk drives
    private static ObservableCollection<LogicalDrives>? _logicalDrivesList;
    public static ObservableCollection<LogicalDrives> LogicalDrivesList
    {
        get
        {
            if (_logicalDrivesList?.Count > 0)
            {
                return _logicalDrivesList;
            }
            _logicalDrivesList = new ObservableCollection<LogicalDrives>(DiskDriveHelpers.GetLogicalDriveInfo()!);
            return _logicalDrivesList;
        }
    }
    #endregion Logical disk drives

    #region Physical disk drives
    /// <summary>
    /// Physical drives collection
    /// </summary>
    private static ObservableCollection<PhysicalDrives>? _physicalDrivesList;
    public static ObservableCollection<PhysicalDrives> PhysicalDrivesList
    {
        get
        {
            if (_physicalDrivesList?.Count > 0)
            {
                return _physicalDrivesList;
            }
            _physicalDrivesList = new ObservableCollection<PhysicalDrives>(DiskDriveHelpers.GetPhysicalDriveInfo());
            return _physicalDrivesList;
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
