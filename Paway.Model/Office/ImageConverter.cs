using Paway.Helper;
using Spire.Doc;
using Spire.Pdf;
using Spire.Pdf.Print;
using Spire.Presentation;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paway.Model
{
    /// <summary>
    /// Word、Excel、PPT、PDF文件转换为图片
    /// </summary>
    public class ImageConverter
    {
        public event Action<int, int> ProgressChanged;
        static ImageConverter()
        {
            SpireLicence.Init();
        }

        /// <summary>
        /// Word、Excel、PPT、PDF文件转换为图片
        /// <para>参数1：文件</para>
        /// <para>参数2：图片生成路径</para>
        /// <para>参数2-flase(0)(默认):本程序下Images文件夹下，创建文件同名目录，后辍中的.以_代替</para>
        /// <para>参数2-true(1):文件同目录下，创建文件同名目录，后辍中的.以_代替</para>
        /// <para>参数2-其它:指定目录</para>
        /// </summary>
        public string ToImage(string file, string toPath = null)
        {
            try
            {
                var filePath = Path.GetFileNameWithoutExtension(file) + Path.GetExtension(file).Replace(".", "_");
                if (toPath == null)
                {
                    toPath = Path.Combine(Path.GetDirectoryName(file), filePath);
                }
                if (Directory.Exists(toPath)) Directory.Delete(toPath, true);
                Directory.CreateDirectory(toPath);
                var type = Path.GetExtension(file);
                switch (type.ToLower())
                {
                    case ".doc":
                    case ".docx": WordToImage(file, toPath); break;
                    case ".xls":
                    case ".xlsx": ExcelToImage(file, toPath); break;
                    case ".ppt":
                    case ".pptx": PPTToImage(file, toPath); break;
                    case ".pdf": PDFToImage(file, toPath); break;
                    default: return $"不支持的格式文件: {type}";
                }
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message();
            }
        }
        public void WordToImage(string file, string toPath)
        {
            var doc = new Document();
            doc.LoadFromFile(file);
            for (var i = 0; i < doc.PageCount; i++)
            {
                ProgressChanged?.Invoke(i, doc.PageCount);
                doc.SaveToImages(i, Spire.Doc.Documents.ImageType.Bitmap).Save(Path.Combine(toPath, $"{i}.jpg"), ImageFormat.Jpeg);
            }
            doc.Dispose();
        }
        public void ExcelToImage(string file, string toPath)
        {
            var excel = new Workbook();
            excel.LoadFromFile(file);
            for (var i = 0; i < excel.Worksheets.Count && i < 1; i++)
            {
                var pdfFile = Path.Combine(toPath, $"{i}.pdf");
                excel.Worksheets[i].SaveToPdf(pdfFile);
                {
                    var pdf = new PdfDocument();
                    pdf.LoadFromFile(pdfFile);
                    for (var j = 0; j < pdf.Pages.Count; j++)
                    {
                        ProgressChanged?.Invoke(j, pdf.Pages.Count);
                        pdf.SaveAsImage(j).Save(Path.Combine(toPath, $"{i}_{j}.jpg"), ImageFormat.Jpeg);
                    }
                    pdf.Dispose();
                }
                File.Delete(pdfFile);
            }
            excel.Dispose();
        }
        public void PPTToImage(string file, string toPath)
        {
            var ppt = new Presentation();
            ppt.LoadFromFile(file);
            for (var i = 0; i < ppt.Slides.Count; i++)
            {
                ProgressChanged?.Invoke(i, ppt.Slides.Count);
                ppt.Slides[i].SaveAsImage().Save(Path.Combine(toPath, $"{i}.jpg"), ImageFormat.Jpeg);
            }
            ppt.Dispose();
        }
        public void PDFToImage(string file, string toPath)
        {
            var pdf = new PdfDocument();
            pdf.LoadFromFile(file);
            for (var i = 0; i < pdf.Pages.Count; i++)
            {
                ProgressChanged?.Invoke(i, pdf.Pages.Count);
                pdf.SaveAsImage(i).Save(Path.Combine(toPath, $"{i}.jpg"), ImageFormat.Jpeg);
            }
            pdf.Dispose();
        }
    }
}
