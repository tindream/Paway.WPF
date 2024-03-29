﻿using System;
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
                new PropertyMetadata(new BrushEXT()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty MenuModeProperty =
            DependencyProperty.RegisterAttached(nameof(MenuMode), typeof(bool), typeof(TabControlEXT), new PropertyMetadata(true));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义项背景色
        /// <para>默认值：默认</para>
        /// </summary>
        [Category("扩展")]
        [Description("自定义项背景色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
        /// <summary>
        /// 菜单模式
        /// <para>默认值：true</para>
        /// </summary>
        [Category("扩展")]
        [Description("菜单模式")]
        public bool MenuMode
        {
            get { return (bool)GetValue(MenuModeProperty); }
            set { SetValue(MenuModeProperty, value); }
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
