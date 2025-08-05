using Paway.Helper;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Paway.Model
{
    /// <summary>
    /// 星星背景动画
    /// </summary>
    public class StarAdorner
    {
        #region 私有成员变量
        /// <summary>
        /// 容器
        /// </summary>
        private Canvas starContainer;
        /// <summary>
        /// 星星PathData
        /// </summary>
        private readonly Geometry pathStar = Geometry.Parse("M16.001007,0L20.944,10.533997 32,12.223022 23.998993,20.421997 25.889008,32 16.001007,26.533997 6.1109924,32 8,20.421997 0,12.223022 11.057007,10.533997z");
        /// <summary>
        /// 星星运动的最大速度
        /// </summary>
        private readonly int _starVMax = 60;
        /// <summary>
        /// 星星转动的最小速度
        /// </summary>
        private readonly int _starRVMin = 30;
        /// <summary>
        /// 星星转动的最大速度
        /// </summary>
        private readonly int _starRVMax = 360;
        /// <summary>
        /// 随机数
        /// </summary>
        private readonly Random _random = new Random();
        /// <summary>
        /// 星星数组
        /// </summary>
        private StarInfo[] _stars;

        #endregion

        /// <summary>
        /// 初始化星星
        /// </summary>
        public StarAdorner InitStar(Canvas container, int count = 60, int minSize = 5, int maxSize = 15)
        {
            this.starContainer = container;
            //清空星星容器
            _stars = new StarInfo[count];
            starContainer.Children.Clear();
            //生成星星
            for (int i = 0; i < count; i++)
            {
                double size = _random.Next(minSize, maxSize + 1);//星星尺寸
                Path star = new Path()
                {
                    Data = pathStar,
                    Width = size,
                    Height = size,
                    Stretch = Stretch.Fill,
                    Fill = GetRandomColorBursh(),
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = new RotateTransform() { Angle = 0 }
                };
                StarInfo starInfo = new StarInfo()
                {
                    Star = star,
                    X = _random.Next(0, (int)(container.ActualWidth - star.Width)),
                    XV = (double)_random.Next(-_starVMax, _starVMax) / 60,
                    XT = _random.Next(6, 301),//帧
                    Y = _random.Next(0, (int)(container.ActualHeight - star.Height)),
                    YV = (double)_random.Next(-_starVMax, _starVMax) / 60,
                    YT = _random.Next(6, 301),//帧
                };
                //设置星星旋转动画
                StarRotateAnimation(star);
                //StarMove(star);
                //添加到容器
                _stars[i] = starInfo;
                starContainer.Children.Add(star);
            }
            return this;
        }
        /// <summary>
        /// 获取随机颜色画刷(偏亮)
        /// </summary>
        private SolidColorBrush GetRandomColorBursh()
        {
            byte r = (byte)_random.Next(128, 256);
            byte g = (byte)_random.Next(128, 256);
            byte b = (byte)_random.Next(128, 256);
            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }
        /// <summary>
        /// 星星旋转动画
        /// </summary>
        private void StarRotateAnimation(Path star)
        {
            double v = _random.Next(_starRVMin, _starRVMax + 1);//速度
            double a = _random.Next(0, 360 * 5);//角度
            double t = a / v;//时间
            Duration dur = new Duration(new TimeSpan(0, 0, 0, 0, (int)(t * 1000)));

            var storyboard = new Storyboard
            {
                Duration = dur,
                RepeatBehavior = RepeatBehavior.Forever
            };
            //动画完成事件 再次设置此动画
            storyboard.Completed += (S, E) =>
            {
                //SetStarRotateAnimation(star);
            };

            DoubleAnimation da = new DoubleAnimation()
            {
                To = 360,//a
                Duration = dur
            };
            Storyboard.SetTarget(da, star);
            Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            storyboard.Children.Add(da);
            storyboard.Begin();
        }
        /// <summary>
        /// 星星漫游动画
        /// </summary>
        public void StarRoamAnimation()
        {
            if (_stars == null) return;

            foreach (StarInfo starInfo in _stars)
            {
                //X轴运动
                if (starInfo.XT > 0)
                {
                    //运动时间大于0,继续运动
                    if (starInfo.X >= starContainer.ActualWidth - starInfo.Star.Width || starInfo.X <= 0)
                    {
                        //碰到边缘,速度取反向
                        starInfo.XV = -starInfo.XV;
                    }
                    //位移加,时间减
                    starInfo.X += starInfo.XV;
                    starInfo.XT--;
                    Canvas.SetLeft(starInfo.Star, starInfo.X);
                }
                else
                {
                    //运动时间小于0,重新设置速度和时间
                    starInfo.XV = (double)_random.Next(-_starVMax, _starVMax) / 60;
                    starInfo.XT = _random.Next(100, 1001);
                }
                //Y轴运动
                if (starInfo.YT > 0)
                {
                    //运动时间大于0,继续运动
                    if (starInfo.Y >= starContainer.ActualHeight - starInfo.Star.Height || starInfo.Y <= 0)
                    {
                        //碰到边缘,速度取反向
                        starInfo.YV = -starInfo.YV;
                    }
                    //位移加,时间减
                    starInfo.Y += starInfo.YV;
                    starInfo.YT--;
                    Canvas.SetTop(starInfo.Star, starInfo.Y);
                }
                else
                {
                    //运动时间小于0,重新设置速度和时间
                    starInfo.YV = (double)_random.Next(-_starVMax, _starVMax) / 60;
                    starInfo.YT = _random.Next(100, 1001);
                }
            }
        }
    }
    /// <summary>
    /// 星星模型
    /// </summary>
    partial class StarInfo
    {
        /// <summary>
        /// X坐标
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// X轴速度(单位距离/帧)
        /// </summary>
        public double XV { get; set; }

        /// <summary>
        /// X坐标以X轴速度运行的时间(帧)
        /// </summary>
        public int XT { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Y轴速度(单位距离/帧)
        /// </summary>
        public double YV { get; set; }

        /// <summary>
        /// Y坐标以Y轴速度运行的时间(帧)
        /// </summary>
        public int YT { get; set; }

        /// <summary>
        /// 对星星的引用
        /// </summary>
        public Path Star { get; set; }
    }
}
