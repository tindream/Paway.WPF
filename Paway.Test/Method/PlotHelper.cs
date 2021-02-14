using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Paway.Test
{
    /// <summary>
    /// PlotModel
    /// </summary>
    public class PlotHelper
    {
        public static void UpdateLine(PlotModel plotModel, MonitorType monitorType, MonitorType all)
        {
            if (plotModel == null) return;
            for (int i = plotModel.Series.Count - 1; i >= 0; i--)
            {
                var value = plotModel.Series[i].Title.Parse<MonitorType>();
                if ((monitorType & value) == 0)
                {
                    plotModel.Series.RemoveAt(i);
                }
            }
            var list = WPF.TMethod.List<MonitorType>(0);
            foreach (var item in list)
            {
                if ((all & item) != 0 && (monitorType & item) != 0)
                {
                    if (plotModel.Series.FirstOrDefault(c => c.Title == item.Description()) == null)
                    {
                        PlotHelper.AddLine(plotModel, item);
                    }
                }
            }
            for (int i = 0; i < plotModel.Series.Count; i++)
            {
                var line = plotModel.Series[i] as LineSeries;
                if (i == 0)
                {
                    line.TrackerFormatString = "Time: {2}\r\n{0}: {4:0.0}";
                }
                else
                {
                    line.TrackerFormatString = "\r\n{0}: {4:0.0}";
                }
            }
        }

        public static LineSeries AddLine(PlotModel plotModel, MonitorType type, bool first = true)
        {
            var line = new LineSeries()
            {
                Title = type.Description(),
                //MarkerType = MarkerType.Circle,
                //InterpolationAlgorithm = InterpolationAlgorithms.CatmullRomSpline,
                //Smooth = true,
                TrackerFormatString = "\r\n{0}: {4:0,0.00}"
            };
            if (type.Tag() is byte[] colors)
            {
                line.Color = OxyColor.FromRgb(colors[0], colors[1], colors[2]);
            }
            if (first) line.TrackerFormatString = "时间: {2}\r\n{0}: {4:0,0.00}";
            plotModel.Series.Add(line);
            return line;
        }
        public static void AddXY(PlotModel plotModel, bool yZoom = false, bool xZoom = true)
        {
            //定义y轴
            var leftAxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                //Minimum = 0,
                //Maximum = 100,
                //Title = "Y轴",//显示标题内容
                //TitlePosition = 0,//显示标题位置
                //TitleColor = OxyColor.Parse("#d3d3d3"),//显示标题颜色
                IsZoomEnabled = yZoom,//false:坐标轴缩放关闭
                IsPanEnabled = yZoom,//false:图表缩放功能关闭
                //MajorGridlineStyle = LineStyle.Solid,//主刻度设置格网
                //MajorGridlineColor = OxyColor.Parse("#7379a0"),
                //MinorGridlineStyle = LineStyle.Dot,//子刻度设置格网样式
                //MinorGridlineColor = OxyColor.Parse("#666b8d")
            };
            plotModel.Axes.Add(leftAxis);
            //定义x轴
            var bottomAxis = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(1)),
                IsZoomEnabled = xZoom,//false:坐标轴缩放关闭
                IsPanEnabled = xZoom,//false:图表缩放功能关闭
                IntervalLength = 45,
                MinorIntervalType = DateTimeIntervalType.Seconds,
                IntervalType = DateTimeIntervalType.Seconds,
            };
            plotModel.Axes.Add(bottomAxis);
        }
        public static void AddPoint(PlotModel plotModel, DateTime time, TempInfo temp)
        {
            bool bToMove = false;
            for (int i = 0; i < plotModel.Series.Count; i++)
            {
                var line = plotModel.Series[i] as LineSeries;
                var type = line.Title.Parse<MonitorType>();
                var value = (double)temp.GetValue(type.ToString());
                line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time), value));
            }

            var bottomAxis = plotModel.Axes.FirstOrDefault(c => c.Position == AxisPosition.Bottom);
            if (!bToMove)
            {
                //当前时间减去起始时间达到30秒后开始左移时间轴
                TimeSpan timeSpan = DateTime.Now - DateTimeAxis.ToDateTime(bottomAxis.Minimum);
                if (timeSpan.TotalSeconds >= 40)
                {
                    bToMove = true;
                }
            }
            if (bToMove)
            {
                //左移时间轴，跨度维持在60秒
                var start = DateTime.Now.AddSeconds(-40);
                bottomAxis.Minimum = DateTimeAxis.ToDouble(start);
                bottomAxis.Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(20));

                //删除历史节点，防止DataPoint过多影响效率，也防止出现内存泄漏        
                for (int i = 0; i < plotModel.Series.Count; i++)
                {
                    var line = plotModel.Series[i] as LineSeries;
                    while (line.Points.Count > 0)
                    {
                        if (line.Points[0].X < DateTimeAxis.ToDouble(start))
                            line.Points.RemoveAt(0);
                        else break;
                    }
                }
            }
            AutoMaxMin(plotModel);
            //刷新视图
            plotModel.InvalidatePlot(true);
        }
        public static void AutoMaxMin(PlotModel plotModel, double max = double.NaN, double min = double.NaN)
        {
            //根据报单笔数判断是否需要更新y轴刻度     
            //首先找出统计线中当前最大的节点
            for (int i = 0; i < plotModel.Series.Count; i++)
            {
                var value = (plotModel.Series[i] as LineSeries).MaxY;
                if (max < value) max = value;
                value = (plotModel.Series[i] as LineSeries).MinY;
                if (min > value) min = value;
            }
            var total = max - min;
            total *= 0.1;
            max += total;
            min -= total;

            var leftAxis = plotModel.Axes.FirstOrDefault(c => c.Position == AxisPosition.Left);
            //如果当前的y轴最大刻度小于数据集中的最大值，放大
            if (!min.Equals(double.NaN)) leftAxis.Minimum = min;
            if (!max.Equals(double.NaN)) leftAxis.Maximum = max;
        }
    }
}