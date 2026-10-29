using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Paway.Test
{
    /// <summary>
    /// TestImage.xaml 的交互逻辑
    /// </summary>
    public partial class TestImage : Page
    {
        public TestImage()
        {
            InitializeComponent();
        }
        private void SelectImage(object sender, RoutedEventArgs e)
        {
            if (PMethod.OpenFile(out string file, null, "All Image|*.jpg;*.png;*.bmp|Jpeg|*.jpg|Png|*.png|BMP|*.bmp"))
            {
                image.Source = file.ToSource();
                image.Title = System.IO.Path.GetFileName(file);
            }
        }
    }
}
