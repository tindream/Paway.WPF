using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Paway.Test
{
    public class TipWindowModel : ViewModelBase
    {
        #region 属性
        private TipWindow tipWindow;
        private double tipWidth = 80;
        public double TipWidth
        {
            get { return tipWidth; }
            set { tipWidth = value; RaisePropertyChanged(); }
        }
        private bool _iAll;
        public bool IAll
        {
            get { return _iAll; }
            set
            {
                _iAll = value;
                if (value) TipWidth = 248;
                else TipWidth = 80;
                RaisePropertyChanged();
                TipAnimation();
            }
        }
        private void TipAnimation()
        {
            var element = IAll ? this.tipWindow.borderAll : this.tipWindow.borderOne;
            var time = 125;
            if (IAll)
            {
                AnimationHelper.Start(element, TransitionType.ScanX, 1, 0.33, time);
                AnimationHelper.Start(element, TransitionType.ScanY, 1, 0.33, time);
            }
            else
            {
                AnimationHelper.Start(element, TransitionType.ScanX, 1, 3, time);
                AnimationHelper.Start(element, TransitionType.ScanY, 1, 3, time);
            }
            var desktopWorkingArea = SystemParameters.WorkArea;
            if (this.tipWindow.Left < 0)
            {
                AnimationHelper.Start(this.tipWindow, Window.LeftProperty, this.tipWindow.Left, 0, time);
            }
            if (this.tipWindow.Left > desktopWorkingArea.Width - this.tipWindow.ActualWidth)
            {
                AnimationHelper.Start(this.tipWindow, Window.LeftProperty, this.tipWindow.Left, desktopWorkingArea.Width - this.tipWindow.ActualWidth, time);
            }
            if (this.tipWindow.Top > desktopWorkingArea.Height - this.tipWindow.ActualHeight)
            {
                AnimationHelper.Start(this.tipWindow, Window.TopProperty, this.tipWindow.Top, desktopWorkingArea.Height - this.tipWindow.ActualHeight, time);
            }
        }

        #endregion

        #region 命令
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
                        break;
                    case "F":
                        break;
                    default:
                        Action(info.Tag.ToStrings());
                        break;
                }
            }
            listView1.SelectedIndex = -1;
        });
        public ICommand MenuCommand => new RelayCommand<string>(item => Action(item));
        public void Action(string item)
        {
            switch (item)
            {
                case "展开": this.IAll = true; break;
                case "收缩": this.IAll = false; break;
            }
        }

        #endregion

        public TipWindowModel()
        {
            Messenger.Default.Register<TipLoadMessage>(this, msg =>
            {
                if (msg.Obj is TipWindow window) this.tipWindow = window;
            });
        }
    }
}