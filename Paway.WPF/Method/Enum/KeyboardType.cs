using log4net.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 虚拟键盘类别
    /// </summary>
    public enum KeyboardType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 自动
        /// <para>按绑定数据类型</para>
        /// </summary>
        [Description("自动")]
        Auto,
        /// <summary>
        /// 数字键盘
        /// </summary>
        [Description("数字键盘")]
        Num,
        /// <summary>
        /// 全键盘
        /// </summary>
        [Description("全键盘")]
        All,
        /// <summary>
        /// 全键盘(默认显示数字页)
        /// </summary>
        [Description("全键盘_数字")]
        All_Num,
    }
    /// <summary>
    /// 虚拟键盘模式
    /// </summary>
    public enum KeyboardMode
    {
        /// <summary>
        /// 自动
        /// <para>按绑定数据类型</para>
        /// </summary>
        [Description("自动")]
        Auto = 0,
        /// <summary>
        /// 装饰器
        /// </summary>
        [Description("装饰器")]
        Adorner,
        /// <summary>
        /// 窗体
        /// </summary>
        [Description("窗体")]
        Window,
    }
}
