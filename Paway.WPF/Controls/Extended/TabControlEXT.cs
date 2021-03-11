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
    /// <para>不推荐(样式刷新有问题)</para>
    /// </summary>
    public partial class TabControlEXT : TabControl
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(TabControlEXT),
                new PropertyMetadata(new BrushEXT(null, 205, 250)));

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
        /// 不推荐(样式刷新有问题)
        /// </summary>
        public TabControlEXT()
        {
            DefaultStyleKey = typeof(TabControlEXT);
        }
    }
}
