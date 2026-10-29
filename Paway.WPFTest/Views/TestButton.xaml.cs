using CommunityToolkit.Mvvm.Messaging;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.Test
{
    /// <summary>
    /// TestButton.xaml 的交互逻辑
    /// </summary>
    public partial class TestButton : Page
    {
        /// <summary>
        /// 故事板
        /// </summary>
        private readonly Storyboard storyboard = new Storyboard();
        private Path pathNew;
        private object pathCurrent;

        public TestButton()
        {
            InitializeComponent();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.pathCurrent == null)
            {
                this.pathCurrent = transition.Content;
                this.pathNew = new Path
                {
                    Style = this.FindResource("PathRound") as Style
                };
            }
        }

        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            badge.Text = DateTime.Now.Second.ToString();

            var type = (TransitionType)Method.Random(0, (int)TransitionType.Bottom + 1);
            if (btnCancel.Opacity == 0) type = TransitionType.FadeIn;
            AnimationHelper.Start(btnCancel, type);

            storyboard.Stop(this);
            Method.DoEvents();
            storyboard.Begin(this, true);

            type = (TransitionType)Method.Random((int)TransitionType.Left, (int)TransitionType.Bottom + 1);
            transition.TransitionType = type;
            if (transition.Content != pathNew) transition.Content = this.pathNew;
            else transition.Content = this.pathCurrent;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            storyboard.Stop(this);
        }
    }
}
