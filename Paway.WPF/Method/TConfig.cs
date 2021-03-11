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
    public class TConfig : Paway.Helper.TConfig
    {
        static TConfig()
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
        private static Color color = Color.FromArgb(255, 35, 175, 255);
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

        #endregion
    }
}
