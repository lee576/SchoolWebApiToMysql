﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Alipay.AopSdk;
using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Domain;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SchoolWebApi.Utility;
using RestSharp;
using SqlSugar;
using Service;
using System.Collections;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using SchoolWebApi.HeadParams;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 学校卡片接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]

    public class SchoolCodrController : Controller
    {

        /// <summary>
        /// 初始化日志对象
        /// </summary>
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private static int count = 0;
        private static int pcount = 0;

        private Itb_school_InfoService _tb_school_InfoService;
        private Ish_areaService _sh_areaService;
        private ISchoolCodrService _schoolcodrservice;
        private Itb_school_userService _tb_school_userService;
        private Itb_school_card_templateService _tb_school_card_templateService;
        private Itb_alipay_imageService _tb_alipay_imageService;
        private Itb_school_card_template_columnService _tb_school_card_template_columnService;
        private Itb_school_card_template_formerService _tb_school_card_template_formerService;
        private Itb_school_departmentService _tb_school_departmentService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_school_InfoService"></param>
        /// <param name="sh_areaService"></param>
        /// <param name="schoolcodrservice"></param>
        /// <param name="tb_school_userService"></param>
        /// <param name="tb_school_card_templateService"></param>
        /// <param name="tb_alipay_imageService"></param>
        /// <param name="tb_school_card_template_columnService"></param>
        /// <param name="tb_school_card_template_formerService"></param>
        /// <param name="tb_school_departmentService"></param>
        public SchoolCodrController(Itb_school_InfoService tb_school_InfoService,
            Ish_areaService sh_areaService,
            ISchoolCodrService schoolcodrservice,
            Itb_school_userService tb_school_userService,
            Itb_school_card_templateService tb_school_card_templateService,
            Itb_alipay_imageService tb_alipay_imageService,
            Itb_school_card_template_columnService tb_school_card_template_columnService,
            Itb_school_card_template_formerService tb_school_card_template_formerService,
            Itb_school_departmentService tb_school_departmentService)
        {
            _tb_school_InfoService = tb_school_InfoService;
            _sh_areaService = sh_areaService;
            _schoolcodrservice = schoolcodrservice;
            _tb_school_userService = tb_school_userService;
            _tb_school_card_templateService = tb_school_card_templateService;
            _tb_alipay_imageService = tb_alipay_imageService;
            _tb_school_card_template_columnService = tb_school_card_template_columnService;
            _tb_school_card_template_formerService = tb_school_card_template_formerService;
            _tb_school_departmentService = tb_school_departmentService;
        }



      

        /// <summary>
        /// 根据区域获取学校信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolInfoToProvince()
        {

            try
            {
                //var result = getlst.GroupBy(x => x.guigecategoryid).Select(x => x.First()).ToList()
                var allSchool = _tb_school_InfoService.FindAll().ToList();
                var schoolinfo = allSchool.GroupBy(x => x.province).Select(x => x.First()).ToList();
                List<SchoolCord> schoollist = new List<SchoolCord>();
                foreach (var item in schoolinfo)
                {
                    SchoolCord schoolcord = new SchoolCord();
                    schoolcord.Province = item.province;
                    var schools_info = allSchool.Where(x => x.province == item.province);
                    foreach (var item2 in schools_info)
                    {
                        int code = Convert.ToInt32(item2.School_Code);
                        if (code < 10000)
                        {
                            continue;
                        }
                        tb_school_info schoolInfo = new tb_school_info();
                        schoolInfo = item2 as tb_school_info;
                        schoolInfo.private_key = null;
                        schoolInfo.publicKey = null;
                        schoolInfo.alipay_public_key = null;
                        schoolcord.schoolInfo.Add(schoolInfo);
                    }
                    schoollist.Add(schoolcord);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = schoollist
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
        /// <summary>
        /// 【小程序接口】获取地区信息接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAreaInfo()
        {
            try
            {
                var data = _sh_areaService.GetAreaInfo();
                JsonSerializerSettings settings = new JsonSerializerSettings();
                return Json(data, settings);
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
        /// <summary>
        /// 【小程序接口】通过省份获取学校名
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolName(string areaName)
        {

            try
            {
                var alipayRequest = new AlipayUserInfoShareRequest();
                List<AreaInfoList> lal = new List<AreaInfoList>();
                var dt = _tb_school_InfoService.GetSchoolName(areaName);
                //var dt = _tb_school_InfoService.FindAll().ToArray().ToList().Where(x => x.School_name == areaName);
                string[] zm = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                int index = 0;
                foreach (var item in zm)
                {
                    AreaInfoList al = new AreaInfoList();
                    List<Models.ViewModels.AreaInfo> las = new List<Models.ViewModels.AreaInfo>();
                    foreach (var item2 in dt)
                    {
                        int code = Convert.ToInt32(item2.School_Code);
                        if (code < 10000)
                        {
                            continue;
                        }
                        Models.ViewModels.AreaInfo info = new Models.ViewModels.AreaInfo();
                        string schoolzm = PinYinHelper.GetFirstPinyin(item2.School_name.Substring(0, 1));
                        if (item == schoolzm)
                        {
                            index++;
                            info.id = "v" + index;
                            info.cityName = item2.School_name;
                            info.type = "1";
                            info.schooltype = item2.type.ToString();
                            info.schoolcode = item2.School_Code;
                            las.Add(info);
                        }
                    }
                    if (las.Count == 0)
                    {
                        continue;
                    }
                    al.letter = item;
                    al.show = true;
                    al.data = las;
                    lal.Add(al);
                }
                JsonSerializerSettings settings = new JsonSerializerSettings();
                return Json(lal, settings);
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
        /// <summary>
        /// 开卡组件配置
        /// </summary>
        /// <param name="cert_no"></param>
        /// <param name="schoolcode"></param>
        /// <param name="app_auth_token"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AppActivateurl(string cert_no, string schoolcode, string app_auth_token)
        {
            try
            {
                AppHelper helper = new AppHelper("GetRuoHuoSchool");
                var stundentinfo = _tb_school_userService.FindByClause(x => x.passport == cert_no);
                var schoolusers = _schoolcodrservice.GetSchoolUser(cert_no);
                if (stundentinfo == null)
                {
                    return Json(new
                    {
                        code = "0",
                        msg = "未能找到学员信息！"
                    });
                }
                var schooluser = schoolusers.First();
                var schoolinfo = _tb_school_InfoService.FindByClause(x => x.School_Code == schoolcode);
                var formtemplate = helper.alipay_marketing_card_formtemplate_set(schooluser.template_id, app_auth_token);
                var activateurl = helper.alipay_marketing_card_activateurl_apply(schooluser.template_id, app_auth_token);
                if (formtemplate.IsError || activateurl.IsError)
                {
                    return Json(new
                    {
                        code = "0",
                        msg = "开卡表单模板异常或者领卡投放链接异常！",
                        formtemplate = formtemplate.Body,
                        activateurl = activateurl.Body

                    });
                }
                var DynamicObject = JsonConvert.DeserializeObject<dynamic>(activateurl.Body);
                return Json(new
                {
                    code = "1",
                    formtemplate = formtemplate.Body,
                    data = DynamicObject,
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "获取授权失败！"
                });
            }
        }
        /// <summary>
        /// 授权解码url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UrlDecode(string url)
        {
            try
            {
                string strUrl = WebUtility.UrlDecode(url);
                return Json(new
                {
                    url = strUrl
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "链接解码失败！"
                });
            }
        }


        /// <summary>
        /// 查询开卡信息
        /// </summary>
        /// <param name="target_card_no"></param>
        /// <param name="app_auth_token"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolCardInfo(string target_card_no, string app_auth_token)
        {
            try
            {
                //query.target_card_no = card_info["biz_card_no"].ToString();
                //query.target_card_no_type = "BIZ_CARD";
                AlipayMarketingCardQueryModel model = new AlipayMarketingCardQueryModel();
                model.TargetCardNo = target_card_no;
                model.TargetCardNoType = "BIZ_CARD";
                AppHelper helper = new AppHelper("GetRuoHuoSchool");
                var data = helper.alipay_marketing_card_query(model, app_auth_token);
                return Json(data);
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "跳转卡包失败"
                });
            }
        }
        /// <summary>
        /// 通过授权码获取userID等信息
        /// </summary>
        /// <param name="authcode"></param>
        /// <param name="app_auth_token"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AppAuthorize(string authcode, string app_auth_token)
        {
            try
            {
                //string app_auth_token = "201809BB13ef23abf249447da1955fff1fce1X05";
                AppHelper helper1 = new AppHelper("GetCampusCard");
                var userinfo = helper1.GetUserInfo(authcode);
                AppHelper helper = new AppHelper("GetRuoHuoSchool");
                var userinfoShareResponse = helper.GetUserIdAndToken(authcode, app_auth_token);
                if (userinfoShareResponse == null)
                {
                    return Json(new
                    {
                        code = "0",
                        msg = "未能获取用户信息！"
                    });
                }
                var DynamicObject = JsonConvert.DeserializeObject<dynamic>(userinfoShareResponse.Body);
                return Json(new
                {
                    code = "1",
                    msg = "获取用户信息",
                    data = DynamicObject
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "获取用户信息失败！"
                });
            }
        }
        /// <summary>
        /// 授权回调接口
        /// </summary>
        /// <param name="cert_no"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AppCallBack(string cert_no)
        {
            try
            {
                return Json(new
                {
                    data = ""
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "链接解码失败！"
                });
            }
        }
        /// <summary>
        /// 发行校园卡
        /// </summary>
        [HttpPost]
        public IActionResult MarketingCard([FromBody]JObject obj)
        {
            try
            {
                string userid = obj["user_id"].ToString();
                string template_id = obj["template_id"].ToString();
                string cert_no = obj["cert_no"].ToString();
                string name = obj["name"].ToString();
                string schoolcode = obj["schoolcode"].ToString();
                string access_token = obj["access_token"].ToString();
                string app_auth_token = obj["app_auth_token"].ToString();
                string phone = obj["phone"].ToString();
                var schooluser = _schoolcodrservice.GetSchoolUser(cert_no).First();
                var schoolinfo = _tb_school_InfoService.FindByClause(x => x.School_Code == schoolcode);
                var studentName = _tb_school_userService.FindByClause(x => x.passport == cert_no).user_name;
                alipay_marketing_card_open_Model.gotojsonbean gt = new alipay_marketing_card_open_Model.gotojsonbean();
                gt.card_template_id = schooluser.template_id;
                gt.user_uni_id = userid;
                gt.external_card_no = schooluser.student_id;
                gt.open_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                gt.valid_date = DateTime.Parse(schooluser.card_validity).ToString("yyyy-MM-dd HH:mm:ss");
                gt.name = studentName;//"LT";                                  //String可选64姓名 李洋 
                gt.dept = schooluser.department;
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                var config = builder.Build();
                var alipaySection = config.GetSection("GetRuoHuoSchool");
                string PID = alipaySection.GetSection("PID").Value;
                AppHelper helper = new AppHelper("GetRuoHuoSchool");
                var car_open = helper.alipay_marketing_card_open(gt, schoolinfo, app_auth_token, PID, access_token);
                if (car_open == null)
                {
                    return Json(new
                    {
                        msg = "0",
                        data = "开卡失败"
                    });
                }
                var DynamicObject = JsonConvert.DeserializeObject<dynamic>(car_open.Body);
                return Json(new
                {
                    code = "1",
                    msg = "开卡返回信息",
                    data = DynamicObject
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "领卡失败！"
                });
            }
        }
        /// <summary>
        /// 更新学员信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateSchoolUser([FromBody]JObject obj)
        {
            try
            {
                string cert_no = obj["cert_no"] + "";
                var schooluser = _schoolcodrservice.GetSchoolUser(cert_no).First();
                string biz_card_no = obj["biz_card_no"] + "";
                int schooluserid = Convert.ToInt32(schooluser.user_id);
                string phone = obj["phone"] + "";
                string user_id = obj["user_id"] + "";


                tb_school_user suser = _tb_school_userService.FindByClause(x => x.user_id == schooluserid);//biz_card_no, school_user_id, user_id, cell
                suser.biz_card_no = biz_card_no;
                suser.ali_user_id = user_id;
                suser.card_state = 1;
                suser.cell = phone;
                suser.Collarcard_time = DateTime.Now;



                bool isOK = _tb_school_userService.Update(suser);
                //推送用户信息
                if (isOK)
                    PushSchoolCard("2", "1", schooluserid.ToString());



                return Json(new
                {
                    code = "1",
                    msg = "学员信息添加成功！"
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "修改学员信息失败！"
                });
            }
        }

        /// <summary>
        /// 【序列化匿名对象】
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult TestJson()
        {
            try
            {
                var data = new { Name = "张三", age = 18, sex = new { nan = "男", nv = "女" }, phone = "123456789" };
                return Json(new
                {
                    data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(data))
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "链接解码失败！"
                });
            }
        }

        /// <summary>
        /// 返回校园卡领卡地址
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetSchoolCardURL([FromBody]JObject obj)
        {
            string SchoolId = obj["school_id"] + "";
            var school = _tb_school_InfoService.FindByClause(p => p.School_Code == SchoolId);
            string type = school.type + "";
            string schoolname = school.School_name;
            CashierConfig.SetCode();

            string XYKURL = CashierConfig.XYKURL + "?schoolcode=" + SchoolId + "&type=" + type+ "&schoolname="+ schoolname;
            string ZFDTURL = CashierConfig.ZFDTURL;
            var data = _tb_school_departmentService.FindByClause(p => p.schoolcode == SchoolId && p.p_id == 0);

            return Json(new
            {
                code = "10000",
                msg = "查询成功",
                xykurl = XYKURL,
                zfdturl = ZFDTURL,
                data = data
            });
        }


        /// <summary>
        /// 立即领卡
        /// </summary>
        /// <param name="school_id">学校编号</param>
        /// <param name="type">学校类型</param>
        /// <param name="name"></param>
        /// <param name="certno"></param>
        /// <param name="cell"></param>
        [HttpGet]

        public void GetAliauthorize(string school_id, string type,string name, string certno, string cell)
        {
            ZhifubaoLogin zhifubao = CashierConfig.dataZFBlogin(school_id);

            if (type == "1")
            {
              
               
                   
                    string  s= $"https://openauth.alipay.com/oauth2/publicAppAuthorize.htm?app_id=" + zhifubao.appId + "&scope=auth_user,auth_ecard&redirect_uri=" + HttpUtility.UrlEncode(@"http://www.newxiaoyuan.com/1.html?school_id=" + school_id + "&type=" + type);
                    Log.Info("地址1:"+s);
                   
                 


                    Response.Redirect(s);
                Log.Info("地址1结束");


            }

            else if (type == "0")
            {
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(certno) && !string.IsNullOrEmpty(cell))
                {

                    
                    string s = $"https://openauth.alipay.com/oauth2/publicAppAuthorize.htm?app_id=" + zhifubao.appId + "&scope=auth_user,auth_ecard&redirect_uri=" + HttpUtility.UrlEncode(@"http://www.newxiaoyuan.com/1.html?school_id=" + school_id + "&type=" + type+ "&name="+ name + "&certno="+ certno+ "&cell="+ cell);
                    Log.Info("地址2:"+s);
                    Response.Redirect(s);
                    Log.Info("地址2结束"+"_"+ name);
                }


            }



        }

        /// <summary>
        /// 用户授权回调地址
        /// </summary>
        /// <param name="auth_code">授权码</param>
        /// <param name="school_id">学校编号</param>
        /// <param name="type">是否授权1已授权,2未授权</param>
        /// <param name="name">姓名</param>
        /// <param name="certno">身份证号</param>
        /// <param name="cell">电话</param>
        /// <param name="url">领卡成功展示地址</param>
        /// <returns></returns>
        [HttpGet]
        public void UserOpenSchoolCard(string auth_code, string school_id, int type, string name, string certno, string cell, string url)
        {
            Log.Info("未解码:" + name+"|"+ Request.Query["name"].ToString());
            name = HttpUtility.UrlDecode(Request.Query["name"],System.Text.Encoding.UTF8);

            Log.Info("回调:"+auth_code+"type="+ type+"name="+ name);
            if (auth_code != "")
            {
                //获取授权令牌
                Aop.Api.Response.AlipaySystemOauthTokenResponse token = CashierConfig.getUserToken(auth_code, school_id);
                string alipay_user_info = token.Body;
                Log.Info("授权令牌:" + alipay_user_info);

                Dictionary<string, Object> objt = JsonConvert.DeserializeObject<Dictionary<string, Object>>(alipay_user_info);

              
                Dictionary<string, Object> response_json = JsonConvert.DeserializeObject<Dictionary<string, Object>>(objt["alipay_system_oauth_token_response"].ToString());
                string access_token = response_json["access_token"].ToString();
                string user_id = response_json["user_id"].ToString();
                //用户支付宝用户信息
                Aop.Api.Response.AlipayUserInfoShareResponse userinfo = CashierConfig.getUserInfo(access_token, school_id);
                string user_info = userinfo.Body;
                Log.Info("授权用户信息:" + user_info);

                Dictionary<string, Object> obj2 = JsonConvert.DeserializeObject<Dictionary<string, Object>>(user_info);
                Dictionary<string, Object> response_json2 = JsonConvert.DeserializeObject<Dictionary<string, Object>>(obj2["alipay_user_info_share_response"].ToString());


                CashierConfig.SetCode();
                Log.Info("继续下");

                string user_name = "";
                string cert_no = "";
                string scell = "";
                //如果是已授权学校直接领卡
                if (type == 1)
                {
                    try
                    {
                        user_name = response_json2["user_name"].ToString();
                        cert_no = response_json2["cert_no"].ToString();
                    }
                    catch
                    {
                        Response.Redirect(CashierConfig.XYZY + "/web/card/error.html?msg="+ HttpUtility.UrlEncode("支付宝接口获取不到人员信息!"));


                    }
                }
                //如果是为授权学校需要手动填写信息后领卡
                else
                {

                
                    Log.Info("看值:" + name + "|" + certno + "|" + cell);
                    if (!string.IsNullOrEmpty(certno))
                    {
                       
                        cert_no = certno;
                        scell = cell;
                        Log.Info("未授权信息:" + user_name + "|" + cert_no + "|" + scell);
                    }
                    else
                    {

                        Response.Redirect(CashierConfig.XYZY + "/web/card/error.html?msg="+ HttpUtility.UrlEncode("获取不到填入的领卡信息!"));

                    }



                }

                if (school_id == "9999")
                {
                    var deluser = _tb_school_userService.FindByClause(p => p.ali_user_id == user_id);
                    _tb_school_userService.Delete(deluser);
                    var usero = _tb_school_departmentService.FindByClause(p => p.schoolcode == school_id && p.p_id != 0);

                    string uname = usero.name;
                    tb_school_user info = new tb_school_user();
                    info.school_id = school_id;
                    info.class_id = 1;
                    info.user_name = "zhangsan";
                    info.passport = cert_no;
                    info.student_id = DateTime.Now.ToString("yyyyMMddhhmmss");
                    info.card_add_id = 1;
                    info.card_state = 0;
                    info.card_validity = DateTime.Now.AddYears(1);
                    info.create_time = DateTime.Now;
                    info.department = uname;
                    _tb_school_userService.Insert(info);

                }

                Log.Info("身份号:"+ cert_no);
                var userstudent = _tb_school_card_templateService.GetStudentSchoolCard(cert_no,school_id).FirstOrDefault();
                Log.Info("学生id:"+userstudent.card_show_name);
                user_name = userstudent.user_name;
                //if (userstudent.welcome_flg == "0")
                //{

                //    string stuid = userstudent.student_id;
                //    string schoolcode = userstudent.school_id;
                //    Response.Redirect(CashierConfig.XYZY + $"m/xchl/welcome_pay.html?stuid={ stuid }&schoolcode={schoolcode}");
                //}

                if (userstudent != null)
                {
                    string template_id = userstudent.template_id;
                    string card_show_name = userstudent.card_show_name;
                    string student_id = userstudent.student_id;
                    string card_validity = userstudent.card_validity;
                    string school_user_id = userstudent.school_user_id;
                    string dept = userstudent.department;
                    alipay_marketing_card_open_Model.gotojsonbean gt = new alipay_marketing_card_open_Model.gotojsonbean();
                    gt.card_template_id = template_id;
                    gt.user_uni_id = user_id;
                    gt.external_card_no = student_id;
                    gt.open_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    gt.valid_date = DateTime.Parse(card_validity).ToString("yyyy-MM-dd HH:mm:ss");
                    gt.name = user_name;//"LT";                                  //String可选64姓名 李洋 
                                        //gt.gende = gender;//"MALE";                             //String可选32性别（男：MALE；女：FEMALE） MALE 
                    gt.dept = dept;

                    //用户领卡
                    string request_user = SchoolCardHelper.OpenSchoolCard(gt, access_token, userstudent.school_id);
                    Log.Info("领卡后信息:" + request_user);

                    Dictionary<string, Object> user_obj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(request_user);
                    
                    Dictionary<string, Object> user_response_json = JsonConvert.DeserializeObject<Dictionary<string, Object>>(user_obj["alipay_marketing_card_open_response"].ToString());
                    Dictionary<string, Object> card_info = new Dictionary<string, object>();
                    try
                    {
                      
                        card_info = JsonConvert.DeserializeObject < Dictionary<string, Object> >(user_response_json["card_info"].ToString());
                      
                    }
                    catch
                    {
                        Response.Redirect(CashierConfig.XYZY + "/web/card/error.html?msg=用户信息不存在!");

                    }
                    string biz_card_no = card_info["biz_card_no"].ToString();
                    Log.Info("卡号:" + biz_card_no);
                  
                    
                    //领卡后修改用户信息
                    tb_school_user uinfo = _tb_school_userService.FindListByClause(p => p.passport == cert_no, p=>p.user_id, OrderByType.Asc).FirstOrDefault();
                    Log.Info("修改的用户:"+uinfo.user_name+ uinfo.school_id);
                    uinfo.biz_card_no = biz_card_no;
                    uinfo.ali_user_id = user_id;
                    uinfo.card_state = 1;
                    uinfo.cell = cell;
                    uinfo.Collarcard_time = DateTime.Now;
                    if (scell != "")
                        uinfo.cell = scell;
                    _tb_school_userService.Update(uinfo);
                    Log.Info("修改完成-----");
                    //查询卡信息
                    alipay_marketing_card_query_Model query = new alipay_marketing_card_query_Model();
                    query.target_card_no = card_info["biz_card_no"].ToString();
                    query.target_card_no_type = "BIZ_CARD";
                    string request_query = SchoolCardHelper.SchoolCardQuery(query, uinfo.school_id);

                    Log.Info("查询卡信息:" + request_query);
                    Dictionary<string, Object> query_obj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(request_query);
                    
                    Dictionary<string, Object> query_response_json = JsonConvert.DeserializeObject<Dictionary<string, Object>>(query_obj["alipay_marketing_card_query_response"].ToString());
                  
                    string schema_url = query_response_json["schema_url"].ToString();
                    Log.Info("查询地址:"+ schema_url);
                    user_name = System.Web.HttpUtility.UrlEncode(user_name, System.Text.Encoding.UTF8);//
                    student_id = System.Web.HttpUtility.UrlEncode(student_id, System.Text.Encoding.UTF8);//
                    card_show_name = System.Web.HttpUtility.UrlEncode(card_show_name, System.Text.Encoding.UTF8);//
                                                                                                                 //schema_url = System.Web.HttpUtility.UrlEncode(schema_url, System.Text.Encoding.UTF8);//
                    schema_url = schema_url.Replace('=', '*');
                    schema_url = schema_url.Replace('&', '(');

                    Response.Redirect(CashierConfig.XYZY+ "web/card/success.html?user_name=" + user_name + "&student_id=" + student_id + "&card_show_name=" + card_show_name + "&schema_url=" + schema_url + " ");

                }
                else
                {
                    Response.Redirect(CashierConfig.XYZY+ "web/card/error.html?msg=领卡失败!");
                }


            }


        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="certno">身份证号</param>
        /// <param name="school_id">学校编号</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CheckSchoolUser(string name, string certno, string school_id)
        {
            if (school_id != "9999")
            {
                var user = _tb_school_userService.FindListByClause(p => p.user_name == name && p.school_id == school_id && p.passport == certno, p => p.user_id, OrderByType.Asc).FirstOrDefault();
                if (user != null)
                {
                    return Json(new
                    {
                        code = "10000",
                        return_msg = "用户存在"

                    });
                }
                else
                {
                    return Json(new
                    {
                        code = "10001",
                        return_msg = "用户不存在"

                    });

                }
            }
            else
            {
                return Json(new
                {
                    code = "10000",
                    return_msg = "用户存在"

                });

            }
        }








        /// <summary>
        /// 校园卡种列表查询
        /// </summary>
        /// <param name="obj">school_id 学校编号</param>
        /// <returns></returns>
        [HttpPost]

        public IActionResult GetSchoolCardQuery([FromBody]JObject obj)
        {

            string school_id = obj["school_id"] + "";
            string name= obj["name"] + "";
            tb_school_card_templateService card_server = new tb_school_card_templateService();
            var list= card_server.GetSchoolCardList(school_id);
            if (!string.IsNullOrEmpty(name))
            {
                return Json(list.Where(p => p.card_show_name.Contains(name)));

            }
            else
            {
                return Json(list);

            }
        }


        /// <summary>
        /// 通过ID获取卡模板基本信息
        /// </summary>
        /// <param name="obj">card_add_id|卡ID和school_id学校编号</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult GetSchoolCarBaseInfo([FromBody]JObject obj)
        {
            string CardId = obj["card_add_id"] + "";
            string SchoolId = obj["school_id"] + "";

            tb_school_card_template tp = _tb_school_card_templateService.FindByClause(p => p.Card_add_ID == Convert.ToInt32(CardId) && p.School_ID == SchoolId);
            return Json(new
            {
                name = tp.card_show_name,
                info = tp.T_template_benefit_info,
                layout = tp.column_info_layout,
                code = "10000",
                card_add_id=tp.Card_add_ID,
                return_msg = "查询成功",
            });
        }

        /// <summary>
        /// 修改卡基本信息
        /// </summary>
        /// <param name="obj">card_add_id|卡ID和school_id学校编号|info描述|layout布局|name卡名</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult UpdateSchoolCarBaseInfo([FromBody]JObject obj)
        {
            string CardId = obj["card_add_id"] + "";
            string SchoolId = obj["school_id"] + "";


            tb_school_card_template tp = _tb_school_card_templateService.FindByClause(p => p.Card_add_ID == Convert.ToInt32(CardId) && p.School_ID == SchoolId);
            tp.T_template_benefit_info = obj["info"] + "";
            tp.column_info_layout = obj["layout"] + "";
            tp.card_show_name = obj["name"] + "";
            if (_tb_school_card_templateService.Update(tp))
            {
                PushSchoolCard("1", "2", tp.ID+"");
                return Json(new
                {
                    code = "10000",
                    msg = "修改成功",

                });
            }
            else
            {
                return Json(new
                {
                    code = "10001",
                    msg = "修改失败",

                });

            }


        }



        /// <summary>
        /// 创建校园卡
        /// </summary>
        /// <param name="obj">school_id学校编号|info描述|layout布局|name卡名</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult CreateSchoolCard([FromBody]JObject obj)
        {
            string info = obj["info"] + "";

            string name = obj["name"] + "";

            string layout = obj["layout"] + "";

            string schoolid = obj["school_id"] + "";

            if(_tb_school_card_templateService.FindByClause(p=>p.card_show_name==name&&p.School_ID==schoolid)==null)
            {

                Dictionary<string, string> dic = SchoolCardHelper.CreateSchoolCard(info, name, layout, schoolid);
                PushSchoolCard("1", "1", dic.Keys.First());

                return Json(dic.Values.First());

            }
            else
            {

                return Json(new
                {
                    code = "10001",
                    msg = "卡名已存在",

                });
            }

           

        }

        /// <summary>
        /// 校园卡发布
        /// </summary>
        /// <param name="obj">school_id学校编号|baseinfoshow是否显示基本信息0否1是|card_add_id卡id|column_info_list节点json|card_action_list按钮json</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult UpdateSchoolCard([FromBody]JObject obj)
        {
            try
            {
                string schoolid = obj["school_id"] + "";
                bool baseinfoshow = Convert.ToBoolean(Convert.ToInt32(obj["baseinfoshow"] + ""));

                int card_add_id = Convert.ToInt32(obj["card_add_id"] + "");
                var card = SchoolCardHelper.GetSschoolCardInfo(schoolid, card_add_id);
                card.column_info_list = JsonConvert.DeserializeObject<card_template_model.column_info_model[]>(obj["column_info_list"] + "");
                card.card_action_list = JsonConvert.DeserializeObject<card_template_model.card_action_model[]>(obj["card_action_list"] + "");
                card.front_text_list_enable = baseinfoshow;

                card.logo_id = obj["Logo_id"] + "";

                card.background_id = obj["background_id"] + "";
                string retrun = SchoolCardHelper.ModifySchoolCard(card, schoolid);

                


                return Json(new
                {
                    code = "10000",
                    msg = "success",
                    body = retrun
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    code = "10001",
                    msg = e.Message
                });
            }
        }





        /// <summary>
        /// 获取已上传图片列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Authorize]
        public IActionResult GetIconList()
        {
            return Json(new
            {
                code = "10000",
                msg = "查询成功",
                data = _tb_alipay_imageService.FindListByClause(p => p.type == 1, p => p.image_id, OrderByType.Asc)
            });
        }

        /// <summary>
        /// 获取当前校园的导航库
        /// </summary>
        /// <param name="obj">school_id</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult GetSchoolCardColumn([FromBody]JObject obj)
        {
            string schoolid = obj["school_id"] + "";
            var cardcolum = _tb_school_card_template_columnService.FindListByClause(p => p.School_ID == schoolid, p => p.ColumId, OrderByType.Asc);
            return Json(cardcolum);
        }

        /// <summary>
        /// 修改模板导航
        /// </summary>
        /// <param name="obj">导航节点id|T_column_info导航内容json</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult UpdateSchoolCardColum([FromBody]JObject obj)
        {
            string id = obj["ColumId"] + "";
            string T_column_info = obj["T_column_info"] + "";
            tb_school_card_template_column column = _tb_school_card_template_columnService.FindByClause(p => p.ColumId == id);
            column.T_column_info = T_column_info;

            bool tag = _tb_school_card_template_columnService.Update(column);
            if (tag)
                return Json(new
                {
                    code = "10000",
                    msg = "修改成功"
                });
            else
                return Json(new
                {
                    code = "10001",
                    msg = "修改失败"
                });
        }

        /// <summary>
        /// 新增模板导航
        /// </summary>
        /// <param name="obj">导航节点id|T_column_info导航内容json|school_id</param>
        /// <returns></returns>
        [HttpPost] 
        public IActionResult AddSchoolCardColum([FromBody]JObject obj)
        {

            string T_column_info = obj["T_column_info"] + "";
            string schoolid = obj["school_id"] + "";
            tb_school_card_template_column colums = new tb_school_card_template_column();
            colums.ColumId = Guid.NewGuid().ToString();
            colums.School_ID = schoolid;
            colums.T_column_info = T_column_info;
            _tb_school_card_template_columnService.Insert(colums);
            return Json(new
            {
                code = "10000",
                msg = "新增成功"
            });


        }

        /// <summary>
        /// 查询卡信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult QueryCard(string code)
        {

            Aop.Api.IAopClient client = CashierConfig.getDefaultAopClient(code);


            Aop.Api.Request.AlipayMarketingCardTemplateQueryRequest request = new Aop.Api.Request.AlipayMarketingCardTemplateQueryRequest();
            request.BizContent = "{" +
            "\"template_id\":\"20180828000000001154115000300937\"" +
            "  }";
            Aop.Api.Response.AlipayMarketingCardTemplateQueryResponse response = client.Execute(request);

            Dictionary<string, Object> objt = JsonConvert.DeserializeObject<Dictionary<string, Object>>(response.Body);

            return Json(response.Body);
        }

        /// <summary>
        /// 删除模板导航
        /// </summary>
        /// <param name="obj">导航模板id|school_id</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult DelSchoolCardColum([FromBody]JObject obj)
        {
            string id = obj["ColumId"] + "";
            string schoolid = obj["school_id"] + "";
            var colums = _tb_school_card_template_columnService.FindByClause(p=>p.ColumId==id);
            bool tag = _tb_school_card_template_columnService.Delete(colums);
            if (tag)
                return Json(new
                {
                    code = "10000",
                    msg = "删除成功"
                });
            else
                return Json(new
                {
                    code = "10001",
                    msg = "删除失败"
                });
        }

        /// <summary>
        /// 保存校园卡模板
        /// </summary>
        /// <param name="obj">school_id|Logo_id|column_info_list|card_action_list|layout|background_id|baseinfoshow</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Authorize]
        public IActionResult SaveSchoolCardTempFormer([FromBody]JObject obj)
        {
            int id =Convert.ToInt32(obj["id"] + "");
            string schoolid = obj["school_id"] + "";
            string Logo_id = obj["Logo_id"] + "";
            string T_column_info_list = obj["column_info_list"] + "";
            string T_card_action_list = obj["card_action_list"] + "";
            string column_info_layout = obj["layout"] + "";
            string background_id = obj["background_id"] + "";
            bool baseinfoshow = Convert.ToBoolean(Convert.ToInt32(obj["baseinfoshow"] + ""));
            tb_school_card_template_former former = new tb_school_card_template_former();

            if (id == 0)
            {
              

                former.schoolid = schoolid;
                former.Logo_id = Logo_id;
                former.T_column_info_list = T_column_info_list;
                former.T_card_action_list = T_card_action_list;
                former.column_info_layout = column_info_layout;
                former.background_id = background_id;
                former.baseinfoshow = baseinfoshow;
                _tb_school_card_template_formerService.Insert(former);
            }
            else
            {
                former = _tb_school_card_template_formerService.FindByClause(p => p.id == id);
                former.schoolid = schoolid;
                former.Logo_id = Logo_id;
                former.T_column_info_list = T_column_info_list;
                former.T_card_action_list = T_card_action_list;
                former.column_info_layout = column_info_layout;
                former.background_id = background_id;
                former.baseinfoshow = baseinfoshow;
                _tb_school_card_template_formerService.Update(former);
            }
           


           
           

            return Json(new
            {
                code = "10000",
                msg = "模板保存成功"
            });
        }
        /// <summary>
        /// 获取校园卡模板
        /// </summary>
        /// <param name="obj">school_id</param>
        /// <returns></returns>
        [HttpPost]
       
        public IActionResult GetSchoolCardTempFormer([FromBody]JObject obj)
        {
            string schoolid = obj["school_id"] + "";
            var data =  _tb_school_card_template_formerService.GetSchoolCardTempFormer(schoolid);
           

            //var formerlist = _tb_school_card_template_formerService.FindListByClause(p => p.schoolid == schoolid, p => p.id, OrderByType.Asc);
            return Json(data);
        }





        /// <summary>
        /// 获取当前设置的卡模板配置
        /// </summary>
        /// <param name="obj">school_id|card_add_id</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult GetSschoolTemp([FromBody]JObject obj)
        {
            int card_add_id = Convert.ToInt32(obj["card_add_id"] + "");
            string schoolid = obj["school_id"] + "";

            var m = SchoolCardHelper.GetSschoolCardInfo(schoolid, card_add_id);
            var bgurl = _tb_alipay_imageService.FindByClause(p => p.alipay_id == m.background_id);
            var logourl = _tb_alipay_imageService.FindByClause(p => p.alipay_id == m.logo_id);
            return Json(new
            {
                code = "0",
                msg = "success",
                m.column_info_list,
                m.card_action_list,
                layout = m.column_info_layout,
                bgurl = bgurl == null?"": bgurl.alipay_url,
                logourl = logourl==null?"": logourl.alipay_url,
                cardname = m.card_show_name,
                background_id=m.background_id,
                logo_id=m.logo_id,
                baseinfoshow=Convert.ToInt32(m.front_text_list_enable)
            });

        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="Files"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [HttpPost("Upload")]
        public IActionResult Upload(IFormCollection Files, int index)
        {

            try
            {
                //var form = Request.Form;//直接从表单里面获取文件名不需要参数
                int indexc = index;
              

                string Name = Request.Form["name"]+"";
                string Schoolcode = Request.Form["school_id"]+"";
             

                var form = Files;//定义接收类型的参数
                Hashtable hash = new Hashtable();
                IFormFile file = Request.Form.Files[indexc];
                if (file == null)
                {
                    return Json(new { code = "10003", msg = "请选择要上传的文件!" });
                }


                //定义图片数组后缀格式
                string[] LimitPictureType = { "jpg", "png", "gif" };
                //获取图片后缀是否存在数组中

                string fileName = file.FileName;
                string suffix = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();/*获取后缀名并转为小写： jpg*/



                if (LimitPictureType.Contains(suffix))
                {

                    //为了查看图片就不在重新生成文件名称了
                    // var new_path = DateTime.Now.ToString("yyyyMMdd")+ file.FileName;


                    string imgtype = "";
                    switch (suffix)
                    {
                        case "jpg":
                            imgtype = "jpg";
                            break;
                        case "png":
                            imgtype = "png";
                            break;
                        case "gif":
                            imgtype = "gif";
                            break;
                    }
                    string logo = "";
                    if (indexc==0)
                     logo = DateTime.Now.Ticks.ToString() + "_" + Name + "_logo." + imgtype;
                    else
                        logo = DateTime.Now.Ticks.ToString() + "_" + Name + "_background." + imgtype;

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "alipay_image", logo);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        //再把文件保存的文件夹中
                        file.CopyTo(stream);
                    }
                        string request = SchoolCardHelper.UploadImage(Name, path, imgtype, Schoolcode);

                        Dictionary<string, Object> objt = JsonConvert.DeserializeObject<Dictionary<string, Object>>(request);


                        Dictionary<string, Object> response_json = JsonConvert.DeserializeObject<Dictionary<string, Object>>(objt["alipay_offline_material_image_upload_response"].ToString());
                        string code = response_json["code"].ToString();
                        if (code == "10000")
                        {
                            string image_id = response_json["image_id"].ToString();
                            string image_url = response_json["image_url"].ToString();
                            //查看是否存在
                            var img = _tb_alipay_imageService.FindByClause(p => p.alipay_id == image_id);
                            if (img != null)
                            {
                                img.alipay_url = image_url;
                                img.url = "/alipay_image/" + logo;
                                _tb_alipay_imageService.Update(img);
                            }
                            else
                            {
                                tb_alipay_image img1 = new tb_alipay_image();
                                img1.alipay_url = image_url;
                                img1.alipay_id = image_id;
                                img1.type = 0;
                                img1.url = "/alipay_image/" + logo;
                                _tb_alipay_imageService.Insert(img1);

                            }



                            return Json(new
                            {
                                code = "10000",
                                msg = new
                                {
                                    image_id = image_id,
                                    image_url = image_url,
                                    url = "/alipay_image/" + logo
                                }
                            });


                        }

                        else
                        {
                            return Json(new
                            {
                                code = code,
                                msg = "上传支付宝失败"
                            });

                        }
                    
                }
                else
                {
                    return Json(new { code = "10003", msg = "只能上传*.jpg,*.png,*.gif!" });
                }
            }
            catch (Exception)
            {
                return Json(new { code = "10003", msg = "上传失败" });
            }
        }

        /// <summary>
        /// 上传本地ICO
        /// </summary>
        /// <param name="schoolid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UploadIco(string schoolid)
        {


            string imgtype = "*.JPG|*.PNG";
            string[] ImageType = imgtype.Split('|');
            string schoolcode = schoolid;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Icon_image");
            //判断是否上传成功
            bool tag = true;
            for (int i = 0; i < ImageType.Length; i++)
            {
                string[] dirs = Directory.GetFiles(path, ImageType[i]);


                foreach (string dir in dirs)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(dir);


                    string request = SchoolCardHelper.UploadImage(fi.Name, fi.FullName, fi.Name.Substring(fi.Name.Length - 3, 3), schoolcode);


                    Dictionary<string, Object> objt = JsonConvert.DeserializeObject<Dictionary<string, Object>>(request);
                   
                    Dictionary<string, Object> response_json = JsonConvert.DeserializeObject<Dictionary<string, Object>>(objt["alipay_offline_material_image_upload_response"].ToString());
                    string code = response_json["code"].ToString();
                    if (code == "10000")
                    {
                        string image_id = response_json["image_id"].ToString();
                        string image_url = response_json["image_url"].ToString();
                        //查看是否存在
                        var img = _tb_alipay_imageService.FindByClause(p => p.alipay_id == image_id);
                        if (img != null)
                        {
                            img.alipay_url = image_url;
                            img.url = "/Icon_image/" + fi.Name;
                            _tb_alipay_imageService.Update(img);
                        }
                        else
                        {
                            tb_alipay_image img1 = new tb_alipay_image();
                            img1.alipay_url = image_url;
                            img1.alipay_id = image_id;
                            img1.type = 1;
                            img1.url = "/Icon_image/" + fi.Name;
                            _tb_alipay_imageService.Insert(img1);

                        }

                    }
                    else
                    {
                        tag = false;
                    }

                }
            }

            if (tag)
                return Json("上传成功");
            else
                return Json("上传失败");


        }







        /// <summary>
        /// 往喜云推送卡和人员信息
        /// </summary>
        /// <param name="datatype">数据类型1:卡类型，2:学生信息</param>
        /// <param name="operationtype">操作符1:新增,2:修改,3:删除</param>
        /// <param name="id">新增的卡种id或者用户id</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PushSchoolCard(string datatype = "", string operationtype = "", string id = "")
        {
            try
            {
                count = 0;
                pcount = 0;
                Object result = null;

                //如果卡类型为空推送所有数据
                if (datatype == "")
                {
                    //推送卡类型
                    var schooltype = _schoolcodrservice.SchoolType();

                    foreach (var item in schooltype)
                    {
                        //获取该学校所有卡种
                        var schoolCrdtype = _schoolcodrservice.SchoolCrdType();

                        var biz_content = schoolCrdtype.Where(p => p.school_id == item.school_id).Select(p => new { card_type_id = p.card_type_id, card_type_name = p.card_type_name }).ToList();

                        var sbiz_content = new { card_type_list = biz_content, pid = item.pid, type = operationtype };

                        // result = PushInfo(sbiz_content, "xy.push.cardtype");
                        //推送当前学校学生信息


                        var userlist = _tb_school_userService.FindListByClause(p => p.school_id == item.school_id && !SqlFunc.IsNullOrEmpty(p.biz_card_no) && !SqlFunc.IsNullOrEmpty(p.ali_user_id), p => p.user_id, SqlSugar.OrderByType.Asc);


                        foreach (var uitem in userlist)
                        {
                            pcount = pcount + 1;
                            var usbiz_content = new { card_type_id = uitem.card_add_id.ToString(), uid = uitem.ali_user_id, card_no = uitem.biz_card_no, gender = "", nationality = "", user_name = uitem.user_name, pid = item.pid, type = operationtype, phoneno = "", user_no = uitem.student_id };
                            //  PushInfo(usbiz_content, "xy.push.idinfo");
                        }
                    }
                }
                //推送卡类型
                else if (datatype == "1")
                {
                    //获取pid
                    var schooltype = _schoolcodrservice.SchoolTypebyid(id).FirstOrDefault();


                    var schoolCrdtype = _schoolcodrservice.SchoolCrdTypebyid(id);

                    var biz_content = schoolCrdtype.Select(p => new { card_type_id = p.card_type_id, card_type_name = p.card_type_name }).ToList();

                    var sbiz_content = new { card_type_list = biz_content, pid = schooltype.pid == null ? "" : schooltype.pid, type = operationtype };

                 //   result = PushInfo(sbiz_content, "xy.push.cardtype");

                }
                //推送单个学生
                else if (datatype == "2")
                {

                    //推送当前学校学生信息
                    foreach (var item in id.Split(','))
                    {

                        try
                        {
                            if (item != "")
                            {
                                var user = _tb_school_userService.FindById(Convert.ToInt32(item));
                                //   var user = _tb_school_userService.FindByClause(p => p.ali_user_id == item);

                                if (user != null)
                                {
                                    var school = _tb_school_InfoService.FindByClause(p => p.School_Code == user.school_id);

                                    var usbiz_content = new { card_type_id = user.card_add_id.ToString(), uid = user.ali_user_id, card_no = user.biz_card_no, gender = "", nationality = "", user_name = user.user_name, pid = school.pid, type = operationtype, phoneno = "", user_no = user.student_id };
                                  //  PushInfo(usbiz_content, "xy.push.idinfo");
                                }
                            }



                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                        }
                    }




                }
                return Json(new
                {

                    msg = "调用成功"
                });

            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {

                    msg = ex.ToString()
                });
            }
            finally
            {

                Log.Info("++++++++++\n总成功数" + count + "\n");
                Log.Info("++++++++++\n总人数" + pcount + "\n");
            }


        }


        private Object PushInfo(Object biz_content, string method)
        {
            string appid = "10000925";
            string url = "https://openapi.xiyun.net/push/gateway.do";
            string privateKey = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAKusBxjLQrHcHopGp0xqNxcSrJpp4dp35obWW+BwdefFFzKoRIwRGzrnUvFDGuoSl3v3zl8Y1MFUNzo+hXjuXeMzs2DO78AAuoYgvfVybY/Fs98FqTR60RboL7dCHZowCi4iHGa0kOSMI4IHrws2iPUGETF+m4wjlcEkYCWMmsmXAgMBAAECgYEApLmj8amQLKQfdeVHwK4mgHA9yMkSm5PzUqy7akffCu72THhjldcQPtwanUAbwkDmtGBa2Ks18vBBMhUt85Ud5kG2Dm8v1KbaCY3/T5GtIGV4vmffUzUn+g7a6TIQDL/DJt7QpoLD3hPY0QFo+CWcHPyV1bxoLR5pQWMhHK7YOXkCQQDnj9tvB6X5IQzoPiBE7cfm+KS5IRYJsi/JqRI0eyPDLEbCNSxik8P9bnGBHNkPqly3M6mtnDFkXqMxK6O9CzwtAkEAvcodYs/s1N5yDTnZ65/sqR3iCvuZNO1jJhPgh5DfdseoX4bOgiFGBwL6glqMVjJu5VLFlWIb3n+yw97V06DDUwJALOeb0RM1n3NGUn9BuLw3yNWs8+2znVu9oqizzBOZIs8iRaUUH2WyWyIgxr32ZfBOnIRbQjyI44LquK1SymU0XQJANbdf69i3ymQPWcj28ea4wADyOnONoFpUBrH3iccqSF8oO6lXB6PNQyzHpF9mevsZQhTUUXyMMMRp32BRmu1UewJAYyb9faJWAANnsE54LPyDOc/3CTxC3ggGDEItZB1zZ6gXD+JvxXwje42IHFJ1CDBYTfikOaq5hFKJ3bDw7doFrg==";
            string sign_type = "RSA";
            string charset = "UTF-8";
            string version = "1.0";
            long timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            var client = new RestClient(url);
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json");
            request.Parameters.Clear();

            IDictionary<string, string> paramsMap = new Dictionary<string, string>();

            paramsMap.Add("appid", appid);
            paramsMap.Add("method", method);
            paramsMap.Add("charset", charset);
            paramsMap.Add("sign_type", sign_type);
            paramsMap.Add("version", version);
            paramsMap.Add("timestamp", timestamp.ToString());
            paramsMap.Add("biz_content", JsonConvert.SerializeObject(biz_content));

            string singstr = "";

            singstr = RSAHelper.RSASign(paramsMap, privateKey, charset, sign_type);

            //请求字符串
            var reqjson = new
            {
                charset = charset,
                biz_content = biz_content,
                method = method,
                appid = appid,
                sign_type = sign_type,
                version = version,
                timestamp = timestamp.ToString(),//时间戳
                sign = singstr
            };

            request.AddJsonBody(reqjson);
            string str = JsonConvert.SerializeObject(reqjson);

            var response = client.Execute(request);


            if (((JObject)JsonConvert.DeserializeObject(response.Content))["code"] + "" == "10000")
            {
                count = count + 1;

            }



            return reqjson;
        }

        /// <summary>
        /// 喜云人员核对查询接口
        /// </summary>
        /// <param name="method"></param>
        /// <param name="sign"></param>
        /// <param name="appId"></param>
        /// <param name="pid"></param>
        /// <param name="timestamp"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult QuerySchooluserInfo(string method, string sign, string appId, string pid, string timestamp, string uid)
        {
            var schoolinfo = _tb_school_InfoService.FindByClause(p => p.app_id == appId);

            if (schoolinfo == null)
            {
                return Json(new
                {
                    code = "20000",
                    msg = "学校信息未注册."
                });

            }


            string raw = "appId=" + appId + "&timestamp=" + timestamp + "&publicKey=" + schoolinfo.publicKey;

            string signs = MD5Helper.MD5Encrypt16(raw).ToUpper();

            if (sign == signs)
            {
                if (method == "queryuser")
                {
                    string mz = "01";
                    string gender = "";
                    var uinfo = _tb_school_userService.FindByClause(p => p.ali_user_id == uid);
                    var temp = _tb_school_card_templateService.FindByClause(p => p.School_ID == uinfo.school_id && p.Card_add_ID == uinfo.card_add_id);
                    return Json(new
                    {
                        code = "10000",
                        msg = "Success",
                        user_no = schoolinfo,
                        user_name = uinfo.user_name,
                        gender = gender,
                        nationality = mz,
                        identity_type = uinfo.class_id,
                        identity_name = temp.card_show_name,
                        phoneno = "",
                        campuscard = uinfo.biz_card_no,
                        sign = signs


                    });

                }
                else
                {
                    return Json(new
                    {
                        code = "20000",
                        msg = "学校信息未注册."
                    });
                }

            }
            else
            {
                return Json(new
                {
                    code = "20000",
                    msg = "Service Currently Unavailable.",
                    sub_code = "isp.unknow-error",
                    sub_msg = "信息没找到"
                });

            }

        }

        /// <summary>
        /// 修改学校名称
        /// </summary>
        /// <param name="name">修改名称</param>
        /// <param name="school_id">学校编号</param>
        /// <returns></returns>

        [HttpGet]
        [Authorize]
        public IActionResult UpdateDeptName(string name,string school_id)
        {
            var school= _tb_school_departmentService.FindByClause(p => p.schoolcode == school_id);

            try
            {
                if (school == null)
                {
                    tb_school_department sc = new tb_school_department();
                    sc.name = name;
                    sc.p_id = 0;
                    sc.treeLevel = 0;
                    sc.schoolcode = school_id;
                    _tb_school_departmentService.Insert(sc);

                }
                else
                {
                    school.name = name;
                    _tb_school_departmentService.Update(school);
                }

                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UpdateFail
                });
            }

        }

        /// <summary>
        /// 校园卡发卡量统计
        /// </summary>
        /// <param name="school_id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SchoolCardCount(string school_id,int type)
        {
           var  userlist=  _tb_school_userService.FindListByClause(p => p.school_id == school_id, p => p.user_id, OrderByType.Asc);
            int card_count =0;
            int registered_count = 0;
            int unregistered_count = 0;
            if (type == 0)
            {
                card_count=userlist.Count();
                registered_count = userlist.Where(p => p.card_state == 1).Count();
                 unregistered_count = userlist.Where(p => p.card_state == 0).Count();
            }
            else if(type==1)
            {
                //教师未领卡
                var lsobj = _tb_school_card_templateService.FindByClause(p => p.card_show_name == "教师卡" && p.School_ID == school_id);
                if (lsobj != null)
                {
                    card_count = userlist.Where(p => p.card_id == lsobj.ID).Count();
                    registered_count = userlist.Where(p => p.card_state == 1 && p.card_id == lsobj.ID).Count();
                    unregistered_count = userlist.Where(p => p.card_state == 0 && p.card_id == lsobj.ID).Count();
                }

            }
            else if(type==2)
            {
                var xsobj = _tb_school_card_templateService.FindByClause(p => p.card_show_name == "学生卡" && p.School_ID == school_id);
                if (xsobj != null)
                {
                    card_count = userlist.Where(p =>p.card_id == xsobj.ID).Count();
                    registered_count = userlist.Where(p => p.card_state == 1 && p.card_id == xsobj.ID).Count();
                    unregistered_count = userlist.Where(p => p.card_state == 0 && p.card_id == xsobj.ID).Count();
                }

            }
            else if(type==3)
            {
                var qtobj = _tb_school_card_templateService.FindListByClause(p => p.card_show_name !="学生卡"&& p.card_show_name != "教师卡" && p.School_ID == school_id,p=>p.ID, OrderByType.Asc).ToList();
                if(qtobj!=null)
                {
                    foreach (var item in qtobj)
                    {
                        card_count += userlist.Where(p => p.card_id == item.ID).Count();
                        registered_count += userlist.Where(p => p.card_state == 1 && p.card_id == item.ID).Count();
                        unregistered_count += userlist.Where(p => p.card_state == 0 && p.card_id == item.ID).Count();
                    }
                  
                }


            }


            return Json(new
            {
                code = JsonReturnMsg.SuccessCode,
                msg = "查询成功",
                card_count =card_count,
                registered_count=registered_count,
                unregistered_count=unregistered_count
            });
        }


        /// <summary>
        /// 获取领卡情况
        /// </summary>
        /// <param name="school_id"></param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SchoolCardGrowth(string school_id,string stime,string etime)
        {

            try
            {
                stime = stime + " 00:00:00";
                etime = etime + " 23:59:59";
                DateTime st= Convert.ToDateTime(stime);
                DateTime et = Convert.ToDateTime(etime);
                var data = _tb_school_userService.FindListByClause(p =>p.school_id == school_id && p.create_time >=SqlFunc.ToDate(st) && p.create_time <= SqlFunc.ToDate(et) && p.card_state == 1, p=>p.user_id, OrderByType.Asc);
              
                List<int> stulist = new List<int>();
                List<string> array_alldays = new List<string>();
                List<int> teacherlist = new List<int>();
                List<int> qtlist = new List<int>();
                if (!string.IsNullOrWhiteSpace(stime) && !string.IsNullOrWhiteSpace(etime))
                {
                  
                    int day = (et - st).Days;

                    int lsid = 0;
                    int stid = 0;
                    
                    var lsobj = _tb_school_card_templateService.FindByClause(p => p.card_show_name == "教师卡" && p.School_ID == school_id);
                    var xsobj = _tb_school_card_templateService.FindByClause(p => p.card_show_name == "学生卡" && p.School_ID == school_id);
                    var qtobj = _tb_school_card_templateService.FindListByClause(p => p.card_show_name != "学生卡" && p.card_show_name != "教师卡" && p.School_ID == school_id, p => p.ID, OrderByType.Asc).ToList();
                    if (lsobj!=null)
                    {
                        lsid = lsobj.ID;

                    }
                    if(xsobj!=null)
                    {
                        stid = xsobj.ID;
                    }


                    for (int c = 0; c <= day; c++)
                    {
                        array_alldays.Add(et.AddDays(-c).ToString("yyyy/MM/dd"));
                        int stul = data.Where(x =>x.card_id== lsid && x.create_time>= et.AddDays(-c)&& x.create_time <et.AddDays(-c).AddDays(1)).Count();
                        int teach = data.Where(x => x.card_id == stid && x.create_time >= et.AddDays(-c) && x.create_time <et.AddDays(-c).AddDays(1)).Count();
                        int qt = data.Where(x => x.card_id != stid&& x.card_id != lsid && x.create_time >= et.AddDays(-c) && x.create_time < et.AddDays(-c).AddDays(1)).Count();

                        stulist.Add(stul);
                        teacherlist.Add(teach);
                        qtlist.Add(qt);
                    }

                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        msg = JsonReturnMsg.GetSuccess,
                        rqs = array_alldays,
                        stulist = stulist,
                        teacherlist = teacherlist,
                        qilist= qtlist
                    });

                }
               else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }




        /// <summary>
        /// 删除校园卡
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult deleteSchoolCodr([FromBody]JObject obj)
        {
            try
            {
                var arrcard_add_id = obj["card_add_id"].ToString().Split(',');
                var arrbiz_card_no = obj["biz_card_no"].ToString().Split(',');
                var arrali_user_id = obj["ali_user_id"].ToString().Split(',');
                //DataTable dt = bll.schoolcard.card_template.getschoolcrad(schoolcode);
                for (int c = 0; c < arrcard_add_id.Length; c++)
                {
                    if (!string.IsNullOrEmpty(arrbiz_card_no[c].ToString()))
                    {
                        //DataRow[] dr = dt.Select($"card_add_id={arrcard_add_id[c].ToString()}");
                        //string card_no = dr[0]["template_id"].ToString();

                        card_template_deletemodel dm = new card_template_deletemodel()
                        {
                            out_serial_no = DateTime.Now.Ticks.ToString(),
                            target_card_no = arrbiz_card_no[c].ToString(),
                            target_card_no_type = "BIZ_CARD",
                            reason_code = "USER_UNBUND",
                            ext_info = "{\"donee_user_id\":\"" + arrali_user_id[c].ToString() + "\"}"
                        };
                        AlipayMarketingCardDeleteModel model = new AlipayMarketingCardDeleteModel();
                        model.OutSerialNo = dm.out_serial_no;
                        model.TargetCardNo = dm.target_card_no;
                        model.TargetCardNoType = dm.target_card_no_type;
                        model.ReasonCode = dm.reason_code;
                        model.ExtInfo = dm.ext_info;
                        var schoolinfo = _tb_school_InfoService.FindByClause(x => x.School_Code == obj["schoolcode"].ToString());
                        IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", schoolinfo.app_id, schoolinfo.private_key, "json", "1.0", "RSA2", schoolinfo.alipay_public_key, "utf-8", false);
                        AlipayMarketingCardDeleteRequest request = new AlipayMarketingCardDeleteRequest();
                        request.SetBizModel(model);
                        AlipayMarketingCardDeleteResponse response = client.Execute(request);
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
                Log.Error(ex.ToString());
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.DeleteFail
                });
            }
        }

    }
}