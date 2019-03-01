using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Domain;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// 红包帮助类
    /// </summary>
    public class AliRedPacketHelper
    {
        private IConfiguration RuohuoAppid = null;
        private string app_id = "";
        private string private_key = "";
        private string alipay_public_key = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userinfoConfig"></param>
        public AliRedPacketHelper(string userinfoConfig)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = builder.Build();
            RuohuoAppid = config.GetSection(userinfoConfig);
            app_id = RuohuoAppid.GetSection("app_id").Value;
            private_key = RuohuoAppid.GetSection("private_key").Value;
            alipay_public_key = RuohuoAppid.GetSection("alipay_public_key").Value;
        }
        /// <summary>
        /// 创建红包
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public AlipayMarketingCampaignCashCreateResponse CreateRedPack(AlipayMarketingCampaignCashCreateModel m)
        {

            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app_id, private_key, "json", "1.0", "RSA2", alipay_public_key, "utf-8", false);
            AlipayMarketingCampaignCashCreateRequest request = new AlipayMarketingCampaignCashCreateRequest();
            request.SetBizModel(m);
            AlipayMarketingCampaignCashCreateResponse response = client.Execute(request);
            return response;
        }
        /// <summary>
        /// 触发现金红包活动
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public AlipayMarketingCampaignCashTriggerResponse TriggerRedPack(AlipayMarketingCampaignCashTriggerModel m)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app_id, private_key, "json", "1.0", "RSA2", alipay_public_key, "utf-8", false);
            AlipayMarketingCampaignCashTriggerRequest request = new AlipayMarketingCampaignCashTriggerRequest();
            request.SetBizModel(m);
            AlipayMarketingCampaignCashTriggerResponse response = client.Execute(request);
            return response;
        }
        /// <summary>
        /// 更改现金活动状态
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public AlipayMarketingCampaignCashStatusModifyResponse StatusModifyRedPack(AlipayMarketingCampaignCashStatusModifyModel m)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app_id, private_key, "json", "1.0", "RSA2", alipay_public_key, "utf-8", false);
            AlipayMarketingCampaignCashStatusModifyRequest request = new AlipayMarketingCampaignCashStatusModifyRequest();
            request.SetBizModel(m);
            AlipayMarketingCampaignCashStatusModifyResponse response = client.Execute(request);
            return response;
        }
        /// <summary>
        /// 现金活动详情查询
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public AlipayMarketingCampaignCashDetailQueryResponse DetailQueryRedPack(AlipayMarketingCampaignCashDetailQueryModel m)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app_id, private_key, "json", "1.0", "RSA2", alipay_public_key, "utf-8", false);
            AlipayMarketingCampaignCashDetailQueryRequest request = new AlipayMarketingCampaignCashDetailQueryRequest();
            request.SetBizModel(m);
            AlipayMarketingCampaignCashDetailQueryResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public async Task<AlipayMarketingCampaignCashDetailQueryResponse> DetailQueryRedPackAsync(AlipayMarketingCampaignCashDetailQueryModel m)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app_id, private_key, "json", "1.0", "RSA2", alipay_public_key, "utf-8", false);
            AlipayMarketingCampaignCashDetailQueryRequest request = new AlipayMarketingCampaignCashDetailQueryRequest();
            request.SetBizModel(m);
            AlipayMarketingCampaignCashDetailQueryResponse response = await client.ExecuteAsync(request);
            return response;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="authCode"></param>
        /// <param name="app_id"></param>
        /// <param name="private_key"></param>
        /// <param name="alipay_public_key"></param>
        /// <returns></returns>
        public AlipaySystemOauthTokenResponse GetaliuserInfo(string authCode,string app_id,string private_key,string alipay_public_key)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app_id, private_key, "json", "1.0", "RSA2", alipay_public_key, "utf-8", false);
            var alipayRequest = new AlipaySystemOauthTokenRequest
            {
                Code = authCode,
                GrantType = "authorization_code"
            };
            var alipayResponse = client.Execute(alipayRequest);
            return alipayResponse;


        }
    }
}
