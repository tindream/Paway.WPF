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
            using (var binaryReader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                var fileInfo = new FileInfo(file);
                var buffer = binaryReader.ReadBytes((int)fileInfo.Length);
                return ToSource(buffer);
            }
        }
        /// <summary>
        /// 内存流转图片资源
        /// </summary>
        public static BitmapSource ToSource(this byte[] buffer)
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
        public static BitmapSource ToSource(this Bitmap bitmap)
        {
            var intPtr = bitmap.GetHbitmap();
            var image = Imaging.CreateBitmapSourceFromHBitmap(intPtr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            NativeMethods.DeleteObject(intPtr);
            return image;
        }

        #endregion
    }
}