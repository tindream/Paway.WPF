
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
    /// Window进度条
    /// </summary>
    public partial class WindowProgress : Window
    {
        /// <summary>
        /// </summary>
        public WindowProgress(string msg = Config.Loading)
        {
            InitializeComponent();
            desc.Text = msg;
        }
        /// <summary>
        /// 自动大小
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (this.Width > desc.ActualWidth) this.Width = desc.ActualWidth;
            base.OnRender(drawingContext);
        }
    }
}
