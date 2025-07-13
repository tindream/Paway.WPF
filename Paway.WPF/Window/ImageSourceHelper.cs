using Paway.Helper;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paway.WPF
{
    /// <summary>
    /// WPF图像转换器
    /// </summary>
    public static partial class ImageSourceHelper
    {
        /// <summary>
        /// Image转图片资源
        /// </summary>
        public static BitmapSource ToSource(this Image image)
        {
            var source = new BitmapImage();
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);

                source.BeginInit();
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.StreamSource = ms;
                source.EndInit();
            }
            source.Freeze(); // 可选：使对象跨线程可用
            return source;
        }
        /// <summary>
        /// 图像转图片资源
        /// </summary>
        public static BitmapSource ToSource(this Bitmap bitmap)
        {
            if (bitmap == null) return null;
            var intPtr = bitmap.GetHbitmap();
            var source = Imaging.CreateBitmapSourceFromHBitmap(intPtr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            NativeMethods.DeleteObject(intPtr);
            source.Freeze(); // 可选：使对象跨线程可用
            return source;
        }
        /// <summary>
        /// BitmapSource转Bitmap
        /// </summary> 
        public static Bitmap ToBitmap(this BitmapSource bitmapSource)
        {
            if (bitmapSource == null) return null;
            ////慢
            //// 创建内存流
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    // 使用 BmpBitmapEncoder 将 BitmapSource 编码为 PNG 格式
            //    BitmapEncoder enc = new PngBitmapEncoder();
            //    enc.Frames.Add(BitmapFrame.Create(bitmapSource));
            //    enc.Save(ms);

            //    // 从内存流创建 Bitmap
            //    return new Bitmap(ms);
            //}
            // 如果源图像不是32bpp格式，先转换为Pbgra32格式
            if (bitmapSource.Format != PixelFormats.Pbgra32)
            {
                var formattedBitmapSource = new FormatConvertedBitmap();
                formattedBitmapSource.BeginInit();
                formattedBitmapSource.Source = bitmapSource;
                formattedBitmapSource.DestinationFormat = PixelFormats.Pbgra32;
                formattedBitmapSource.EndInit();
                bitmapSource = formattedBitmapSource;
            }

            // 获取图像参数
            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;
            int stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8);

            // 创建字节数组来存储像素数据
            byte[] pixels = new byte[height * stride];

            // 复制像素数据
            bitmapSource.CopyPixels(pixels, stride, 0);

            // 创建 Bitmap 对象
            Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // 锁定 Bitmap 的位图数据
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            try
            {
                // 将像素数据复制到 Bitmap
                System.Runtime.InteropServices.Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);
            }
            finally
            {
                // 解锁位图数据
                bitmap.UnlockBits(bitmapData);
            }

            return bitmap;
        }
    }
}