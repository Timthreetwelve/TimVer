// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

internal sealed partial class WindowsInfo : ObservableObject
{
    #region Properties
    /// <summary>
    /// Version of the .NET runtime.
    /// </summary>
    [ObservableProperty]
    private string? _dotNetVersion;

    /// <summary>
    /// Windows architecture. 32 or 64 bit.
    /// </summary>
    [ObservableProperty]
    private string? _osArch;

    /// <summary>
    /// Windows build branch.
    /// </summary>
    [ObservableProperty]
    private string? _osBranch;

    /// <summary>
    /// Windows build number.
    /// </summary>
    [ObservableProperty]
    private string? _osBuild;

    /// <summary>
    /// Windows Edition. Windows 11 Home, Windows 11 pro, etc.
    /// </summary>
    [ObservableProperty]
    private string? _osEdition;

    /// <summary>
    /// Windows Edition ID. Home, Pro, Enterprise, etc.
    /// </summary>
    [ObservableProperty]
    private string? _osEditionID;

    /// <summary>
    /// The date Windows was installed.
    /// </summary>
    [ObservableProperty]
    private string? _osInstallDate;

    /// <summary>
    /// Windows version. 22H1, 23H2, etc.
    /// </summary>
    [ObservableProperty]
    private string? _osVersion;

    /// <summary>
    /// Registered organization.
    /// </summary>
    [ObservableProperty]
    private string? _registeredOrg;

    /// <summary>
    /// Registered user.
    /// </summary>
    [ObservableProperty]
    private string? _registeredUser;

    /// <summary>
    /// Location of the Windows folder.
    /// </summary>
    [ObservableProperty]
    private string? _windowsFolder;

    /// <summary>
    /// Location of the user's Temp folder.
    /// </summary>
    [ObservableProperty]
    private string? _tempFolder;
    #endregion Properties
}
