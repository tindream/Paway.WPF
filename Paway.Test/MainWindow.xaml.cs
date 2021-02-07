using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Media.Animation;
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
        /// <summary>
        /// 故事板
        /// </summary>
        private Storyboard storyboard = new Storyboard();
        private Path pathNew;
        private object pathCurrent;

        public MainWindow()
        {
            InitializeComponent();
            Animation(e1);
            Animation(e2, 750);
            Animation(e3, 1500);
            Animation(e4, 2250);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Messenger.Default.Send(new StatuMessage(TConfig.Loading));
            Method.Progress(this, () =>
            {
                DataService.Default.Load();
            }, () =>
            {
                Messenger.Default.Send(new StatuMessage("加载完成"));
            },
            ex =>
            {
                ex.Log();
                Messenger.Default.Send(new StatuMessage(ex.Message()));
                Method.Error(this, ex.Message());
            });
            if (this.pathCurrent == null)
            {
                this.pathCurrent = transition.Content;
                this.pathNew = new Path
                {
                    Style = this.FindResource("PathRound") as Style
                };
            }
        }
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            var myAdornerLayer = Method.EllipseAdorner(e);
            if (myAdornerLayer == null) Method.EllipseAdorner(panel, e, 100);
            base.OnPreviewMouseDown(e);
        }
        private void Animation(Ellipse ellipse, double beginTime = 0, double time = 3000)
        {
            var widthAnimation = new DoubleAnimation(10, 100, new Duration(TimeSpan.FromMilliseconds(time)));
            widthAnimation.BeginTime = TimeSpan.FromMilliseconds(beginTime);
            widthAnimation.RepeatBehavior = RepeatBehavior.Forever;
            //ellipse.BeginAnimation(FrameworkElement.WidthProperty, widthAnimation);
            Storyboard.SetTargetName(widthAnimation, ellipse.Name);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(FrameworkElement.WidthProperty));
            storyboard.Children.Add(widthAnimation);

            var heightAnimation = new DoubleAnimation(10, 100, new Duration(TimeSpan.FromMilliseconds(time)));
            heightAnimation.BeginTime = TimeSpan.FromMilliseconds(beginTime);
            heightAnimation.RepeatBehavior = RepeatBehavior.Forever;
            //ellipse.BeginAnimation(FrameworkElement.HeightProperty, widthAnimation);
            Storyboard.SetTargetName(heightAnimation, ellipse.Name);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(FrameworkElement.HeightProperty));
            storyboard.Children.Add(heightAnimation);

            var colorAnimation = new ColorAnimation(Color.FromArgb(160, 255, 0, 0), Color.FromArgb(10, 255, 0, 0), new Duration(TimeSpan.FromMilliseconds(time)));
            colorAnimation.BeginTime = TimeSpan.FromMilliseconds(beginTime);
            colorAnimation.RepeatBehavior = RepeatBehavior.Forever;
            //var solid = ellipse.Fill = (SolidColorBrush)ellipse.Fill.Clone();
            //solid.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            var propertyChain = new DependencyProperty[] { Ellipse.FillProperty, SolidColorBrush.ColorProperty };
            Storyboard.SetTargetName(colorAnimation, ellipse.Name);
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("(0).(1)", propertyChain));
            storyboard.Children.Add(colorAnimation);
        }

        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            storyboard.Stop(this);
            Method.DoEvents();
            storyboard.Begin(this, true);

            var r = Validation.GetHasError(tb);
            Method.Toast(this, r);
            var xml = Method.GetTemplateXaml(dp);
            //Method.Toast(this, xml);

            Method.Progress(this, false);

            transition.TransitionType = (TransitionType)new Random().Next(0, 5);
            if (transition.Content != pathNew) transition.Content = this.pathNew;
            else transition.Content = this.pathCurrent;

            var opacity1 = new DoubleAnimation(line1.X1, 0, new Duration(TimeSpan.FromMilliseconds(200)));
            line1.BeginAnimation(Line.X1Property, opacity1);
            var opacity2 = new DoubleAnimation(line2.X2, 137, new Duration(TimeSpan.FromMilliseconds(200)));
            line2.BeginAnimation(Line.X2Property, opacity2);
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            storyboard.Stop(this);

            Method.Hide();

            var opacity1 = new DoubleAnimation(line1.X1, 137, new Duration(TimeSpan.FromMilliseconds(200)));
            line1.BeginAnimation(Line.X1Property, opacity1);
            var opacity2 = new DoubleAnimation(line2.X2, 0, new Duration(TimeSpan.FromMilliseconds(200)));
            line2.BeginAnimation(Line.X2Property, opacity2);
        }
    }
}
