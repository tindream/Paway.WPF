using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// BookClass.xaml 的交互逻辑
    /// </summary>
    public partial class WrapList : PanelListBase
    {
        #region 公开属性
        public static readonly DependencyProperty OrientationProperty = 
            DependencyProperty.RegisterAttached("Orientation", typeof(Orientation), typeof(WrapList), new PropertyMetadata(Orientation.Horizontal, OnOrientationChanged));
        private static void OnOrientationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is WrapList wrap)
            {
                if (Method.Child(wrap, out WrapPanel wrapPanel))
                {
                    wrapPanel.Orientation = (Orientation)e.NewValue;
                }
            }
        }

        [Category("扩展")]
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        #endregion

        public WrapList()
        {
            InitializeComponent();
            base.Init(wrapPanel, scrollViewer);
        }
    }
}
