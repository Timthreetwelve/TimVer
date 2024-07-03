// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

/// <summary>
/// Environment variables.
/// </summary>
public partial class EnvVariable : ObservableObject
{
    #region Properties
    /// <summary>
    /// Name of the environment variable.
    /// </summary>
    [ObservableProperty]
    private string? _variable;

    /// <summary>
    /// Value of the environment variable.
    /// </summary>
    [ObservableProperty]
    private string? _value;
    #endregion Properties
}
