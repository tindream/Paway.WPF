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
    public class ComboDate : ComboBoxEXT
    {
        /// <summary>
        /// </summary>
        public ComboDate()
        {
            DefaultStyleKey = typeof(ComboDate);
        }

        #region 关联选择
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Template.FindName("PART_Calendar", this) is Calendar calendar)
            {
                if (this.SelectedValue is DateTime date) calendar.SelectedDate = date;
                calendar.SelectedDatesChanged -= Calendar_SelectedDatesChanged;
                calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
            }
        }
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is Calendar calendar)
            {
                this.SelectedValue = calendar.SelectedDate;
            }
            this.IsDropDownOpen = false;
        }

        #endregion
    }
}
