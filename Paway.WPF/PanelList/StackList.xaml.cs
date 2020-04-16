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
    public partial class StackList : PanelListBase
    {
        #region 公开属性
        public static readonly DependencyProperty OrientationProperty = 
            DependencyProperty.RegisterAttached("Orientation", typeof(Orientation), typeof(StackList), new PropertyMetadata(Orientation.Vertical, OnOrientationChanged));
        private static void OnOrientationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is StackList stack)
            {
                if (Method.Child(stack, out StackPanel stackPanel))
                {
                    stackPanel.Orientation = (Orientation)e.NewValue;
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

        public StackList()
        {
            InitializeComponent();
            base.Init(stackPanel, scrollViewer);
        }
    }
}
