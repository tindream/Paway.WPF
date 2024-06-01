using Paway.Helper;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paway.WPF
{
    /// <summary>
    /// 转换器 - 图像处理
    /// </summary>
    public static partial class ConverHelper
    {
        #region 转换
        /// <summary>
        /// 文件转图片资源(不占用文件)
        /// </summary>
        public static BitmapSource ToSource(this string file)
        {
            var source = new BitmapImage();
            using (var fs = File.OpenRead(file))
            {
                source.BeginInit();
                source.CacheOption = BitmapCacheOption.OnLoad;//图像缓存到内存中，不会占用文件，没有被引用时会被自动回收。
                source.StreamSource = fs;
                source.EndInit();
            }
            return source;
        }
        /// <summary>
        /// 内存流转图片资源
        /// </summary>
        public static BitmapSource ToSource(this byte[] buffer)
        {
            var source = new BitmapImage();
            using (var ms = new MemoryStream(buffer))
            {
                source.BeginInit();
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.StreamSource = ms;
                source.EndInit();
            }
            return source;
        }
        /// <summary>
        /// 图像转图片资源
        /// </summary>
        public static BitmapSource ToSource(this Bitmap bitmap)
        {
            var intPtr = bitmap.GetHbitmap();
            var source = Imaging.CreateBitmapSourceFromHBitmap(intPtr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            NativeMethods.DeleteObject(intPtr);
            return source;
        }
        /// <summary>
        /// BitmapSource转Bitmap
        /// </summary>
        public static Bitmap ToBitmap(this BitmapSource bitmapSource)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(ms);
                return new Bitmap(ms);
            }
        }

        #endregion
    }
}