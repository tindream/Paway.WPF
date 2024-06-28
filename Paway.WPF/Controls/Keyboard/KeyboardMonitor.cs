using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Paway.WPF
{
    /// <summary>
    /// 绑定自定义虚拟键盘
    /// </summary>
    public class KeyboardMonitor
    {
        private CustomAdorner desktopAdorner;
        private Rect boxRect;
        private Rect keyboardRect;
        private FrameworkElement element;
        private KeyboardType Keyboard;

        /// <summary>
        /// 绑定自定义虚拟键盘
        /// </summary>
        public KeyboardMonitor(FrameworkElement element, KeyboardType type = KeyboardType.All)
        {
            element.PreviewMouseDown -= Element_PreviewMouseDown;
            element.PreviewMouseDown += Element_PreviewMouseDown;
            element.GotFocus -= Element_GotFocus;
            element.GotFocus += Element_GotFocus;
            element.LostFocus -= Element_LostFocus;
            element.LostFocus += Element_LostFocus;
            this.element = element;
            this.Keyboard = type;
        }
        /// <summary>
        /// 鼠标点击时自动打开虚拟键盘
        /// </summary>
        private void Element_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.OpenKeyboard();
        }
        /// <summary>
        /// 获取焦点时自动打开虚拟键盘
        /// </summary>
        private void Element_GotFocus(object sender, RoutedEventArgs e)
        {
            this.OpenKeyboard();
        }
        private void OpenKeyboard()
        {
            if (!element.IsLoaded || !element.IsEnabled) return;
            if (element is TextBoxBase textBox && textBox.IsReadOnly) return;
            if (desktopAdorner == null && PMethod.Parent(element, out Window owner) && owner.Content is FrameworkElement content)
            {
                var keyboardType = Keyboard;
                if (keyboardType == KeyboardType.Auto && element is TextBoxBase)
                {
                    Binding binding = BindingOperations.GetBinding(element, TextBox.TextProperty);
                    if (binding != null && element.DataContext != null)
                    {
                        var property = element.DataContext.Property(binding.Path.Path);
                        if (property != null && property.PropertyType == typeof(string)) keyboardType = KeyboardType.All;
                        else keyboardType = KeyboardType.Num;
                    }
                    else keyboardType = KeyboardType.All;
                }
                owner.PreviewMouseLeftButtonDown += Owner_PreviewMouseLeftButtonDown;
                FrameworkElement keyboard;
                switch (keyboardType)
                {
                    case KeyboardType.Num: var keyboardNum = new KeyboardNum(); keyboard = keyboardNum; keyboardNum.CloseEvent += CloseKeyboard; break;
                    default:
                    case KeyboardType.All: var keyboardAll = new KeyboardAll(); keyboard = keyboardAll; keyboardAll.CloseEvent += CloseKeyboard; break;
                }
                var point = element.TransformToAncestor(content).Transform(new Point(0, 0));
                var ownerPoint = element.TransformToAncestor(owner).Transform(new Point(0, 0));
                this.boxRect = new Rect(ownerPoint.X, ownerPoint.Y, element.ActualWidth, element.ActualHeight);
                desktopAdorner = PMethod.CustomAdorner(owner, keyboard, true, false);
                if (desktopAdorner != null)
                {
                    var x = point.X;
                    if (content.ActualWidth < keyboard.Width + x)
                    {
                        x = content.ActualWidth - keyboard.Width;
                    }
                    var y = point.Y + element.ActualHeight;
                    if (content.ActualHeight < keyboard.Height + y)
                    {
                        y = point.Y - keyboard.Height;
                    }
                    Canvas.SetLeft(keyboard, x);
                    Canvas.SetTop(keyboard, y);
                    var ownerPoint2 = content.TransformToAncestor(owner).Transform(new Point(x, y));
                    this.keyboardRect = new Rect(ownerPoint2.X, ownerPoint2.Y, keyboard.Width, keyboard.Height);
                }
            }
        }

        /// <summary>
        /// 失去焦点时自动关闭虚拟键盘
        /// </summary>
        private void Element_LostFocus(object sender, RoutedEventArgs e)
        {
            this.CloseKeyboard();
        }
        private void Owner_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is IInputElement element)
            {
                var point = e.GetPosition(element);
                if (desktopAdorner != null && !boxRect.Contains(point) && !keyboardRect.Contains(point))
                {
                    this.CloseKeyboard();
                }
            }
        }
        private void CloseKeyboard()
        {
            if (desktopAdorner != null && PMethod.Parent(element, out Window owner) && owner.Content is FrameworkElement content)
            {
                owner.PreviewMouseLeftButtonDown -= Owner_PreviewMouseLeftButtonDown;
                var myAdornerLayer = PMethod.ReloadAdorner(content);
                if (myAdornerLayer == null) return;

                lock (myAdornerLayer) myAdornerLayer.Remove(desktopAdorner);
                desktopAdorner = null;
                KeyboardHelper.StopHook();
            }
        }
    }
}
