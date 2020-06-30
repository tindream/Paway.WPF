using System;
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
    /// Calendar扩展
    /// </summary>
    public partial class CalendarEXT : Calendar
    {
        #region 扩展
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty HasButtonProperty =
            DependencyProperty.RegisterAttached(nameof(HasButton), typeof(bool), typeof(CalendarEXT));

        /// <summary>
        /// 启用扩展按钮
        /// </summary>
        [Category("扩展")]
        [Description("启用扩展按钮")]
        public bool HasButton
        {
            get { return (bool)GetValue(HasButtonProperty); }
            set { SetValue(HasButtonProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public CalendarEXT()
        {
            DefaultStyleKey = typeof(CalendarEXT);
        }

        #region 扩展按钮
        /// <summary>
        /// 响应扩展按钮
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Method.Child(this, out CalendarItem item))
            {
                item.Loaded -= Item_Loaded;
                item.Loaded += Item_Loaded;
            }
        }
        private void Item_Loaded(object sender, RoutedEventArgs e)
        {
            if (Method.Child(sender, out ButtonEXT goToday, "PART_GoToday"))
            {
                goToday.Click -= GoToday_Click;
                goToday.Click += GoToday_Click;
            }
            if (Method.Child(sender, out ButtonEXT goClear, "PART_GoClear"))
            {
                goClear.Click -= GoClear_Click;
                goClear.Click += GoClear_Click;
            }
        }
        private void GoClear_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedDate = null;
            this.DisplayDate = DateTime.Now;
        }
        private void GoToday_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedDate = DateTime.Now;
            this.DisplayDate = DateTime.Now;
        }

        #endregion
    }
}
