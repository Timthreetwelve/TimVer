// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Configuration;

/// <summary>
/// Class for non-persistent settings.
/// </summary>
[INotifyPropertyChanged]
internal sealed partial class TempSettings : ConfigManager<TempSettings>
{
    [ObservableProperty]
    private static bool _appExpanderOpen;

    [ObservableProperty]
    private static bool _driveExpanderOpen;

    [ObservableProperty]
    private static bool _logicalExpanderOpen;

    [ObservableProperty]
    private static bool _physicalExpanderOpen;

    [ObservableProperty]
    private static int _driveSelectedTab;

    [ObservableProperty]
    private bool _historyOnBoot = RegistryHelpers.RegRunEntry("TimVer");

    [ObservableProperty]
    private static bool _langExpanderOpen;

    [ObservableProperty]
    private static bool _uIExpanderOpen;

    [ObservableProperty]
    private static bool _runAccessPermitted;
}
