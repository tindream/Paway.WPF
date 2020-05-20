using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;

namespace Paway.WPF
{
    internal static class DpiHelper
    {
        /// <summary>
        /// Convert a point in system coordinates to a point in device independent pixels (1/96").
        /// </summary>
        /// <param name="deviceSize"></param>
        /// <param name="dpiScaleX"></param>
        /// <param name="dpiScaleY"></param>
        /// <returns></returns>
        public static Size DeviceSizeToLogical(Size deviceSize, double dpiScaleX, double dpiScaleY)
        {
            var _transformToDip = Matrix.Identity;
            _transformToDip.Scale(1d / dpiScaleX, 1d / dpiScaleY);
            var pt = _transformToDip.Transform(new Point(deviceSize.Width, deviceSize.Height));
            return new Size(pt.X, pt.Y);
        }
    }
}
