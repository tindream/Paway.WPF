using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Paway.Helper;

namespace Invengo.Utils
{
    public class Method
    {
        #region 一般方法
        public static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }
        private static Object ExitFrame(Object state)
        {
            ((DispatcherFrame)state).Continue = false;
            return null;
        }

        #endregion

        #region Window弹出
        public static void Debug(DependencyObject parent, string msg)
        {
            Show(parent, msg, LeveType.Debug);
        }
        public static void Warning(DependencyObject parent, string msg)
        {
            Show(parent, msg, LeveType.Warn);
        }
        public static void Error(DependencyObject parent, string msg)
        {
            Show(parent, msg, LeveType.Error);
        }
        public static void Show(DependencyObject parent, string msg, LeveType level = LeveType.Debug)
        {
            if (Parent(parent, out Window window))
            {
                switch (level)
                {
                    case LeveType.Debug:
                    default:
                        MessageBox.Show(window, msg, window.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case LeveType.Warn:
                        MessageBox.Show(window, msg, window.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                    case LeveType.Error:
                        MessageBox.Show(window, msg, window.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
        }
        public static bool Ask(DependencyObject parent, string msg)
        {
            if (Parent(parent, out Window window))
            {
                return MessageBox.Show(window, msg, window.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK;
            }
            return false;
        }
        public static Window Window(DependencyObject obj)
        {
            if (Parent(obj, out Window window)) return window;
            return null;
        }
        public static bool Parent<T>(DependencyObject obj, out T parent) where T : FrameworkElement
        {
            while (obj != null)
            {
                if (obj is T)
                {
                    parent = (T)obj;
                    return true;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
            parent = null;
            return false;
        }
        internal static bool Child<T>(DependencyObject obj, out T child) where T : FrameworkElement
        {
            var count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                var value = VisualTreeHelper.GetChild(obj, i);
                if (value is T)
                {
                    child = (T)value;
                    return true;
                }
                else
                {
                    if (Child<T>(value, out child))
                    {
                        return true;
                    }
                }
            }
            while (obj is ContentControl control)
            {
                obj = control.Content as DependencyObject;
                if (obj is T)
                {
                    child = (T)obj;
                    return true;
                }
            }
            child = null;
            return false;
        }

        #endregion
    }
}
