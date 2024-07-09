// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

public class EnvVarViewModel : ObservableObject
{
    #region constructor
    public EnvVarViewModel()
    {
        if (EnvVariableList.Count < 1)
        {
            LoadData();
        }
    }
    #endregion constructor

    #region Collection of environment variables
    /// <summary>
    /// List of environment variables.
    /// </summary>
    /// <remarks>A generic list is okay here since a change in the environment won't be detected in the app.</remarks>
    public static List<EnvVariable> EnvVariableList { get; private set; } = [];
    #endregion Collection of environment variables

    #region Load data
    private static void LoadData()
    {
        EnvVariableList = EnvironmentHelpers.GetEnvironmentVariables();
    }
    #endregion Load data

    #region Filter text
    public static event EventHandler<PropertyChangedEventArgs>? StaticPropertyChanged;

    private static string? _filterText;
    public static string FilterText
    {
        get => _filterText!;
        set
        {
            if (_filterText != value)
            {
                _filterText = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(FilterText));
            }
        }
    }
    #endregion Filter text
}
