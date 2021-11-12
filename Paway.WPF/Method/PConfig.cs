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
    public class PConfig : TConfig
    {
        static PConfig()
        {
            FontAwesome = new FontFamily(new Uri(@"pack://application:,,,/Paway.WPF;component/Resource/"), "./#fontawesome");
        }

        #region 字体
        /// <summary>
        /// QuartZ字体
        /// </summary>
        public static FontFamily FontAwesome { get; private set; }

        #endregion

        #region 主题
        /// <summary>
        /// 主题字体大小变化事件
        /// </summary>
        public static event Action<double> FontSizeChanged;
        private static double fontSize = 15d;
        /// <summary>
        /// 主题字体大小
        /// </summary>
        public static double FontSize
        {
            get { return fontSize; }
            set
            {
                if (value < 1) return;
                if (fontSize != value)
                {
                    var old = fontSize;
                    fontSize = value;
                    FontSizeChanged?.Invoke(old);
                }
            }
        }

        /// <summary>
        /// 主题颜色变化事件
        /// </summary>
        public static event Action<Color> ColorChanged;
        private static Color color = Color.FromArgb(255, 64, 158, 255);
        /// <summary>
        /// 主题颜色
        /// </summary>
        public static Color Color
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    var old = color;
                    color = value;
                    ColorChanged?.Invoke(old);
                }
            }
        }

        /// <summary>
        /// 主题窗体背景色事件
        /// </summary>
        public static event Action<Color> BackgroundChanged;
        private static Color background = Color.FromArgb(255, 204, 213, 240);
        /// <summary>
        /// 窗体背景色
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
        /// 颜色的 Alpha 通道默认值
        /// </summary>
        internal const int Alpha = 200;
        /// <summary>
        /// 颜色差量
        /// </summary>
        internal const int Interval = 40;

        /// <summary>
        /// 主题深色
        /// </summary>
        public static Color High { get; private set; } = Color.AddLight(-90);

        /// <summary>
        /// 成功色
        /// </summary>
        public static Color Success { get; set; } = Color.FromArgb(255, 45, 184, 77);
        /// <summary>
        /// 警告色
        /// </summary>
        public static Color Warn { get; set; } = Color.FromArgb(255, 255, 153, 0);
        /// <summary>
        /// 错误色
        /// </summary>
        public static Color Error { get; set; } = Color.FromArgb(255, 248, 51, 30);

        /// <summary>
        /// 文本
        /// </summary>
        public static Color TextColor { get; private set; } = Color.FromArgb(255, 34, 34, 34);
        /// <summary>
        /// 文本(二级浅色)
        /// </summary>
        public static Color TextSub { get; private set; } = Color.FromArgb(255, 68, 68, 68);
        /// <summary>
        /// 文本(三级浅色)
        /// </summary>
        public static Color TextLight { get; private set; } = Color.FromArgb(255, 119, 119, 119);

        /// <summary>
        /// 浅色
        /// </summary>
        public static Color Light { get; private set; } = Color.FromArgb(255, 242, 246, 252);

        #endregion
    }
}
