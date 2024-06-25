using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.Utils;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// 数据管理基类，必须指定当前菜单、及初始化
    /// </summary>
    public abstract class DataGridPageModel<T> : OperateItemModel where T : class, IOperateOnceInfo, ICompare<T>, new()
    {
        #region 属性
        /// <summary>
        /// 数据处理接口
        /// </summary>
        protected IDataGridService server;
        /// <summary>
        /// 数据控件
        /// </summary>
        protected DataGridEXT DataGrid;
        /// <summary>
        /// 刷新时数据库过滤条件
        /// </summary>
        private string sqlFilter;
        /// <summary>
        /// 源数据列表
        /// </summary>
        public List<T> List;
        /// <summary>
        /// List为全局缓存，当前页可能需要过滤显示
        /// </summary>
        protected Func<T, bool> listFilter;
        /// <summary>
        /// 重载-自定义过滤列表
        /// </summary>
        protected virtual List<T> FilterList()
        {
            return List.FindAll(c => c != null && listFilter?.Invoke(c) != false);
        }
        /// <summary>
        /// 当前显示数据列表
        /// </summary>
        private List<T> showList;
        /// <summary>
        /// 界面绑定列表
        /// </summary>
        public ObservableCollection<T> ObList { get; private set; } = new ObservableCollection<T>();
        /// <summary>
        /// 分页标记
        /// </summary>
        private bool IPage;
        /// <summary>
        /// 分页列表
        /// </summary>
        public PagedCollectionView PagedList { get; private set; }

        /// <summary>
        /// 选中项后处理
        /// </summary>
        protected virtual void SelectedChanged() { }
        private T _selectedItem;
        /// <summary>
        /// 当前选中项
        /// </summary>
        public virtual T SelectedItem
        {
            get { return _selectedItem; }
            set { if (_selectedItem != value) { _selectedItem = value; SelectedChanged(); OnPropertyChanged(); } }
        }
        /// <summary>
        /// 设置添加模型
        /// </summary>

        protected virtual AddWindowModel<T> ViewModel()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取主DataGrid选中项
        /// <para>选择单元格时，SelectedItem为空</para>
        /// </summary>
        protected T SelectedInfo()
        {
            if (SelectedItem != null) return SelectedItem;
            if (DataGrid.CurrentItem is T t) return t;
            if (DataGrid.SelectionUnit != DataGridSelectionUnit.FullRow && DataGrid.SelectedCells.Count > 0)
            {
                if (DataGrid.SelectedCells.First().Item is T t2) return t2;
            }
            return default;
        }
        /// <summary>
        /// 多选行
        /// <para>选择单元格时，SelectedItems为空</para>
        /// </summary>
        protected List<T> SelectedInfos()
        {
            var list = new List<T>();
            foreach (T item in DataGrid.SelectedItems)
            {
                list.Add(item);
            }
            if (list.Count > 0) return list;
            if (DataGrid.SelectionUnit != DataGridSelectionUnit.FullRow && DataGrid.SelectedCells.Count > 0)
            {
                for (var i = 0; i < DataGrid.SelectedCells.Count; i++)
                {
                    if (DataGrid.SelectedCells[i].Item is T item) list.Add(item);
                }
            }
            return list.Distinct().ToList();
        }

        #endregion

        #region 命令
        /// <summary>
        /// 设置添加窗体
        /// </summary>
        protected virtual Window AddWindow() { return null; }
        /// <summary>
        /// 重载-自定义查询列表
        /// </summary>
        protected virtual List<T> Find()
        {
            var list = server.Find<T>(this.sqlFilter); list.Sorted();
            return list;
        }
        /// <summary>
        /// 重载-自定义插入实体
        /// </summary>
        protected virtual void Insert(T info)
        {
            server.Insert(info); Cache.Update(info);
            var index = this.FilterList().FindIndex(c => c.Id == info.Id);
            if (!this.SearchReset() && index != -1) PMethod.Invoke(() => ObList.Insert(index, info));
            MoveTo(index, info);
        }
        private void MoveTo(int index, T info)
        {
            PMethod.BeginInvoke(() =>
            {
                if (IPage)
                {
                    PagedList.MoveToPage(index / PagedList.PageSize);
                    index %= PagedList.PageSize;
                }
                DataGrid.ScrollIntoView(info);
                if (DataGrid.SelectionUnit == DataGridSelectionUnit.FullRow) DataGrid.SelectedIndex = index;
                else DataGrid.Select(info.Id, true);
            });
        }
        /// <summary>
        /// 重载-自定义插入列表
        /// </summary>
        protected virtual void Insert(List<T> list)
        {
            if (list.Count == 0) return;
            server.Insert(list); Cache.Update(list);
            int index = 0;
            foreach (var info in list)
            {
                index = this.FilterList().FindIndex(c => c.Id == info.Id);
                if (!this.SearchReset()) PMethod.Invoke(() => ObList.Insert(index, info));
            }
            MoveTo(index, list.Last());
        }
        /// <summary>
        /// 重载-自定义更新实体
        /// </summary>
        protected virtual void Updated(T info)
        {
            if (info is IOperateInfo operateUser)
            {
                operateUser.UpdateOn = DateTime.Now;
            }
            server.Update(info);
        }
        /// <summary>
        /// 重载-自定义删除实体
        /// </summary>
        protected virtual void Deleted(T info)
        {
            var index = DataGrid.SelectedIndex;
            if (DataGrid.SelectionUnit != DataGridSelectionUnit.FullRow && DataGrid.SelectedCells.Count > 0)
            {
                if (DataGrid.ItemContainerGenerator.ContainerFromItem(DataGrid.SelectedCells[0].Item) is DataGridRow row)
                {
                    index = row.GetIndex();
                }
            }
            try
            {
                server.Delete(info); Cache.Delete(info);
                PMethod.Invoke(() => ObList.Remove(info));
            }
            finally
            {
                if (index >= DataGrid.Items.Count) index = DataGrid.Items.Count - 1;
                if (DataGrid.SelectionUnit == DataGridSelectionUnit.FullRow) DataGrid.SelectedIndex = index;
                else if (index >= 0) DataGrid.Select(this.FilterList()[index].Id, true);
            }
        }
        /// <summary>
        /// 重载-自定义删除列表
        /// </summary>
        protected virtual void Deleted(List<T> list)
        {
            if (list.Count == 0) return;
            var index = DataGrid.SelectedIndex;
            if (DataGrid.SelectionUnit != DataGridSelectionUnit.FullRow && DataGrid.SelectedCells.Count > 0)
            {
                if (DataGrid.ItemContainerGenerator.ContainerFromItem(DataGrid.SelectedCells[0].Item) is DataGridRow row)
                {
                    index = row.GetIndex();
                }
            }
            try
            {
                server.Delete(list); Cache.Delete(list);
                PMethod.Invoke(() => { foreach (var info in list) ObList.Remove(info); });
            }
            finally
            {
                if (index >= DataGrid.Items.Count) index = DataGrid.Items.Count - 1;
                if (DataGrid.SelectionUnit == DataGridSelectionUnit.FullRow) DataGrid.SelectedIndex = index;
                else if (index >= 0) DataGrid.Select(this.FilterList()[index].Id, true);
            }
        }
        /// <summary>
        /// 重载-自定义刷新操作
        /// </summary>
        protected override void Refresh(Action action = null)
        {
            PMethod.BeginInvoke(() =>
            {
                PMethod.Progress(PMethod.Window(DataGrid), () =>
                {
                    Init(Find());
                    action?.Invoke();
                }, null, ex =>
                {
                    Messenger.Default.Send(new StatuMessage(ex));
                });
            });
        }
        /// <summary>
        /// 重载-自定义导入列表前检查
        /// </summary>
        protected virtual void ImportChecked(List<T> list)
        {
            if (typeof(IIndex).IsAssignableFrom(typeof(T)))
            {
                var tList = Cache.FindAll<T>();
                var index = tList.Count == 0 ? 0 : tList.Max(c => ((IIndex)c).Index) + 1;
                for (var i = 0; i < list.Count; i++)
                {
                    ((IIndex)list[i]).Index = index + i;
                }
            }
        }
        /// <summary>
        /// 重载-自定义导入列表
        /// </summary>
        protected virtual void Import(List<T> list)
        {
            var updateList = PMethod.Import(this.FilterList(), list);
            var timeNow = DateTime.Now;
            if (typeof(IOperateInfo).IsAssignableFrom(list.GenericType()))
            {
                updateList.ForEach(c => ((IOperateInfo)c).UpdateOn = timeNow);
            }
            server.Replace(updateList); Cache.Update(updateList);
            this.Reload();
        }
        /// <summary>
        /// 重载-自定义导出到文件
        /// </summary>
        protected virtual void Export(string file, bool iOpen = true)
        {
            Export(DataGrid, this.FilterList(), file, iOpen);
        }
        /// <summary>
        /// 重载-导出指定列表到文件
        /// </summary>
        protected virtual void Export<O>(List<O> list, string file, bool iOpen = true) where O : class
        {
            base.Export(DataGrid, list, file, iOpen);
        }

        #endregion

        #region 操作命令
        /// <summary>
        /// 双击命令
        /// </summary>
        public ICommand RowDoubleCommand => new RelayCommand<SelectItemEventArgs>(e =>
        {
            ActionInternalMsg("编辑");
        });
        /// <summary>
        /// 按钮列表-点击命令处理
        /// </summary>
        protected override void Action(ListViewCustom listView1)
        {
            base.Action(listView1);
            if (listView1.SelectedItem is IListViewItem item)
            {
                Selectioned(listView1, item);
            }
        }
        /// <summary>
        /// 按钮列表-选中
        /// </summary>
        protected virtual void Selectioned(ListViewCustom listView1, IListViewItem item)
        {
            Action(item.Text);
            listView1.SelectedIndex = -1;
        }
        /// <summary>
        /// 键盘按键事件
        /// </summary>
        protected override void Action(KeyMessage msg)
        {
            if (PConfig.Menu != this.Menu) return;
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (PMethod.Find(DataGrid, out TextBoxEXT tbSearch, "tbSearch"))
                {
                    if (tbSearch.IsKeyboardFocusWithin) return;
                }
            }
            base.Action(msg);
        }
        /// <summary>
        /// 通用动作命令
        /// </summary>
        public override bool Action(string item)
        {
            switch (item)
            {
                default:
                    base.Action(item);
                    break;
                case "添加":
                    ViewModel().Info = new T();
                    var add = AddWindow();
                    if (add != null && PMethod.ShowWindow(DataGrid, add) == true)
                    {
                        Insert(ViewModel().Info);
                    }
                    break;
                case "编辑":
                    if (SelectedInfo() is T info)
                    {
                        ViewModel().Info = info;
                        var edit = AddWindow();
                        if (edit != null && PMethod.ShowWindow(DataGrid, edit) == true)
                        {
                            Updated(info);
                        }
                    }
                    break;
                case "删除":
                    if (SelectedInfo() is T infoDel)
                    {
                        if (PMethod.Ask(DataGrid, $"确认删除：[{infoDel.GetType().Description()}]" + infoDel))
                        {
                            Deleted(infoDel);
                        }
                    }
                    break;
                case "导入":
                    var title = typeof(T).Description();
                    if (PMethod.OpenFile($"选择要导入的 {title} 表", out string file))
                    {
                        PMethod.Progress(PMethod.Window(DataGrid), "正在导入..", adorner =>
                        {
                            var list = ExcelBuilder.Create(file).ToList<T>();
                            ImportChecked(list);
                            Import(list);
                        }, () =>
                        {
                            Messenger.Default.Send(new StatuMessage($"{title} 导入完成", DataGrid));
                        }, error: ex =>
                        {
                            Messenger.Default.Send(new StatuMessage("导入失败", ex, DataGrid));
                        });
                    }
                    break;
                case "导出":
                    title = $"{typeof(T).Description()}{DateTime.Now:yyyy-MM-dd}.xlsx";
                    if (PMethod.SaveFile(title, out file))
                    {
                        Export(file);
                    }
                    break;
            }
            return base.Action(item);
        }
        /// <summary>
        /// 初始化列表、数据库获取接口、分页
        /// </summary>
        protected void Init(IDataGridService server, List<T> list, DataGridEXT dataGrid, bool iPage = false)
        {
            this.Init(server, list, dataGrid, null, iPage);
        }
        /// <summary>
        /// 初始化列表、数据库获取接口、分页
        /// <para>允许数据过滤</para>
        /// </summary>
        protected void Init(IDataGridService server, List<T> list, DataGridEXT dataGrid, string sqlFilter, bool iPage = false)
        {
            this.server = server;
            this.sqlFilter = sqlFilter;
            this.IPage = iPage;
            this.DataGrid = dataGrid;
            this.Init(list, list.Count > 0);
            if (this.List.Count == 0) this.Refresh();
        }

        #endregion
        #region 拖拽排序
        /// <summary>
        /// 拖拽排序
        /// </summary>
        public ICommand DragCompletedCmd => new RelayCommand<DataGridDragEventArgs>(e =>
        {
            if (typeof(IIndex).IsAssignableFrom(typeof(T)))
            {
                var updateList = new List<T>();
                for (var i = 0; i < ObList.Count; i++)
                {
                    var item = List.Find(c => c != null && c.Id == ObList[i].Id);
                    if (item is IIndex index)
                    {
                        index.Index = i;
                        updateList.Add(item);
                    }
                }
                server.Update(updateList, null, nameof(IIndex.Index)); List.Sorted();
            }
        });

        #endregion

        #region 加载列表
        /// <summary>
        /// 加载列表
        /// </summary>
        protected void Init(List<T> list, bool iReload = true)
        {
            if (this.List == null) this.List = list;
            else
            {
                PMethod.UpdateList(OperType.Reset, List, list);
            }
            if (iReload) this.Reload();
        }
        /// <summary>
        /// 搜索列表
        /// </summary>
        protected override void Search()
        {
            if (SearchText.IsEmpty())
            {
                this.showList = null;
            }
            else
            {
                this.showList = this.FilterList().FindLabbda(SearchText.Trim());
            }
            this.ReloadObList();
        }
        /// <summary>
        /// 重加载
        /// </summary>
        protected virtual void Reload()
        {
            if (!SearchReset()) ReloadObList();
        }
        private bool SearchReset()
        {
            if (!SearchText.IsEmpty())
            {
                base.ClearSearch();
                this.showList = null;
                this.ReloadObList();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 重加载列表
        /// </summary>
        protected virtual void ReloadObList()
        {
            PMethod.Invoke(() =>
            {
                ObList.Clear();
                var list = showList ?? this.FilterList();
                foreach (var item in list) ObList.Add(item);
            });
        }
        /// <summary>
        /// 默认权限
        /// <para>刷新、添加、编辑、删除、导入、导出、搜索</para>
        /// </summary>
        protected override void AuthNormal()
        {
            this.Auth = MenuAuthType.Refresh | MenuAuthType.Add | MenuAuthType.Edit | MenuAuthType.Delete | MenuAuthType.Import | MenuAuthType.Export | MenuAuthType.Search;
        }
        /// <summary>
        /// 只读权限
        /// <para>刷新、导出、搜索</para>
        /// </summary>
        protected void AuthView()
        {
            this.Auth = MenuAuthType.Refresh | MenuAuthType.Export | MenuAuthType.Search;
        }
        /// <summary>
        /// 无搜索
        /// </summary>
        protected void AuthNoSearch()
        {
            if ((Auth & MenuAuthType.Search) == MenuAuthType.Search)
            {
                this.Auth ^= MenuAuthType.Search;
            }
        }

        #endregion

        /// <summary>
        /// 数据管理基类，必须指定当前菜单、及初始化
        /// </summary>
        public DataGridPageModel()
        {
            this.PagedList = new PagedCollectionView(ObList) { PageSize = 20 };
        }
    }
}