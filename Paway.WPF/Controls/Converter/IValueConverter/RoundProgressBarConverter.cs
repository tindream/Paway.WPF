using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 绘制进度圆环
    /// </summary>
    internal class RoundProgressBarConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var width = (double)values[0];
            var height = (double)values[1];

            double radius = 0;
            if (values[2] is Thickness) radius = ((Thickness)values[2]).Left;
            else radius = values[2].ToDouble();

            var percent = 0.33;
            if (values.Length == 6)
            {
                var min = (double)values[3];
                var max = (double)values[4];
                var value = (double)values[5];
                value = value > max ? max : value;
                value = value < min ? min : value;
                percent = (value - min) / (max - min);
            }

            var point1X = height / 2 * Math.Cos((2 * percent - 0.5) * Math.PI) + height / 2;
            var point1Y = height / 2 - height / 2 * Math.Sin((2 * percent + 0.5) * Math.PI);
            var point2X = (height - radius) / 2 * Math.Cos((2 * percent - 0.5) * Math.PI) + height / 2;
            var point2Y = height / 2 - (height - radius) / 2 * Math.Sin((2 * percent + 0.5) * Math.PI);

            var path = "";

            if (percent == 0)
            {
                path = "";
            }
            else if (percent < 0.5)
            {
                path = "M " + width / 2 + "," + radius / 2 + " A " + (width - radius) / 2 + "," + (width - radius) / 2 + " 0 0 1 " + point2X + "," + point2Y + "";
            }
            else if (percent == 0.5)
            {
                path = "M " + width / 2 + "," + radius / 2 + " A " + (width - radius) / 2 + "," + (width - radius) / 2 + " 0 0 1 " + width / 2 + "," + (height - radius / 2);
            }
            else
            {
                path = "M " + width / 2 + "," + radius / 2 + " A " + (width - radius) / 2 + "," + (width - radius) / 2 + " 0 0 1 " + width / 2 + "," + (height - radius / 2) +
                    " A " + (width - radius) / 2 + "," + (width - radius) / 2 + " 0 0 1 " + point2X + "," + point2Y + "";
            }
            return PathGeometry.Parse(path);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { DependencyProperty.UnsetValue, DependencyProperty.UnsetValue };
        }
    }
}
