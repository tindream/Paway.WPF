using System.Windows;

namespace Paway.WPF
{
    /// <summary>
    /// 虚拟键盘-全键盘窗口
    /// <para>外壳行为（定位、不抢焦点、关闭转发）见 <see cref="KeyboardWindowBase"/></para>
    /// </summary>
    public partial class KeyboardAllWindow : KeyboardWindowBase
    {
        /// <summary>
        /// 虚拟键盘-全键盘
        /// </summary>
        public KeyboardAllWindow() : this(null, true) { }
        /// <summary>
        /// 虚拟键盘-全键盘
        /// </summary>
        public KeyboardAllWindow(FrameworkElement element, bool iTitle = false, bool iKeyboardNum = false) : base(element)
        {
            InitializeComponent();
            this.InitTitle(iTitle);
            keyboardAll.IKeyboardNum(iKeyboardNum);
            keyboardAll.CloseEvent += OnCloseClick;
            keyboardAll.DragMovedEvent += OnDragMoved;
            btnClose.Click += OnCloseClick;
        }
        /// <summary>
        /// 标题栏元素
        /// </summary>
        protected override FrameworkElement TitleBar { get { return dpTitle; } }
    }
}
