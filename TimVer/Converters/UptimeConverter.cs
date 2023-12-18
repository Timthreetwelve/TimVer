// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Converters
{
    class UptimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan span)
            {
                int days = span.Days;
                int hours = span.Hours;
                int minutes = span.Minutes;
                int seconds = span.Seconds;
                return string.Format(GetStringResource("HardwareInfo_UptimeString"), days, hours, minutes, seconds);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
