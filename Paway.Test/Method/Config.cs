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

namespace Paway.Test
{
    public class Config : WPF.PConfig
    {
        #region 常量
        public const string Title = "测试系统";
        public static string Text;
        public const string LogConfig = "Log.xml";
        /// <summary>
        /// 图表绽放率
        /// </summary>
        public const int Zoom = 10;
        public static double MinRate = Math.Pow(20.1, 1.0 / Config.Zoom);
        public static double MaxRate = Math.Pow(19999.9, 1.0 / Config.Zoom);
        public const double MinIncrease = -15;
        public const double MaxIncrease = 15;
        /// <summary>
        /// 双击间隔
        /// </summary>
        public static int Interval;

        #endregion

        #region 全局数据
        public static Window Window { get; set; }
        public static AdminInfo Admin { get; set; }
        public static AuthInfo Auth { get; set; } = new AuthInfo();
        /// <summary>
        /// 当前菜单
        /// </summary>
        public static string Menu { get; set; }

        #endregion

        static Config()
        {
            Interval = NativeMethods.GetDoubleClickTime();
        }
    }
}
