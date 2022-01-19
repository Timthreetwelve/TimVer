// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using System.Windows.Data;

namespace TimVer;

public class BooleanInverter : IValueConverter
{
    // Flips true to false & false to true
    // http://www.nullskull.com/faq/1298/enable-or-disable-a-control-with-a-checkbox-using-data-binding.aspx

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !(bool)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !(bool)value;
    }
}
