using System;
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
                else if (calendarItem.TemplatedParent is Calendar calendar1)
                {
                    if (calendar1.Parent is FrameworkElement element)
                    {
                        if (element.TemplatedParent is DatePickerEXT datePicker)
                        {
                            calendarItem.SetValue(HasButtonProperty, datePicker.HasButton);
                        }
                    }
                }
            }
            else if (obj is CalendarButton calendarButton)
            {
                ButtonBrush(calendarButton);
            }
            else if (obj is CalendarDayButton calendarDayButton)
            {
                ButtonBrush(calendarDayButton);
            }
        }
        private static void ButtonBrush(Button buttom)
        {
            buttom.Loaded += delegate
            {
                if (buttom.Parent is FrameworkElement element)
                {
                    if (element.TemplatedParent is CalendarItem calendarItem1)
                    {
                        if (calendarItem1.TemplatedParent is CalendarEXT calendar)
                        {
                            buttom.SetValue(ItemBrushProperty, calendar.ItemBrush);
                        }
                        else if (calendarItem1.TemplatedParent is Calendar calendar1)
                        {
                            if (calendar1.Parent is FrameworkElement element1)
                            {
                                if (element1.TemplatedParent is DatePickerEXT datePicker)
                                {
                                    buttom.SetValue(ItemBrushProperty, datePicker.ItemBrush);
                                }
                            }
                        }
                    }
                }
            };
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
        /// 启用扩展按钮
        /// <para>默认值：false</para>
        /// </summary>
        public static readonly DependencyProperty HasButtonProperty =
            DependencyProperty.RegisterAttached(nameof(HasButton), typeof(bool), typeof(CalendarMonitor));
        /// <summary>
        /// 启用扩展按钮
        /// <para>默认值：true</para>
        /// </summary>
        public bool HasButton
        {
            get { return (bool)GetValue(HasButtonProperty); }
            set { SetValue(HasButtonProperty, value); }
        }

        /// <summary>
        /// 自定义项背景色
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(CalendarMonitor),
                new PropertyMetadata(new BrushEXT()));
        /// <summary>
        /// 自定义项背景色
        /// <para>默认值：默认</para>
        /// </summary>
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }

        #endregion
    }
}
