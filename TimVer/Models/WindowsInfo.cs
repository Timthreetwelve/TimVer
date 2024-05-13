// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

internal partial class WindowsInfo : ObservableObject
{
    #region Properties
    [ObservableProperty]
    private string? _dotNetVersion;

    [ObservableProperty]
    private string? _osArch;

    [ObservableProperty]
    private string? _osBranch;

    [ObservableProperty]
    private string? _osBuild;

    [ObservableProperty]
    private string? _osEdition;

    [ObservableProperty]
    private string? _osEditionID;

    [ObservableProperty]
    private string? _osInstallDate;

    [ObservableProperty]
    private string? _osVersion;

    [ObservableProperty]
    private string? _registeredOrg;

    [ObservableProperty]
    private string? _registeredUser;

    [ObservableProperty]
    private string? _windowsFolder;

    [ObservableProperty]
    private string? _tempFolder;
    #endregion Properties
}
