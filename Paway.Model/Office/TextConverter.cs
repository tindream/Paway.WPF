﻿using Paway.Helper;
using Spire.Pdf;
using Spire.Pdf.Texts;
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
    /// PDF文件转换为文本
    /// </summary>
    public class TextConverter
    {
        /// <summary>
        /// 转换进度事件
        /// </summary>
        public event Action<ProgressEventArgs> ProgressChanged;

        /// <summary>
        /// PDF文件转换为文本
        /// </summary>
        public List<string> PDFToText(string file)
        {
            var pdf = new PdfDocument();
            pdf.LoadFromFile(file);
            var option = new PdfTextExtractOptions
            {
                IsSimpleExtraction = true
            };
            var list = new List<string>();
            for (var i = 0; i < pdf.Pages.Count; i++)
            {
                ProgressChanged?.Invoke(new ProgressEventArgs(i, pdf.Pages.Count));
                var text = new PdfTextExtractor(pdf.Pages[i]).ExtractText(option);
                list.Add(text);
            }
            pdf.Dispose();
            return list;
        }
    }
}
