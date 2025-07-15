using Paway.Helper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Paway.WPF
{
    /// <summary>
    /// 虚拟键盘-数字键盘
    /// </summary>
    public partial class KeyboardNum : ContentControl, IWindowAdorner
    {
        /// <summary>
        /// 关闭路由事件
        /// </summary>
        public event EventHandler<RoutedEventArgs> CloseEvent;
        /// <summary>
        /// 点击空白处拖动窗体后事件
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> DragMovedEvent;

        /// <summary>
        /// 虚拟键盘-数字键盘
        /// </summary>
        public KeyboardNum()
        {
            DefaultStyleKey = typeof(KeyboardNum);
        }
        /// <summary>
        /// 获取模板控件
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var listview1 = Template.FindName("listview1", this) as ListViewCustom;
            var listview2 = Template.FindName("listview2", this) as ListViewCustom;
            listview1.SelectionChanged += Listview_SelectionChanged;
            listview2.SelectionChanged += Listview_SelectionChanged;
            listview1.DragMovedEvent += Listview_DragMovedEvent;
            listview2.DragMovedEvent += Listview_DragMovedEvent;
        }
        private void Listview_DragMovedEvent(object sender, MouseButtonEventArgs e)
        {
            DragMovedEvent?.Invoke(sender, e);
        }
        private void Listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListViewCustom listView && listView.SelectedItem is ListBoxItemEXT viewItem)
            {
                var tag = viewItem.Tag.ToString();
                switch (tag)
                {
                    case "backspace": KeyboardHelper.Send(Keys.Back); break;
                    case "enter":
                        KeyboardHelper.Send(Keys.Enter);
                        this.OnCloseEvent();
                        break;
                    case ".": KeyboardHelper.Send(Keys.Decimal); break;
                    case "-": KeyboardHelper.Send(Keys.Subtract); break;
                    default: KeyboardHelper.Send((Keys)(Convert.ToInt32(tag) + 48)); break;
                }
                listView.SelectedIndex = -1;
            }
        }
        private void OnCloseEvent()
        {
            if (CloseEvent != null) CloseEvent?.Invoke(this, new RoutedEventArgs());
            else this.Visibility = Visibility.Collapsed;
        }
    }
}
