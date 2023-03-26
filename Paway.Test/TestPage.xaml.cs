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
            var errorList = Method.ValidationError(this);
            if (errorList.Count > 0)
            {
                Method.Hit(this, errorList.Join("\r\n"), ColorType.Error);
                return;
            }
            Method.Progress(this, adorner =>
            {
                adorner.Progress(1);
                Thread.Sleep(500);
                adorner.Text("12");
                adorner.Progress(12);
                Thread.Sleep(500);
                adorner.Text("25");
                adorner.Progress(25);
                Thread.Sleep(500);
                adorner.Text("48");
                adorner.Progress(48);
                Thread.Sleep(500);
                adorner.Text("70");
                adorner.Progress(70);
                Thread.Sleep(500);
                adorner.Text("99");
                adorner.Progress(99);
                Thread.Sleep(500);
            }, iProgressBar: false, iProgressRound: true);
        }
    }
}
