using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 第三方提供接口配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class ThirdpartyAPIConfigController : Controller
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private Itb_thirdpartyAPI_ConfigService _tb_thirdpartyAPI_ConfigService;
        private Itb_appaccounts_itemService _tb_appaccounts_itemService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_thirdpartyAPI_ConfigService"></param>
        /// <param name="tb_appaccounts_itemService"></param>
        public ThirdpartyAPIConfigController(Itb_thirdpartyAPI_ConfigService tb_thirdpartyAPI_ConfigService,
            Itb_appaccounts_itemService tb_appaccounts_itemService)
        {
            _tb_thirdpartyAPI_ConfigService = tb_thirdpartyAPI_ConfigService;
            _tb_appaccounts_itemService = tb_appaccounts_itemService;
        }
        /// <summary>
        /// 通过schoolcode和api接口名获取第三方接口url和接口类型
        /// </summary>
        /// <param name="schoolCode"></param>
        /// <param name="apiname"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetApiToAPINameAndCode(string schoolCode,string apiname)
        {
            try
            {
                //查询套餐
                var records = _tb_thirdpartyAPI_ConfigService.FindByClause(x => x.schoolcode == schoolCode && x.APIName == apiname);
                if (records == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail
                    });
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    apiURL = records.APIURL,
                    apiType = records.APIType
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
        /// 小程序支付配置接口（配置谁收款）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddAppAccountItem([FromBody] JObject obj)
        {
            try
            {
                string appId = obj["appId"].ToString();
                string typename = obj["typename"].ToString();
                string schoolcode = obj["schoolcode"].ToString();
                int accounts_id = Convert.ToInt32(obj["accounts_id"].ToString());
                var tb = _tb_appaccounts_itemService.FindByClause(x => x.appId == appId && x.typename == typename && x.schoolcode == schoolcode);
                tb_appaccounts_item item = new tb_appaccounts_item();
                item.appId = appId;
                item.typename = typename;
                item.schoolcode = schoolcode;
                item.accounts_id = accounts_id;
                if (tb==null)
                {
                    _tb_appaccounts_itemService.Insert(item);
                }
                else
                {
                    item.id = tb.id;
                    _tb_appaccounts_itemService.Update(item);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess
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
    }
}