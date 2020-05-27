// Thanks: http://briandunnington.github.io/progressring-wp8.html

using System.Windows;

namespace Paway.WPF
{
    /// <summary>
    /// Progress模板
    /// </summary>
    public class ProgressTemplate : System.Windows.DependencyObject
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty MaxSideLengthProperty =
            DependencyProperty.Register(nameof(MaxSideLength), typeof(double), typeof(ProgressTemplate), new PropertyMetadata(0D));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty EllipseDiameterProperty =
            DependencyProperty.Register(nameof(EllipseDiameter), typeof(double), typeof(ProgressTemplate), new PropertyMetadata(0D));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty EllipseOffsetProperty =
            DependencyProperty.Register(nameof(EllipseOffset), typeof(Thickness), typeof(ProgressTemplate), new PropertyMetadata(default(Thickness)));

        #endregion

        #region 扩展
        /// <summary>
        /// Using a DependencyProperty as the backing store for MaxSideLength.  This enables animation, styling, binding, etc...
        /// </summary>
        public double MaxSideLength
        {
            get { return (double)GetValue(MaxSideLengthProperty); }
            set { SetValue(MaxSideLengthProperty, value); }
        }
        /// <summary>
        /// Using a DependencyProperty as the backing store for EllipseDiameter.  This enables animation, styling, binding, etc...
        /// </summary>
        public double EllipseDiameter
        {
            get { return (double)GetValue(EllipseDiameterProperty); }
            set { SetValue(EllipseDiameterProperty, value); }
        }
        /// <summary>
        /// Using a DependencyProperty as the backing store for EllipseOffset.  This enables animation, styling, binding, etc...
        /// </summary>
        public Thickness EllipseOffset
        {
            get { return (Thickness)GetValue(EllipseOffsetProperty); }
            set { SetValue(EllipseOffsetProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ProgressTemplate(double width)
        {
            if (width <= 40)
            {
                EllipseDiameter = (width / 10) + 1;
            }
            else
            {
                EllipseDiameter = width / 10;
            }
            MaxSideLength = width - EllipseDiameter;
            EllipseOffset = new System.Windows.Thickness(0, EllipseDiameter * 2.5, 0, 0);
        }
    }
}
