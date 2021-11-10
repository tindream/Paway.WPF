using System;
using System.Collections.Generic;
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
    /// Calendar扩展监听
    /// </summary>
    internal class CalendarMonitor : DependencyObject
    {
        #region 启用监听，获取CalendarItem、CalendarButton属性并设置
        /// <summary>
        /// 启用监听，获取CalendarItem、CalendarButton属性并设置
        /// </summary>
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(CalendarMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is CalendarItem calendarItem)
            {
                if (calendarItem.TemplatedParent is CalendarEXT calendar)
                {
                    calendarItem.SetValue(HasButtonProperty, calendar.HasButton);
                }
            }
            else if (obj is CalendarButton calendarButton)
            {
                if (calendarButton.Parent != null)
                { }
                if (calendarButton.TemplatedParent != null)
                { }
                if (calendarButton.TemplatedParent is CalendarEXT calendar)
                {
                    calendarButton.SetValue(ItemBrushProperty, calendar.ItemBrush);
                }
            }
            else
            { }
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
        /// </summary>
        public static readonly DependencyProperty HasButtonProperty =
            DependencyProperty.RegisterAttached(nameof(HasButton), typeof(bool), typeof(CalendarMonitor));
        /// <summary>
        /// 启用扩展按钮
        /// </summary>
        public bool HasButton
        {
            get { return (bool)GetValue(HasButtonProperty); }
            set { SetValue(HasButtonProperty, value); }
        }
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(CalendarMonitor),
                new PropertyMetadata(new BrushEXT()));
        /// <summary>
        /// 自定义项背景色
        /// </summary>
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }

        #endregion
    }
}
