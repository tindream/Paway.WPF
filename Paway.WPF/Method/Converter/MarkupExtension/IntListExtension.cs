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
    /// 整形值列表转换器
    /// </summary>
    public class IntListExtension : MarkupExtension
    {
        /// <summary>
        /// 显示数量
        /// </summary>
        public int Count { get; set; } = 9;
        /// <summary>
        /// 从指定值开始显示
        /// </summary>
        public int Start { get; set; } = 1;
        /// <summary>
        /// 值间隔
        /// </summary>
        public int Interval { get; set; } = 1;

        /// <summary>
        /// </summary>
        public IntListExtension() { }
        /// <summary>
        /// </summary>
        public IntListExtension(int count)
        {
            this.Count = count;
        }
        /// <summary>
        /// 转换器
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            //Interval 为 0 或负数时 i 不递增，原循环条件恒真 → XAML 解析阶段直接卡死 UI 线程
            var interval = Interval > 0 ? Interval : 1;
            var count = Count > 0 ? Count : 0;
            var list = new List<int>(count);
            for (var i = Start; list.Count < count; i += interval)
            {
                list.Add(i);
            }
            return list;
        }
    }
}
