using GalaSoft.MvvmLight.Messaging;
using Paway.WPF;
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

namespace Paway.Model
{
    /// <summary>
    /// ImageAdorner.xaml 的交互逻辑
    /// </summary>
    public partial class ImageAdorner
    {
        /// <summary>
        /// 装饰器
        /// </summary>
        public CustomAdorner Adorner { get; set; }
        /// <summary>
        /// Image装饰器
        /// </summary>
        public ImageAdorner()
        {
            InitializeComponent();
            this.image.CloseEvent += Image_CloseEvent;
        }
        private void Image_CloseEvent(object sender, RoutedEventArgs e)
        {
            if (Adorner != null && PMethod.Parent(this, out Window window))
            {
                PMethod.ClearAdorner(window, this.Adorner);
                this.Adorner = null;
            }
        }

        /// <summary>
        /// Image装饰器
        /// </summary>
        public ImageAdorner(string name, BitmapSource source) : this()
        {
            this.image.Title = name;
            this.image.Source = source;
        }

        /// <summary>
        /// Image装饰器，指定显示Window
        /// </summary>
        public ImageAdorner(Window window, string name, BitmapSource source) : this(name, source)
        {
            var iExit = false;
            var exitTime = DateTime.MinValue;
            window.PreviewKeyDown += (sender, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    if (iExit && DateTime.Now.Subtract(exitTime).TotalMilliseconds < PConfig.DoubleInterval)
                    {
                        var handle = this.Adorner != null;
                        Image_CloseEvent(this, e);
                        e.Handled = handle;
                        return;
                    }
                    iExit = true;
                    exitTime = DateTime.Now;
                }
            };
            this.Adorner = PMethod.CustomAdorner(window, this, true);
            var canvas = this.Adorner.GetCanvas();
            canvas.Background = Colors.Black.ToAlpha(100).ToBrush();
        }
    }
}
