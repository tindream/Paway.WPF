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
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(ExpanderEXT),
                new PropertyMetadata(new BrushEXT(Colors.Transparent)));

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
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
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
                var suffix = "Y";
                switch (ExpandDirection)
                {
                    case ExpandDirection.Down:
                    case ExpandDirection.Up:
                        break;
                    case ExpandDirection.Left:
                    case ExpandDirection.Right:
                        suffix = "X";
                        break;
                }
                VisualStateManager.GoToElementState(this.PART_Root, this.IsExpanded ? $"Storyboard_Expanded_{suffix}" : $"Storyboard_Collapsed_{suffix}", true);
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
                var suffix = "Y";
                switch (ExpandDirection)
                {
                    case ExpandDirection.Down:
                    case ExpandDirection.Up:
                        break;
                    case ExpandDirection.Left:
                    case ExpandDirection.Right:
                        suffix = "X";
                        break;
                }
                VisualStateManager.GoToElementState(this.PART_Root, $"Storyboard_Collapsed_{suffix}", true);
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
                var suffix = "Y";
                switch (ExpandDirection)
                {
                    case ExpandDirection.Down:
                    case ExpandDirection.Up:
                        break;
                    case ExpandDirection.Left:
                    case ExpandDirection.Right:
                        suffix = "X";
                        break;
                }
                VisualStateManager.GoToElementState(this.PART_Root, $"Storyboard_Expanded_{suffix}", true);
            }
        }

        #endregion
    }
}