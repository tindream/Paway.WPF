using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// ComboBox扩展时间选择器
    /// </summary>
    public class ComboTime : ComboBoxEXT
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TimeTypeProperty =
            DependencyProperty.RegisterAttached(nameof(TimeType), typeof(TimePickerType), typeof(ComboTime), new UIPropertyMetadata(TimePickerType.HourMinute, OnValueChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty HourProperty =
            DependencyProperty.RegisterAttached(nameof(Hour), typeof(int), typeof(ComboTime), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty MinuteProperty =
            DependencyProperty.RegisterAttached(nameof(Minute), typeof(int), typeof(ComboTime), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty SecondProperty =
            DependencyProperty.RegisterAttached(nameof(Second), typeof(int), typeof(ComboTime), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TimesProperty =
            DependencyProperty.RegisterAttached(nameof(Times), typeof(string), typeof(ComboTime));
        /// <summary>
        /// </summary>
        public static readonly new DependencyProperty SelectedValueProperty =
            DependencyProperty.RegisterAttached(nameof(SelectedValue), typeof(object), typeof(ComboTime), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ComboTime cbxTime && cbxTime.SelectedValue is DateTime time)
            {
                LoadTimes(cbxTime);
                if (e.Property.Name == nameof(SelectedValue))
                {
                    switch (cbxTime.TimeType)
                    {
                        case TimePickerType.Hour: cbxTime.Hour = time.Hour; break;
                        case TimePickerType.HourMinute: cbxTime.Hour = time.Hour; cbxTime.Minute = time.Minute; break;
                        case TimePickerType.Time: cbxTime.Hour = time.Hour; cbxTime.Minute = time.Minute; cbxTime.Second = time.Second; break;
                    }
                }
                else
                {
                    cbxTime.SelectedValue = new DateTime(time.Year, time.Month, time.Day, cbxTime.Hour, cbxTime.Minute, cbxTime.Second);
                }
            }
        }
        private static void LoadTimes(ComboTime cbxTime)
        {
            switch (cbxTime.TimeType)
            {
                case TimePickerType.Hour: cbxTime.Times = $"{cbxTime.Hour:D2}"; break;
                case TimePickerType.HourMinute: cbxTime.Times = $"{cbxTime.Hour:D2}:{cbxTime.Minute:D2}"; break;
                case TimePickerType.Time: cbxTime.Times = $"{cbxTime.Hour:D2}:{cbxTime.Minute:D2}:{cbxTime.Second:D2}"; break;
            }
        }

        #endregion

        #region 扩展
        /// <summary>
        /// 选择样式
        /// </summary>
        [Category("扩展")]
        [Description("选择样式")]
        public TimePickerType TimeType
        {
            get { return (TimePickerType)GetValue(TimeTypeProperty); }
            set { SetValue(TimeTypeProperty, value); }
        }
        /// <summary>
        /// 时
        /// </summary>
        [Category("扩展")]
        [Description("时")]
        public int Hour
        {
            get { return (int)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }
        /// <summary>
        /// 分
        /// </summary>
        [Category("扩展")]
        [Description("分")]
        public int Minute
        {
            get { return (int)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); }
        }
        /// <summary>
        /// 秒
        /// </summary>
        [Category("扩展")]
        [Description("秒")]
        public int Second
        {
            get { return (int)GetValue(SecondProperty); }
            set { SetValue(SecondProperty, value); }
        }
        /// <summary>
        /// 时间
        /// </summary>
        [Category("扩展")]
        [Description("时间")]
        public string Times
        {
            get { return (string)GetValue(TimesProperty); }
            set { SetValue(TimesProperty, value); }
        }
        /// <summary>
        /// 重写
        /// </summary>
        public new object SelectedValue
        {
            get { return (object)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ComboTime()
        {
            DefaultStyleKey = typeof(ComboTime);
        }
        /// <summary>
        /// 滚动到指定位置
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.SelectedValue == null) this.SelectedValue = DateTime.Now;
            if (Template.FindName("PART_Popup", this) is Popup popup)
            {
                popup.Opened -= Popup_Opened;
                popup.Opened += Popup_Opened;
            }
        }
        private void Popup_Opened(object sender, EventArgs e)
        {
            if (Template.FindName("listHour", this) is ListBoxEXT listHour)
            {
                listHour.ScrollViewer(this.Hour / (24.0 - 5.0));
            }
            if (Template.FindName("listMinute", this) is ListBoxEXT listMinute)
            {
                listMinute.ScrollViewer(this.Minute / (60.0 - 5.0));
            }
            if (Template.FindName("listSecond", this) is ListBoxEXT listSecond)
            {
                listSecond.ScrollViewer(this.Second / (60.0 - 5.0));
            }
        }
    }
}
