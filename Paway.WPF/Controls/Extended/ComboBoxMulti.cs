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
    /// ComboBox扩展多选
    /// </summary>
    public class ComboBoxMulti : ComboBoxEXT
    {
        /// <summary>
        /// </summary>
        public ComboBoxMulti()
        {
            DefaultStyleKey = typeof(ComboBoxMulti);
        }

        #region 属性
        /// <summary>
        /// 选中项列表
        /// </summary>
        [Browsable(false)]
        public ObservableCollection<IComboBoxMulti> ChekedItems { get; } = new ObservableCollection<IComboBoxMulti>();

        #endregion

        #region 关联选择
        /// <summary>
        /// ListBox竖向列表
        /// </summary>
        private ListBox _ListBoxV;
        /// <summary>
        /// ListBox横向列表
        /// </summary>
        private ListBox _ListBoxH;
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _ListBoxV = Template.FindName("PART_ListBox", this) as ListBox;
            _ListBoxH = Template.FindName("PART_ListBoxChk", this) as ListBox;
            _ListBoxH.ItemsSource = ChekedItems;
            _ListBoxV.SelectionChanged += ListBoxV_SelectionChanged;
            _ListBoxH.SelectionChanged += ListBoxH_SelectionChanged;
            if (ItemsSource != null)
            {
                foreach (var item in ItemsSource)
                {
                    if (item is IComboBoxMulti multi)
                    {
                        if (multi.IsChecked)
                        {
                            _ListBoxV.SelectedItems.Add(multi);
                        }
                    }
                }
            }
        }
        private void ListBoxH_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.RemovedItems)
            {
                if (item is IComboBoxMulti multi)
                {
                    for (int i = 0; i < _ListBoxV.SelectedItems.Count; i++)
                    {
                        if (_ListBoxV.SelectedItems[i] is IComboBoxMulti temp)
                        {
                            if (temp.Id == multi.Id)
                            {
                                _ListBoxV.SelectedItems.Remove(_ListBoxV.SelectedItems[i]);
                            }
                        }
                    }
                }

            }
        }
        private void ListBoxV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                if (item is IComboBoxMulti multi)
                {
                    multi.IsChecked = true;
                    if (ChekedItems.IndexOf(multi) < 0)
                    {
                        ChekedItems.Add(multi);
                    }
                }
            }

            foreach (var item in e.RemovedItems)
            {
                if (item is IComboBoxMulti multi)
                {
                    multi.IsChecked = false;
                    ChekedItems.Remove(multi);
                }
            }
        }

        #endregion
    }
}
