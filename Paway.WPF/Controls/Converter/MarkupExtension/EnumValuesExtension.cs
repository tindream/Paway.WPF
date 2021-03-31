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
    /// 枚举列表转换器
    /// </summary>
    public class EnumValuesExtension : MarkupExtension
    {
        /// <summary>
        /// 文本图标
        /// </summary>
        public Type EnumType { get; set; }
        /// <summary>
        /// 从指定位置开始显示
        /// </summary>
        public int Start { get; set; } = 1;
        /// <summary>
        /// 显示数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// </summary>
        public EnumValuesExtension() { }
        /// <summary>
        /// </summary>
        public EnumValuesExtension(Type type)
        {
            this.EnumType = type;
        }
        /// <summary>
        /// 转换器
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var list = EnumType.GetFields(TConfig.Flags).ToList();
            if (Count == 0 || Count > list.Count - Start) Count = list.Count - Start;
            if (Count < 0) Count += list.Count - Start;
            var result = new List<string>();
            for (int i = Start; i < Start + Count; i++)
            {
                result.Add(list[i].Description());
            }
            return result;
        }
    }
}
