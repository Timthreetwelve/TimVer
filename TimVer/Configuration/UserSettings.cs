// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Configuration;

[INotifyPropertyChanged]
public partial class UserSettings : ConfigManager<UserSettings>
{
    #region Properties (some with default values)

    [ObservableProperty]
    private bool _includeDebug = true;

    [ObservableProperty]
    private bool _includeNetwork;

    [ObservableProperty]
    private bool _includeNotReady;

    [ObservableProperty]
    private bool _includeRemovable;

    [ObservableProperty]
    private NavPage _initialPage = NavPage.WindowsInfo;

    [ObservableProperty]
    private bool _keepHistory = true;

    [ObservableProperty]
    private bool _keepOnTop;

    [ObservableProperty]
    private AccentColor _primaryColor = AccentColor.Blue;

    [ObservableProperty]
    private Spacing _rowSpacing = Spacing.Comfortable;

    [ObservableProperty]
    private bool _showBusType = true;

    [ObservableProperty]
    private bool _showDiskType = true;

    [ObservableProperty]
    private bool _showDrives = true;

    [ObservableProperty]
    private bool _showFormat = true;

    [ObservableProperty]
    private bool _showHealth = true;

    [ObservableProperty]
    private bool _showInterface = true;

    [ObservableProperty]
    private bool _showMediaType = true;

    [ObservableProperty]
    private bool _showModel = true;

    [ObservableProperty]
    private bool _showName = true;

    [ObservableProperty]
    private bool _showPartitions = true;

    [ObservableProperty]
    private bool _showTypeL = true;

    [ObservableProperty]
    private bool _showUser = true;

    [ObservableProperty]
    private bool _startCentered = true;

    [ObservableProperty]
    private MySize _uISize = MySize.Default;

    [ObservableProperty]
    private ThemeType _uITheme = ThemeType.System;

    [ObservableProperty]
    private bool _use1024;

    [ObservableProperty]
    private double _windowHeight = 500;

    [ObservableProperty]
    private double _windowLeft = 100;

    [ObservableProperty]
    private double _windowTop = 100;

    [ObservableProperty]
    private double _windowWidth = 750;
    #endregion Properties (some with default values)
}
