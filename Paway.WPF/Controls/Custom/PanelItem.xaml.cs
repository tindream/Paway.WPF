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
    public partial class PanelItem : BorderControlBase
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
                        lbName.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    }
                    else
                    {
                        border.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                        lbName.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
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
        protected override void BlueHandle(string borderName)
        {
            lbName.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            base.BlueHandle(borderName);
        }
        protected override void Trans_MouseLeave(object sender, InputEventArgs e)
        {
            if (Selected) return;
            base.Trans_MouseLeave(sender, e);
            lbName.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        }
    }
}
