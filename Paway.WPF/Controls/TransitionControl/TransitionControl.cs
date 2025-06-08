using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Paway.WPF
{
    /// <summary>
    /// 动画
    /// </summary>
    public partial class TransitionControl : ContentControl
    {
        #region ContentPresenter
        /// <summary>
        /// Gets or sets the current content presentation site.
        /// </summary>
        /// <value>The current content presentation site.</value>
        private ContentPresenter CurrentContentPresentationSite { get; set; }
        /// <summary>
        /// Gets or sets the previous content presentation site.
        /// </summary>
        /// <value>The previous content presentation site.</value>
        private ContentPresenter PreviousContentPresentationSite { get; set; }

        #endregion

        #region Events
        /// <summary>
        /// Occurs when the current transition has completed.
        /// </summary>
        public event RoutedEventHandler TransitionCompleted;

        #endregion Events

        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached(nameof(Value), typeof(double), typeof(TransitionControl));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.RegisterAttached(nameof(Time), typeof(int), typeof(TransitionControl));
        /// <summary>
        /// Identifies the Transition dependency property.
        /// </summary>
        public static readonly DependencyProperty TransitionTypeProperty =
            DependencyProperty.Register(nameof(TransitionType), typeof(TransitionType), typeof(TransitionControl), new PropertyMetadata(TransitionType.FadeIn, OnTransitionPropertyChanged));
        /// <summary>
        /// TransitionProperty property changed handler.
        /// </summary>
        /// <param name="obj">TransitionControl that changed its Transition.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnTransitionPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue && obj is TransitionControl source)
            {
                source.AbortTransition();
                source.StartTransition();
            }
        }

        /// <summary>
        /// 过渡值
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("过渡值")]
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        /// <summary>
        /// 过渡时间
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("过渡时间")]
        public int Time
        {
            get { return (int)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
        /// <summary>
        /// 过渡动画类型
        /// <para>默认值：FadeIn</para>
        /// <para>有效值：None, FadeIn, Left, Right, Top, Bottom, Width, Height, ScanX, ScanY</para>
        /// </summary>
        [Category("扩展")]
        [Description("过渡动画类型")]
        public TransitionType TransitionType
        {
            get { return (TransitionType)GetValue(TransitionTypeProperty); }
            set
            {
                SetValue(TransitionTypeProperty, value);
                Refresh();
            }
        }
        /// <summary>
        /// 过渡动画类型（不触发动画）
        /// <para>默认值：FadeIn</para>
        /// <para>有效值：None, FadeIn, Left, Right, Top, Bottom, Width, Height, ScanX, ScanY</para>
        /// </summary>
        [Category("扩展")]
        [Description("过渡动画类型（不触发动画）")]
        public TransitionType TransitionTypeNoTrigger
        {
            get { return (TransitionType)GetValue(TransitionTypeProperty); }
            set { SetValue(TransitionTypeProperty, value); }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionControl"/> class.
        /// </summary>
        public TransitionControl()
        {
            DefaultStyleKey = typeof(TransitionControl);
        }
        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh()
        {
            var content = this.Content;
            this.Content = null;
            this.Content = content;
        }

        #region 执行动画
        /// <summary>
        /// Builds the visual tree for the TransitionControl control
        /// when a new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            AbortTransition();
            base.OnApplyTemplate();

            PreviousContentPresentationSite = GetTemplateChild(nameof(PreviousContentPresentationSite)) as ContentPresenter;
            CurrentContentPresentationSite = GetTemplateChild(nameof(CurrentContentPresentationSite)) as ContentPresenter;

            if (CurrentContentPresentationSite != null)
            {
                CurrentContentPresentationSite.Content = Content;
                StartTransition();
            }
        }
        /// <summary>
        /// Called when the value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property changes.
        /// </summary>
        /// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
        /// <param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            StartTransition(oldContent, newContent);
        }

        /// <summary>
        /// Starts the transition.
        /// </summary>
        /// <param name="oldContent">The old content.</param>
        /// <param name="newContent">The new content.</param>
        private void StartTransition(object oldContent, object newContent)
        {
            // both presenters must be available, otherwise a transition is useless.
            if (CurrentContentPresentationSite != null && PreviousContentPresentationSite != null)
            {
                CurrentContentPresentationSite.Content = newContent;
                PreviousContentPresentationSite.Content = oldContent;
                StartTransition();
            }
        }
        /// <summary>
        /// Aborts the transition and releases the previous content.
        /// </summary>
        private void AbortTransition()
        {
            if (PreviousContentPresentationSite != null)
            {
                PreviousContentPresentationSite.Content = null;
            }
        }
        /// <summary>
        /// Handles the Completed event of the transition storyboard.
        /// </summary>
        private void OnTransitionCompleted()
        {
            AbortTransition();
            TransitionCompleted?.Invoke(this, new RoutedEventArgs());
        }
        /// <summary>
        /// Gets or sets the storyboard that is used to transition old and new content.
        /// </summary>
        public void StartTransition()
        {
            if (PreviousContentPresentationSite != null) PreviousContentPresentationSite.RenderTransform = null;
            switch (TransitionType)
            {
                case TransitionType.Left:
                    AnimationHelper.Start(PreviousContentPresentationSite, TransitionType.ToRight, Value, Time);
                    break;
                case TransitionType.Right:
                    AnimationHelper.Start(PreviousContentPresentationSite, TransitionType.ToLeft, Value, Time);
                    break;
                case TransitionType.Top:
                    AnimationHelper.Start(PreviousContentPresentationSite, TransitionType.ToBottom, Value, Time);
                    break;
                case TransitionType.Bottom:
                    AnimationHelper.Start(PreviousContentPresentationSite, TransitionType.ToTop, Value, Time);
                    break;
                case TransitionType.Width:
                    AnimationHelper.Start(PreviousContentPresentationSite, TransitionType.Width, Value, Time);
                    break;
                case TransitionType.Height:
                    AnimationHelper.Start(PreviousContentPresentationSite, TransitionType.Height, Value, Time);
                    break;
                case TransitionType.ScanX:
                case TransitionType.ScanY:
                    AnimationHelper.Start(PreviousContentPresentationSite, TransitionType, Value, 1, Time);
                    break;
            }
            switch (TransitionType)
            {
                case TransitionType.Left:
                case TransitionType.Right:
                case TransitionType.Top:
                case TransitionType.Bottom:
                    AnimationHelper.Start(CurrentContentPresentationSite, TransitionType, Value, Time, OnTransitionCompleted);
                    break;
                case TransitionType.Width:
                    AnimationHelper.Start(CurrentContentPresentationSite, TransitionType.Width, CurrentContentPresentationSite.ActualWidth, 0, Time);
                    break;
                case TransitionType.Height:
                    AnimationHelper.Start(CurrentContentPresentationSite, TransitionType.Height, CurrentContentPresentationSite.ActualHeight, 0, Time);
                    break;
                case TransitionType.ScanX:
                case TransitionType.ScanY:
                    AnimationHelper.Start(CurrentContentPresentationSite, TransitionType, 1, 0, Time);
                    break;
            }
            switch (TransitionType)
            {
                case TransitionType.None:
                    break;
                default:
                    AnimationHelper.Start(PreviousContentPresentationSite, TransitionType.FadeOut, Value, Time);
                    AnimationHelper.Start(CurrentContentPresentationSite, TransitionType.FadeIn, Value, Time, OnTransitionCompleted);
                    break;
            }
        }

        #endregion
    }
}