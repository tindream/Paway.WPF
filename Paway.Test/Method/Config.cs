using log4net;
using Paway.Helper;
using Paway.Model;
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
    public abstract class Config : MConfig
    {
        #region 常量
        public const string Title = "测试系统";
        public static string Text;
        public static string LogConfig = "Log.xml";
        /// <summary>
        /// 图表绽放率
        /// </summary>
        public const int Zoom = 10;
        public static double MinRate = Math.Pow(20.1, 1.0 / Config.Zoom);
        public static double MaxRate = Math.Pow(19999.9, 1.0 / Config.Zoom);
        public const double MinIncrease = -15;
        public const double MaxIncrease = 15;

        #endregion

        #region 全局数据 
        public static AdminInfo Admin { get; set; }
        private static LanguageInfo _language;
        public static LanguageInfo Language
        {
            get
            {
                if (_language == null) _language = Proxy.Create<LanguageInfo>(typeof(InterceptorNotify), nameof(InterceptorNotify.Invoke));
                return _language;
            }
        }
        public static void InitLanguage()
        {
            var file = Path.Combine(LanguagePath, $"{Config.LanguageStr}.xml");
            LanguageHelper.Reset(file, Language, Config.LanguageStr);
        }
        public static string LanguageStr = "中文";
        public static string LanguagePath
        {
            get
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "languages");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }
        public static List<string> LanguageList = new List<string>();

        #endregion
    }
}
