// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Class for UEFI related things.
/// </summary>
internal static partial class UefiHelpers
{
    #region Get secure boot state
    /// <summary>
    /// Determines if UEFI secure boot is enabled.
    /// </summary>
    /// <returns>Localized string</returns>
    public static string UEFISecureBoot()
    {
        try
        {
            using RegistryKey? key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\SecureBoot\State");
            object? secureBootValue = key?.GetValue("UEFISecureBootEnabled");
            if (secureBootValue is int secureBootEnabled)
            {
                return secureBootEnabled == 1 ?
                    GetStringResource("HardwareInfo_SecureBoot_Enabled") :
                    GetStringResource("HardwareInfo_SecureBoot_Disabled");
            }

            _log.Warn("UEFISecureBootEnabled registry value is not available.");
            return GetStringResource("MsgText_NotAvailable");
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Call to UEFISecureBoot has failed.");
            return GetStringResource("MsgText_ErrorGeneral");
        }
    }
    #endregion Get secure boot state

    #region Get UEFI mode
    /// <summary>
    /// Determines if computer is in UEFI mode.
    /// </summary>
    /// <returns>
    /// Localized string for "UEFI mode" if the computer is in UEFI mode. "BIOS mode" if the computer is in BIOS mode.
    /// </returns>
    /// <remarks>
    /// https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-getfirmwaretype
    /// </remarks>

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetFirmwareType(out FirmwareType firmwareType);

    public static string UEFIEnabled()
    {
        try
        {
            if (GetFirmwareType(out FirmwareType firmwareType))
            {
                switch (firmwareType)
                {
                    case FirmwareType.Bios:
                        return GetStringResource("HardwareInfo_BIOS");
                    case FirmwareType.Uefi:
                        return GetStringResource("HardwareInfo_UEFI");
                    case FirmwareType.Unknown:
                    case FirmwareType.Max:
                        return GetStringResource("MsgText_UefiUnknown");
                }
            }
            else
            {
                _log.Error($"Error retrieving firmware type. Win32 Error: {Marshal.GetLastWin32Error()}");
                return GetStringResource("MsgText_ErrorGeneral");
            }
        }
        catch (EntryPointNotFoundException ex)
        {
            _log.Error(ex, "GetFirmwareType is not supported on this version of Windows.");
            return GetStringResource("MsgText_ErrorGeneral");
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Unexpected error: {ex.Message}");
            return GetStringResource("MsgText_ErrorGeneral");
        }

        return string.Empty;
    }
}
    #endregion

#region Enum for firmware types
enum FirmwareType : uint
{
    Unknown = 0,
    Bios = 1,
    Uefi = 2,
    Max = 3
}
#endregion Enum for firmware types
