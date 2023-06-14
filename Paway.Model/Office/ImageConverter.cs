using Paway.Helper;
using Spire.Doc;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Print;
using Spire.Presentation;
using Spire.Xls;
using System;
using System.Collections;
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
        /// <para>PPT：不支持zoom缩放</para>
        /// </summary>
        public void ToImageFile(string file, string toPath = null, double zoom = 1)
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
                case ".docx": WordToImage(file, toPath, zoom); break;
                case ".xls":
                case ".xlsx": ExcelToImage(file, toPath, zoom); break;
                case ".ppt":
                case ".pptx": PPTToImage(file, toPath); break;
                case ".pdf": PDFToImage(file, toPath, zoom); break;
                default: throw new WarningException($"不支持的格式文件: {type}");
            }
        }
        /// <summary>
        /// Word、Excel、PPT、PDF文件转换为图片
        /// <para>file：文件</para>
        /// <para>toPath:指定目录，为空时在文件同目录下，创建文件同名目录，文件中的.以_代替</para>
        /// </summary>
        public List<Image> ToImages(string file, double zoom = 1)
        {
            var imageList = new List<Image>();
            var type = Path.GetExtension(file);
            switch (type.ToLower())
            {
                case ".doc":
                case ".docx": WordToImage(file, imageList, zoom); break;
                case ".xls":
                case ".xlsx": ExcelToImage(file, imageList, zoom); break;
                case ".ppt":
                case ".pptx": PPTToImage(file, imageList); break;
                case ".pdf": PDFToImage(file, imageList, zoom); break;
                default: throw new WarningException($"不支持的格式文件: {type}");
            }
            return imageList;
        }

        private void WordToImage(string file, string toPath, double zoom)
        {
            if (zoom == 1) WordToImage(file, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                image.SaveTo(Path.Combine(toPath, $"{index}.jpg"), ImageFormat.Jpeg);
            });
            else WordToImage(file, zoom, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                image.SaveTo(Path.Combine(toPath, $"{index}.jpg"), ImageFormat.Jpeg);
            });
        }
        private void WordToImage(string file, List<Image> imageList, double zoom)
        {
            if (zoom == 1) WordToImage(file, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                imageList.Add(image);
            });
            else WordToImage(file, zoom, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                imageList.Add(image);
            });
        }
        private void WordToImage(string file, Action<int, int, Image> action)
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
        private void WordToImage(string file, double zoom, Action<int, int, Image> action)
        {
            using (var doc = new Document())
            {
                doc.LoadFromFile(file);
                var toPdf = new ToPdfParameterList();
                toPdf.PdfConformanceLevel = PdfConformanceLevel.Pdf_A1B;
                var toPath = Path.Combine(Path.GetDirectoryName(file), Path.GetFileName(file).Replace(".", "_"));
                var pdfFile = Path.Combine(toPath, $"temp.pdf");
                doc.SaveToFile(pdfFile, toPdf);
                PDFToImage(pdfFile, zoom, action);
                File.Delete(pdfFile);
            }
        }

        private void ExcelToImage(string file, string toPath, double zoom)
        {
            using (var excel = new Workbook())
            {
                excel.LoadFromFile(file);
                for (var i = 0; i < excel.Worksheets.Count; i++)
                {
                    var pdfFile = Path.Combine(toPath, $"{i}.pdf");
                    excel.Worksheets[i].SaveToPdf(pdfFile);
                    PDFToImage(pdfFile, zoom, (index, total, image) =>
                    {
                        ProgressChanged?.Invoke(index + i * total, total * excel.Worksheets.Count);
                        image.SaveTo(Path.Combine(toPath, $"{i}_{index}.jpg"), ImageFormat.Jpeg);
                    });
                    File.Delete(pdfFile);
                }
            }
        }
        private void ExcelToImage(string file, List<Image> imageList, double zoom)
        {
            ExcelToImage(file, zoom, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                imageList.Add(image);
            });
        }
        private void ExcelToImage(string file, double zoom, Action<int, int, Image> action)
        {
            using (var excel = new Workbook())
            {
                excel.LoadFromFile(file);
                var toPath = Path.Combine(Path.GetDirectoryName(file), Path.GetFileName(file).Replace(".", "_"));
                for (var i = 0; i < excel.Worksheets.Count; i++)
                {
                    var pdfFile = Path.Combine(toPath, $"{i}.pdf");
                    excel.Worksheets[i].SaveToPdf(pdfFile);
                    PDFToImage(pdfFile, zoom, (index, total, image) => action.Invoke(index + i * total, total * excel.Worksheets.Count, image));
                    File.Delete(pdfFile);
                }
            }
        }

        private void PPTToImage(string file, string toPath)
        {
            PPTToImage(file, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                image.SaveTo(Path.Combine(toPath, $"{index}.jpg"), ImageFormat.Jpeg);
            });
        }
        private void PPTToImage(string file, List<Image> imageList)
        {
            PPTToImage(file, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                imageList.Add(image);
            });
        }
        private void PPTToImage(string file, Action<int, int, Image> action)
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

        private void PDFToImage(string file, string toPath, double zoom)
        {
            PDFToImage(file, zoom, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                image.SaveTo(Path.Combine(toPath, $"{index}.jpg"), ImageFormat.Jpeg);
            });
        }
        private void PDFToImage(string file, List<Image> imageList, double zoom)
        {
            PDFToImage(file, zoom, (index, total, image) =>
            {
                ProgressChanged?.Invoke(index, total);
                imageList.Add(image);
            });
        }
        private void PDFToImage(string file, double zoom, Action<int, int, Image> action)
        {
            using (var pdf = new PdfDocument())
            {
                pdf.LoadFromFile(file);
                for (var i = 0; i < pdf.Pages.Count; i++)
                {
                    action.Invoke(i, pdf.Pages.Count, pdf.SaveAsImage(i, (int)(96 * zoom), (int)(96 * zoom)));
                }
            }
        }

        /// <summary>
        /// 图像列表转PDF文件
        /// </summary>
        public void ImageToPdf(List<Image> fileList, string toFile)
        {
            ImageToPdf(fileList, toFile, index => PdfImage.FromImage(fileList[index]));
        }
        /// <summary>
        /// 图片文件列表转PDF文件
        /// </summary>
        public void ImageToPdf(List<string> fileList, string toFile)
        {
            ImageToPdf(fileList, toFile, index => PdfImage.FromFile(fileList[index]));
        }
        /// <summary>
        /// 图像或文件列表转PDF文件
        /// </summary>
        private void ImageToPdf(IList list, string toFile, Func<int, PdfImage> toImage)
        {
            // Create a pdf document with a section and page added.
            PdfDocument doc = new PdfDocument();
            for (var i = 0; i < list.Count; i++)
            {
                ProgressChanged?.Invoke(i, list.Count);
                PdfPageBase page = doc.Pages.Add();
                //Load a tiff image from system
                PdfImage image = toImage(i);
                //Set image display location and size in PDF
                float widthFitRate = image.PhysicalDimension.Width / page.Canvas.ClientSize.Width;
                float heightFitRate = image.PhysicalDimension.Height / page.Canvas.ClientSize.Height;
                float fitRate = Math.Max(widthFitRate, heightFitRate);
                float fitWidth = image.PhysicalDimension.Width / fitRate;
                float fitHeight = image.PhysicalDimension.Height / fitRate;
                page.Canvas.DrawImage(image, 0, 0, fitWidth, fitHeight);
            }
            //save and launch the file
            doc.SaveToFile(toFile);
            doc.Close();
        }
    }
}
