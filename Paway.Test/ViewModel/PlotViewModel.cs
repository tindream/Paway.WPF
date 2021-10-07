using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Test.ViewModel
{
    public class PlotViewModel : ViewModelPlus
    {
        #region 属性
        private PlotModel plotModel;
        /// <summary>
        /// 折线图
        /// </summary>
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { plotModel = value; RaisePropertyChanged(); }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public PlotViewModel()
        {
            AddPlot();
        }
        private void AddPlot()
        {
            var plotList = new List<RateInfo>();
            plotList.Add(new RateInfo(20, 0));
            plotList.Add(new RateInfo(40, 0));
            plotList.Add(new RateInfo(60, 0));
            plotList.Add(new RateInfo(80, 0));
            plotList.Add(new RateInfo(100, 0));
            plotList.Add(new RateInfo(200, 12));
            plotList.Add(new RateInfo(300, 0));
            plotList.Add(new RateInfo(400, 0));
            plotList.Add(new RateInfo(500, 0));
            plotList.Add(new RateInfo(600, 0));
            plotList.Add(new RateInfo(700, 0));
            plotList.Add(new RateInfo(800, 0));
            plotList.Add(new RateInfo(900, 0));
            plotList.Add(new RateInfo(1000, 0));
            plotList.Add(new RateInfo(2000, 0));
            plotList.Add(new RateInfo(3000, -12));
            plotList.Add(new RateInfo(4000, 0));
            plotList.Add(new RateInfo(20000, 0));

            this.PlotModel = new PlotModel()
            {
                PlotAreaBorderColor = OxyColor.FromArgb(255, 35, 175, 255),
            };
            AddXY(plotModel);

            var line = AddLine(plotModel, plotList.Count);
            line.Color = OxyColor.FromRgb(2, 232, 250);
            line.DataFieldX = nameof(RateInfo.X);
            line.DataFieldY = nameof(RateInfo.Value);
            line.ItemsSource = plotList;
            var bottomAxis = plotModel.Axes.FirstOrDefault(c => c.Position == AxisPosition.Bottom);
            if (plotList.Count > 0)
            {
                bottomAxis.Minimum = Math.Pow(10, 1.0 / Config.Zoom);
                bottomAxis.Maximum = Math.Pow(40000, 1.0 / Config.Zoom);
            }

            plotModel.Annotations.Clear();
            plotModel.Annotations.Add(AddLineY(-18));
            plotModel.Annotations.Add(AddLineY(-12));
            plotModel.Annotations.Add(AddLineY(-6));
            plotModel.Annotations.Add(AddLineY(0));
            plotModel.Annotations.Add(AddLineY(6));
            plotModel.Annotations.Add(AddLineY(12));
            plotModel.Annotations.Add(AddLineY(18));
            foreach (var point in plotList)
            {
                plotModel.Annotations.Add(AddLineX(point.X));
            }

            PlotModel.ResetAllAxes();
            PlotModel.InvalidatePlot(true);
        }
        private LineAnnotation AddLineY(double y)
        {
            return new LineAnnotation
            {
                Type = LineAnnotationType.Horizontal,
                Y = y,
                Color = OxyColor.FromArgb(50, 0, 0, 0)
            };
        }
        private LineAnnotation AddLineX(double x)
        {
            return new LineAnnotation
            {
                Type = LineAnnotationType.Vertical,
                X = x,
                Color = OxyColor.FromArgb(50, 0, 0, 0)
            };
        }

        #region LinearAxis
        private static int index;
        public static void AddXY(PlotModel plotModel, bool yZoom = false, bool xZoom = false)
        {
            //定义y轴
            var leftAxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -18,
                Maximum = 18,
                IntervalLength = 10,
                LabelFormatter = y => { return y % 6 == 0 ? $"{y}dB" : null; },
                IsZoomEnabled = yZoom,//false:坐标轴缩放关闭
                IsPanEnabled = yZoom,//false:图表缩放功能关闭
                MajorTickSize = 0,
                MinorTickSize = 0,
            };
            plotModel.Axes.Add(leftAxis);
            //定义x轴
            var bottomAxis = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                IsZoomEnabled = xZoom,//false:坐标轴缩放关闭
                IsPanEnabled = xZoom,//false:图表缩放功能关闭
                MajorTickSize = 0,
                MinorTickSize = 0,
                IntervalLength = 10,
                LabelFormatter = x =>
                {
                    var value = Math.Pow(x, Config.Zoom).ToInt();
                    if (value == 10) return "10Hz";
                    if (value > 95 && value < 105 && index != 2) { index = 2; return "100Hz"; }
                    if (value > 950 && value < 1050 && index != 3) { index = 3; return "1KHz"; }
                    if (value > 9600 && value < 10400 && index != 4) { index = 4; return "10KHz"; }
                    return null;
                },
            };
            plotModel.Axes.Add(bottomAxis);
        }
        public static LineSeries AddLine(PlotModel plotModel, int count)
        {
            var line = new LineSeries()
            {
                Title = "增益",
                MarkerType = MarkerType.Circle,

                Color = OxyColor.Parse("#4CAF50"),
                MarkerSize = 8,
                MarkerFill = OxyColor.Parse("#FFFFFFFF"),
                MarkerStroke = OxyColor.Parse("#23AFFF"),
                MarkerStrokeThickness = 1.5,
                StrokeThickness = 2,
                LabelMargin = -8,
                LabelFormatString = "{2}",
                LabelFormatStringAction = (item, point, index) =>
                {
                    if (index == 0) return "H";
                    if (index == count - 1) return "L";
                    return $"{index}";
                },

                InterpolationAlgorithm = InterpolationAlgorithms.CatmullRomSpline,
                TrackerFormatString = "频率: {2}\r\n{0}: {4:#,0.0}",
                TrackerFormatStringAction = (title, xTitle, x, yTitle, y) =>
                {
                    if (x is double xValue && y is double yValue)
                    {
                        return $"频率: {Math.Pow(xValue, Config.Zoom).ToInt()}\r\n{title}: {yValue:#,0.0}";
                    }
                    return null;
                }
            };
            plotModel.Series.Add(line);

            return line;
        }

        #endregion
    }
}