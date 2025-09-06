﻿using Microsoft.Win32;
using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法 - 系统
    /// </summary>
    public partial class PMethod
    {
        #region 键盘事件转输入字符
        /// <summary>
        /// 键盘事件转输入字符
        /// <para>System.Windows.Input.Key解析结构</para>
        /// </summary>
        public static bool KeyToChar(Key key, out char keycode)
        {
            var decodeInfo = KeyToChar(key);
            keycode = '\0';
            if (decodeInfo.Printable)
            {
                keycode = decodeInfo.Character;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 键盘事件转输入字符
        /// <para>System.Windows.Input.Key解析结构</para>
        /// </summary>
        public static KeyDecodeInfo KeyToChar(Key key)
        {
            var keyDecode = new KeyDecodeInfo();
            keyDecode.Key = key;

            keyDecode.Alt = Keyboard.IsKeyDown(Key.LeftAlt) ||
                              Keyboard.IsKeyDown(Key.RightAlt);

            keyDecode.Ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) ||
                              Keyboard.IsKeyDown(Key.RightCtrl);

            keyDecode.Shift = Keyboard.IsKeyDown(Key.LeftShift) ||
                              Keyboard.IsKeyDown(Key.RightShift);

            if (keyDecode.Alt || keyDecode.Ctrl)
            {
                keyDecode.Printable = false;
                keyDecode.Type = 1;
            }
            else
            {
                keyDecode.Printable = true;
                keyDecode.Type = 0;
            }

            var shift = keyDecode.Shift;
            var caplock = Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.Toggled;
            var iscap = (caplock && !shift) || (!caplock && shift);
            switch (key)
            {
                case Key.Enter: keyDecode.Character = '\n'; break;
                case Key.A: keyDecode.Character = (iscap ? 'A' : 'a'); break;
                case Key.B: keyDecode.Character = (iscap ? 'B' : 'b'); break;
                case Key.C: keyDecode.Character = (iscap ? 'C' : 'c'); break;
                case Key.D: keyDecode.Character = (iscap ? 'D' : 'd'); break;
                case Key.E: keyDecode.Character = (iscap ? 'E' : 'e'); break;
                case Key.F: keyDecode.Character = (iscap ? 'F' : 'f'); break;
                case Key.G: keyDecode.Character = (iscap ? 'G' : 'g'); break;
                case Key.H: keyDecode.Character = (iscap ? 'H' : 'h'); break;
                case Key.I: keyDecode.Character = (iscap ? 'I' : 'i'); break;
                case Key.J: keyDecode.Character = (iscap ? 'J' : 'j'); break;
                case Key.K: keyDecode.Character = (iscap ? 'K' : 'k'); break;
                case Key.L: keyDecode.Character = (iscap ? 'L' : 'l'); break;
                case Key.M: keyDecode.Character = (iscap ? 'M' : 'm'); break;
                case Key.N: keyDecode.Character = (iscap ? 'N' : 'n'); break;
                case Key.O: keyDecode.Character = (iscap ? 'O' : 'o'); break;
                case Key.P: keyDecode.Character = (iscap ? 'P' : 'p'); break;
                case Key.Q: keyDecode.Character = (iscap ? 'Q' : 'q'); break;
                case Key.R: keyDecode.Character = (iscap ? 'R' : 'r'); break;
                case Key.S: keyDecode.Character = (iscap ? 'S' : 's'); break;
                case Key.T: keyDecode.Character = (iscap ? 'T' : 't'); break;
                case Key.U: keyDecode.Character = (iscap ? 'U' : 'u'); break;
                case Key.V: keyDecode.Character = (iscap ? 'V' : 'v'); break;
                case Key.W: keyDecode.Character = (iscap ? 'W' : 'w'); break;
                case Key.X: keyDecode.Character = (iscap ? 'X' : 'x'); break;
                case Key.Y: keyDecode.Character = (iscap ? 'Y' : 'y'); break;
                case Key.Z: keyDecode.Character = (iscap ? 'Z' : 'z'); break;
                case Key.D0: keyDecode.Character = (shift ? ')' : '0'); break;
                case Key.D1: keyDecode.Character = (shift ? '!' : '1'); break;
                case Key.D2: keyDecode.Character = (shift ? '@' : '2'); break;
                case Key.D3: keyDecode.Character = (shift ? '#' : '3'); break;
                case Key.D4: keyDecode.Character = (shift ? '$' : '4'); break;
                case Key.D5: keyDecode.Character = (shift ? '%' : '5'); break;
                case Key.D6: keyDecode.Character = (shift ? '^' : '6'); break;
                case Key.D7: keyDecode.Character = (shift ? '&' : '7'); break;
                case Key.D8: keyDecode.Character = (shift ? '*' : '8'); break;
                case Key.D9: keyDecode.Character = (shift ? '(' : '9'); break;
                case Key.OemPlus: keyDecode.Character = (shift ? '+' : '='); break;
                case Key.OemMinus: keyDecode.Character = (shift ? '_' : '-'); break;
                case Key.OemQuestion: keyDecode.Character = (shift ? '?' : '/'); break;
                case Key.OemComma: keyDecode.Character = (shift ? '<' : ','); break;
                case Key.OemPeriod: keyDecode.Character = (shift ? '>' : '.'); break;
                case Key.OemOpenBrackets: keyDecode.Character = (shift ? '{' : '['); break;
                case Key.OemQuotes: keyDecode.Character = (shift ? '"' : '\''); break;
                case Key.Oem1: keyDecode.Character = (shift ? ':' : ';'); break;
                case Key.Oem3: keyDecode.Character = (shift ? '~' : '`'); break;
                case Key.Oem5: keyDecode.Character = (shift ? '|' : '\\'); break;
                case Key.Oem6: keyDecode.Character = (shift ? '}' : ']'); break;
                case Key.Tab: keyDecode.Character = '\t'; break;
                case Key.Space: keyDecode.Character = ' '; break;

                // Number Pad
                case Key.NumPad0: keyDecode.Character = '0'; break;
                case Key.NumPad1: keyDecode.Character = '1'; break;
                case Key.NumPad2: keyDecode.Character = '2'; break;
                case Key.NumPad3: keyDecode.Character = '3'; break;
                case Key.NumPad4: keyDecode.Character = '4'; break;
                case Key.NumPad5: keyDecode.Character = '5'; break;
                case Key.NumPad6: keyDecode.Character = '6'; break;
                case Key.NumPad7: keyDecode.Character = '7'; break;
                case Key.NumPad8: keyDecode.Character = '8'; break;
                case Key.NumPad9: keyDecode.Character = '9'; break;
                case Key.Subtract: keyDecode.Character = '-'; break;
                case Key.Add: keyDecode.Character = '+'; break;
                case Key.Decimal: keyDecode.Character = '.'; break;
                case Key.Divide: keyDecode.Character = '/'; break;
                case Key.Multiply: keyDecode.Character = '*'; break;

                default:
                    keyDecode.Type = 1;
                    keyDecode.Printable = false;
                    keyDecode.Character = '\x00';
                    break;
            }

            return keyDecode;
        }

        #endregion

        #region 导入导出框
        /// <summary>
        /// 选择文件
        /// </summary>
        public static new bool OpenFile(out string file, string title = null, string filter = "Excel 工作簿|*.xls;*.xlsx")
        {
            file = null;
            var ofd = new OpenFileDialog
            {
                Title = title ?? "选择文件",
                Filter = filter,
                InitialDirectory = TConfig.FileDialogPath,
            };
            if (ofd.ShowDialog() == true)
            {
                file = ofd.FileName;
                TConfig.FileDialogPath = Path.GetDirectoryName(ofd.FileName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 选择多个文件
        /// </summary>
        public static new bool OpenFiles(out string[] files, string title = null, string filter = "Excel 工作簿|*.xls;*.xlsx")
        {
            files = new string[0];
            var ofd = new OpenFileDialog
            {
                Title = title ?? "选择多个文件",
                Filter = filter,
                Multiselect = true,
                InitialDirectory = TConfig.FileDialogPath,
            };
            if (ofd.ShowDialog() == true)
            {
                files = ofd.FileNames;
                TConfig.FileDialogPath = Path.GetDirectoryName(ofd.FileName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        public static new bool SaveFile(out string outFile, string title = null, string filter = null)
        {
            return SaveFileName(out outFile, null, title, filter);
        }
        /// <summary>
        /// 保存文件，并指定文件名称
        /// </summary>
        public static new bool SaveFileName(out string outFile, string fileName, string title = null, string filter = null)
        {
            if (filter == null)
            {
                var extension = Path.GetExtension(fileName);
                switch (extension)
                {
                    case ".xls":
                    case ".xlsx": filter = $"Excel 工作簿|*{extension}|所有文件|*.*"; break;
                    case ".doc":
                    case ".docx": filter = $"Word 文档|*{extension}|所有文件|*.*"; break;
                    case ".ppt":
                    case ".pptx": filter = $"PPT 文稿|*{extension}|所有文件|*.*"; break;
                    case ".pdf": filter = $"PDF 文件|*{extension}|所有文件|*.*"; break;
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".bmp": filter = $"图像文件|*{extension}|所有文件|*.*"; break;
                    case ".avi":
                    case ".wmv":
                    case ".mp4":
                    case ".mpg":
                    case ".mpeg":
                    case ".mov":
                    case ".rm":
                    case ".ram":
                    case ".swf":
                    case ".flv": filter = $"视频文件|*{extension}|所有文件|*.*"; break;
                    case ".txt": filter = $"文本文件|*{extension}|所有文件|*.*"; break;
                    default: filter = $"文件|*{extension}|所有文件|*.*"; break;
                }
            }
            outFile = null;
            var sfd = new SaveFileDialog
            {
                Title = title ?? PConfig.LanguageBase.SelectSaveFile,
                Filter = filter,
                FileName = fileName,
                InitialDirectory = TConfig.FileDialogPath,
            };
            if (sfd.ShowDialog() == true)
            {
                outFile = sfd.FileName;
                TConfig.FileDialogPath = Path.GetDirectoryName(sfd.FileName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 打开文件夹
        /// </summary>
        public static new bool OpenFolder(IntPtr ower, out string outPath, string title = null, string selectedPath = null)
        {
            FolderSelectDialog dialog = new FolderSelectDialog
            {
                Title = title ?? "打开文件夹",
            };
            if (selectedPath != null) dialog.InitialDirectory = selectedPath;
            outPath = null;
            if (dialog.ShowDialog(ower))
            {
                outPath = dialog.FileName;
                return true;
            }
            return false;
        }

        #endregion

        #region 对一个Handle控件进行截图
        /// <summary>
        /// 对一个Handle控件进行截图
        /// </summary>
        public static BitmapSource PrintWindow(FrameworkElement framework, IntPtr intptr)
        {
            // 获取控件的实际尺寸（考虑缩放和布局）
            double actualHeight = framework.ActualHeight;
            double actualWidth = framework.ActualWidth;

            // 如果控件尺寸为0，则无法截图
            if (actualHeight <= 0 || actualWidth <= 0) return null;

            // 创建 RenderTargetBitmap
            var renderTargetBitmap = new RenderTargetBitmap(
                (int)actualWidth,
                (int)actualHeight,
                96, 96, PixelFormats.Pbgra32);

            // 渲染控件到 bitmap
            renderTargetBitmap.Render(framework);

            return renderTargetBitmap;
        }

        #endregion

        #region 代码移除全局焦点样式
        /// <summary>
        /// 代码移除全局焦点样式
        /// <para>在主窗体构造或更早之前调用</para>
        /// </summary>
        public static void RemoveFocusVisualStyle()
        {
            EventManager.RegisterClassHandler(typeof(FrameworkElement), FrameworkElement.GotFocusEvent, new RoutedEventHandler(RemoveFocusVisualStyle), true);
        }
        private static void RemoveFocusVisualStyle(object sender, RoutedEventArgs e)
        {
            (sender as FrameworkElement).FocusVisualStyle = null;
        }

        #endregion

        #region 让系统可以处理队列中的所有Windows消息
        /// <summary>
        /// 让系统可以处理队列中的所有Windows消息
        /// </summary>
        public static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }
        private static Object ExitFrame(Object state)
        {
            ((DispatcherFrame)state).Continue = false;
            return null;
        }
        /// <summary>
        /// 刷新主题样式
        /// </summary>
        public static void DoStyles()
        {
            var list = new List<ResourceDictionary>();
            foreach (var item in Application.Current.Resources.MergedDictionaries) list.Add(item);

            Application.Current.Resources.MergedDictionaries.Clear();
            foreach (var item in list) Application.Current.Resources.MergedDictionaries.Add(item);
        }

        #endregion

        #region Window弹框
        /// <summary>
        /// Window模式弹框（带灰层背景）
        /// </summary>
        public static bool? ShowWindow(DependencyObject parent, Window window, int alpha = 100, bool iFocus = true, bool iEscExit = true)
        {
            if (!Parent(parent, out Window owner)) return null;
            //蒙板
            var layer = new Grid() { Background = Colors.Black.ToAlpha(alpha).ToBrush() };
            //使用装饰器装载
            var desktopAdorner = CustomAdorner(owner, layer);

            window.ShowInTaskbar = false;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Closed += delegate
            {
                //移除装饰器
                ClearAdorner(owner, desktopAdorner);
            };
            if (iFocus) window.LostKeyboardFocus += delegate
            {
                if (!window.IsKeyboardFocusWithin) PMethod.BeginInvoke(() => window.Focus());
            };
            if (!(window is WindowEXT))
            {
                //window.Loaded += delegate { window.Activate(); };
            }
            //弹出消息框 
            window.Owner = owner;
            if (iEscExit)
            {
                var iExit = false;
                var exitTime = DateTime.MinValue;
                window.KeyDown += delegate (object sender, KeyEventArgs e)
                {
                    if (e.Key == Key.Escape)
                    {
                        if (iExit && DateTime.Now.Subtract(exitTime).TotalMilliseconds < PConfig.DoubleInterval)
                        {
                            if (window.DataContext is IWindowModel windowModel)
                            {
                                window.DialogResult = windowModel.OnCancel(window);
                            }
                            else
                            {
                                window.Close();
                            }
                            return;
                        }
                        iExit = true;
                        exitTime = DateTime.Now;
                    }
                };
            }
            return window.ShowDialog();
        }
        /// <summary>
        /// 再次加载空窗体，达到释放资源的目的？
        /// CleanWindow
        /// </summary>
        public static void CleanWindow(Window window)
        {
            window.ShowInTaskbar = false;
            window.WindowState = WindowState.Normal;
            window.Left = SystemParameters.PrimaryScreenWidth;
            window.Show();
            window.Close();
        }
        /// <summary>
        /// Window全屏模式弹框
        /// </summary>
        public static void FullscreenWindow(UIElement element)
        {
            var fullScreenWindow = new Window
            {
                WindowStyle = System.Windows.WindowStyle.None,
                WindowState = WindowState.Maximized,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false,
            };
            if (PMethod.Find(element, out Window window)) fullScreenWindow.Owner = window;
            var parent = VisualTreeHelper.GetParent(element);
            ContentPresenter content = null;
            Panel panel = null;
            if (parent is ContentPresenter content2)
            {
                content = content2;
                content.Content = null;
            }
            else if (parent is Panel panel2)
            {
                panel = panel2;
                panel.Children.Remove(element);
            }
            fullScreenWindow.Content = element;
            fullScreenWindow.Closed += (sender, e) =>
            {
                fullScreenWindow.Content = null;
                if (content != null) content.Content = element;
                else panel?.Children.Add(element);
                parent = null;
            };
            fullScreenWindow.ShowDialog();
        }
        #endregion

        #region Window系统消息框
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和Warning图标。</para>
        /// </summary>
        public static void ShowWarning(DependencyObject parent, string msg)
        {
            Show(parent, msg, LevelType.Warn);
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和Error图标。</para>
        /// </summary>
        public static void ShowError(DependencyObject parent, string msg)
        {
            Show(parent, msg, LevelType.Error);
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和指定图标(默认Information)。</para>
        /// </summary>
        public static void Show(DependencyObject parent, string msg, LevelType level = LevelType.Info)
        {
            if (!Parent(parent, out Window window)) return;
            BeginInvoke(obj =>
            {
                switch (level)
                {
                    default:
                    case LevelType.Info:
                        MessageBox.Show(window, obj, window.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case LevelType.Warn:
                        MessageBox.Show(window, obj, window.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                    case LevelType.Error:
                    case LevelType.Fatal:
                        MessageBox.Show(window, obj, window.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }, msg);
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OKCancel按钮和Question图标;它也会返回一个结果。</para>
        /// </summary>
        public static bool Ask(DependencyObject parent, string msg)
        {
            if (Parent(parent, out Window window))
            {
                return MessageBox.Show(window, msg, window.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK;
            }
            return false;
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 是否按钮和Question图标;它也会返回一个结果。</para>
        /// </summary>
        public static bool AskYN(DependencyObject parent, string msg)
        {
            if (Parent(parent, out Window window))
            {
                return MessageBox.Show(window, msg, window.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
            }
            return false;
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 是否与取消按钮和Question图标;它也会返回一个结果。</para>
        /// <para>选择Yes，返回true</para>
        /// <para>选择No，返回false</para>
        /// <para>选择Cancel，返回null</para>
        /// </summary>
        public static bool? AskYNC(DependencyObject parent, string msg)
        {
            if (Parent(parent, out Window window))
            {
                var result = MessageBox.Show(window, msg, window.Title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes: return true;
                    case MessageBoxResult.No: return false;
                    default:
                    case MessageBoxResult.Cancel: return null;
                }
            }
            return null;
        }
        /// <summary>
        /// 返回控件的顶层Window
        /// </summary>
        public static Window Window(DependencyObject obj)
        {
            if (Parent(obj, out Window window)) return window;
            return null;
        }

        #endregion

        #region 返回指定控件的上下层控件
        /// <summary>
        /// 返回控件树中指定类型控件
        /// </summary>
        public static bool Find<T>(object obj, out T parent, string name = null) where T : FrameworkElement
        {
            parent = null;
            if (!(obj is DependencyObject dependency)) return false;
            if (Child(obj, out parent, name))
            {
                return true;
            }
            var hasParent = false;
            while (dependency != null)
            {
                if (dependency is T t)
                {
                    if (name == null || t.Name == name)
                    {
                        parent = t;
                        return true;
                    }
                }
                var temp = VisualTreeHelper.GetParent(dependency);
                if (temp == null) break;
                dependency = temp;
                hasParent = true;
            }
            if (hasParent && Child(dependency, out parent, name))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 返回控件的顶层指定类型控件
        /// </summary>
        public static bool Parent<T>(object obj, out T parent, string name = null) where T : FrameworkElement
        {
            if (obj is T t1)
            {
                if (name == null || t1.Name == name)
                {
                    parent = t1;
                    return true;
                }
            }
            parent = null;
            if (!(obj is DependencyObject dependency)) return false;
            dependency = VisualTreeHelper.GetParent(dependency);
            while (dependency != null)
            {
                if (dependency is T t)
                {
                    if (name == null || t.Name == name)
                    {
                        parent = t;
                        return true;
                    }
                }
                dependency = VisualTreeHelper.GetParent(dependency);
            }
            return false;
        }
        /// <summary>
        /// 查找指定类型子(同级)控件
        /// </summary>
        /// <typeparam name="T">查找控件类型</typeparam>
        /// <param name="obj">控件</param>
        /// <param name="child">返回指定类型控件</param>
        /// <param name="name">指定控件名称</param>
        /// <param name="iParent">指定搜索同级控件</param>
        /// <param name="func">外部条件，在多子项时判断</param>
        /// <returns></returns>
        public static bool Child<T>(object obj, out T child, string name = null, bool iParent = true, Func<T, bool> func = null) where T : FrameworkElement
        {
            child = null;
            if (!(obj is DependencyObject dependency)) return false;
            if (iParent)
            {
                var parent = VisualTreeHelper.GetParent(dependency);
                if (parent != null) dependency = parent;
            }
            var count = VisualTreeHelper.GetChildrenCount(dependency);
            for (int i = count - 1; i >= 0; i--)
            {
                var value = VisualTreeHelper.GetChild(dependency, i);
                if (value is T temp)
                {
                    if ((name == null || temp.Name == name) && func?.Invoke(temp) != false)
                    {
                        child = temp;
                        return true;
                    }
                }
                if (Child(value, out child, name, false, func))
                {
                    return true;
                }
            }
            while (dependency is ContentControl control)
            {
                dependency = control.Content as DependencyObject;
                if (dependency is T temp)
                {
                    if ((name == null || temp.Name == name) && func?.Invoke(temp) != false)
                    {
                        child = temp;
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取所有子控件中的验证错误列表
        /// </summary>
        public static List<string> ValidationError(DependencyObject dependency)
        {
            var errorList = new List<string>();
            ValidationError(dependency, errorList);
            return errorList;
        }
        private static void ValidationError(DependencyObject dependency, List<string> errorList)
        {
            var count = VisualTreeHelper.GetChildrenCount(dependency);
            for (var i = 0; i < count; i++)
            {
                var value = VisualTreeHelper.GetChild(dependency, i);
                if (Validation.GetHasError(value))
                {
                    foreach (ValidationError error in Validation.GetErrors(value))
                    {
                        errorList.Add(error.ErrorContent.ToString());
                    }
                }
                ValidationError(value, errorList);
            }
        }
        /// <summary>
        /// 验证模型中的指定名称控件值输入错误
        /// <para>输入控件限定为TextBoxEXT，控件名称为tb+name</para>
        /// </summary>
        public static bool ValidationError<T>(DependencyObject parent, T mode, string name, bool allEmpty = false) where T : class
        {
            return ValidationError(parent, mode, name, mode.Property(name).Description(), allEmpty);
        }
        /// <summary>
        /// 验证模型中的指定名称控件值输入错误
        /// <para>输入控件限定为TextBoxEXT，控件名称为tb+name</para>
        /// </summary>
        public static bool ValidationError<T>(DependencyObject parent, T mode, string name, string desc, bool allEmpty = false) where T : class
        {
            Control control = null;
            if (Find(parent, out TextBoxEXT tbName, "tb" + name) && tbName.Visibility == Visibility.Visible)
            {
                control = tbName;
            }
            else if (Find(parent, out PasswordBox tbPad, "tb" + name) && tbPad.Visibility == Visibility.Visible)
            {
                control = tbPad;
            }
            if (control != null)
            {
                if (!allEmpty && mode.GetValue(name).ToStrings().IsEmpty())
                {
                    Hit(parent, $"{PConfig.LanguageBase.PleaseInput}{desc}", ColorType.Warn);
                    control.Focus();
                    return false;
                }
                if (Validation.GetHasError(control))
                {
                    Hit(parent, Validation.GetErrors(control).First().ErrorContent, ColorType.Error);
                    control.Focus();
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 获取控件的XAML代码
        /// <summary>
        /// 获取控件模板的XAML代码
        /// </summary>
        public static string GetTemplateXaml(Control ctrl)
        {
            string xaml;
            if (ctrl.Template != null)
            {
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = new string(' ', 4),
                    NewLineOnAttributes = true
                };
                var strbuild = new StringBuilder();
                var xmlwrite = XmlWriter.Create(strbuild, settings);
                try
                {
                    XamlWriter.Save(ctrl.Template, xmlwrite);
                    xaml = strbuild.ToString();
                }
                catch (Exception ex)
                {
                    xaml = ex.Message;
                }
            }
            else
            {
                xaml = "no template";
            }
            return xaml;
        }
        /// <summary>
        /// 获取组件界面XAML代码
        /// <para>从相对路径URI</para>
        /// </summary>
        public static string GetComponentXmal(string uriStr)
        {
            var uri = new Uri(uriStr, UriKind.Relative);
            var obj = Application.LoadComponent(uri);
            var xaml = XamlWriter.Save(obj);
            return xaml;
        }

        #endregion

        #region 统一Invoke处理
        /// <summary>
        /// 同步调用
        /// <para>任何与 Application 不在同一个线程的代码，都可能遭遇 Application.Current 为 null。如Shutdown关闭</para>
        /// </summary>
        public static void Invoke(Action action, Action<Exception> error = null)
        {
            try
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    action.Invoke();
                });
            }
            catch (Exception ex)
            {
                if (error == null) ex.Log();
                else error.Invoke(ex);
            }
        }
        /// <summary>
        /// 带参数同步调用
        /// </summary>
        public static void Invoke<T>(Action<T> action, T t, Action<Exception> error = null)
        {
            try
            {
                Application.Current?.Dispatcher.Invoke(new Action<T>(arg =>
                {
                    action.Invoke(arg);
                }), t);
            }
            catch (Exception ex)
            {
                if (error == null) ex.Log();
                else error.Invoke(ex);
            }
        }
        /// <summary>
        /// 同步调用，并返回结果
        /// </summary>
        public static T Invoke<T>(Func<T> action, Action<Exception> error = null)
        {
            try
            {
                return Application.Current == null ? default : Application.Current.Dispatcher.Invoke(() =>
                {
                    return action.Invoke();
                });
            }
            catch (Exception ex)
            {
                if (error == null) ex.Log();
                else error.Invoke(ex);
                return default;
            }
        }
        /// <summary>
        /// 带参数同步调用，并返回结果
        /// </summary>
        public static O Invoke<T, O>(Func<T, O> action, T t, Action<Exception> error = null)
        {
            try
            {
                return Application.Current == null ? default : (O)Application.Current.Dispatcher.Invoke(new Func<T, O>(arg =>
                {
                    return action.Invoke(arg);
                }), t);
            }
            catch (Exception ex)
            {
                if (error == null) ex.Log();
                else error.Invoke(ex);
                return default;
            }
        }
        /// <summary>
        /// 异步调用
        /// </summary>
        public static void BeginInvoke(Action action, Action<Exception> error = null)
        {
            try
            {
                Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch (Exception ex)
                    {
                        if (error == null) ex.Log();
                        else error.Invoke(ex);
                    }
                }));
            }
            catch (Exception ex)
            {
                if (error == null) ex.Log();
                else error.Invoke(ex);
            }
        }
        /// <summary>
        /// 带参数异步调用
        /// </summary>
        public static void BeginInvoke<T>(Action<T> action, T t, Action<Exception> error = null)
        {
            try
            {
                Application.Current?.Dispatcher.BeginInvoke(new Action<T>(arg =>
                {
                    try
                    {
                        action.Invoke(arg);
                    }
                    catch (Exception ex)
                    {
                        if (error == null) ex.Log();
                        else error.Invoke(ex);
                    }
                }), t);
            }
            catch (Exception ex)
            {
                if (error == null) ex.Log();
                else error.Invoke(ex);
            }
        }

        #endregion

        #region Init
        /// <summary>
        /// 初始化App
        /// <para>existTitle：不为空时，检查实例，实例存在时激活指定标题的窗体，未找到实例时 关闭当前进程名称的其它所有进程</para>
        /// <para>logFirstChanceException：记录异常，默认记录</para>
        /// </summary>
        public new static bool InitApp(string logFile = "Log.xml", string existTitle = null, object key = null, bool logFirstChanceException = true)
        {
            if (!InitAppInstance(existTitle, key, () => Application.Current.Shutdown())) return false;
            InitAppError(logFile, logFirstChanceException);
            //禁用Backspace退格导航返回Page页
            NavigationCommands.BrowseBack.InputGestures.Clear();
            Application.Current.DispatcherUnhandledException += App_DispatcherUnhandledException;
            Task.Run(() =>
            {
                try
                {
                    for (var i = 0; i < 1000; i++)
                    {
                        if (PMethod.Invoke(() => Application.Current.MainWindow) != null) break;
                        Thread.Sleep(5);
                    }
                    PMethod.Invoke(() =>
                    {
                        PConfig.Window = Application.Current.MainWindow;
                        if (PConfig.Window == null) "未获取到MainWindow".Warn();
                    });
                }
                catch (Exception ex)
                {
                    $"获取MainWindow失败：{ex.Message()}".Warn();
                }
            });
            return true;
        }
        private static void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                var desc = $"未经处理的UI异常";
                e.Exception.Log(desc);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                var desc = $"不可恢复的未经处理UI异常";
                ex.Log(desc);
            }
        }

        #endregion
    }
}
