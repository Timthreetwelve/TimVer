// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Converters;

/// <summary>
/// Converts a boolean value of true to "GiB" and false to "GB"
/// </summary>
internal class BoolToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool boolValue = value is bool? && (bool)value;
        string parm = (string)parameter;
        return boolValue ? $"{parm} (GiB)" : $"{parm} (GB)";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
