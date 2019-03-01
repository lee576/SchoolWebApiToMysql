using Aop.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// 消息推送帮助类
    /// </summary>
    public class MsgPushHelper
    {
        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="appName">小程序appname</param>
        /// <param name="sendUserId">推送人uid</param>
        /// <param name="formId"></param>
        /// <param name="userTemplateId">模板id</param>
        /// <param name="page">返回页面</param>
        /// <param name="data">模板配置消息</param>
        /// <returns></returns>
        public string SendMsg(string appName, string sendUserId, string formId, string userTemplateId, string page, string data)
        {
            AppHelper helper1 = new AppHelper(appName);
            string appId = helper1.AppId;
            string private_key = helper1.PrivateKey;
            string aliPublicKey = helper1.AlipayPublicKey;
            Aop.Api.IAopClient client = new Aop.Api.DefaultAopClient("https://openapi.alipay.com/gateway.do", appId, private_key, "json", "1.0", "RSA2", aliPublicKey, "GBK", false);
            Aop.Api.Request.AlipayOpenAppMiniTemplatemessageSendRequest request = new Aop.Api.Request.AlipayOpenAppMiniTemplatemessageSendRequest();
            var datainfo = "{" +
                "\"to_user_id\":\"" + sendUserId + "\"," +
                "\"form_id\":\"" + formId + "\"," +
                "\"user_template_id\":\"" + userTemplateId + "\"," +
                "\"page\":\"" + page + "\"," +
                "\"data\":\"" + data + "\"" +
                "}";
            request.BizContent = datainfo;
            Aop.Api.Response.AlipayOpenAppMiniTemplatemessageSendResponse response = client.Execute(request);
            Console.WriteLine(response.Body);
            return response.Body;
        }
    }
    /// <summary>
    /// 消息模板类
    /// </summary>
    public static class UserTemplate
    {
        /// <summary>
        /// 待付款提醒ID
        /// </summary>
        public static string Payment_Reminder = "OGUwNTQ0NzhmMjE4N2E2ZjQwYjcwNjk0OGI2YzJiNTM=";
        /// <summary>
        /// 待付款提醒配置data
        /// </summary>
        public static string Payment_ReminderData { get; set; }
        /// <summary>
        /// 设置待付款提醒消息
        /// </summary>
        /// <param name="money"></param>
        /// <param name="count"></param>
        public static void setPayment_ReminderData(string money, string count)
        {
            Payment_ReminderData = "{\\\"keyword1\\\":{\\\"value\\\":\\\"您好，您有应缴费用项" + count + "个未能付款，请尽快缴纳\\\"}," +
                                    "\\\"keyword2\\\":{\\\"value\\\":\\\"" + money + "元\\\"}}";
        }

        /// <summary>
        /// 交易提醒
        /// </summary>
        public static string Transaction_reminder = "MGQzZWY3OTE0MTgxZGM4MWUwYzkzYzZhNDRjZGVjN2Y=";
        /// <summary>
        /// 交易提醒配置data
        /// </summary>
        public static string Transaction_reminderData { get; set; }
        /// <summary>
        /// 设置交易提醒消息
        /// </summary>
        /// <param name="money"></param>
        /// <param name="count"></param>
        public static void setTransaction_reminderData(string money, string count)
        {
            Transaction_reminderData = "{\\\"keyword1\\\":{\\\"value\\\":\\\"您好，您有应缴费用项"+count+",总计"+money+"元，请尽快缴纳\\\"}" +
                                    "\\\"}";
        }

        /// <summary>
        /// 缴费成功通知消息模板ID
        /// </summary>
        public static string PaySuccessNotice = "479e07815f5d473aa36f03596dd33fbe";
        /// <summary>
        /// 获取支付成功配置信息（data）
        /// </summary>
        public static string PaySuccessNoticeData { get; set; }
        /// <summary>
        /// 设置支付成功消息
        /// </summary>
        /// <param name="money">金额</param>
        /// <param name="payname">缴费名称（水费，电费）</param>
        /// <param name="name">姓名</param>
        /// <param name="paytype">支付方式（支付宝）</param>
        /// <param name="time">支付时间</param>
        /// <param name="remark">备注</param>
        public static void setPaySuccessNoticeData(string money,string payname,string name,string paytype,string time,string remark)
        {
            PaySuccessNoticeData = "{\\\"first\\\":{\\\"value\\\":\\\"您好，您已成功缴费\\\"}," +
                                    "\\\"keyword1\\\":{\\\"value\\\":\\\""+ money + "\\\"}," +
                                    "\\\"keyword2\\\":{\\\"value\\\":\\\""+payname+"\\\"},"+
                                    "\\\"keyword3\\\":{\\\"value\\\":\\\"" + name + "\\\"},"+
                                    "\\\"keyword4\\\":{\\\"value\\\":\\\"" + paytype + "\\\"},"+
                                    "\\\"keyword5\\\":{\\\"value\\\":\\\"" + time + "\\\"},"+
                                    "\\\"remark\\\":{\\\"value\\\":\\\"" + remark + "\\\"}," +
                                    "\\\"}";
        }
        /// <summary>
        /// 缴费任务提醒通知
        /// </summary>
        public static string Payment_Task_Reminder = "8cd96dfd5d694a5ab0919a897811bbf4";
        /// <summary>
        /// 获取缴费任务提醒配置信息（data）
        /// </summary>
        public static string Payment_Task_ReminderData { get; set; }
        /// <summary>
        /// 设置缴费任务提醒
        /// </summary>
        /// <param name="payname">缴费名称</param>
        /// <param name="name">缴费人姓名</param>
        /// <param name="classname">班级</param>
        /// <param name="money">收费金额</param>
        /// <param name="details">收费明细</param>
        /// <param name="remark">备注</param>
        /// <param name="msg_count">缴费任务个数</param>
        public static void setPayment_Task_ReminderData(string payname,string name,string classname,string money,string details,string remark,string msg_count)
        {
            Payment_Task_ReminderData = "{\\\"first\\\":{\\\"value\\\":\\\"您好，您有"+ msg_count + "条缴费任务待处理。\\\"}," +
                                    "\\\"keyword1\\\":{\\\"value\\\":\\\"" + payname + "\\\"}," +
                                    "\\\"keyword2\\\":{\\\"value\\\":\\\"" + name + "\\\"}," +
                                    "\\\"keyword3\\\":{\\\"value\\\":\\\"" + classname + "\\\"}," +
                                    "\\\"keyword4\\\":{\\\"value\\\":\\\"" + money + "\\\"}," +
                                    "\\\"keyword5\\\":{\\\"value\\\":\\\"" + details + "\\\"}," +
                                    "\\\"remark\\\":{\\\"value\\\":\\\"" + remark + "\\\"}," +
                                    "\\\"}";
        }
    }
}
