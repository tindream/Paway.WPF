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
        #region PieSeries
        /// <summary>
        /// 饼状图
        /// </summary>
        public static PlotModel LoadPieSeries(List<double> valueList, List<string> labelList, List<Color> colorList)
        {
            var model = new PlotModel
            {
                PlotAreaBorderColor = OxyColor.FromArgb(255, 86, 162, 226),
                PlotAreaBorderThickness = new OxyThickness(0)
            };

            var pie = new PieSeries
            {
                //InnerDiameter = 0.2,
                //TickLabelDistance = 0,
                //FillColor = OxyColor.FromArgb(255, 40, 255, 252),
                //TextColor = OxyColors.White,//文本颜色
                //BarWidth = 24,
                //LabelFormatString = "{0:#,0.#}",
                InsideLabelFormat = "{2:#}%({0})" + Environment.NewLine + "{1}",
                OutsideLabelFormat = null,
                TrackerFormatString = "{1}：{2:#,0.##}",
            };
            model.Series.Add(pie);

            for (var i = 0; i < labelList.Count; i++)
            {
                pie.Slices.Add(new PieSlice(labelList[i], valueList[i]));
            }
            pie.Slices[0].Fill = OxyColor.FromArgb(colorList[0].A, colorList[0].R, colorList[0].G, colorList[0].B);
            pie.Slices[1].Fill = OxyColor.FromArgb(colorList[1].A, colorList[1].R, colorList[1].G, colorList[1].B);
            pie.Slices[2].Fill = OxyColor.FromArgb(colorList[2].A, colorList[2].R, colorList[2].G, colorList[2].B);

            return model;
        }

        #endregion

        #region CategoryAxis
        /// <summary>
        /// 柱状图
        /// </summary>
        public static PlotModel LoadCategoryAxis(List<double> valueList, List<string> labelList, List<Color> colorList, Func<object, string, string, object, string, object, string> action = null)
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
                ItemsSource = labelList,
            };
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
                TextColor = OxyColors.Red,//文本颜色
                BarWidth = 24,
                //LabelFormatString = "{0:#,0.#}",
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatStringAction = (item, value) =>
                {
                    return $"  {value:#,0.#}";
                },
                TrackerFormatString = "{1}：{2:#,0.#}",
                TrackerFormatStringAction = action
            };
            model.Series.Add(series);
            UpdateLine(model, valueList, colorList, false);

            return model;
        }
        /// <summary>
        /// 更新值
        /// </summary>
        public static void UpdateLine(PlotModel plotModel, List<double> valueList, List<Color> colorList, bool Invalidate = true)
        {
            var series = plotModel.Series[0] as BarSeries;
            series.Items.Clear();
            for (var i = 0; i < valueList.Count; i++) series.Items.Add(new BarItem(valueList[i]) { Color = OxyColor.FromArgb(colorList[i].A, colorList[i].R, colorList[i].G, colorList[i].B) });
            if (Invalidate) plotModel.InvalidatePlot(true);
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
        public static LineSeries AddLine(PlotModel plotModel, string title, Color? color = null, bool first = true)
        {
            var line = new LineSeries()
            {
                Title = title,
                //MarkerType = MarkerType.Circle,
                //InterpolationAlgorithm = InterpolationAlgorithms.CatmullRomSpline,
                //Smooth = true,
                TrackerFormatString = $"{Environment.NewLine}{0}: {4:0,0.00}"
            };
            if (color != null) line.Color = OxyColor.FromRgb(color.Value.R, color.Value.G, color.Value.B);
            if (first) line.TrackerFormatString = $"时间: {2}{Environment.NewLine}{0}: {4:0,0.00}";
            plotModel.Series.Add(line);
            return line;
        }
        /// <summary>
        /// 添加曲线
        /// </summary>
        public static void AddLine(PlotModel plotModel, int index, string title, OxyColor color, List<double> valueList, Func<object, string, string, object, string, object, string> action = null, string stringFormat = "{2}: {4:#,0.#}")
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
            plotModel.Series.Add(line);
        }
        /// <summary>
        /// 自动曲线最大最小值到坐标系
        /// </summary>
        public static void AutoMaxMin(PlotModel plotModel)
        {
            //根据报单笔数判断是否需要更新y轴刻度     
            //首先找出统计线中当前最大的节点
            var min = double.NaN;
            var max = double.NaN;
            for (int i = 0; i < plotModel.Series.Count; i++)
            {
                var value = (plotModel.Series[i] as LineSeries).MaxY;
                if (max < value) max = value;
                value = (plotModel.Series[i] as LineSeries).MinY;
                if (min > value) min = value;
            }

            var leftAxis = plotModel.Axes.FirstOrDefault(c => c.Position == AxisPosition.Left);
            //如果当前的y轴最大刻度小于数据集中的最大值，放大
            if (!min.Equals(double.NaN)) leftAxis.Minimum = min;
            if (!max.Equals(double.NaN)) leftAxis.Maximum = max;
        }

        #endregion
    }
}