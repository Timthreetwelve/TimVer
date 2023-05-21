// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Class to check, add or remove entries in HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
/// </summary>
public static class RegRun
{
    private const string _regPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    /// <summary>
    /// Checks to see if an entry exists in HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
    /// </summary>
    /// <param name="name">Name of entry to check</param>
    /// <returns>True if entry exists</returns>
    public static bool RegRunEntry(string name)
    {
        using RegistryKey key = Registry.CurrentUser.OpenSubKey(_regPath, true);
        return key.GetValue(name) != null;
    }

    /// <summary>
    /// Adds an entry to HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
    /// </summary>
    /// <param name="name">Name to add</param>
    /// <param name="data">Full path and arguments</param>
    /// <returns>"OK" if successful, Exception message if not successful</returns>
    public static string AddRegEntry(string name, string data)
    {
        try
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(_regPath, true);
            key.SetValue(name, data);

            return "OK";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    /// Removes an entry from HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
    /// </summary>
    /// <param name="name">Name to remove</param>
    /// <returns>"OK" if successful, Exception message if not successful</returns>
    public static string RemoveRegEntry(string name)
    {
        try
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(_regPath, true);
            key.DeleteValue(name, false);

            return "OK";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
