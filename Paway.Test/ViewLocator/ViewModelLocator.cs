using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.Generic;

namespace Paway.Test
{
    public class ViewModelLocator : Paway.Model.ViewModelLocator
    {
        public static ViewModelLocator Default => new ViewModelLocator();

        public TipWindowModel Tip => GetInstance<TipWindowModel>();
        public TestWindowModel Test => GetInstance<TestWindowModel>();
        public MainWindowModel Main => GetInstance<MainWindowModel>();
    }
}