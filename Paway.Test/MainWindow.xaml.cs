using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.Test.ViewModel;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly Storyboard storyboard = new Storyboard();
        private Path pathNew;
        private object pathCurrent;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            badge.Text = DateTime.Now.Second.ToString();

            rt.AddLine(DateTime.Now.ToString());
            rt.Focus();

            var type = (TransitionType)Method.Random(0, (int)TransitionType.Bottom + 1);
            if (btnCancel.Opacity == 0) type = TransitionType.FadeIn;
            AnimationHelper.Start(btnCancel, type);

            progress.AnimationValue = Method.Random(100);

            //Method.Progress(this, () =>
            //{
            //    Thread.Sleep(2000);
            //});

            storyboard.Stop(this);
            Method.DoEvents();
            storyboard.Begin(this, true);

            var r = Validation.GetHasError(tb);
            Method.Hit(this, r);
            var xml = Method.GetTemplateXaml(tb);
            Debug.WriteLine(xml);
            //Method.Toast(this, xml);

            type = (TransitionType)Method.Random((int)TransitionType.Left, (int)TransitionType.Bottom + 1);
            transition.TransitionType = type;
            if (transition.Content != pathNew) transition.Content = this.pathNew;
            else transition.Content = this.pathCurrent;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            storyboard.Stop(this);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (IsLoaded) return;
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
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Messenger.Default.Send(new StatuMessage(Config.Loading));
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
            //WPF.TMethod.WaterAdorner(e);
            base.OnPreviewMouseDown(e);
        }
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (Method.Parent(listView3, out Window window))
            {
                if (window.Content is Panel panel)
                {
                    Method.WaterAdornerFixed(panel, e, 10);
                }
            }
            base.OnPreviewMouseMove(e);
        }
    }
}
