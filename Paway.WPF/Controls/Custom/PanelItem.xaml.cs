using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// BookClass.xaml 的交互逻辑
    /// </summary>
    public partial class PanelItem : BorderControl
    {
        public event Action<PanelItem> SelectedEvent;

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    if (value)
                    {
                        border.Background = new SolidColorBrush(Color.FromArgb(200, 35, 175, 255));
                        lbName.Foreground = new SolidColorBrush(Colors.White);
                    }
                    else
                    {
                        border.Background = new SolidColorBrush(Colors.Transparent);
                        lbName.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    if (value) SelectedEvent?.Invoke(this);
                }
            }
        }
        public string Text
        {
            get { return lbName.Text; }
            set { lbName.Text = value; }
        }
        public string Desc
        {
            get { return lbDesc.Text; }
            set { lbDesc.Text = value; }
        }
        public PanelItem()
        {
            InitializeComponent();
        }
        public PanelItem(string text, string desc = null, object tag = null) : this()
        {
            this.Text = text;
            this.Desc = desc;
            this.Tag = tag;
        }
    }
}
