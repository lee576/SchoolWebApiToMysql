﻿using System;
using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SchoolWebApi;
using SchoolWebApi.Utility;
using ServiceStack.Redis;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 门禁设备向服务端接口传入门禁数据
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class UploadRecordController : Controller
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private readonly Itb_entrance_recordService _tb_entrance_record;
        private readonly Itb_school_deviceService _tb_school_device;
        private readonly Itb_school_userService _tb_school_userService;
        private readonly Itb_school_InfoService _tb_school_InfoService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_entrance_record"></param>
        /// <param name="tb_school_device"></param>
        /// <param name="tb_school_userService"></param>
        /// <param name="tb_school_InfoService"></param>
        public UploadRecordController(
          Itb_entrance_recordService tb_entrance_record,
            Itb_school_deviceService tb_school_device, Itb_school_userService tb_school_userService, Itb_school_InfoService tb_school_InfoService)
        {
            _tb_entrance_record = tb_entrance_record;
            _tb_school_device = tb_school_device;
            _tb_school_userService = tb_school_userService;
            _tb_school_InfoService = tb_school_InfoService;
        }

        /// <summary>
        /// 设备心跳状态
        /// </summary>
        /// <param name="DeviceTime">设备时间：171018142347 （2017-10-18 14:23:47）</param>
        /// <param name="Version">Version: 设备版本号</param>
        /// <param name="DoorStatus">DoorStatus：00第1个0表示门1门磁是状态，第2个0表示门2门磁状态0表示闭合的，1表示断开的，如果只有一个门，则门2值不用管</param>
        /// <param name="DeviceID"></param>
        /// <param name="SN">项目编号:  项目编号，不用默认填:001</param>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult HeatBeat(
            [FromForm] string DeviceTime,
            [FromForm] string Version,
            [FromForm] string DoorStatus,
            [FromForm] string DeviceID,
            [FromForm] string SN,
            [FromForm] string UserName,
            [FromForm] string PassWord)
        {
            try
            {
                //设备心跳事件传递过来的设备当前时间
                var deviceDate = DateTime.Parse("20" + DeviceTime.Substring(0, 2) + "-" + DeviceTime.Substring(2, 2) + "-" +
                                                DeviceTime.Substring(4, 2) + " " + DeviceTime.Substring(6, 2) + ":" +
                                                DeviceTime.Substring(8, 2) + ":" + DeviceTime.Substring(10, 2));

                //标准北京时间
                var webTime = TimeHelper.GetWebTime();
                //得出设备时间与北京时间的时间差
                TimeSpan timeSpan = webTime - deviceDate;
                var minutes = timeSpan.TotalMinutes;

                //设备心跳时间与北京时间误差,当大于一分钟的时候,向设备返回北京时间,以此校正设备时间
                if (minutes > 1)
                {
                    var webTimeStr = webTime.Year.ToString().Substring(2, 2) + webTime.ToString("MMddhhmmss");
                    return Json(new { ResultCode = "2", CorrectTime = webTimeStr });
                }
                AppHelper helper = new AppHelper("Alipay");

                //心跳数据上传支付宝云端
                Aop.Api.IAopClient client = new Aop.Api.DefaultAopClient("https://openapi.alipay.com/gateway.do", helper.AppId, helper.PrivateKey, "json", "1.0", "RSA2", helper.AlipayPublicKey, "utf-8", false);
                Aop.Api.Request.AlipayCommerceDataSendRequest request =
                    new Aop.Api.Request.AlipayCommerceDataSendRequest
                    {
                        BizContent = "{\n" +
                                     "        \"scene_code\": \"iot_heartbeat_event\",\n" +
                                     "        \"op_code\": \"data_send\",\n" +
                                     "        \"channel\": \"iot_cloud\",\n" +
                                     "        \"version\": \"2.0\",\n" +
                                     "        \"op_data\": [\n" +
                                     "            {\n" +
                                     "                \"supplier_sn\": \"" + helper.SupplierID + "\",\n" +
                                     "                \"device_sn\": \"" + DeviceID + "\",\n" +
                                     "                \"biz_tid\": \"\",\n" +
                                     "                \"log_version\": \"1\",\n" +
                                     "                \"os_version\": \"linux\",\n" +
                                     "                \"net_type\": \"2G\",\n" +
                                     "                \"biz_time\": \"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") +
                                     "\",\n" +
                                     "                \"poi\":\"\",\n" +
                                     "                \"online_status\": \"ONLINE\",\n" +
                                     "                \"signal_intensity\": \"H\"\n" +
                                     "            }\n" +
                                     "        ]\n" +
                                     "    }"
                    };
                Aop.Api.Response.AlipayCommerceDataSendResponse response = client.Execute(request);
                return Json(new { ResultCode = "1", CorrectTime = "" });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new { ResultCode = "1", CorrectTime = "" });
            }
        }

        /// <summary>
        /// 门禁开、关门记录
        /// </summary>
        /// <param name="SCode"></param>
        /// <param name="DeviceID"></param>
        /// <param name="SN"></param>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertRecord(
            [FromForm] string SCode,
            [FromForm] string DeviceID,
            [FromForm] string SN,
            [FromForm] string UserName,
            [FromForm] string PassWord)
        {
            try
            {
                //事件号
                var eventNo = Int32.Parse(SCode.Substring(20, 2), System.Globalization.NumberStyles.HexNumber);
                //卡号
                var cardNo = SCode.Substring(0, 8);
                //事件时间
                var eventTime = SCode.Substring(8, 12);
                //事件时间转为yyyy-MM-dd hh:mm:ss
                var openTime = "20" + eventTime.Substring(0, 2) + "-" +
                               eventTime.Substring(2, 2) + "-" +
                               eventTime.Substring(4, 2) + " " +
                               eventTime.Substring(6, 2) + ":" +
                               eventTime.Substring(8, 2) + ":" +
                               eventTime.Substring(10, 2);

                //读头号
                var entrance_status = string.Empty;
                var readNo = Int32.Parse(SCode.Substring(22, 2), System.Globalization.NumberStyles.HexNumber);
                switch (readNo)
                {
                    case 0:
                        entrance_status = "进";
                        break;
                    case 1:
                        entrance_status = "出";
                        break;
                }

                //事件号70 表示正常开门,除了正常开门以外的记录没有入库的意义，直接返回
                if (eventNo != 70)
                {
                    //没有意义的扫码记录移出缓存
                    using (RedisClient redisClient = RedisHelper.CreateClient())
                    {
                        redisClient.Remove(cardNo);
                    }
                    return Json(new { ResultCode = "1", msg = "上传成功" });
                }

                //门禁进出数据插入数据库
                using (RedisClient redisClient = RedisHelper.CreateClient())
                {
                    var entranceRecord = redisClient.Get<tb_entrance_record>(cardNo);
                    if (entranceRecord != null)
                    {
                        entranceRecord.device_id = DeviceID;
                        entranceRecord.open_time = DateTime.Parse(openTime);
                        entranceRecord.entrance_status = entrance_status;

                        _tb_entrance_record.Insert(entranceRecord);
                        redisClient.Remove(cardNo);

                        // 行为上传阿里云
                        AppHelper helper = new AppHelper("Alipay");
                        Aop.Api.IAopClient client = new Aop.Api.DefaultAopClient("https://openapi.alipay.com/gateway.do", helper.AppId, helper.PrivateKey, "json", "1.0", "RSA2", helper.AlipayPublicKey, "utf-8", false);

                        //获取学生学校设备信息
                        var users = _tb_school_userService.FindById(entranceRecord.user_id);
                        var schoolinfo = _tb_school_InfoService.FindByClause(p => p.School_Code == users.school_id);
                        var device = _tb_school_device.FindByClause(p => p.device_id == DeviceID);

                        if (!_tb_school_device.Any(t => t.device_id == DeviceID))
                        {
                            _tb_school_device.Insert(new tb_school_device { device_id = DeviceID, school_id = schoolinfo.School_Code });
                            //注册设备
                            Aop.Api.Request.AlipayCommerceDataSendRequest req =
                                new Aop.Api.Request.AlipayCommerceDataSendRequest
                                {
                                    BizContent = "{\n" +
                                                 "        \"scene_code\": \"device_info\",\n" +
                                                 "        \"op_code\": \"register\",\n" +
                                                 "        \"channel\": \"iot_cloud\",\n" +
                                                 "        \"version\": \"2.0\",\n" +
                                                 "        \"op_data\": [\n" +
                                                 "            {\n" +
                                                 "                \"item_id\": \"" + helper.ItemID + "\",\n" +
                                                 "                \"supplier_sn\": \"" + helper.SupplierID + "\",\n" +
                                                 "                \"device_sn\": \"" + DeviceID + "\",\n" +
                                                 "                \"mac\": \"" + DeviceID + "\",\n" +
                                                 "                \"imei\": \"\",\n" +
                                                 "                \"os_version\": \"linux\",\n" +
                                                 "                \"net_type\": \"WIFI\",\n" +
                                                 "                \"isv_id\": \"" + helper.Pid + "\",\n" +
                                                 "                \"pid\": \"" + schoolinfo.pid + "\",\n" +
                                                 "                \"shop_id\": \"\",\n" +
                                                 "                \"plain_text\": \"\",\n" +
                                                 "                \"sign\": \"\",\n" +
                                                 "                \"ext_info\": {}\n" +
                                                 "            }\n" +
                                                 "        ]\n" +
                                                 "    }"
                                };
                            Aop.Api.Response.AlipayCommerceDataSendResponse resp = client.Execute(req);
                        }

                        //上传行为数据
                        Aop.Api.Request.AlipayCommerceDataSendRequest request =
                            new Aop.Api.Request.AlipayCommerceDataSendRequest
                            {
                                BizContent = "{\n" +
                                             "        \"scene_code\": \"school_guard_event\",\n" +
                                             "        \"op_code\": \"data_send\",\n" +
                                             "        \"channel\": \"iot_cloud\",\n" +
                                             "        \"version\": \"2.0\",\n" +
                                             "        \"target_id\": \"" + users.ali_user_id + "\",\n" +
                                             "        \"op_data\": [{\n" +
                                             "            \"biz_type\": \"FUTURE_CAMPUS\",\n" +
                                             "            \"scene\": \"GUARD\",\n" +
                                             "            \"item_id\": \"" + helper.ItemID + "\",\n" +
                                             "            \"supplier_sn\": \"" + helper.SupplierID + "\",\n" +
                                             "            \"device_sn\": \"" + DeviceID + "\",\n" +
                                             "            \"biz_tid\": \"\",\n" +
                                             "            \"biz_time\": \"" + DateTime.Now + "\",\n" +
                                             "            \"appid\": \"" + helper.AppId + "\",\n" +
                                             "            \"uid\": \"" + users.ali_user_id + "\",\n" +
                                             "            \"code_type\": \"ALIPAY\",\n" +
                                             "            \"identify_type\": \"CODE\",\n" +
                                             "            \"school_pid\": \"" + schoolinfo.pid + "\",\n" +
                                             "            \"school_name\": \"" + schoolinfo.School_name + "\",\n" +
                                             "            \"isv_pid\": \"" + helper.Pid + "\",\n" +
                                             "            \"isv_name\": \"湖北若火科技有限公司\",\n" +
                                             "            \"shop_id\": \"" + device.shop_id + "\",\n" +
                                             "            \"result\": \"T\",\n" +
                                             "            \"place\": \"GYM\",\n" +
                                             "            \"ext_info\": {}\n" +
                                             "        }]\n" +
                                             "    }"
                            };
                        Aop.Api.Response.AlipayCommerceDataSendResponse response = client.Execute(request);
                    }
                }
                return Json(new { ResultCode = "1", msg = "上传成功" });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return Json(new { ResultCode = "0", msg = "上传失败" });
            }
        }
    }
}
