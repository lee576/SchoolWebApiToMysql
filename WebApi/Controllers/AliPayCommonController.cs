using System;
using System.Web;
using Alipay.AopSdk.Core.Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SchoolWebApi.Utility;
using IService;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 支付宝支付接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class AliPayCommonController : Controller
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private Itb_payment_accountsService _tb_payment_accountsService;
        private IConfiguration RuohuoAppid = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_payment_accountsService"></param>
        public AliPayCommonController(Itb_payment_accountsService tb_payment_accountsService)
        {
            _tb_payment_accountsService = tb_payment_accountsService;
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = builder.Build();
            RuohuoAppid = config.GetSection("RuohuoAppid");
        }
        /// <summary>
        /// 支付宝支付接口ToAPP
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AliPayToAppPay([FromBody] JObject obj)
        {
            try
            {
                AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
                if (obj["Body"] != null)
                {
                    model.Body = obj["Body"].ToString();
                }
                if (obj["Subject"] != null)
                {
                    model.Subject = obj["Subject"].ToString();
                }
                if (obj["TotalAmount"] == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = "TotalAmount不能为空"
                    });
                }
                if (obj["TimeoutExpress"] == null)
                {
                    model.TimeoutExpress = "90m";
                }
                else
                {
                    model.TimeoutExpress = obj["TimeoutExpress"].ToString();
                }
                if (obj["SetNotifyUrl"] == null || obj["appName"] == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = "SetNotifyUrl,appName不能为空"
                    });
                }
                ExtendParams ep = new ExtendParams();
                ep.SysServiceProviderId = RuohuoAppid.GetSection("sys_service_provider_id").Value;
                model.ExtendParams = ep;
                model.TotalAmount = obj["TotalAmount"].ToString();
                model.ProductCode = "QUICK_MSECURITY_PAY";
                model.OutTradeNo = DateTime.Now.ToString("yyyyMMddHHmmssffff") + new Random().Next(1000).ToString().PadLeft(4, '0');
                var app = new AppHelper(obj["appName"].ToString());
                string SetNotifyUrl = obj["SetNotifyUrl"].ToString();
                string response_Body = AliPayHelper.AliPayToAppPay(model, app.AppId, app.PrivateKey, app.AlipayPublicKey, SetNotifyUrl);
                //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
                //Response.WriteAsync(HttpUtility.HtmlEncode(response_Body));
                //页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
                return Json(HttpUtility.HtmlEncode(response_Body));
            }
            catch (Exception e)
            {
                log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
           
            //AliPayHelper
        }
        /// <summary>
        /// 支付宝支付接口To Web
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult H5PayToWebPay([FromBody] JObject obj)
        {
            try
            {
                AlipayTradeWapPayModel model = new AlipayTradeWapPayModel();
                string SetReturnUrl = null;
                string SetNotifyUrl = null;
                if (obj["SetReturnUrl"] != null)
                {
                    SetReturnUrl = obj["SetReturnUrl"].ToString();
                }
                if (obj["SetNotifyUrl"] != null)
                {
                    SetReturnUrl = obj["SetNotifyUrl"].ToString();
                }
                if (obj["Body"] != null)
                {
                    model.Body = obj["Body"].ToString();
                }
                if (obj["Subject"] != null)
                {
                    model.Subject = obj["Subject"].ToString();
                }
                if (obj["TotalAmount"] == null || obj["appName"] == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = "TotalAmount,appName不能为空"
                    });
                }
                if (obj["QuitUrl"] != null)
                {
                    model.QuitUrl = obj["QuitUrl"].ToString();
                }
                model.OutTradeNo = DateTime.Now.ToString("yyyyMMddHHmmssffff") + new Random().Next(1000).ToString().PadLeft(4, '0');
                model.ProductCode = "QUICK_WAP_WAY";
                ExtendParams ep = new ExtendParams();
                ep.SysServiceProviderId = RuohuoAppid.GetSection("sys_service_provider_id").Value;
                model.ExtendParams = ep;
                var app = new AppHelper(obj["appName"].ToString());
                string response_Body = AliPayHelper.H5PayToWebPay(model, app.AppId, app.PrivateKey, app.AlipayPublicKey, SetReturnUrl, SetNotifyUrl);
                //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
                //Response.WriteAsync(response_Body);
                //页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    data = response_Body
                });
            }
            catch (Exception e)
            {
                log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
           
            //AliPayHelper
        }
        /// <summary>
        /// 支付宝支付接口To 小程序
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AliCreateOrders([FromBody] JObject obj)
        {
            try
            {
                AlipayTradeCreateModel model = new AlipayTradeCreateModel();
                if (obj["Body"] != null)
                {
                    model.Body = obj["Body"].ToString();
                }
                if (obj["Subject"] != null)
                {
                    model.Subject = obj["Subject"].ToString();
                }
                if (obj["TotalAmount"] == null || obj["AliUserId"] == null || obj["appName"] == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = "TotalAmount,AliUserId,AppName不能为空"
                    });
                }
                if (obj["TimeoutExpress"] == null)
                {
                    model.TimeoutExpress = "90m";
                }
                else
                {
                    model.TimeoutExpress = obj["TimeoutExpress"].ToString();
                }
                model.TotalAmount = obj["TotalAmount"].ToString();
                model.OutTradeNo = DateTime.Now.ToString("yyyyMMddHHmmssffff") + new Random().Next(1000).ToString().PadLeft(4, '0');
                model.BuyerId = obj["AliUserId"].ToString();

                ExtendParams ep = new ExtendParams();
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                var config = builder.Build();
                var RuohuoAppid = config.GetSection("RuohuoAppid");
                ep.SysServiceProviderId = RuohuoAppid.GetSection("sys_service_provider_id").Value;
                model.ExtendParams = ep;
                var app = new AppHelper(obj["appName"].ToString());

                string response_Body = AliPayHelper.AliCreateOrders(app.AppId, app.PrivateKey, app.AlipayPublicKey, model);
                //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
                //Response.WriteAsync(response_Body);
                //页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    data = response_Body
                });
            }
            catch (Exception e)
            {
                log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
            
        }
        /// <summary>
        /// 支付宝支付接口To 小程序 不需要小程序appid
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AliCreateOrders2([FromBody] JObject obj)
        {
            try
            {
                AlipayTradeCreateModel model = new AlipayTradeCreateModel();
                if (obj["Body"] != null)
                {
                    model.Body = obj["Body"].ToString();
                }
                if (obj["Subject"] != null)
                {
                    model.Subject = obj["Subject"].ToString();
                }
                if (obj["TotalAmount"] == null || obj["AliUserId"] == null || obj["appName"] == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = "TotalAmount,AliUserId,AppName不能为空"
                    });
                }
                if (obj["TimeoutExpress"] == null)
                {
                    model.TimeoutExpress = "90m";
                }
                else
                {
                    model.TimeoutExpress = obj["TimeoutExpress"].ToString();
                }
                model.TotalAmount = obj["TotalAmount"].ToString();
                model.OutTradeNo = DateTime.Now.ToString("yyyyMMddHHmmssffff") + new Random().Next(1000).ToString().PadLeft(4, '0');
                model.BuyerId = obj["AliUserId"].ToString();

                ExtendParams ep = new ExtendParams();
                ep.SysServiceProviderId = RuohuoAppid.GetSection("sys_service_provider_id").Value;
                model.ExtendParams = ep;
                var app = _tb_payment_accountsService.FindByClause(x => x.appid == obj["appid"].ToString());

                string response_Body = AliPayHelper.AliCreateOrders(app.appid, app.private_key, app.alipay_public_key, model);
                //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
                //Response.WriteAsync(response_Body);
                //页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    data = response_Body
                });
            }
            catch (Exception e)
            {
                log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }

        }
    }
}
