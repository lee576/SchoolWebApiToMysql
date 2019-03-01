using System;
using System.Linq;
using Hangfire;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using IService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using RestSharp;
using SchoolWebApi.Utility;
using Service;
using SqlSugar;
using DbModel;
using SchoolWebApi.Extensions;

namespace SchoolWebApi.HangfireJobs
{
    /// <summary>
    /// 轮询服务类
    /// </summary>
    public class RecurringJobService
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 查询数据库中未完成支付的订单，请求支付宝获取状态，如果完成，则修改状态
        /// </summary>
        /// <param name="context"></param>
        [RecurringJob("*/60 * * * *")]
        [Queue("jobs")]
        public void PayOrders(PerformContext context)
        {
            Itb_payment_recordService _tb_payment_recordService = new tb_payment_recordService();
            Policy.Handle<Exception>()
                 .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
                 {
                     #region 处理订单表中未完成状态的订单数据
                     _tb_payment_recordService.EditOutOrdersStatus(this.SelectedDay);
                     #endregion
                 });
        }
        /// <summary>
        /// 查询数据库中未完成支付的订单，请求支付宝获取状态，如果完成，则修改状态
        /// </summary>
        /// <param name="context"></param>
        [RecurringJob("*/60 * * * *")]
        [Queue("jobs")]
        public void PayElectrictybills(PerformContext context)
        {
            Itb_payment_electricitybillsService _tb_payment_electricitybillsService = new tb_payment_electricitybillsService();
            Itb_appaccounts_itemService _tb_appaccounts_itemService = new tb_appaccounts_itemService();
            Itb_payment_accountsService _tb_payment_accountsService = new tb_payment_accountsService();
            Policy.Handle<Exception>()
                 .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
                 {
                     #region 处理订单表中未完成状态的订单数据
                     _tb_payment_electricitybillsService.EditOutOrdersStatus(this.SelectedDay);
                     var dt = _tb_payment_electricitybillsService.FindListByClause(
                         x => x.pay_time <= DateTime.Now && x.pay_time >= DateTime.Now.AddDays(-5) &&
                              x.pay_status == false, t => t.id, SqlSugar.OrderByType.Asc);
                     if (dt.LongCount() > 0)
                     {
                         foreach (var item in dt)
                         {
                             string ordernumber = item.ordernumber;
                             int appaccounts_id = Convert.ToInt32(item.appaccounts_id);
                             var appaccount = _tb_appaccounts_itemService.FindByClause(x => x.id == appaccounts_id);
                             var payment_accounts = _tb_payment_accountsService.FindByClause(x => x.id == appaccount.accounts_id);
                             bool isOk = AliPayHelper.GetOrdersStatus2(ordernumber, payment_accounts.appid, payment_accounts.private_key, payment_accounts.alipay_public_key);
                             if (isOk)
                             {
                                 var client = new RestClient("http://59.51.42.150:8003/api/Pay/RemotePay");
                                 var request = new RestRequest { Method = Method.POST };
                                 request.AddHeader("Accept", "application/x-www-form-urlencoded");
                                 request.Parameters.Clear();
                                 request.AddJsonBody(new { UserCode = item.room_id, FactMoney = item.pay_amount, PayDate = item.pay_time, TransId = item.ordernumber, PayWay = 5 });
                                 var response = client.Execute(request);
                                 var content = response.Content; // 返回的网页内容
                                 var dd = JsonConvert.DeserializeObject<ReturnPayInfo>(content);
                                 if (dd.ResultCode == 1)
                                 {
                                     item.pay_status = true;
                                     _tb_payment_electricitybillsService.Update(item);
                                 }
                             }
                         }
                     }
                     #endregion
                 });
        }
        ///// <summary>
        /////定时更新学生班级id信息
        ///// </summary>
        ///// <param name="context"></param>
        //[RecurringJob("*/60 * * * *")]
        //[Queue("jobs")]
        //public void UpdateSchoolUserInfoToDepartmentid(PerformContext context)
        //{
        //    Itb_school_InfoService _tb_school_InfoService = new tb_school_InfoService();
        //    Itb_school_userService _tb_school_userService = new tb_school_userService();
        //    Itb_school_departmentService _tb_school_departmentService = new tb_school_departmentService();
        //    Policy.Handle<Exception>()
        //         .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1)).Execute(() =>
        //         {
        //             try
        //             {
        //                 var schoolinfo = _tb_school_InfoService.FindAll();
        //                 var department = _tb_school_departmentService.FindAll();
        //                 foreach (var item in schoolinfo)
        //                 {
        //                     string schoolcode = item.School_Code.Trim();
        //                     var department2 = department.Where(x => x.schoolcode == schoolcode && x.p_id != 0).ToList();
        //                     foreach (var item2 in department2)
        //                     {
        //                         var school_user = _tb_school_userService.FindListByClause(x => x.school_id == item2.schoolcode
        //                         && !SqlFunc.HasValue(x.department_id), t => t.user_id, SqlSugar.OrderByType.Desc);

        //                         var modelList2 = school_user.GroupBy(x => x.department);

        //                         var modelList = school_user.GroupBy(x => x.department)
        //                         .Select(x => new DepartmentGroupModel
        //                         {
        //                             departmentname = x.Key,
        //                             List = x.ToList()
        //                         });
        //                         foreach (var item3 in modelList)
        //                         {
        //                             string departmentnameinfo = "";
        //                             int count = 0;
        //                             string[] sp = null;
        //                             int class_id = 0;
        //                             if (string.IsNullOrEmpty(item3.departmentname))
        //                             {
        //                                 continue;
        //                             }

        //                             if (item3.departmentname.Contains('/'))
        //                             {
        //                                 sp = item3.departmentname.Split('/');
        //                                 count = sp.Length;
        //                                 int index = count - 1;
        //                                 departmentnameinfo = sp[index].Trim();
        //                             }
        //                             if (string.IsNullOrEmpty(departmentnameinfo))
        //                             {
        //                                 departmentnameinfo = item3.departmentname.Trim();
        //                             }
        //                             if (departmentnameinfo.Contains(item2.name.Trim()))
        //                             {
        //                                 var list = department2.Where(x => x.name == departmentnameinfo).ToList();
        //                                 if (list.Count == 0)
        //                                 {
        //                                     continue;
        //                                 }
        //                                 class_id = list[0].id;
        //                                 foreach (var citem in item3.List)
        //                                 {
        //                                     citem.department_id = class_id;
        //                                     _tb_school_userService.Update(citem);
        //                                 }
        //                             }
        //                         }
        //                     }
        //                 }
        //             }
        //             catch (Exception ex)
        //             {
        //                 log.Error(ex);
        //                 throw;
        //             }
        //         });
        //}

        /// <summary>
        /// 注册绑定校园设备到支付宝
        /// </summary>
        /// <param name="context"></param>
        [RecurringJob("*/60 * * * *")]
        [Queue("jobs")]
        public void UpShooldevice(PerformContext context)
        {
            try
            {

                //设置一天只执行一次的范围时间
                string _strWorkingDayAM = "22:00";
                string _strWorkingDayPM = "23:00";
                TimeSpan dspWorkingDayAM = DateTime.Parse(_strWorkingDayAM).TimeOfDay;
                TimeSpan dspWorkingDayPM = DateTime.Parse(_strWorkingDayPM).TimeOfDay;
                DateTime t1 = Convert.ToDateTime(DateTime.Now);

                TimeSpan dspNow = t1.TimeOfDay;
                if (!(dspNow >= dspWorkingDayAM && dspNow <= dspWorkingDayPM))
                {
                    return;
                }

                //获取未绑定设备
                Itb_school_deviceService device = new tb_school_deviceService();
                var list = device.FindListByClause(p => p.device_state != 2 || p.shop_id == null || p.shop_id == "", p => p.id, OrderByType.Asc).ToList();

                AppHelper helper = new AppHelper("Alipay");
                Aop.Api.IAopClient client = new Aop.Api.DefaultAopClient("https://openapi.alipay.com/gateway.do", helper.AppId, helper.PrivateKey, "json", "1.0", "RSA2", helper.AlipayPublicKey, "utf-8", false);
                Aop.Api.Request.AlipayCommerceDataSendRequest request = new Aop.Api.Request.AlipayCommerceDataSendRequest();
                foreach (var item in list)
                {
                    string schoolpid = "";
                    string shoopid = "";
                    //获取设备学校信息取学校PID
                    Itb_school_InfoService _tb_school_InfoService = new tb_school_InfoService();
                    if (!string.IsNullOrEmpty(item.shop_id))
                    {
                        var schoolinfo = _tb_school_InfoService.FindByClause(p => p.School_Code == item.school_id);
                        schoolpid = schoolinfo.pid;
                        shoopid = item.shop_id;
                    }

                    request.BizContent = "{\n" +
                                         "        \"scene_code\": \"device_info\",\n" +
                                         "        \"op_code\": \"register\",\n" +
                                         "        \"channel\": \"iot_cloud\",\n" +
                                         "        \"version\": \"2.0\",\n" +
                                         "        \"op_data\": [\n" +
                                         "            {\n" +
                                         "                \"item_id\": \"" + helper.ItemID + "\",\n" +
                                         "                \"supplier_sn\": \"" + helper.SupplierID + "\",\n" +
                                         "                \"device_sn\": \"" + item.device_id + "\",\n" +
                                         "                \"mac\": \"" + item.device_id + "\",\n" +
                                         "                \"imei\": \"\",\n" +
                                         "                \"os_version\": \"linux\",\n" +
                                         "                \"net_type\": \"WIFI\",\n" +
                                         "                \"isv_id\": \"" + helper.Pid + "\",\n" +
                                         "                \"pid\": \"" + schoolpid + "\",\n" +
                                         "                \"shop_id\": \"" + shoopid + "\",\n" +
                                         "                \"plain_text\": \"\",\n" +
                                         "                \"sign\": \"\",\n" +
                                         "                \"ext_info\": {}\n" +
                                         "            }\n" +
                                         "        ]\n" +
                                         "    }";
                    Aop.Api.Response.AlipayCommerceDataSendResponse response = client.Execute(request);
                    //如果注册成功
                    if (((JObject)JsonConvert.DeserializeObject(response.Body))["alipay_commerce_data_send_response"]["code"].ToString() == "10000")
                    {
                        //查询服务器该设备状态回写到本地记录
                        Aop.Api.Request.AlipayCommerceIotMdeviceprodDeviceQueryRequest reg = new Aop.Api.Request.AlipayCommerceIotMdeviceprodDeviceQueryRequest();
                        Aop.Api.Domain.AlipayCommerceIotMdeviceprodDeviceQueryModel m = new Aop.Api.Domain.AlipayCommerceIotMdeviceprodDeviceQueryModel();
                        m.DeviceSn = item.device_id;
                        m.ItemId = helper.ItemID;
                        m.SupplierSn = helper.SupplierID;
                        reg.SetBizModel(m);

                        Aop.Api.Response.AlipayCommerceIotMdeviceprodDeviceQueryResponse res = client.Execute(reg);
                        if (((JObject)JsonConvert.DeserializeObject(res.Body))["alipay_commerce_iot_mdeviceprod_device_query_response"]["code"].ToString() == "10000")
                        {
                            //未注册
                            int state = 0;
                            tb_school_device devmodel = device.FindById(item.id);
                            if (((JObject)JsonConvert.DeserializeObject(res.Body))["alipay_commerce_iot_mdeviceprod_device_query_response"]["bind_status"] != null)
                            {
                                if (((JObject)JsonConvert.DeserializeObject(res.Body))["alipay_commerce_iot_mdeviceprod_device_query_response"]["bind_status"].ToString() == "BIND")
                                {
                                    //已注册未完全绑定
                                    state = 1;
                                    if (((JObject)JsonConvert.DeserializeObject(res.Body))["alipay_commerce_iot_mdeviceprod_device_query_response"]["shop_id"] != null)
                                    {
                                        //有门店ID说明全部绑定完成
                                        state = 2;
                                        devmodel.shop_id = ((JObject)JsonConvert.DeserializeObject(res.Body))["alipay_commerce_iot_mdeviceprod_device_query_response"]["shop_id"].ToString();
                                    }
                                }
                            }
                            devmodel.device_state = state;
                            device.Update(devmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ExtractAllStackTrace());
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SelectedDay { get; } = "5";
    }
}

