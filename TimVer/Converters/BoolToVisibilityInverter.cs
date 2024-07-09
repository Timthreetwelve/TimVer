// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Converters;

/// <summary>
/// An inverse bool to visibility converter
/// </summary>
/// <seealso cref="System.Windows.Data.IValueConverter" />
internal class BoolToVisibilityInverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (bool)value! ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
