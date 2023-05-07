// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Converters;

internal class TodayConverter : IValueConverter
{
    #region NLog Instance
    private static readonly Logger _log = LogManager.GetLogger("logTemp");
    #endregion NLog Instance

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string)
        {
            //todo see if code from wuview will work here
            try
            {
                DateTime dt = DateTime.ParseExact(value.ToString(), "yyyy/MM/dd HH:mm", null);
                if (dt.Date == DateTime.Today)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"TodayConverter failed {ex.Message}");
                return false;
            }
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
