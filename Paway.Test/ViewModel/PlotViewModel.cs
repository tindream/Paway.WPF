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
        private double minValue = Math.Pow(20.1, 1.0 / Config.Zoom);
        private double maxValue = Math.Pow(19999.9, 1.0 / Config.Zoom);
        private List<RateInfo> rateList;
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
            this.rateList = new List<RateInfo>();
            rateList.Add(new RateInfo(null, 0, 0));
            rateList.Add(new RateInfo("H", 20, 0));
            rateList.Add(new RateInfo("1", 40, 0));
            rateList.Add(new RateInfo("2", 60, 0));
            rateList.Add(new RateInfo("3", 80, 0));
            rateList.Add(new RateInfo("4", 100, 0));
            rateList.Add(new RateInfo("5", 200, 12));
            rateList.Add(new RateInfo("6", 300, 0));
            rateList.Add(new RateInfo("7", 400, 0));
            rateList.Add(new RateInfo("8", 500, 0));
            rateList.Add(new RateInfo("9", 600, 0));
            rateList.Add(new RateInfo("10", 700, 0));
            rateList.Add(new RateInfo("11", 800, 0));
            rateList.Add(new RateInfo("12", 900, 0));
            rateList.Add(new RateInfo("13", 1000, 0));
            rateList.Add(new RateInfo("14", 2000, 0));
            rateList.Add(new RateInfo("15", 3000, -12));
            rateList.Add(new RateInfo("16", 4000, 0));
            rateList.Add(new RateInfo("L", 20000, 0));
            rateList.Add(new RateInfo(null, 50000, 0));

            this.PlotModel = new PlotModel()
            {
                PlotAreaBorderColor = OxyColor.FromArgb(50, 0, 0, 0),
            };
            AddXY(plotModel);

            var line = AddLine(plotModel, rateList.Count);
            line.Color = OxyColor.FromRgb(2, 232, 250);
            line.DataFieldX = nameof(RateInfo.X);
            line.DataFieldY = nameof(RateInfo.Value);
            line.ItemsSource = rateList;
            var bottomAxis = plotModel.Axes.FirstOrDefault(c => c.Position == AxisPosition.Bottom);
            if (rateList.Count > 0)
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
            foreach (var point in rateList)
            {
                plotModel.Annotations.Add(AddLineX(point.X));
            }

            PlotModel.ResetAllAxes();
            PlotModel.InvalidatePlot(true);
        }
        private double? Axis_NodeMoveEvent(Axis axis, TrackerHitResult result, bool horizontal, double value)
        {
            double? resut = null;
            if (result.Item is RateInfo item)
            {
                double dx = value / axis.Scale;
                if (horizontal)
                {
                    if (item.X + dx < this.minValue)
                    {
                        resut = (this.minValue - item.X) * axis.Scale;
                        item.X = this.minValue;
                    }
                    else if (item.X + dx > this.maxValue)
                    {
                        resut = (this.maxValue - item.X) * axis.Scale;
                        item.X = this.maxValue;
                    }
                    else item.X += dx;
                }
                else
                {
                    if (item.Value + dx > 18)
                    {
                        resut = (18 - item.Value) * axis.Scale;
                        item.Value = 18;
                    }
                    else if (item.Value + dx < -18)
                    {
                        resut = (-18 - item.Value) * axis.Scale;
                        item.Value = -18;
                    }
                    else item.Value += dx;
                }
                if (resut == 0) return resut;
                this.rateList.Sort((x, y) => x.X > y.X ? 1 : -1);
                PlotModel.InvalidatePlot(true);
            }
            return resut;
        }

        #region LinearAxis
        private int index;
        public void AddXY(PlotModel plotModel, bool yZoom = false, bool xZoom = false)
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
                IsNodeMoveEnabled = true,
            };
            leftAxis.NodeMoveEvent += Axis_NodeMoveEvent;
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
                IsNodeMoveEnabled = true,
            };
            bottomAxis.NodeMoveEvent += Axis_NodeMoveEvent;
            plotModel.Axes.Add(bottomAxis);
        }
        public LineSeries AddLine(PlotModel plotModel, int count)
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
                    if (item is RateInfo temp) return temp.Text;
                    return null;
                },

                InterpolationAlgorithm = InterpolationAlgorithms.CatmullRomSpline,
                TrackerFormatString = "频率: {2}\n{0}: {4:#,0.0}",
                TrackerFormatStringAction = (item, title, xTitle, x, yTitle, y) =>
                {
                    if (item is RateInfo info)
                    {
                        return $"频率: {Math.Pow(info.X, Config.Zoom).ToInt()}\n增益: {info.Value:#,0.0}";
                    }
                    return null;
                }
            };
            plotModel.Series.Add(line);

            return line;
        }
        public LineAnnotation AddLineY(double y)
        {
            return new LineAnnotation
            {
                Type = LineAnnotationType.Horizontal,
                Y = y,
                Color = OxyColor.FromArgb(50, 0, 0, 0)
            };
        }
        public LineAnnotation AddLineX(double x)
        {
            return new LineAnnotation
            {
                Type = LineAnnotationType.Vertical,
                X = x,
                Color = OxyColor.FromArgb(50, 0, 0, 0)
            };
        }

        #endregion
    }
}