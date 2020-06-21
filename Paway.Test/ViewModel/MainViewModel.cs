using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Paway.WPF;
using System.Collections.Generic;
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

        #endregion

        #region 命令
        private ICommand selectionCommand;
        public ICommand SelectionCommand
        {
            get
            {
                return selectionCommand ?? (selectionCommand = new RelayCommand<ListViewEXT>(listView1 =>
                {
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
                    Method.Toast(btn, "Hello");
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

            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }
    }
}