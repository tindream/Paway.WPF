using System.Collections.Generic;

namespace Paway.Test
{
    public class ViewModelLocator : Model.ViewModelLocator
    {
        public static ViewModelLocator Default => new ViewModelLocator();
        public TestWindowModel TestWindow => GetModelInstance<TestWindowModel>();

        public TipWindowModel TipWindow => GetModelInstance<TipWindowModel>();

        public TestDataGridModel TestDataGrid => GetModelInstance<TestDataGridModel>();
        public TestImageModel TestImage => GetModelInstance<TestImageModel>();
        public TestTabControlModel TestTabControl => GetModelInstance<TestTabControlModel>();
        public TestExpanderModel TestExpander => GetModelInstance<TestExpanderModel>();
        public TestTreeViewModel TestTreeView => GetModelInstance<TestTreeViewModel>();
        public TestPlotViewModel TestPlotView => GetModelInstance<TestPlotViewModel>();
        public TestComboBoxModel TestComboBox => GetModelInstance<TestComboBoxModel>();
        public TestTextBoxModel TestTextBox => GetModelInstance<TestTextBoxModel>();
        public TestButtonModel TestButton => GetModelInstance<TestButtonModel>();
        public TestRadioModel TestRadio => GetModelInstance<TestRadioModel>();
        public TestCheckBoxModel TestCheckBox => GetModelInstance<TestCheckBoxModel>();
        public TestProgressBarModel TestProgressBar => GetModelInstance<TestProgressBarModel>();
        
        public EmptyModel Empty => GetModelInstance<EmptyModel>(); 

        public override Model.LoginPageModel Login => GetModelInstance<LoginPageModel>();
    }
}