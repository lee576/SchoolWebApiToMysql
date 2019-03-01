using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SchoolWebApi.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using SchoolWebApi.HeadParams;
using NPOI.XSSF.UserModel;
using AutoMapper;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 宿舍管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class DormitoryController : Controller
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TempPath = @"C:\Temp\";
        private readonly Itb_Building_Room_ConfigService _tb_Building_Room_ConfigService;
        private readonly Itb_School_User_RoomService _tb_School_User_RoomService;
        private readonly Itb_school_userService _tb_school_userService;
        private readonly Ibanding_dormitoryService _banding_dormitoryService;
        private readonly Itb_school_InfoService _tb_school_InfoService;
        private readonly Itb_QRCode_Base_ConfigService _tb_QRCode_Base_ConfigService;
        private readonly Iview_entrance_recordService _view_entrance_recordService;
        private readonly Itb_school_deviceService _tb_school_deviceService;
        private readonly Itb_entrance_recordService _tb_entrance_recordService;
        private readonly Itb_school_deviceService _tb_school_device;
        private readonly IMapper _mapper;
        private string GetSchoolCode(string schoolCode)
        {
            var headers = HttpContext.Request.Headers;
            if (headers.TryGetValue("schoolcode", out var headerValues))
            {
                schoolCode = headerValues.First();
            }
            return schoolCode;
        }

        private void FillCell(IRow row, view_entrance_Record[] list, int i)
        {
            row.CreateCell(0).SetCellValue(list[i].student_id);
            row.CreateCell(1).SetCellValue(list[i].user_realname);
            row.CreateCell(2).SetCellValue(list[i].device_id);
            row.CreateCell(3).SetCellValue(list[i].device_name);
            row.CreateCell(4).SetCellValue(list[i].school_name);
            row.CreateCell(5).SetCellValue(list[i].open_time?.ToString("yyyy-MM-dd hh:mm:ss"));
            row.CreateCell(6).SetCellValue(list[i].entrance_status);
        }

        /// <inheritdoc />
        public DormitoryController(Itb_Building_Room_ConfigService tb_Building_Room_ConfigService,
            Itb_School_User_RoomService tb_School_User_RoomService,
            Itb_school_userService tb_school_userService,
            Ibanding_dormitoryService banding_dormitoryService,
            Itb_school_InfoService tb_school_InfoService,
            Itb_QRCode_Base_ConfigService tb_QRCode_Base_ConfigService,
            Iview_entrance_recordService view_entrance_recordService,
            Itb_school_deviceService tb_school_deviceService,
            Itb_entrance_recordService tb_entrance_recordService,
            Itb_school_deviceService tb_school_device,
            IMapper mapper)
        {
            _tb_Building_Room_ConfigService = tb_Building_Room_ConfigService;
            _tb_School_User_RoomService = tb_School_User_RoomService;
            _tb_school_userService = tb_school_userService;
            _banding_dormitoryService = banding_dormitoryService;
            _tb_school_InfoService = tb_school_InfoService;
            _tb_QRCode_Base_ConfigService = tb_QRCode_Base_ConfigService;
            _view_entrance_recordService = view_entrance_recordService;
            _tb_school_deviceService = tb_school_deviceService;
            _tb_entrance_recordService = tb_entrance_recordService;
            _tb_school_device = tb_school_device;
            _mapper = mapper;
        }

        /// <summary>
        /// 删除门禁设备
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteSchoolDevice([FromBody] JObject obj)
        {
            try
            {
                var _ids = obj["id"].AsJEnumerable();
                var idList = _ids.Select(id => id + "").ToList();
                if (idList.Count < 1)
                {
                    var model = _tb_school_deviceService.FindByClause(x => x.id.ToString().Contains(obj["id"].ToString()));
                    if (!_tb_school_deviceService.Delete(model))
                        return Json(new
                        {
                            code = JsonReturnMsg.FailCode,
                            msg = JsonReturnMsg.DeleteFail
                        });
                }
                else if (!_tb_school_deviceService.DeleteByIds(idList.ToArray()))
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.DeleteFail
                    });
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.DeleteFail
                });
            }
        }

        /// <summary>
        /// 修改门禁设备
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditSchoolDevice([FromBody] JObject obj)
        {
            try
            {
                string schoolcode = obj["schoolcode"].ToString(),
                        id = obj["id"].ToString(),
                        deviceName = obj["deviceName"].ToString(),
                        shopid = obj["shopid"].ToString();
                tb_school_device model = _tb_school_deviceService.FindByClause(x => x.id.ToString().Equals(id));
                model.device_name = deviceName;
                model.school_id = schoolcode;
                model.shop_id = shopid;

                if (_tb_school_deviceService.Update(model))
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        msg = JsonReturnMsg.UpdateSuccess
                    });
                }
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UpdateFail
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UpdateFail
                });
            }
        }
        /// <summary>
        /// 通过学校Code查询门禁设备
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSchoolDeviceToSchoolCode(string schoolcode)
        {
            try
            {

                List<tb_school_device> lst = _tb_school_deviceService.FindListByClause(x => x.school_id == schoolcode, t => t.id, OrderByType.Asc) as List<tb_school_device>;
                var data = lst.Select(x => new
                {
                    device_name = x.device_name,
                    device_state = x.device_state
                }).ToList();
                data = data.Distinct().ToList();
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = data
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 通过学校Code查询门禁设备（可以通过设备Code和名称进行筛选查询）
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="schoolCode"></param>
        /// <param name="deviceCode"></param>
        /// <param name="deviceName"></param>
        /// <param name="iDisplayStart"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSchoolDevice(int sEcho, int iDisplayStart, int iDisplayLength, string schoolCode, string deviceCode = "", string deviceName = "")
        {
            try
            {
                int pageStart = iDisplayStart;
                int pageSize = iDisplayLength;
                int pageIndex = (pageStart / pageSize) + 1;
                int totalRecordNum = default(int);
                List<tb_school_device> lst = _tb_school_deviceService.GetList(pageIndex, pageSize, schoolCode, deviceCode, deviceName, ref totalRecordNum);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    sEcho = sEcho,
                    iTotalRecords = totalRecordNum,
                    iTotalDisplayRecords = totalRecordNum,
                    data = lst
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }

        /// <summary>
        /// 查询公共区域,如图书馆,食堂等,条件: ispublic == 1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetPublicArea(int sEcho,
            int iDisplayStart,
            int iDisplayLength,
            string publicName = "")
        {
            try
            {
                int pageStart = iDisplayStart;
                int pageSize = iDisplayLength;
                int pageIndex = (pageStart / pageSize) + 1;
                int totalRecordNum = default(int);
                //从请求Http Headers中获取学校编码
                var schoolCode = string.Empty;
                schoolCode = GetSchoolCode(schoolCode);
                List<tb_building_room_config> dormitory;
                Expression<Func<tb_building_room_config, bool>> expression = null;
                if (!string.IsNullOrEmpty(schoolCode))
                {
                    if (!string.IsNullOrEmpty(publicName))
                        expression = t => t.school_id == SqlFunc.ToInt32(schoolCode) && t.ispublic == 1 && t.building_room_no == publicName;
                    else
                        expression = t => t.school_id == SqlFunc.ToInt32(schoolCode) && t.ispublic == 1;
                    dormitory = _tb_Building_Room_ConfigService.FindPublicArea(pageIndex, pageSize, ref totalRecordNum,
                        expression) as List<tb_building_room_config>;
                }
                else
                {
                    if (!string.IsNullOrEmpty(publicName))
                        expression = t => t.ispublic == 1 && t.building_room_no == publicName;
                    else
                        expression = t => t.ispublic == 1;
                    dormitory = _tb_Building_Room_ConfigService.FindPublicArea(pageIndex, pageSize, ref totalRecordNum,
                        expression) as List<tb_building_room_config>;
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    sEcho = sEcho,
                    iTotalRecords = totalRecordNum,
                    iTotalDisplayRecords = totalRecordNum,
                    data = dormitory
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }

        /// <summary>
        /// 查询宿舍信息 条件: ispublic == 0
        /// </summary>
        /// <returns></returns>
        [HeaderParams(nameof(HeadParamsType.SchoolCode))]
        [HttpGet]
        public JsonResult GetDormitory()
        {
            try
            {
                //从请求Http Headers中获取学校编码
                var schoolCode = string.Empty;
                schoolCode = GetSchoolCode(schoolCode);
                var dormitory = new List<tb_building_room_config>();
                if (!string.IsNullOrEmpty(schoolCode))
                {
                    dormitory = _tb_Building_Room_ConfigService.FindListByClause(
                                t => t.school_id == SqlFunc.ToInt32(schoolCode) && t.ispublic == 0,
                                t => t.id) as List<tb_building_room_config>;
                    //如果一个节点也没有,插入根节点,以学校名称为节点名称
                    if (dormitory != null && dormitory.Count == 0)
                    {
                        var schoolInfo = _tb_school_InfoService.FindByClause(t => t.School_Code == schoolCode);
                        if (schoolInfo != null)
                        {
                            var inserObj = new tb_building_room_config
                            {
                                school_id = int.Parse(schoolCode),
                                building_room_no = schoolInfo.School_name,
                                parent_id = 0,
                                ispublic = 0
                            };
                            _tb_Building_Room_ConfigService.Insert(inserObj);
                            dormitory.Add(inserObj);
                        }
                        //检查二维码配置,没有值就插入默认值
                        var qrCodeConfig = _tb_QRCode_Base_ConfigService.FindByClause(t => t.school_id == SqlFunc.ToInt32(schoolCode));
                        if (qrCodeConfig == null)
                        {
                            _tb_QRCode_Base_ConfigService.Insert(new tb_qrcode_base_config
                            {
                                school_id = int.Parse(schoolCode),
                                user_key = QRCodeDefaultConfig.user_key,
                                door_limit = QRCodeDefaultConfig.door_limit,
                                door_openusetime = QRCodeDefaultConfig.door_openusetime,
                                door_openstyle = QRCodeDefaultConfig.door_openstyle,
                                door_opentime = QRCodeDefaultConfig.door_opentime,
                                extend_field = QRCodeDefaultConfig.extend_field
                            });
                        }
                    }

                    //学校节点取最新的学校名称
                    if (dormitory != null && dormitory.Count > 0)
                    {
                        var schoolObj = (from obj in dormitory
                                         where obj.school_id == int.Parse(schoolCode) &&
                                               obj.parent_id == 0 &&
                                               obj.ispublic == 0
                                         select obj).FirstOrDefault();
                        if (schoolObj != null)
                        {
                            var schoolInfo = _tb_school_InfoService.FindByClause(t => t.School_Code == schoolCode);
                            if (schoolObj.building_room_no != schoolInfo.School_name)
                            {
                                schoolObj.building_room_no = schoolInfo.School_name;
                                _tb_Building_Room_ConfigService.Update(schoolObj);
                            }
                        }
                    }
                }
                else
                    dormitory = _tb_Building_Room_ConfigService.FindListByOrder(t => t.id) as List<tb_building_room_config>;

                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = dormitory
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }

        /// <summary>
        /// 添加宿舍信息
        /// </summary>
        /// <returns></returns>
        [HeaderParams(nameof(HeadParamsType.SchoolCode))]
        [HttpPost]
        public JsonResult AddDormitory([FromBody]JObject obj)
        {
            try
            {
                //从请求Http Headers中获取学校编码
                var schoolCode = string.Empty;
                schoolCode = GetSchoolCode(schoolCode);
                long inertReturn = 0;
                var insertObj = new tb_building_room_config
                {
                    building_room_no = obj["name"] + "",
                    parent_id = int.Parse(obj["pid"] + ""),
                    ispublic = 0
                };
                if (obj.ContainsKey("isPublic"))
                    insertObj.ispublic = int.Parse(obj["isPublic"] + "");

                if (!string.IsNullOrEmpty(schoolCode))
                {
                    insertObj.school_id = int.Parse(schoolCode);
                    inertReturn = _tb_Building_Room_ConfigService.Insert(insertObj);
                }
                else
                    inertReturn = _tb_Building_Room_ConfigService.Insert(insertObj);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
                    data = inertReturn
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }

        /// <summary>
        /// 删除宿舍信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DeleteDormitory(string id)
        {
            try
            {
                var _id = int.Parse(id);
                var boolReturn = _tb_Building_Room_ConfigService.Delete(new tb_building_room_config() { id = _id });
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.DeleteSuccess,
                    data = boolReturn
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.DeleteFail
                });
            }
        }

        /// <summary>
        ///  批量删除宿舍信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteDormitory([FromBody]JObject obj)
        {
            try
            {
                var _ids = obj["ids"].AsJEnumerable();
                var boolReturn = _tb_Building_Room_ConfigService.DeleteByIds(_ids.Select(id => id + "").ToArray());
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.DeleteSuccess,
                    data = boolReturn
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.DeleteFail
                });
            }
        }

        /// <summary>
        /// 更新宿舍信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateDormitory([FromBody]JObject obj)
        {
            try
            {
                var boolReturn = _tb_Building_Room_ConfigService.UpdateColumnsById(
                    new tb_building_room_config { id = int.Parse(obj["id"] + ""), building_room_no = obj["name"] + "" }, it => new { it.building_room_no });
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess,
                    data = boolReturn
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UpdateFail
                });
            }
        }

        /// <summary>
        /// 得到未分配房间号人员
        /// </summary>
        /// <returns></returns>
        [HeaderParams(nameof(HeadParamsType.SchoolCode))]
        [HttpGet]
        public JsonResult GetUnBandingDormitory(int sEcho,
            int iDisplayStart,
            int iDisplayLength,
            string floor_no,
            string room_no,
            string studentIdentity)
        {
            try
            {
                int pageStart = iDisplayStart;
                int pageSize = iDisplayLength;
                int pageIndex = (pageStart / pageSize) + 1;
                int totalRecordNum = default(int);
                //从请求Http Headers中获取学校编码
                var schoolCode = string.Empty;
                schoolCode = GetSchoolCode(schoolCode);
                var pageRecords = _banding_dormitoryService.FindUnBandingDormitory(pageIndex, pageSize, ref totalRecordNum, schoolCode, floor_no, room_no, studentIdentity);
                return Json(new
                {
                    code = JsonReturnMsg.GetSuccess,
                    sEcho = sEcho,
                    iTotalRecords = totalRecordNum,
                    iTotalDisplayRecords = totalRecordNum,
                    aaData = pageRecords
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }

        /// <summary>
        /// 得到已分配房间号人员
        /// </summary>
        /// <returns></returns>
        [HeaderParams(nameof(HeadParamsType.SchoolCode))]
        [HttpGet]
        public JsonResult GetBandingDormitory(int sEcho,
            int iDisplayStart,
            int iDisplayLength,
            string floor_no,
            string room_no,
            string studentIdentity)
        {
            try
            {
                int pageStart = iDisplayStart;
                int pageSize = iDisplayLength;
                int pageIndex = (pageStart / pageSize) + 1;
                int totalRecordNum = default(int);
                //从请求Http Headers中获取学校编码
                var schoolCode = string.Empty;
                schoolCode = GetSchoolCode(schoolCode);
                var pageRecords = _banding_dormitoryService.FindBandingDormitory(pageIndex, pageSize, ref totalRecordNum, schoolCode, floor_no, room_no, studentIdentity);
                foreach (BandingDormitory dorm in pageRecords)
                {
                    dorm.access_code = dorm.access_code.PadLeft(8, '0');
                }
                return Json(new
                {
                    code = JsonReturnMsg.GetSuccess,
                    sEcho = sEcho,
                    iTotalRecords = totalRecordNum,
                    iTotalDisplayRecords = totalRecordNum,
                    aaData = pageRecords
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }

        /// <summary>
        /// 绑定楼栋房间到学校人员
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HeaderParams(nameof(HeadParamsType.SchoolCode))]
        [HttpPost]
        public JsonResult AddBandingDormitory([FromBody]JObject obj)
        {
            try
            {
                //从请求Http Headers中获取学校编码
                var schoolCode = string.Empty;
                schoolCode = GetSchoolCode(schoolCode);
                var userIds = obj["user_id"].AsJEnumerable();
                long inertReturn = 0;
                var inserList = new List<tb_school_user_room>();
                foreach (JToken userId in userIds)
                {
                    var insertObj = new tb_school_user_room
                    {
                        school_code = int.Parse(schoolCode),
                        user_id = userId + "",
                        floor_no = int.Parse(obj["floor_no"] + ""),
                        room_no = int.Parse(obj["room_no"] + "")
                    };
                    if (!string.IsNullOrEmpty(schoolCode))
                        insertObj.school_code = int.Parse(schoolCode);
                    inserList.Add(insertObj);
                }
                _tb_School_User_RoomService.Insert(inserList);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
                    data = inertReturn
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }

        /// <summary>
        ///  删除楼栋房间和学校人员绑定关系
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteBandingDormitory([FromBody]JObject obj)
        {
            try
            {
                var _ids = obj["ids"].AsJEnumerable();
                var boolReturn = _tb_School_User_RoomService.DeleteByIds(_ids.Select(id => id + "").ToArray());
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.DeleteSuccess,
                    data = boolReturn
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.DeleteFail
                });
            }
        }

        /// <summary>
        /// 导入宿舍关系树
        /// </summary>
        /// <returns></returns>
        [HeaderParams(nameof(HeadParamsType.SchoolCode),
            nameof(HeadParamsType.FileUpload))]
        [HttpPost]
        public ActionResult UploadDormitory()
        {
            //从请求Http Headers中获取学校编码
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
                    FileHelper.CreateFiles(TempPath, true);
                    filePath = TempPath + $@"{fileNewName}.{fileExt}";
                    //保存文件到临时文件夹下
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    //读取文件并导入
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
                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                            {
                                IRow row = sheet.GetRow(i);
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    if (!string.IsNullOrEmpty(row.GetCell(j) + ""))
                                    {
                                        var dormArr = (row.GetCell(j) + "").Split('/');
                                        if (dormArr.Length > 2)
                                        {
                                            return Json(new
                                            {
                                                code = JsonReturnMsg.FailCode,
                                                msg = JsonReturnMsg.ImportFail + $":第{i + 1}行超过了两层关系,请跟模板格式保持一致"
                                            });
                                        }
                                    }
                                }
                            }

                            //遍历excel sheet
                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                            {
                                IRow row = sheet.GetRow(i);
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    if (!string.IsNullOrEmpty(row.GetCell(j) + ""))
                                    {
                                        var dormArr = (row.GetCell(j) + "").Split('/');
                                        string pid = string.Empty;
                                        for (int index = 0; index < dormArr.Length; index++)
                                        {
                                            if (index == 0)
                                            {
                                                var findObj = _tb_Building_Room_ConfigService.FindByClause(t => t.school_id == int.Parse(schoolCode) && t.parent_id == 0);
                                                if (findObj != null)
                                                {
                                                    pid = findObj.id + "";
                                                }
                                            }
                                            var dorm = dormArr[index];
                                            var splitParentId = long.Parse(pid.Split('/')[pid.Split('/').Length - 1]);
                                            var listDorm = _tb_Building_Room_ConfigService.FindListByClause(t => t.school_id == int.Parse(schoolCode) &&
                                                            t.building_room_no == dorm &&
                                                            t.parent_id == splitParentId,
                                                            t => t.id);
                                            if (index == 0 && !listDorm.Any())
                                            {
                                                dorm = dormArr[index];
                                                var insertId = _tb_Building_Room_ConfigService.Insert(
                                                     new tb_building_room_config
                                                     {
                                                         school_id = int.Parse(schoolCode),
                                                         building_room_no = dorm,
                                                         parent_id = long.Parse(pid),
                                                         ispublic = 0
                                                     });
                                                pid += "/" + insertId;
                                            }
                                            else if (index > 0 && !listDorm.Any())
                                            {
                                                dorm = dormArr[index];
                                                var insertId = _tb_Building_Room_ConfigService.Insert(
                                                    new tb_building_room_config
                                                    {
                                                        school_id = int.Parse(schoolCode),
                                                        building_room_no = dorm,
                                                        parent_id = int.Parse(pid.Split('/')[pid.Split('/').Length - 1]),
                                                        ispublic = 0
                                                    });
                                                pid += "/" + insertId;
                                            }
                                            else
                                            {
                                                dorm = dormArr[index];
                                                var findObj = _tb_Building_Room_ConfigService.FindByClause(t => t.school_id == int.Parse(schoolCode) &&
                                                            t.building_room_no == dorm);
                                                if (findObj != null)
                                                {
                                                    pid += "/" + findObj.id;
                                                }
                                            }
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
                    msg = JsonReturnMsg.UploadSuccess
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UploadFail
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
        /// 导入宿舍分配的人员
        /// </summary>
        /// <returns></returns>
        [HeaderParams(nameof(HeadParamsType.SchoolCode), 
            nameof(HeadParamsType.FileUpload))]
        [HttpPost]
        public ActionResult UploadBandingDormitory()
        {
            //从请求Http Headers中获取学校编码
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
                    FileHelper.CreateFiles(TempPath, true);
                    filePath = TempPath + $@"{fileNewName}.{fileExt}";
                    //保存文件到临时文件夹下
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    //读取文件并导入
                    using (FileStream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        var workbook = new XSSFWorkbook(stream);
                        var sheetNum = workbook.NumberOfSheets;
                        if (sheetNum > 0 && !string.IsNullOrEmpty(schoolCode))
                        {
                            var schoolRoomList = _tb_Building_Room_ConfigService.FindListByClause(t => t.school_id == int.Parse(schoolCode), t => t.id);
                            var schoolUserList = _tb_school_userService.FindListByClause(t => t.school_id == schoolCode, t => t.user_id);

                            var sheet = workbook.GetSheetAt(0);
                            //获取sheet的首行
                            var headerRow = sheet.GetRow(0);
                            //获取sheet的最后一列
                            var cellCount = headerRow.LastCellNum;
                            //获取sheet的最后一行
                            var rowCount = sheet.LastRowNum;
                            //遍历excel sheet
                            var insertList = new List<tb_school_user_room>();

                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                            {
                                IRow row = sheet.GetRow(i);
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    //遍历添加Column的数据
                                    if (row.GetCell(j) != null)
                                    {
                                        switch (j)
                                        {
                                            //学工号
                                            case 0:
                                                {
                                                    var findUser = schoolUserList.FirstOrDefault(t => t.student_id == (row.GetCell(j) + "").Trim());
                                                    if (findUser == null)
                                                    {
                                                        return Json(new
                                                        {
                                                            code = JsonReturnMsg.FailCode,
                                                            msg = JsonReturnMsg.ImportFail + $":第{i + 1}行不存在此人员信息,请先录入此人员信息!"
                                                        });
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                            }

                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                            {
                                IRow row = sheet.GetRow(i);
                                var insertObj = new tb_school_user_room { school_code = int.Parse(schoolCode) };
                                var floorName = string.Empty;
                                var roomName = string.Empty;
                                var userId = string.Empty;
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    //遍历添加Column的数据
                                    if (row.GetCell(j) != null)
                                    {
                                        switch (j)
                                        {
                                            //学工号
                                            case 0:
                                                {
                                                    var findUser = schoolUserList.FirstOrDefault(t => t.student_id == (row.GetCell(j) + "").Trim());
                                                    if (findUser != null)
                                                    {
                                                        insertObj.user_id = findUser.user_id + "";
                                                        userId = findUser.user_id + "";
                                                    }
                                                }
                                                break;
                                            //楼栋号
                                            case 2:
                                                floorName = (row.GetCell(j) + "").Trim();
                                                break;
                                            //房间号
                                            case 3:
                                                roomName = (row.GetCell(j) + "").Trim();
                                                break;
                                        }
                                    }
                                }
                                var findUserRoom = schoolRoomList.FirstOrDefault(t => t.building_room_no == floorName);
                                if (findUserRoom != null)
                                {
                                    insertObj.floor_no = (int)findUserRoom.id;
                                    findUserRoom = schoolRoomList.FirstOrDefault(t => t.parent_id == insertObj.floor_no && t.building_room_no == roomName);
                                    if (findUserRoom != null)
                                        insertObj.room_no = (int)findUserRoom.id;
                                    else
                                    {
                                        return Json(new
                                        {
                                            code = JsonReturnMsg.FailCode,
                                            msg = JsonReturnMsg.ImportFail + $":第{i}行不存在此楼栋名称或房间名称"
                                        });
                                    }
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        code = JsonReturnMsg.FailCode,
                                        msg = JsonReturnMsg.ImportFail + $":第{i}行不存在此楼栋名称或房间名称"
                                    });
                                }
                                insertList.Add(insertObj);
                                //删除数据,以免导入重复数据
                                _tb_School_User_RoomService.Delete(new tb_school_user_room
                                {
                                    school_code = int.Parse(schoolCode),
                                    user_id = userId,
                                    floor_no = insertObj.floor_no,
                                    room_no = insertObj.room_no
                                });
                            }
                            _tb_School_User_RoomService.Insert(insertList);
                        }
                    }
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UploadSuccess
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UploadFail
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
        /// 按条件获取门禁数据并导出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileContentResult GetEntranceRecord(string schoolCode, string deviceId, string studentIdentity, string startTime, string endTime, string stuffType)
        {
            try
            {
                var records = _view_entrance_recordService.FindAll(deviceId, studentIdentity, startTime, endTime, schoolCode, stuffType);
                if (records == null)
                {
                    return null;
                }
                //导出数据excel
                return ExcelHelper.ExportAction(@"门禁详细信息.xls",
                    new List<string> { "人员ID", "人员姓名", "设备号", "设备名称", "学校名称", "门禁时间", "门禁状态" }, records.ToArray(),
                    FillCell);
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return null;
            }
        }
        /// <summary>
        /// 按条件获取门禁数据并导出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetEntranceRecord2(string schoolCode, string deviceId, string studentIdentity, string startTime, string endTime, string stuffType)
        {
            try
            {
                var records = _view_entrance_recordService.FindAll(deviceId, studentIdentity, startTime, endTime, schoolCode, stuffType);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = records
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 按条件获取门禁数据
        /// </summary>
        /// <param name="schoolCode">学校编码</param>
        /// <param name="deviceId">设备id(可选参数)</param>
        /// <param name="studentIdentity">学工号或姓名(可选参数)</param>
        /// <param name="startTime">检索开始时间(可选参数)</param>
        /// <param name="endTime">检索结束时间(可选参数)</param>
        /// <param name="stuffType"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult GetEntranceResult(string schoolCode, string deviceId, string studentIdentity, string startTime, string endTime, string stuffType)
        {
            try
            {
                //查询套餐
                var records = _view_entrance_recordService.FindAll(deviceId, studentIdentity, startTime, endTime, schoolCode, stuffType);
                if (records == null)
                {
                    return null;
                }
                return Json(new
                {
                    code = JsonReturnMsg.GetSuccess,
                    records
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return null;
            }
        }

        /// <summary>
        /// 按条件获取分页门禁数据
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="schoolCode"></param>
        /// <param name="deviceId"></param>
        /// <param name="studentIdentity"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="stuffType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEntranceResultPage(int sEcho, int iDisplayStart, int iDisplayLength, string schoolCode,
            string deviceId, string studentIdentity, string startTime, string endTime, string stuffType)
        {
            try
            {
                int pageStart = iDisplayStart;
                int pageSize = iDisplayLength;
                int pageIndex = (pageStart / pageSize) + 1;
                int totalRecordNum = default(int);

                var records = _view_entrance_recordService.FindAll(pageIndex, pageSize, ref totalRecordNum,
                    deviceId, studentIdentity,
                    startTime, endTime, schoolCode, stuffType);

                if (records == null || !records.Any())
                {
                    return Json(new
                    {
                        return_code = "990005",
                        return_msg = "暂无数据",
                        sEcho,
                        iTotalRecords = 0,
                        iTotalDisplayRecords = 0,
                        aaData = records
                    });
                }

                return Json(new
                {
                    return_code = "990005",
                    return_msg = "查询成功",
                    sEcho,
                    iTotalRecords = totalRecordNum,
                    iTotalDisplayRecords = totalRecordNum,
                    aaData = records
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return null;
            }
        }

        /*
        /// <summary>
        /// 门禁卡获取查询时间横坐标
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        //[HttpGet]
        //public IActionResult Get_xAxisIncomingFrequencyInfo(string schoolcode,string sTime,string eTime)
        //{
        //    try
        //    {
        //        var schoolInfo = _tb_school_userService.FindByClause(x=>x.school_id == schoolcode);
        //        DateTime stime = Convert.ToDateTime(sTime);
        //        DateTime etime = Convert.ToDateTime(eTime);
        //        int days = (etime - stime).Days;
                
        //        List<string> data = new List<string>();
        //        data.Add(stime.ToString("yyyy/MM/dd"));
        //        if (days!=0)
        //        {
        //            for (int i = 1; i <= days; i++)
        //            {
        //                data.Add(stime.AddDays(i).ToString("yyyy/MM/dd"));
        //            }
        //        }
        //        xAxis _xAxis = new xAxis(data);
        //        //var einfo = _view_entrance_recordService.FindListByClause(x => x.open_time >= stime && x.open_time <= etime && x.entrance_status == "进",t=>t.open_time,OrderByType.Asc);
        //        return Json(new {
        //            code=""
        //            data=_xAxis.data
        //        } );
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("错误:" + ex);
        //        return null;
        //    }
        //}
        */

        /// <summary>
        /// 门禁卡时间段进入频率
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get_seriesIncomingFrequencyInfo(string schoolcode, string sTime, string eTime)
        {
            try
            {
                var schoolInfo = _tb_school_userService.FindByClause(x => x.school_id == schoolcode);
                DateTime stime = Convert.ToDateTime(sTime);
                DateTime etime = Convert.ToDateTime(eTime);
                int days = (etime - stime).Days;

                List<string> data = new List<string>();
                data.Add(stime.ToString("yyyy/MM/dd"));
                if (days != 0)
                {
                    for (int i = 1; i <= days; i++)
                    {
                        data.Add(stime.AddDays(i).ToString("yyyy/MM/dd"));
                    }
                }
                xAxis _xAxis = new xAxis(data);
                stime = Convert.ToDateTime(sTime);
                int n_count = 0;
                List<int> nlist = new List<int>();
                if (days == 0)
                {
                    n_count = _tb_entrance_recordService.GetListInfo(schoolcode, stime.ToString("yyyy-MM-dd"), etime.ToString("yyyy-MM-dd"));
                    nlist.Add(n_count);
                }
                else
                {
                    for (int i = 0; i <= days; i++)
                    {
                        n_count = _tb_entrance_recordService.GetListInfo(schoolcode, stime.AddDays(i).ToString("yyyy-MM-dd"), stime.AddDays(i).ToString("yyyy-MM-dd"));
                        nlist.Add(n_count);
                    }

                }
                List<int> ndataArrTime = new List<int>();
                foreach (var item in nlist)
                {
                    ndataArrTime.Add(0);
                }
                //var einfo = _view_entrance_recordService.FindListByClause(x => x.open_time >= stime && x.open_time <= etime && x.entrance_status == "进",t=>t.open_time,OrderByType.Asc);
                return Json(new
                {
                    code = "00000",
                    dataArrTime = ndataArrTime,
                    dateArr = _xAxis.data,
                    dataArrRate = nlist,
                });

            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return null;
            }
        }



        /// <summary>
        /// 当前图书馆人数
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="devicetype"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get_LibraryCount(string schoolcode, string devicetype)
        {
            try
            {

                int count = _tb_entrance_recordService.Get_LibraryCount(schoolcode, devicetype);
                return Json(new
                {

                    code = "00000",
                    count = count
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return null;
            }
        }

        /// <summary>
        /// 图书馆入馆排行和入馆总人数
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="schoolcode"></param>
        /// <param name="devicetype"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get_LibraryRanking(string stime, string etime, string schoolcode, string devicetype)
        {
            try
            {
                if (string.IsNullOrEmpty(stime) || string.IsNullOrEmpty(etime) || string.IsNullOrEmpty(schoolcode) || string.IsNullOrEmpty(devicetype))
                {
                    return null;
                }
                stime = Convert.ToDateTime(stime).ToString("yyyy-MM-dd");
                etime = Convert.ToDateTime(etime).ToString("yyyy-MM-dd");

                var data = _tb_entrance_recordService.Get_LibraryRanking(stime, etime, schoolcode, devicetype);
                int count = _tb_entrance_recordService.Get_LibraryCount(schoolcode, devicetype);
                return Json(new
                {
                    code = "00000",
                    count = data,
                    allperson = count
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return null;
            }
        }

        /// <summary>
        /// 领卡使用率
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get_dataRate(string schoolcode)
        {
            try
            {
                var dataRate = _tb_entrance_recordService.GetdataRate(schoolcode);
                return Json(new
                {
                    code = "00000",
                    dataRate = dataRate
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return null;
            }
        }

        /// <summary>
        /// 查询学校设备
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DealQueryDevice(string schoolcode)
        {
            var devicelist = _tb_school_device.FindListByClause(t => t.school_id == schoolcode, t => t.id);
            var dropDownList = _mapper.Map<List<tb_school_device>, List<dropDownListModel>>(devicelist.ToList());
            return Json(new
            {
                return_code = 0,
                return_msg = "查询成功",
                data = dropDownList
            });
        }
    }
}
