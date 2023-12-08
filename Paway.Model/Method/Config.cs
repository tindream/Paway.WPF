using log4net;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Paway.Model
{
    public class Config : PConfig
    {
        #region 全局数据
        public static Window Window { get; set; }
        /// <summary>
        /// 当前菜单
        /// </summary>
        public static string Menu { get; set; }

        #endregion
    }
}
