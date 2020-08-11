using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

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
                return teach ?? (teach = new RelayCommand<Button>((btn) =>
                {
                    Method.Toast(btn, "Hello", 5);
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

            MultiList.Add(new ComboBoxMultiModel("张三"));
            MultiList.Add(new ComboBoxMultiModel("李四"));
            MultiList.Add(new ComboBoxMultiModel("王五"));
            MultiList.Add(new ComboBoxMultiModel("马六"));
            MultiList.Add(new ComboBoxMultiModel("赵七"));
            MultiList.Add(new ComboBoxMultiModel("王八"));
            MultiList.Add(new ComboBoxMultiModel("陈九"));
        }
    }
}