using Paway.Helper;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Paway.WPF
{
    /// <summary>
    /// 徽章
    /// </summary>
    public partial class Badge : ContentControl
    {
        #region Identity
        private readonly Storyboard _storyboard_ScaleBigger;
        private readonly Storyboard _storyboard_ScaleSmaller;

        #endregion

        /// <summary>
        /// </summary>
        public Badge()
        {
            InitializeComponent();
            AlwaysCenter = true;
            _storyboard_ScaleBigger = FindResource("Storyboard_ScaleBigger") as Storyboard;
            _storyboard_ScaleSmaller = FindResource("Storyboard_ScaleSmaller") as Storyboard;
        }

        #region Internal Property
        internal new static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof(Content), typeof(object), typeof(Badge));
        internal new object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        #endregion

        #region 显示文本
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(Badge), new PropertyMetadata(OnTextChanged));
        /// <summary>
        /// 显示文本
        /// </summary>
        [Category("自定义")]
        [Description("显示文本")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var badge = d as Badge;
            if (!badge.IsLoaded)
            {
                badge.Scale.ScaleX = 1;
                badge.Scale.ScaleY = 1;
                badge.TxtBlock.Text = badge.Text;
                return;
            }
            if ((e.OldValue as string).IsEmpty())
            {
                badge.StopWave();
                badge._storyboard_ScaleBigger.Completed += badge.Storyboard_ScaleBigger_Completed;
                badge._storyboard_ScaleBigger.Begin();
                return;
            }
            else if ((e.NewValue as string).IsEmpty())
            {
                badge.StopWave();
                badge.ShowText("");
                badge._storyboard_ScaleSmaller.Completed += badge.Storyboard_ScaleSmaller_Completed;
                badge._storyboard_ScaleSmaller.Begin();
                return;
            }
            else
            {
                badge.ShowText(badge.Text);
            }
        }
        private void Storyboard_ScaleSmaller_Completed(object sender, EventArgs e)
        {
            ChangeWaving();
            _storyboard_ScaleSmaller.Completed -= Storyboard_ScaleSmaller_Completed;
        }
        private void Storyboard_ScaleBigger_Completed(object sender, EventArgs e)
        {
            ChangeWaving();
            ShowText(Text);
            _storyboard_ScaleBigger.Completed -= Storyboard_ScaleBigger_Completed;
        }
        private void ShowText(string text)
        {
            AnimationHelper.Start(TxtBlock, TransitionType.Opacity, 0, 100, () =>
            {
                TxtBlock.Text = text;
                AnimationHelper.Start(TxtBlock, TransitionType.Opacity, 1, 100);
            });
        }
        private void ChangeWaving()
        {
            if (IsWaving)
            {
                RectWave.Visibility = Visibility.Visible;
                BeginWave();
            }
            else
            {
                RectWave.Visibility = Visibility.Collapsed;
                StopWave();
            }
        }

        #endregion

        #region 显示动画
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IsWavingProperty =
            DependencyProperty.Register(nameof(IsWaving), typeof(bool), typeof(Badge), new PropertyMetadata(OnIsWavingChanged));
        /// <summary>
        /// 显示动画
        /// </summary>
        [Category("自定义")]
        [Description("显示动画")]
        public bool IsWaving
        {
            get { return (bool)GetValue(IsWavingProperty); }
            set { SetValue(IsWavingProperty, value); }
        }
        private static void OnIsWavingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var badge = d as Badge;
            badge.ChangeWaving();
        }

        #endregion

        #region 居中
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty AlwaysCenterProperty =
            DependencyProperty.Register(nameof(AlwaysCenter), typeof(bool), typeof(Badge), new PropertyMetadata(false, OnAlwaysCenterChanged));
        /// <summary>
        /// 居中
        /// </summary>
        [Category("自定义")]
        [Description("居中")]
        public bool AlwaysCenter
        {
            get { return (bool)GetValue(AlwaysCenterProperty); }
            set { SetValue(AlwaysCenterProperty, value); }
        }
        private static void OnAlwaysCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var badge = d as Badge;
            badge.SizeChanged -= badge.Badge_SizeChanged;
            if (badge.AlwaysCenter)
            {
                badge.SizeChanged += badge.Badge_SizeChanged;
            }
        }
        private void Badge_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (HorizontalAlignment == HorizontalAlignment.Center)
            {
                RenderTransform = null;
                return;
            }
            else if (HorizontalAlignment == HorizontalAlignment.Left)
            {
                var delta = (ActualWidth - ActualHeight) / 2;
                RenderTransform = new TranslateTransform() { X = -delta };
            }
            else if (HorizontalAlignment == HorizontalAlignment.Right)
            {
                var delta = (ActualWidth - ActualHeight) / 2;
                RenderTransform = new TranslateTransform() { X = delta };
            }
        }

        #endregion

        #region Function
        private void BeginWave()
        {
            var anima1 = new DoubleAnimation()
            {
                From = Text.IsEmpty() ? 0.4 : 1,
                To = 2,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = TimeSpan.FromSeconds(1.5),
            };
            ScaleWave.BeginAnimation(ScaleTransform.ScaleXProperty, anima1);

            var anima2 = new DoubleAnimation()
            {
                From = Text.IsEmpty() ? 0.4 : 1,
                To = 2,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = TimeSpan.FromSeconds(1.5),
            };
            ScaleWave.BeginAnimation(ScaleTransform.ScaleYProperty, anima2);

            var anima3 = new DoubleAnimation()
            {
                To = 0,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = TimeSpan.FromSeconds(1.5),
            };
            RectWave.BeginAnimation(OpacityProperty, anima3);
        }
        private void StopWave()
        {
            ScaleWave.BeginAnimation(ScaleTransform.ScaleXProperty, null);
            ScaleWave.BeginAnimation(ScaleTransform.ScaleYProperty, null);
            RectWave.BeginAnimation(OpacityProperty, null);
        }

        #endregion
    }
}
