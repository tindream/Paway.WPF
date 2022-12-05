using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法 - 内部
    /// </summary>
    public partial class PMethod
    {
        /// <summary>
        /// 主题颜色
        /// </summary>
        internal static Color ThemeColor(int alpha = PConfig.Alpha)
        {
            return AlphaColor(alpha, PConfig.Color);
        }

        /// <summary>
        /// 鼠标移入移出动画
        /// </summary>
        internal static void Animation(FrameworkElement obj, bool value, string line1Name = "line1", string line2Name = "line2")
        {
            PMethod.Child(obj, out Line line1, line1Name, false);
            PMethod.Child(obj, out Line line2, line2Name, false);
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
        /// 回车、Tab时移动焦点到下一控件
        /// </summary>
        internal static void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                // Gets the element with keyboard focus.
                // Change keyboard focus. 
                //if (Keyboard.FocusedElement is UIElement elementWithFocus)
                if (e.Source is UIElement elementWithFocus)
                {
                    // MoveFocus takes a TraveralReqest as its argument.
                    var direction = FocusNavigationDirection.Down;
                    if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) direction = FocusNavigationDirection.Up;
                    var request = new TraversalRequest(direction);
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
            Func<string, I> funcParse, Func<T, string, I?> funcOld)
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
                if (!string.IsNullOrEmpty(strs[0])) normal = funcParse(strs[0]);
                else normal = funcOld(old, "Normal");
            }
            if (strs.Length == 2 && byte.TryParse(strs[1], out byte alphaValue))
            {
                alpha = alphaValue;
            }
            else if (strs.Length > 1)
            {
                if (!string.IsNullOrEmpty(strs[1])) mouse = funcParse(strs[1]);
                else mouse = funcOld(old, "Mouse");
            }
            if (strs.Length > 2)
            {
                if (!string.IsNullOrEmpty(strs[2])) pressed = funcParse(strs[2]);
                else pressed = funcOld(old, "Pressed");
            }
            return new ElementData<T, I>(old, normal, mouse, pressed, alpha);
        }

        #endregion
    }
}
