﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DbModel;
using Exceptionless.Json.Linq;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SchoolWebApi.HeadParams;
using SchoolWebApi.Utility;
using SqlSugar;
using NPOI.HSSF.UserModel;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 学校部门控制层
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class SchoolDepartmentController : Controller
    {
        private static string tempPath = @"C:\Temp\";
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private Itb_school_departmentService _tb_school_departmentService;
        private Itb_school_classinfoService _tb_school_classinfoService;
        private Itb_school_departmentinfoService _tb_school_departmentinfoService;
        private Itb_school_userService _tb_school_userService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_school_departmentService"></param>
        /// <param name="tb_school_classinfoService"></param>
        /// <param name="tb_school_departmentinfoService"></param>
        /// <param name="tb_school_userService"></param>
        public SchoolDepartmentController(Itb_school_departmentService tb_school_departmentService,
            Itb_school_classinfoService tb_school_classinfoService,
            Itb_school_departmentinfoService tb_school_departmentinfoService,
            Itb_school_userService tb_school_userService)
        {
            _tb_school_departmentService = tb_school_departmentService;
            _tb_school_classinfoService = tb_school_classinfoService;
            _tb_school_departmentinfoService = tb_school_departmentinfoService;
            _tb_school_userService = tb_school_userService;
        }

        private string GetSchoolCode(string schoolCode)
        {
            var headers = HttpContext.Request.Headers;
            if (headers.TryGetValue("schoolcode", out var headerValues))
            {
                schoolCode = headerValues.First();
            }
            return schoolCode;
        }

        /// <summary>
        /// 从Excel导入部门
        /// </summary>
        /// <returns></returns>
        [HeaderParams(nameof(HeadParamsType.SchoolCode),
           nameof(HeadParamsType.FileUpload))]
        [HttpPost]
        public IActionResult ImportSchoolDepartment()
        {
            var schoolcode = string.Empty;
            schoolcode = GetSchoolCode(schoolcode);
            var files = HttpContext.Request.Form.Files;
            var filePath = string.Empty;

            var isColumnName = true;
            System.Data.DataColumn column = null;
            System.Data.DataRow dataRow = null;
            IRow row = null;
            ICell cell = null;
            int startRow = 0;

            try
            {
                if (files.Count > 0)
                {
                    var file = files[0];
                    //给文件一个随机的名字
                    var fileNewName = Guid.NewGuid().ToString();
                    string fileExt = file.FileName.Split('.')[1];
                    FileHelper.CreateFiles(tempPath, true);
                    filePath = tempPath + $@"{fileNewName}.{fileExt}";

                    //保存文件到临时文件夹下
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    using (FileStream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        HSSFWorkbook workbook = new HSSFWorkbook(stream);
                        var sheetNum = workbook.NumberOfSheets;
                        if (sheetNum > 0)
                        {
                            ISheet sheet = workbook.GetSheetAt(0);
                            var dataTable = new System.Data.DataTable();
                            if (sheet != null)
                            {
                                int rowCount = sheet.LastRowNum;//总行数  
                                if (rowCount > 0)
                                {
                                    IRow firstRow = sheet.GetRow(0);//第一行  
                                    int cellCount = firstRow.LastCellNum;//列数  

                                    //构建datatable的列  
                                    if (isColumnName)
                                    {
                                        startRow = 1;//如果第一行是列名，则从第二行开始读取  
                                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                        {
                                            cell = firstRow.GetCell(i);
                                            if (cell != null)
                                            {
                                                if (cell.StringCellValue != null)
                                                {
                                                    column = new System.Data.DataColumn(cell.StringCellValue);
                                                    dataTable.Columns.Add(column);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                        {
                                            column = new System.Data.DataColumn("column" + (i + 1));
                                            dataTable.Columns.Add(column);
                                        }
                                    }

                                    //填充行  
                                    for (int i = startRow; i <= rowCount; ++i)
                                    {
                                        row = sheet.GetRow(i);
                                        if (row == null) continue;

                                        dataRow = dataTable.NewRow();
                                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                                        {
                                            cell = row.GetCell(j);
                                            if (cell == null)
                                            {
                                                dataRow[j] = "";
                                            }
                                            else
                                            {
                                                switch (cell.CellType)
                                                {
                                                    case CellType.Blank:
                                                        dataRow[j] = "";
                                                        break;
                                                    case CellType.Numeric:
                                                        short format = cell.CellStyle.DataFormat;
                                                        //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理  
                                                        if (format == 14 || format == 31 || format == 57 || format == 58)
                                                            dataRow[j] = cell.DateCellValue;
                                                        else
                                                            dataRow[j] = cell.NumericCellValue;
                                                        break;
                                                    case CellType.String:
                                                        dataRow[j] = cell.StringCellValue;
                                                        break;
                                                }
                                            }
                                        }
                                        dataTable.Rows.Add(dataRow);
                                    }
                                }

                                var dt = dataTable;
                                foreach (System.Data.DataRow item in dt.Rows)
                                {
                                    if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                                    {
                                        string department = item[0].ToString().Trim();
                                        var deparr = department.Split('/');
                                        string pid = "";
                                        for (int c = 0; c < deparr.Length; c++)
                                        {
                                            if (c == 0)
                                            {
                                                SqlSugar.DataTable dtp = _tb_school_departmentService.selbypid(schoolcode);
                                                pid = dtp.Rows[0]["id"].ToString();
                                            }

                                            SqlSugar.DataTable depdt = _tb_school_departmentService.selbyname(schoolcode, deparr[c].ToString());
                                            if (c == 0 && depdt.Rows.Count == 0)
                                            {
                                                _tb_school_departmentService.ADD(schoolcode, deparr[c].ToString(), int.Parse(pid), c + 1);
                                                SqlSugar.DataTable pdt = _tb_school_departmentService.selbyname(schoolcode, deparr[c].ToString());
                                                pid += "/" + pdt.Rows[0]["id"].ToString();
                                            }
                                            else if (c > 0 && depdt.Rows.Count == 0)
                                            {
                                                _tb_school_departmentService.ADD(schoolcode, deparr[c].ToString(), int.Parse(pid.Split('/')[pid.Split('/').Length - 1]), c + 1);
                                                SqlSugar.DataTable pdt = _tb_school_departmentService.selbyname(schoolcode, deparr[c].ToString());
                                                pid += "/" + pdt.Rows[0]["id"].ToString();
                                            }
                                            else
                                            {
                                                SqlSugar.DataTable pdt = _tb_school_departmentService.selbyname(schoolcode, deparr[c].ToString());
                                                pid += "/" + pdt.Rows[0]["id"].ToString();
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        msg = JsonReturnMsg.GetSuccess,
                        data = "数据保存成功！"
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.UploadFail,
                        data = "文件上传失败"
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UploadFail,
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

        /// <summary>
        ///  添加学校部门PS:学校根目录必须存在不然报错
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddSchoolDepartmentInfo(string schoolcode)
        {
            try
            {
                var dt = _tb_school_departmentService.GetDepartmentInfo(schoolcode);
                foreach (System.Data.DataRow item in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                    {
                        string department = item[0].ToString().Trim();
                        var deparr = department.Split('/');
                        string pid = "";
                        for (int c = 0; c < deparr.Length; c++)
                        {
                            if (c == 0)
                            {
                                SqlSugar.DataTable dtp = _tb_school_departmentService.selbypid(schoolcode);
                                pid = dtp.Rows[0]["id"].ToString();
                            }

                            SqlSugar.DataTable depdt = _tb_school_departmentService.selbyname(schoolcode, deparr[c].ToString());
                            if (c == 0 && depdt.Rows.Count == 0)
                            {
                                _tb_school_departmentService.ADD(schoolcode, deparr[c].ToString(), int.Parse(pid), c + 1);
                                SqlSugar.DataTable pdt = _tb_school_departmentService.selbyname(schoolcode, deparr[c].ToString());
                                pid += "/" + pdt.Rows[0]["id"].ToString();
                            }
                            else if (c > 0 && depdt.Rows.Count == 0)
                            {
                                _tb_school_departmentService.ADD(schoolcode, deparr[c].ToString(), int.Parse(pid.Split('/')[pid.Split('/').Length - 1]), c + 1);
                                SqlSugar.DataTable pdt = _tb_school_departmentService.selbyname(schoolcode, deparr[c].ToString());
                                pid += "/" + pdt.Rows[0]["id"].ToString();
                            }
                            else
                            {
                                SqlSugar.DataTable pdt = _tb_school_departmentService.selbyname(schoolcode, deparr[c].ToString());
                                pid += "/" + pdt.Rows[0]["id"].ToString();
                            }
                        }
                    }
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = "数据保存成功！"
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }
        /// <summary>
        /// 获取学校部门信息(2.0首页下拉对应数据源)
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolDepartmentInfo(string schoolcode)
        {
            try
            {
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = _tb_school_departmentService.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, OrderByType.Asc)
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolUserInfoToDepartmentID(string schoolcode, string departmentID)
        {
            try
            {

                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = _tb_school_departmentService.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, OrderByType.Asc)
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 添加部门（tree下点击添加）
        /// 参数isClass 班级信息为0 学校部门为1
        /// treeLevel 点击节点的等级3级就需要给班级表或者部门表添加数据
        /// p_id 点击节点的pid
        /// name 添加部门的名称
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddDepartment([FromBody]JObject obj)
        {
            try
            {
                string isClass = obj["isClass"].ToString();
                string schoolcode = obj["schoolcode"].ToString();
                string treeLevel = obj["treeLevel"].ToString();
                int p_id = Convert.ToInt32(obj["p_id"].ToString());
                tb_school_department model = new tb_school_department();
                model.name = obj["name"].ToString();
                model.p_id = p_id;
                model.schoolcode = schoolcode;
                model.treeLevel = Convert.ToInt32(treeLevel) + 1;
                treeLevel = model.treeLevel + "";
                var treeID = _tb_school_departmentService.Insert(model);
                if (treeLevel == "3" && isClass == "0")
                {
                    _tb_school_departmentService.UpdateColumnsByConditon(x => new tb_school_department { isType = false }, x => x.id == treeID);
                    var department = _tb_school_departmentService.FindByClause(x => x.id == p_id);
                    var branch = _tb_school_departmentService.FindByClause(x => x.id == department.p_id);
                    tb_school_classinfo classModel = new tb_school_classinfo();
                    classModel.department_classID = Convert.ToInt32(treeID);
                    classModel.DepartmentID = department.id;
                    classModel.BranchID = branch.id;
                    classModel.schoolcode = schoolcode;
                    _tb_school_classinfoService.Insert(classModel);
                }
                if (treeLevel == "3" && isClass == "1")
                {
                    _tb_school_departmentService.UpdateColumnsByConditon(x => new tb_school_department { isType = true }, x => x.id == treeID);
                    var department = _tb_school_departmentService.FindByClause(x => x.id == p_id);
                    var branch = _tb_school_departmentService.FindByClause(x => x.id == department.p_id);
                    tb_school_departmentinfo departmentModel = new tb_school_departmentinfo();
                    departmentModel.department_treeID = Convert.ToInt32(treeID);
                    departmentModel.departmentID = department.id;
                    departmentModel.BranchID = branch.id;
                    departmentModel.schoolcode = schoolcode;
                    _tb_school_departmentinfoService.Insert(departmentModel);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess
                });
            }
            catch (Exception ex)
            {

                Log.Error(ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }
        /// <summary>
        /// 删除树形名称（树形结构名---部门）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteDepartment([FromBody]JObject obj)
        {
            try
            {
                string schoolcode = obj["schoolcode"].ToString();
                int id = Convert.ToInt32(obj["id"].ToString());
                var model = _tb_school_departmentService.FindByClause(x => x.id == id && x.schoolcode == schoolcode);
                _tb_school_departmentService.Delete(model);
                if (model.treeLevel == 1)
                {
                    var listmodel = _tb_school_departmentService.FindListByClause(x => x.p_id == model.id && x.schoolcode == schoolcode, t => t.id, OrderByType.Asc) as List<tb_school_department>;
                    foreach (var item in listmodel)//第三级
                    {
                        //第四级
                        var listmodel2 = _tb_school_departmentService.FindListByClause(x => x.p_id == item.id && x.schoolcode == schoolcode, t => t.id, OrderByType.Asc) as List<tb_school_department>;
                        foreach (var item2 in listmodel2)
                        {
                            _tb_school_departmentService.Delete(item2);
                            if (item2.isType == false)
                            {
                                var classinfo = _tb_school_classinfoService.FindByClause(x => x.department_classID == item2.id);
                                _tb_school_classinfoService.Delete(classinfo);
                            }
                            else
                            {
                                var depinfo = _tb_school_departmentinfoService.FindByClause(x => x.department_treeID == item2.id);
                                _tb_school_departmentinfoService.Delete(depinfo);
                            }
                        }
                        _tb_school_departmentService.Delete(item);
                    }
                }
                if (model.treeLevel == 2)
                {
                    var listmodel = _tb_school_departmentService.FindListByClause(x => x.p_id == model.id && x.schoolcode == schoolcode, t => t.id, OrderByType.Asc) as List<tb_school_department>;
                    foreach (var item in listmodel)//第四级
                    {
                        _tb_school_departmentService.Delete(item);
                        if (item.isType == false)
                        {
                            var classinfo = _tb_school_classinfoService.FindByClause(x => x.department_classID == item.id);
                            _tb_school_classinfoService.Delete(classinfo);
                        }
                        else
                        {
                            var depinfo = _tb_school_departmentinfoService.FindByClause(x => x.department_treeID == item.id);
                            _tb_school_departmentinfoService.Delete(depinfo);
                        }
                    }
                }
                if (model.treeLevel == 3)
                {
                    if (model.isType == false)
                    {
                        var classinfo = _tb_school_classinfoService.FindByClause(x => x.department_classID == model.id);
                        _tb_school_classinfoService.Delete(classinfo);
                    }
                    else
                    {
                        var depinfo = _tb_school_departmentinfoService.FindByClause(x => x.department_treeID == model.id);
                        _tb_school_departmentinfoService.Delete(depinfo);
                    }
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.DeleteSuccess
                });
            }
            catch (Exception ex)
            {

                Log.Error(ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.DeleteFail
                });
            }
        }
        /// <summary>
        /// 修改树形名称（树形结构名---部门）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateDepartment([FromBody]JObject obj)
        {
            try
            {
                int id = Convert.ToInt32(obj["id"].ToString());
                string schoolcode = obj["schoolcode"].ToString();
                var model = _tb_school_departmentService.FindByClause(x => x.id == id && x.schoolcode == schoolcode);
                model.name = obj["name"].ToString();
                _tb_school_departmentService.Update(model);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess
                });
            }
            catch (Exception ex)
            {

                Log.Error(ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UpdateFail
                });
            }
        }
        /// <summary>
        /// 获取班级列表通过schoolcode
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolClassInfoToSchoolcode(string schoolcode)
        {
            try
            {
                var data = _tb_school_departmentService.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, OrderByType.Asc);
                var data2 = _tb_school_classinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);
                List<SchoolClassModel> listmodel = new List<SchoolClassModel>();
                foreach (var item in data2)
                {
                    SchoolClassModel model = new SchoolClassModel();
                    model.Branchname = data.Where(x => x.id == item.BranchID).Select(x => x.name).ToList()[0];
                    model.id = item.ID;
                    model.schoolcode = item.schoolcode;
                    model.Departmentname = data.Where(x => x.id == item.DepartmentID).Select(x => x.name).ToList()[0];
                    model.classname = data.Where(x => x.id == item.department_classID).Select(x => x.name).ToList()[0];
                    listmodel.Add(model);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = listmodel
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 获取部门列表通过schoolcode
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolDeparmentInfoToSchoolcode(string schoolcode)
        {
            try
            {
                var data = _tb_school_departmentService.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, OrderByType.Asc);
                var data2 = _tb_school_departmentinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);
                List<SchoolClassModel> listmodel = new List<SchoolClassModel>();
                foreach (var item in data2)
                {
                    SchoolClassModel model = new SchoolClassModel();
                    model.Branchname = data.Where(x => x.id == item.BranchID).Select(x => x.name).ToList()[0];
                    model.id = item.ID;
                    model.schoolcode = item.schoolcode;
                    model.Departmentname = data.Where(x => x.id == item.departmentID).Select(x => x.name).ToList()[0];
                    model.classname = data.Where(x => x.id == item.department_treeID).Select(x => x.name).ToList()[0];
                    listmodel.Add(model);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = listmodel
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 获取学校Tree
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolDepartmentTree(string schoolcode)
        {
            try
            {
                var departmentTree = _tb_school_departmentService.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, OrderByType.Asc);
                var classinfo = _tb_school_classinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);
                var departmentInfo = _tb_school_departmentinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);
                var tree0 = departmentTree.Where(x => x.treeLevel == 0);
                var tree1 = departmentTree.Where(x => x.treeLevel == 1);
                var tree2 = departmentTree.Where(x => x.treeLevel == 2);
                var tree3 = departmentTree.Where(x => x.treeLevel == 3);
                DepartmentTree deptree = new DepartmentTree();
                foreach (var item in tree0)
                {
                    deptree.id = item.id;
                    deptree.isType = item.isType;
                    deptree.label = item.name;
                    deptree.name = item.name;
                    deptree.p_id = item.p_id;
                    deptree.schoolcode = item.schoolcode;
                    deptree.treeLever = item.treeLevel;
                    foreach (var item2 in tree1)
                    {
                        DepartmentTree deptree2 = new DepartmentTree();
                        deptree2.id = item2.id;
                        deptree2.isType = item2.isType;
                        deptree2.label = item2.name;
                        deptree2.name = item2.name;
                        deptree2.p_id = item2.p_id;
                        deptree2.schoolcode = item2.schoolcode;
                        deptree2.treeLever = item2.treeLevel;
                        deptree.children.Add(deptree2);
                        var dd = tree2.Where(x => x.p_id == deptree2.id);
                        foreach (var item3 in dd)
                        {
                            DepartmentTree deptree3 = new DepartmentTree();
                            deptree3.id = item3.id;
                            deptree3.isType = item3.isType;
                            deptree3.label = item3.name;
                            deptree3.name = item3.name;
                            deptree3.p_id = item3.p_id;
                            deptree3.schoolcode = item3.schoolcode;
                            deptree3.treeLever = item3.treeLevel;
                            deptree2.children.Add(deptree3);
                            var ddd = tree3.Where(x => x.p_id == deptree3.id);
                            foreach (var item4 in ddd)
                            {
                                DepartmentTree deptree4 = new DepartmentTree();
                                if (Convert.ToBoolean(item4.isType))
                                {
                                    var dep = departmentInfo.Where(x => x.department_treeID == item4.id).ToList();
                                    if (dep.Count() == 0)
                                    {
                                        continue;
                                    }
                                    deptree4.classid = dep[0].ID;
                                }
                                else
                                {
                                    var classinfo2 = classinfo.Where(x => x.department_classID == item4.id).ToList();
                                    if (classinfo2.Count() == 0)
                                    {
                                        continue;
                                    }
                                    deptree4.classid = classinfo2[0].ID;
                                }
                                deptree4.id = item4.id;
                                deptree4.isType = item4.isType;
                                deptree4.label = item4.name;
                                deptree4.name = item4.name;
                                deptree4.p_id = item4.p_id;
                                deptree4.schoolcode = item4.schoolcode;
                                deptree4.treeLever = item4.treeLevel;
                                deptree3.children.Add(deptree4);
                            }
                        }
                    }

                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = deptree,
                    defaultProps = new { children = "children", label = "label" },
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 根据树形班级点击查询学员信息
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="id"></param>
        /// <param name="level"></param>
        /// <param name="isType"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolUserToTreeId(string schoolcode, int id, int level, bool isType, int iDisplayStart, int iDisplayLength)
        {
            try
            {
                int pageStart = iDisplayStart;
                int pageSize = iDisplayLength;
                int pageIndex = (pageStart / pageSize) + 1;
                //List<tb_school_user> list = new List<tb_school_user>();
                Infrastructure.Service.IPagedList<tb_school_user> list2 = null;
                if (level != 3)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail
                    });
                }
                else
                {
                    var dData = _tb_school_departmentinfoService.FindByClause(x => x.department_treeID == id);
                    var cData = _tb_school_classinfoService.FindByClause(x => x.department_classID == id);
                    if (isType)
                    {
                        //list = _tb_school_userService.FindListByClause(x => x.school_id == schoolcode && x.department_id == dData.ID,t=>t.user_id,OrderByType.Asc).ToList();
                        list2 = _tb_school_userService.FindPagedListOrderyType(z => z.school_id == schoolcode && z.department_id == dData.ID, pageIndex: pageIndex, pageSize: pageSize, expression: x => x.user_id, type: SqlSugar.OrderByType.Desc);
                    }
                    else
                    {
                        //list = _tb_school_userService.FindListByClause(x => x.school_id == schoolcode && x.department_id == cData.ID, t => t.user_id, OrderByType.Asc).ToList();

                        list2 = _tb_school_userService.FindPagedListOrderyType(x => x.school_id == schoolcode && x.department_id == cData.ID, pageIndex: pageIndex, pageSize: pageSize, expression: x => x.user_id, type: SqlSugar.OrderByType.Desc);
                    }
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    //sEcho = sEcho,
                    iTotalRecords = list2.TotalCount,
                    iTotalDisplayRecords = list2.TotalCount,
                    aaData = list2
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 班级联动数据源
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolDepartmentCascader(string schoolcode)
        {
            try
            {
                var departmentTree = _tb_school_departmentService.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, OrderByType.Asc);
                var classInfo = _tb_school_classinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);
                var departmentInfo = _tb_school_departmentinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);
                var tree0 = departmentTree.Where(x => x.treeLevel == 0);
                var tree1 = departmentTree.Where(x => x.treeLevel == 1);
                var tree2 = departmentTree.Where(x => x.treeLevel == 2);
                var tree3 = departmentTree.Where(x => x.treeLevel == 3);
                DepartmentCascader deptree = new DepartmentCascader();
                foreach (var item in tree0)
                {
                    deptree.label = item.name;
                    deptree.value = item.id.ToString();
                    foreach (var item2 in tree1)
                    {
                        DepartmentCascader deptree2 = new DepartmentCascader();
                        deptree2.label = item2.name;
                        deptree2.value = item2.id.ToString();
                        deptree.children.Add(deptree2);
                        var dd = tree2.Where(x => x.p_id == item2.id);
                        foreach (var item3 in dd)
                        {
                            DepartmentCascader deptree3 = new DepartmentCascader();
                            deptree3.label = item3.name;
                            deptree3.value = item3.id.ToString();
                            deptree2.children.Add(deptree3);
                            var ddd = tree3.Where(x => x.p_id == item3.id);
                            foreach (var item4 in ddd)
                            {
                                DepartmentCascader deptree4 = new DepartmentCascader();
                                int id = 0;
                                if (item4.isType == true)
                                {
                                    id = Convert.ToInt32(departmentInfo.Where(x => x.department_treeID == item4.id).Select(x => x.ID).ToList()[0].ToString());
                                }
                                else
                                {
                                    id = Convert.ToInt32(classInfo.Where(x => x.department_classID == item4.id).Select(x => x.ID).ToList()[0].ToString());
                                }
                                deptree4.id = id;
                                deptree4.label = item4.name;
                                deptree4.value = id.ToString();
                                deptree3.children.Add(deptree4);
                            }
                        }
                    }

                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = deptree,
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }

    }
}