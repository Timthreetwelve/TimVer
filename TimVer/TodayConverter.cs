using System.Windows.Data;

namespace TimVer;

internal class TodayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string)
        {
            DateTime dt = DateTime.ParseExact(value.ToString(), "yyyy/MM/dd HH:mm", null);
            if (dt.Date == DateTime.Today)
            {
                return true;
            }
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
