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
    public class ComboMulti : ComboBoxEXT
    {
        /// <summary>
        /// </summary>
        public ComboMulti()
        {
            DefaultStyleKey = typeof(ComboMulti);
        }

        #region 属性
        /// <summary>
        /// 选中项列表
        /// </summary>
        [Browsable(false)]
        public ObservableCollection<IComboMulti> ChekedItems { get; } = new ObservableCollection<IComboMulti>();

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
            _ListBoxV.SelectionChanged -= ListBoxV_SelectionChanged;
            _ListBoxV.SelectionChanged += ListBoxV_SelectionChanged;
            _ListBoxH.SelectionChanged -= ListBoxH_SelectionChanged;
            _ListBoxH.SelectionChanged += ListBoxH_SelectionChanged;
            if (ItemsSource != null)
            {
                foreach (var item in ItemsSource)
                {
                    if (item is IComboMulti multi)
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
                if (item is IComboMulti multi)
                {
                    for (int i = 0; i < _ListBoxV.SelectedItems.Count; i++)
                    {
                        if (_ListBoxV.SelectedItems[i] is IComboMulti temp)
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
                if (item is IComboMulti multi)
                {
                    multi.IsChecked = true;
                    if (ChekedItems.IndexOf(multi) < 0)
                    {
                        var value = _ListBoxV.Items.IndexOf(multi);
                        var index = 0;
                        for (var i = 0; i < ChekedItems.Count; i++)
                        {
                            var j = _ListBoxV.Items.IndexOf(ChekedItems[i]);
                            if (j < value) index = i + 1;
                        }
                        ChekedItems.Insert(index, multi);
                    }
                }
            }

            foreach (var item in e.RemovedItems)
            {
                if (item is IComboMulti multi)
                {
                    multi.IsChecked = false;
                    ChekedItems.Remove(multi);
                }
            }
        }

        #endregion
    }
}
