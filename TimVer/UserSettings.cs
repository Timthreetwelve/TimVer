// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

[INotifyPropertyChanged]
public partial class UserSettings : SettingsManager<UserSettings>
{
    #region Methods
    public void SaveWindowPos()
    {
        Window mainWindow = Application.Current.MainWindow;
        WindowHeight = Math.Floor(mainWindow.Height);
        WindowLeft = Math.Floor(mainWindow.Left);
        WindowTop = Math.Floor(mainWindow.Top);
        WindowWidth = Math.Floor(mainWindow.Width);
    }

    public void SetWindowPos()
    {
        Window mainWindow = Application.Current.MainWindow;
        mainWindow.Height = WindowHeight;
        mainWindow.Left = WindowLeft;
        mainWindow.Top = WindowTop;
        mainWindow.Width = WindowWidth;
    }
    #endregion Methods

    #region Properties (some with default values)
    [ObservableProperty]
    private bool _includeDebug = true;

    [ObservableProperty]
    private NavPage _initialPage = NavPage.ComputerInfo;

    [ObservableProperty]
    private bool _keepOnTop;

    [ObservableProperty]
    private AccentColor _primaryColor = AccentColor.Blue;

    [ObservableProperty]
    private bool _showUser;

    [ObservableProperty]
    private bool _showDrives;

    [ObservableProperty]
    private bool _showLabels;

    [ObservableProperty]
    private MySize _uISize = MySize.Default;

    [ObservableProperty]
    private ThemeType _uITheme = ThemeType.System;

    [ObservableProperty]
    private double _windowHeight = 450;

    [ObservableProperty]
    private double _windowLeft = 100;

    [ObservableProperty]
    private double _windowTop = 100;

    [ObservableProperty]
    private double _windowWidth = 850;

    [ObservableProperty]
    [property: JsonIgnore]
    private bool _historyOnBoot;
    #endregion Properties (some with default values)
}
