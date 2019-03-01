using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class NPOIHelper
    {
        /// <summary>
        /// 获取类型转成字符串
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static string getCellValueByCell(ICell cell)
        {
            //判断是否为null或空串
            if (cell == null || cell.ToString().Trim().Equals(""))
            {
                return "";
            }
            String cellValue = "";
            CellType cellType = cell.CellType;
            switch (cellType)
            {
                case CellType.String: //字符串类型 StringCellValue
                    cellValue = cell.StringCellValue.Trim();
                    cellValue = string.Empty == cellValue ? null : cellValue;
                    break;
                case CellType.Boolean:  //布尔类型
                    cellValue = cell.BooleanCellValue.ToString();
                    break;
                case CellType.Numeric: //数值类型
                    if (HSSFDateUtil.IsCellDateFormatted(cell))
                    {  //判断日期类型
                        //cellValue = DateUtil.FormatDateByFormat(cell.DateCellValue, "yyyy-MM-dd");
                        cellValue = cell.DateCellValue.ToString("yyyy-MM-dd");
                    }
                    else
                    {  //否
                        cellValue = new DecimalFormat("#.######").Format(cell.NumericCellValue);
                    }
                    break;
                default: //其它类型，取空串吧
                    cellValue = "";
                    break;
            }
            return cellValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static string SetExcelTime(ICell cell)
        {
            string unit = "";
            if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
            {
                unit = cell.DateCellValue.ToString();
            }
            else
            {
                unit = cell.ToString();
            }
            //string time2 = DateTime.Parse(time).ToString("yyyy-MM-dd 00:00:00.000");
            //return time2;
            return unit;
        }
    }
}
