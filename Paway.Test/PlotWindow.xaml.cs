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
    /// PlotWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PlotWindow : Window
    {
        public PlotWindow()
        {
            InitializeComponent();
        }
        private void Slider_TrackValueEvent(object sender, WPF.ValueChangeEventArgs e)
        {
            if (e.Value < 0) e.Value = e.Value * 8 / 3;
        }
    }
}
