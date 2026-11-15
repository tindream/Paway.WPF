using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 文本图标转换器
    /// </summary>
    public class FontImageExtension : MarkupExtension
    {
        /// <summary>
        /// 文本图标
        /// </summary>
        public FontImageType Type { get; set; }

        /// <summary>
        /// </summary>
        public FontImageExtension() { }
        /// <summary>
        /// </summary>
        public FontImageExtension(FontImageType type)
        {
            this.Type = type;
        }
        /// <summary>
        /// 转换器
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var service = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (service != null)
            {
                if (service.TargetObject is Control obj) obj.FontFamily = PConfig.FontAwesome;
                else if (service.TargetObject is TextBlock textBlock) textBlock.FontFamily = PConfig.FontAwesome;
            }
            return Type.Description();
        }
    }
    /// <summary>
    /// 文本图标值
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class CharAttribute : Attribute
    {
        /// <summary>
        /// 值
        /// </summary>
        public char Value { get; private set; }
        /// <summary>
        /// </summary>
        public CharAttribute(char value)
        {
            this.Value = value;
        }
    }
}
