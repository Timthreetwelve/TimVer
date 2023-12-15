// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Converters;

internal class BoolToStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a boolean value of true to "GiB" and false to "GB".
    /// </summary>
    /// <remarks>Same converter is used for the Size and Free column headings</remarks>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool boolValue = value is bool? && (bool)value;
        string parm = (string)parameter;
        if (parm.Equals("Free", StringComparison.OrdinalIgnoreCase))
        {
            string free = GetStringResource("DriveInfo_Free");
            return boolValue ? $"{free} (GiB)" : $"{free} (GB)";
        }
        else if (parm.Equals("Size", StringComparison.OrdinalIgnoreCase))
        {
            string size = GetStringResource("DriveInfo_Size");
            return boolValue ? $"{size} (GiB)" : $"{size} (GB)";
        }
        return boolValue ? $"{parm} (GiB)" : $"{parm} (GB)";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
