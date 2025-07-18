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
    /// ImageEXTWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ImageEXTWindow : WindowEXT
    {
        /// <summary>
        /// Image显示窗体
        /// </summary>
        public ImageEXTWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// ImageEXTWindow显示窗体
        /// </summary>
        public ImageEXTWindow(string name, ImageSource source) : this()
        {
            this.Title = name;
            this.image.Source = source;
        }
        /// <summary>
        /// ImageEXTWindow显示窗体
        /// </summary>
        public ImageEXTWindow(string name, string filePath) : this()
        {
            this.Title = name;
            this.image.Source = filePath.ToSource();
        }
    }
}
