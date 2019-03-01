using System;
using System.IO;
using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using DbModel;
using Microsoft.Extensions.Configuration;
using Models.ViewModels;
using Polly;
using Newtonsoft.Json;
using System.Collections.Generic;
using Alipay.AopSdk.Core.Domain;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// 小程序工具类,获取用户身份数据等
    /// </summary>
    public class AppHelper
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public string AlipayPublicKey { set; get; }
        /// <summary>
        /// App ID 小程序唯一标识
        /// </summary>
        public string AppId { set; get; }
        /// <summary>
        /// 字符类型
        /// </summary>
        public string CharSet { set; get; }
        /// <summary>
        /// 支付宝网关,有两种,一种线上,一种沙箱
        /// </summary>
        public string GatewayUrl { set; get; }
        /// <summary>
        /// 私匙,小程序开发方自己保留,不能外泄
        /// </summary>
        public string PrivateKey { set; get; }
        /// <summary>
        /// 签名类型,即令牌的加密方式
        /// </summary>
        public string SignType { set; get; }
        /// <summary>
        /// Uid 不是必填字段
        /// </summary>
        public string Uid { set; get; }


        /// <summary>
        /// ItemID云端设备接入物料号
        /// </summary>
        public string ItemID { set; get; }

        /// <summary>
        /// 云端设备接入设备供应商id
        /// </summary>
        public string SupplierID { set; get; }

        /// <summary>
        /// 支付宝合作伙伴身份ID
        /// </summary>
        public string Pid { set; get; }
        /// <summary>
        /// 水费程序独有appid
        /// </summary>
        public string CAppId { get; set; }
        /// <summary>
        /// 支付宝异步通知地址
        /// </summary>
        public string Zfburl { get; set; }


        /// <summary>
        /// 构造函数读取小程序配置文件
        /// </summary>
        /// <param name="key">配置节名称</param>
        public AppHelper(string key)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = builder.Build();
            var alipaySection = config.GetSection(key);
            AlipayPublicKey = alipaySection.GetSection("AlipayPublicKey").Value;
            AppId = alipaySection.GetSection("AppId").Value;
            CharSet = alipaySection.GetSection("CharSet").Value;
            GatewayUrl = alipaySection.GetSection("GatewayUrl").Value;
            PrivateKey = alipaySection.GetSection("PrivateKey").Value;
            SignType = alipaySection.GetSection("SignType").Value;
            Uid = alipaySection.GetSection("Uid").Value;
            if (key == "Alipay")
            {
                ItemID = alipaySection.GetSection("ItemID").Value;
                SupplierID = alipaySection.GetSection("SupplierID").Value;
                Pid = alipaySection.GetSection("pid").Value;
            }
            else if (key == "RuohuoWater")
            {
                CAppId = alipaySection.GetSection("CAppId").Value;
                Zfburl = alipaySection.GetSection("Zfburl").Value;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="authCode">授权码</param>
        public AlipayUserInfoShareResponse GetUserInfo(string authCode)
        {
            //如果失败,重试5次,每次之间停顿1秒
            return Policy.Handle<AopException>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
            {
                var alipayClient = new DefaultAopClient(GatewayUrl, AppId, PrivateKey, "json", null, SignType, AlipayPublicKey, CharSet, false);
                var alipayRequest = new AlipaySystemOauthTokenRequest
                {
                    Code = authCode,
                    GrantType = "authorization_code"
                };
                var alipayResponse = alipayClient.Execute(alipayRequest);
                if (!alipayResponse.IsError)
                {
                    var requestUser = new AlipayUserInfoShareRequest();
                    var userinfoShareResponse = alipayClient.Execute(requestUser, alipayResponse.AccessToken);
                    if (!userinfoShareResponse.IsError)
                    {
                        return userinfoShareResponse;
                    }
                }
                return null;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authCode"></param>
        /// <returns></returns>
        public AlipaySystemOauthTokenResponse GetAliUserTokenAndAliuserid(string authCode)
        {
            //如果失败,重试5次,每次之间停顿1秒
            return Policy.Handle<AopException>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
            {
                var alipayClient = new DefaultAopClient(GatewayUrl, AppId, PrivateKey, "json", null, SignType, AlipayPublicKey, CharSet, false);
                var alipayRequest = new AlipaySystemOauthTokenRequest
                {
                    Code = authCode,
                    GrantType = "authorization_code"
                };
                var alipayResponse = alipayClient.Execute(alipayRequest);
                return alipayResponse;
            });
        }

        #region 第三方授权

        /// <summary>
        /// 获取用户信息第三方获取
        /// </summary>
        /// <param name="authCode">授权码</param>
        /// <param name="app_auth_Token">授权码</param>
        public AlipayUserInfoShareResponse GetUserInfo(string authCode, string app_auth_Token)
        {
            //如果失败,重试5次,每次之间停顿1秒
            return Policy.Handle<AopException>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
            {
                var alipayClient = new DefaultAopClient(GatewayUrl, AppId, PrivateKey, "json", null, SignType, AlipayPublicKey, CharSet, false);
                var alipayRequest = new AlipaySystemOauthTokenRequest
                {
                    Code = authCode,
                    GrantType = "authorization_code"
                };
                var alipayResponse = alipayClient.Execute(alipayRequest, null, app_auth_Token);
                if (!alipayResponse.IsError)
                {
                    var requestUser = new AlipayUserInfoShareRequest();
                    var userinfoShareResponse = alipayClient.Execute(requestUser, null, app_auth_Token);
                    if (!userinfoShareResponse.IsError)
                    {
                        return userinfoShareResponse;
                    }
                }
                return null;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app_auth_token"></param>
        /// <returns></returns>
        public AlipayUserInfoShareResponse alipay_user_info_share(string app_auth_token)
        {
            //如果失败,重试5次,每次之间停顿1秒
            return Policy.Handle<AopException>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
            {
                var alipayClient = new DefaultAopClient(GatewayUrl, AppId, PrivateKey, "json", null, SignType, AlipayPublicKey, CharSet, false);
                AlipayUserInfoShareRequest request = new AlipayUserInfoShareRequest();
                AlipayUserInfoShareResponse alipayResponse = alipayClient.Execute(request, null, app_auth_token);
                if (!alipayResponse.IsError)
                {
                    return alipayResponse;
                }
                return null;
            });
        }
        /// <summary>
        /// 查询某个应用授权AppAuthToken的授权信息
        /// </summary>
        /// <param name="app_auth_Token">授权码</param>
        public AlipayOpenAuthTokenAppQueryResponse alipay_open_auth_token_app_query(string app_auth_Token)
        {
            //如果失败,重试5次,每次之间停顿1秒
            return Policy.Handle<AopException>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
            {
                var alipayClient = new DefaultAopClient(GatewayUrl, AppId, PrivateKey, "json", null, SignType, AlipayPublicKey, CharSet, false);
                AlipayOpenAuthTokenAppQueryRequest request = new AlipayOpenAuthTokenAppQueryRequest();
                request.BizContent = "{" +
                "\"app_auth_token\":\"" + app_auth_Token + "\"" +
                "  }";
                AlipayOpenAuthTokenAppQueryResponse response = alipayClient.Execute(request);
                if (!response.IsError)
                {
                    return response;
                }
                return null;
            });
        }
        /// <summary>
        /// 换取应用授权令牌
        /// </summary>
        /// <param name="app_auth_code">授权码</param>
        public AlipayOpenAuthTokenAppResponse alipay_open_auth_token_app(string app_auth_code)
        {
            //如果失败,重试5次,每次之间停顿1秒
            return Policy.Handle<AopException>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
            {
                var alipayClient = new DefaultAopClient(GatewayUrl, AppId, PrivateKey, "json", null, SignType, AlipayPublicKey, CharSet, false);
                AlipayOpenAuthTokenAppRequest request = new AlipayOpenAuthTokenAppRequest();
                request.BizContent = "{" +
                "\"grant_type\":\"authorization_code\"," +
                "\"code\":\"" + app_auth_code + "\"" +
                "  }";
                AlipayOpenAuthTokenAppResponse response = alipayClient.Execute(request);
                if (!response.IsError)
                {
                    return response;
                }
                return response;
            });
        }
        #endregion

        #region 开卡组件和开卡

        /// <summary>
        /// 获取用户UserID和token
        /// </summary>
        /// <param name="authCode">授权码</param>
        /// <param name="app_auth_token"></param>
        public AlipaySystemOauthTokenResponse GetUserIdAndToken(string authCode, string app_auth_token)
        {
            //如果失败,重试5次,每次之间停顿1秒
            return Policy.Handle<AopException>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
            {
                var alipayClient = new DefaultAopClient(GatewayUrl, AppId, PrivateKey, "json", null, SignType, AlipayPublicKey, CharSet, false);
                var alipayRequest = new AlipaySystemOauthTokenRequest
                {
                    Code = authCode,
                    GrantType = "authorization_code"
                };
                var alipayResponse = alipayClient.Execute(alipayRequest, null, app_auth_token);
                if (alipayResponse.IsError)
                {
                    return null;
                }
                return alipayResponse;
            });
        }

        /// <summary>
        /// 获取会员卡领卡投放链接
        /// </summary>
        /// <param name="template_id"></param>
        /// <param name="app_auth_token"></param>
        /// <returns></returns>
        public AlipayMarketingCardActivateurlApplyResponse alipay_marketing_card_activateurl_apply(string template_id, string app_auth_token)
        {
            //如果失败,重试5次,每次之间停顿1秒
            return Policy.Handle<AopException>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
            {
                var alipayClient = new DefaultAopClient(GatewayUrl, AppId, PrivateKey, "json", null, SignType, AlipayPublicKey, CharSet, false);
                var alipayRequest = new AlipayMarketingCardActivateurlApplyRequest();
                alipay_marketing_card_activateurl_apply apply = new alipay_marketing_card_activateurl_apply();
                apply.template_id = template_id;
                //apply.callback = "http://ali.v0719.com/m/xchl/card_good.html";
                //apply.callback = "http://api.ali.v0719.com:666/api/SchoolCodr/AppCallBack";
                apply.callback = "http://api.newxiaoyuan.com/api/SchoolCodr/AppCallBack";
                JsonSerializerSettings settings = new JsonSerializerSettings();
                string strjson = JsonConvert.SerializeObject(apply, settings);
                alipayRequest.BizContent = strjson;
                var alipayResponse = alipayClient.Execute(alipayRequest, null, app_auth_token);
                //if (alipayResponse.IsError)
                //{
                //    return null;
                //}
                return alipayResponse;
            });
        }

        /// <summary>
        /// 会员卡开卡表单模板配置
        /// </summary>
        /// <returns></returns>
        public AlipayMarketingCardFormtemplateSetResponse alipay_marketing_card_formtemplate_set(string template_id, string app_auth_token)
        {
            //如果失败,重试5次,每次之间停顿1秒
            return Policy.Handle<AopException>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
            {
                var alipayClient = new DefaultAopClient(GatewayUrl, AppId, PrivateKey, "json", null, SignType, AlipayPublicKey, CharSet, false);
                var alipayRequest = new AlipayMarketingCardFormtemplateSetRequest();
                alipay_marketing_card_formtemplate_set apply = new alipay_marketing_card_formtemplate_set();
                apply.template_id = template_id;
                apply.fields.optional = "{\"common_fields\":[\"OPEN_FORM_FIELD_GENDER\"]}";
                apply.fields.required = "{\"common_fields\":[\"OPEN_FORM_FIELD_MOBILE\"]}";
                JsonSerializerSettings settings = new JsonSerializerSettings();
                string strjson = JsonConvert.SerializeObject(apply, settings);
                alipayRequest.BizContent = strjson;
                var alipayResponse = alipayClient.Execute(alipayRequest, null, app_auth_token);
                //if (alipayResponse.IsError)
                //{
                //    return null;
                //}
                return alipayResponse;
            });
        }
        /// <summary>
        /// 查询用户提交的会员卡表单信息
        /// </summary>
        /// <returns></returns>
        public AlipayMarketingCardActivateformQueryResponse alipay_marketing_card_activateform_query(string template_id, string app_auth_token)
        {
            //如果失败,重试5次,每次之间停顿1秒
            return Policy.Handle<AopException>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
            {
                var alipayClient = new DefaultAopClient(GatewayUrl, AppId, PrivateKey, "json", null, SignType, AlipayPublicKey, CharSet, false);
                var alipayRequest = new AlipayMarketingCardActivateformQueryRequest();
                alipay_marketing_card_activateform_query apply = new alipay_marketing_card_activateform_query();
                apply.template_id = template_id;
                apply.biz_type = "MEMBER_CARD";
                apply.request_id = DateTime.Now.Ticks.ToString();
                JsonSerializerSettings settings = new JsonSerializerSettings();
                string strjson = JsonConvert.SerializeObject(apply, settings);
                alipayRequest.BizContent = strjson;
                var alipayResponse = alipayClient.Execute(alipayRequest, null, app_auth_token);
                if (alipayResponse.IsError)
                {
                    return null;
                }
                return alipayResponse;
            });
        }

        /// <summary>
        /// 查询用户提交的会员卡表单信息
        /// </summary>
        /// <returns></returns>
        public AlipayMarketingCardQueryResponse alipay_marketing_card_query(AlipayMarketingCardQueryModel model, string app_auth_token)
        {
            //如果失败,重试5次,每次之间停顿1秒
            return Policy.Handle<AopException>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
            {
                var alipayClient = new DefaultAopClient(GatewayUrl, AppId, PrivateKey, "json", null, SignType, AlipayPublicKey, CharSet, false);
                var alipayRequest = new AlipayMarketingCardActivateformQueryRequest();

                AlipayMarketingCardQueryRequest request = new AlipayMarketingCardQueryRequest();
                request.SetBizModel(model);
                AlipayMarketingCardQueryResponse response = alipayClient.Execute(request, null, app_auth_token);
                if (response.IsError)
                {
                    return null;
                }
                return response;
            });
        }
        /// <summary>
        /// 会员开卡
        /// </summary>
        /// <returns></returns>
        public AlipayMarketingCardOpenResponse alipay_marketing_card_open(alipay_marketing_card_open_Model.gotojsonbean gt, tb_school_info schoolinfo, string app_auth_token, string PID, string access_token)
        {
            alipay_marketing_card_open_Model.jsonbean list = new alipay_marketing_card_open_Model.jsonbean();
            list.out_serial_no = DateTime.Now.Ticks.ToString();// "201801080000002";                     //外部商户流水号（商户需要确保唯一性控制，类似request_id唯一请求标识） 
            list.card_template_id = gt.card_template_id;//"20180117000000000746252000300239";                  //支付宝分配的卡模板Id（卡模板创建接口返回的模板ID） 
            // card_user_info 
            alipay_marketing_card_open_Model.TCard_user_info card_user_info = new alipay_marketing_card_open_Model.TCard_user_info();
            card_user_info.user_uni_id = gt.user_uni_id;// "2088002443001736";           //用户唯一标识, 根据user_id_type类型来定 （目前暂支持支付宝userId） 支付宝userId说明：支付宝用户号是以2088开头的16位纯数字组成 
            card_user_info.user_uni_id_type = "UID";                    //ID类型：UID， 即传值UID即可 
            list.card_user_info = card_user_info;
            list.open_card_channel = "college_card_isv";
            list.open_card_channel_id = PID;
            //card_ext_info
            alipay_marketing_card_open_Model.TCard_ext_info card_ext_info = new alipay_marketing_card_open_Model.TCard_ext_info();
            card_ext_info.external_card_no = gt.external_card_no;// "EXT0001";                 //商户外部会员卡卡号 说明： 1、会员卡开卡接口，如果卡类型为外部会员卡，请求中则必须提供该参数； 2、更新、查询、删除等接口，请求中则不需要提供该参数值； 
            card_ext_info.open_date = gt.open_date;//"2018-01-16 15:07:46";            //会员卡开卡时间，格式为yyyy-MM-dd HH:mm:ss 
            card_ext_info.valid_date = gt.valid_date;//"2030-02-20 21:20:46";           //会员卡有效期 
            alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list front_text_list = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list front_text_list1 = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list front_text_list2 = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list front_text_list3 = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            card_ext_info.front_text_list = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list[1];

            front_text_list = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            front_text_list.label = "姓名";
            front_text_list.value = gt.name;

            //front_text_list1 = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            //front_text_list1.label = "性别";
            //front_text_list1.value = gt.gende.Substring(0,1) =="M"?"男":"女";//"2018000100001";// 

            front_text_list2 = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            front_text_list2.label = "学号";
            front_text_list2.value = gt.external_card_no;//"2018000100001";// 

            front_text_list3 = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            front_text_list3.label = "班级";
            front_text_list3.value = gt.dept.Substring(gt.dept.LastIndexOf("/") + 1);//"钢琴";

            List<alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list> ft = new List<alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list>();
            ft.Add(front_text_list);
            ft.Add(front_text_list1);
            ft.Add(front_text_list2);
            ft.Add(front_text_list3);
            card_ext_info.front_text_list = ft.ToArray();

            list.card_ext_info = card_ext_info;
            // member_ext_info 
            alipay_marketing_card_open_Model.TMember_ext_info member_ext_info = new alipay_marketing_card_open_Model.TMember_ext_info();
            member_ext_info.name = gt.name;//"LT";                                  //String可选64姓名 李洋 
            member_ext_info.gende = gt.gende;//"MALE";                             //String可选32性别（男：MALE；女：FEMALE） MALE 
            member_ext_info.birth = gt.birth;//"2018-01-10";                         //String可选32生日 yyyy-MM-dd 2016-06-27 
            member_ext_info.cell = gt.cell;//"13000000000";                       //String可选32手机号 13000000000  
            list.member_ext_info = member_ext_info;
            var alipayClient = new DefaultAopClient(GatewayUrl, AppId, PrivateKey, "json", null, SignType, AlipayPublicKey, CharSet, false);
            AlipayMarketingCardOpenRequest request = new AlipayMarketingCardOpenRequest();
            request.GetParameters();
            request.BizContent = JsonConvert.SerializeObject(list);
            var alipayResponse = alipayClient.Execute(request, access_token, app_auth_token);
            if (alipayResponse.IsError)
            {
                return null;
            }
            return alipayResponse;
        }
        #endregion
    }
}
