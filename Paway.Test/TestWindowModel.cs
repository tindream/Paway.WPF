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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Test
{
    public class TestWindowModel : ViewModelBase
    {
        #region 属性
        private double angle = -27;
        public double Angle
        {
            get { return angle; }
            set { angle = value; RaisePropertyChanged(); }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                var angleMin = -27;
                var angleMax = 207;
                Angle = angleMin + (angleMax - angleMin) * value / 100.0;
                _value = value; RaisePropertyChanged();
            }
        }

        private double _value2 = 10;
        public double Value2
        {
            get { return _value2; }
            set { _value2 = value; RaisePropertyChanged(); }
        }

        private int _value3 = 10;
        public int Value3
        {
            get { return _value3; }
            set { _value3 = value; RaisePropertyChanged(); }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value; RaisePropertyChanged();
            }
        }

        private MenuType userType = MenuType.None;
        public MenuType UserType
        {
            get { return userType; }
            set
            {
                if (userType != value)
                {
                    userType = value;
                    RaisePropertyChanged();
                }
            }
        }

        private DateTime time = DateTime.Now.Date;
        public DateTime Time
        {
            get { return time; }
            set { time = value; RaisePropertyChanged(); }
        }
        private FontInfo font;
        public FontInfo Font
        {
            get { return font; }
            set { font = value; RaisePropertyChanged(); }
        }
        private ColorInfo color;
        public ColorInfo Color
        {
            get { return color; }
            set
            {
                color = value; RaisePropertyChanged();
                if (value != null) ColorBrush = value.Color.ToBrush(); RaisePropertyChanged(nameof(ColorBrush));
            }
        }
        public DoubleEXT FontSizes { get; set; } = new DoubleEXT(20);
        public SolidColorBrush ColorBrush { get; set; }
        private MenuType _menuType;
        public MenuType MenuType
        {
            get { return _menuType; }
            set { if (_menuType != value) { _menuType = value; RaisePropertyChanged(); } }
        }

        #endregion

        #region 命令
        public ICommand ButtonCommand => new RelayCommand<ListViewCustom>(view =>
        {
            //var index = Method.Random(0, ViewList.Count);
            //ViewList[index].ItemTextForeground = new BrushEXT(Colors.Red);
            //if (view.ItemsSource == null) view.ItemsSource = ViewList;
            UserType = MenuType.N2;
        });
        public ICommand SelectionDeviceCommand => new RelayCommand<ListViewCustom>(listView =>
        {
            if (!(listView.SelectedItem is IListViewItem item)) return;
            Method.Show(listView, new Window());
            listView.SelectedIndex = -1;
        });
        public ICommand ListViewMouseDown => new RelayCommand<MouseButtonEventArgs>(e =>
        {
            if (e.Source is ListViewEXT listView1)
            {
                var point = Mouse.GetPosition(listView1);
                var obj = listView1.InputHitTest(point);
                if (PMethod.Parent(obj, out ListViewItem viewItem))
                {
                    Method.WaterAdorner(e, viewItem, 0, 0);
                }
            }
        });
        public ICommand SelectionCommand => new RelayCommand<ListViewEXT>(listView1 =>
        {
            if (listView1.SelectedItem is IListViewItem info)
            {
                switch (info.Text)
                {
                    case "A":
                    case "B":
                    case "C":
                    case "D":
                        var menuType = info.Text.Parse<MenuType>();
                        if (MenuType != menuType)
                        {
                            if (listView1.Items.Find("Text", MenuType.Description()) is IListViewItem last)
                            {
                                last.Desc = "\uf0d7";
                            }
                            MenuType = menuType;
                            info.Desc = "\uf0d8";
                        }
                        else
                        {
                            info.Desc = "\uf0d7";
                            MenuType = MenuType.None;
                        }
                        listView1.SelectedIndex = -1;
                        break;
                }
            }
        });

        #endregion

        public ObservableCollection<ListViewItemModel> ViewList { get; private set; } = new ObservableCollection<ListViewItemModel>();
        public ObservableCollection<IComboBoxItem> MultiList { get; } = new ObservableCollection<IComboBoxItem>();
        public ObservableCollection<ITreeViewItem> TreeList { get; private set; } = new ObservableCollection<ITreeViewItem>();
        public ObservableCollection<ListViewItemModel> GridList { get; } = new ObservableCollection<ListViewItemModel>();
        public List<ColorInfo> ColorList { get; } = new List<ColorInfo>();
        public List<FontInfo> FontList { get; } = new List<FontInfo>();

        public TestWindowModel()
        {
            this.Text = "111\r\n222\r\n333";
            var index = 0;
            foreach (var font in Fonts.SystemFontFamilies)
            {
                var info = new FontInfo { Id = index++, Name = font.Source, FontFamily = font };
                FontList.Add(info);
                if (info.Name == Config.Window.FontFamily.Source) this.Font = info;
            }
            var piList = typeof(Colors).Properties(Config.Flags);
            for (var i = 0; i < piList.Count; i++)
            {
                var color = (Color)ColorConverter.ConvertFromString(piList[i].Name);
                var info = new ColorInfo { Id = i, Name = piList[i].Name, Color = color };
                ColorList.Add(info);
                if (piList[i].Name == nameof(Colors.White)) this.Color = info;
            }

            for (var i = 0; i < 9; i++) ViewList.Add(new ListViewItemModel($"{Method.Random(1, 1000000) * Method.Random(1, 1000000)}"));
            for (var i = 0; i < 9; i++) MultiList.Add(new ComboBoxItemModel($"{i + 1}"));
            var treeInfo = new TreeViewItemModel("分类A", true);
            this.TreeList.Add(treeInfo);
            treeInfo.Add("刘棒A1");
            treeInfo.Add("刘棒A2");
            treeInfo.Add("刘棒A3");
            treeInfo.Add("刘棒A4");
            treeInfo.Add("刘棒A5");
            treeInfo = new TreeViewItemModel("分类B", true);
            this.TreeList.Add(treeInfo);
            var treeInfo2 = new TreeViewItemModel("分类C", true);
            treeInfo.Add(treeInfo2);
            treeInfo2.Add("刘棒C1");
            treeInfo2.Add("刘棒C2");
            treeInfo.Add("刘棒B1");
            treeInfo.Add("刘棒B2");

            GridList.Add(new ListViewItemModel("Hello"));
            GridList.Add(new ListViewItemModel("你好123")
            {
                IsEnabled = false,
                Image = new ImageEXT(null, @"pack://application:,,,/Paway.Test;component/Images/close_white.png")
            });
            for (int i = 0; i < 10; i++) GridList.Add(new ListViewItemModel("A" + i, "D" + i)
            {
                IsEnabled = i != 5,
                Image = new ImageEXT(@"pack://application:,,,/Paway.Test;component/Images/close.png")
            });
        }
    }
    public class FontInfo : ModelBase
    {
        [FillSize]
        public string Name { get; set; }
        [NoShow]
        public FontFamily FontFamily { get; set; }
    }
    public class ColorInfo : ModelBase
    {
        [FillSize]
        public string Name { get; set; }
        [NoShow]
        public Color Color { get; set; }
        [NoShow]
        public SolidColorBrush ColorBrush { get { return Color.ToBrush(); } }
    }
}