using Paway.Helper;
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
            MergeThemeResource();
        }
        /// <summary>
        /// 加载全局资源
        /// <para>静态构造时 Application 可能尚未创建，此时本次合并不生效；此方法可在之后重试</para>
        /// </summary>
        internal static void InitResources()
        {
            MergeThemeResource();
        }
        /// <summary>
        /// 合并主题资源字典（幂等）
        /// </summary>
        private static void MergeThemeResource()
        {
            var application = Application.Current;
            if (application == null) return;
            var dictionaries = application.Resources.MergedDictionaries;
            foreach (var item in dictionaries)
            {
                if (item.Source != null && item.Source.ToStrings().Contains("Paway.xaml")) return;
            }
            // 创建资源字典
            var globalResources = new ResourceDictionary
            {
                // 指定资源字典的源（确保"Resources.xaml"的生成操作设置为Page或Resource）
                Source = new Uri("/Paway.WPF;component/Themes/Paway.xaml", UriKind.Relative)
            };
            // 合并到应用程序资源
            dictionaries.Add(globalResources);
        }

        #region 全局配置
        private static Window _window;
        /// <summary>
        /// 主窗体
        /// </summary>
        public static Window Window
        {
            get { return _window; }
            set
            {
                if (ReferenceEquals(_window, value)) return;
                if (_window != null) _window.Closed -= Window_Closed;
                _window = value;
                if (value == null) return;
                value.Closed += Window_Closed;
                Handle = value.Handle();
            }
        }
        private static void Window_Closed(object sender, EventArgs e)
        {
            IClose = true;
        }
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
        /// 主题事件弱引用通道
        /// <para>供库内部按实例创建的转换器类型订阅，避免静态事件强引用导致实例永不回收</para>
        /// </summary>
        internal static readonly WeakEventSource<string> FontFamilyChangedWeak = new WeakEventSource<string>();
        internal static readonly WeakEventSource<double> FontSizeChangedWeak = new WeakEventSource<double>();
        internal static readonly WeakEventSource<Color> ColorChangedWeak = new WeakEventSource<Color>();
        internal static readonly WeakEventSource<Color> ForegroundChangedWeak = new WeakEventSource<Color>();
        internal static readonly WeakEventSource<Color> BackgroundChangedWeak = new WeakEventSource<Color>();

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
                    FontFamilyChangedWeak.Raise(old);
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
                    FontSizeChangedWeak.Raise(old);
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
                    ColorChangedWeak.Raise(old);
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
                    ForegroundChangedWeak.Raise(old);
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
                    BackgroundChangedWeak.Raise(old);
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
        /// 信息色
        /// <para>00BCD4</para>
        /// </summary>
        public new static Color Info { get; set; } = Color.FromArgb(255, 0, 188, 212);
        /// <summary>
        /// 禁用色
        /// <para>DimGray 105,105,105 #696969</para>
        /// </summary>
        public new static Color Disable { get; set; } = Colors.DimGray;

        #endregion
    }
}
