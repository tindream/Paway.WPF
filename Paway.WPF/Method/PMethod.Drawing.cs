using System;
using System.Drawing;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法
    /// </summary>
    public partial class PMethod : TMethod
    {
        /// <summary>
        /// 椭圆求指定角度坐标点公式
        /// </summary>
        /// <param name="lpRect">椭圆边框</param>
        /// <param name="angle">角度</param>
        /// <returns></returns>
        public static Point GetArcPoint(Rectangle lpRect, double angle)
        {
            Point pt = new Point();
            double a = lpRect.Width / 2.0f;
            double b = lpRect.Height / 2.0f;
            if (a == 0 || b == 0) return new Point(lpRect.X, lpRect.Y);

            //弧度
            double radian = angle * Math.PI / 180.0f;

            //获取弧度正弦值
            double yc = Math.Sin(radian);
            //获取弧度余弦值
            double xc = Math.Cos(radian);
            //获取曲率  r = ab/\Sqrt((a.Sinθ)^2+(b.Cosθ)^2
            double radio = (a * b) / Math.Sqrt(Math.Pow(yc * a, 2.0) + Math.Pow(xc * b, 2.0));

            //计算坐标
            double ax = radio * xc;
            double ay = radio * yc;
            pt.X = (int)(lpRect.X + a + ax);
            pt.Y = (int)(lpRect.Y + b + ay);
            return pt;
        }
    }
}
