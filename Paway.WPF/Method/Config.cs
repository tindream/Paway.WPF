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
        #region 常量
        #region GroupActive
        /// <summary>
        /// Active state
        /// </summary>
        public const string StateActive = "Active";
        /// <summary>
        /// Inactive state
        /// </summary>
        public const string StateInactive = "Inactive";
        /// <summary>
        /// Active state group
        /// </summary>
        public const string GroupActive = "ActiveStates";

        #endregion GroupActive

        #endregion

        #region 主题
        /// <summary>
        /// 全局字体大小
        /// </summary>
        public static double FontSize = 15d;

        #endregion
    }
}
