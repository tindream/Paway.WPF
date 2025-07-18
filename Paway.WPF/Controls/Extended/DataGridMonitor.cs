﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// DataGrid扩展监听
    /// </summary>
    internal class DataGridMonitor : DependencyObject
    {
        #region 启用监听，获取DataGrid属性并设置
        /// <summary>
        /// 启用监听，获取Slider属性并设置到Thumb、RepeatButton
        /// </summary>
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(DataGridMonitor), new PropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is DataGridColumnHeader columnHeader)
            {
                if (PMethod.Parent(columnHeader, out DataGridEXT dataGrid))
                {
                    columnHeader.SetValue(RadiusProperty, dataGrid.Radius);
                    columnHeader.SetValue(HeaderBrushProperty, dataGrid.HeaderBrush);
                    columnHeader.SetValue(HorizontalAlignmentProperty, dataGrid.HorizontalContentAlignment);
                }
            }
        }
        /// <summary>
        /// 启用监听
        /// </summary>
        public bool IsMonitoring
        {
            get { return (bool)GetValue(IsMonitoringProperty); }
            set { SetValue(IsMonitoringProperty, value); }
        }

        #endregion

        #region 依赖属性_自定义边框圆角
        /// <summary>
        /// 自定义边框圆角
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
                DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(DataGridMonitor));
        /// <summary>
        /// 自定义边框圆角
        /// <para>默认值：未设置</para>
        /// </summary>
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        /// <summary>
        /// 标题列背景颜色
        /// </summary>
        public static readonly DependencyProperty HeaderBrushProperty =
            DependencyProperty.RegisterAttached(nameof(HeaderBrush), typeof(BrushEXT), typeof(DataGridMonitor),
                new PropertyMetadata(new BrushEXT() { Normal = new ThemeForeground(PConfig.Background, 15, true) }));
        /// <summary>
        /// 标题列背景颜色
        /// <para>默认值：主题背景, 默认, 默认</para>
        /// </summary>
        public BrushEXT HeaderBrush
        {
            get { return (BrushEXT)GetValue(HeaderBrushProperty); }
            set { SetValue(HeaderBrushProperty, value); }
        }

        #endregion

        #region 依赖属性_标题水平样式
        /// <summary>
        /// 标题水平样式
        /// </summary>
        public static readonly DependencyProperty HorizontalAlignmentProperty =
                DependencyProperty.RegisterAttached(nameof(HorizontalAlignment), typeof(HorizontalAlignment), typeof(DataGridMonitor));
        public HorizontalAlignment HorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalAlignmentProperty); }
            set { SetValue(HorizontalAlignmentProperty, value); }
        }

        #endregion
    }
}
