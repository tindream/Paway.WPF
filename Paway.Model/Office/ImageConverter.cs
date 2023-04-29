using Paway.Helper;
using Spire.Doc;
using Spire.Pdf;
using Spire.Pdf.Print;
using Spire.Presentation;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
        /// <para>file：文件</para>
        /// <para>toPath:指定目录，为空时在文件同目录下，创建文件同名目录，文件中的.以_代替</para>
        /// </summary>
        public void ToImageFile(string file, string toPath = null)
        {
            if (toPath == null)
            {
                toPath = Path.Combine(Path.GetDirectoryName(file), Path.GetFileName(file).Replace(".", "_"));
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
                default: throw new WarningException($"不支持的格式文件: {type}");
            }
        }
        /// <summary>
        /// Word、Excel、PPT、PDF文件转换为图片
        /// <para>file：文件</para>
        /// <para>toPath:指定目录，为空时在文件同目录下，创建文件同名目录，文件中的.以_代替</para>
        /// </summary>
        public List<Image> ToImages(string file)
        {
            var imageList = new List<Image>();
            var type = Path.GetExtension(file);
            switch (type.ToLower())
            {
                case ".doc":
                case ".docx": WordToImage(file, imageList); break;
                case ".xls":
                case ".xlsx": ExcelToImage(file, imageList); break;
                case ".ppt":
                case ".pptx": PPTToImage(file, imageList); break;
                case ".pdf": PDFToImage(file, imageList); break;
                default: throw new WarningException($"不支持的格式文件: {type}");
            }
            return imageList;
        }

        public void WordToImage(string file, string toPath)
        {
            WordToImage(file, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                image.Save(Path.Combine(toPath, $"{index}.jpg"), ImageFormat.Jpeg);
            });
        }
        public void WordToImage(string file, List<Image> imageList)
        {
            WordToImage(file, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                imageList.Add(image);
            });
        }
        public void WordToImage(string file, Action<int, int, Image> action)
        {
            using (var doc = new Document())
            {
                doc.LoadFromFile(file);
                for (var i = 0; i < doc.PageCount; i++)
                {
                    action.Invoke(i, doc.PageCount, doc.SaveToImages(i, Spire.Doc.Documents.ImageType.Bitmap));
                }
            }
        }

        public void ExcelToImage(string file, string toPath)
        {
            using (var excel = new Workbook())
            {
                excel.LoadFromFile(file);
                for (var i = 0; i < excel.Worksheets.Count; i++)
                {
                    var pdfFile = Path.Combine(toPath, $"{i}.pdf");
                    excel.Worksheets[i].SaveToPdf(pdfFile);
                    PDFToImage(pdfFile, (index, total, image) =>
                    {
                        ProgressChanged?.Invoke(index + i * total, total * excel.Worksheets.Count);
                        image.Save(Path.Combine(toPath, $"{i}_{index}.jpg"), ImageFormat.Jpeg);
                    });
                    File.Delete(pdfFile);
                }
            }
        }
        public void ExcelToImage(string file, List<Image> imageList)
        {
            ExcelToImage(file, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                imageList.Add(image);
            });
        }
        public void ExcelToImage(string file, Action<int, int, Image> action)
        {
            using (var excel = new Workbook())
            {
                excel.LoadFromFile(file);
                var toPath = Path.Combine(Path.GetDirectoryName(file), Path.GetFileName(file).Replace(".", "_"));
                for (var i = 0; i < excel.Worksheets.Count; i++)
                {
                    var pdfFile = Path.Combine(toPath, $"{i}.pdf");
                    excel.Worksheets[i].SaveToPdf(pdfFile);
                    PDFToImage(pdfFile, (index, total, image) => action.Invoke(index + i * total, total * excel.Worksheets.Count, image));
                    File.Delete(pdfFile);
                }
            }
        }

        public void PPTToImage(string file, string toPath)
        {
            PPTToImage(file, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                image.Save(Path.Combine(toPath, $"{index}.jpg"), ImageFormat.Jpeg);
            });
        }
        public void PPTToImage(string file, List<Image> imageList)
        {
            PPTToImage(file, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                imageList.Add(image);
            });
        }
        public void PPTToImage(string file, Action<int, int, Image> action)
        {
            using (var ppt = new Presentation())
            {
                ppt.LoadFromFile(file);
                for (var i = 0; i < ppt.Slides.Count; i++)
                {
                    action.Invoke(i, ppt.Slides.Count, ppt.Slides[i].SaveAsImage());
                }
            }
        }

        public void PDFToImage(string file, string toPath)
        {
            PDFToImage(file, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                image.Save(Path.Combine(toPath, $"{index}.jpg"), ImageFormat.Jpeg);
            });
        }
        public void PDFToImage(string file, List<Image> imageList)
        {
            PDFToImage(file, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                imageList.Add(image);
            });
        }
        public void PDFToImage(string file, Action<int, int, Image> action)
        {
            using (var pdf = new PdfDocument())
            {
                pdf.LoadFromFile(file);
                for (var i = 0; i < pdf.Pages.Count; i++)
                {
                    action.Invoke(i, pdf.Pages.Count, pdf.SaveAsImage(i));
                }
            }
        }
    }
}
