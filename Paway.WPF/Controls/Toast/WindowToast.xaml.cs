
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
    /// 自定义消息框-Toast
    /// </summary>
    public partial class WindowToast : WindowEXT
    {
        private bool iFirst = true;

        /// <summary>
        /// </summary>
        public WindowToast()
        {
            InitializeComponent();
            this.DataContext = new WindowToastModel();
            this.MouseDoubleClick += MessageBoxExtend_MouseDoubleClick;
            this.ShowInTaskbar = false;
            this.Loaded += delegate
            {
                (this.Resources["showSb"] as Storyboard).Begin();
            };
        }
        /// <summary>
        /// 显示
        /// </summary>
        public void Show(string msg, int time = 0, bool iError = false)
        {
            if (iError)
            {
                border1.Background = new SolidColorBrush(Color.FromArgb(255, 221, 51, 51));
            }
            var model = this.DataContext as WindowToastModel;
            model.Message = msg;
            if (time != 0) model.Time = KeyTime.FromTimeSpan(new TimeSpan(0, 0, time));
            this.Show();
        }
        /// <summary>
        /// 自动大小、位置
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (iFirst)
            {
                iFirst = false;

                var left = this.Width - border1.ActualWidth - 3;
                var top = this.Height - border1.ActualHeight - 3;
                this.Width = border1.ActualWidth + 3;
                this.Height = border1.ActualHeight + 3;
                if (this.Owner == null)
                {
                    this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2;
                    this.Top = (SystemParameters.WorkArea.Height - this.Height) * 4 / 5;
                }
                else
                {
                    this.Left += left / 2;
                    this.Top += top / 2 + (this.Owner.ActualHeight - this.Height) * 2 / 5;
                }
            }
            base.OnRender(drawingContext);
        }
        private void MessageBoxExtend_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText((this.DataContext as WindowToastModel).Message);
        }
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
