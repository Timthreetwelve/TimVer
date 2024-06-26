﻿// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

/// <summary>
/// Environment variables.
/// </summary>
public partial class EnvVariable : ObservableObject
{
    #region Properties
    [ObservableProperty]
    private string? _variable;

    [ObservableProperty]
    private string? _value;
    #endregion Properties
}
