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
        public const string Text = "测试系统";
        public const string LogConfig = "log.xml";
        public const int Zoom = 10;

        #endregion

        #region 全局数据
        public static Window Window { get; set; }
        public static AdminInfo Admin { get; set; }
        public static AuthInfo Auth { get; set; } = new AuthInfo();

        #endregion
    }
}
