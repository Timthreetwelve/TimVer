// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

/// <summary>
/// Environment variables
/// </summary>
public partial class EnvVariable : ObservableObject
{
    #region Properties
    [ObservableProperty]
    private string? _variable;

    [ObservableProperty]
    private string? _value;
    #endregion Properties

    #region Environment variables
    private static List<EnvVariable>? _envVariableList;
    public static List<EnvVariable> EnvVariableList
    {
        get
        {
            return _envVariableList ??= EnvironmentHelpers.GetEnvironmentVariables();
        }
    }
    #endregion Environment variables
}
