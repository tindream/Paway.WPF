using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
    public class TestWindowModel : ViewModelBasePlus
    {
        #region 属性
        public string Title => Config.Title;

        private double _dValue = 10;
        public double DValue
        {
            get { return _dValue; }
            set { _dValue = value; OnPropertyChanged(); }
        }

        private int _iValue = 10;
        public int IValue
        {
            get { return _iValue; }
            set { _iValue = value; OnPropertyChanged(); }
        }

        private bool _bValue;
        public bool BValue
        {
            get { return _bValue; }
            set { if (_bValue != value) { _bValue = value; OnPropertyChanged(); } }
        }

        private string _sValue;
        public string SValue
        {
            get { return _sValue; }
            set { _sValue = value; OnPropertyChanged(); }
        }

        private MenuAuthType _menuType;
        public MenuAuthType MenuType
        {
            get { return _menuType; }
            set { if (_menuType != value) { _menuType = value; OnPropertyChanged(); } }
        }

        public string Language
        {
            get { return Config.LanguageStr; }
            set { Config.LanguageStr = value; Config.InitLanguage(); OnPropertyChanged(); }
        }
        public ObservableCollection<NameInfo> LanguageObList { get; private set; } = new ObservableCollection<NameInfo>();
        public ObservableCollection<ListViewItemModel> GridList { get; } = new ObservableCollection<ListViewItemModel>();

        #endregion

        #region 命令
        public override bool Action(string item)
        {
            switch (item)
            {
                case "关于":
                    var version = $"V{Assembly.GetEntryAssembly().GetName().Version}";
                    Method.Hit(Config.Window, version);
                    Messenger.Default.Send(new StatuMessage(version, true));
                    break;
                case "主题":
                    Method.ShowWindow(Config.Window, new ThemeWindow());
                    break;
                case "颜色":
                    Method.ShowWindow(Config.Window, new SelectColorWindow());
                    break;
                case "3D":
                    MessageWindow.Hit(Config.Window, item);
                    break;
                case "空":
                    Method.Hit(Config.Window, item);
                    break;
                default:
                    if (Config.LanguageList.Any(c => c == item))
                    {
                        this.Language = item;
                    }
                    break;
            }
            return base.Action(item);
        }
        protected override void Action(ListViewCustom listView1)
        {
            base.Action(listView1);
            if (listView1.SelectedItem is IListViewItem info)
            {
                switch (info.Text)
                {
                    case "A":
                    case "B":
                    case "C":
                    case "D":
                        var menuType = info.Text.Parse<MenuAuthType>();
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
                            MenuType = MenuAuthType.None;
                        }
                        listView1.SelectedIndex = -1;
                        break;
                }
            }
        }
        public ICommand ListViewMouseDown => new RelayCommand<MouseButtonEventArgs>(e =>
        {
            if (e.Source is ListViewEXT listView1)
            {
                var point = Mouse.GetPosition(listView1);
                var obj = listView1.InputHitTest(point);
                if (Method.Parent(obj, out ListViewItem viewItem))
                {
                    Method.WaterAdorner(e, viewItem, 0, 0);
                }
            }
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

        #endregion

        public TestWindowModel()
        {
            LanguageObList.Add(new NameInfo("中文"));
            LanguageObList.Add(new NameInfo("A"));
            LanguageObList.Add(new NameInfo("B"));


            GridList.Add(new ListViewItemModel("Hello"));
            GridList.Add(new ListViewItemModel("你好123")
            {
                IsEnabled = false,
                Image = new ImageEXT(null, @"pack://application:,,,/Paway.Test;component/Images/close_white.png")
            });
            for (int i = 0; i < 10; i++) GridList.Add(new ListViewItemModel()
            {
                Text = i % 3 == 0 ? "A" + i : null,
                Desc = i % 4 == 0 ? "D" + i : "",
                IsEnabled = i != 5,
                Image = new ImageEXT(@"pack://application:,,,/Paway.Test;component/Images/close.png")
            });
        }
    }
}