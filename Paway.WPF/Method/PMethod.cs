using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Odbc;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法
    /// </summary>
    public partial class PMethod : TMethod
    {
        #region Image
        /// <summary>
        /// 文件转图片资源(不占用文件)
        /// </summary>
        public static ImageSource Image(string file)
        {
            using (var binaryReader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                var fileInfo = new FileInfo(file);
                var buffer = binaryReader.ReadBytes((int)fileInfo.Length);
                return Image(buffer);
            }
        }
        /// <summary>
        /// 内存流转图片资源
        /// </summary>
        public static ImageSource Image(byte[] buffer)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(buffer);
            image.EndInit();
            return image;
        }
        /// <summary>
        /// 图像转图片资源
        /// </summary>
        public static ImageSource Image(System.Drawing.Bitmap bitmap)
        {
            var intPtr = bitmap.GetHbitmap();
            var image = Imaging.CreateBitmapSourceFromHBitmap(intPtr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            NativeMethods.DeleteObject(intPtr);
            return image;
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
