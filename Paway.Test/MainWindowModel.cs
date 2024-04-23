using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using Paway.Model;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Test
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainWindowModel : ViewModelBasePlus
    {
        #region 属性
        public string Text => Config.Text;
        private readonly List<ListViewItemModel> list = new List<ListViewItemModel>();
        public List<ListViewItemModel> GridList { get { return list; } }
        public PagedCollectionView PagedList { get; private set; }
        public ObservableCollection<TreeViewItemModel> TreeList { get; private set; } = new ObservableCollection<TreeViewItemModel>();

        private ListViewItemModel selectedItem;
        public ListViewItemModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    if (value != null) Id = value.Id;
                    OnPropertyChanged();
                }
            }
        }
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    SelectedItem = GridList.Find(c => c.Id == value);
                    //Pad = value.ToString();
                    OnPropertyChanged();
                }
            }
        }
        private int treeId;
        public int TreeId
        {
            get { return treeId; }
            set
            {
                if (treeId != value)
                {
                    treeId = value;
                    OnPropertyChanged();
                }
            }
        }

        private PlotModel plotModel;
        /// <summary>
        /// 折线图
        /// </summary>
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { plotModel = value; OnPropertyChanged(); }
        }

        private bool _isSelected = true;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(); }
        }

        private MenuAuthType _menuAuthType;
        public MenuAuthType MenuAuthType
        {
            get { return _menuAuthType; }
            set { _menuAuthType = value; OnPropertyChanged(); }
        }

        public ObservableCollection<IComboBoxItem> MultiList { get; } = new ObservableCollection<IComboBoxItem>();

        private DateTime datePickerTime = DateTime.Now;
        public DateTime DatePickerTime
        {
            get { return datePickerTime; }
            set { datePickerTime = value; OnPropertyChanged(); }
        }

        private int _value = 11;
        public int Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); }
        }
        private string pad = "1";
        public string Pad
        {
            get { return pad; }
            set { pad = value; OnPropertyChanged(); }
        }

        private string desc = "准备就绪";
        public string Desc
        {
            get { return desc; }
            set { desc = value; OnPropertyChanged(); }
        }

        private PlotLineType monitorType = PlotLineType.RightToeAngle;
        public PlotLineType MonitorType
        {
            get { return monitorType; }
            set { monitorType = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 字体列表
        /// </summary>
        public List<FontInfo> FontList { get; } = new List<FontInfo>();
        private int _fontFamilyCount;
        public int FontFamilyCount
        {
            get { return _fontFamilyCount; }
            set { _fontFamilyCount = value; OnPropertyChanged(); }
        }
        private int _fontFamilyIndex;
        public int FontFamilyIndex
        {
            get { return _fontFamilyIndex; }
            set
            {
                if (FontList.Count <= value) return;
                if (_fontFamilyIndex != value)
                {
                    _fontFamilyIndex = value;
                    PConfig.FontFamily = FontList[value].Name;
                    OnPropertyChanged();
                }
            }
        }
        private string _colors = "成功";
        public string Colors
        {
            get { return _colors; }
            set { _colors = value; OnPropertyChanged(); }
        }

        #endregion

        #region 命令
        public ICommand RowDoubleCommand => new RelayCommand<SelectItemEventArgs>(e =>
        {
            if (e.Source is DataGridEXT datagrid1)
            {
                if (e.Item is ListViewItemModel info)
                {
                }
            }
        });

        public ICommand SelectedItemChanged => new RelayCommand<TreeViewEXT>(treeView =>
        {
            if (treeView.SelectedItem is ITreeViewItem item)
            {
                this.TreeId = item.Id;
            }
        });
        public ICommand SizeChanged => new RelayCommand<SliderEXT>(slider =>
        {
            Config.FontSize = slider.Value;
        });
        public ICommand ColorChanged => new RelayCommand<SliderEXT>(slider =>
        {
            var color = Method.ColorSelector(slider.Value / 7);
            Config.Color = color;
            this.Colors = color.ToString();
            //Method.DoStyles();
        });
        public ICommand ForegroundChanged => new RelayCommand<SliderEXT>(slider =>
        {
            var color = Method.ColorSelector(slider.Value / 7);
            Config.Foreground = color;
        });
        public ICommand BackgroundChanged => new RelayCommand<SliderEXT>(slider =>
        {
            var color = Method.ColorSelector(slider.Value / 7);
            Config.Background = color;
        });
        public ICommand FontFamilyChanged => new RelayCommand<ValuesChangeEventArgs>(e =>
        {
            e.Values = $"{this.FontFamilyIndex}：{this.FontList[this.FontFamilyIndex].Name}";
        });

        public ICommand CbxFilterCmd => new RelayCommand<CustomFilterEventArgs>(e =>
        {
            if (e.Source is DataGridEXT dataGrid)
            {
                var p = dataGrid.Columns.Predicate<ListViewItemModel>(e.Filter);
                e.List = this.list.AsParallel().Where(p).ToList();
            }
        });

        protected override void Action(ListViewCustom listView1)
        {
            base.Action(listView1);
            if (listView1.SelectedItem is IListViewItem info)
            {
                Method.Toast(listView1, info.Text);
            }
        }
        public ICommand RectDoubleCommand => new RelayCommand<MouseButtonEventArgs>(e =>
        {
            if (e.Source is ListViewCustom listView)
            {
                var point = e.GetPosition(listView);
                var obj = listView.InputHitTest(point);
                if (Method.Parent(obj, out ListViewItem temp) && temp.Content is IListViewItem item)
                {
                    Method.Hit(listView, item.Text);
                }
            }
        });

        public ICommand Teach => new RelayCommand<Button>(btn =>
        {
            MultiList[0].IsChecked = !MultiList[0].IsChecked;
            var desc = string.Join(",", MultiList.ToList().FindAll(c => c.IsChecked).Select(c => c.Text));
            Method.Toast(btn, desc, 5000);
        });

        #endregion

        #region 消息
        private void Statu(string msg)
        {
            this.Desc = msg;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainWindowModel()
        {
            this.FontFamilyCount = 0;
            foreach (var font in Fonts.SystemFontFamilies)
            {
                if (font.Source == PConfig.FontFamily) this._fontFamilyIndex = this.FontFamilyCount;
                var info = new FontInfo { Id = this.FontFamilyCount++, Name = font.Source, FontFamily = font };
                FontList.Add(info);
            }

            this.MessengerInstance.Register<StatuMessage>(this, msg => Statu(msg.Msg));

            list.Add(new ListViewItemModel("Hello"));
            list.Add(new ListViewItemModel("你好123")
            {
                IsEnabled = false,
                Image = new ImageEXT(null, @"pack://application:,,,/Paway.Test;component/Images/close_white.png")
            });
            for (int i = 0; i < 20; i++) list.Add(new ListViewItemModel("A" + i, "D" + i)
            {
                IsEnabled = i != 5,
                Image = new ImageEXT(@"pack://application:,,,/Paway.Test;component/Images/close.png")
            });
            this.PagedList = new PagedCollectionView(list) { PageSize = 10 };

            AddComboMulti();
            AddTree();
            AddPlot();
        }
        private void AddComboMulti()
        {
            MultiList.Add(new ComboBoxItemModel("张三") { IsChecked = true });
            MultiList.Add(new ComboBoxItemModel("李四"));
            MultiList.Add(new ComboBoxItemModel("王五"));
            MultiList.Add(new ComboBoxItemModel("马六"));
            MultiList.Add(new ComboBoxItemModel("赵七") { IsChecked = true });
            MultiList.Add(new ComboBoxItemModel("王八"));
            MultiList.Add(new ComboBoxItemModel("陈九"));
        }
        private void AddTree()
        {
            var treeInfo = new TreeViewItemModel("单位名称(3/7)", true);
            this.TreeList.Add(treeInfo);
            var treeInfo2 = new TreeViewItemModel("未分组联系人(2/4)", true);
            treeInfo.Add(treeInfo2);
            treeInfo2.Add("刘棒", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒1", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒2", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒3", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒4", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒5", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒6", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒7", "我要走向天空！", "3人");

            treeInfo2 = new TreeViewItemModel("未分组联系人", true);
            treeInfo.Add(treeInfo2);
            treeInfo2.Add("刘棒A", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒B", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒C", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒5", "我要走向天空！", "3人");

            this.TreeId = treeInfo2.Children[0].Id;
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

            var line = PlotHelper.AddLine(plotModel, PlotLineType.LeftToeAngle);
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
                Text = "均值",
                Color = OxyColors.Green
            });

            PlotModel.ResetAllAxes();
            PlotModel.InvalidatePlot(true);
            Method.BeginInvoke(() =>
            {
                PlotHelper.AutoMaxMin(PlotModel, 10, 10);
                PlotModel.InvalidatePlot(true);
            });
        }
    }
}