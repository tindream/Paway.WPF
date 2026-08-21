using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Paway.Helper;
using Paway.Model;
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
    public class TipWindowModel : ViewModelBasePlus
    {
        #region 属性
        private TipWindow tipWindow;
        private bool _iAll;
        public bool IAll
        {
            get { return _iAll; }
            set
            {
                _iAll = value;
                OnPropertyChanged();
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
            WeakReferenceMessenger.Default.Send(new TipStateMessage());
        }

        #endregion

        #region 命令
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
        protected override void Action(ListViewCustom listView1, SelectionChangedEventArgs e)
        {
            base.Action(listView1, e);
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
        }
        public override bool Action(string item)
        {
            switch (item)
            {
                case "展开": this.IAll = true; break;
                case "收缩": this.IAll = false; break;
            }
            return base.Action(item);
        }

        #endregion

        public TipWindowModel()
        {
            WeakReferenceMessenger.Default.Register<TipLoadMessage>(this, (obj, msg) =>
            {
                if (msg.Obj is TipWindow window) this.tipWindow = window;
            });
        }
    }
}