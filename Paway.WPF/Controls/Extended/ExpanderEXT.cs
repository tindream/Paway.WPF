using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// Expander扩展
    /// </summary>
    public class ExpanderEXT : Expander
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(ExpanderEXT), new PropertyMetadata(new CornerRadius(3)));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ExpanderEXT()
        {
            DefaultStyleKey = typeof(ExpanderEXT);
        }

        #region Private属性
        private FrameworkElement PART_Root;

        #endregion

        #region Override方法
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.PART_Root = this.GetTemplateChild("PART_Root") as FrameworkElement;
            if (this.PART_Root != null)
            {
                VisualStateManager.GoToElementState(this.PART_Root, this.IsExpanded ? "Storyboard_Expanded" : "Storyboard_Collapsed", true);
            }
        }
        /// <summary>
        /// 折叠
        /// </summary>
        protected override void OnCollapsed()
        {
            base.OnCollapsed();
            if (this.PART_Root != null)
            {
                VisualStateManager.GoToElementState(this.PART_Root, "Storyboard_Collapsed", true);
            }
        }
        /// <summary>
        /// 展开
        /// </summary>
        protected override void OnExpanded()
        {
            base.OnExpanded();
            if (this.PART_Root != null)
            {
                VisualStateManager.GoToElementState(this.PART_Root, "Storyboard_Expanded", true);
            }
        }

        #endregion
    }
}