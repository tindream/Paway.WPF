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
using System.Windows;

namespace Paway.Model
{
    public partial class Method : PMethod
    {
        #region File
        public static string ReadFile(string file, out int length)
        {
            var buffer = ReadFileBuffer(file, out length);
            var str = Convert.ToBase64String(buffer);
            return str;
        }
        public static byte[] ReadFileBuffer(string file, out int length)
        {
            using (var fs = File.OpenRead(file))
            {
                var buffer = new byte[fs.Length];
                length = fs.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
        public static void SaveFile(string file, string str)
        {
            var buffer = Convert.FromBase64String(str);
            SaveFile(file, buffer);
        }
        public static void SaveFile(string file, byte[] buffer)
        {
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(buffer, 0, buffer.Length);
            }
        }

        #endregion

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
        public static bool Export(string fileName, out string outFile, string filter = "Excel 工作簿|*.xlsx|Excel 97-2003 工作簿|*.xls")
        {
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

        #region 同步
        /// <summary>
        /// 同步更新
        /// </summary>
        public static void Update<T>(T info) where T : class, IId
        {
            Update(new List<T> { info });
        }
        /// <summary>
        /// 同步更新
        /// </summary>
        public static void Update<T>(List<T> fList) where T : class, IId
        {
            var tList = Cache.List<T>();
            Method.Update(OperType.Update, tList, fList);
        }
        /// <summary>
        /// 同步删除项
        /// </summary>
        public static void Delete<T>(T info) where T : class, IId
        {
            var tList = Cache.List<T>();
            Method.Update(OperType.Delete, tList, info);
        }
        /// <summary>
        /// 同步删除项
        /// </summary>
        public static void Delete<T>(List<T> fList) where T : class, IId
        {
            var tList = Cache.List<T>();
            Method.Update(OperType.Delete, tList, fList);
        }

        #endregion

        #region 其它一些方法
        /// <summary>
        /// 椭圆求指定角度坐标点公式
        /// </summary>
        /// <param name="lpRect">椭圆边框</param>
        /// <param name="angle">角度</param>
        /// <returns></returns>
        public static Point GetArcPoint(System.Drawing.Rectangle lpRect, float angle)
        {
            Point pt = new Point();
            double a = lpRect.Width / 2.0f;
            double b = lpRect.Height / 2.0f;
            if (a == 0 || b == 0) return new Point(lpRect.X, lpRect.Y);

            //弧度
            double radian = angle * Math.PI / 180.0f;

            //获取弧度正弦值
            double yc = Math.Sin(radian);
            //获取弧度余弦值
            double xc = Math.Cos(radian);
            //获取曲率  r = ab/\Sqrt((a.Sinθ)^2+(b.Cosθ)^2
            double radio = (a * b) / Math.Sqrt(Math.Pow(yc * a, 2.0) + Math.Pow(xc * b, 2.0));

            //计算坐标
            double ax = radio * xc;
            double ay = radio * yc;
            pt.X = (int)(lpRect.X + a + ax);
            pt.Y = (int)(lpRect.Y + b + ay);
            return pt;
        }

        #endregion
    }
}
