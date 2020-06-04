
using Paway.Helper;
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
    public partial class WindowProgress : WindowEXT
    {
        private bool iFirst = true;
        /// <summary>
        /// </summary>
        public WindowProgress(string msg = TConfig.Loading)
        {
            InitializeComponent();
            desc.Text = msg;
        }
        /// <summary>
        /// 自动大小
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (iFirst)
            {
                iFirst = false;
                var left = desc.ActualWidth;
                if (left < this.MinWidth) left = this.MinWidth;
                left = this.Width - left;
                if (this.Width > desc.ActualWidth) this.Width = desc.ActualWidth;
                if (this.Owner == null)
                {
                    this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2;
                    this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2;
                }
                else
                {
                    this.Left += left / 2;
                }
            }
            base.OnRender(drawingContext);
        }
    }
}
