using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Invengo.Utils
{
    public partial class ComboBoxRound : ComboBox
    {
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached("Radius", typeof(CornerRadius), typeof(ComboBoxRound), new PropertyMetadata(new CornerRadius(3)));
        public static readonly DependencyProperty SelectedBackgroundProperty =
            DependencyProperty.RegisterAttached("SelectedBackground", typeof(Brush), typeof(ComboBoxRound),
            new PropertyMetadata(new SolidColorBrush(Color.FromArgb(200, 35, 175, 255))));
        public static readonly DependencyProperty SelectedForegroundProperty =
            DependencyProperty.RegisterAttached("SelectedForeground", typeof(Brush), typeof(ComboBoxRound),
            new PropertyMetadata(new SolidColorBrush(Colors.White)));
        public static readonly DependencyProperty MouseOverBackgroundProperty =
           DependencyProperty.RegisterAttached("MouseOverBackground", typeof(Brush), typeof(ComboBoxRound),
           new PropertyMetadata(new SolidColorBrush(Color.FromArgb(170, 35, 175, 255))));
        public static readonly DependencyProperty MouseOverForegroundProperty =
           DependencyProperty.RegisterAttached("MouseOverForeground", typeof(Brush), typeof(ComboBoxRound),
           new PropertyMetadata(new SolidColorBrush(Colors.White)));

        [Category("扩展")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        [Category("扩展")]
        [Description("Item项不可用。仅用于控件激活后边框颜色")]
        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }
        [Category("扩展")]
        public Brush SelectedForeground
        {
            get { return (Brush)GetValue(SelectedForegroundProperty); }
            set { SetValue(SelectedForegroundProperty, value); }
        }
        [Category("扩展")]
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }
        [Category("扩展")]
        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        public ComboBoxRound() { }
    }
}
