﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 喜云控制层
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class xiyunController : Controller
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private Itb_xiyun_notifyService _tb_xiyun_notifyService;
        private Itb_school_InfoService _tb_school_InfoService;
        private Itb_school_userService _tb_school_userService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_xiyun_notifyService"></param>
        /// <param name="tb_school_InfoService"></param>
        /// <param name="tb_school_userService"></param>
        public xiyunController(Itb_xiyun_notifyService tb_xiyun_notifyService,
            Itb_school_InfoService tb_school_InfoService,
            Itb_school_userService tb_school_userService)
        {
            _tb_xiyun_notifyService = tb_xiyun_notifyService;
            _tb_school_InfoService = tb_school_InfoService;
            _tb_school_userService = tb_school_userService;
        }
        /// <summary>
        /// 获取所有mid
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult getxiyuncount()
        {
            try
            {

                var list = _tb_xiyun_notifyService.getMIDlist();

                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    data = list,
                    count = list.Count()
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }

        /*
        /// <summary>
        /// 获取所有mid
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public IActionResult UpdateSchoolInfoToSchoolcode(string schoolcode)
        //{
        //    try
        //    {

        //        var list = _tb_xiyun_notifyService.getMIDlist();
        //        string mid = "";
        //        foreach (var item in list)
        //        {
        //            mid += item + ","; 
        //        }
        //        mid = mid.Substring(0, mid.Length - 1);
        //        var schoolinfo = _tb_school_InfoService.FindByClause(x => x.School_Code == schoolcode);
        //        schoolinfo.xiyunMCode = mid;
        //        _tb_school_InfoService.Update(schoolinfo);
        //        return Json(new
        //        {
        //            code = JsonReturnMsg.SuccessCode,
        //            data = mid,
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("错误:" + ex);
        //        return new JsonResult(new { data = ex.ToString() });
        //    }
        //}
        */

        /// <summary>
        /// 查看mid通过schoolcode
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult getxiyunMidToschoolcode(string schoolcode)
        {
            try
            {
                var schoolinfo = _tb_school_InfoService.FindByClause(x => x.School_Code == schoolcode);
                var sp = schoolinfo.xiyunMCode.Split(',');
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    data = schoolinfo,
                    count = sp.Length
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }
        /// <summary>
        /// 通过时间获取当前月食堂信息
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="mids"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetxiyunMidCountInfo(string stime, string etime, string mids)
        {
            var data = _tb_xiyun_notifyService.GetxiyunMidCountInfo(stime, etime, mids);
            return Json(new
            {
                data = data
            });
        }
        /// <summary>
        /// 获取喜云领卡学生消费信息
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="YYMM"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetxiyunGetCardStudentPayCount(string stime, string etime, string YYMM)
        {
            try
            {
                DateTime sTime = Convert.ToDateTime(stime);
                DateTime eTime = Convert.ToDateTime(etime);
                var schooluserlist = _tb_school_userService.FindListByClause(x => x.Collarcard_time >= sTime && x.Collarcard_time <= eTime, t => t.Collarcard_time, SqlSugar.OrderByType.Asc) as List<tb_school_user>;
                log.Info("时段领卡总个数" + schooluserlist.Count);
                var count = _tb_xiyun_notifyService.GetxiyunGetCardStudentPayInfo(stime, etime, schooluserlist, YYMM);
                return Json(new
                {
                    getCardUserCount = schooluserlist.Count,
                    payUserCount = count
                });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(new
                {
                    code = "10000",
                    msg = ex
                });
            }
        }

        /// <summary>
        /// 禧云交易流水推送的消息通知
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task Notify()
        {
            try
            {
                IFormCollection form = Request.Form;

                var xiyunNotify = new tb_xiyun_notify
                {
                    notify_time = form["notify_time"],
                    sign_type = form["sign_type"],
                    charset = form["charset"],
                    notify_type = form["charset"],
                    notify_id = form["notify_id"],
                    appid = form["appid"],
                    merchantCode = form["merchantCode"],
                    bizId = form["bizId"],
                    tradeType = string.IsNullOrEmpty(form["tradeType"]) ? default(int?) : int.Parse(form["tradeType"]),
                    payType = string.IsNullOrEmpty(form["payType"]) ? default(int?) : int.Parse(form["payType"]),
                    tradeNum = form["tradeNum"],
                    orderNum = form["orderNum"],
                    tradeFinishedTime = string.IsNullOrEmpty(form["tradeFinishedTime"]) ? default(DateTime?) : DateTime.Parse(form["tradeFinishedTime"]),
                    receivableAmount = string.IsNullOrEmpty(form["receivableAmount"]) ? default(Double?) : Double.Parse(form["receivableAmount"]) / 100,
                    extendedAmount = string.IsNullOrEmpty(form["extendedAmount"]) ? default(Double?) : Double.Parse(form["extendedAmount"]) / 100,
                    Amount = string.IsNullOrEmpty(form["Amount"]) ? default(Double?) : Double.Parse(form["Amount"]) / 100,
                    thirdUserId = form["thirdUserId"],
                    consumerCode = form["consumerCode"],
                    consumerName = form["consumerName"],
                    canteenId = form["canteenId"],
                    canteenName = form["canteenName"],
                    cashierUserName = form["cashierUserName"],
                    deviceSn = form["deviceSn"],
                    refundBizNo = form["refundBizNo"],
                    refundReason = form["refundReason"]
                };

                tb_cashier_trade_order tradeOrder = new tb_cashier_trade_order()
                {
                    order = form["tradeNum"],
                    user_code = "",//学工号
                    name = "",     //姓名
                    shop = 0,      //消费食堂
                    stall = 0,     //档口
                    machine = 0,   //机器
                    paid = Decimal.Parse(form["receivableAmount"]) / 100, //付款金额WAIT_PAY
                    status = 1,
                    payer_account = form["thirdUserId"],
                    pay_amount = Decimal.Parse(form["Amount"]) / 100,
                    alipay_order = form["tradeNum"],
                    type = form["tradeType"] == "1" ? 0 : 1,
                    refund = form["tradeType"] == "1" ? 0 : Decimal.Parse(form["Amount"]) / 100,
                    trade_name = form["canteenName"],
                    create_time = DateTime.Parse(form["notify_time"]),
                    finish_time = DateTime.Parse(form["tradeFinishedTime"]),
                    @operator = "操作员设备" + form["deviceSn"],
                    terminal_number = form["deviceSn"],
                    alipay_red = 0,
                    collection_treasure = 0,
                    alipay_discount = 0,
                    merchant_discount = 0,
                    ticket_money = 0,
                    ticket_name = null,
                    merchant_red_consumption = 0,
                    card_consumption = 0,
                    refund_batch_number = null,
                    service_charge = 0,
                    shares_profit = 1,
                    remark = form["merchantCode"]
                };

                _tb_xiyun_notifyService.Notify(xiyunNotify, tradeOrder);
                await Response.WriteAsync("success");
            }
            catch (Exception ex)
            {
                log.Error("禧云交易流水推送错误:" + ex);
            }
        }
    }
}