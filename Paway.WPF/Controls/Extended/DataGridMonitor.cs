using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(DataGridMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is DataGridColumnHeader columnHeader)
            {
                if (PMethod.Parent(columnHeader, out DataGridEXT dataGrid))
                {
                    columnHeader.SetValue(RadiusProperty, dataGrid.Radius);
                    columnHeader.SetValue(HeaderBrushProperty, dataGrid.HeaderBrush);
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

        #region 依赖属性
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
                new PropertyMetadata(new BrushEXT() { Normal = new ThemeForeground(PMethod.AlphaColor(200, PConfig.Background), 15) }));
        /// <summary>
        /// 标题列背景颜色
        /// <para>默认值：(200, PConfig.Light), 默认, 默认</para>
        /// </summary>
        public BrushEXT HeaderBrush
        {
            get { return (BrushEXT)GetValue(HeaderBrushProperty); }
            set { SetValue(HeaderBrushProperty, value); }
        }

        #endregion
    }
}
