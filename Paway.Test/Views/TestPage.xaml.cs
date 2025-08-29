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
    /// TestPage.xaml 的交互逻辑
    /// </summary>
    public partial class TestPage : Page
    {
        public TestPage()
        {
            InitializeComponent();
        }
        private void ButtonEXT_Click(object sender, RoutedEventArgs e)
        {
            if (PMethod.OpenFile(out string file, null, "All Image|*.jpg;*.png;*.bmp|Jpeg|*.jpg|Png|*.png|BMP|*.bmp"))
            {
                image.Source = file.ToSource();
                image2.Source = file.ToSource();
            }
        }
        private void ButtonEXT_Click_1(object sender, RoutedEventArgs e)
        {
            "今夕是何年".Hit(this);
        }
    }
}
