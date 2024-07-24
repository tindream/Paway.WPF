namespace Paway.Test
{
    public class ViewModelLocator : Model.ViewModelLocator
    {
        public static ViewModelLocator Default => new ViewModelLocator();

        public TipWindowModel Tip => GetModelInstance<TipWindowModel>();
        public TestWindowModel Test => GetModelInstance<TestWindowModel>();
        public MainWindowModel Main => GetModelInstance<MainWindowModel>();
    }
}