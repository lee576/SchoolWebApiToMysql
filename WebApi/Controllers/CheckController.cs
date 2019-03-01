using DbModel;
using Infrastructure;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Models.ViewModels;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SchoolWebApi.Utility;
using Service;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 数据校验
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class CheckController : Controller
    {
        private static string tempPath = @"C:\Temp\";
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private Itb_payment_ARService _tb_payment_ARService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_payment_ARServic"></param>
        public CheckController(Itb_payment_ARService tb_payment_ARServic)
        {
            _tb_payment_ARService = tb_payment_ARServic;
        }
        private string GetSchoolCode(string schoolCode)
        {
            StringValues headerValues;
            var headers = HttpContext.Request.Headers;
            if (headers.TryGetValue("schoolcode", out headerValues))
            {
                schoolCode = headerValues.First();
            }
            return schoolCode;
        }
        private string SetExcelTime(ICell cell)
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
     
        /// <summary>
        /// 查询批号和身份证好是否有重复数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpFileARIDandPassportIsRepeat()
        {
            var schoolCode = string.Empty;
            schoolCode = GetSchoolCode(schoolCode);
            var files = HttpContext.Request.Form.Files;
            var filePath = string.Empty;
            try
            {
                if (files.Count > 0)
                {
                    var file = files[0];
                    //给文件一个随机的名字
                    var fileNewName = Guid.NewGuid().ToString();
                    string fileExt = file.FileName.Split('.')[1];
                    if (!(fileExt.ToUpper()).Equals("XLSX"))
                    {
                        return Json(new
                        {
                            code = JsonReturnMsg.FailCode,
                            msg = JsonReturnMsg.GetFail,
                            data = "请上传xlsx文件"
                        });
                    }
                    FileHelper.CreateFiles(tempPath, true);
                    filePath = tempPath + $@"{fileNewName}.{fileExt}";
                    //保存文件到临时文件夹下
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    using (FileStream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        XSSFWorkbook workbook = new XSSFWorkbook(stream);
                        var sheetNum = workbook.NumberOfSheets;
                        if (sheetNum > 0 && !string.IsNullOrEmpty(schoolCode))
                        {
                            ISheet sheet = workbook.GetSheetAt(0);
                            //获取sheet的首行
                            IRow headerRow = sheet.GetRow(0);
                            //获取sheet的最后一列
                            int cellCount = headerRow.LastCellNum;
                            //获取sheet的最后一行
                            int rowCount = sheet.LastRowNum;
                            //检测excel数据正确性
                            List<string> aridList = new List<string>();//批号集合方便查询是否重复批号
                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                            {
                                IRow row = sheet.GetRow(i);
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    if (!string.IsNullOrEmpty(row.GetCell(j) + ""))
                                    {
                                        if (j == 0)
                                        {
                                            aridList.Add(row.GetCell(j) + "");
                                        }
                                    }
                                }
                            }
                            //检测数据组批号和身份证是否有重复
                            //var query = aridList.GroupBy(x => x)
                            //.Where(g => g.Count() > 1)
                            //.ToDictionary(x => x.Key, y => y.Count());
                            //if (query.Count > 0)
                            //{
                            //    return Json(new
                            //    {
                            //        code = JsonReturnMsg.FailCode,
                            //        msg = JsonReturnMsg.GetFail,
                            //        data = "批号有重复"
                            //    });
                            //}
                            //准备数据库录入数据
                            if (cellCount != 7)
                            {
                                return Json(new
                                {
                                    code = JsonReturnMsg.FailCode,
                                    msg = JsonReturnMsg.GetFail,
                                    data = "请使用新模板!"
                                });
                            }
                            bool isBreak = false;
                            List<tb_payment_ar> list = new List<tb_payment_ar>();
                            LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(600);
                            TaskFactory factory = new TaskFactory(lcts);
                            List<Task> tasks = new List<Task>();
                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                            {
                                IRow row = sheet.GetRow(i);
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    if (string.IsNullOrEmpty(row.GetCell(j) + ""))
                                    {
                                        isBreak = true;
                                        break;
                                    }
                                }
                                if (isBreak)
                                    break;
                                
                                tb_payment_ar tbPayment = new tb_payment_ar
                                {
                                    schoolcode = schoolCode,
                                    ARID = row.GetCell(0).ToString(),
                                    name = row.GetCell(1).ToString(),
                                    amount = Convert.ToDecimal(row.GetCell(2).ToString()),
                                    AR_account = row.GetCell(3).ToString(),
                                    star_date = Convert.ToDateTime(SetExcelTime(row.GetCell(4))),
                                    //end_date = Convert.ToDateTime(SetExcelTime(row.GetCell(5))),
                                    st_name = row.GetCell(5).ToString(),
                                    passport = row.GetCell(6).ToString(),
                                    JSstatus = 0,
                                    status = 0,
                                    fact_amount = 0
                                };
                                tasks.Add(factory.StartNew(s =>
                                {
                                    _tb_payment_ARService.AddPayment_ARInfo(tbPayment);
                                    //开始推送消息
                                    MsgPushHelper msghelper = new MsgPushHelper();
                                }, i));
                                //list.Add(tbPayment);
                               
                            }
                            Task.WaitAll(tasks.ToArray());
                           
                        }
                        return Json(new
                        {
                            code = JsonReturnMsg.SuccessCode,
                            msg = JsonReturnMsg.GetSuccess,
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail,
                        data="文件上传失败"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail,
                    data = "数据存储失败，请查看表是否按模板规则!"
                });
            }
            finally
            {
                //清理临时上传的文件
                if (!string.IsNullOrEmpty(filePath) && FileHelper.IsExist(filePath, false))
                {
                    FileHelper.DeleteFiles(filePath, false);
                }
            }
        }
    }
}