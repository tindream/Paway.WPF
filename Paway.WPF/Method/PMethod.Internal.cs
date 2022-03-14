using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Odbc;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法-内部
    /// </summary>
    public partial class PMethod
    {
        /// <summary>
        /// 鼠标移入移出动画
        /// </summary>
        internal static void Animation(FrameworkElement obj, bool value)
        {
            PMethod.Child(obj, out Line line1, "line1", false);
            PMethod.Child(obj, out Line line2, "line2", false);
            var storyboard = new Storyboard();
            if (value)
            {
                var itemWidth = (int)(obj.ActualWidth / 2);
                var itemWidth2 = obj.ActualWidth - itemWidth;
                var animTime = PMethod.AnimTime(itemWidth) / 2;
                var animX1 = new DoubleAnimation(line1.X2, itemWidth, new Duration(TimeSpan.FromMilliseconds(animTime)));
                var animX2 = new DoubleAnimation(line2.X2, itemWidth2, new Duration(TimeSpan.FromMilliseconds(animTime)));
                if (line1 != null)
                {
                    Storyboard.SetTargetName(animX1, line1.Name);
                    Storyboard.SetTargetProperty(animX1, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX1);
                }
                if (line2 != null)
                {
                    Storyboard.SetTargetName(animX2, line2.Name);
                    Storyboard.SetTargetProperty(animX2, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX2);
                }
            }
            else
            {
                var animX1 = new DoubleAnimation(line1.X2, 0, new Duration(TimeSpan.FromMilliseconds(100)));
                var animX2 = new DoubleAnimation(line2.X2, 0, new Duration(TimeSpan.FromMilliseconds(100)));
                if (line1 != null)
                {
                    Storyboard.SetTargetName(animX1, line1.Name);
                    Storyboard.SetTargetProperty(animX1, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX1);
                }
                if (line2 != null)
                {
                    Storyboard.SetTargetName(animX2, line2.Name);
                    Storyboard.SetTargetProperty(animX2, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX2);
                }
            }
            if (line1 != null) storyboard.Begin(line1);
        }

        /// <summary>
        /// 回车时移动焦点到下一控件
        /// </summary>
        internal static void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // MoveFocus takes a TraveralReqest as its argument.
                var direction = FocusNavigationDirection.Down;
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) direction = FocusNavigationDirection.Up;
                var request = new TraversalRequest(direction);
                // Gets the element with keyboard focus.
                // Change keyboard focus.
                if (Keyboard.FocusedElement is UIElement elementWithFocus)
                {
                    elementWithFocus.MoveFocus(request);
                }
                //e.Handled = true;
            }
        }

        #region TypeConverter
        /// <summary>
        /// 获取ITypeDescriptorContext中的属性值
        /// </summary>
        internal static T GetValue<T>(ITypeDescriptorContext context)
        {
            if (context != null)
            {
                var service = (IProvideValueTarget)context.GetService(typeof(IProvideValueTarget));
                if (service != null && service.TargetObject != null)
                {
                    var objType = service.TargetObject.GetType();
                    var obj = (DependencyObject)Activator.CreateInstance(objType);
                    var property = (DependencyProperty)service.TargetProperty;
                    return (T)obj.GetValue(property);
                }
            }
            return default;
        }
        /// <summary>
        /// 控件状态转换
        /// </summary>
        internal static ElementData<T, I> ElementStatu<T, I>(ITypeDescriptorContext context, CultureInfo culture, string str,
            Func<string, I> func, Func<T, string, I?> funcValue)
            //where T : IElementStatu<I> 
            where T : class
            where I : struct
        {
            var old = GetValue<T>(context);

            var strs = str.Split(';');
            I? normal = null;
            I? mouse = null;
            I? pressed = null;
            int? alpha = null;
            if (strs.Length > 0)
            {
                if (!string.IsNullOrEmpty(strs[0])) normal = func(strs[0]);
                else normal = funcValue(old, "Normal");
            }
            if (strs.Length > 1)
            {
                if (!string.IsNullOrEmpty(strs[1])) mouse = func(strs[1]);
                else mouse = funcValue(old, "Mouse");
            }
            if (strs.Length > 2)
            {
                if (!string.IsNullOrEmpty(strs[2])) pressed = func(strs[2]);
                else pressed = funcValue(old, "Pressed");
            }
            if (strs.Length > 3)
            {
                if (!string.IsNullOrEmpty(strs[3])) alpha = Convert.ToInt32(strs[3], culture);
                else if (old != null) alpha = (int)old.GetValue("Alpha");
            }
            return new ElementData<T, I>(old, normal, mouse, pressed, alpha);
        }

        #endregion
    }
}
