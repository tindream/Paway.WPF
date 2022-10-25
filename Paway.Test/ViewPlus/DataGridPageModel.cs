using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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

namespace Paway.Test
{
    public class DataGridPageModel<T> : ViewModelBase where T : class, IId, new()
    {
        #region 属性
        protected DataGridEXT DataGrid;
        public ObservableCollection<T> List { get; protected set; } = new ObservableCollection<T>();
        private T _selectedItem;
        public virtual T SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; RaisePropertyChanged(); }
        }

        protected virtual AddWindowModel<T> ViewModel()
        {
            return new AddWindowModel<T>();
        }
        private AddWindowModel<T> addViewModel;
        protected AddWindowModel<T> AddViewModel
        {
            get
            {
                if (addViewModel == null) addViewModel = ViewModel();
                return addViewModel;
            }
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
        protected virtual void Insert(T info)
        {
            DataService.Default.Insert(info);
            Method.Update(info);
            Method.Sorted(Cache.List<T>());
            var index = Cache.List<T>().FindIndex(c => c.Id == info.Id);
            List.Insert(index, info);
        }
        protected virtual void Updated(DependencyObject obj, T info)
        {
            DataService.Default.Update(info);
        }
        protected virtual void Deleted(DependencyObject obj, T info)
        {
            var temp = List.ToList().Find(c => c.Id == info.Id);
            if (temp != null)
            {
                DataService.Default.Delete(temp);
                List.Remove(temp);
            }
        }
        protected virtual void Selectioned(ListViewEXT listView1, IListViewItem item)
        {
            try
            {
                Selectioned(listView1, item.Text);
                listView1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new StatuMessage(ex, listView1));
            }
        }
        protected virtual void Selectioned(DependencyObject obj, string text)
        {
            switch (text)
            {
                case "刷新":
                    Refresh(obj);
                    break;
                case "添加":
                    AddViewModel.Info = new T();
                    var add = AddWindow();
                    if (add != null && Method.Show(obj, add) == true)
                    {
                        Insert(AddViewModel.Info);
                        var index = List.ToList().FindIndex(c => c.Id == AddViewModel.Info.Id);
                        DataGrid.SelectedIndex = index;
                    }
                    break;
                case "编辑":
                    if (SelectedItem is T info)
                    {
                        AddViewModel.Info = info;
                        var edit = AddWindow();
                        if (Method.Show(obj, edit) == true)
                        {
                            try
                            {
                                Updated(obj, info);
                            }
                            catch (Exception ex)
                            {
                                Messenger.Default.Send(new StatuMessage(ex, obj));
                            }
                        }
                    }
                    break;
                case "删除":
                    if (SelectedItem is T infoDel)
                    {
                        if (Method.Ask(obj, $"确认删除：[{infoDel.GetType().Description()}]" + infoDel))
                        {
                            var index = DataGrid.SelectedIndex;
                            try
                            {
                                Deleted(obj, infoDel);
                            }
                            finally
                            {
                                if (index >= List.Count) index = List.Count - 1;
                                DataGrid.SelectedIndex = index;
                            }
                        }
                    }
                    break;
            }
        }
        public ICommand SelectionCommand => new RelayCommand<ListViewEXT>(listView1 =>
        {
            if (listView1.SelectedItem is IListViewItem item)
            {
                Selectioned(listView1, item);
            }
        });
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
        public ICommand DoubleClick => new RelayCommand<DataGridEXT>(datagrid1 =>
        {
            Selectioned(datagrid1, "编辑");
        });

        #endregion

        public DataGridPageModel() { }
    }
}