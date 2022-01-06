using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 日历-周末颜色转换器
    /// </summary>
    internal class WeekendConvert : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            var calendarDayButton = (CalendarDayButton)values;
            if (calendarDayButton.DataContext is DateTime dateTime)
            {
                if (!calendarDayButton.IsMouseOver && !calendarDayButton.IsSelected && !calendarDayButton.IsBlackedOut && (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday))
                    return new SolidColorBrush(PConfig.Error);
            }
            return new SolidColorBrush(PConfig.TextSub);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
