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
using System.Windows.Media.Animation;

namespace Paway.WPF
{
    /// <summary>
    /// Represents a control with a single piece of content and when that content
    /// changes performs a transition animation.
    /// </summary>
    /// <QualityBand>Experimental</QualityBand>
    /// <remarks>The API for this control will change considerably in the future.</remarks>
    [TemplateVisualState(GroupName = PresentationGroup, Name = NormalState)]
    [TemplateVisualState(GroupName = PresentationGroup, Name = DefaultTransitionState)]
    [TemplatePart(Name = PreviousContentPresentationSitePartName, Type = typeof(ContentControl))]
    [TemplatePart(Name = CurrentContentPresentationSitePartName, Type = typeof(ContentControl))]
    public partial class TransitionControl : ContentControl
    {
        #region Visual state names
        /// <summary>
        /// The name of the group that holds the presentation states.
        /// </summary>
        private const string PresentationGroup = "PresentationStates";
        /// <summary>
        /// The name of the state that represents a normal situation where no
        /// transition is currently being used.
        /// </summary>
        private const string NormalState = "Normal";
        /// <summary>
        /// The name of the state that represents the default transition.
        /// </summary>
        private const string DefaultTransitionState = "Default";

        /// <summary>
        /// The name of the control that will display the previous content.
        /// </summary>
        internal const string PreviousContentPresentationSitePartName = "PreviousContentPresentationSite";

        /// <summary>
        /// The name of the control that will display the current content.
        /// </summary>
        internal const string CurrentContentPresentationSitePartName = "CurrentContentPresentationSite";

        #endregion

        #region TemplateParts
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

        #endregion TemplateParts

        #region public bool IsTransitioning
        /// <summary>
        /// Identifies the IsTransitioning dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTransitioningProperty =
            DependencyProperty.Register(nameof(IsTransitioning), typeof(bool), typeof(TransitionControl), new PropertyMetadata(OnIsTransitioningPropertyChanged));
        /// <summary>
        /// IsTransitioningProperty property changed handler.
        /// </summary>
        /// <param name="d">TransitionControl that changed its IsTransitioning.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsTransitioningPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TransitionControl source = (TransitionControl)d;
            if (!source._allowIsTransitioningWrite)
            {
                source.IsTransitioning = (bool)e.OldValue;
                throw new InvalidOperationException("Exception thrown when IsTransitioning is altered.");
            }
        }
        /// <summary>
        /// Indicates whether the control allows writing IsTransitioning.
        /// </summary>
        private bool _allowIsTransitioningWrite;
        /// <summary>
        /// Gets a value indicating whether this instance is currently performing
        /// a transition.
        /// </summary>
        [Browsable(false)]
        public bool IsTransitioning
        {
            get { return (bool)GetValue(IsTransitioningProperty); }
            private set
            {
                _allowIsTransitioningWrite = true;
                SetValue(IsTransitioningProperty, value);
                _allowIsTransitioningWrite = false;
            }
        }

        #endregion

        #region public string Transition
        /// <summary>
        /// The storyboard that is used to transition old and new content.
        /// </summary>
        private Storyboard _currentTransition;
        /// <summary>
        /// Gets or sets the storyboard that is used to transition old and new content.
        /// </summary>
        private void SetCurrentTransition(Storyboard value)
        {
            // decouple event
            if (_currentTransition != null)
            {
                _currentTransition.Completed -= OnTransitionCompleted;
            }
            _currentTransition = value;
            if (_currentTransition != null)
            {
                _currentTransition.Completed += OnTransitionCompleted;
            }
        }
        /// <summary>
        /// Gets or sets the name of the transition to use. These correspond
        /// directly to the TransitionControl inside the PresentationStates group.
        /// </summary>
        [Category("扩展")]
        [Description("过渡类型")]
        public TransitionType Transition
        {
            get { return (TransitionType)GetValue(TransitionProperty); }
            set
            {
                SetValue(TransitionProperty, value);
                Refresh();
            }
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
        /// <summary>
        /// Identifies the Transition dependency property.
        /// </summary>
        public static readonly DependencyProperty TransitionProperty =
            DependencyProperty.Register(nameof(Transition), typeof(TransitionType), typeof(TransitionControl),
                new PropertyMetadata(TransitionType.Default, OnTransitionPropertyChanged));
        /// <summary>
        /// TransitionProperty property changed handler.
        /// </summary>
        /// <param name="d">TransitionControl that changed its Transition.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnTransitionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TransitionControl source = (TransitionControl)d;
            var oldTransition = (TransitionType)e.OldValue;
            var newTransition = (TransitionType)e.NewValue;

            if (source.IsTransitioning)
            {
                source.AbortTransition();
            }

            // find new transition
            Storyboard newStoryboard = source.GetStoryboard(newTransition);
            // unable to find the transition.
            if (newStoryboard == null)
            {
                // could be during initialization of xaml that presentationgroups was not yet defined
                if (TryGetVisualStateGroup(source, PresentationGroup) == null)
                {
                    // will delay check
                    source.SetCurrentTransition(null);
                }
                else
                {
                    // revert to old value
                    source.SetValue(TransitionProperty, oldTransition);
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture, "Exception thrown when a Transition is set that is not available.", newTransition));
                }
            }
            else
            {
                source.SetCurrentTransition(newStoryboard);
            }
        }

        #endregion

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached(nameof(Value), typeof(double), typeof(TransitionControl), new PropertyMetadata(30d, OnValueChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ValueReverseProperty =
            DependencyProperty.RegisterAttached(nameof(ValueReverse), typeof(double), typeof(TransitionControl), new PropertyMetadata(-30d));
        /// <summary>
        /// 过渡值
        /// </summary>
        [Category("扩展")]
        [Description("过渡值")]
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        /// <summary>
        /// 反向过渡值
        /// </summary>
        [Browsable(false)]
        public double ValueReverse
        {
            get { return (double)GetValue(ValueReverseProperty); }
            set { SetValue(ValueReverseProperty, value); }
        }
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TransitionControl transition)
            {
                transition.Loaded += delegate
                {
                    transition.ValueReverse = -transition.Value;
                };
            }
        }

        #region Events
        /// <summary>
        /// Occurs when the current transition has completed.
        /// </summary>
        public event RoutedEventHandler TransitionCompleted;

        #endregion Events

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionControl"/> class.
        /// </summary>
        public TransitionControl()
        {
            DefaultStyleKey = typeof(TransitionControl);
        }
        /// <summary>
        /// Builds the visual tree for the TransitionControl control
        /// when a new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            if (IsTransitioning)
            {
                AbortTransition();
            }
            base.OnApplyTemplate();

            PreviousContentPresentationSite = GetTemplateChild(PreviousContentPresentationSitePartName) as ContentPresenter;
            CurrentContentPresentationSite = GetTemplateChild(CurrentContentPresentationSitePartName) as ContentPresenter;
            if (CurrentContentPresentationSite != null)
            {
                CurrentContentPresentationSite.Content = Content;
            }

            // hookup currenttransition
            Storyboard transition = GetStoryboard(Transition);
            SetCurrentTransition(transition);
            if (transition == null)
            {
                string invalidTransition = Transition.Description();

                // revert to default
                Transition = TransitionType.Default;

                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, "Exception thrown when a Transition is set that is not available.", invalidTransition));
            }

            VisualStateManager.GoToState(this, NormalState, false);
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
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "newContent", Justification = "Should be used in the future.")]
        private void StartTransition(object oldContent, object newContent)
        {
            // both presenters must be available, otherwise a transition is useless.
            if (CurrentContentPresentationSite != null && PreviousContentPresentationSite != null)
            {
                CurrentContentPresentationSite.Content = newContent;
                PreviousContentPresentationSite.Content = oldContent;
                // and start a new transition
                if (!IsTransitioning)
                {
                    IsTransitioning = true;
                    VisualStateManager.GoToState(this, NormalState, false);
                    VisualStateManager.GoToState(this, Transition.Description(), true);
                }
            }
        }
        /// <summary>
        /// Handles the Completed event of the transition storyboard.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnTransitionCompleted(object sender, EventArgs e)
        {
            AbortTransition();
            TransitionCompleted?.Invoke(this, new RoutedEventArgs());
        }
        /// <summary>
        /// Aborts the transition and releases the previous content.
        /// </summary>
        public void AbortTransition()
        {
            // go to normal state and release our hold on the old content.
            VisualStateManager.GoToState(this, NormalState, false);
            IsTransitioning = false;
            if (PreviousContentPresentationSite != null)
            {
                PreviousContentPresentationSite.Content = null;
            }
        }

        /// <summary>
        /// Attempts to find a storyboard that matches the newTransition name.
        /// </summary>
        /// <param name="type">The new transition.</param>
        /// <returns>A storyboard or null, if no storyboard was found.</returns>
        private Storyboard GetStoryboard(TransitionType type)
        {
            VisualStateGroup presentationGroup = TryGetVisualStateGroup(this, PresentationGroup);
            Storyboard newStoryboard = null;
            if (presentationGroup != null)
            {
                newStoryboard = presentationGroup.States
                    .OfType<VisualState>()
                    .Where(state => state.Name == type.Description())
                    .Select(state => state.Storyboard)
                    .FirstOrDefault();
            }

            return newStoryboard;
        }
    }
}