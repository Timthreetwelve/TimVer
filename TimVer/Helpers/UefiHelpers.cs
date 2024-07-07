// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.
using static Vanara.PInvoke.Kernel32;

namespace TimVer.Helpers;
/// <summary>
/// Class for UEFI related things.
/// </summary>
internal static class UefiHelpers
{
    /// <summary>
    /// Determines if UEFI secure boot is enabled.
    /// </summary>
    /// <returns><c>true</c> if UEFI secure boot is enabled.</returns>
    public static string UEFISecureBoot()
    {
        try
        {
            using RegistryKey? key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\SecureBoot\State");
            return (int?)key!.GetValue("UEFISecureBootEnabled") == 1 ?
                GetStringResource("HardwareInfo_SecureBoot_Enabled") :
                GetStringResource("HardwareInfo_SecureBoot_Disabled");
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Call to UEFISecureBoot has failed.");
            return GetStringResource("ErrorGeneral");
        }
    }

    /// <summary>
    /// Determines if computer is in EUFI mode.
    /// </summary>
    /// <returns>"UEFI mode" if the computer is in UEFI mode. "BIOS mode"if the computer is in BIOS mode.</returns>
    /// <remarks>https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-getfirmwareenvironmentvariablea</remarks>
    public static string UEFIEnabled()
    {
        try
        {
            const int ERROR_INVALID_FUNCTION = 1;
            _ = GetFirmwareEnvironmentVariable("",
                                               "{00000000-0000-0000-0000-000000000000}",
                                               IntPtr.Zero,
                                               0);
            return GetLastError() != ERROR_INVALID_FUNCTION ?
                GetStringResource("HardwareInfo_UEFI") :
                GetStringResource("HardwareInfo_BIOS");
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Call to UEFIEnabled has failed.");
            return GetStringResource("ErrorGeneral");
        }
    }
}
