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
        private TipWindow tipWindow;

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
                case "测试页":
                    Frame.Content = ViewModelLocator.GetViewInstance<TestPage>();
                    break;
                case "报告问题":
                    if (tipWindow == null)
                    {
                        tipWindow = new TipWindow();
                        tipWindow.Show();
                    }
                    else
                    {
                        tipWindow.Close();
                        tipWindow = null;
                    }
                    break;
                case "3D模型":
                    Frame.Content = ViewModelLocator.GetViewInstance<Test3DPage>();
                    break;
                case "登录页":
                    Frame.Content = ViewModelLocator.GetViewInstance<LoginPage>();
                    break;
                case "DataGrid":
                    Frame.Content = ViewModelLocator.GetViewInstance<TestDataGrid>();
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
        public void Close()
        {
            tipWindow?.Close();
        }

        #endregion

        public TestWindowModel()
        {
            this.MessengerInstance.Register<TestLoadMessage>(this, msg =>
            {
                this.Root = msg.Obj;
                if (Method.Find(Root, out Frame frame, "frame"))
                {
                    this.Frame = frame;
                    this.Frame.Content = ViewModelLocator.GetViewInstance<TestPage>();
                }
            });
        }
    }
}