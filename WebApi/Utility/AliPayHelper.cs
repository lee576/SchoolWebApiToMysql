﻿using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Domain;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using Microsoft.Extensions.Configuration;
using Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SchoolWebApi.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class AliPayHelper
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 在线创建订单
        /// </summary>
        /// <param name="AppId">小程序ID</param>
        /// <param name="PrivateKey">私钥（商家的PrivateKey）</param>
        /// <param name="AlipayPublicKey">公钥(商家的PublicKey)</param>
        /// <param name="AliUserId">支付宝用户ID</param>
        /// <param name="OrderName">订单名称</param>
        /// <param name="Price">金额</param>
        /// <param name="OrderNo">订单编号</param>
        /// <param name="notifyurl">回调函数地址</param>
        /// <returns></returns>
        public static string AliCreateOrders(string AppId, string PrivateKey, string AlipayPublicKey, string AliUserId, string OrderName, string Price, string OrderNo, string notifyurl = "")
        {
            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                var config = builder.Build();
                var RuohuoAppid = config.GetSection("RuohuoAppid");

                IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppId, PrivateKey, "json", "1.0", "RSA2", AlipayPublicKey, "utf-8", false);
                AlipayTradeCreateRequest request = new AlipayTradeCreateRequest();
                if (notifyurl != "")
                {
                    request.SetNotifyUrl(notifyurl);

                }
                //request.BizContent = "{" +
                //"\"out_trade_no\":\"" + OrderNo + "\"," +//商户订单号,64个字符以内、可包含字母、数字、下划线；需保证在商户端不重复
                ////"\"seller_id\":\"2088102146225135\"," +//如果该值为空，则默认为商户签约账号对应的支付宝用户ID
                //"\"total_amount\":" + Price + "," +
                ////"\"total_amount\":1.00," +
                ////"\"discountable_amount\":8.88," +
                //"\"subject\":\"" + OrderName + "\"," +
                ////"\"body\":\"订单描述（可选）\"," +
                //"\"extend_params\":{" +
                //    "\"sys_service_provider_id\":\"" + this.sys_service_provider_id + "\"" +//公司分成时的标记
                //                                                                            //"\"sys_service_provider_id\":\"2088131404015935\"," +
                //                                                                            //"\"industry_reflux_info\":\"{\\\\\\\"scene_code\\\\\\\":\\\\\\\"metro_tradeorder\\\\\\\",\\\\\\\"channel\\\\\\\":\\\\\\\"xxxx\\\\\\\",\\\\\\\"scene_data\\\\\\\":{\\\\\\\"asset_name\\\\\\\":\\\\\\\"ALIPAY\\\\\\\"}}\"," +
                //                                                                            //"\"card_type\":\"S0JP0000\"" +
                //"}," +
                //"\"buyer_id\":\"" + AliUserId + "\"," +
                //"\"timeout_express\":\"" + PayTimeOut + "\"," +//该笔订单允许的最晚付款时间逾期将关闭交易。取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。 
                //"\"logistics_detail\":{" +//	物流信息
                //"\"logistics_type\":\"DIRECT\"" +//POST 平邮, EXPRESS 其他快递, VIRTUAL 虚拟物品,EMS EMS, DIRECT 无需物流。
                //"    }" +
                //"  }";
                AlipayTradeCreate tc = new AlipayTradeCreate();
                tc.out_trade_no = OrderNo;
                tc.total_amount = Price;
                tc.subject = OrderName;
                tc.extend_params.sys_service_provider_id = RuohuoAppid.GetSection("sys_service_provider_id").Value;
                tc.buyer_id = AliUserId;
                tc.timeout_express = PayTimeOut;
                tc.logistics_detail.logistics_type = "DIRECT";
                JsonSerializerSettings settings = new JsonSerializerSettings();
                string strjson = JsonConvert.SerializeObject(tc, settings);
                request.BizContent = strjson;
                AlipayTradeCreateResponse response = client.Execute(request);
                return response.Body;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.ToString();
            }

        }
        /// <summary>
        /// 该笔订单允许的最晚付款时间逾期将关闭交易。取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。 
        /// </summary>
        public static string PayTimeOut => "90m";

        /// <summary>
        /// 检查订单状态
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="appid"></param>
        /// <param name="privateKey"></param>
        /// <param name="alipayPublicKey"></param>
        /// <returns></returns>
        public static string GetOrdersStatus(string out_trade_no, string appid, string privateKey, string alipayPublicKey)
        {
            try
            {
                //通过应缴费用缴费ID获取订单号、appid、私钥、公钥、
                string Url = "https://openapi.alipay.com/gateway.do";
                IAopClient client = new DefaultAopClient(Url, appid, privateKey, "json", "1.0", "RSA2", alipayPublicKey);
                AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
                request.BizContent = "{" +
                "\"out_trade_no\":\"" + out_trade_no + "\"" +
                "  }";
                AlipayTradeQueryResponse response = client.Execute(request);
                return response.Body;
                //bool isOK = true;
                //switch (((JObject)JsonConvert.DeserializeObject(response.Body))["alipay_trade_query_response"]["trade_status"].ToString().ToUpper())
                //{
                //    case "WAIT_BUYER_PAY"://交易创建，等待买家付款
                //        //return new JsonResult(new { Return = "交易创建，等待买家付款" });
                //    case "TRADE_CLOSED"://未付款交易超时关闭，或支付完成后全额退款
                //        //return new JsonResult(new { Return = "未付款交易超时关闭，或支付完成后全额退款" });
                //    case "TRADE_FINISHED"://交易结束，不可退款
                //        //return new JsonResult(new { Return = "交易结束，不可退款" });
                //        isOK = false;
                //        break;
                //    default://TRADE_SUCCESS（交易支付成功）
                //            //修改数据库支付状态
                //            //if (_tb_payment_ARService.PayOk(out_trade_no) == true)
                //            //    return new JsonResult(new { Return = "True" });
                //        break;
                //}
                //return isOK;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return ex.ToString();
            }

        }
        /// <summary>
        /// 判断是否支付成功
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="appId"></param>
        /// <param name="privateKey"></param>
        /// <param name="alipayPublicKey"></param>
        /// <returns></returns>
        public static bool GetOrdersStatus2(string out_trade_no, string appId, string privateKey, string alipayPublicKey)
        {
            //通过应缴费用缴费ID获取订单号、appid、私钥、公钥、
            string Url = "https://openapi.alipay.com/gateway.do";
            IAopClient client = new DefaultAopClient(Url, appId, privateKey, "json", "1.0", "RSA2", alipayPublicKey);
            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
            request.BizContent = "{" +
            "\"out_trade_no\":\"" + out_trade_no + "\"" +
            "  }";
            AlipayTradeQueryResponse response = client.Execute(request);
            if (((JObject)JsonConvert.DeserializeObject(response.Body))["alipay_trade_query_response"]["trade_status"].ToString().ToUpper() == "TRADE_SUCCESS")
                return true;
            return false;
        }
        /// <summary>
        /// app支付接口
        /// </summary>
        /// <param name="app_pay"></param>
        /// <param name="appid"></param>
        /// <param name="private_key"></param>
        /// <param name="alipay_public_key"></param>
        /// <param name="SetNotifyUrl"></param>
        /// <returns></returns>
        public static string AliPayToAppPay(AlipayTradeAppPayModel app_pay, string appid, string private_key, string alipay_public_key, string SetNotifyUrl)
        {

            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", appid, private_key, "json", "1.0", "RSA2", alipay_public_key, "utf-8", false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            request.SetBizModel(app_pay);
            request.SetNotifyUrl(SetNotifyUrl);
            //这里和普通的接口调用不同，使用的是sdkExecute
            AlipayTradeAppPayResponse response = client.SdkExecute(request);

            return response.Body;

        }
        /// <summary>
        /// H5支付接口
        /// </summary>
        /// <param name="web_pay"></param>
        /// <param name="appid"></param>
        /// <param name="private_key"></param>
        /// <param name="alipay_public_key"></param>
        /// <param name="SetReturnUrl"></param>
        /// <param name="SetNotifyUrl"></param>
        /// <returns></returns>
        public static string H5PayToWebPay(AlipayTradeWapPayModel web_pay, string appid, string private_key, string alipay_public_key, string SetReturnUrl = null, string SetNotifyUrl = null)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", appid, private_key, "json", "1.0", "RSA2", alipay_public_key, "utf-8", false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
            request.SetBizModel(web_pay);
            // 设置支付完成同步回调地址
            request.SetReturnUrl(SetReturnUrl);
            // 设置支付完成异步通知接收地址
            request.SetNotifyUrl(SetNotifyUrl);
            //这里和普通的接口调用不同，使用的是sdkExecute
            AlipayTradeWapPayResponse response = client.PageExecute(request, null, "post");
            return response.Body;

        }
        /// <summary>
        /// 在线创建订单（小程序）
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="PrivateKey"></param>
        /// <param name="AlipayPublicKey"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string AliCreateOrders(string AppId, string PrivateKey, string AlipayPublicKey, AlipayTradeCreateModel model)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppId, PrivateKey, "json", "1.0", "RSA2", AlipayPublicKey, "utf-8", false);
            AlipayTradeCreateRequest request = new AlipayTradeCreateRequest();
            request.SetBizModel(model);
            AlipayTradeCreateResponse response = client.Execute(request);
            return response.Body;
        }




    }

}