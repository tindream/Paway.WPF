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
        private DependencyObject Root;
        private Frame Frame;

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

        public string Language
        {
            get { return Config.LanguageStr; }
            set { Config.LanguageStr = value; Config.InitLanguage(); OnPropertyChanged(); }
        }
        public ICommand CurrentCellCommand => new RelayCommand<DataGridEXT>(e =>
        {
            if (e.CurrentCell.Column.Header.ToStrings() != "List")
            {
                IValue = -1;
            }
        });
        public ObservableCollection<TestInfo> GridList { get; } = new ObservableCollection<TestInfo>();
        public List<ListViewItemModel> List { get; } = new List<ListViewItemModel>();

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
                case "3D模型":
                    Frame.Content = ViewlLocator.GetInstance<Test3DPage>();
                    break;
                case "登录页":
                    Frame.Content = ViewlLocator.GetInstance<LoginPage>();
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

        #endregion

        public TestWindowModel()
        {
            var index = 0;
            for (int i = 0; i < 5; i++)
            {
                var info = new TestInfo { Name = $"Hello{i + 1}" };
                for (int j = 0; j < i; j++) info.List.Add(new ListViewItemModel($"B{j + 1}") { Id = ++index });
                GridList.Add(info);
            }
            for (int j = 0; j < 5; j++) List.Add(new ListViewItemModel($"A{j + 1}") { Id = ++index });
            this.MessengerInstance.Register<TestLoadMessage>(this, msg =>
            {
                this.Root = msg.Obj;
                if (Method.Find(Root, out Frame frame, "frame"))
                {
                    this.Frame = frame;
                }
            });
        }
    }
    public class TestInfo : BaseModelInfo
    {
        public string Name { get; set; }
        public List<ListViewItemModel> List { get; set; } = new List<ListViewItemModel>();
    }
}