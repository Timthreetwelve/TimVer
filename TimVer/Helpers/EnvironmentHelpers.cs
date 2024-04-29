// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

internal static class EnvironmentHelpers
{
    #region Get environment variables
    /// <summary>
    /// Gets the environment variables and sorts by variable name
    /// </summary>
    public static List<EnvVariable> GetEnvironmentVariables()
    {
        try
        {
            List<EnvVariable> envList = [];
            foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
            {
                EnvVariable envVariable = new()
                {
                    Variable = entry.Key.ToString()!,
                    Value = entry.Value!.ToString()!
                };
                envList.Add(envVariable);
            }
            return [.. envList.OrderBy(envVariable => envVariable.Variable)];
        }
        catch (Exception ex)
        {
            _log.Debug(ex, "Error occurred in GetEnvironmentVariables.");
            List<EnvVariable> errorList = [];
            EnvVariable envVariable = new()
            {
                Variable = "An Error has occurred.",
                Value = "See the log for more information."
            };
            errorList.Add(envVariable);
            return errorList;
        }
    }
    #endregion Get environment variables

    #region Get special folder
    /// <summary>
    /// Get special folder
    /// </summary>
    /// <param name="value">Special folder name</param>
    /// <returns>Path to folder</returns>
    public static string GetSpecialFolder(Environment.SpecialFolder value)
    {
        return Environment.GetFolderPath(value);
    }
    #endregion Get special folder

    #region Get Uptime
    /// <summary>
    /// Get time since last boot up
    /// </summary>
    /// <returns>Time as TimeSpan</returns>
    public static TimeSpan GetUptime()
    {
        long tickCountMs = Environment.TickCount64;
        return TimeSpan.FromMilliseconds(tickCountMs);
    }
    #endregion Get Uptime
}
