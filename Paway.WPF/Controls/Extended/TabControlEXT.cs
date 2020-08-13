using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// TabControl扩展
    /// </summary>
    public partial class TabControlEXT : TabControl
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(TabControlEXT),
                new PropertyMetadata(new BrushEXT(Colors.Transparent, Color.FromArgb(205, Config.Color.R, Config.Color.G, Config.Color.B))));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义项背景色
        /// </summary>
        [Category("扩展")]
        [Description("自定义项背景色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public TabControlEXT()
        {
            DefaultStyleKey = typeof(TabControlEXT);
        }
    }
}
