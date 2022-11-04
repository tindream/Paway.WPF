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
    /// 多值相乘转换
    /// </summary>
    public class MoreValueMultiConverter : IMultiValueConverter
    {
        /// <summary>
        /// 多值相乘转换
        /// </summary>
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(int))
            {
                var result = 1;
                for (var i = 0; i < value.Length; i++) result *= value[i].ToInt();
                return result;
            }
            {
                var result = 1d;
                for (var i = 0; i < value.Length; i++) result *= value[i].ToDouble();
                return result;
            }
        }
        /// <summary>
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值乘数转换(默认*0.85)
    /// </summary>
    internal class ValueMultiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = value.ToDouble();
            var param = parameter == null ? 0.85 : parameter.ToDouble();
            return temp * param;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = value.ToDouble();
            var param = parameter == null ? 0.85 : parameter.ToDouble();
            return temp / param;
        }
    }
    /// <summary>
    /// 值加减转换(默认-2)
    /// </summary>
    internal class ValueAddConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = value.ToDouble();
            var param = parameter == null ? -2 : parameter.ToDouble();
            var result = temp + param;
            if (result < 0) result = 0;
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = value.ToDouble();
            var param = parameter == null ? -2 : parameter.ToDouble();
            var result = temp - param;
            if (result < 0) result = 0;
            return result;
        }
    }

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
                result = valueNormal == valueReg;
            }
            else result = value.ToString() == parameter.ToStrings();
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.ToBool()) return null;//重复绑定且枚举值超出范围时绑定报错
            return parameter;
        }
    }
    /// <summary>
    /// 值转True(多枚举有并集)
    /// </summary>
    internal class ValueUnionToTrue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            if (value == null || value == DBNull.Value) result = true;
            else if (value is Enum)
            {
                var valueNormal = value.GetHashCode();
                var valueReg = parameter.ToInt();
                result = (valueNormal & valueReg) != 0;
            }
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值转True(多原始枚举值)
    /// </summary>
    internal class ValueMoreToTrue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result;
            if (value == null || value == DBNull.Value) result = true;
            else if (value is Enum)
            {
                var valueNormal = value.GetHashCode();
                var valueReg = parameter.ToInt();
                result = (valueNormal & valueReg) == valueReg;
            }
            else result = value.ToString() == parameter.ToStrings();
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PConfig.IConvertBack = true;
            var result = value.ToBool() ? 1 : -1;
            return result * parameter.ToInt();
        }
    }
    /// <summary>
    /// 值转True(多目标枚举值)
    /// </summary>
    internal class ValueToMoreTrue : IValueConverter
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
            var result = (bool)new ValueToTrue().Convert(value, targetType, parameter, culture);
            return !result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToBool()) return null;//重复绑定且枚举值超出范围时绑定报错
            return parameter;
        }
    }
    /// <summary>
    /// 值转False(多枚举无并集)
    /// </summary>
    internal class ValueUnionToFalse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueUnionToTrue().Convert(value, targetType, parameter, culture);
            return !result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值转False(多原始枚举值)
    /// </summary>
    internal class ValueMoreToFalse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueMoreToTrue().Convert(value, targetType, parameter, culture);
            return !result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PConfig.IConvertBack = true;
            var result = value.ToBool() ? -1 : 1;
            return result * parameter.ToInt();
        }
    }
    /// <summary>
    /// 值转False(多目标枚举值)
    /// </summary>
    internal class ValueToMoreFalse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueToMoreTrue().Convert(value, targetType, parameter, culture);
            return !result;
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
            var result = (bool)new ValueToTrue().Convert(value, targetType, parameter, culture);
            return result ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值转Visibility(多枚举无并集)
    /// </summary>
    internal class ValueUnionToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueUnionToTrue().Convert(value, targetType, parameter, culture);
            return result ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值转Visibility(多原始枚举值)
    /// </summary>
    internal class ValueMoreToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueMoreToTrue().Convert(value, targetType, parameter, culture);
            return result ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值转Visibility(多目标枚举值)
    /// </summary>
    internal class ValueToMoreVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueToMoreTrue().Convert(value, targetType, parameter, culture);
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
            var result = (bool)new ValueToTrue().Convert(value, targetType, parameter, culture);
            return result ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值转Collapsed(多枚举无并集)
    /// </summary>
    internal class ValueUnionToCollapsed : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueUnionToTrue().Convert(value, targetType, parameter, culture);
            return result ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值转Collapsed(多原始枚举值)
    /// </summary>
    internal class ValueMoreToCollapsed : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueMoreToTrue().Convert(value, targetType, parameter, culture);
            return result ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值转Collapsed(多目标枚举值)
    /// </summary>
    internal class ValueToMoreCollapsed : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueToMoreTrue().Convert(value, targetType, parameter, culture);
            return result ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
