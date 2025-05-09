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
    /// WPF图像转换器
    /// </summary>
    public static class ImageSourceHelper
    {
        #region 转换
        /// <summary>
        /// 文件转图片资源(不占用文件)
        /// </summary>
        public static BitmapSource ToSource(this string fileName)
        {
            try
            {
                var source = new BitmapImage();
                using (var fs = File.OpenRead(fileName))
                {
                    source.BeginInit();
                    source.CacheOption = BitmapCacheOption.OnLoad;//图像缓存到内存中，不会占用文件，没有被引用时会被自动回收。
                    source.StreamSource = fs;
                    source.EndInit();
                }
                source.Freeze(); // 可选：使对象跨线程可用
                return source;
            }
            catch (OutOfMemoryException ex)
            {
                if (!TMethod.IsValidImageFile(fileName))
                {
                    throw new Exception("文件不是有效的图像格式！", ex);
                }
                // 如果文件头有效，可能是真的内存不足
                throw new Exception("加载图像时内存不足！", ex);
            }
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
        /// <summary>
        /// BitmapSource保存到文件
        /// </summary>
        public static void Save(this BitmapSource bitmapSource, string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                var extension = Path.GetExtension(fileName);
                BitmapEncoder encoder = null;
                switch (extension)
                {
                    case ".jpg":
                    case ".jpeg":
                    default:
                        encoder = new JpegBitmapEncoder();
                        break;
                    case ".png":
                        encoder = new PngBitmapEncoder();
                        break;
                    case ".bmp":
                        encoder = new BmpBitmapEncoder();
                        break;
                }
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(fileStream);
            }
        }

        #endregion
    }
}