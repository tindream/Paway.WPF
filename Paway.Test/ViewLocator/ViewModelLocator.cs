using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.Generic;

namespace Paway.Test
{
    public class ViewModelLocator : Model.ViewModelLocator
    {
        public static ViewModelLocator Default => new ViewModelLocator();

        public TipWindowModel TipWindow => GetModelInstance<TipWindowModel>();
        public TestWindowModel TestWindow => GetModelInstance<TestWindowModel>();
        public TestDataGridModel TestDataGrid => GetModelInstance<TestDataGridModel>();
        public TestPageModel TestPage => GetModelInstance<TestPageModel>();
        public MainWindowModel MainWindow => GetModelInstance<MainWindowModel>();
    }
}