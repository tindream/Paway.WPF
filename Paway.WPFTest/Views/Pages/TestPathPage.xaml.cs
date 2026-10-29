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
    /// TestPathPage.xaml 的交互逻辑
    /// </summary>
    public partial class TestPathPage : Page
    {
        Path pathRed, pathBlue;
        public TestPathPage()
        {
            InitializeComponent();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.pathBlue = transition.Content as Path;
            this.pathRed = new Path
            {
                Style = this.FindResource("PathRound") as Style,
                Fill = new SolidColorBrush(Colors.Red),
                Width = 28,
            };
        }
        private void ButtonEXT_Click(object sender, RoutedEventArgs e)
        {
            var type = (TransitionType)Method.Random((int)TransitionType.Left, (int)TransitionType.Bottom + 1);
            transition.TransitionType = type;
            if (transition.Content != pathBlue) transition.Content = this.pathBlue;
            else transition.Content = this.pathRed;
        }
    }
}
