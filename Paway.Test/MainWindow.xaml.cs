using Paway.Helper;
using Paway.Test.ViewModel;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowEXT
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool b;
        private Path pathNew;
        private object pathCurrent;
        private void ButtonEXT_Click(object sender, RoutedEventArgs e)
        {
            var r = Validation.GetHasError(tb);
            Method.Toast(this, r);
            var xml = Method.GetTemplateXaml(dp);
            //Method.Toast(this, xml);
            b = !b;
            if (b) Method.Progress(this);
            else Method.Hide();
            if (this.pathCurrent == null)
            {
                this.pathCurrent = transition.Content;
                this.pathNew = new Path
                {
                    Style = this.FindResource("PathRound") as Style
                };
            }
            transition.TransitionType = (TransitionType)new Random().Next(0, 5);
            //transition.TransitionType = TransitionType.Left;
            if (b) transition.Content = this.pathNew;
            else transition.Content = this.pathCurrent;
        }
    }
}
