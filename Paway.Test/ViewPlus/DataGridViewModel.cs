using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Test.ViewModel
{
    public class DataGridViewModel<T> : ViewModelBase where T : class, IId, new()
    {
        #region 属性
        public ObservableCollection<T> List { get; protected set; } = new ObservableCollection<T>();
        private object selectedItem;
        public object SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; RaisePropertyChanged(); }
        }

        protected virtual AddViewModelPlus<T> ViewModel()
        {
            return new AddViewModelPlus<T>();
        }
        private AddViewModelPlus<T> addViewModel;
        protected AddViewModelPlus<T> AddViewModel
        {
            get
            {
                if (addViewModel == null) addViewModel = ViewModel();
                return addViewModel;
            }
        }

        public AuthInfo Auth
        {
            get { return Config.Auth; }
            set { RaisePropertyChanged(); }
        }

        #endregion

        #region 命令
        protected virtual Window AddWindow()
        {
            return null;
        }
        protected virtual List<T> Find()
        {
            return DataService.Default.Find<T>();
        }
        protected virtual void Added(T info)
        {
            DataService.Default.Insert(info);
            List.Add(info);
        }
        protected virtual void Updated(DependencyObject obj, T info)
        {
            DataService.Default.Update(info);
        }
        protected virtual void Deleted(DependencyObject obj, T info)
        {
            DataService.Default.Delete(info);
            List.Remove(info);
        }
        protected virtual void Selectioned(ListViewEXT listView1, IListViewItem item)
        {
            switch (item.Text)
            {
                case "刷新":
                    Refresh(listView1);
                    break;
                case "新加":
                    AddViewModel.Info = new T();
                    var add = AddWindow();
                    if (add != null && Method.ShowDialog(listView1, add) == true)
                    {
                        Added(AddViewModel.Info);
                        if (Method.Child(listView1, out DataGridEXT datagrid))
                        {
                            datagrid.SelectedIndex = List.Count - 1;
                        }
                    }
                    break;
                case "编辑":
                    Edit(listView1, selectedItem);
                    break;
                case "删除":
                    if (selectedItem is T infoDel)
                    {
                        if (Method.Ask(listView1, "确认删除：" + infoDel))
                        {
                            var index = -1;
                            if (Method.Child(listView1, out DataGridEXT datagrid))
                            {
                                index = datagrid.SelectedIndex;
                            }
                            Deleted(listView1, infoDel);
                            if (index >= List.Count) index = List.Count - 1;
                            datagrid.SelectedIndex = index;
                        }
                    }
                    break;
            }
            listView1.SelectedIndex = -1;
        }
        private ICommand selectionCommand;
        public ICommand SelectionCommand
        {
            get
            {
                return selectionCommand ?? (selectionCommand = new RelayCommand<ListViewEXT>(listView1 =>
                {
                    if (listView1.SelectedItem is IListViewItem item)
                    {
                        Selectioned(listView1, item);
                    }
                }));
            }
        }
        protected virtual void Refresh(DependencyObject obj)
        {
            Method.Progress(obj, () =>
            {
                var list = Find();
                Method.BeginInvoke(obj, () =>
                {
                    List.Clear();
                    foreach (var item in list) List.Add(item);
                });
            });
        }
        private void Edit(DependencyObject obj, object item)
        {
            if (item is T info)
            {
                AddViewModel.Info = info;
                var edit = AddWindow();
                if (Method.ShowDialog(obj, edit) == true)
                {
                    Updated(obj, info);
                }
            }
        }
        private ICommand doubleClick;
        public ICommand DoubleClick
        {
            get
            {
                return doubleClick ?? (doubleClick = new RelayCommand<DataGridEXT>(datagrid1 =>
                {
                    if ((Config.Auth.ButtonType & ButtonType.Update) == ButtonType.Update)
                    {
                        Edit(datagrid1, datagrid1.SelectedItem);
                    }
                }));
            }
        }


        #endregion

        #region 消息
        private void AuthApply()
        {
            this.Auth = Config.Auth;
        }

        #endregion

        public DataGridViewModel()
        {
            this.MessengerInstance.Register<AuthApplyMessage>(this, msg => AuthApply());
        }
    }
}