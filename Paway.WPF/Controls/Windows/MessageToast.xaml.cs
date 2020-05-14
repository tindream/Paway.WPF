
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// MessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MessageToast : Window
    {
        public MessageToast()
        {
            InitializeComponent();
            this.DataContext = new MessageToastModel();
            this.MouseDoubleClick += MessageBoxExtend_MouseDoubleClick;
            this.ShowInTaskbar = false;
            this.Loaded += delegate
            {
                (this.Resources["ShowSb"] as Storyboard).Begin();
            };
        }
        public void Show(string msg, bool iError = false)
        {
            if (iError)
            {
                tb.Foreground = new SolidColorBrush(Color.FromArgb(255, 240, 0, 0));
            }
            (this.DataContext as MessageToastModel).Message = msg;
            this.Show();
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            this.Width = border1.ActualWidth + 2;
            this.Height = border1.ActualHeight + 2;
            this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2;
            this.Top = (SystemParameters.WorkArea.Height - this.Height) * 2 / 3;
            base.OnRender(drawingContext);
        }
        private void MessageBoxExtend_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText((this.DataContext as MessageToastModel).Message);
        }
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
