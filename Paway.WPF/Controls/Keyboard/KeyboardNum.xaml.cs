using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Keys = System.Windows.Forms.Keys;

namespace Paway.WPF
{
    /// <summary>
    /// KeyboardNum.xaml 的交互逻辑
    /// </summary>
    public partial class KeyboardNum : UserControl
    {
        /// <summary>
        /// 关闭事件
        /// </summary>
        public event Action CloseEvent;

        /// <summary>
        /// 软键盘-数字键盘
        /// </summary>
        public KeyboardNum()
        {
            InitializeComponent();
            listview1.SelectionChanged += Listview_SelectionChanged;
            listview2.SelectionChanged += Listview_SelectionChanged;
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
            if (CloseEvent != null) CloseEvent.Invoke();
            else this.Visibility = Visibility.Collapsed;
        }
    }
}
