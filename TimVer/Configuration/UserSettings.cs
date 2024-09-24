// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Configuration;

[INotifyPropertyChanged]
public partial class UserSettings : ConfigManager<UserSettings>
{
    #region Properties (some with default values)
    /// <summary>
    /// Height of the details pane.
    /// </summary>
    [ObservableProperty]
    private double _detailsHeight = 350;

    /// <summary>
    /// Option to gather info on physical drives. Can be slow.
    /// </summary>
    [ObservableProperty]
    private bool _getPhysicalDrives;

    /// <summary>
    ///  Used to determine if Debug level messages are included in the application log.
    /// </summary>
    [ObservableProperty]
    private bool _includeDebug = true;

    /// <summary>
    /// Option to include network drives on Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _includeNetwork;

    /// <summary>
    /// Option to include "not ready" drives on Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _includeNotReady;

    /// <summary>
    /// Option to include removable drives.
    /// </summary>
    [ObservableProperty]
    private bool _includeRemovable;

    /// <summary>
    /// The page displayed on startup.
    /// </summary>
    [ObservableProperty]
    private NavPage _initialPage = NavPage.WindowsInfo;

    /// <summary>
    /// Option to keep build history.
    /// </summary>
    [ObservableProperty]
    private bool _keepHistory = true;

    /// <summary>
    /// Keep window topmost.
    /// </summary>
    [ObservableProperty]
    private bool _keepOnTop;

    /// <summary>
    /// Enable language testing.
    /// </summary>
    [ObservableProperty]
    private bool _languageTesting;

    /// <summary>
    /// Accent color.
    /// </summary>
    [ObservableProperty]
    private AccentColor _primaryColor = AccentColor.Blue;

    /// <summary>
    /// Vertical spacing in the data grids.
    /// </summary>
    [ObservableProperty]
    private Spacing _rowSpacing = Spacing.Comfortable;

    /// <summary>
    /// Font used in datagrids.
    /// </summary>
    [ObservableProperty]
    private string? _selectedFont = "Segoe UI";

    /// <summary>
    /// Show the Boot Drive column on the Logical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showBootDrive;

    /// <summary>
    /// Show the Bus Type column on the Logical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showBusType;

    /// <summary>
    /// Show the Disk Type column on the Logical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showDiskType = true;

    /// <summary>
    /// Show the Drive Letters column on the Physical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showDriveLetters = true;

    /// <summary>
    /// Show Exit in the navigation menu.
    /// </summary>
    [ObservableProperty]
    private bool _showExitInNav = true;

    /// <summary>
    /// Show the Format column on the Logical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showFormat = true;

    /// <summary>
    /// Show the Friendly Name column on the Physical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showFriendlyName = true;

    /// <summary>
    /// Show the Health column on the Physical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showHealth;

    /// <summary>
    /// Show the Interface column on the Physical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showInterface;

    /// <summary>
    /// Show the Media Type column on the Physical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showMediaType = true;

    /// <summary>
    /// Show the Model column on the Physical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showModel = true;

    /// <summary>
    /// Show the Name column on the Physical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showName;

    /// <summary>
    /// Show the Partitions column on the Physical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showPartitions;

    /// <summary>
    /// Show the Partition Style column on the Physical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showPartitionStyle;

    /// <summary>
    /// Show the Serial Number column on the Physical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showSerialNumber;

    /// <summary>
    /// Show the Type column on the Logical Drives page.
    /// </summary>
    [ObservableProperty]
    private bool _showTypeL = true;

    /// <summary>
    /// Show the user and organization on the Windows page.
    /// </summary>
    [ObservableProperty]
    private bool _showUser = true;

    /// <summary>
    /// Show the video adapter drop-down on the Graphics page.
    /// </summary>
    [ObservableProperty]
    private bool _showVideoSelector = true;

    /// <summary>
    /// Option start with window centered on screen.
    /// </summary>
    [ObservableProperty]
    private bool _startCentered = true;

    /// <summary>
    /// Defined language to use in the UI.
    /// </summary>
    [ObservableProperty]
    private string _uILanguage = "en-US";

    /// <summary>
    /// Amount of UI zoom.
    /// </summary>
    [ObservableProperty]
    private MySize _uISize = MySize.Default;

    /// <summary>
    /// Theme type.
    /// </summary>
    [ObservableProperty]
    private ThemeType _uITheme = ThemeType.System;

    /// <summary>
    /// Use GiB (1024^3 bytes) if true; otherwise use GB (1000^3 bytes)
    /// </summary>
    [ObservableProperty]
    private bool _use1024;

    /// <summary>
    /// Use the operating system language (if one has been provided).
    /// </summary>
    [ObservableProperty]
    private bool _useOSLanguage;

    /// <summary>
    /// Height of the window.
    /// </summary>
    [ObservableProperty]
    private double _windowHeight = 700;

    /// <summary>
    /// Position of left side of the window.
    /// </summary>
    [ObservableProperty]
    private double _windowLeft = 100;

    /// <summary>
    /// Position of the top side of the window.
    /// </summary>
    [ObservableProperty]
    private double _windowTop = 100;

    /// <summary>
    /// Width of the window.
    /// </summary>
    [ObservableProperty]
    private double _windowWidth = 1000;
    #endregion Properties (some with default values)
}
