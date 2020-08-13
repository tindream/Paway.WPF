// Thanks: http://briandunnington.github.io/progressring-wp8.html

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// Window圆点进度条
    /// </summary>
    [TemplateVisualState(GroupName = Progress.GroupActive, Name = Progress.StateActive)]
    [TemplateVisualState(GroupName = Progress.GroupActive, Name = Progress.StateInactive)]
    public partial class Progress : Control
    {
        #region GroupActive
        /// <summary>
        /// Active state
        /// </summary>
        private const string StateActive = "Active";
        /// <summary>
        /// Inactive state
        /// </summary>
        private const string StateInactive = "Inactive";
        /// <summary>
        /// Active state group
        /// </summary>
        private const string GroupActive = "ActiveStates";

        #endregion GroupActive

        private bool hasAppliedTemplate = false;

        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(Progress), new PropertyMetadata(true, new PropertyChangedCallback(IsActiveChanged)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TemplateSettingsProperty =
            DependencyProperty.Register(nameof(TemplateSettings), typeof(ProgressTemplate), typeof(Progress), new PropertyMetadata(null));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(Progress),
                new PropertyMetadata(new BrushEXT(null, Color.FromArgb(205, Config.Color.R, Config.Color.G, Config.Color.B))));

        #endregion

        #region 扩展
        /// <summary>
        /// Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        /// </summary>
        [Browsable(false)]
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        /// <summary>
        /// Using a DependencyProperty as the backing store for TemplateSettings.  This enables animation, styling, binding, etc...
        /// </summary>
        [Browsable(false)]
        public ProgressTemplate TemplateSettings
        {
            get { return (ProgressTemplate)GetValue(TemplateSettingsProperty); }
            set { SetValue(TemplateSettingsProperty, value); }
        }
        /// <summary>
        /// 项颜色
        /// </summary>
        [Category("扩展")]
        [Description("项颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public Progress()
        {
            DefaultStyleKey = typeof(Progress);
            TemplateSettings = new ProgressTemplate(60);
        }
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            hasAppliedTemplate = true;
            UpdateState(IsActive);
        }
        private static void IsActiveChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is Progress progress)
            {
                var isActive = (bool)args.NewValue;
                progress.UpdateState(isActive);
            }
        }
        private void UpdateState(bool isActive)
        {
            if (hasAppliedTemplate)
            {
                string state = isActive ? Progress.StateActive : Progress.StateInactive;
                VisualStateManager.GoToState(this, state, true);
            }
        }
    }
}
