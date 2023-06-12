// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

#pragma warning disable RCS1102 // Make class static.
public class EnvVarViewModel
#pragma warning restore RCS1102 // Make class static.
{
    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

    private static string _filterText;
    public static string FilterText
    {
        get => _filterText;
        set
        {
            if (_filterText != value)
            {
                _filterText = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(FilterText));
            }
        }
    }
}