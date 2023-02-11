// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private static bool _uIExpanderOpen;

    [ObservableProperty]
    private static bool _appExpanderOpen;
}
