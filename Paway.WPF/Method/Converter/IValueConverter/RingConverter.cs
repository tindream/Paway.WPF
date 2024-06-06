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
    internal class RingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var width = values[0].ToDouble(); if (width < 0) width = 0;
            var height = values[1].ToDouble(); if (height < 0) height = 0;

            double radius;
            if (values[2] is Thickness thickness) radius = thickness.Left;
            else radius = values[2].ToDouble();
            if (width < radius) width = radius;
            if (height < radius) height = radius;

            var percent = 0.33;
            if (values.Length >= 6)
            {
                var min = values[3].ToDouble();
                var max = values[4].ToDouble();
                var value = values[5].ToDouble();
                var rate = values.Length >= 7 ? values[6].ToDouble() : 1;
                value = min + (value - min) * rate;
                value = value > max ? max : value;
                value = value < min ? min : value;
                percent = (value - min) / (max - min);
            }

            var point2X = (height - radius) / 2 * Math.Cos((2 * percent - 0.5) * Math.PI) + height / 2;
            var point2Y = height / 2 - (height - radius) / 2 * Math.Sin((2 * percent + 0.5) * Math.PI);

            string path;
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
