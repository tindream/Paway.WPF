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

namespace Paway.WPF
{
    /// <summary>
    /// PImageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PImageWindow : WindowEXT
    {
        /// <summary>
        /// Image显示窗体
        /// </summary>
        public PImageWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// PImageWindow显示窗体
        /// </summary>
        public PImageWindow(string name, BitmapSource source) : this()
        {
            this.Title = name;
            this.image.Source = source;
        }
        /// <summary>
        /// PImageWindow显示窗体
        /// </summary>
        public PImageWindow(string name, string filePath) : this()
        {
            this.Title = name;
            this.image.Source = filePath.ToSource();
        }
    }
}
