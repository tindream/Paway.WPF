using CommunityToolkit.Mvvm.Messaging;
using Paway.WPF;
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
using System.Windows.Shapes;

namespace Paway.Test
{
    /// <summary>
    /// TipWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TipWindow : WindowEXT
    {
        public TipWindow()
        {
            Config.Window = this;
            InitializeComponent();
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            WeakReferenceMessenger.Default.Send(new TipLoadMessage() { Obj = this });
            var desktopWorkingArea = SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Width - this.ActualWidth - 50;
            this.Top = 100;
        }
    }
}
