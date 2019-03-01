using System;
using System.IO;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Spire.Doc;
using Spire.Doc.Documents;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class SpireDocHelper
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        public SpireDocHelper(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="type"></param>
        /// <param name="memi"></param>
        /// <returns></returns>
        public OpResult<Stream> SwaggerHtmlConvers(string html, string type, out string memi)
        {
            string fileName = Guid.NewGuid().ToString() + type;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string path = webRootPath + @"\Files\TempFiles\";
            var addrUrl = path + $"{fileName}";
            FileStream fileStream = null;
            var provider = new FileExtensionContentTypeProvider();
            memi = provider.Mappings[type];
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var data = System.Text.Encoding.Default.GetBytes(html);
                var stream = ByteHelper.BytesToStream(data);
                //创建Document实例
                Document document = new Document();
                //加载HTML文档
                //document.LoadFromFile("APIDocument.html", FileFormat.Html, XHTMLValidationType.None);
                document.LoadFromStream(stream, FileFormat.Html, XHTMLValidationType.None);
                //document.LoadText(stream, Encoding.Default);
                //保存为Word
                switch (type)
                {
                    case ".docx":
                        //Word
                        document.SaveToFile(addrUrl, FileFormat.Docx);
                        break;
                    case ".pdf":
                        //PDF
                        document.SaveToFile(addrUrl, FileFormat.PDF);
                        break;
                    case ".html":
                        //Html
                        FileStream fs = new FileStream(addrUrl, FileMode.Append, FileAccess.Write, FileShare.None);//html直接写入不用spire.doc
                        StreamWriter sw = new StreamWriter(fs); // 创建写入流
                        sw.WriteLine(html); // 写入Hello World
                        sw.Close(); //关闭文件
                        fs.Close();
                        break;
                    case ".xml":
                        //PDF
                        document.SaveToFile(addrUrl, FileFormat.WordXml);
                        break;
                    case ".svg":
                        //PDF
                        document.SaveToFile(addrUrl, FileFormat.SVG);
                        break;
                }

                document.Close();
                fileStream = File.Open(addrUrl, FileMode.OpenOrCreate);
                var filedata = ByteHelper.StreamToBytes(fileStream);
                var outdata = ByteHelper.BytesToStream(filedata);
                return new OpResult<Stream>(OpResultType.Success, "转换成功！", outdata);
            }
            catch (Exception e)
            {
                return new OpResult<Stream>(OpResultType.Error, $"转换失败，{e.Message}", null);
            }
            finally
            {
                fileStream?.Close();
                if (System.IO.File.Exists(addrUrl))
                    System.IO.File.Delete(addrUrl);//删掉文件
            }
        }
    }
}
