﻿// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Configuration;

[INotifyPropertyChanged]
public partial class UserSettings : ConfigManager<UserSettings>
{
    #region Properties (some with default values)
    [ObservableProperty]
    private double _detailsHeight = 350;

    [ObservableProperty]
    private bool _getPhysicalDrives = false;

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
    private bool _languageTesting;

    [ObservableProperty]
    private AccentColor _primaryColor = AccentColor.Blue;

    [ObservableProperty]
    private Spacing _rowSpacing = Spacing.Comfortable;

    [ObservableProperty]
    private bool _showBootDrive;

    [ObservableProperty]
    private bool _showBusType;

    [ObservableProperty]
    private bool _showDiskType = true;

    [ObservableProperty]
    private bool _showExitInNav = true;

    [ObservableProperty]
    private bool _showFormat = true;

    [ObservableProperty]
    private bool _showFriendlyName = true;

    [ObservableProperty]
    private bool _showHealth;

    [ObservableProperty]
    private bool _showInterface;

    [ObservableProperty]
    private bool _showMediaType = true;

    [ObservableProperty]
    private bool _showModel = true;

    [ObservableProperty]
    private bool _showName;

    [ObservableProperty]
    private bool _showPartitions;

    [ObservableProperty]
    private bool _showPartitionStyle;

    [ObservableProperty]
    private bool _showSerialNumber;

    [ObservableProperty]
    private bool _showTypeL = true;

    [ObservableProperty]
    private bool _showUser = true;

    [ObservableProperty]
    private bool _showVideoSelector = true;

    [ObservableProperty]
    private bool _startCentered = true;

    [ObservableProperty]
    private string _uILanguage = "en-US";

    [ObservableProperty]
    private MySize _uISize = MySize.Default;

    [ObservableProperty]
    private ThemeType _uITheme = ThemeType.System;

    [ObservableProperty]
    private bool _use1024;

    [ObservableProperty]
    private bool _useOSLanguage;

    [ObservableProperty]
    private double _windowHeight = 700;

    [ObservableProperty]
    private double _windowLeft = 100;

    [ObservableProperty]
    private double _windowTop = 100;

    [ObservableProperty]
    private double _windowWidth = 1000;
    #endregion Properties (some with default values)
}
