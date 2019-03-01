using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// Excel 帮助类
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 创建WorkBook
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lstTitle"></param>
        /// <param name="list"></param>
        /// <param name="fillCell"></param>
        /// <returns></returns>
        public static IWorkbook CreateWorkbook<T>(List<string> lstTitle, T[] list, Action<IRow, T[], int> fillCell)
        {
            IWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("Sheet1");
            IRow rowTitle = sheet.CreateRow(0);
            ICellStyle style = book.CreateCellStyle();
            style.VerticalAlignment = VerticalAlignment.Center; //垂直居中  
            for (int i = 0; i < lstTitle.Count; i++)
            {
                rowTitle.CreateCell(i).SetCellValue(lstTitle[i]);
            }

            for (int i = 0; i < list.Length; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                fillCell(row, list, i);
            }

            for (int i = 0; i < lstTitle.Count; i++)
            {
                sheet.AutoSizeColumn(i); //i：根据标题的个数设置自动列宽
            }

            return book;
        }

        /// <summary>
        /// 创建WorkBook的MemoryStream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lstTitle"></param>
        /// <param name="list"></param>
        /// <param name="fillCell"></param>
        /// <returns></returns>
        public static MemoryStream CreateWorkbookMemoryStream<T>(List<string> lstTitle, T[] list,
            Action<IRow, T[], int> fillCell)
        {
            var book = CreateWorkbook(lstTitle, list, fillCell);
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        /// <summary>
        /// 创建WorkBook的字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lstTitle"></param>
        /// <param name="list"></param>
        /// <param name="fillCell"></param>
        /// <returns></returns>
        public static Byte[] CreateWorkbookByte<T>(List<string> lstTitle, T[] list,
            Action<IRow, T[], int> fillCell)
        {
            var stream = CreateWorkbookMemoryStream(lstTitle, list, fillCell);
            return StreamHelper.StreamToBytes(stream);
        }

        /// <summary>
        /// 返回Workbook的FileContentResult对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="lstTitle"></param>
        /// <param name="list"></param>
        /// <param name="fillCell"></param>
        /// <returns></returns>
        public static FileContentResult ExportAction<T>(string fileName, List<string> lstTitle, T[] list,
            Action<IRow, T[], int> fillCell)
        {
            //导出数据excel
            var bytes = CreateWorkbookByte(lstTitle, list, fillCell);
            FileContentResult fileContentResult =
                new FileContentResult(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = fileName
                };
            return fileContentResult;
        }
    }
}
