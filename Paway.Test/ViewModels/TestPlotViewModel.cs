using CommunityToolkit.Mvvm.Input;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Test
{
    public class TestPlotViewModel : ViewModelBasePlus
    {

        private PlotModel plotModel;
        /// <summary>
        /// ’€œﬂÕº
        /// </summary>
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { plotModel = value; OnPropertyChanged(); }
        }
        public TestPlotViewModel()
        {
            AddPlot();
        }
        private void AddPlot()
        {
            var plotList = new List<TempInfo>();
            var time = DateTime.Now;
            plotList.Add(new TempInfo(time.AddSeconds(-10), 1));
            plotList.Add(new TempInfo(time.AddSeconds(-5), 20));
            plotList.Add(new TempInfo(time.AddSeconds(0), 3));
            plotList.Add(new TempInfo(time.AddSeconds(5), 13));
            plotList.Add(new TempInfo(time.AddSeconds(10), 7));

            this.PlotModel = new PlotModel();
            PlotHelper.AddXY(plotModel);

            var line = PlotHelper.AddLine(plotModel, "A", Colors.Red);
            line.Color = OxyColor.FromRgb(2, 232, 250);
            line.DataFieldX = "DateTime";
            line.DataFieldY = "Value";
            line.ItemsSource = plotList;
            var bottomAxis = plotModel.Axes.FirstOrDefault(c => c.Position == AxisPosition.Bottom);
            bottomAxis.StringFormat = "HH:mm:ss";
            if (plotList.Count > 0)
            {
                bottomAxis.Minimum = DateTimeAxis.ToDouble(plotList.First().DateTime);
                bottomAxis.Maximum = DateTimeAxis.ToDouble(plotList.Last().DateTime);
            }

            plotModel.Annotations.Clear();
            plotModel.Annotations.Add(new LineAnnotation
            {
                Type = LineAnnotationType.Horizontal,
                Y = 10,
                Text = "æ˘÷µ",
                Color = OxyColors.Green
            });

            PlotModel.ResetAllAxes();
            PlotModel.InvalidatePlot(true);
            Method.BeginInvoke(() =>
            {
                PlotHelper.AutoMaxMin(PlotModel);
                PlotModel.InvalidatePlot(true);
            });
        }
    }
}