using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.Generic;

namespace Paway.Test
{
    public class ViewModelLocator : Paway.Model.ViewModelLocator
    {
        public static ViewModelLocator Default => new ViewModelLocator();

        public TipWindowModel Tip => GetModelInstance<TipWindowModel>();
        public TestWindowModel Test => GetModelInstance<TestWindowModel>();
        public MainWindowModel Main => GetModelInstance<MainWindowModel>();
    }
}