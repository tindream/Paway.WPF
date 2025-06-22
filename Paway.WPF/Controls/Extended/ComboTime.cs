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
            DependencyProperty.RegisterAttached(nameof(TimeType), typeof(TimePickerType), typeof(ComboTime), new PropertyMetadata(TimePickerType.HourMinute));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty HourProperty =
            DependencyProperty.RegisterAttached(nameof(Hour), typeof(int), typeof(ComboTime), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty MinuteProperty =
            DependencyProperty.RegisterAttached(nameof(Minute), typeof(int), typeof(ComboTime), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty SecondProperty =
            DependencyProperty.RegisterAttached(nameof(Second), typeof(int), typeof(ComboTime), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TimesProperty =
            DependencyProperty.RegisterAttached(nameof(Times), typeof(string), typeof(ComboTime));

        #endregion

        #region 扩展
        /// <summary>
        /// 选择样式
        /// <para>默认值：HourMinute</para>
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
        /// <para>默认值：0</para>
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
        /// <para>默认值：0</para>
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
        /// <para>默认值：0</para>
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
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("时间")]
        public string Times
        {
            get { return (string)GetValue(TimesProperty); }
            set { SetValue(TimesProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ComboTime()
        {
            DefaultStyleKey = typeof(ComboTime);
            DependencyPropertyDescriptor.FromProperty(TimeTypeProperty, typeof(ComboTime)).AddValueChanged(this, OnValueChanged);
            DependencyPropertyDescriptor.FromProperty(HourProperty, typeof(ComboTime)).AddValueChanged(this, OnValueChanged);
            DependencyPropertyDescriptor.FromProperty(MinuteProperty, typeof(ComboTime)).AddValueChanged(this, OnValueChanged);
            DependencyPropertyDescriptor.FromProperty(SecondProperty, typeof(ComboTime)).AddValueChanged(this, OnValueChanged);
            DependencyPropertyDescriptor.FromProperty(SelectedValueProperty, typeof(ComboTime)).AddValueChanged(this, OnSelectedValueValueChanged);
        }
        private void OnSelectedValueValueChanged(object sender, EventArgs e)
        {
            if (this.SelectedValue is DateTime time)
            {
                LoadTimes(this);
                switch (this.TimeType)
                {
                    case TimePickerType.Hour: this.Hour = time.Hour; break;
                    case TimePickerType.HourMinute: this.Hour = time.Hour; this.Minute = time.Minute; break;
                    case TimePickerType.Time: this.Hour = time.Hour; this.Minute = time.Minute; this.Second = time.Second; break;
                }
            }
        }
        private void OnValueChanged(object sender, EventArgs e)
        {
            if (this.SelectedValue is DateTime time)
            {
                LoadTimes(this);
                this.SelectedValue = new DateTime(time.Year, time.Month, time.Day, this.Hour, this.Minute, this.Second);
            }
        }
        private void LoadTimes(ComboTime cbxTime)
        {
            switch (cbxTime.TimeType)
            {
                case TimePickerType.Hour: cbxTime.Times = $"{cbxTime.Hour:D2}"; break;
                case TimePickerType.HourMinute: cbxTime.Times = $"{cbxTime.Hour:D2}:{cbxTime.Minute:D2}"; break;
                case TimePickerType.Time: cbxTime.Times = $"{cbxTime.Hour:D2}:{cbxTime.Minute:D2}:{cbxTime.Second:D2}"; break;
            }
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
            if (Template.FindName("listHour", this) is ListBoxEXT listHour)
            {
                listHour.MouseDoubleClick -= ListBox_MouseDoubleClick;
                listHour.MouseDoubleClick += ListBox_MouseDoubleClick;
            }
            if (Template.FindName("listMinute", this) is ListBoxEXT listMinute)
            {
                listMinute.MouseDoubleClick -= ListBox_MouseDoubleClick;
                listMinute.MouseDoubleClick += ListBox_MouseDoubleClick;
            }
            if (Template.FindName("listSecond", this) is ListBoxEXT listSecond)
            {
                listSecond.MouseDoubleClick -= ListBox_MouseDoubleClick;
                listSecond.MouseDoubleClick += ListBox_MouseDoubleClick;
            }
        }
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.IsDropDownOpen = false;
        }
        private void Popup_Opened(object sender, EventArgs e)
        {
            var count = 150 / this.ActualHeight;
            if (Template.FindName("listHour", this) is ListBoxEXT listHour)
            {
                listHour.ScrollViewer(this.Hour / (24.0 - count));
            }
            if (Template.FindName("listMinute", this) is ListBoxEXT listMinute)
            {
                listMinute.ScrollViewer(this.Minute / (60.0 - count));
            }
            if (Template.FindName("listSecond", this) is ListBoxEXT listSecond)
            {
                listSecond.ScrollViewer(this.Second / (60.0 - count));
            }
        }
    }
}
