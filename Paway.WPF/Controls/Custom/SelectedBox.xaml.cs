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

namespace Paway.WPF
{
    /// <summary>
    /// 选择框
    /// </summary>
    public partial class SelectedBox : BorderControl
    {
        /// <summary>
        /// 值更新时引发抛出事件
        /// </summary>
        public event Action<string, bool> ValueChangeEvent;
        private bool _value;
        /// <summary>
        /// 当前选择值
        /// </summary>
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
                        cbxImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Paway.WPF;component/Images/selectedBox_checked.png"));
                    }
                    else
                    {
                        cbxImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Paway.WPF;component/Images/selectedBox.png"));
                    }
                    ValueChangeEvent?.Invoke(this.Name, value);
                }
            }
        }
        /// <summary>
        /// 显示的内容
        /// </summary>
        public string Title
        {
            get { return this.cbxTitle.Text; }
            set { this.cbxTitle.Text = value; }
        }
        /// <summary>
        /// </summary>
        public SelectedBox()
        {
            InitializeComponent();
            this.FontSize = new ThemeMonitor().FontSize;
            cbxImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Paway.WPF;component/Images/selectedBox.png"));
        }
        /// <summary>
        /// </summary>
        protected override void ClickHandle(object sender)
        {
            Value = !Value;
            base.ClickHandle(sender);
        }
    }
}
