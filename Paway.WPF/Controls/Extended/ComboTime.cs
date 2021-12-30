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
            DependencyProperty.RegisterAttached(nameof(TimeType), typeof(TimePickerType), typeof(ComboTime), new UIPropertyMetadata(OnValueChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty HourProperty =
            DependencyProperty.RegisterAttached(nameof(Hour), typeof(int), typeof(ComboTime), new UIPropertyMetadata(OnValueChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty MinuteProperty =
            DependencyProperty.RegisterAttached(nameof(Minute), typeof(int), typeof(ComboTime), new UIPropertyMetadata(OnValueChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty SecondProperty =
            DependencyProperty.RegisterAttached(nameof(Second), typeof(int), typeof(ComboTime), new UIPropertyMetadata(OnValueChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.RegisterAttached(nameof(Time), typeof(DateTime), typeof(ComboTime), new UIPropertyMetadata(DateTime.Now, OnValueChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TimesProperty =
            DependencyProperty.RegisterAttached(nameof(Times), typeof(string), typeof(ComboTime));
        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ComboTime cbxTime)
            {
                switch (cbxTime.TimeType)
                {
                    case TimePickerType.Hour: cbxTime.Times = $"{cbxTime.Time.Hour:D2}"; break;
                    case TimePickerType.HourMinute: cbxTime.Times = $"{cbxTime.Time.Hour:D2}:{cbxTime.Time.Minute:D2}"; break;
                    case TimePickerType.Time: cbxTime.Times = $"{cbxTime.Time.Hour:D2}:{cbxTime.Time.Minute:D2}:{cbxTime.Time.Second:D2}"; break;
                }
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
        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
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

        #endregion

        /// <summary>
        /// 时
        /// </summary>
        public List<ListViewItemModel> HourList = new List<ListViewItemModel>();
        /// <summary>
        /// 分
        /// </summary>
        public List<ListViewItemModel> MinuteList = new List<ListViewItemModel>();
        /// <summary>
        /// 秒
        /// </summary>
        public List<ListViewItemModel> SecondList = new List<ListViewItemModel>();
        /// <summary>
        /// </summary>
        public ComboTime()
        {
            DefaultStyleKey = typeof(ComboTime);
            for (var i = 0; i < 24; i++) HourList.Add(new ListViewItemModel($"{i}") { Id = i });
            for (var i = 0; i < 60; i++) MinuteList.Add(new ListViewItemModel($"{i}") { Id = i });
            for (var i = 0; i < 60; i++) SecondList.Add(new ListViewItemModel($"{i}") { Id = i });
            this.ItemsSource = new List<int> { 1 };
            this.Hour = this.Time.Hour;
            this.Minute = this.Time.Minute;
            this.Second = this.Time.Second;
        }

        #region 属性
        /// <summary>
        /// 选中项列表
        /// </summary>
        [Browsable(false)]
        public ObservableCollection<IComboBoxItem> ChekedItems { get; } = new ObservableCollection<IComboBoxItem>();

        #endregion

        #region 关联选择
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion
    }
}
