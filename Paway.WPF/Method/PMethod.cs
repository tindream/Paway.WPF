using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法
    /// </summary>
    public partial class PMethod : TMethod
    {
        #region Resources
        /// <summary>
        /// 从Resource文件读取文本
        /// </summary>
        public static string ResourceText(Uri uri)
        {
            var info = Application.GetResourceStream(uri);
            using (var reader = new StreamReader(info.Stream, Encoding.UTF8))
            {
                var txt = reader.ReadToEnd();
                return txt;
            }
        }
        /// <summary>
        /// 从Resource文件读取byte[]
        /// </summary>
        public static byte[] ResourceBuffer(Uri uri)
        {
            var info = Application.GetResourceStream(uri);
            using (info.Stream)
            {
                var buffer = new byte[info.Stream.Length];
                info.Stream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        #endregion

        #region 颜色
        /// <summary>
        /// 取颜色拾取器中的颜色值
        /// </summary>
        /// <param name="offset"></param>
        public static Color ColorSelector(double offset)
        {
            if (!(Application.Current.FindResource("ColorSelector") is LinearGradientBrush linearGradientBrush)) return Colors.Transparent;

            var collection = linearGradientBrush.GradientStops;
            var stops = collection.OrderBy(x => x.Offset).ToArray();
            if (offset <= 0) return stops[0].Color;
            if (offset >= 1) return stops[stops.Length - 1].Color;
            var left = stops.Where(s => s.Offset <= offset).Last();
            var right = stops.Where(s => s.Offset > offset).First();
            offset = Math.Round((offset - left.Offset) / (right.Offset - left.Offset), 2);
            var a = (byte)((right.Color.A - left.Color.A) * offset + left.Color.A);
            var r = (byte)((right.Color.R - left.Color.R) * offset + left.Color.R);
            var g = (byte)((right.Color.G - left.Color.G) * offset + left.Color.G);
            var b = (byte)((right.Color.B - left.Color.B) * offset + left.Color.B);
            return Color.FromArgb(a, r, g, b);
        }
        /// <summary>
        /// 指定Alpha颜色
        /// </summary>
        public static Color AlphaColor(int alpha, Color color)
        {
            if (color == Colors.Transparent) alpha = 0;
            if (alpha < 0) alpha = 0;
            else if (alpha > 255) alpha = 255;
            return Color.FromArgb((byte)alpha, color.R, color.G, color.B);
        }

        #endregion

        #region 位移动画
        /// <summary>
        /// 计算动画时间
        /// </summary>
        public static double AnimTime(double value, int minTime = 250)
        {
            var animTime = (int)(Math.Pow(Math.Abs(value), 1.0 / 4) * 100);
            if (animTime < minTime) animTime = minTime;
            if (animTime > 1000) animTime = 1000;
            return animTime;
        }

        #endregion
    }
}
