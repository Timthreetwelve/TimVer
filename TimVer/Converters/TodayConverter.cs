// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Converters;

internal class TodayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string date)
        {
            DateTime dt = DateTime.ParseExact(date, "yyyy/MM/dd HH:mm", null);
            if (dt.Date == DateTime.Today)
            {
                return true;
            }
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
