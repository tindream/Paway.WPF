using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Paway.Helper;
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

namespace Paway.Test.ViewModel
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
    public class MainViewModel : ViewModelPlus
    {
        #region 属性
        private readonly List<ListViewModel> list;
        public List<ListViewModel> GridList { get { return list; } }
        public PagedCollectionView PagedList { get; private set; }
        public ObservableCollection<TreeViewModel> TreeList { get; private set; } = new ObservableCollection<TreeViewModel>();

        private ListViewModel selectedItem;
        public ListViewModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    if (value != null) Id = value.Id;
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<IComboBoxMulti> MultiList { get; } = new ObservableCollection<IComboBoxMulti>();

        private DateTime datePickerTime = DateTime.Now;
        public DateTime DatePickerTime
        {
            get { return datePickerTime; }
            set { datePickerTime = value; RaisePropertyChanged(); }
        }

        private int _value = 11;
        public int Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 命令
        private ICommand rowDoubleCommand;
        public ICommand RowDoubleCommand
        {
            get
            {
                return rowDoubleCommand ?? (rowDoubleCommand = new RelayCommand<RowDoubleEventArgs>(e =>
                {
                    if (e.Source is DataGridEXT datagrid1)
                    {
                        if (e.Item is ListViewModel info)
                        {
                        }
                    }
                }));
            }
        }

        private ICommand selectedItemChanged;
        public ICommand SelectedItemChanged
        {
            get
            {
                return selectedItemChanged ?? (selectedItemChanged = new RelayCommand<TreeViewEXT>(treeView =>
                {
                    if (treeView.SelectedItem is ITreeView item)
                    {
                        this.TreeId = item.Id;
                    }
                }));
            }
        }
        private ICommand sizeChanged;
        public ICommand SizeChanged
        {
            get
            {
                return sizeChanged ?? (sizeChanged = new RelayCommand<SliderEXT>(slider =>
                {
                    Config.FontSize = slider.Value;
                }));
            }
        }
        private ICommand colorChanged;
        public ICommand ColorChanged
        {
            get
            {
                return colorChanged ?? (colorChanged = new RelayCommand<SliderEXT>(slider =>
                {
                    var color = Method.ColorSelector(slider.Value / 7);
                    Config.Color = color;
                    Config.Background = Method.ColorSelector((slider.Value + 0.4) / 7).AddLight(0.93);
                    //Method.DoStyles();
                }));
            }
        }

        private ICommand cbxFilterCmd;
        public ICommand CbxFilterCmd
        {
            get
            {
                return cbxFilterCmd ?? (cbxFilterCmd = new RelayCommand<CustomFilterEventArgs>(e =>
                {
                    if (e.Source is DataGridEXT dataGrid)
                    {
                        var p = dataGrid.Columns.Predicate<ListViewModel>(e.Filter);
                        e.List = this.list.AsParallel().Where(p).ToList();
                    }
                }));
            }
        }

        private ICommand selectionCommand;
        public ICommand SelectionCommand
        {
            get
            {
                return selectionCommand ?? (selectionCommand = new RelayCommand<ListViewEXT>(listView1 =>
                {
                    if (listView1.SelectedItem is IListView info)
                    {
                        Method.Toast(listView1, Value);
                    }
                    //if (listView1.SelectedItem is IListViewInfo info) Method.Show(listView1, info.Content);
                    //listView1.SelectedIndex = -1;
                }));
            }
        }

        private ICommand teach;
        public ICommand Teach
        {
            get
            {
                return teach ?? (teach = new RelayCommand<Button>(btn =>
                {
                    MultiList[0].IsChecked = !MultiList[0].IsChecked;
                    var desc = string.Join(",", MultiList.ToList().FindAll(c => c.IsChecked).Select(c => c.Text));
                    Method.Toast(btn, desc, 5);
                }));
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            list = new List<ListViewModel>();
            list.Add(new ListViewModel("Hello"));
            list.Add(new ListViewModel("你好123")
            {
                Image = new ImageEXT(null, @"pack://application:,,,/Paway.Test;component/Images/close_while.png")
            });
            for (int i = 0; i < 20; i++) list.Add(new ListViewModel("A" + i, "D" + i)
            {
                Image = new ImageEXT(@"pack://application:,,,/Paway.Test;component/Images/close.png")
            });

            this.PagedList = new PagedCollectionView(list) { PageSize = 10 };

            MultiList.Add(new ComboBoxMultiModel("张三") { IsChecked = true });
            MultiList.Add(new ComboBoxMultiModel("李四"));
            MultiList.Add(new ComboBoxMultiModel("王五"));
            MultiList.Add(new ComboBoxMultiModel("马六"));
            MultiList.Add(new ComboBoxMultiModel("赵七") { IsChecked = true });
            MultiList.Add(new ComboBoxMultiModel("王八"));
            MultiList.Add(new ComboBoxMultiModel("陈九"));

            var treeInfo = new TreeViewModel("单位名称(3/7)", true);
            this.TreeList.Add(treeInfo);
            var treeInfo2 = new TreeViewModel("未分组联系人(2/4)", true);
            treeInfo.Add(treeInfo2);
            treeInfo2.Add("刘棒", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒1", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒2", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒3", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒4", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒5", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒6", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒7", "我要走向天空！", "3人");

            treeInfo2 = new TreeViewModel("未分组联系人", true);
            treeInfo.Add(treeInfo2);
            treeInfo2.Add("刘棒A", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒B", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒C", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒5", "我要走向天空！", "3人");

            this.TreeId = treeInfo2.Children[0].Id;
        }
    }
}