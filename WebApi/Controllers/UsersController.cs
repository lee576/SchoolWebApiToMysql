﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using DbModel;
using Exceptionless.Json;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Models.ViewModels;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RestSharp;
using SchoolWebApi.Utility;
using SqlSugar;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 获取用户信息
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class UsersController : Controller
    {
        /// <summary>
        /// 初始化日志对象
        /// </summary>
        private static string tempPath = @"C:\Temp\";
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private Itb_school_userService _tb_school_userService;
        private Itb_school_InfoService _tb_school_InfoService;
        private static Itb_school_departmentService _tb_school_departmentService;
        private Itb_school_classinfoService _tb_school_classinfoService;
        private Itb_school_departmentinfoService _tb_school_departmentinfoService;
        private Itb_school_user_multipleService _tb_school_user_multipleService;
        private Itb_school_card_templateService _tb_school_card_templateService;
        private ISchoolCodrService _schoolcodrservice;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_school_userService"></param>
        /// <param name="tb_school_InfoService"></param>
        /// <param name="tb_school_departmentService"></param>
        /// <param name="tb_school_classinfoService"></param>
        /// <param name="tb_school_departmentinfoService"></param>
        /// <param name="tb_school_user_multipleService"></param>
        public UsersController(Itb_school_userService tb_school_userService,
            Itb_school_InfoService tb_school_InfoService,
            Itb_school_departmentService tb_school_departmentService,
            Itb_school_classinfoService tb_school_classinfoService,
            Itb_school_departmentinfoService tb_school_departmentinfoService,
            Itb_school_user_multipleService tb_school_user_multipleService,
            Itb_school_card_templateService tb_school_card_templateService,
             ISchoolCodrService schoolcodrservice
            )
        {
            _tb_school_userService = tb_school_userService;
            _tb_school_InfoService = tb_school_InfoService;
            _tb_school_departmentService = tb_school_departmentService;
            _tb_school_classinfoService = tb_school_classinfoService;
            _tb_school_departmentinfoService = tb_school_departmentinfoService;
            _tb_school_user_multipleService = tb_school_user_multipleService;
            _tb_school_card_templateService = tb_school_card_templateService;
            _schoolcodrservice = schoolcodrservice;
        }

        /// <summary>
        /// 获取支付宝用户信息
        /// </summary>
        /// <param name="appName">小程序配置节Key,到appstettiongs.json中检查</param>
        /// <param name="authCode">授权码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserInfo(string appName, string authCode)
        {
            try
            {
                var app = new AppHelper(appName);
                var userinfoShareResponse = app.GetUserInfo(authCode);
                return new JsonResult(new { data = userinfoShareResponse });
            }
            catch (AopException ex)
            {
                Log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }
        /// <summary>
        /// 获取用户aliuserid 和 token
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="authCode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAliUserTokenAndAliuserid(string appName, string authCode)
        {
            try
            {
                var app = new AppHelper(appName);
                var userinfoShareResponse = app.GetAliUserTokenAndAliuserid(authCode);
                return new JsonResult(new { data = userinfoShareResponse });
            }
            catch (AopException ex)
            {
                Log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }
        /// <summary>
        /// 获取学生信息
        /// </summary>
        /// <param name="appName">小程序配置节Key,到appstettiongs.json中检查</param>
        /// <param name="authCode">授权码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStudentInfo(string appName, string authCode)
        {
            try
            {
                var app = new AppHelper(appName);
                var userinfoShareResponse = app.GetUserInfo(authCode);
                if (userinfoShareResponse.UserId != null)
                {
                    var user = _tb_school_userService.FindByClause(t => t.ali_user_id == userinfoShareResponse.UserId);
                    if (user != null)
                    {
                        return new JsonResult(new { data = user });
                    }
                }
                return new JsonResult(new { data = string.Empty });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }
        /// <summary>
        /// 获取支付宝用户信息(非小程序)
        /// </summary>
        /// <param name="schoolcode">学校code</param>
        /// <param name="authCode">授权码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserInfoToSchoolCode(string schoolcode, string authCode)
        {
            try
            {
                var schoolinfo = _tb_school_InfoService.FindByClause(x => x.School_Code == schoolcode);
                string GatewayUrl = "https://openapi.alipay.com/gateway.do";
                var alipayClient = new DefaultAopClient(GatewayUrl, schoolinfo.app_id, schoolinfo.private_key, "json", null, "RSA2", schoolinfo.alipay_public_key, "UTF-8", false);
                var alipayRequest = new AlipaySystemOauthTokenRequest
                {
                    Code = authCode,
                    GrantType = "authorization_code"
                };
                AlipaySystemOauthTokenResponse response = alipayClient.Execute(alipayRequest);
                Log.Info(response.Body);
                var alipayResponse = alipayClient.Execute(alipayRequest);
                if (!response.IsError)
                {
                    var ali_user_id = alipayResponse.UserId;
                    Log.Info(ali_user_id);
                    var data = _tb_school_userService.FindByClause(x => x.ali_user_id == ali_user_id);
                    return Json(new
                    {
                        msg = "获取人员信息结束",
                        data = data
                    });
                }
                return Json(new
                {
                    msg = "获取人员信息结束",
                });
            }
            catch (AopException ex)
            {
                Log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }
        /// <summary>
        /// 根据学工号查找学生/教师
        /// </summary>
        /// <param name="studentIdentity"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FindUser(string studentIdentity)
        {
            try
            {
                var user = _tb_school_userService.FindByClause(t => t.student_id == studentIdentity || t.user_name == studentIdentity);
                if (user != null)
                {
                    return new JsonResult(new { data = user });
                }
                return new JsonResult(new { data = string.Empty });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }

        /// <summary>
        /// 根据学工号,学校id,查找学生/教师
        /// </summary>
        /// <param name="studentIdentity">学生/教师学工号</param>
        /// <param name="schoolCode">学校id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FindUserBySchool(string studentIdentity, string schoolCode)
        {
            try
            {
                var user = _tb_school_userService.FindByClause(t =>
                    (t.student_id == studentIdentity || t.user_name == studentIdentity) && t.school_id == schoolCode);
                if (user != null)
                {
                    return new JsonResult(new { data = user });
                }
                return new JsonResult(new { data = string.Empty });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }
        /// <summary>
        /// 根据学生身份证找到学生信息
        /// </summary>
        /// <param name="passpart"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSchoolUserInfoToPasspart(string passpart)
        {
            try
            {
                var userinfo = _tb_school_userService.FindByClause(x => x.passport == passpart);
                if (userinfo==null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail,
                        data = "未找到学生信息"
                    });
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = userinfo
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
        #region 老版本学生班级老师信息导入

        /// <summary>
        /// 对外添加数据接口（学校人员信息导入）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddUserInfoAndDepartment([FromBody]JObject obj)
        {
            string schoolcode = obj["schoolcode"].ToString();
            string sign = obj["sign"].ToString();
            var schoolinfo = _tb_school_InfoService.FindByClause(x => x.School_Code == schoolcode);
            if (schoolinfo == null)
            {
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
            string signs = MD5Helper.MD5Encrypt32(schoolinfo.School_Code + schoolinfo.publicKey);
            if (!sign.Equals("0"))
            {
                if (sign != signs)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = "md5验证失败！"
                    });
                }
            }
            try
            {
                List<UserInfo_sdwgyxy> userinf_sdwgyxy = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserInfo_sdwgyxy>>(obj["data"].ToString());

                if (userinf_sdwgyxy.Count > 0)
                {
                    string class_id = userinf_sdwgyxy[0].class_id;
                    if (class_id == "1")
                    {
                        _tb_school_userService.Delete(x => x.school_id == schoolcode && x.class_id == 1);//删除学生信息
                        _tb_school_departmentService.Delete(x => x.schoolcode == schoolcode);
                    }
                    else
                    {
                        _tb_school_userService.Delete(x => x.school_id == schoolcode && x.class_id == 2);//删除老师信息
                    }
                }
                else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = "没有数据请查看导入数据是否正确"
                    });
                }
                var depinfo = userinf_sdwgyxy.Select(x => x.depart_ment).Distinct().ToList();
                tb_school_department dep = new tb_school_department();
                dep.p_id = 0;
                dep.schoolcode = schoolcode;
                dep.treeLevel = 0;
                dep.name = userinf_sdwgyxy[0].depart_ment.Split("-")[0];
                _tb_school_departmentService.Insert(dep);
                //var isok = addDepartment(obj["schoolcode"].ToString(), depinfo);
                var _depmentAll = addDepartment(obj["schoolcode"].ToString(), depinfo);
                //if (!isok)//导入部门成功后
                if (userinf_sdwgyxy[0].class_id == "1")
                {
                    if (_depmentAll == null || _depmentAll.Count <= 0)
                    {
                        return Json(new
                        {
                            code = JsonReturnMsg.FailCode,
                            msg = JsonReturnMsg.AddFail,
                            data = "部门信息导入失败，学生信息未导入成功！"
                        });
                    }
                }
                List<List<UserInfo_sdwgyxy>> listGroup = new List<List<UserInfo_sdwgyxy>>();
                LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(600);
                TaskFactory factory = new TaskFactory(lcts);
                List<Task> tasks = new List<Task>();
                List<tb_school_user> schooluserlist = new List<tb_school_user>();
                //var _depmentAll = _tb_school_departmentService.FindAll() as List<tb_school_department>;
                var _depmentAll2 = _tb_school_departmentService.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, OrderByType.Asc);
                for (int i = 0; i < userinf_sdwgyxy.Count; i++)
                {
                    int n_department_id = 0;
                    string name = "";
                    if (userinf_sdwgyxy[i].depart_ment.Contains('-'))
                    {
                        var sp = userinf_sdwgyxy[i].depart_ment.Split('-');
                        name = sp[sp.Length - 1];
                    }
                    else
                    {
                        name = userinf_sdwgyxy[i].depart_ment;
                    }

                    n_department_id = _depmentAll2.Where(x => x.schoolcode == schoolcode && x.name == name).Select(t => t.id).ToList()[0];

                    tb_school_user user = new tb_school_user();
                    user.user_name = userinf_sdwgyxy[i].user_name;
                    user.passport = userinf_sdwgyxy[i].pass_port;
                    user.department = userinf_sdwgyxy[i].depart_ment.Replace("-", "/");
                    user.card_validity = Convert.ToDateTime(userinf_sdwgyxy[i].validity_time);
                    user.school_id = obj["schoolcode"].ToString();//山东外国语学院
                    user.create_time = DateTime.Now;
                    user.class_id = Convert.ToInt32(userinf_sdwgyxy[i].class_id);
                    user.card_add_id = Convert.ToInt32(userinf_sdwgyxy[i].class_id);
                    user.nationality = "1";
                    user.class_code = "0";
                    user.welcome_flg = 1;
                    user.student_id = userinf_sdwgyxy[i].student_id;
                    user.card_state = 0;
                    user.department_id = n_department_id;
                    tasks.Add(factory.StartNew(s =>
                    {
                        schooluserlist.Add(user);
                    }, i));
                    //_tb_school_userService.AddUserInfoToSDYGYXY(user);
                }
                Task.WaitAll(tasks.ToArray());

                _tb_school_userService.Insert(schooluserlist);
                //_tb_school_userService.
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UploadSuccess,
                    sign = signs,
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
        #region 山东外国语学院数据处理方式  吴其高 2018/9/18
        /// <summary>
        /// 山东外国语学院同步数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SynchronizationInfo()
        {
            try
            {
                string json = allow();
                UserInfo_sdwgyxy_respond userinf_sdwgyxy_respond = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfo_sdwgyxy_respond>(json);
                if (userinf_sdwgyxy_respond.code == "500")
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail,
                        data = "接口获取数据失败接口[allow]!"
                    });
                }
                List<List<UserInfo_sdwgyxy>> listGroup = new List<List<UserInfo_sdwgyxy>>();
                int j = 600;
                for (int i = 0; i < userinf_sdwgyxy_respond.data.Count; i += 600)
                {
                    List<UserInfo_sdwgyxy> cList = new List<UserInfo_sdwgyxy>();
                    cList = userinf_sdwgyxy_respond.data.Take(j).Skip(i).ToList();
                    j += 600;
                    listGroup.Add(cList);
                }
                //List<UserInfo_sdwgyxy> userinf_sdwgyxy = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserInfo_sdwgyxy>>(obj["data"].ToString());
                LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(listGroup.Count);
                TaskFactory factory = new TaskFactory(lcts);
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < userinf_sdwgyxy_respond.data.Count; i++)
                {
                    tb_school_user user = new tb_school_user();
                    user.user_name = userinf_sdwgyxy_respond.data[i].user_name;
                    user.passport = userinf_sdwgyxy_respond.data[i].pass_port;
                    user.department = userinf_sdwgyxy_respond.data[i].depart_ment.Replace("-", "/");
                    user.card_validity = Convert.ToDateTime(userinf_sdwgyxy_respond.data[i].validity_time);
                    user.school_id = "10018";//山东外国语学院
                    user.create_time = DateTime.Now;
                    user.class_id = Convert.ToInt32(userinf_sdwgyxy_respond.data[i].class_id);
                    user.card_add_id = Convert.ToInt32(userinf_sdwgyxy_respond.data[i].class_id);
                    user.nationality = "1";
                    user.class_code = "0";
                    user.welcome_flg = 1;
                    user.student_id = userinf_sdwgyxy_respond.data[i].student_id;
                    user.card_state = 0;
                    tasks.Add(factory.StartNew(s =>
                    {
                        _tb_school_userService.AddUserInfoToSDYGYXY(user);
                    }, i));
                }
                Task.WaitAll(tasks.ToArray());



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
        /// 获取山东外国语学院所有学生信息
        /// </summary>
        /// <returns></returns>
        public static string allow()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string sign = MD5Encrypt32(MD5Encrypt32(date + "7c34a218-b9a6-11e8-ab16-005056bd8790"));
            //RestSharp post 请求
            var client = new RestClient("http://service.sdflc.com/dock/alipay/user/allow");
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/x-www-form-urlencoded");
            request.Parameters.Clear();
            string body = "sign=" + sign;
            request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);
            var response = client.Execute(request);
            var content = response.Content; // 返回的网页内容
            return content;
        }
        /// <summary>
        /// 山东外国语学院指定学生是否在校信息
        /// </summary>
        /// <param name="studentCode"></param>
        /// <returns></returns>
        public static string validity(string studentCode)
        {
            string guid = System.Guid.NewGuid().ToString();
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string sign = MD5Encrypt32(MD5Encrypt32(date + guid));
            //RestSharp post 请求
            var client = new RestClient("http://service.sdflc.com/dock/alipay/user/validity");
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/x-www-form-urlencoded");
            request.Parameters.Clear();
            string body = "sign=" + sign + "&studentCode=" + studentCode;//201800002524
            request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);
            var response = client.Execute(request);
            var content = response.Content; // 返回的网页内容
            return content;
        }
        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encrypt32(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x2");

            }
            return pwd;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="depinfo"></param>
        /// <returns></returns>
        public static List<tb_school_department> addDepartment(string schoolcode, List<string> depinfo)
        {
            try
            {
                var dt = _tb_school_departmentService.GetDepartmentInfo(schoolcode);
                List<tb_school_department> list = new List<tb_school_department>();
                foreach (var item in depinfo)
                {

                    if (!string.IsNullOrEmpty(item.ToString().Trim()))
                    {
                        string department = item.ToString().Trim();
                        var deparr = department.Split('-');
                        string pid = "";
                        for (int c = 0; c < deparr.Length; c++)
                        {
                            if (c == 0)
                            {
                                var dep = _tb_school_departmentService.FindByClause(x => x.schoolcode == schoolcode && x.p_id == 0);
                                pid = dep.id.ToString();
                            }
                            string name = deparr[c].ToString();
                            var depdt = _tb_school_departmentService.FindListByClause(x => x.schoolcode == schoolcode && x.name == name, t => t.id, OrderByType.Desc) as List<tb_school_department>;
                            if (c == 0 && depdt.Count == 0)
                            {
                                tb_school_department departmentModel = new tb_school_department()
                                {
                                    schoolcode = schoolcode,
                                    p_id = int.Parse(pid),
                                    name = name,
                                    treeLevel = c + 1
                                };

                                list.Add(departmentModel);
                                _tb_school_departmentService.Insert(departmentModel);
                                var pdt = _tb_school_departmentService.FindByClause(x => x.schoolcode == schoolcode && x.name == name);
                                pid += "-" + pdt.id.ToString();

                            }
                            else if (c > 0 && depdt.Count == 0)
                            {
                                tb_school_department departmentModel = new tb_school_department()
                                {
                                    schoolcode = schoolcode,
                                    p_id = int.Parse(pid.Split('-')[pid.Split('-').Length - 1]),
                                    name = deparr[c].ToString(),
                                    treeLevel = c + 1
                                };

                                list.Add(departmentModel);
                                _tb_school_departmentService.Insert(departmentModel);
                                var pdt = _tb_school_departmentService.FindByClause(x => x.schoolcode == schoolcode && x.name == name);
                                pid += "-" + pdt.id.ToString();

                            }
                            else
                            {
                                var pdt = _tb_school_departmentService.FindByClause(x => x.schoolcode == schoolcode && x.name == name);
                                pid += "-" + pdt.id.ToString();
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return null;
            }
        }
        #endregion
        #region 2.0版本需要接口
        /// <summary>
        /// 学生信息转json
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult StudenttempletChangeJson()
        {
            var schoolCode = string.Empty;
            schoolCode = GetSchoolCode(schoolCode);
            var files = HttpContext.Request.Form.Files;
            var filePath = string.Empty;
            List<UserInfo_sdwgyxy> list = new List<UserInfo_sdwgyxy>();
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

                            LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(600);
                            TaskFactory factory = new TaskFactory(lcts);
                            List<Task> tasks = new List<Task>();
                            List<string> passlist = new List<string>();
                            var cardtemplate = _tb_school_card_templateService.FindListByClause(x => x.School_ID == schoolCode, t => t.ID, OrderByType.Asc);
                            int s_card;
                            int t_card;
                            try
                            {
                                s_card = cardtemplate.Where(x => x.card_show_name == "学生卡").ToList()[0].ID;
                                t_card = cardtemplate.Where(x => x.card_show_name == "教师卡").ToList()[0].ID;
                            }
                            catch (Exception ex)
                            {
                                return Json(new
                                {
                                    code = JsonReturnMsg.FailCode,
                                    msg = JsonReturnMsg.GetFail,
                                    data = "请创建学生卡和教师卡"
                                });
                            }
                            
                           
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
                                if (row.GetCell(4).ToString().Split('/').Count() != 4)
                                {
                                    return Json(new
                                    {
                                        code = JsonReturnMsg.FailCode,
                                        msg = JsonReturnMsg.GetFail,
                                        data = "部门信息为4级，请查看导入文件是否正确"
                                    });
                                }
                                UserInfo_sdwgyxy userinfo = new UserInfo_sdwgyxy
                                {
                                    class_id = (row.GetCell(0).ToString() == "学生") ? "1" : "2",
                                    card_id = (row.GetCell(0).ToString() == "学生") ? s_card : t_card,
                                    student_id = row.GetCell(1).ToString(),
                                    user_name = row.GetCell(2).ToString(),
                                    pass_port = row.GetCell(3).ToString(),
                                    depart_ment = (row.GetCell(0).ToString() == "学生") ? row.GetCell(4).ToString() + "|" : row.GetCell(4).ToString(),
                                    validity_time = SetExcelTime(row.GetCell(5)),
                                    isMultipleIdentities = row.GetCell(6).ToString()
                                };
                                tasks.Add(factory.StartNew(s =>
                                {
                                    list.Add(userinfo);
                                    passlist.Add(userinfo.pass_port);
                                }, i));
                                //list.Add(tbPayment);
                            }
                            Task.WaitAll(tasks.ToArray());

                            //核对身份证是否有重复
                            var passportList = _tb_school_userService.FindListByClause(x => x.school_id == schoolCode, t => t.user_id, OrderByType.Asc).Select(x => x.passport).ToList();
                            var exp1 = passportList.Intersect(passlist);
                            if (exp1.Count() > 0)
                            {
                                string msg = "";
                                foreach (var item in exp1)
                                {
                                    msg += item + ",";
                                }
                                return Json(new
                                {
                                    code = JsonReturnMsg.FailCode,
                                    msg = "身份证重复请核查",
                                    data = msg
                                });
                            }
                        }
                        return Json(new
                        {
                            code = JsonReturnMsg.SuccessCode,
                            studentJson = list,
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail,
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
        #region 测试批量导入
       
        private List<UserInfo_sdwgyxy> StudenttempletChangeJson2()
        {
            List<UserInfo_sdwgyxy> list = new List<UserInfo_sdwgyxy>();
            try
            {
                string filePath = "D:/studentinfo/studentInfo10064.xlsx";
                string schoolCode = "10064";
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
                        //准备数据库录入数据
                        bool isBreak = false;

                        LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(600);
                        TaskFactory factory = new TaskFactory(lcts);
                        List<Task> tasks = new List<Task>();
                        List<string> passlist = new List<string>();
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
                            UserInfo_sdwgyxy userinfo = new UserInfo_sdwgyxy
                            {
                                class_id = (row.GetCell(0).ToString() == "学生") ? "1" : "2",
                                student_id = row.GetCell(1).ToString(),
                                user_name = row.GetCell(2).ToString(),
                                pass_port = row.GetCell(3).ToString(),
                                depart_ment = (row.GetCell(0).ToString() == "学生")?row.GetCell(4).ToString()+"|": row.GetCell(4).ToString(),
                                validity_time = SetExcelTime(row.GetCell(5)),
                                isMultipleIdentities = row.GetCell(6).ToString()
                            };
                            tasks.Add(factory.StartNew(s =>
                            {
                                list.Add(userinfo);
                                passlist.Add(userinfo.pass_port);
                            }, i));
                            //list.Add(tbPayment);
                        }
                        Task.WaitAll(tasks.ToArray());

                        //核对身份证是否有重复
                        var passportList = _tb_school_userService.FindListByClause(x => x.school_id == schoolCode, t => t.user_id, OrderByType.Asc).Select(x => x.passport).ToList();
                        //var exp1 = passportList.Where(a => passlist.Exists(t => a.Contains(t))).ToList();
                        var exp1 = passportList.Intersect(passlist);
                        if (exp1.Count()>0)
                        {
                            string msg = "";
                            foreach (var item in exp1)
                            {
                                msg += item + ",";
                            }
                            msg += "身份证重复请核查";
                            return null;
                        }
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return null;
            }
        }
        /// <summary>
        /// 测试导入学生信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddUserInfoAndDepartmentToV3([FromBody]JObject obj)
        {
            string schoolcode = "10064";
            string sign = "0";
            string err_department_id = "";
            string erruser = "";
            tb_school_user err_user = new tb_school_user();
            //var schoolinfo = _tb_school_InfoService.FindByClause(x => x.School_Code == schoolcode);
            //if (schoolinfo == null)
            //{
            //    return Json(new
            //    {
            //        code = JsonReturnMsg.FailCode,
            //        msg = JsonReturnMsg.GetFail
            //    });
            //}
            //string signs = MD5Helper.MD5Encrypt32(schoolinfo.School_Code + schoolinfo.publicKey);
            //if (!sign.Equals("0"))
            //{
            //    if (sign != signs)
            //    {
            //        return Json(new
            //        {
            //            code = JsonReturnMsg.FailCode,
            //            msg = "md5验证失败！"
            //        });
            //    }
            //}
            try
            {
                List<UserInfo_sdwgyxy> userinf_sdwgyxy = StudenttempletChangeJson2();
                string class_id = userinf_sdwgyxy[0].class_id;
                if (userinf_sdwgyxy.Count > 0)
                {
                    //有数据正确
                }
                else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = "没有数据请查看导入数据是否正确"
                    });
                }
                var depinfo = userinf_sdwgyxy.Select(x => x.depart_ment).Distinct().ToList();
                bool isOK = addDepartmentToV2(schoolcode, depinfo, class_id);
                if (!isOK)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.AddFail,
                        data = "部门信息导入失败，学生信息未导入成功！"
                    });
                }
                List<List<UserInfo_sdwgyxy>> listGroup = new List<List<UserInfo_sdwgyxy>>();
                LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(600);
                TaskFactory factory = new TaskFactory(lcts);
                List<Task> tasks = new List<Task>();
                List<tb_school_user> schooluserlist = new List<tb_school_user>();
                List<tb_school_user_multiple> usermultipleList = new List<tb_school_user_multiple>();
                var _depmentAll2 = _tb_school_departmentService.FindListByClause(x => x.schoolcode == schoolcode && x.treeLevel == 3, t => t.id, OrderByType.Asc);
                var _classinfoAll = _tb_school_classinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);
                var _depinfoAll = _tb_school_departmentinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);

                for (int i = 0; i < userinf_sdwgyxy.Count; i++)
                {
                    int n_department_id = 0;
                    bool isOKStudent = userinf_sdwgyxy[i].depart_ment.Contains("|");
                    string name = userinf_sdwgyxy[i].depart_ment.Split('/')[3].Replace("|", "");
                    int department_id = 0;
                    try
                    {
                        n_department_id = _depmentAll2.Where(x => x.schoolcode == schoolcode && x.name == name).Select(t => t.id).ToList()[0];
                        if (isOKStudent)
                        {
                            department_id = _classinfoAll.Where(x => x.department_classID == n_department_id).Select(t => t.ID).ToList()[0];
                        }
                        else
                        {
                            err_department_id = n_department_id.ToString();
                            department_id = _depinfoAll.Where(x => x.department_treeID == n_department_id).Select(t => t.ID).ToList()[0];
                        }
                    }
                    catch (Exception)
                    {

                        erruser += userinf_sdwgyxy[i].student_id + ",";
                        continue;
                    }
                    
                    if (userinf_sdwgyxy[i].isMultipleIdentities == "0")
                    {
                        tb_school_user user = new tb_school_user();
                        user.user_name = userinf_sdwgyxy[i].user_name;
                        user.passport = userinf_sdwgyxy[i].pass_port;
                        user.department = userinf_sdwgyxy[i].depart_ment.Replace("|", ""); ;
                        user.card_validity = Convert.ToDateTime(userinf_sdwgyxy[i].validity_time);
                        user.school_id = schoolcode;
                        user.create_time = DateTime.Now;
                        user.class_id = Convert.ToInt32(userinf_sdwgyxy[i].class_id);
                        user.card_add_id = Convert.ToInt32(userinf_sdwgyxy[i].class_id);
                        user.nationality = "1";
                        user.class_code = "0";
                        user.welcome_flg = 1;
                        user.student_id = userinf_sdwgyxy[i].student_id;
                        user.card_state = 0;
                        user.department_id = department_id;
                        tasks.Add(factory.StartNew(s =>
                        {
                            schooluserlist.Add(user);
                            //_tb_school_userService.Insert(schooluserlist);
                        }, i));

                    }
                    else
                    {
                        tb_school_user_multiple usermultiple = new tb_school_user_multiple();
                        usermultiple.class_id = Convert.ToInt32(userinf_sdwgyxy[i].class_id);
                        usermultiple.department_id = department_id;
                        usermultiple.passport = userinf_sdwgyxy[i].pass_port;
                        usermultiple.school_id = schoolcode;
                        usermultiple.student_id = userinf_sdwgyxy[i].student_id;
                        usermultiple.user_name = userinf_sdwgyxy[i].user_name;
                        tasks.Add(factory.StartNew(s =>
                        {
                            usermultipleList.Add(usermultiple);
                            //_tb_school_user_multipleService.Insert(usermultipleList);
                        }, i));
                    }

                }
                Task.WaitAll(tasks.ToArray());
                //foreach (var item in schooluserlist)
                //{
                //    err_user = item;
                //    _tb_school_userService.Insert(item);
                //}
                int batchCount = 500;
                for (int i = 0; i < schooluserlist.Count(); i+= batchCount)
                {
                    try
                    {
                        _tb_school_userService.Insert(schooluserlist.Skip(i).Take(batchCount).ToList());
                    }
                    catch (Exception ex)
                    {
                        Log.Error("------i=" + i + "  " + ex);
                    }
                }
                if (usermultipleList.Count > 0)
                {
                    _tb_school_user_multipleService.Insert(usermultipleList);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UploadSuccess,
                    erruser = erruser,
                    //sign = signs,
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
        #endregion
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
        /// <summary>
        /// 【可对外接口】添加人员信息和部门信息（2.0版本添加学生，老师信息）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddUserInfoAndDepartmentToV2([FromBody]JObject obj)
        {
            string schoolcode = obj["schoolcode"].ToString();
            string sign = obj["sign"].ToString();
            string err_department_id = "";
            string erruser = "";
            var schoolinfo = _tb_school_InfoService.FindByClause(x => x.School_Code == schoolcode);
            if (schoolinfo == null)
            {
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
            string signs = MD5Helper.MD5Encrypt32(schoolinfo.School_Code + schoolinfo.publicKey);
            if (!sign.Equals("0"))
            {
                if (sign != signs)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = "md5验证失败！"
                    });
                }
            }
            try
            {
                List<UserInfo_sdwgyxy> userinf_sdwgyxy = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserInfo_sdwgyxy>>(obj["data"].ToString());
                string class_id = userinf_sdwgyxy[0].class_id;
                if (userinf_sdwgyxy.Count > 0)
                {
                    //有数据正确
                }
                else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = "没有数据请查看导入数据是否正确"
                    });
                }
                var depinfo = userinf_sdwgyxy.Select(x => x.depart_ment).Distinct().ToList();
                bool isOK = addDepartmentToV2(schoolcode, depinfo, class_id);
                if (!isOK)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.AddFail,
                        data = "部门信息导入失败，学生信息未导入成功！"
                    });
                }
                List<List<UserInfo_sdwgyxy>> listGroup = new List<List<UserInfo_sdwgyxy>>();
                LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(600);
                TaskFactory factory = new TaskFactory(lcts);
                List<Task> tasks = new List<Task>();
                List<tb_school_user> schooluserlist = new List<tb_school_user>();
                List<tb_school_user_multiple> usermultipleList = new List<tb_school_user_multiple>();

                var _depmentAll2 = _tb_school_departmentService.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, OrderByType.Asc);
                var _classinfoAll = _tb_school_classinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);
                var _depinfoAll = _tb_school_departmentinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);

                for (int i = 0; i < userinf_sdwgyxy.Count; i++)
                {
                    int n_department_id = 0;
                    bool isOKStudent = userinf_sdwgyxy[i].depart_ment.Contains("|");
                    string name = userinf_sdwgyxy[i].depart_ment.Split('/')[3].Replace("|", "");
                    int department_id = 0;
                    try
                    {
                        n_department_id = _depmentAll2.Where(x => x.schoolcode == schoolcode && x.name == name).Select(t => t.id).ToList()[0];
                        if (isOKStudent)
                        {
                            department_id = _classinfoAll.Where(x => x.department_classID == n_department_id).Select(t => t.ID).ToList()[0];
                        }
                        else
                        {
                            err_department_id = n_department_id.ToString();
                            department_id = _depinfoAll.Where(x => x.department_treeID == n_department_id).Select(t => t.ID).ToList()[0];
                        }
                    }
                    catch (Exception)
                    {

                        erruser += userinf_sdwgyxy[i].student_id + ",";
                        continue;
                    }
                    if (userinf_sdwgyxy[i].isMultipleIdentities == "0")
                    {
                        tb_school_user user = new tb_school_user();
                        user.user_name = userinf_sdwgyxy[i].user_name;
                        user.passport = userinf_sdwgyxy[i].pass_port;
                        user.department = userinf_sdwgyxy[i].depart_ment.Replace("|", "");
                        user.card_validity = Convert.ToDateTime(userinf_sdwgyxy[i].validity_time);
                        user.school_id = schoolcode;
                        user.create_time = DateTime.Now;
                        user.class_id = Convert.ToInt32(userinf_sdwgyxy[i].class_id);
                        user.card_add_id = Convert.ToInt32(userinf_sdwgyxy[i].class_id);
                        user.nationality = "1";
                        user.class_code = "0";
                        user.welcome_flg = 1;
                        user.student_id = userinf_sdwgyxy[i].student_id;
                        user.card_state = 0;
                        user.department_id = department_id;
                        user.card_id = userinf_sdwgyxy[i].card_id;
                        tasks.Add(factory.StartNew(s =>
                        {
                            schooluserlist.Add(user);
                        }, i));

                    }
                    else
                    {
                        tb_school_user_multiple usermultiple = new tb_school_user_multiple();
                        usermultiple.class_id = Convert.ToInt32(userinf_sdwgyxy[i].class_id);
                        usermultiple.department_id = department_id;
                        usermultiple.passport = userinf_sdwgyxy[i].pass_port;
                        usermultiple.school_id = schoolcode;
                        usermultiple.student_id = userinf_sdwgyxy[i].student_id;
                        usermultiple.user_name = userinf_sdwgyxy[i].user_name;
                        tasks.Add(factory.StartNew(s =>
                        {
                            usermultipleList.Add(usermultiple);
                        }, i));
                    }

                }
                Task.WaitAll(tasks.ToArray());
                int batchCount = 500;
                for (int i = 0; i < schooluserlist.Count(); i += batchCount)
                {
                    try
                    {
                        _tb_school_userService.Insert(schooluserlist.Skip(i).Take(batchCount).ToList());
                    }
                    catch (Exception ex)
                    {
                        Log.Error("------i=" + i + "  " + ex);
                    }
                }
                if (usermultipleList.Count > 0)
                {
                    _tb_school_user_multipleService.Insert(usermultipleList);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UploadSuccess,
                    sign = signs,
                    data = "数据保存成功！"
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail,
                    errcode = "erruser",
                    erruser = erruser
                });
            }
        }
        private bool addDepartmentToV2(string schoolcode, List<string> depinfo, string class_id)
        {
            string errname = "";
            try
            {
                //树形表添加根目录
                tb_school_department depmodel = new tb_school_department();
                var sproot = depinfo[0].Split('/');
                long school_pid = 0;
                long branch_pid = 0;
                var departmentName = "";//部门名、系名 第三级
                var department_pid = 0;
                long depid = 0;//系部门id
                var dataroot = _tb_school_departmentService.FindByClause(x => x.schoolcode == schoolcode && x.p_id == 0);
                if (dataroot == null)
                {
                    depmodel.p_id = 0;
                    depmodel.schoolcode = schoolcode;
                    depmodel.treeLevel = 0;
                    depmodel.name = sproot[0] + "";
                    school_pid = _tb_school_departmentService.Insert(depmodel);
                }
                else
                {
                    school_pid = Convert.ToInt32(dataroot.id);
                }
                //树形表添加分院
                string branchName = "";
                var datadepinfo = _tb_school_departmentService.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, OrderByType.Asc) as List<tb_school_department>;
                foreach (var item in depinfo)
                {
                    var sp = item.Split('/');
                    string name = sp[1] + "";
                    if (!datadepinfo.Any(x => x.name == name&&x.treeLevel==1))
                    {
                        tb_school_department depmodel2 = new tb_school_department();
                        depmodel2.p_id = Convert.ToInt32(school_pid);
                        depmodel2.schoolcode = schoolcode;
                        depmodel2.treeLevel = 1;
                        depmodel2.name = name;
                        branchName = name;
                        branch_pid = _tb_school_departmentService.Insert(depmodel2);
                        depmodel2.id = int.Parse(branch_pid.ToString());
                        datadepinfo.Add(depmodel2);
                    }
                    else
                    {
                        branchName = name;
                        errname = name;
                        var aaa = datadepinfo.Where(x => x.name == name && x.treeLevel==1).Select(t => t.id).ToList()[0];
                        branch_pid = long.Parse(aaa.ToString());
                    }
                    name = sp[2] + "";
                    if (!datadepinfo.Any(x => x.name == name && x.treeLevel == 2))
                    {
                        tb_school_department depmodel3 = new tb_school_department();
                        depmodel3.p_id = Convert.ToInt32(branch_pid);
                        depmodel3.schoolcode = schoolcode;
                        depmodel3.treeLevel = 2;
                        depmodel3.name = name;
                        department_pid = Convert.ToInt32(_tb_school_departmentService.Insert(depmodel3));
                        departmentName = name;
                        depmodel3.id = department_pid;
                        datadepinfo.Add(depmodel3);
                    }
                    else
                    {
                        var aaa = datadepinfo.Where(x => x.name == name && x.treeLevel == 2).Select(t => t.id).ToList()[0];
                        department_pid = aaa;
                        departmentName = name;
                    }
                    tb_school_department depmodel4 = new tb_school_department();
                    depmodel4.p_id = department_pid;
                    depmodel4.schoolcode = schoolcode;
                    depmodel4.treeLevel = 3;
                    name = (sp[3] + "").Replace("|", "");
                    depmodel4.name = name;
                    if (item.Contains("|"))
                    {
                        depmodel4.isType = false;//是否为学生
                        
                        if (!datadepinfo.Any(x => x.name == name && x.treeLevel == 3))
                        {
                            depid = _tb_school_departmentService.Insert(depmodel4);
                            datadepinfo.Add(depmodel4);
                            tb_school_classinfo classinfomodel = new tb_school_classinfo();
                            classinfomodel.schoolcode = schoolcode;
                            classinfomodel.BranchID = Convert.ToInt32(branch_pid);
                            classinfomodel.DepartmentID = department_pid;
                            classinfomodel.department_classID = Convert.ToInt32(depid);
                            _tb_school_classinfoService.Insert(classinfomodel);
                        }
                       
                    }
                    else
                    {
                        depmodel4.isType = true;//是否为学生
                        if (!datadepinfo.Any(x => x.name == name))
                        {
                            depid = _tb_school_departmentService.Insert(depmodel4);
                            datadepinfo.Add(depmodel4);
                            tb_school_departmentinfo departmentinfomodel = new tb_school_departmentinfo();
                            departmentinfomodel.schoolcode = schoolcode;
                            departmentinfomodel.BranchID = Convert.ToInt32(branch_pid);
                            departmentinfomodel.departmentID = department_pid;
                            departmentinfomodel.department_treeID = Convert.ToInt32(depid);
                            _tb_school_departmentinfoService.Insert(departmentinfomodel);
                        }
                       
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                string ii = errname;
                Log.Error(ex);
                return false;
            }

        }
        /// <summary>
        /// 批量删除学生信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteSchoolUser([FromBody]JObject obj)
        {
            try
            {
                string schoolcode = obj["schoolcode"].ToString();
                string user_id = obj["user_id"].ToString();

                //向喜云推送数据
                SchoolCodrController cc = new SchoolCodrController(_tb_school_InfoService, null, _schoolcodrservice, _tb_school_userService, null, null, null, null, null);
                cc.PushSchoolCard("2", "3", user_id + "");

                var userids = user_id.Split(',');
                _tb_school_userService.DeleteByIds(userids);
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
        /// 修改学生班级信息（批量）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateSchoolUserToDepartment([FromBody]JObject obj)
        {
            try
            {
                //Dictionary<string, Object> obj2 = (Dictionary<string, Object>)JsonConvert.DeserializeObject(obj["json"].ToString());
                string schoolcode = obj["schoolcode"].ToString();
                string user_id = obj["user_id"].ToString();
                var userids = user_id.Split(',');
                int depamentid = Convert.ToInt32(obj["depamentid"].ToString());
                var classinfo = _tb_school_classinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);
                var departmentinfo = _tb_school_departmentinfoService.FindListByClause(x => x.schoolcode == schoolcode, t => t.ID, OrderByType.Asc);
                var departmenttree = _tb_school_departmentService.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, OrderByType.Asc);
                foreach (var item in userids)
                {
                    var userinfo = _tb_school_userService.FindById(Convert.ToInt32(item));
                    string departmentName = "";
                    if (userinfo.class_id == 1)
                    {
                        var classmode = classinfo.Where(x => x.ID == depamentid).ToList()[0];
                        if (classmode == null)
                        {
                            return Json(new
                            {
                                code = JsonReturnMsg.FailCode,
                                msg = "未找到学生班级"
                            });
                        }

                        departmentName = departmenttree.Where(x => x.p_id == 0).ToList()[0].name + "/";
                        departmentName += departmenttree.Where(x => x.id == classmode.BranchID).ToList()[0] + "/";
                        departmentName += departmenttree.Where(x => x.id == classmode.DepartmentID).ToList()[0] + "/";
                        departmentName += departmenttree.Where(x => x.id == classmode.department_classID).ToList()[0] + "/";
                    }
                    else
                    {
                        var department = departmentinfo.Where(x => x.ID == depamentid).ToList()[0];
                        if (department == null)
                        {
                            return Json(new
                            {
                                code = JsonReturnMsg.FailCode,
                                msg = "未找到老师职务"
                            });
                        }

                        departmentName = departmenttree.Where(x => x.p_id == 0).ToList()[0].name + "/";
                        departmentName += departmenttree.Where(x => x.id == department.BranchID).ToList()[0].name + "/";
                        departmentName += departmenttree.Where(x => x.id == department.departmentID).ToList()[0].name + "/";
                        departmentName += departmenttree.Where(x => x.id == department.department_treeID).ToList()[0].name + "/";
                    }
                    _tb_school_userService.UpdateColumnsByConditon(a => new tb_school_user { department_id = depamentid, department = departmentName },
                        a => a.user_id == SqlFunc.ToInt32(item) && a.school_id == schoolcode);
                }
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
        /// 获取学员信息通过userid
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolUserInfoToUser_id(string schoolcode, int user_id)
        {
            try
            {
                var data = _tb_school_userService.FindByClause(x => x.user_id == user_id && x.school_id == schoolcode);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
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
    }
}
