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
        private readonly double minValue = Math.Pow(20.1, 1.0 / Config.Zoom);
        private readonly double maxValue = Math.Pow(19999.9, 1.0 / Config.Zoom);
        private int index;
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

        private double sliderValue = 3;
        public double SliderValue
        {
            get { return sliderValue; }
            set { sliderValue = value; RaisePropertyChanged(); }
        }
        private string sliderValues = "3.0";
        public string SliderValues
        {
            get { return sliderValues; }
            set
            {
                sliderValues = value;
                if (PMethod.Round(SliderValue, 1) != value.ToDouble())
                    SliderValue = value.ToDouble() > 0 ? value.ToDouble() : value.ToDouble() * 3 / 8;
                RaisePropertyChanged();
            }
        }

        private double boardValue = -65.0;
        public double BoardValue
        {
            get { return boardValue; }
            set { boardValue = value; RaisePropertyChanged(); }
        }
        private string boardValues = "-65.0";
        public string BoardValues
        {
            get { return boardValues; }
            set
            {
                boardValues = value;
                if (PMethod.Round(BoardValue, 1) != value.ToDouble())
                    BoardValue = value.ToDouble();
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public PlotViewModel()
        {
            AddPlot();
        }

        #region Slider
        private ICommand sliderToolTipValueChanged;
        public ICommand SliderToolTipValueChanged
        {
            get
            {
                return sliderToolTipValueChanged ?? (sliderToolTipValueChanged = new RelayCommand<ValueChangeEventArgs>(e =>
                {
                    var value = e.Value;
                    if (e.Value < 0) e.Value = e.Value * 8 / 3;
                }));
            }
        }
        private ICommand sliderValueChanged;
        public ICommand SliderValueChanged
        {
            get
            {
                return sliderValueChanged ?? (sliderValueChanged = new RelayCommand<SliderEXT>(e =>
                {
                    var value = e.Value;
                    if (value < 0) value = e.Value * 8 / 3;
                    this.SliderValues = PMethod.Rounds(value, 1, 1);
                }));
            }
        }
        private ICommand boardValueChanged;
        public ICommand BoardValueChanged
        {
            get
            {
                return boardValueChanged ?? (boardValueChanged = new RelayCommand<ProgressBoard>(e =>
                {
                    this.BoardValues = PMethod.Rounds(e.Value, 1, 1);
                }));
            }
        }

        #endregion

        #region Plot
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

            this.PlotModel = new PlotModel() { PlotAreaBorderColor = OxyColor.FromArgb(50, 0, 0, 0) };
            AddAxis(plotModel);

            var line = AddSeries(plotModel);
            line.DataFieldX = nameof(RateInfo.X);
            line.DataFieldY = nameof(RateInfo.Value);
            line.ItemsSource = rateList;

            plotModel.Annotations.Clear();
            plotModel.Annotations.Add(AddLine(LineAnnotationType.Horizontal, 0, -18));
            plotModel.Annotations.Add(AddLine(LineAnnotationType.Horizontal, 0, -12));
            plotModel.Annotations.Add(AddLine(LineAnnotationType.Horizontal, 0, -6));
            plotModel.Annotations.Add(AddLine(LineAnnotationType.Horizontal, 0, 0));
            plotModel.Annotations.Add(AddLine(LineAnnotationType.Horizontal, 0, 6));
            plotModel.Annotations.Add(AddLine(LineAnnotationType.Horizontal, 0, 12));
            plotModel.Annotations.Add(AddLine(LineAnnotationType.Horizontal, 0, 18));
            foreach (var point in rateList)
            {
                plotModel.Annotations.Add(AddLine(LineAnnotationType.Vertical, point.X));
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

        public void AddAxis(PlotModel plotModel, bool yZoom = false, bool xZoom = false)
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
                Minimum = Math.Pow(10, 1.0 / Config.Zoom),
                Maximum = Math.Pow(40000, 1.0 / Config.Zoom),
                IsNodeMoveEnabled = true,
            };
            bottomAxis.NodeMoveEvent += Axis_NodeMoveEvent;
            plotModel.Axes.Add(bottomAxis);
        }
        public LineSeries AddSeries(PlotModel plotModel)
        {
            var line = new LineSeries()
            {
                Title = "增益",
                MarkerType = MarkerType.Circle,
                TrackerPoint = 9,
                Color = OxyColor.FromRgb(Config.Color.R, Config.Color.G, Config.Color.B),

                MarkerSize = 8,
                MarkerFill = OxyColors.White,
                MarkerStroke = OxyColor.FromRgb(Config.Color.R, Config.Color.G, Config.Color.B),
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
        public LineAnnotation AddLine(LineAnnotationType type, double x = 0, double y = 0)
        {
            return new LineAnnotation
            {
                Type = type,
                X = x,
                Y = y,
                Color = OxyColor.FromArgb(50, 0, 0, 0)
            };
        }

        #endregion
    }
}