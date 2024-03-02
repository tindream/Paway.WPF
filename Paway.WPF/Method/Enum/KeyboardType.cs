using log4net;
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
    /// 软键盘类别
    /// </summary>
    public enum KeyboardType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
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
    }
}
