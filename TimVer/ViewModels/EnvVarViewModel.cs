// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

public static class EnvVarViewModel
{
    #region NLog Instance
    private static readonly Logger _log = LogManager.GetCurrentClassLogger();
    #endregion NLog Instance

    #region Get environment variables
    /// <summary>
    /// Gets the environment variables and sorts by key
    /// </summary>
    public static void GetEnvironmentVariables()
    {
        List<EnvVariable> envList = new();
        IDictionary env = Environment.GetEnvironmentVariables();
        _log.Debug($"Found {env.Count} environment variables");

        foreach (DictionaryEntry x in env)
        {
            EnvVariable envVariable = new()
            {
                Variable = x.Key.ToString(),
                Value = x.Value.ToString()
            };
            envList.Add(envVariable);
        }
        EnvVariable.EnvVariableList = envList.OrderBy(envVariable => envVariable.Variable).ToList();
    }
    #endregion Get environment variables
}
