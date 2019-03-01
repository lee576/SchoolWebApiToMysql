using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Domain;
using Alipay.AopSdk.Core.Response;
using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SchoolWebApi.Utility;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 红包活动控制层
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class RedPacketsController : Controller
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private AliRedPacketHelper aliRed;
        private AliRedPacketHelper aliRed2;//2为若火公司
        private Itb_RedPacketService _tb_RedPacketService;
        private Itb_RedPacket_MIDService _tb_RedPacket_MIDService;
        private Itb_RedPacket_JumpURLService _tb_RedPacket_JumpURLService;
        private Itb_RedPacket_TriggerService _tb_RedPacket_TriggerService;
        private Itb_xiyun_notifyService _tb_xiyun_notifyService;
        private Itb_school_InfoService _tb_school_InfoService;
        private Itb_RedPacket_AllMidService _tb_RedPacket_AllMidService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_RedPacketService"></param>
        /// <param name="tb_RedPacket_MIDService"></param>
        /// <param name="tb_RedPacket_JumpURLService"></param>
        /// <param name="tb_RedPacket_TriggerService"></param>
        /// <param name="tb_xiyun_notifyService"></param>
        /// <param name="tb_school_InfoService"></param>
        /// <param name="tb_RedPacket_AllMidService"></param>
        public RedPacketsController(Itb_RedPacketService tb_RedPacketService,
            Itb_RedPacket_MIDService tb_RedPacket_MIDService,
            Itb_RedPacket_JumpURLService tb_RedPacket_JumpURLService,
            Itb_RedPacket_TriggerService tb_RedPacket_TriggerService,
            Itb_xiyun_notifyService tb_xiyun_notifyService,
            Itb_school_InfoService tb_school_InfoService,
            Itb_RedPacket_AllMidService tb_RedPacket_AllMidService)
        {
            _tb_RedPacketService = tb_RedPacketService;
            _tb_RedPacket_MIDService = tb_RedPacket_MIDService;
            _tb_RedPacket_JumpURLService = tb_RedPacket_JumpURLService;
            _tb_RedPacket_TriggerService = tb_RedPacket_TriggerService;
            _tb_xiyun_notifyService = tb_xiyun_notifyService;
            _tb_school_InfoService = tb_school_InfoService;
            _tb_RedPacket_AllMidService = tb_RedPacket_AllMidService;
            aliRed = new AliRedPacketHelper("RedPacketsUserInfo2");
            aliRed2 = new AliRedPacketHelper("RedPacketsUserInfo");
        }
        /// <summary>
        /// 创建红包
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreatRedPacket([FromBody]JObject obj)
        {
            try
            {
                AlipayMarketingCampaignCashCreateModel m = new AlipayMarketingCampaignCashCreateModel()
                {
                    CouponName = obj["coupon_name"].ToString(),
                    PrizeType = obj["prize_type"].ToString() == "0" ? "fixed" : "random",
                    TotalMoney = obj["total_money"].ToString(),
                    TotalNum = obj["total_num"].ToString(),
                    PrizeMsg = obj["prize_msg"].ToString(),
                    StartTime = DateTime.Parse(obj["start_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                    EndTime = DateTime.Parse(obj["end_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                    MerchantLink = "",
                    SendFreqency = obj["send_freqency"].ToString()
                };
                var res = aliRed.CreateRedPack(m);

                //获取当前状态
                AlipayMarketingCampaignCashDetailQueryModel m2 = new AlipayMarketingCampaignCashDetailQueryModel();
                m2.CrowdNo = res.CrowdNo;
                var res2 = aliRed.DetailQueryRedPack(m2);

                if (res.Code == "10000")
                {
                    tb_redpacket redinfo = new tb_redpacket()
                    {
                        coupon_name = m.CouponName,
                        end_time = Convert.ToDateTime(m.EndTime),
                        merchant_link = m.MerchantLink,
                        prize_msg = m.PrizeMsg,
                        prize_type = m.PrizeType,
                        send_freqency = m.SendFreqency,
                        start_time = Convert.ToDateTime(m.StartTime),
                        total_money = Convert.ToDecimal(m.TotalMoney),
                        total_num = Convert.ToInt32(m.TotalNum),
                        pay_url = res.PayUrl,
                        origin_crowd_no = res.OriginCrowdNo,
                        crowd_no = res.CrowdNo,
                        camp_status = res2.CampStatus,
                        paycount = Convert.ToInt32(obj["paycount"].ToString())
                    };
                    //插入数据不包含 paycount camp_status 
                    long redpacketID = _tb_RedPacketService.Insert(redinfo);
                    //给红包绑定mid组，和url组
                    if (obj["mid"].ToString().Contains('|'))
                    {
                        var sp = obj["mid"].ToString().Split('|');
                        List<tb_redpacket_mid> tblist = new List<tb_redpacket_mid>();
                        foreach (var item in sp)
                        {
                            if (_tb_RedPacket_MIDService.Any(x => x.id == redpacketID && x.mid == item))
                            {
                                continue;
                            }
                            tb_redpacket_mid tb = new tb_redpacket_mid();
                            tb.mid = item;
                            tb.redpacket_id = Convert.ToInt32(redpacketID);
                            tblist.Add(tb);
                        }
                        _tb_RedPacket_MIDService.Insert(tblist);
                    }
                    else
                    {
                        if (!_tb_RedPacket_MIDService.Any(x => x.id == redpacketID && x.mid == obj["mid"].ToString()))
                        {
                            tb_redpacket_mid tb = new tb_redpacket_mid();
                            tb.mid = obj["mid"].ToString();
                            tb.redpacket_id = Convert.ToInt32(redpacketID);
                            _tb_RedPacket_MIDService.Insert(tb);
                        }
                    }
                    if (!obj["jumpUrl"].ToString().Equals(""))
                    {
                        if (!obj["jumpUrl"].ToString().Contains('|') && !obj["jumpUrl"].ToString().Equals(""))
                        {
                            var sp = obj["jumpUrl"].ToString().Split('*');
                            tb_redpacket_jumpurl tb = new tb_redpacket_jumpurl();
                            tb.jumpUrl = sp[1];
                            tb.activityName = sp[0];
                            tb.pay_Index = Convert.ToInt32(sp[2] == "" ? "0" : sp[2]);
                            tb.redpacket_id = Convert.ToInt32(redpacketID);
                            _tb_RedPacket_JumpURLService.Insert(tb);
                        }
                        else
                        {
                            var sp = obj["jumpUrl"].ToString().Split('|');
                            List<tb_redpacket_jumpurl> tblist = new List<tb_redpacket_jumpurl>();
                            foreach (var item in sp)
                            {
                                var sp2 = item.Split('*');
                                tb_redpacket_jumpurl tb = new tb_redpacket_jumpurl();
                                tb.jumpUrl = sp2[1];
                                tb.activityName = sp2[0];
                                tb.pay_Index = Convert.ToInt32(sp2[2] == "" ? "0" : sp2[2]);
                                tb.redpacket_id = Convert.ToInt32(redpacketID);
                                tblist.Add(tb);
                            }
                            _tb_RedPacket_JumpURLService.Insert(tblist);
                        }
                    }
                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        msg = JsonReturnMsg.AddSuccess,
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = res
                    });
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }
        /// <summary>
        /// 获取红包活动信息
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="code"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRedPacketInfo(int sEcho, int code, int iDisplayStart, int iDisplayLength)
        {
            try
            {
                int pageStart = iDisplayStart;
                int pageSize = iDisplayLength;
                int pageIndex = (pageStart / pageSize) + 1;
                var data = _tb_RedPacketService.FindPagedListOrderyType(pageIndex: pageIndex, pageSize: pageSize, expression: x => x.start_time, type: SqlSugar.OrderByType.Desc);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    sEcho = sEcho,
                    iTotalRecords = data.TotalCount,
                    iTotalDisplayRecords = data.TotalCount,
                    aaData = data
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }
        /// <summary>
        /// 根据红包号查询红包活动信息
        /// </summary>
        /// <param name="CrowdNo"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRedPacketInfoToCrowdNo(string CrowdNo)
        {
            try
            {

                var data = _tb_RedPacketService.FindByClause(x => x.crowd_no == CrowdNo);
                var mid = _tb_RedPacket_MIDService.FindListByClause(x => x.redpacket_id == data.id, x => x.id, SqlSugar.OrderByType.Asc);
                var JumpUrl = _tb_RedPacket_JumpURLService.FindListByClause(x => x.redpacket_id == data.id, x => x.id, SqlSugar.OrderByType.Asc);
                return Json(new
                {
                    code = "10000",
                    msg = "查询成功",
                    data = data,
                    mid = mid,
                    jumpinfo = JumpUrl
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }
        /// <summary>
        /// 修改红包最低消费次数和mid以及活动url
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateRedPacketInfo([FromBody]JObject obj)
        {
            try
            {
                var data = _tb_RedPacketService.FindByClause(x => x.crowd_no == obj["crowd_no"].ToString());
                data.paycount = Convert.ToInt32(obj["paycount"].ToString());
                _tb_RedPacketService.Update(data);
                int redpacketID = data.id;
                _tb_RedPacket_MIDService.Delete(x => x.redpacket_id == data.id);
                if (obj["mid"].ToString().Contains('|'))
                {
                    var sp = obj["mid"].ToString().Split('|');
                    List<tb_redpacket_mid> tblist = new List<tb_redpacket_mid>();
                    foreach (var item in sp)
                    {

                        tb_redpacket_mid tb = new tb_redpacket_mid();
                        tb.mid = item;
                        tb.redpacket_id = Convert.ToInt32(redpacketID);
                        tblist.Add(tb);
                    }
                    _tb_RedPacket_MIDService.Insert(tblist);
                }
                else
                {
                    if (!_tb_RedPacket_MIDService.Any(x => x.id == redpacketID && x.mid == obj["mid"].ToString()))
                    {
                        tb_redpacket_mid tb = new tb_redpacket_mid();
                        tb.mid = obj["mid"].ToString();
                        tb.redpacket_id = Convert.ToInt32(redpacketID);
                        _tb_RedPacket_MIDService.Insert(tb);
                    }

                }
                if (_tb_RedPacket_JumpURLService.Any(x => x.redpacket_id == data.id))
                {
                    _tb_RedPacket_JumpURLService.Delete(x => x.redpacket_id == data.id);

                }
                if (!obj["jumpUrl"].ToString().Equals(""))
                {
                    if (!obj["jumpUrl"].ToString().Contains('|') && !obj["jumpUrl"].ToString().Equals(""))
                    {
                        var sp = obj["jumpUrl"].ToString().Split('*');
                        tb_redpacket_jumpurl tb = new tb_redpacket_jumpurl();
                        tb.jumpUrl = sp[1];
                        tb.activityName = sp[0];
                        tb.pay_Index = Convert.ToInt32(sp[2] == "" ? "0" : sp[2]);
                        tb.redpacket_id = Convert.ToInt32(redpacketID);
                        _tb_RedPacket_JumpURLService.Insert(tb);
                    }
                    else
                    {
                        var sp = obj["jumpUrl"].ToString().Split('|');
                        List<tb_redpacket_jumpurl> tblist = new List<tb_redpacket_jumpurl>();
                        foreach (var item in sp)
                        {
                            var sp2 = item.Split('*');
                            tb_redpacket_jumpurl tb = new tb_redpacket_jumpurl();
                            tb.jumpUrl = sp2[1];
                            tb.activityName = sp2[0];
                            tb.pay_Index = Convert.ToInt32(sp2[2] == "" ? "0" : sp2[2]);
                            tb.redpacket_id = Convert.ToInt32(redpacketID);
                            tblist.Add(tb);
                        }
                        _tb_RedPacket_JumpURLService.Insert(tblist);
                    }
                }
                return Json(new
                {
                    code = "10000",
                    msg = "修改成功"
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return new JsonResult(new
                {
                    code = "20000",
                    msg = "修改失败"
                });
            }
        }
        /// <summary>
        /// 获取红包活动，领款详细接口
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="code"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="crowd_no"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRedPacketTrigger(int sEcho, int code, int iDisplayStart, int iDisplayLength, string crowd_no)
        {
            try
            {
                int pageStart = iDisplayStart;
                int pageSize = iDisplayLength;
                int pageIndex = (pageStart / pageSize) + 1;
                var data = _tb_RedPacket_TriggerService.FindPagedList(predicate: x => x.crowd_no == crowd_no, orderBy: "id", pageIndex: pageIndex, pageSize: pageSize);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    sEcho = sEcho,
                    iTotalRecords = data.TotalCount,
                    iTotalDisplayRecords = data.TotalCount,
                    aaData = data
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }
        /// <summary>
        /// 领取红包调用接口
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TriggerRedPacketInfo([FromBody]JObject obj)
        {
            try
            {
                string UserId = obj["UserId"].ToString();
                string CrowdNo = obj["CrowdNo"].ToString();
                DateTime day = DateTime.Now;
                //获取红包项目信息
                var redPacketInfo = _tb_RedPacketService.FindByClause(x => x.start_time <= day && x.end_time >= day && x.crowd_no == CrowdNo);
                if (redPacketInfo == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        errmsg = "活动未开启",
                        msg = "/m/xchl/red_activity.html"
                    });
                }
                AlipayMarketingCampaignCashDetailQueryModel mmm = new AlipayMarketingCampaignCashDetailQueryModel()
                {
                    CrowdNo = redPacketInfo.crowd_no
                };
                var dqr = aliRed.DetailQueryRedPack(mmm);
                int totle_count = 0;
                //查询红包当前状态
                if (dqr.CampStatus == "READY")
                {
                    CrowdNo = redPacketInfo.crowd_no;
                    totle_count = Convert.ToInt32(redPacketInfo.paycount);//当前 红包活动领取红包资格数
                }
                var midlist = _tb_RedPacket_MIDService.FindListByClause(x => x.redpacket_id == redPacketInfo.id, x => x.id, SqlSugar.OrderByType.Asc) as List<tb_redpacket_mid>;
                string xiyunMCode = "";
                for (int i = 0; i < midlist.Count; i++)
                {
                    xiyunMCode += midlist[i].mid + ",";
                }
                xiyunMCode = xiyunMCode.Substring(0, xiyunMCode.Length - 1);
                xiyunMCode = xiyunMCode.Replace(",", "','");
                //查询当前支付次数    发布时要加上-------------------------
                int xiyun_count = _tb_xiyun_notifyService.CountToUserid(UserId, xiyunMCode);//用户当前支付次数
                if (totle_count == 0)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = "/m/xchl/red_activity.html"
                    });
                }
                //log.Info(xiyun_count)
                //int xiyun_count = 1;
                if (xiyun_count >= totle_count)
                {
                    string outbizno = DateTime.Now.ToString("yyyyMMddHHmmssffff") + new Random().Next(1000).ToString().PadLeft(4, '0');
                    AlipayMarketingCampaignCashTriggerModel mm = new AlipayMarketingCampaignCashTriggerModel()
                    {
                        OutBizNo = outbizno,
                        CrowdNo = obj["CrowdNo"].ToString(),//"pzC2ZqVLsqpdGl2uacYYndHU8JIBTv9EeUtdCWwP1-Wsn4ge4wBRFkiLOjq1h1Jd",
                        UserId = obj["UserId"].ToString()//"2088802186716632"
                    };
                    var res = aliRed.TriggerRedPack(mm);
                    log.Info(res.Body);
                    if (res.Code == "10000")
                    {

                        tb_redpacket_trigger trigger = new tb_redpacket_trigger();
                        trigger.biz_no = res.BizNo;
                        trigger.coupon_name = res.CouponName;
                        trigger.crowd_no = CrowdNo;
                        trigger.error_msg = res.ErrorMsg;
                        trigger.merchant_logo = res.MerchantLogo;
                        trigger.out_biz_no = res.OutBizNo;
                        trigger.partner_id = res.PartnerId;
                        trigger.pay_time = DateTime.Now;
                        trigger.prize_amount = Convert.ToDecimal(res.PrizeAmount);
                        trigger.prize_msg = res.PrizeMsg;
                        trigger.repeat_trigger_flag = res.RepeatTriggerFlag;
                        trigger.trigger_result = res.TriggerResult;
                        trigger.user_id = UserId;
                        var trigger_id = _tb_RedPacket_TriggerService.Insert(trigger);
                        string msg = "";
                        if (!string.IsNullOrEmpty(res.PrizeAmount))
                        {
                            msg = "现金红包" + res.PrizeAmount + "元已领取成功，并已放入您的支付宝余额，请关注查收!";
                        }
                        else
                        {
                            msg = "您已经领取过了现金红包!";
                        }
                        return Json(new
                        {
                            code = "success",
                            percent = 100,
                            userId = UserId,
                            crowd_no = CrowdNo,
                            totle_count = totle_count,
                            xiyun_count = xiyun_count,
                            state = 1,
                            msg = msg
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            code = res.Code,
                            percent = xiyun_count / totle_count * 100,
                            userId = UserId,
                            crowd_no = CrowdNo,
                            totle_count = totle_count,
                            xiyun_count = xiyun_count,
                            state = 1,
                            msg = "红包已派发完"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        code = "sorry",
                        percent = xiyun_count / totle_count * 100,
                        userId = UserId,
                        crowd_no = CrowdNo,
                        totle_count = totle_count,
                        xiyun_count = xiyun_count,
                        state = 0,
                        msg = "您的交易笔数还未达到，不能领取红包!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    errmsg = ex,
                    msg = "/m/xchl/red_activity.html"
                });
            }
        }
        /// <summary>
        /// 领取红包调用接口2(根据mid选择红包领取)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TriggerRedPacketInfo2([FromBody]JObject obj)
        {
            try
            {
                string UserId = obj["UserId"].ToString();
                string CrowdNo = obj["CrowdNo"].ToString();
                List<string> CrowdNoList = new List<string>();
                CrowdNoList.Add("zNAuo5epgDnGzXom0Tny0L2smaDXBAmdgzhPV-jU_Omsn4ge4wBRFkiLOjq1h1Jd");//云纵
                CrowdNoList.Add("527h4sDPEN52JUwazKvzV26V_xlJJq_VhwsQDt7fkCOsn4ge4wBRFkiLOjq1h1Jd");//若火订单号
                DateTime day = DateTime.Now;
                //获取红包项目信息
                var redPacketInfo = _tb_RedPacketService.FindByClause(x => x.start_time <= day && x.end_time >= day && x.crowd_no == CrowdNoList[0]);
                //var redPacketInfo2 = _tb_RedPacketService.FindByClause(x => x.start_time <= day && x.end_time >= day && x.crowd_no == CrowdNoList[1]);
                AlipayMarketingCampaignCashDetailQueryModel mmm = new AlipayMarketingCampaignCashDetailQueryModel()
                {
                    CrowdNo = redPacketInfo.crowd_no
                };
                AlipayMarketingCampaignCashDetailQueryModel mmm2 = new AlipayMarketingCampaignCashDetailQueryModel()
                {
                    CrowdNo = CrowdNoList[1]
                };
                var dqr = aliRed.DetailQueryRedPack(mmm);
                var dqr2 = aliRed2.DetailQueryRedPack(mmm2);
                int totle_count = 0;
                int totle_count2 = 0;
                //查询红包当前状态
                if (dqr.CampStatus == "READY" || dqr2.CampStatus == "READY")
                {
                    //CrowdNo = redPacketInfo.crowd_no;
                    totle_count = Convert.ToInt32(redPacketInfo.paycount);//当前 红包活动领取红包资格数
                    totle_count2 = Convert.ToInt32(redPacketInfo.paycount);//当前 红包活动领取红包资格数
                }
                if (totle_count == 0 && totle_count2 == 0)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = "/m/xchl/red_activity.html"
                    });
                }

                var midlist = _tb_RedPacket_MIDService.FindListByClause(x => x.redpacket_id == redPacketInfo.id, x => x.id, SqlSugar.OrderByType.Asc) as List<tb_redpacket_mid>;
                var midlist2 = _tb_school_InfoService.FindByClause(x => x.School_Code == "8888");
                string xiyunMCode = "";
                string xiyunMCode2 = midlist2.xiyunMCode.Replace(",", "','");
                for (int i = 0; i < midlist.Count; i++)
                {
                    xiyunMCode += midlist[i].mid + ",";
                }
                xiyunMCode = xiyunMCode.Substring(0, xiyunMCode.Length - 1);
                xiyunMCode = xiyunMCode.Replace(",", "','");
                //查询当前支付次数    发布时要加上-------------------------
                int xiyun_count = _tb_xiyun_notifyService.CountToUserid(UserId, xiyunMCode);//用户当前支付次数
                int xiyun_count2 = _tb_xiyun_notifyService.CountToUserid(UserId, xiyunMCode2);//用户当前支付次数
                int xiyun_count_new = 0;
                if (xiyun_count != 0)
                    xiyun_count_new = xiyun_count;
                else
                    xiyun_count_new = xiyun_count2;



                //int xiyun_count = 1;
                if (xiyun_count >= totle_count || xiyun_count2 >= totle_count2)
                {
                    string outbizno = DateTime.Now.ToString("yyyyMMddHHmmssffff") + new Random().Next(1000).ToString().PadLeft(4, '0');
                    AlipayMarketingCampaignCashTriggerModel mm = new AlipayMarketingCampaignCashTriggerModel();
                    AlipayMarketingCampaignCashTriggerResponse res = new AlipayMarketingCampaignCashTriggerResponse();

                    if (xiyun_count != 0)
                    {
                        CrowdNo = CrowdNoList[0];
                        mm.OutBizNo = outbizno;
                        mm.CrowdNo = CrowdNoList[0];
                        mm.UserId = obj["UserId"].ToString();//"2088802186716632"
                        res = aliRed.TriggerRedPack(mm);
                    }
                    else
                    {
                        CrowdNo = CrowdNoList[1];
                        mm.OutBizNo = outbizno;
                        mm.CrowdNo = CrowdNoList[1];
                        mm.UserId = obj["UserId"].ToString();//"2088802186716632"
                        res = aliRed2.TriggerRedPack(mm);
                    }

                    if (res.Code == "10000")
                    {

                        tb_redpacket_trigger trigger = new tb_redpacket_trigger();
                        trigger.biz_no = res.BizNo;
                        trigger.coupon_name = res.CouponName;
                        trigger.crowd_no = CrowdNo;
                        trigger.error_msg = res.ErrorMsg;
                        trigger.merchant_logo = res.MerchantLogo;
                        trigger.out_biz_no = res.OutBizNo;
                        trigger.partner_id = res.PartnerId;
                        trigger.pay_time = DateTime.Now;
                        trigger.prize_amount = Convert.ToDecimal(res.PrizeAmount);
                        trigger.prize_msg = res.PrizeMsg;
                        trigger.repeat_trigger_flag = res.RepeatTriggerFlag;
                        trigger.trigger_result = res.TriggerResult;
                        trigger.user_id = UserId;
                        var trigger_id = _tb_RedPacket_TriggerService.Insert(trigger);
                        string msg = "";
                        if (!string.IsNullOrEmpty(res.PrizeAmount))
                        {
                            msg = "现金红包" + res.PrizeAmount + "元已领取成功，并已放入您的支付宝余额，请关注查收!";
                        }
                        else
                        {
                            msg = "您已经领取过了现金红包!";
                        }
                        return Json(new
                        {
                            code = "success",
                            percent = 100,
                            userId = UserId,
                            crowd_no = CrowdNo,
                            totle_count = totle_count,
                            xiyun_count = xiyun_count_new,
                            state = 1,
                            msg = msg
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            code = res.Code,
                            percent = xiyun_count_new / totle_count * 100,
                            userId = UserId,
                            crowd_no = CrowdNo,
                            totle_count = totle_count,
                            xiyun_count = xiyun_count_new,
                            state = 1,
                            msg = "红包已派发完"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        code = "sorry",
                        percent = xiyun_count_new / totle_count * 100,
                        userId = UserId,
                        crowd_no = CrowdNo,
                        totle_count = totle_count,
                        xiyun_count = xiyun_count_new,
                        state = 0,
                        msg = "您的交易笔数还未达到，不能领取红包!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    errmsg = ex,
                    msg = "/m/xchl/red_activity.html"
                });
            }
        }
        /// <summary>
        /// 修改红包活动状态(通过按钮点击修改状态)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StatusModifyToRedPacket([FromBody]JObject obj)
        {

            try
            {
                string status = "";
                string CampStatus = obj["CampStatus"].ToString();
                if (CampStatus == "1")
                {
                    status = "READY";
                }
                if (CampStatus == "2")
                {
                    status = "PAUSE";
                }
                if (CampStatus == "3")
                {
                    status = "CLOSED";
                }
                AlipayMarketingCampaignCashStatusModifyModel m = new AlipayMarketingCampaignCashStatusModifyModel()
                {
                    CrowdNo = obj["crowd_no"].ToString(),
                    CampStatus = status
                };

                var r = aliRed.StatusModifyRedPack(m);
                if (r.Code == "10000")
                {
                    var redpacketinfo = _tb_RedPacketService.FindByClause(x => x.crowd_no == obj["crowd_no"].ToString());
                    redpacketinfo.camp_status = status;
                    _tb_RedPacketService.Update(redpacketinfo);
                    return Json(new
                    {
                        code = 10000,
                        msg = JsonReturnMsg.UpdateSuccess
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = "40004",
                        msg = r.SubMsg
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.UpdateFail,
                    errmsg = ex,
                });
            }
        }
        /// <summary>
        /// 查询红包活动状态（并修改状态）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DetailQueryToRedPacket([FromBody]JObject obj)
        {

            try
            {
                AlipayMarketingCampaignCashDetailQueryModel mmm = new AlipayMarketingCampaignCashDetailQueryModel()
                {
                    CrowdNo = obj["crowd_no"].ToString()
                };
                var r = aliRed.DetailQueryRedPack(mmm);
                if (r.Code == "40004")
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.UpdateFail,
                        errmsg = r.SubMsg,
                    });
                }
                string status = "";
                if (r.CampStatus == "CREATED")
                {
                    status = "已创建";
                }
                else if (r.CampStatus == "PAID")
                {
                    status = "已打款";
                }
                else if (r.CampStatus == "SETTLED")
                {
                    status = "已清算";
                }
                else if (r.CampStatus == "READY")
                {
                    status = "已开启";
                }
                else if (r.CampStatus == "PAUSE")
                {
                    status = "已暂停";
                }
                else if (r.CampStatus == "CLOSED")
                {
                    status = "已关闭";
                }
                else
                {
                    status = r.CampStatus;
                }
                var redpacket = _tb_RedPacketService.FindByClause(x => x.crowd_no == obj["crowd_no"].ToString());
                redpacket.camp_status = status;
                _tb_RedPacketService.Update(redpacket);
                return Json(new
                {
                    code = "10000",
                    msg = "查询成功",
                    camp_status = status,
                    crowd_no = redpacket.crowd_no,
                    redpacketInfo = redpacket
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.UpdateFail,
                    errmsg = ex,
                });
            }
        }
        /// <summary>
        /// 查询红包活动状态（支付宝接口返回）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> DetailQueryToRedPacket2Async([FromBody]JObject obj)
        {

            try
            {
                AlipayMarketingCampaignCashDetailQueryModel mmm = new AlipayMarketingCampaignCashDetailQueryModel()
                {
                    CrowdNo = obj["crowd_no"].ToString()
                };
                var r = await aliRed.DetailQueryRedPackAsync(mmm);

                return Json(new
                {
                    code = "10000",
                    msg = "查询成功",
                    data = r,
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.UpdateFail,
                    errmsg = ex,
                });
            }
        }

        /// <summary>
        /// 查看选择的mid和未选择的mid
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="redpacketid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult getxiyunMidToschoolcode(string schoolcode, int redpacketid)
        {
            try
            {
                var schoolinfo = _tb_school_InfoService.FindByClause(x => x.School_Code == schoolcode);
                var sp = schoolinfo.xiyunMCode.Split(',').ToList();

                var midinfo = _tb_RedPacket_MIDService.FindListByClause(x => x.redpacket_id == redpacketid, x => x.id, SqlSugar.OrderByType.Asc);
                List<string> select = new List<string>();
                if (midinfo.Count() >= 0)
                {
                    foreach (var item in midinfo)
                    {
                        if (sp.Exists(p => p == item.mid))
                        {
                            sp.Remove(item.mid);
                        }
                    }
                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        data = sp,
                        select = midinfo
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        data = sp,
                        select = "",
                    });
                }

            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="authCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetaliuserInfo(string authCode)
        {
            try
            {
                var schoolinfo = _tb_school_InfoService.FindByClause(x => x.School_Code == "8888");
                var userinfo = aliRed.GetaliuserInfo(authCode, schoolinfo.app_id, schoolinfo.private_key, schoolinfo.alipay_public_key);
                return new JsonResult(new
                {
                    userinfo = userinfo
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }

        /// <summary>
        /// 获取所有mid
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllMid()
        {
            try
            {
                var data = _tb_RedPacket_AllMidService.FindByClause(x => x.id == 1);
                var list = data.mids.Split(',');
                return new JsonResult(new
                {
                    userinfo = data,
                    count = list.Length
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }

        /// <summary>
        /// 设置红包Mid
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetRadMID([FromBody]JObject obj)
        {
            try
            {
                string[] sp = obj["mids"].ToString().Split(',');
                List<tb_redpacket_mid> list = new List<tb_redpacket_mid>();
                foreach (var item in sp)
                {
                    tb_redpacket_mid model = new tb_redpacket_mid();
                    model.redpacket_id = Convert.ToInt32(obj["redpacket_id"]);
                    model.mid = item;
                    list.Add(model);
                }
                _tb_RedPacket_MIDService.Insert(list);
                var data = _tb_RedPacket_AllMidService.FindByClause(x => x.id == 1);
                return new JsonResult(new
                {
                    userinfo = data,
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }
    }
}