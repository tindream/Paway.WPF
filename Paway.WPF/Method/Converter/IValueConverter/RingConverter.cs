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
            //绑定尚未就绪时会传入 DependencyProperty.UnsetValue 或少于4个值，原实现直接越界
            if (values == null || values.Length < 4) return Geometry.Empty;
            var width = values[0].ToDouble(); if (width < 0) width = 0;
            var height = values[1].ToDouble(); if (height < 0) height = 0;
            var iForward = values[2].ToBool() ? 1 : 0;

            double radius;
            if (values[3] is Thickness thickness) radius = thickness.Left;
            else radius = values[3].ToDouble();
            if (width < radius) width = radius;
            if (height < radius) height = radius;
            if (width <= 0 || height <= 0) return Geometry.Empty;

            var percent = 0.33;
            if (values.Length > 6)
            {
                var min = values[4].ToDouble();
                var max = values[5].ToDouble();
                var value = values[6].ToDouble();
                var rate = values.Length > 7 ? values[7].ToDouble() / 360 : 1;
                value = min + (value - min) * rate;
                value = value > max ? max : value;
                value = value < min ? min : value;
                //Minimum == Maximum（数据未加载/单值进度）时原实现除零得到 NaN，
                //拼进 path 后 PathGeometry.Parse 每帧抛 FormatException
                var span = max - min;
                percent = span <= 0 ? 0 : (value - min) / span;
            }

            var point2X = (height - radius) / 2 * Math.Cos((2 * percent - 0.5) * Math.PI) + height / 2;
            if (iForward == 0) point2X = height / 2 - (height - radius) / 2 * Math.Cos((2 * percent - 0.5) * Math.PI);
            var point2Y = height / 2 - (height - radius) / 2 * Math.Sin((2 * percent + 0.5) * Math.PI);

            string path;
            string start = "M " + width / 2 + "," + radius / 2 + " A " + (width - radius) / 2 + "," + (width - radius) / 2 + $" 0 0 {iForward} ";
            if (percent <= 0)
            {
                //percent 为 0 时路径为空字符串，PathGeometry.Parse("") 会抛异常
                return Geometry.Empty;
            }
            else if (percent < 0.5)
            {
                path = start + point2X + "," + point2Y + "";
            }
            else if (percent == 0.5)
            {
                path = start + width / 2 + "," + (height - radius / 2);
            }
            else
            {
                path = start + width / 2 + "," + (height - radius / 2) + $" A " + (width - radius) / 2 + "," + (width - radius) / 2 + $" 0 0 {iForward} " + point2X + "," + point2Y + "";
            }
            try
            {
                return PathGeometry.Parse(path);
            }
            catch (FormatException)
            {
                //极端尺寸下仍可能拼出非法坐标，宁可少画一个圆环也不要每帧刷异常
                return Geometry.Empty;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { DependencyProperty.UnsetValue, DependencyProperty.UnsetValue };
        }
    }
}
