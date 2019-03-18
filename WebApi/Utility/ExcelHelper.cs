﻿using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

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
        /// <param name="detail"></param>
        /// <returns></returns>
        public static IWorkbook CreateWorkbook<T>(List<string> lstTitle, T[] list, Action<IRow, T[], int> fillCell,string detail="")
        {
            IWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("Sheet1");
            IRow rowTitle;
            ICellStyle style = book.CreateCellStyle();
            style.VerticalAlignment = VerticalAlignment.Center; //垂直居中
            string[] sp = null;
            int rowcount = 0;
            if (string.IsNullOrWhiteSpace(detail))
            {
                rowTitle = sheet.CreateRow(0);
                rowcount += 1;
            }
            else
            {
                sp = detail.Split('#');
                rowcount = sp.Length;
                rowTitle = sheet.CreateRow(rowcount + 1);
                for (int i = 0; i < rowcount; i++)
                {
                    IRow row = sheet.CreateRow(i);
                    row.CreateCell(0).SetCellValue("#" + sp[i]);
                }
                rowcount += 2;
            }
            for (int i = 0; i < lstTitle.Count; i++)
            {
                rowTitle.CreateCell(i).SetCellValue(lstTitle[i]);
            }

            for (int i = 0; i < list.Length; i++)
            {
                IRow row = sheet.CreateRow(i + rowcount);
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
        /// <param name="detail"></param>
        /// <returns></returns>
        public static MemoryStream CreateWorkbookMemoryStream<T>(List<string> lstTitle, T[] list,
            Action<IRow, T[], int> fillCell,string detail="")
        {
            var book = CreateWorkbook(lstTitle, list, fillCell, detail);
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
        /// <param name="detail"></param>
        /// <returns></returns>
        public static Byte[] CreateWorkbookByte<T>(List<string> lstTitle, T[] list,
            Action<IRow, T[], int> fillCell,string detail="")
        {
            var stream = CreateWorkbookMemoryStream(lstTitle, list, fillCell, detail);
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
        /// <param name="detail"></param>
        /// <returns></returns>
        public static FileContentResult ExportAction<T>(string fileName, List<string> lstTitle, T[] list,
            Action<IRow, T[], int> fillCell,string detail = "")
        {
            //导出数据excel
            var bytes = CreateWorkbookByte(lstTitle, list, fillCell, detail);
            FileContentResult fileContentResult =
                new FileContentResult(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = fileName
                };
            return fileContentResult;
        }
    }
}
