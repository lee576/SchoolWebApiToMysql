using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using NPOI.SS.UserModel;
using SchoolWebApi.Utility;
using System.IO;
using System.Net.Http;
using System.Drawing;
using System.Net;
using System.Net.Http.Headers;
using Exceptionless.Json.Linq;
using SqlSugar;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 学校人员管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class SchoolUserController : Controller
    {
        /// <summary>
        /// 初始化日志对象
        /// </summary>
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private Itb_school_InfoService _tb_school_InfoService;
        private ISchoolCodrService _schoolcodrservice;
        private Itb_school_userService _tb_school_userService;
        private Itb_school_card_templateService _tb_school_card_templateService;
        private Itb_school_classinfoService _itb_School_ClassinfoService;
        private Itb_school_departmentService _itb_School_DepartmentService;
        private Itb_userinfoService _Itb_userinfoService;
        private Itb_school_departmentinfoService _itb_school_departmentinfoService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_school_InfoService"></param>
        /// <param name="schoolcodrservice"></param>
        /// <param name="tb_school_userService"></param>
        /// <param name="tb_school_card_templateService"></param>
        /// <param name="tb_School_ClassinfoService"></param>
        /// <param name="tb_School_DepartmentinfoService"></param>
        /// <param name="tb_userinfoService"></param>
        /// <param name="tb_school_departmentinfoService"></param>
        public SchoolUserController(Itb_school_InfoService tb_school_InfoService,
            ISchoolCodrService schoolcodrservice,
            Itb_school_userService tb_school_userService,
            Itb_school_card_templateService tb_school_card_templateService,
            Itb_school_classinfoService tb_School_ClassinfoService,
            Itb_school_departmentService tb_School_DepartmentinfoService,
            Itb_userinfoService tb_userinfoService,
            Itb_school_departmentinfoService tb_school_departmentinfoService)
        {
            _tb_school_InfoService = tb_school_InfoService;
            _schoolcodrservice = schoolcodrservice;
            _tb_school_userService = tb_school_userService;
            _tb_school_card_templateService = tb_school_card_templateService;
            _itb_School_ClassinfoService = tb_School_ClassinfoService;
            _itb_School_DepartmentService = tb_School_DepartmentinfoService;
            _Itb_userinfoService = tb_userinfoService;
            _itb_school_departmentinfoService = tb_school_departmentinfoService;
        }
        #region 按分院，系，班级查询
        /// <summary>
        /// 按分院，系，班级查询
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="classid"></param>
        /// <param name="level"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="isCord"></param>
        /// <param name="cardtype"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolUserInfo(string schoolcode,int classid,int level, int iDisplayStart, int iDisplayLength,
            string isCord = "",string cardtype = "")
        {
            try
            {
                int pageStart = iDisplayStart;
                int pageSize = iDisplayLength;
                int pageIndex = pageStart;
                int totalRecordNum = 0;

                var departmentTree = _itb_School_DepartmentService.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, OrderByType.Asc);
                var classinfo = _itb_School_ClassinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);
                var departmentInfo = _itb_school_departmentinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);
                var schooluserinfo = _tb_school_userService.FindListByClause(x => x.school_id == schoolcode, t => t.user_id, OrderByType.Asc) as List<tb_school_user>;
                if (!string.IsNullOrWhiteSpace(isCord))
                {
                    schooluserinfo = schooluserinfo.Where(x => x.card_state == Convert.ToInt32(isCord)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(cardtype))
                {
                    schooluserinfo = schooluserinfo.Where(x => x.class_id == Convert.ToInt32(cardtype)).ToList();
                }
                //var tree0 = departmentTree.Where(x => x.treeLevel == 0);
                //var tree1 = departmentTree.Where(x => x.treeLevel == 1);
                //var tree2 = departmentTree.Where(x => x.treeLevel == 2);
                //var tree3 = departmentTree.Where(x => x.treeLevel == 3);
                List<tb_school_user> data = new List<tb_school_user>();
                if (level==0)
                {
                    totalRecordNum += schooluserinfo.Count();
                    data = schooluserinfo.Skip(pageIndex * pageSize).Take(iDisplayLength).ToList();
                }
                if (level==1)
                {
                    var depdata = departmentTree.Where(x => x.treeLevel == 1 && x.id == classid).ToList();
                    //获取到系，在获取到班级id
                    foreach (var item in depdata)//系
                    {
                        var depnode = departmentTree.Where(x => x.p_id == item.id).ToList();
                        foreach (var item2 in depnode)//班级
                        {
                            var depnode2 = departmentTree.Where(x => x.p_id == item2.id).ToList();
                            foreach (var item3 in depnode2)
                            {
                                if (Convert.ToBoolean(item3.isType))//老师
                                {
                                    var class_id = departmentInfo.Where(x => x.department_treeID == item3.id).ToList()[0].ID;
                                    var data2 = schooluserinfo.Where(x => x.department_id == class_id);
                                    data = data.Union(data2).ToList();
                                    totalRecordNum += data.Count();
                                }
                                else
                                {
                                    var class_id = classinfo.Where(x => x.department_classID == item3.id).ToList()[0].ID;
                                    var data2 = schooluserinfo.Where(x => x.department_id == class_id);
                                    data = data.Union(data2).ToList();
                                    totalRecordNum += data.Count();
                                }
                            }
                        }
                    }
                    data = data.Skip(pageIndex * pageSize).Take(iDisplayLength).ToList();
                }
                if (level==2)//系
                {
                    var depdata = departmentTree.Where(x => x.treeLevel == 2 && x.id == classid).ToList();
                    //获取到系，在获取到班级id
                    foreach (var item in depdata)//班
                    {
                        var depnode = departmentTree.Where(x => x.p_id == item.id).ToList();
                        foreach (var item2 in depnode)//班级
                        {
                            var depnode2 = departmentTree.Where(x => x.p_id == item2.id).ToList();
                            if (Convert.ToBoolean(item2.isType))//老师
                            {
                                var class_id = departmentInfo.Where(x => x.department_treeID == item2.id).ToList()[0].ID;
                                var data2 = schooluserinfo.Where(x => x.department_id == class_id);
                                data = data.Union(data2).ToList();
                                totalRecordNum += data.Count();
                            }
                            else
                            {
                                var class_id = classinfo.Where(x => x.department_classID == item2.id).ToList()[0].ID;
                                var data2 = schooluserinfo.Where(x => x.department_id == class_id);
                                data = data.Union(data2).ToList();
                                totalRecordNum += data.Count();
                                
                            }
                        }
                    }
                    data = data.Skip(pageIndex * pageSize).Take(iDisplayLength).ToList();
                }
                if (level==3)
                {
                    var data2 = schooluserinfo.Where(x => x.department_id == classid).ToList();
                    totalRecordNum += data2.Count();
                    data = data2.Skip(pageIndex * pageSize).Take(iDisplayLength).ToList();
                    
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    iTotalRecords = totalRecordNum,
                    iTotalDisplayRecords = totalRecordNum,
                    data = data
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

        #endregion

        #region 电子校园卡概论——获取学校名称
        /// <summary>
        /// 电子校园卡概论——获取学校名称
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSchoolNameByschoolcode(string schoolcode)
        {
            try
            {
                var schoolInfo = _tb_school_InfoService.FindByClause(a => a.School_Code == schoolcode);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    aaData = new { schoolCode = schoolInfo.School_Code, schoolName = schoolInfo.School_name }
                });
            }
            catch (Exception e)
            {
                Log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }

        }
        #endregion

        #region 电子校园卡概论——修改学校名称
        /// <summary>
        /// 电子校园卡概论——修改学校名称
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="schoolName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateSchoolNameByschoolcode(string schoolcode, string schoolName)
        {
            try
            {
                var boolres = _tb_school_InfoService.UpdateColumnsByConditon(a => new tb_school_info
                { School_name = schoolName }, a => a.School_Code == schoolcode);
                if (boolres)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        msg = JsonReturnMsg.UpdateSuccess,
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.UpdateFail,
                    });
                }

            }
            catch (Exception e)
            {
                Log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }

        }
        #endregion

        #region 电子校园卡概论——获取学校领卡信息
        /// <summary>
        /// 电子校园卡概论——获取学校领卡信息
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult FindSchoolCardInfo(string schoolcode)
        {
            try
            {
                var resCardInfo = _tb_school_InfoService.FindSchoolCardInfoBySchoolCode(schoolcode);
                var resgrowthInfo = _tb_school_InfoService.FindSchoolCardGrowth(schoolcode);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    aaData = new { CardInfo = resCardInfo, GrowthInfo = resgrowthInfo }
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        #endregion

        #region 获取校园卡人员管理信息
        /// <summary>
        /// 获取校园卡人员管理信息
        /// </summary>
        /// <param name="school_Code"></param>
        /// <param name="userNameOrId"></param>
        /// <param name="branchId"></param>
        /// <param name="departmentId"></param>
        /// <param name="department_classId"></param>
        /// <param name="classId"></param>
        /// <param name="card_state"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="iDisplayStart"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult FindSchoolUserInfo(string school_Code,string userNameOrId,string branchId,string departmentId,
            string department_classId,string classId,string card_state,string iDisplayLength,string iDisplayStart)
        {
            try
            {
                int pageStart = int.Parse(iDisplayStart);
                int pageSize = int.Parse(iDisplayLength);
                int pageIndex = (pageStart / pageSize) + 1;
                int totalRecordNum = default(int);
                var resList = _tb_school_userService.FindSchoolStudentInfo(school_Code, pageIndex, pageSize, ref totalRecordNum, userNameOrId, branchId,
                   departmentId, department_classId, classId, card_state);
                //var resList = _tb_school_userService.FindSchoolStudentInfo(schoolStudent.school_Code, schoolStudent.userNameOrId, schoolStudent.branchId,
                //    schoolStudent.departmentId, schoolStudent.department_classId, schoolStudent.classId, schoolStudent.card_state, pageIndex, pageIndex, ref totalRecordNum);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    iTotalRecords = totalRecordNum,
                    iTotalDisplayRecords = totalRecordNum,
                    aaData = resList
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        #endregion

        #region 根据学校编号获取班级总数及人总数
        /// <summary>
        /// 根据学校编号获取所有的分院信息
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="class_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSchoolPeopleClassCount(string schoolcode, string class_id)
        {
            try
            {
                int peopleCount = 0;
                int classCount = 0;
                if (!string.IsNullOrEmpty(class_id) && class_id == "1")
                {
                    //学校学生总人数
                    peopleCount = _tb_school_userService.Count(a => a.school_id == schoolcode && a.class_id == int.Parse(class_id));
                    //班级总数
                    classCount = _itb_School_ClassinfoService.Count(a => a.schoolcode == schoolcode);
                }
                else
                {
                    //学校总人数
                    peopleCount = _tb_school_userService.Count(a => a.school_id == schoolcode);
                    //classCount = _itb_School_ClassinfoService.Count(a => a.schoolcode == schoolcode);
                }

                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess,
                    aaData = new { PeopleCount = peopleCount, ClassCount = classCount }
                });
            }
            catch (Exception e)
            {
                Log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }

        }
        #endregion

        #region 根据学校编号获取所有的分院信息
        /// <summary>
        /// 根据学校编号获取所有的分院信息
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSchoolBranchInfo(string schoolcode)
        {
            try
            {
                //根据校园编号获取所有的院系信息
                var resList = _itb_School_DepartmentService.FindListByClause(a => a.schoolcode == schoolcode, a => a.id);
                //获取所有的分院信息
                var branchInfo = resList.Select(a => a.treeLevel == 1).ToList();
                //学校总人数
                var peopleCount = _tb_school_userService.Count(a => a.school_id == schoolcode);
                //班级总数
                var classCount = _itb_School_ClassinfoService.Count(a => a.schoolcode == schoolcode);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess,
                    aaData = new { BranchInfo = branchInfo, PeopleCount = peopleCount, ClassCount = classCount }
                });
            }
            catch (Exception e)
            {
                Log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }

        }
        #endregion

        #region 获取分院的系信息
        /// <summary>
        /// 获取分院的系信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSchoolDmpInfo(int id)
        {
            try
            {
                //获取所有的系
                var dmpInfo = _itb_School_DepartmentService.FindListByClause(a => a.treeLevel == 2 && a.id == id, a

=> a.id).ToList();
                //获取所有的班级信息
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess,
                    aaData = dmpInfo
                });
            }
            catch (Exception e)
            {
                Log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }

        }
        #endregion

        #region 获取系的班级信息
        /// <summary>
        /// 获取系的班级信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSchoolDmpClassInfo(int id)
        {
            try
            {
                //获取所有的班级信息
                var dmpClassInfo = _itb_School_DepartmentService.FindListByClause(a => a.treeLevel == 3 && a.id ==

id, a => a.id).ToList();

                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess,
                    aaData = dmpClassInfo
                });
            }
            catch (Exception e)
            {
                Log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }

        }
        #endregion

        #region 根据学校编号获取校园卡列表
        /// <summary>
        /// 根据学校编号获取校园卡列表
        /// </summary>
        /// <param name="School_ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSchoolCardList(string School_ID)
        {
            try
            {
                //获取所有的班级信息
                var cardInfo = _tb_school_card_templateService.GetCardListtype(School_ID);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess,
                    data = cardInfo
                });
            }
            catch (Exception e)
            {
                Log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }

        }
        #endregion

        #region 添加或编辑学校人员信息
        /// <summary>
        /// 添加或编辑学校人员信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrUpdateSchoolUserInfo([FromBody]JObject obj)
        {
            try
            {
                string userId = obj["userId"] + "";
                string schoolcode = obj["schoolcode"] + "";
                string class_id = obj["class_id"] + "";
                string card_add_id = obj["card_add_id"] + "";
                string student_id = obj["student_id"] + "";
                string user_name = obj["user_name"] + "";
                string passport = obj["passport"] + "";
                string department_id = obj["department_id"] + "";
                string card_validity = obj["card_validity"] + "";
                string welcome_flg = obj["welcome_flg"] + "";
                string card_id = obj["card_id"] + "";
                //AddOrUpdateSchoolUserRequest schoolUserRequest
                var isExistSid = _tb_school_userService.Any(t => t.student_id == student_id);
                if (isExistSid)
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        string sValue = _tb_school_userService.FindByClause(t => t.user_id == int.Parse(userId)).student_id;
                        if (sValue != student_id)
                        {
                            return Json(new
                            {
                                code = "111111",
                                msg = "学工号已存在，请重新输入！"
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            code = "111111",
                            msg = " 学工号已存在，请重新输入！"
                        });
                    }
                }
                var isExist = _tb_school_userService.Any(t => t.passport == passport);
                if (isExist)
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        string pValue = _tb_school_userService.FindByClause(t => t.user_id == int.Parse(userId)).passport;
                        if (pValue != passport)
                        {
                            return Json(new
                            {
                                code = "111111",
                                msg = "身份证号已存在，请重新输入！"
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            code = "111111",
                            msg = " 身份证号已存在，请重新输入！"
                        });
                    }
                }
                tb_school_user _School_User = new tb_school_user();
                _School_User.school_id = schoolcode;
                _School_User.class_id = int.Parse(class_id);
                _School_User.student_id = student_id;
                _School_User.user_name = user_name;
                _School_User.passport = passport;
                _School_User.department_id = int.Parse(department_id);
                _School_User.card_validity = DateTime.Parse(card_validity);
                _School_User.welcome_flg = Byte.Parse(welcome_flg);
                _School_User.card_add_id = int.Parse(card_add_id);
                _School_User.card_id = int.Parse(card_id);
                if (_School_User.department_id != null)
                {
                    if (class_id == "2")
                    {
                        var classInfo = _itb_school_departmentinfoService.FindByClause(a => a.ID == _School_User.department_id);
                        var schoolName = _itb_School_DepartmentService.FindByClause(a => a.schoolcode == schoolcode && a.treeLevel == 0)?.name;
                        var branchValue = _itb_School_DepartmentService.FindByClause(a => a.id == classInfo.BranchID)?.name;
                        var departmentValue = _itb_School_DepartmentService.FindByClause(a => a.id == classInfo.departmentID)?.name;
                        var demClassValue = _itb_School_DepartmentService.FindByClause(a => a.id == classInfo.department_treeID)?.name;
                        _School_User.department = schoolName + "/" + branchValue + "/" + departmentValue + "/" + demClassValue;
                    }
                    else
                    {
                        var classInfo = _itb_School_ClassinfoService.FindByClause(a => a.ID == _School_User.department_id);
                        var schoolName = _itb_School_DepartmentService.FindByClause(a => a.schoolcode == schoolcode && a.treeLevel == 0)?.name;
                        var branchValue = _itb_School_DepartmentService.FindByClause(a => a.id == classInfo.BranchID)?.name;
                        var departmentValue = _itb_School_DepartmentService.FindByClause(a => a.id == classInfo.DepartmentID)?.name;
                        var demClassValue = _itb_School_DepartmentService.FindByClause(a => a.id == classInfo.department_classID)?.name;
                        _School_User.department = schoolName + "/" + branchValue + "/" + departmentValue + "/" + demClassValue;
                    }

                }
                if (!string.IsNullOrEmpty(userId))
                {

                    bool boolres = _tb_school_userService.UpdateColumnsByConditon(a => new tb_school_user
                    {
                        class_id = _School_User.class_id,
                        student_id = _School_User.student_id,
                        user_name = _School_User.user_name,
                        passport = _School_User.passport,
                        department_id = _School_User.department_id,
                        card_validity = _School_User.card_validity,
                        welcome_flg = _School_User.welcome_flg,
                        department = _School_User.department,
                        card_id = _School_User.card_id
                    }, a => a.user_id == int.Parse(userId));
                    if (boolres)
                    {

                        //向喜云推送数据
                        SchoolCodrController cc = new SchoolCodrController(_tb_school_InfoService,null,_schoolcodrservice,_tb_school_userService,null,null,null,null,null);
                        cc.PushSchoolCard("2", "2", userId + "");
                        return Json(new
                        {
                            code = JsonReturnMsg.SuccessCode,
                            msg = JsonReturnMsg.UpdateSuccess
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            code = JsonReturnMsg.GetFail,
                            msg = JsonReturnMsg.UpdateFail
                        });
                    }
                }
                else
                {
                    _School_User.create_time = DateTime.Now; ;
                    _School_User.nationality = "1";
                    _School_User.class_code = "0";
                    _School_User.card_state = 0;
                    var addres = _tb_school_userService.Insert(_School_User);
                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        msg = JsonReturnMsg.AddSuccess
                    });
                }

            }
            catch (Exception e)
            {
                Log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }

        }
        #endregion

        #region 删除人员信息
        /// <summary>
        /// 删除人员信息
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSchoolUserBy(int user_id)
        {
            try
            {
                


                var boolres = _tb_school_userService.Delete(a => a.user_id == user_id);
                if (boolres)
                {
                    //向喜云推送数据
                    SchoolCodrController cc = new SchoolCodrController(_tb_school_InfoService, null, _schoolcodrservice, _tb_school_userService, null, null, null, null, null);
                    cc.PushSchoolCard("2", "3", user_id + "");

                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        msg = JsonReturnMsg.DeleteSuccess,
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.DeleteFail,
                    });
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.DeleteFail,
                });
            }

        }
        #endregion

        #region 导出校园卡人员管理信息
        /// <summary>
        /// 导出校园卡人员管理信息
        /// </summary>
        /// <param name="school_Code"></param>
        /// <param name="userNameOrId"></param>
        /// <param name="branchId"></param>
        /// <param name="departmentId"></param>
        /// <param name="department_classId"></param>
        /// <param name="classId"></param>
        /// <param name="card_state"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InduceSchoolUserInfo(string school_Code, string userNameOrId, string branchId, string departmentId, string department_classId, string classId, string card_state)
        {
            try
            {

                var data = _tb_school_userService.InduceSchoolUserInfo(school_Code, userNameOrId, branchId,
                    departmentId, department_classId, classId, card_state);
                var info = _tb_school_card_templateService.FindAll();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        item.class_id = info.Where(a => a.Card_add_ID == int.Parse(item.class_id)).FirstOrDefault()?.card_show_name;
                        if (item.card_state == "1")
                        {
                            item.card_state = "已领卡";
                        }
                        else
                        {
                            item.card_state = "未领卡";
                        }
                        if (item.welcome_flg == 1)
                        {
                            item.welcome_flgName = "迎新";
                        }
                        else
                        {
                            item.welcome_flgName = "不迎新";
                        }
                    }
                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        msg = JsonReturnMsg.GetSuccess,
                        aaData = data
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail,
                        aaData = "暂无数据可导出！"
                    });
                }


                //  ExcelHelper.ExportAction(@"人员管理数据.xls",
                //new List<string> { "姓名", "身份证", "学号", "创建时间", "卡类型", "领卡状态", "部门" }, data.ToArray(),
                //FillCell);
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return null;
            }
        }
        //private void FillCell(IRow row, InduceSchoolUserViewModel[] list, int i)
        //{
        //    row.CreateCell(0).SetCellValue(list[i].user_name);
        //    row.CreateCell(1).SetCellValue(list[i].passport);
        //    row.CreateCell(2).SetCellValue(list[i].student_id);
        //    try
        //    {
        //        row.CreateCell(3).SetCellValue(DateTime.Parse(list[i].create_time + "").ToString("yyyy-MM-dd HH:mm:ss"));
        //    }
        //    catch (Exception)
        //    {
        //        row.CreateCell(3).SetCellValue("");
        //    }
        //    row.CreateCell(4).SetCellValue(list[i].class_id);
        //    row.CreateCell(5).SetCellValue(list[i].card_state);
        //    row.CreateCell(6).SetCellValue(list[i].department);


        //}
        #endregion

        #region 根据学生id获取学生信息
        /// <summary>
        /// 根据学生id获取学生信息
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSchoolUserInfoByid(int user_id)
        {
            try
            {
                var schoolUserInfo = _tb_school_userService.FindByClause(a => a.user_id == user_id);
                var sp = (schoolUserInfo.department+"/"+ schoolUserInfo.department_id).Split('/');
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess,
                    aaData = schoolUserInfo,
                    sp = sp
                });
            }
            catch (Exception e)
            {
                Log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }

        }
        #endregion

        #region 用户登陆
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="school_id"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserLogin(string school_id, string username, string password)
        {
            string schoolcode = school_id;
            string Username = username;
            string Password = MD5Helper.MD5Encrypt16(password);
            var userinfo = _Itb_userinfoService.FindByClause(p => p.loginuser == Username && p.password == Password && p.schoolcode == schoolcode);
            if (userinfo == null)
            {
                return Json(new
                {
                    return_code = 1,
                    return_msg = "用户名或密码不正确"
                });

            }
            else
            {
                return Json(new
                {
                    schoolcode = schoolcode,
                    loginuser = userinfo.loginuser,
                    menus = userinfo.menus,
                    return_code = 1,
                    return_msg = "登录成功"

                });
            }
        }

        #endregion

        #region 图片验证码
        /// <summary>
        /// 图片验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ValidImg(string validateNum)
        {
            Random r = new Random();
            //  string validateNum = r.Next(9999).ToString();//生成4位验证码随机字符串
            return CreateImage2(validateNum);//将生成的随机字符串绘成图片
        }

        private Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            int int_Red = RandomNum_First.Next(210);
            int int_Green = RandomNum_Sencond.Next(180);
            int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);// 5+1+a+s+p+x 
        }

        private IActionResult CreateImage2(string validateNum)
        {
            if (validateNum == null || validateNum.Trim() == String.Empty)
                return null;
            string[] fonts = { "Arial", "Georgia" };
            int letterWidth = 16;//单个字体的宽度范围 
            int letterHeight = 22;//单个字体的高度范围
            int int_ImageWidth = validateNum.Length * letterWidth;
            Random newRandom = new Random();
            Bitmap image = new Bitmap(int_ImageWidth, letterHeight);
            Graphics g = Graphics.FromImage(image);
            //生成随机生成器  
            Random random = new Random();  //白色背景  
            g.Clear(Color.White);  //画图片的背景噪音线  
            for (int i = 0; i < 10; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            //画图片的前景噪音点  
            for (int i = 0; i < 10; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            //随机字体和颜色的验证码字符  
            int findex;
            for (int int_index = 0; int_index < validateNum.Length; int_index++)
            {
                findex = newRandom.Next(fonts.Length - 1);
                string str_char = validateNum.Substring(int_index, 1);
                Brush newBrush = new SolidBrush(GetRandomColor());
                Point thePos = new Point(int_index * letterWidth + 1 + newRandom.Next(3), 1 + newRandom.Next(3));
                //5+1+a+s+p+x   
                g.DrawString(str_char, new Font(fonts[findex], 12, FontStyle.Bold), newBrush, thePos);
            }
            //灰色边框  
            g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, int_ImageWidth - 1, (letterHeight - 1));
            //图片扭曲  
            //image = TwistImage(image, true, 3, 4);  
            //将生成的图片发回客户端  

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //将图像保存到指定的流
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            var response = File(ms.ToArray(), "image/jpeg");
            return response;
        }
        #endregion


    }
}