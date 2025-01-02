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
    public class ValueMultiConverter : IValueConverter
    {
        /// <summary>
        /// 值乘数转换(默认*0.85)
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = value.ToDouble();
            var param = parameter == null ? 0.85 : parameter.ToDouble();
            return temp * param;
        }
        /// <summary>
        /// </summary>
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
    public class ValueToTrue : IValueConverter
    {
        /// <summary>
        /// 值转True
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result;
            if (value == null || value == DBNull.Value) result = false;
            else if (value is Enum)
            {
                var valueNormal = value.GetHashCode();
                var valueReg = parameter.ToInt();
                result = valueNormal == valueReg;
            }
            else result = value.ToString() == parameter.ToStrings();
            return result;
        }
        /// <summary>
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.ToBool()) return null;//重复绑定且枚举值超出范围时绑定报错
            return parameter;
        }
    }
    /// <summary>
    /// 值转True(大于)
    /// </summary>
    public class ValueThanToTrue : IValueConverter
    {
        /// <summary>
        /// 值转True
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            if (value == null || value == DBNull.Value) result = false;
            else if (value is Enum)
            {
                var valueNormal = value.GetHashCode();
                var valueReg = parameter.ToInt();
                result = valueNormal > valueReg;
            }
            return result;
        }
        /// <summary>
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.ToBool()) return null;//重复绑定且枚举值超出范围时绑定报错
            return parameter;
        }
    }
    /// <summary>
    /// 值转True(小于)
    /// </summary>
    public class ValueLessToTrue : IValueConverter
    {
        /// <summary>
        /// 值转True
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            if (value == null || value == DBNull.Value) result = false;
            else if (value is Enum)
            {
                var valueNormal = value.GetHashCode();
                var valueReg = parameter.ToInt();
                result = valueNormal < valueReg;
            }
            return result;
        }
        /// <summary>
        /// </summary>
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
            if (value == null || value == DBNull.Value) result = false;
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
    /// 值转True(多枚举值)
    /// </summary>
    public class ValueMoreToTrue : IValueConverter
    {
        /// <summary>
        /// 值转True(多枚举值)
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result;
            if (value == null || value == DBNull.Value) result = false;
            else if (value is Enum)
            {
                var valueNormal = value.GetHashCode();
                var valueParameter = parameter.ToInt();
                result = (valueNormal & valueParameter) == valueParameter;
            }
            else
            {
                var valueParameters = parameter.ToStrings();
                var valueList = value.ToString().Split(new[] { '|', ',' }).ToList();
                if (valueList.Count > 1) result = valueList.Contains(valueParameters);
                else result = value.ToString().Contains(valueParameters);
            }
            return result;
        }
        /// <summary>
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value.ToBool() ? 1 : -1;
            var pStr = parameter.ToString();
            var pInt = parameter.ToInt();
            if (pStr == pInt.ToString()) return (result * pInt).ToString();//需返回字符串格式
            else return (result * pStr.GetHashCode()).ToString();
        }
    }
    /// <summary>
    /// 值转True(多参数)
    /// </summary>
    public class ValueToMoreTrue : IValueConverter
    {
        /// <summary>
        /// 值转True(多参数)
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result;
            var valueParameter = parameter.ToInt();
            if (value == null || value == DBNull.Value) result = false;
            else if (value is Enum)
            {
                var valueNormal = value.GetHashCode();
                if (valueParameter.ToStrings() == parameter.ToStrings())
                {
                    result = (valueParameter & valueNormal) == valueNormal;
                }
                else
                {
                    var valueNormals = valueNormal.ToString();
                    var parameterList = parameter.ToStrings().Split(new[] { '|', ',' }).ToList();
                    result = parameterList.Contains(valueNormals);
                }
            }
            else
            {
                var valueNormals = value.ToString();
                var parameterList = parameter.ToStrings().Split(new[] { '|', ',' }).ToList();
                if (parameterList.Count > 1) result = parameterList.Contains(valueNormals);
                else result = parameter.ToStrings().Contains(valueNormals);
            }
            return result;
        }
        /// <summary>
        /// </summary>
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
    /// 值转False(大于)
    /// </summary>
    internal class ValueThanToFalse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueThanToTrue().Convert(value, targetType, parameter, culture);
            return !result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToBool()) return null;//重复绑定且枚举值超出范围时绑定报错
            return parameter;
        }
    }
    /// <summary>
    /// 值转False(小于)
    /// </summary>
    internal class ValueLessToFalse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueLessToTrue().Convert(value, targetType, parameter, culture);
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
    /// 值转False(多枚举值)
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
            var result = value.ToBool() ? -1 : 1;
            return (result * parameter.ToInt()).ToString();
        }
    }
    /// <summary>
    /// 值转False(多参数)
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
    /// 值转Visibility(大于)
    /// </summary>
    internal class ValueThanToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueThanToTrue().Convert(value, targetType, parameter, culture);
            return result ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值转Visibility(小于)
    /// </summary>
    internal class ValueLessToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueLessToTrue().Convert(value, targetType, parameter, culture);
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
    /// 值转Visibility(多枚举值)
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
    /// 值转Visibility(多参数)
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
    /// 值转Collapsed(大于)
    /// </summary>
    internal class ValueThanToCollapsed : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueThanToTrue().Convert(value, targetType, parameter, culture);
            return result ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 值转Collapsed(小于)
    /// </summary>
    internal class ValueLessToCollapsed : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)new ValueLessToTrue().Convert(value, targetType, parameter, culture);
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
    /// 值转Collapsed(多枚举值)
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
    /// 值转Collapsed(多参数)
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
