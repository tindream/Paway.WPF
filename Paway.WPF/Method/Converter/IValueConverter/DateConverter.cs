using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Paway.WPF
{
    /// <summary>
    /// DateTime格式化(默认G)
    /// </summary>
    internal class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime time)
            {
                if (time == DateTime.MinValue) return null;
                var param = parameter == null ? "G" : parameter.ToString();
                return time.ToString(param);
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Time格式化
    /// </summary>
    internal class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime time)
            {
                if (time == DateTime.MinValue) return null;
                if (parameter is TimePickerType type)
                {
                    switch (type)
                    {
                        case TimePickerType.Hour: return $"{time.Hour:D2}";
                        case TimePickerType.HourMinute: return $"{time.Hour:D2}:{time.Minute:D2}";
                        case TimePickerType.Time: return $"{time.Hour:D2}:{time.Minute:D2}:{time.Second:D2}";
                    }
                }
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
