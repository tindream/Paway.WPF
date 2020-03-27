using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Invengo.Utils
{
    /// <summary>
    /// CheckBoxRound.xaml 的交互逻辑
    /// </summary>
    public partial class CheckBoxRound : BorderControlBase
    {
        public event Action<string, bool> ValueChangeEvent;
        private bool _value;
        public bool Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    if (value)
                    {
                        cbxImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Invengo.Utils;component/Images/checkBox_checked.png"));
                    }
                    else
                    {
                        cbxImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Invengo.Utils;component/Images/checkBox.png"));
                    }
                    ValueChangeEvent?.Invoke(this.Name, value);
                }
            }
        }
        public string Title
        {
            get { return this.cbxTitle.Text; }
            set { this.cbxTitle.Text = value; }
        }
        public CheckBoxRound()
        {
            InitializeComponent();
        }
        protected override void ClickHandle(object sender)
        {
            if (sender is Border border)
            {
                switch (border.Name)
                {
                    case "cbxBorder":
                        Value = !Value;
                        break;
                }
            }
            base.ClickHandle(sender);
        }
    }
}
