using Paway.Helper;
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
    /// 值转True
    /// </summary>
    internal class ValueToTrue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result;
            if (value == null || value == DBNull.Value) result = true;
            else if (value is Enum)
            {
                var valueNormal = value.GetHashCode();
                var valueReg = parameter.ToInt();
                result = (valueNormal & valueReg) == valueNormal;
            }
            else result = value.ToString() == parameter.ToStrings();
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值转False
    /// </summary>
    internal class ValueToFalse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result;
            if (value == null || value == DBNull.Value) result = true;
            else if (value is Enum)
            {
                var valueNormal = value.GetHashCode();
                var valueReg = parameter.ToInt();
                result = (valueNormal & valueReg) == valueNormal;
            }
            else result = value.ToString() == parameter.ToStrings();
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值转Visibility
    /// </summary>
    internal class ValueToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)(new ValueToTrue().Convert(value, targetType, parameter, culture));
            return result ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值转Collapsed
    /// </summary>
    internal class ValueToCollapsed : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)(new ValueToFalse().Convert(value, targetType, parameter, culture));
            return result ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
