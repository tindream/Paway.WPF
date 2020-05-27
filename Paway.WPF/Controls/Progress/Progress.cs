// Thanks: http://briandunnington.github.io/progressring-wp8.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Paway.WPF
{
    /// <summary>
    /// Window圆点进度条
    /// </summary>
    [TemplateVisualState(GroupName = Config.GroupActive, Name = Config.StateActive)]
    [TemplateVisualState(GroupName = Config.GroupActive, Name = Config.StateInactive)]
    public partial class Progress : Control
    {
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

        #endregion

        #region 扩展
        /// <summary>
        /// Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        /// </summary>
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        /// <summary>
        /// Using a DependencyProperty as the backing store for TemplateSettings.  This enables animation, styling, binding, etc...
        /// </summary>
        public ProgressTemplate TemplateSettings
        {
            get { return (ProgressTemplate)GetValue(TemplateSettingsProperty); }
            set { SetValue(TemplateSettingsProperty, value); }
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
                string state = isActive ? Config.StateActive : Config.StateInactive;
                VisualStateManager.GoToState(this, state, true);
            }
        }
    }
}
