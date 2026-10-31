using Paway.Helper;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// TickBar扩展
    /// </summary>
    public partial class TickBarEXT : TickBar
    {
        private double normalWidth;
        private double normalHeight;

        #region 自定义刻度值重写路由事件
        /// <summary>
        /// <parm>通过首先注册RoutedEventID创建自定义路由事件Create a custom routed event by first registering a RoutedEventID</parm>
        /// <parm>此事件使用冒泡路由策略This event uses the bubbling routing strategy</parm>
        /// </summary>
        public static readonly RoutedEvent TrackValueChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(TrackValueChanged), RoutingStrategy.Tunnel, typeof(EventHandler<ValueChangeEventArgs>), typeof(TickBarEXT));
        /// <summary>
        /// 为事件提供CLR访问器Provide CLR accessors for the event
        /// </summary>
        public event EventHandler<ValueChangeEventArgs> TrackValueChanged
        {
            add { AddHandler(TrackValueChangedEvent, value); }
            remove { RemoveHandler(TrackValueChangedEvent, value); }
        }
        /// <summary>
        /// 此方法引发TrackValue事件This method raises the TrackValue event
        /// </summary>
        private double OnTrackValueChanged(double value)
        {
            var arg = new ValueChangeEventArgs(value, TrackValueChangedEvent, this);
            RaiseEvent(arg);
            return arg.Value;
        }

        #endregion

        /// <summary>
        /// 重写刻度值
        /// </summary>
        protected override void OnRender(DrawingContext dc)
        {
            var trackButtonWidth = 0.0;
            var showTrackText = false;
            if (this.Parent(out SliderEXT slider))
            {
                trackButtonWidth = slider.TrackButtonWidth;
                showTrackText = slider.ShowTrackText;
            }
            var showTrackTextLength = showTrackText ? 2 : 0;
            if (this.normalWidth == 0) this.normalWidth = this.Width;
            if (this.normalHeight == 0) this.normalHeight = this.Height;
            Size size = new Size(base.ActualWidth, base.ActualHeight);
            //Maximum == Minimum 或 TickFrequency <= 0 时下面会除零得到 NaN/∞，绘制出非法坐标
            var range = this.Maximum - this.Minimum;
            if (range <= 0 || this.TickFrequency <= 0) return;
            int tickCount = (int)(range / this.TickFrequency);
            if (range % this.TickFrequency > 0) tickCount++;
            var horizontal = size.Width > size.Height;
            var length = horizontal ? size.Width : size.Height;

            // Calculate tick's setting 
            var tickFrequencySize = length * this.TickFrequency / range;
            // Draw each tick text 
            var color = PConfig.Foreground.AddLight(30).ToBrush();
            var maxWidth = 0.0;
            var maxHeight = 0.0;
            //Fill 为 null 时 new Pen 会抛异常
            var pen = new Pen(this.Fill ?? Brushes.Transparent, 1);
            if (pen.CanFreeze) pen.Freeze();

            //每个刻度值只计算一次：原实现测量与绘制两轮循环各抛一遍 TrackValueChanged，产生 2N 次路由事件
            var values = new double[tickCount + 1];
            for (var i = 0; i <= tickCount; i++)
            {
                var value = (this.Minimum + this.TickFrequency * (horizontal ? i : tickCount - i)).ToDouble();
                values[i] = OnTrackValueChanged(value);
            }

            //复用同一个 FormattedText：原实现每帧分配 2×tickCount 个（含 Typeface），滚动/拖动时是明确的GC热点
            var drawText = showTrackText && this.Name == "TopTick";
            var texts = drawText ? new FormattedText[tickCount + 1] : null;
            if (drawText)
            {
                var pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
                for (var i = 0; i <= tickCount; i++)
                {
                    //FormattedText.Text 只读，只能按刻度各建一个；但测量与绘制共用同一批对象，不再重复分配
                    var formattedText = new FormattedText(values[i].ToString(), CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight, tickTypeface, 8, color, pixelsPerDip);
                    texts[i] = formattedText;
                    if (maxWidth < formattedText.Width) maxWidth = formattedText.Width;
                    if (maxHeight < formattedText.Height) maxHeight = formattedText.Height;
                }
            }
            if (horizontal)
            {
                this.Height = this.normalHeight + maxHeight + showTrackTextLength;
                this.Margin = new Thickness(trackButtonWidth / 2, 0, trackButtonWidth / 2, 0);
            }
            else
            {
                this.Width = this.normalWidth + maxWidth + showTrackTextLength;
                this.Margin = new Thickness(0, trackButtonWidth / 2, 0, trackButtonWidth / 2);
            }
            for (var i = 0; i <= tickCount; i++)
            {
                var x = tickFrequencySize * i;
                if (horizontal)
                {
                    dc.DrawLine(pen, new Point(x, maxHeight + showTrackTextLength), new Point(x, maxHeight + showTrackTextLength + this.normalHeight));
                    if (drawText)
                    {
                        dc.DrawText(texts[i], new Point(x - texts[i].Width / 2, maxHeight - texts[i].Height));
                    }
                }
                else
                {
                    dc.DrawLine(pen, new Point(maxWidth + showTrackTextLength, x), new Point(maxWidth + showTrackTextLength + this.normalWidth, x));
                    if (drawText)
                    {
                        dc.DrawText(texts[i], new Point(maxWidth - texts[i].Width, x - texts[i].Height / 2));
                    }
                }
            }
        }
        private static readonly Typeface tickTypeface = new Typeface("Verdana");
    }
}
