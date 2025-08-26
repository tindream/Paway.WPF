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
    public partial class PConfig : TConfig
    {
        static PConfig()
        {
            FontAwesome = new FontFamily(new Uri(@"pack://application:,,,/Paway.WPF;component/Resources/Resource/"), "./#fontawesome");
            if (!Application.Current.Resources.MergedDictionaries.Any(c => c.Source.ToStrings().Contains("Paway.xaml")))
            {
                // 创建资源字典
                ResourceDictionary globalResources = new ResourceDictionary();
                // 指定资源字典的源（确保"Resources.xaml"的生成操作设置为Page或Resource）
                globalResources.Source = new Uri("/Paway.WPF;component/Themes/Paway.xaml", UriKind.Relative);
                // 合并到应用程序资源
                Application.Current.Resources.MergedDictionaries.Add(globalResources);
            }
        }
        /// <summary>
        /// 加载全局资源
        /// </summary>
        internal static void InitResources() { }

        #region 全局配置
        /// <summary>
        /// 主窗体
        /// </summary>
        public static Window Window { get; set; }
        private static LanguageBaseInfo languageBase;
        /// <summary>
        /// 多语言包
        /// </summary>
        public static LanguageBaseInfo LanguageBase
        {
            get
            {
                if (languageBase == null)
                {
                    languageBase = Proxy.Create<LanguageBaseInfo>(typeof(InterceptorNotify), nameof(InterceptorNotify.Invoke));
                }
                return languageBase;
            }
        }
        /// <summary>
        /// 库中语言包自动初始化，可在此处更新值
        /// </summary>
        public static void InitLanguageBase(LanguageBaseInfo language)
        {
            language.Clone(LanguageBase);
        }
        /// <summary>
        /// QuartZ字体
        /// </summary>
        public static FontFamily FontAwesome { get; private set; }
        /// <summary>
        /// 虚拟键盘状态
        /// </summary>
        public static EnableType Keyboard { get; set; }
        /// <summary>
        /// 虚拟键盘模式
        /// </summary>
        public static KeyboardMode KeyboardMode { get; set; }

        #endregion

        #region 主题配置
        /// <summary>
        /// 颜色的 Alpha 通道默认值
        /// </summary>
        internal const int Alpha = 200;
        /// <summary>
        /// 颜色差量
        /// </summary>
        internal const int Interval = 40;

        /// <summary>
        /// 主题文本字体变化事件
        /// </summary>
        public static event Action<string> FontFamilyChanged;
        private static string _fontFamily = "Microsoft YaHei";
        /// <summary>
        /// 主题文本字体
        /// </summary>
        public static string FontFamily
        {
            get { return _fontFamily; }
            set
            {
                if (_fontFamily != value)
                {
                    var old = _fontFamily;
                    _fontFamily = value;
                    FontFamilyChanged?.Invoke(old);
                }
            }
        }

        /// <summary>
        /// 主题字体大小变化事件
        /// </summary>
        public static event Action<double> FontSizeChanged;
        private static double _fontSize = 15d;
        /// <summary>
        /// 主题字体大小
        /// </summary>
        public static double FontSize
        {
            get { return _fontSize; }
            set
            {
                if (value < 1) return;
                if (_fontSize != value)
                {
                    var old = _fontSize;
                    _fontSize = value;
                    FontSizeChanged?.Invoke(old);
                }
            }
        }

        /// <summary>
        /// 主题颜色变化事件
        /// </summary>
        public static event Action<Color> ColorChanged;
        private static Color _color = Color.FromArgb(255, 64, 158, 255);
        /// <summary>
        /// 主题颜色
        /// <para>409EFF</para>
        /// </summary>
        public new static Color Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    var old = _color;
                    _color = value;
                    ColorChanged?.Invoke(old);
                }
            }
        }

        /// <summary>
        /// 主题前景色事件
        /// </summary>
        public static event Action<Color> ForegroundChanged;
        private static Color _foreground = Color.FromArgb(255, 35, 32, 25);
        /// <summary>
        /// 前景色
        /// </summary>
        public static Color Foreground
        {
            get { return _foreground; }
            set
            {
                if (_foreground != value)
                {
                    var old = _foreground;
                    _foreground = value;
                    ForegroundChanged?.Invoke(old);
                }
            }
        }

        /// <summary>
        /// 主题背景色事件
        /// </summary>
        public static event Action<Color> BackgroundChanged;
        private static Color background = Color.FromRgb(247, 249, 251);
        /// <summary>
        /// 背景色
        /// </summary>
        public static Color Background
        {
            get { return background; }
            set
            {
                if (background != value)
                {
                    var old = background;
                    background = value;
                    BackgroundChanged?.Invoke(old);
                }
            }
        }

        /// <summary>
        /// 成功色
        /// <para>2DB84D</para>
        /// </summary>
        public new static Color Success { get; set; } = Color.FromArgb(255, 45, 184, 77);
        /// <summary>
        /// 警告色
        /// <para>FF8900</para>
        /// </summary>
        public new static Color Warn { get; set; } = Color.FromArgb(255, 255, 137, 0);
        /// <summary>
        /// 错误色
        /// <para>F8331E</para>
        /// </summary>
        public new static Color Error { get; set; } = Color.FromArgb(255, 248, 51, 30);
        /// <summary>
        /// 禁用色
        /// <para>DimGray 105,105,105 #696969</para>
        /// </summary>
        public new static Color Disable { get; set; } = Colors.DimGray;

        #endregion
    }
}
