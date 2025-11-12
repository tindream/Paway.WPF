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
using Color = System.Windows.Media.Color;

namespace Paway.WPF
{
    /// <summary>
    /// WPF图像转换器
    /// </summary>
    public static partial class ImageSourceHelper
    {
        #region 转换
        /// <summary>
        /// 文件转图片资源(不占用文件)
        /// </summary>
        public static BitmapSource ToSource(this string fileName)
        {
            try
            {
                if (fileName.IsEmpty()) return null;
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
        /// ImageSource转BitmapSource
        /// </summary>
        public static BitmapSource ToSource(this ImageSource imageSource)
        {
            if (imageSource is BitmapSource bitmapSource)
            {
                return bitmapSource;
            }

            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(imageSource, new Rect(0, 0, imageSource.Width, imageSource.Height));
            }

            var bitmap = new RenderTargetBitmap((int)imageSource.Width, (int)imageSource.Height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(drawingVisual);
            return bitmap;
        }
        /// <summary>
        /// BitmapSource保存到文件
        /// </summary>
        public static void Save(this BitmapSource bitmapSource, string fileName, int quality = 70)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                var extension = Path.GetExtension(fileName)?.ToLower();
                BitmapEncoder encoder = null;
                switch (extension)
                {
                    case ".jpg":
                    default:
                        var encoderJPG = new JpegBitmapEncoder();
                        encoderJPG.QualityLevel = quality;
                        encoder = encoderJPG;
                        break;
                    case ".png":
                        encoder = new PngBitmapEncoder();
                        break;
                }
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(fileStream);
            }
        }
        /// <summary>
        /// 获取图像坐标颜色
        /// </summary>
        public static Color PixelColor(this BitmapSource bitmapSource, int x, int y)
        {
            if (x < 0 || x >= bitmapSource.PixelWidth || y < 0 || y >= bitmapSource.PixelHeight)
                return Colors.Transparent;

            // 计算stride（每行的字节数）
            int stride = (bitmapSource.PixelWidth * bitmapSource.Format.BitsPerPixel + 7) / 8;

            // 创建足够大的数组来保存单个像素
            byte[] pixel = new byte[bitmapSource.Format.BitsPerPixel / 8];

            // 复制像素数据
            bitmapSource.CopyPixels(new Int32Rect(x, y, 1, 1), pixel, stride, 0);

            // 根据格式转换颜色
            if (bitmapSource.Format == PixelFormats.Bgra32 || bitmapSource.Format == PixelFormats.Pbgra32)
            {
                return Color.FromArgb(pixel[3], pixel[2], pixel[1], pixel[0]);
            }
            else if (bitmapSource.Format == PixelFormats.Bgr32)
            {
                return Color.FromRgb(pixel[2], pixel[1], pixel[0]);
            }
            else if (bitmapSource.Format == PixelFormats.Indexed8)
            {
                return Color.FromRgb(pixel[0], pixel[0], pixel[0]);
            }
            else
            {
                return Color.FromRgb(pixel.Length > 2 ? pixel[2] : pixel[0], pixel.Length > 2 ? pixel[1] : pixel[0], pixel[0]);
            }
        }

        #endregion
    }
}