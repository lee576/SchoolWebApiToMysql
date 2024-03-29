﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBService;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using SchoolWebApi.HeadParams;
using SchoolWebApi.Utility;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 课程表控制层
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class SchoolTimetableController : Controller
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private MongodbHost mdb;
        private static string tempPath = @"C:\Temp\";
        private Itb_school_departmentService _tb_school_departmentService;
        private Itb_school_userService _tb_school_userService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_school_departmentService"></param>
        /// <param name="tb_school_userService"></param>
        public SchoolTimetableController(Itb_school_departmentService tb_school_departmentService,
            Itb_school_userService tb_school_userService)
        {
            mdb = new MongodbHost();
            mdb.Connection = ("mongodb://127.0.0.1:27017");
            mdb.DataBase = "ruohuoschool";
            mdb.Table = "SchoolTimetable";
            _tb_school_departmentService = tb_school_departmentService;
            _tb_school_userService = tb_school_userService;
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
        /// 添加课表
        /// </summary>
        /// <returns></returns>
        [HeaderParams(nameof(HeadParamsType.FileUpload))]
        [HttpPost]
        public IActionResult AddSchoolTimetable()
        {

            var schoolCode = string.Empty;
            schoolCode = GetSchoolCode(schoolCode);
            //schoolCode = "10064";
            var files = HttpContext.Request.Form.Files;
            var filePath = string.Empty;
            try
            {
                //删除原有数据库班级信息
                TMongodbHelper<tb_SchoolTimetable>.DeleteMany(mdb, Builders<tb_SchoolTimetable>.Filter.Eq("schoolcode", schoolCode));
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
                            List<tb_SchoolTimetable> schoolTimetList = new List<tb_SchoolTimetable>();//批号集合方便查询是否重复批号
                            tb_SchoolTimetable st = null;
                            string className = "";
                            for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum + 1; i++)
                            {

                                IRow row = sheet.GetRow(i);
                                if (row == null)
                                {
                                    //查找是否有重复数据
                                    var tb = GetSchoolTimetableInfo(st.class_id, st.schoolcode, (st.staTime).ToString("yyyy/MM/dd"));
                                    if (tb.Count > 0)
                                    {
                                        return Json(new
                                        {
                                            code = JsonReturnMsg.FailCode,
                                            msg = JsonReturnMsg.GetFail,
                                            data = "班级为[" + className + "]课表导入重复请排查"
                                        });
                                    }
                                    tb_SchoolTimetable joeClone = st.Clone();
                                    schoolTimetList.Add(joeClone);
                                    continue;
                                }



                                if (row.Cells.Count == 1 || (row.Cells.Count == 9 && NPOIHelper.getCellValueByCell(row.Cells[0]) != ""
                                    && NPOIHelper.getCellValueByCell(row.Cells[1]) == ""
                                    && NPOIHelper.getCellValueByCell(row.Cells[2]) == ""
                                    && NPOIHelper.getCellValueByCell(row.Cells[3]) == ""
                                    && NPOIHelper.getCellValueByCell(row.Cells[4]) == ""
                                    && NPOIHelper.getCellValueByCell(row.Cells[5]) == ""
                                    && NPOIHelper.getCellValueByCell(row.Cells[6]) == ""
                                    && NPOIHelper.getCellValueByCell(row.Cells[7]) == ""
                                   ))
                                {
                                    className = row.Cells[0].StringCellValue;
                                    var dep = _tb_school_departmentService.FindByClause(x => x.schoolcode == schoolCode && x.name == className);
                                    if (dep == null)
                                    {
                                        return Json(new
                                        {
                                            code = JsonReturnMsg.FailCode,
                                            msg = JsonReturnMsg.GetFail,
                                            data = "未找到名字为[" + className + "]的班级!"
                                        });
                                    }
                                    st = new tb_SchoolTimetable();
                                    st.class_id = dep.id.ToString();
                                    st.schoolcode = schoolCode;
                                    continue;
                                }
                                else if (NPOIHelper.getCellValueByCell(row.Cells[0]).Equals("日期"))
                                {
                                    //Convert.ToDateTime(SetExcelTime(row.GetCell(4)))
                                    DateTime staTime = Convert.ToDateTime(SetExcelTime(row.Cells[1]));
                                    DateTime endTime = Convert.ToDateTime(SetExcelTime(row.Cells[7]));
                                    if (staTime.AddDays(6) != endTime)
                                    {
                                        return Json(new
                                        {
                                            code = JsonReturnMsg.FailCode,
                                            msg = className + "日期有错误",

                                        });
                                    }
                                    else
                                    {
                                        st.staTime = staTime;
                                        st.endTime = endTime;
                                    }
                                    i++;//跳过一行
                                    continue;
                                }
                                else
                                {
                                    st = setSchoolTimetable(row, st);
                                }
                            }

                           

                            //将数据批量导入数据库
                            int result = TMongodbHelper<tb_SchoolTimetable>.InsertMany(mdb, schoolTimetList);//批量插入
                            if (result == 1)
                            {
                                return Json(new
                                {
                                    code = JsonReturnMsg.SuccessCode,
                                    msg = "添加成功",

                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    code = JsonReturnMsg.FailCode,
                                    msg = "添加失败",

                                });
                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                code = JsonReturnMsg.FailCode,
                                msg = JsonReturnMsg.GetFail,
                                data = "数据导入失败"
                            });
                        }
                    }
                }
                else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail,
                        data = "数据导入失败"
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
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
        private tb_SchoolTimetable setSchoolTimetable(IRow row, tb_SchoolTimetable st)
        {
            string[] sp = (row.GetCell(1) + "").Split('/');
            if (sp[0] == "")
            {
                sp = ("//").Split('/');
            }
            st.week.Monday.courseAndTimeList.Add(new courseAndTime
            {
                index = row.GetCell(0) + "",
                course = sp[0],
                classtime = row.GetCell(8) + "",
                teacherName = sp[1],
                address = sp[2],
            });
            sp = (row.GetCell(2) + "").Split('/');
            if (sp[0] == "")
            {
                sp = ("//").Split('/');
            }
            st.week.Tuesday.courseAndTimeList.Add(new courseAndTime
            {
                index = row.GetCell(0) + "",
                course = sp[0],
                classtime = row.GetCell(8) + "",
                teacherName = sp[1],
                address = sp[2],
            });
            sp = (row.GetCell(3) + "").Split('/');
            if (sp[0] == "")
            {
                sp = ("//").Split('/');
            }
            st.week.Wednesday.courseAndTimeList.Add(new courseAndTime
            {
                index = row.GetCell(0) + "",
                course = sp[0],
                classtime = row.GetCell(8) + "",
                teacherName = sp[1],
                address = sp[2],
            });
            sp = (row.GetCell(4) + "").Split('/');
            if (sp[0] == "")
            {
                sp = ("//").Split('/');
            }
            st.week.Thursday.courseAndTimeList.Add(new courseAndTime
            {
                index = row.GetCell(0) + "",
                course = sp[0],
                classtime = row.GetCell(8) + "",
                teacherName = sp[1],
                address = sp[2],
            });
            sp = (row.GetCell(5) + "").Split('/');
            if (sp[0] == "")
            {
                sp = ("//").Split('/');
            }
            st.week.Friday.courseAndTimeList.Add(new courseAndTime
            {
                index = row.GetCell(0) + "",
                course = sp[0],
                classtime = row.GetCell(8) + "",
                teacherName = sp[1],
                address = sp[2],
            });
            sp = (row.GetCell(6) + "").Split('/');
            if (sp[0] == "")
            {
                sp = ("//").Split('/');
            }
            st.week.Saturday.courseAndTimeList.Add(new courseAndTime
            {
                index = row.GetCell(0) + "",
                course = sp[0],
                classtime = row.GetCell(8) + "",
                teacherName = sp[1],
                address = sp[2],
            });
            sp = (row.GetCell(7) + "").Split('/');
            if (sp[0] == "")
            {
                sp = ("//").Split('/');
            }
            st.week.Sunday.courseAndTimeList.Add(new courseAndTime
            {
                index = row.GetCell(0) + "",
                course = sp[0],
                classtime = row.GetCell(8) + "",
                teacherName = sp[1],
                address = sp[2],
            });
            return st;
        }

        /// <summary>
        /// 修改课表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UpdataSchoolTimetable(string id)
        {
            try
            {

                tb_SchoolTimetable st = new tb_SchoolTimetable();
                UpdateResult result = TMongodbHelper<tb_SchoolTimetable>.Update(mdb, st, id);
                return Json(new
                {
                    code = "1",
                    msg = "修改成功",

                });

            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "修改错误！"
                });
            }
        }

        /// <summary>
        /// 删除课表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DeleteSchoolTimetable(string id)
        {
            try
            {

                DeleteResult result = TMongodbHelper<tb_SchoolTimetable>.Delete(mdb, id);
                return Json(new
                {
                    code = "1",
                    msg = "删除成功",

                });

            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "删除错误！"
                });
            }
        }
        private List<tb_SchoolTimetable> GetSchoolTimetableInfo(string classID, string schoolcode, string staTime = "",string teacherName="",string address="")
        {
            var list = new List<FilterDefinition<tb_SchoolTimetable>>();
            if (!string.IsNullOrWhiteSpace(staTime))
            {
                var queryTime = DateTime.Parse(staTime);
                list.Add(Builders<tb_SchoolTimetable>.Filter.Gte("endTime", queryTime) & Builders<tb_SchoolTimetable>.Filter.Lte("staTime", queryTime));
            }
            if (!string.IsNullOrEmpty(teacherName)&& !string.IsNullOrEmpty(address))
            {
                list.Add((Builders<tb_SchoolTimetable>.Filter.Eq("week.Monday.courseAndTimeList.teacherName", teacherName)
                        & Builders<tb_SchoolTimetable>.Filter.Eq("week.Monday.courseAndTimeList.address", address))
                       | (Builders<tb_SchoolTimetable>.Filter.Eq("week.Tuesday.courseAndTimeList.teacherName", teacherName)
                        & Builders<tb_SchoolTimetable>.Filter.Eq("week.Tuesday.courseAndTimeList.address", address))
                       | (Builders<tb_SchoolTimetable>.Filter.Eq("week.Wednesday.courseAndTimeList.teacherName", teacherName)
                        & Builders<tb_SchoolTimetable>.Filter.Eq("week.Wednesday.courseAndTimeList.address", address))
                       | (Builders<tb_SchoolTimetable>.Filter.Eq("week.Thursday.courseAndTimeList.teacherName", teacherName)
                        & Builders<tb_SchoolTimetable>.Filter.Eq("week.Thursday.courseAndTimeList.address", address))
                       | (Builders<tb_SchoolTimetable>.Filter.Eq("week.Friday.courseAndTimeList.teacherName", teacherName)
                        & Builders<tb_SchoolTimetable>.Filter.Eq("week.Friday.courseAndTimeList.address", address))
                       | (Builders<tb_SchoolTimetable>.Filter.Eq("week.Saturday.courseAndTimeList.teacherName", teacherName)
                        & Builders<tb_SchoolTimetable>.Filter.Eq("week.Saturday.courseAndTimeList.address", address))
                       | (Builders<tb_SchoolTimetable>.Filter.Eq("week.Sunday.courseAndTimeList.teacherName", teacherName)
                        & Builders<tb_SchoolTimetable>.Filter.Eq("week.Sunday.courseAndTimeList.address", address)));
            }
            else
            {
                if (!string.IsNullOrEmpty(teacherName))
                {
                    list.Add(Builders<tb_SchoolTimetable>.Filter.Eq("week.Monday.courseAndTimeList.teacherName", teacherName)
                        | Builders<tb_SchoolTimetable>.Filter.Eq("week.Tuesday.courseAndTimeList.teacherName", teacherName)
                        | Builders<tb_SchoolTimetable>.Filter.Eq("week.Wednesday.courseAndTimeList.teacherName", teacherName)
                        | Builders<tb_SchoolTimetable>.Filter.Eq("week.Thursday.courseAndTimeList.teacherName", teacherName)
                        | Builders<tb_SchoolTimetable>.Filter.Eq("week.Friday.courseAndTimeList.teacherName", teacherName)
                        | Builders<tb_SchoolTimetable>.Filter.Eq("week.Saturday.courseAndTimeList.teacherName", teacherName)
                        | Builders<tb_SchoolTimetable>.Filter.Eq("week.Sunday.courseAndTimeList.teacherName", teacherName));
                }
                if (!string.IsNullOrEmpty(address))
                {
                    list.Add(Builders<tb_SchoolTimetable>.Filter.Eq("week.Monday.courseAndTimeList.address", address)
                        | Builders<tb_SchoolTimetable>.Filter.Eq("week.Tuesday.courseAndTimeList.address", address)
                        | Builders<tb_SchoolTimetable>.Filter.Eq("week.Wednesday.courseAndTimeList.address", address)
                        | Builders<tb_SchoolTimetable>.Filter.Eq("week.Thursday.courseAndTimeList.address", address)
                        | Builders<tb_SchoolTimetable>.Filter.Eq("week.Friday.courseAndTimeList.address", address)
                        | Builders<tb_SchoolTimetable>.Filter.Eq("week.Saturday.courseAndTimeList.address", address)
                        | Builders<tb_SchoolTimetable>.Filter.Eq("week.Sunday.courseAndTimeList.address", address));
                }
            }
           
            list.Add(Builders<tb_SchoolTimetable>.Filter.Eq("schoolcode", schoolcode));
            list.Add(Builders<tb_SchoolTimetable>.Filter.Eq("class_id", classID));
            //list.Add(Builders<tb_SchoolTimetable>.Filter.Gt("schoolcode", "10000"));
            var filter = Builders<tb_SchoolTimetable>.Filter.And(list);

            //2.查询字段
            //var field = new[] { "schoolcode", "class_id", "staTime", "endTime", "week" };
            //3.排序字段
            var sort = Builders<tb_SchoolTimetable>.Sort.Descending("staTime");
            var res = TMongodbHelper<tb_SchoolTimetable>.FindList(mdb, filter, null,sort);
            // var res = TMongodbHelper<PhoneEntity>.FindList(host, filter, field, sort);
            return res;
        }

        /// <summary>
        /// 查询时间段
        /// </summary>
        /// <param name="classID"></param>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolTimetableTime(string classID, string schoolcode)
        {
            try
            {
                var list = new List<FilterDefinition<tb_SchoolTimetable>>();
                string[] sp = DateTime.Now.ToString("yyyy-MM-dd").Split('-');
                DateTime sTime = Convert.ToDateTime(sp[0] + "/01/01");
                DateTime eTime = Convert.ToDateTime(sp[0] + "/12/31");
                list.Add(Builders<tb_SchoolTimetable>.Filter.Gte("staTime", sTime) & Builders<tb_SchoolTimetable>.Filter.Lte("staTime", eTime));
                list.Add(Builders<tb_SchoolTimetable>.Filter.Eq("schoolcode", schoolcode));
                list.Add(Builders<tb_SchoolTimetable>.Filter.Eq("class_id", classID));
                //list.Add(Builders<tb_SchoolTimetable>.Filter.Gt("schoolcode", "10000"));
                var filter = Builders<tb_SchoolTimetable>.Filter.And(list);

                //2.查询字段
                var field = new[] {"staTime", "endTime" };
                //3.排序字段
                var sort = Builders<tb_SchoolTimetable>.Sort.Descending("staTime");
                var res = TMongodbHelper<tb_SchoolTimetable>.FindList(mdb, filter, field, sort);
                // var res = TMongodbHelper<PhoneEntity>.FindList(host, filter, field, sort);
                List<tb_SchoolTimetable> list2 = new List<tb_SchoolTimetable>();
                foreach (var item in res)
                {
                    item.staTime = item.staTime.AddHours(8);
                    item.endTime = item.endTime.AddHours(8);
                    list2.Add(item);
                }
                return Json(new
                {
                    code = "1",
                    data = list2
                });

            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "查询错误！"
                });
            }
        }
       
        /// <summary>
        /// 查询课表
        /// </summary>
        /// <param name="classID"></param>
        /// <param name="schoolcode"></param>
        /// <param name="staTime"></param>
        /// <param name="auth_code"></param>
        /// <param name="app_id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolTimetableInfoToClassID(string classID, string schoolcode, string staTime = "",string auth_code="",string app_id="")
        {
            try
            {
                //AppHelper app = new AppHelper();
                //app.GetUserInfo(auth_code);
                var res = GetSchoolTimetableInfo(classID, schoolcode, staTime);
                List<tb_SchoolTimetable> list = new List<tb_SchoolTimetable>();
                foreach (var item in res)
                {
                    item.staTime = item.staTime.AddHours(8);
                    item.endTime = item.endTime.AddHours(8);
                    list.Add(item);
                }
                if (list.Count==0)
                {
                    return Json(new
                    {
                        code = "0",
                        msg = "查询错误！"
                    });
                }
                return Json(new
                {
                    code = "1",
                    data = list
                });

            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "查询错误！"
                });
            }
        }
        /// <summary>
        /// 查询课表
        /// </summary>
        /// <param name="classID"></param>
        /// <param name="schoolcode"></param>
        /// <param name="staTime"></param>
        /// <param name="sEcho"></param>
        /// <param name="teacherName"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolTimetableInfoToClassID2(string classID, string schoolcode, string staTime = "", string sEcho = "",string teacherName="",string address="")
        {
            try
            {
                int totalRecordNum = 0;

                var res = GetSchoolTimetableInfo(classID, schoolcode, staTime, teacherName, address);
                if (res.Count == 0)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.GetSuccess,
                        sEcho = sEcho,
                        iTotalRecords = totalRecordNum,
                        iTotalDisplayRecords = totalRecordNum,
                        aaData = res
                        //code = "0",
                        //msg = "数据尚未录入！"
                    });
                }
                List<schoolTimetable2> list = new List<schoolTimetable2>();
                for (int i = 0, j = 1; i < res[0].week.Monday.courseAndTimeList.Count; i++, j++)
                {
                    schoolTimetable2 st = new schoolTimetable2();
                    st.index = "第" + j + "节";
                    st.Monday = (res[0].week.Monday.courseAndTimeList[i].course == "" ? "" : res[0].week.Monday.courseAndTimeList[i].course + "|" + res[0].week.Monday.courseAndTimeList[i].teacherName + "|" + res[0].week.Monday.courseAndTimeList[i].address);
                    st.Tuesday = (res[0].week.Tuesday.courseAndTimeList[i].course == "" ? "" : res[0].week.Tuesday.courseAndTimeList[i].course + "|" + res[0].week.Tuesday.courseAndTimeList[i].teacherName + "|" + res[0].week.Tuesday.courseAndTimeList[i].address);
                    st.Wednesday = (res[0].week.Wednesday.courseAndTimeList[i].course == "" ? "" : res[0].week.Wednesday.courseAndTimeList[i].course + "|" + res[0].week.Wednesday.courseAndTimeList[i].teacherName + "|" + res[0].week.Wednesday.courseAndTimeList[i].address);
                    st.Thursday = (res[0].week.Thursday.courseAndTimeList[i].course == "" ? "" : res[0].week.Thursday.courseAndTimeList[i].course + "|" + res[0].week.Thursday.courseAndTimeList[i].teacherName + "|" + res[0].week.Thursday.courseAndTimeList[i].address);
                    st.Friday = (res[0].week.Friday.courseAndTimeList[i].course == "" ? "" : res[0].week.Friday.courseAndTimeList[i].course + "|" + res[0].week.Friday.courseAndTimeList[i].teacherName + "|" + res[0].week.Friday.courseAndTimeList[i].address);
                    st.Saturday = (res[0].week.Saturday.courseAndTimeList[i].course == "" ? "" : res[0].week.Saturday.courseAndTimeList[i].course + "|" + res[0].week.Saturday.courseAndTimeList[i].teacherName + "|" + res[0].week.Saturday.courseAndTimeList[i].address);
                    st.Sunday = (res[0].week.Sunday.courseAndTimeList[i].course == "" ? "" : res[0].week.Sunday.courseAndTimeList[i].course + "|" + res[0].week.Sunday.courseAndTimeList[i].teacherName + "|" + res[0].week.Sunday.courseAndTimeList[i].address);
                    list.Add(st);
                }

                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    sEcho = sEcho,
                    iTotalRecords = totalRecordNum,
                    iTotalDisplayRecords = totalRecordNum,
                    aaData = list
                });

            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }
        /// <summary>
        /// 获取教师名
        /// </summary>
        /// <param name="classID"></param>
        /// <param name="schoolcode"></param>
        /// <param name="staTime"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTeacherName(string classID, string schoolcode, string staTime)
        {
            try
            {
                var list = new List<FilterDefinition<tb_SchoolTimetable>>();
                var queryTime = DateTime.Parse(staTime);
                list.Add(Builders<tb_SchoolTimetable>.Filter.Gte("endTime", queryTime) & Builders<tb_SchoolTimetable>.Filter.Lte("staTime", queryTime));
                list.Add(Builders<tb_SchoolTimetable>.Filter.Eq("schoolcode", schoolcode));
                list.Add(Builders<tb_SchoolTimetable>.Filter.Eq("class_id", classID));
                //list.Add(Builders<tb_SchoolTimetable>.Filter.Gt("schoolcode", "10000"));
                var filter = Builders<tb_SchoolTimetable>.Filter.And(list);
                var res = TMongodbHelper<tb_SchoolTimetable>.FindList(mdb, filter, null);
                var teacherNameList = res[0].week.Monday.courseAndTimeList.Select(x => x.teacherName).ToList();
                teacherNameList = teacherNameList.Union(res[0].week.Tuesday.courseAndTimeList.Select(x => x.teacherName).ToList()).ToList()
                    .Union(res[0].week.Wednesday.courseAndTimeList.Select(x => x.teacherName).ToList()).ToList()
                    .Union(res[0].week.Thursday.courseAndTimeList.Select(x => x.teacherName).ToList()).ToList()
                    .Union(res[0].week.Saturday.courseAndTimeList.Select(x => x.teacherName).ToList()).ToList()
                    .Union(res[0].week.Sunday.courseAndTimeList.Select(x => x.teacherName).ToList()).ToList();
                teacherNameList = teacherNameList.Where(x => !string.IsNullOrEmpty(x)).ToList();

                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = teacherNameList
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail,
                });
            }
        }
        /// <summary>
        /// 获取教学地点
        /// </summary>
        /// <param name="classID"></param>
        /// <param name="schoolcode"></param>
        /// <param name="staTime"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAddressInfo(string classID, string schoolcode, string staTime)
        {
            try
            {
                var list = new List<FilterDefinition<tb_SchoolTimetable>>();
                var queryTime = DateTime.Parse(staTime);
                list.Add(Builders<tb_SchoolTimetable>.Filter.Gte("endTime", queryTime) & Builders<tb_SchoolTimetable>.Filter.Lte("staTime", queryTime));
                list.Add(Builders<tb_SchoolTimetable>.Filter.Eq("schoolcode", schoolcode));
                list.Add(Builders<tb_SchoolTimetable>.Filter.Eq("class_id", classID));
                //list.Add(Builders<tb_SchoolTimetable>.Filter.Gt("schoolcode", "10000"));
                var filter = Builders<tb_SchoolTimetable>.Filter.And(list);
                var res = TMongodbHelper<tb_SchoolTimetable>.FindList(mdb, filter, null);
                var teacherNameList = res[0].week.Monday.courseAndTimeList.Select(x => x.address).ToList();
                teacherNameList = teacherNameList.Union(res[0].week.Tuesday.courseAndTimeList.Select(x => x.address).ToList()).ToList()
                    .Union(res[0].week.Wednesday.courseAndTimeList.Select(x => x.address).ToList()).ToList()
                    .Union(res[0].week.Thursday.courseAndTimeList.Select(x => x.address).ToList()).ToList()
                    .Union(res[0].week.Saturday.courseAndTimeList.Select(x => x.address).ToList()).ToList()
                    .Union(res[0].week.Sunday.courseAndTimeList.Select(x => x.address).ToList()).ToList();
                teacherNameList = teacherNameList.Where(x => !string.IsNullOrEmpty(x)).ToList();

                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = teacherNameList
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail,
                });
            }
        }
    }
}
/// <summary>
/// 
/// </summary>
public class tb_SchoolTimetable
{
    /// <summary>
    /// 
    /// </summary>
    public tb_SchoolTimetable()
    {
        week = new week();
    }
    /// <summary>
    /// 
    /// </summary>
    public ObjectId _id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string schoolcode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string class_id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime staTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime endTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public week week { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public tb_SchoolTimetable Clone()
    {
        return (tb_SchoolTimetable)this.MemberwiseClone();
    }
}
/// <summary>
/// 
/// </summary>
public class week
{
    /// <summary>
    /// 
    /// </summary>
    public week()
    {
        Monday = new Monday();
        Tuesday = new Tuesday();
        Wednesday = new Wednesday();
        Thursday = new Thursday();
        Friday = new Friday();
        Saturday = new Saturday();
        Sunday = new Sunday();
    }
    /// <summary>
    /// 
    /// </summary>
    public Monday Monday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Tuesday Tuesday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Wednesday Wednesday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Thursday Thursday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Friday Friday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Saturday Saturday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Sunday Sunday { get; set; }
}
/// <summary>
/// 
/// </summary>
public class Monday
{
    /// <summary>
    /// 
    /// </summary>
    public Monday()
    {
        courseAndTimeList = new List<courseAndTime>();
    }
    /// <summary>
    /// 
    /// </summary>
    public List<courseAndTime> courseAndTimeList { get; set; }
}
/// <summary>
/// 
/// </summary>
public class Tuesday
{
    /// <summary>
    /// 
    /// </summary>
    public Tuesday()
    {
        courseAndTimeList = new List<courseAndTime>();
    }
    /// <summary>
    /// 
    /// </summary>
    public List<courseAndTime> courseAndTimeList { get; set; }
}
/// <summary>
/// 
/// </summary>
public class Wednesday
{
    /// <summary>
    /// 
    /// </summary>
    public Wednesday()
    {
        courseAndTimeList = new List<courseAndTime>();
    }
    /// <summary>
    /// 
    /// </summary>
    public List<courseAndTime> courseAndTimeList { get; set; }
}
/// <summary>
/// 
/// </summary>
public class Thursday
{
    /// <summary>
    /// 
    /// </summary>
    public Thursday()
    {
        courseAndTimeList = new List<courseAndTime>();
    }
    /// <summary>
    /// 
    /// </summary>
    public List<courseAndTime> courseAndTimeList { get; set; }
}
/// <summary>
/// 
/// </summary>
public class Friday
{
    /// <summary>
    /// 
    /// </summary>
    public Friday()
    {
        courseAndTimeList = new List<courseAndTime>();
    }
    /// <summary>
    /// 
    /// </summary>
    public List<courseAndTime> courseAndTimeList { get; set; }
}
/// <summary>
/// 
/// </summary>
public class Saturday
{
    /// <summary>
    /// 
    /// </summary>
    public Saturday()
    {
        courseAndTimeList = new List<courseAndTime>();
    }
    /// <summary>
    /// 
    /// </summary>
    public List<courseAndTime> courseAndTimeList { get; set; }
}
/// <summary>
/// 
/// </summary>
public class Sunday
{
    /// <summary>
    /// 
    /// </summary>
    public Sunday()
    {
        courseAndTimeList = new List<courseAndTime>();
    }
    /// <summary>
    /// 
    /// </summary>
    public List<courseAndTime> courseAndTimeList { get; set; }
}
/// <summary>
/// 
/// </summary>
public class courseAndTime
{
    /// <summary>
    /// 
    /// </summary>
    public string index { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string course { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string classtime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string teacherName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string address { get; set; }
}

/// <summary>
/// 
/// </summary>
public class schoolTimetable2
{
    /// <summary>
    /// 
    /// </summary>
    public string index { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Monday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Tuesday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Wednesday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Thursday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Friday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Saturday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Sunday { get; set; }
}