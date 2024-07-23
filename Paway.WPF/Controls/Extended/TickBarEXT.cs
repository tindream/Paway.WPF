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
            if (PMethod.Parent(this, out SliderEXT slider))
            {
                trackButtonWidth = slider.TrackButtonWidth;
                showTrackText = slider.ShowTrackText;
            }
            var showTrackTextLength = showTrackText ? 2 : 0;
            if (this.normalWidth == 0) this.normalWidth = this.Width;
            if (this.normalHeight == 0) this.normalHeight = this.Height;
            Size size = new Size(base.ActualWidth, base.ActualHeight);
            int tickCount = (int)((this.Maximum - this.Minimum) / this.TickFrequency);
            if ((this.Maximum - this.Minimum) % this.TickFrequency > 0) tickCount++;
            var horizontal = size.Width > size.Height;
            var length = horizontal ? size.Width : size.Height;

            // Calculate tick's setting 
            var tickFrequencySize = length * this.TickFrequency / (this.Maximum - this.Minimum);
            // Draw each tick text 
            var color = PConfig.Foreground.AddLight(30).ToBrush();
            var maxWidth = 0.0;
            var maxHeight = 0.0;
            var pen = new Pen(this.Fill, 1);
            if (showTrackText)
            {
                for (var i = 0; i <= tickCount; i++)
                {
                    var value = (this.Minimum + this.TickFrequency * (horizontal ? i : tickCount - i)).ToDouble();
                    value = OnTrackValueChanged(value);
                    //var pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
                    var formattedText = new FormattedText(value.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 8, color);
                    if (this.Name == "TopTick")
                    {
                        if (maxWidth < formattedText.Width) maxWidth = formattedText.Width;
                        if (maxHeight < formattedText.Height) maxHeight = formattedText.Height;
                    }
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
                var value = (this.Minimum + this.TickFrequency * (horizontal ? i : tickCount - i)).ToDouble();
                value = OnTrackValueChanged(value);
                //var pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
                var formattedText = new FormattedText(value.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 8, color);

                var x = tickFrequencySize * i;
                if (horizontal)
                {
                    dc.DrawLine(pen, new Point(x, maxHeight + showTrackTextLength), new Point(x, maxHeight + showTrackTextLength + this.normalHeight));
                    if (this.Name == "TopTick" && showTrackText) dc.DrawText(formattedText, new Point(x - formattedText.Width / 2, maxHeight - formattedText.Height));
                }
                else
                {
                    dc.DrawLine(pen, new Point(maxWidth + showTrackTextLength, x), new Point(maxWidth + showTrackTextLength + this.normalWidth, x));
                    if (this.Name == "TopTick" && showTrackText) dc.DrawText(formattedText, new Point(maxWidth - formattedText.Width, x - formattedText.Height / 2));
                }
            }
        }
    }
}
