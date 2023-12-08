using log4net;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Paway.Helper;
using Paway.Utils;
using Paway.WPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Paway.Model
{
    public partial class Method : PMethod
    {
        #region 导入导出
        /// <summary>
        /// 选择单个文件导入
        /// </summary>
        public static bool Import(string title, out string file, string filter = "Excel 工作簿|*.xls;*.xlsx")
        {
            file = null;
            var ofd = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
            };
            if (ofd.ShowDialog() == true)
            {
                file = ofd.FileName;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 选择多个文件导入
        /// </summary>
        public static bool Imports(string title, out string[] file, string filter = "Excel 工作簿|*.xls;*.xlsx")
        {
            file = null;
            var ofd = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
                Multiselect = true,
            };
            if (ofd.ShowDialog() == true)
            {
                file = ofd.FileNames;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 导入后更新列表
        /// </summary>
        public static List<T> Import<T>(List<T> tList, List<T> fList) where T : class, IId, ICompare<T>
        {
            var updateList = new List<T>();
            foreach (var item in fList)
            {
                var normal = tList.Find(c => c.Compare(item));
                if (normal != null)
                {
                    var id = normal.Id;
                    item.Clone(normal);
                    normal.Id = id;
                    updateList.Add(normal);
                }
                else
                {
                    updateList.Add(item);
                }
            }
            return updateList;
        }
        /// <summary>
        /// 导出到文件
        /// </summary>
        public static bool Export(string fileName, out string outFile, string filter = null)
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
                    default: filter = "Excel 工作簿|*.xlsx|Excel 97-2003 工作簿|*.xls"; break;
                }
            }
            outFile = null;
            var sfd = new SaveFileDialog()
            {
                Title = $"选择要导出的文件位置",
                Filter = filter,
                FileName = fileName,
            };
            if (sfd.ShowDialog() == true)
            {
                outFile = sfd.FileName;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 导出到目录
        /// </summary>
        public static bool ExportPath(out string outPath, string selectedPath = null)
        {
            outPath = null;
            var fbd = new System.Windows.Forms.FolderBrowserDialog()
            {
                Description = $"选择要导出的文件位置",
                RootFolder = Environment.SpecialFolder.Desktop,
            };
            if (selectedPath != null) fbd.SelectedPath = selectedPath;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                outPath = fbd.SelectedPath;
                return true;
            }
            return false;
        }
        public static Task<List<T>> FromExcel<T>(string fileName, int start = 0, int end = 0) where T : class, new()
        {
            var dt = ExcelHelper.ToDataTable(fileName);
            var list = dt.ToList<T>();
            return Task.FromResult(list.Skip(start).Take(list.Count - start - end).ToList());
        }

        #endregion
    }
}
