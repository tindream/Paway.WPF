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
    /// ComboBox扩展选择日期
    /// </summary>
    public class ComboBoxDate : ComboBoxEXT
    {
        /// <summary>
        /// </summary>
        public ComboBoxDate()
        {
            DefaultStyleKey = typeof(ComboBoxDate);
        }

        #region 关联选择
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var calendar = Template.FindName("PART_Calendar", this) as Calendar;
            if (this.SelectedValue is DateTime date) calendar.SelectedDate = date;
            calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
        }
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is Calendar calendar)
            {
                if (calendar.SelectedDate != null) this.SelectedValue = calendar.SelectedDate.Value.ToString("d");
                else this.SelectedValue = null;
            }
            this.IsDropDownOpen = false;
        }

        #endregion
    }
}
