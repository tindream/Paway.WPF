﻿using Paway.Helper;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 缩小圆角(-1)
    /// </summary>
    internal class LessRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius radius)
            {
                return new CornerRadius(Math.Max(0, radius.TopLeft - 1), Math.Max(0, radius.TopRight - 1), Math.Max(0, radius.BottomRight - 1), Math.Max(0, radius.BottomLeft - 1));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    /// <summary>
    /// 无下边圆角(左下、右下)
    /// </summary>
    internal class NoBottomRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius radius)
            {
                return new CornerRadius(radius.TopLeft, radius.TopRight, 0, 0);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    /// <summary>
    /// 无左下边圆角
    /// </summary>
    internal class NoBottomLeftRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius radius)
            {
                return new CornerRadius(radius.TopLeft, radius.TopRight, radius.BottomRight, 0);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    /// <summary>
    /// 左上边圆角
    /// </summary>
    internal class TopLeftRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius radius)
            {
                return new CornerRadius(radius.TopLeft, 0, 0, 0);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    /// <summary>
    /// 右上边圆角
    /// </summary>
    internal class TopRightRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius radius)
            {
                return new CornerRadius(0, radius.TopRight, 0, 0);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    /// <summary>
    /// 上边圆角
    /// </summary>
    internal class TopRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius radius)
            {
                return new CornerRadius(radius.TopLeft, radius.TopRight, 0, 0);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    /// <summary>
    /// 无圆角转换
    /// </summary>
    internal class NoRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new CornerRadius(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
