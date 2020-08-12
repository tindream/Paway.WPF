using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Config
    {
        #region 主题
        /// <summary>
        /// 主题字体大小
        /// </summary>
        public static double FontSize = 15d;
        /// <summary>
        /// 主题颜色
        /// </summary>
        public static Color Color = Color.FromArgb(255, 35, 175, 255);

        #endregion
    }
}
