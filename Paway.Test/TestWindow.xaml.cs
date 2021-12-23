using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Paway.Test
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : WindowEXT
    {
        public TestWindow()
        {
            InitializeComponent();
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }

        private List<ThumbInfo> list = new List<ThumbInfo>();
        private ThumbInfo last;
        private ThumbInfo next;
        private Thumb nextThumb;
        private void ButtonEXT_Click(object sender, RoutedEventArgs e)
        {
            //Method.Show(this, new Window() { Height = 200, Width = 200, Foreground = new SolidColorBrush(Colors.White), Title = "Test123" });
            list.Clear();
            canvas.Children.Clear();

            var width = (int)canvas.ActualWidth / 50; if (canvas.ActualWidth % 50 > 0) width++;
            var height = (int)canvas.ActualHeight / 50; if (canvas.ActualHeight % 50 > 0) height++;
            for (var i = 1; i < height; i++)
            {
                var line = new Line
                {
                    X2 = canvas.ActualWidth,
                    Stroke = new SolidColorBrush(Colors.LightGray),
                    StrokeDashArray = new DoubleCollection(new double[] { 2, 2 }),
                };
                RenderOptions.SetEdgeMode(line, EdgeMode.Aliased);
                line.Y1 = line.Y2 = i * 50;
                canvas.Children.Add(line);
            }
            for (var i = 1; i < width; i++)
            {
                var line = new Line
                {
                    Y2 = canvas.ActualHeight,
                    Stroke = new SolidColorBrush(Colors.LightGray),
                    StrokeDashArray = new DoubleCollection(new double[] { 2, 2 }),
                };
                RenderOptions.SetEdgeMode(line, EdgeMode.Aliased);
                line.X1 = line.X2 = i * 50;
                canvas.Children.Add(line);
            }

            width = (int)canvas.ActualWidth / 50;
            height = (int)canvas.ActualHeight / 50;
            var count = Method.Random(1, width * height);
            for (var i = 0; i < count; i++) list.Add(new ThumbInfo { Index = i + 1 });
            var template = FindResource("ThumbButton") as ControlTemplate;
            for (var i = 0; i < height && i - 1 < list.Count / width; i++)
            {
                for (var j = 0; j < width && i * width + j < list.Count; j++)
                {
                    var info = list[i * width + j];
                    info.X = j * 50;
                    info.Y = i * 50;
                    var thumb = new Thumb()
                    {
                        Width = 50,
                        Height = 50,
                        Template = template,
                        Tag = info,
                    };

                    Canvas.SetLeft(thumb, info.X);
                    Canvas.SetTop(thumb, info.Y);
                    thumb.DragDelta += DragDelta;
                    thumb.DragStarted += DragStarted;
                    thumb.DragCompleted += DragCompleted;
                    canvas.Children.Add(thumb);
                }
            }
        }
        private void DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb thumb && thumb.Tag is ThumbInfo info)
            {
                if (this.last != null) this.last.IStop = false;
                var toX = Canvas.GetLeft(thumb) + e.HorizontalChange; if (toX < 0) toX = 0; else if (toX > canvas.ActualWidth - thumb.ActualWidth) toX = canvas.ActualWidth - thumb.ActualWidth;
                var toY = Canvas.GetTop(thumb) + e.VerticalChange; if (toY < 0) toY = 0; else if (toY > canvas.ActualHeight - thumb.ActualHeight) toY = canvas.ActualHeight - thumb.ActualHeight;
                Canvas.SetLeft(thumb, toX);
                Canvas.SetTop(thumb, toY);

                var toXI = (toX / 50).ToInt();
                toX = toXI * 50;
                var toYI = (toY / 50).ToInt();
                toY = toYI * 50;
                this.last = list.Find(c => c.Index != info.Index && c.X == toX && c.Y == toY);
                if (this.last != null)
                {
                    this.last.IStop = true;
                }
                else
                {
                    next.X = toX; next.Y = toY;
                    Canvas.SetLeft(nextThumb, toX);
                    Canvas.SetTop(nextThumb, toY);
                }
            }
        }
        private void DragStarted(object sender, DragStartedEventArgs e)
        {
            if (sender is Thumb thumb && thumb.Tag is ThumbInfo info)
            {
                info.Selected = true;

                var template = FindResource("ThumbButton") as ControlTemplate;
                next = new ThumbInfo() { Index = info.Index, ITemp = true };
                next.X = info.X;
                next.Y = info.Y;
                nextThumb = new Thumb()
                {
                    Width = 50,
                    Height = 50,
                    Template = template,
                    Tag = next,
                };

                Canvas.SetLeft(nextThumb, info.X);
                Canvas.SetTop(nextThumb, info.Y);
                canvas.Children.Add(nextThumb);
            }
        }
        private void DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (sender is Thumb thumb && thumb.Tag is ThumbInfo info)
            {
                info.Selected = false;
                if (nextThumb != null)
                {
                    canvas.Children.Remove(nextThumb);
                    info.X = next.X;
                    info.Y = next.Y;
                    Canvas.SetLeft(thumb, info.X);
                    Canvas.SetTop(thumb, info.Y);
                }
            }
            if (this.last != null) this.last.IStop = false;
        }
    }
    public class ThumbInfo : ModelBase
    {
        private int index;
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                OnPropertyChanged();
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }
        public double X { get; set; }
        public double Y { get; set; }
        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                Load();
            }
        }
        private bool iStop;
        public bool IStop
        {
            get { return iStop; }
            set
            {
                iStop = value;
                Load();
            }
        }
        private bool iTemp;
        public bool ITemp
        {
            get { return iTemp; }
            set
            {
                iTemp = value;
                Load();
            }
        }
        private void Load()
        {
            Color = IStop ? new SolidColorBrush(Config.Error) : ITemp ? new SolidColorBrush(Config.Warn) : Selected ? new SolidColorBrush(Config.Success) : new SolidColorBrush(Colors.Gray);
            Border = Selected ? new Thickness(1) : new Thickness(0);
        }

        private Brush color = new SolidColorBrush(Colors.Gray);
        public Brush Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged();
            }
        }
        private Thickness border = new Thickness(0);
        public Thickness Border
        {
            get { return border; }
            set
            {
                border = value;
                OnPropertyChanged();
            }
        }
    }
}
