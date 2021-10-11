using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Paway.Helper;
using Paway.WPF;
using System.Windows.Input;

namespace Paway.Test.ViewModel
{
    /// <summary>
    /// ViewModelBase扩展
    /// </summary>
    public class ViewModelPlus : ViewModelBase
    {
        public void RaisePropertyChanged()
        {
            base.RaisePropertyChanged(WPF.PMethod.GetLastModelName());
        }
    }
}