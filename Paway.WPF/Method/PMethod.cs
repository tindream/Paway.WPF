using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法
    /// </summary>
    public partial class PMethod : TMethod
    {
        static PMethod()
        {
            PConfig.InitResources();
        }

        #region Menu
        /// <summary>
        /// Menu菜单绑定多语言
        /// </summary>
        public static void LanguageMenuBinding(MenuItem menu, List<string> languageList, object model, ICommand command, string languageName)
        {
            menu.Items.Clear();
            foreach (var language in languageList)
            {
                var menuItem = new MenuItem() { Header = language };
                menuItem.Command = command;
                menuItem.CommandParameter = language;
                {  //实例化绑定对象
                    var isCheckedBinding = new Binding();
                    //设置要绑定源
                    isCheckedBinding.Source = model;//绑定ViewModel类
                    isCheckedBinding.Path = new PropertyPath(languageName);//绑定MainWindow类下的Language属性。
                    isCheckedBinding.Mode = BindingMode.TwoWay;//绑定模式双向绑定
                    isCheckedBinding.Converter = new ValueToTrue();
                    isCheckedBinding.ConverterParameter = language;
                    menuItem.SetBinding(MenuItem.IsCheckedProperty, isCheckedBinding);//设置绑定到要绑定的控件
                }
                menu.Items.Add(menuItem);
            }
        }
        #endregion

        #region 颜色
        /// <summary>
        /// 取颜色拾取器中的颜色值
        /// <para>拖动取色时高频调用，避免每次 Linq 与数组分配</para>
        /// </summary>
        /// <param name="offset"></param>
        public static Color ColorSelector(double offset)
        {
            var application = Application.Current;
            if (application == null) return Colors.Transparent;
            if (!(application.TryFindResource("ColorSelector") is LinearGradientBrush linearGradientBrush)) return Colors.Transparent;

            var stops = linearGradientBrush.GradientStops;
            var count = stops.Count;
            if (count == 0) return Colors.Transparent;
            if (offset <= 0) return stops[0].Color;
            if (offset >= 1) return stops[count - 1].Color;

            var left = stops[0];
            var right = stops[count - 1];
            for (var i = 0; i < count; i++)
            {
                var stop = stops[i];
                if (stop.Offset <= offset)
                {
                    left = stop;
                    continue;
                }
                right = stop;
                break;
            }
            var interval = right.Offset - left.Offset;
            if (interval == 0) return left.Color;
            offset = Math.Round((offset - left.Offset) / interval, 2);
            var a = (byte)((right.Color.A - left.Color.A) * offset + left.Color.A);
            var r = (byte)((right.Color.R - left.Color.R) * offset + left.Color.R);
            var g = (byte)((right.Color.G - left.Color.G) * offset + left.Color.G);
            var b = (byte)((right.Color.B - left.Color.B) * offset + left.Color.B);
            return Color.FromArgb(a, r, g, b);
        }

        #endregion
    }
}
