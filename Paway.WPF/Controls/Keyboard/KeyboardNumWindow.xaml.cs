using System.Windows;

namespace Paway.WPF
{
    /// <summary>
    /// 虚拟键盘-数字键盘窗口
    /// <para>外壳行为（定位、不抢焦点、关闭转发）见 <see cref="KeyboardWindowBase"/></para>
    /// </summary>
    public partial class KeyboardNumWindow : KeyboardWindowBase
    {
        /// <summary>
        /// 虚拟键盘-数字键盘
        /// </summary>
        public KeyboardNumWindow() : this(null, true) { }
        /// <summary>
        /// 虚拟键盘-数字键盘
        /// </summary>
        public KeyboardNumWindow(FrameworkElement element, bool iTitle = false) : base(element)
        {
            InitializeComponent();
            this.InitTitle(iTitle);
            keyboardNum.CloseEvent += OnCloseClick;
            keyboardNum.DragMovedEvent += OnDragMoved;
            btnClose.Click += OnCloseClick;
        }
        /// <summary>
        /// 标题栏元素
        /// </summary>
        protected override FrameworkElement TitleBar { get { return dpTitle; } }
    }
}
