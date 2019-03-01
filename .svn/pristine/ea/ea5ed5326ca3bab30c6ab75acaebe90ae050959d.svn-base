using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SchoolWebApi.Utility;
using Service;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 数据校验
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class SchoolAuthInfoController : Controller
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private Itb_school_authService _tb_school_authService;
        private Itb_school_InfoService _tb_school_InfoService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_school_authService"></param>
        /// <param name="tb_school_InfoService"></param>
        public SchoolAuthInfoController(Itb_school_authService tb_school_authService,
            Itb_school_InfoService tb_school_InfoService
            )
        {
            _tb_school_authService = tb_school_authService;
            _tb_school_InfoService = tb_school_InfoService;
        }

        /// <summary>
        /// 授权回调地址进入获取token信息并入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SchoolAuthToRuoHuoSchool([FromBody]JObject obj)
        {
            try
            {
                string app_auth_code = obj["app_auth_code"].ToString();
                string app_id = obj["app_id"].ToString();
                //获取学校授权token等信息
                //alipay.open.auth.token.app.query(查询某个应用授权AppAuthToken的授权信息)
                AppHelper helper = new AppHelper("GetRuoHuoSchool");
                var response = helper.alipay_open_auth_token_app(app_auth_code);
                if (response == null)
                {
                    return null;
                }
                //var jsonObj = JObject.Parse(response.Body);

                alipay_open_auth_token_app_response_info response_info =
                    (alipay_open_auth_token_app_response_info)JsonConvert.DeserializeObject(response.Body, typeof(alipay_open_auth_token_app_response_info));
                tb_school_auth tbschoolauth = _tb_school_authService.FindByClause(x => x.ISV_appid == app_id && x.PID == response_info.alipay_open_auth_token_app_response.tokens[0].user_id);
                if (tbschoolauth == null)
                {
                    tbschoolauth = new tb_school_auth();
                    tbschoolauth.PID = response_info.alipay_open_auth_token_app_response.tokens[0].user_id;
                    tbschoolauth.ISV_appid = app_id;
                    tbschoolauth.app_auth_token = response_info.alipay_open_auth_token_app_response.tokens[0].app_auth_token;
                    _tb_school_authService.Insert(tbschoolauth);
                }
                else
                {
                    tbschoolauth.PID = response_info.alipay_open_auth_token_app_response.tokens[0].user_id;
                    tbschoolauth.ISV_appid = app_id;
                    tbschoolauth.app_auth_token = response_info.alipay_open_auth_token_app_response.tokens[0].app_auth_token;
                    _tb_school_authService.Update(tbschoolauth);
                }

                return Json(new
                {
                    code = "1",
                    msg = "商家授权成功！"
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "商家授权失败"
                });
            }
        }
        /// <summary>
        /// 获取app_auth_token
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="ISVappid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAppAuth_Token(string schoolcode, string ISVappid)
        {
            try
            {
                tb_school_info schoolinfo = _tb_school_InfoService.FindByClause(x => x.School_Code == schoolcode);
                if (schoolinfo == null)
                {
                    return Json(new
                    {
                        code = "0",
                        msg = "未能找到学校信息！"
                    });
                }
                string PID = schoolinfo.pid.ToString();
                tb_school_auth schoolauth = new tb_school_auth();
                schoolauth = _tb_school_authService.FindByClause(x => x.PID == PID && x.ISV_appid == ISVappid);
                if (schoolauth == null)
                {
                    return Json(new
                    {
                        code = "0",
                        msg = "未找到商户令牌信息，请联系商户是否授权！"
                    });
                }
                return Json(new
                {
                    code = "1",
                    data = schoolauth.app_auth_token
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = "0",
                    msg = "获取token失败"
                });
            }
        }
    }
}