using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// PlotModel
    /// </summary>
    public class PlotHelper
    {
        #region CategoryAxis
        /// <summary>
        /// 饼状图
        /// </summary>
        public static PlotModel LoadCategoryAxis(List<double> valueList, List<string> labelList, Func<object, string, string, object, string, object, string> action = null)
        {
            var model = new PlotModel
            {
                PlotAreaBorderColor = OxyColor.FromArgb(255, 86, 162, 226),
                PlotAreaBorderThickness = new OxyThickness(0)
            };

            var yAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                TextColor = OxyColors.White,//文本颜色
                MajorTickSize = 0,
                MinorTickSize = 0,
                IsZoomEnabled = false,//坐标轴缩放功能
                IsPanEnabled = false,//图表缩放功能
                Minimum = -0.5,
            };
            foreach (var item in labelList) yAxis.ActualLabels.Add(item);
            model.Axes.Add(yAxis);

            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                TextColor = OxyColors.Transparent,//文本颜色
                MajorTickSize = 0,
                MinorTickSize = 0,
                FontSize = 0.1,
                Minimum = 0,
                //StringFormat = "#,0.#",
                //Maximum = valueList.Max() * 1.15,
                IsZoomEnabled = false,//坐标轴缩放功能
                IsPanEnabled = false,//图表缩放功能
            };
            model.Axes.Add(xAxis);

            var series = new BarSeries
            {
                FillColor = OxyColor.FromArgb(255, 40, 255, 252),
                TextColor = OxyColors.White,//文本颜色
                BarWidth = 24,
                //LabelFormatString = "{0:#,0.#}",
                TrackerFormatString = "{1}：{2:#,0.#}",
                TrackerFormatStringAction = action
            };
            model.Series.Add(series);
            UpdateLine(model, valueList, false);

            return model;
        }
        /// <summary>
        /// 更新值
        /// </summary>
        public static void UpdateLine(PlotModel model, List<double> valueList, bool Invalidate = true)
        {
            var series = model.Series[0] as BarSeries;
            series.Items.Clear();
            foreach (var item in valueList) series.Items.Add(new BarItem(item));
            if (Invalidate) model.InvalidatePlot(true);
        }

        #endregion

        #region LinearAxis
        /// <summary>
        /// 线状图
        /// </summary>
        public static PlotModel LoadLinearAxis(int count, double maxValue, Func<object, string> xAction = null, bool yZoom = false, bool xZoom = true)
        {
            var model = new PlotModel
            {
                PlotAreaBorderColor = OxyColor.FromArgb(255, 38, 132, 143),
                PlotAreaBorderThickness = new OxyThickness(1, 0, 0, 1)
            };
            var legend = new Legend
            {
                LegendPosition = LegendPosition.TopCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPlacement = LegendPlacement.Inside,
                LegendTextColor = OxyColors.White,
                SeriesInvisibleTextColor = OxyColors.Gold,
                LegendPadding = -2
            };
            model.Legends.Add(legend);

            //定义y轴
            var leftAxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                //TicklineColor = OxyColors.Transparent,//刻度线颜色
                //AxislineColor = OxyColors.Red,//小刻度线颜色
                TextColor = OxyColors.White,//文本颜色
                MajorTickSize = 0,
                MinorTickSize = 0,
                //FontSize = 0.1,
                Minimum = 0,
                Maximum = maxValue * 1.15,
                //Title = "Y轴",//显示标题内容
                //TitlePosition = 0,//显示标题位置
                //TitleColor = OxyColor.Parse("#d3d3d3"),//显示标题颜色
                IsZoomEnabled = yZoom,//false:坐标轴缩放功能
                IsPanEnabled = yZoom,//false:图表缩放功能
                //MajorGridlineStyle = LineStyle.Solid,//主刻度设置格网
                //MajorGridlineColor = OxyColors.Red,
                //MinorGridlineStyle = LineStyle.Dot,//子刻度设置格网样式
                //MinorGridlineColor = OxyColors.Blue,
                IntervalLength = 30
            };
            model.Axes.Add(leftAxis);

            //定义x轴
            var bottomAxis = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColors.Transparent,
                TextColor = OxyColors.White,
                MajorTickSize = 0,
                MinorTickSize = 0,
                IntervalLength = 6,
                StringFormatAction = xAction,
                IsZoomEnabled = xZoom,
                IsPanEnabled = xZoom,
                Minimum = -2,
                Maximum = count * 7,
            };
            model.Axes.Add(bottomAxis);

            return model;
        }
        /// <summary>
        /// 添加坐标系
        /// </summary>
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
        /// <summary>
        /// 添加曲线
        /// </summary>
        public static LineSeries AddLine(PlotModel plotModel, PlotLineType type, bool first = true)
        {
            var line = new LineSeries()
            {
                Title = type.Description(),
                //MarkerType = MarkerType.Circle,
                //InterpolationAlgorithm = InterpolationAlgorithms.CatmullRomSpline,
                //Smooth = true,
                TrackerFormatString = "\n{0}: {4:0,0.00}"
            };
            if (type.Tag() is byte[] colors)
            {
                line.Color = OxyColor.FromRgb(colors[0], colors[1], colors[2]);
            }
            if (first) line.TrackerFormatString = "时间: {2}\n{0}: {4:0,0.00}";
            plotModel.Series.Add(line);
            return line;
        }
        /// <summary>
        /// 添加曲线
        /// </summary>
        public static void AddLine(PlotModel model, int index, string title, OxyColor color, List<double> valueList, Func<object, string, string, object, string, object, string> action = null, string stringFormat = "{2}: {4:#,0.#}")
        {
            var line = new LinearBarSeries()
            {
                Title = title,
                FillColor = color,
                //BarWidth = 25,
                TextColor = OxyColors.White,
                //LabelFormatString = "{1:#,0.#}",

                //MarkerType = MarkerType.Circle,
                //InterpolationAlgorithm = InterpolationAlgorithms.CatmullRomSpline,
                TrackerFormatString = stringFormat,
                TrackerFormatStringAction = action
            };
            for (var i = 0; i < valueList.Count; i++) line.Points.Add(new DataPoint(i * 7 + index, valueList[i]));
            model.Series.Add(line);
        }
        /// <summary>
        /// 自动曲线最大最小值到坐标系
        /// </summary>
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

        #endregion
    }
}