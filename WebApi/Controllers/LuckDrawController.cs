using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alipay.AopSdk.Core.Domain;
using DbModel;
using Exceptionless.Json.Linq;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using SchoolWebApi.Utility;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 抽奖活动接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class LuckDrawController : Controller
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static Itb_LuckDrawService _tb_LuckDrawService;
        private static Itb_LuckDraw_PrizeService _tb_LuckDraw_PrizeService;
        private static Itb_LuckDraw_ExtensionUserService _tb_LuckDraw_ExtensionUserService;
        private static Itb_LuckDraw_ExtensionUser_itemService _tb_LuckDraw_ExtensionUser_itemService;
        private static Itb_LuckDraw_cityService _tb_LuckDraw_cityService;
        private static Itb_LuckDraw_itemService _tb_LuckDraw_itemService;
        private static Itb_RedPacket_MIDService _tb_RedPacket_MIDService;
        private static Itb_xiyun_notifyService _tb_xiyun_notifyService;
        private static Itb_RedPacketService _tb_RedPacketService;
        private static AliRedPacketHelper aliRed;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_LuckDrawService"></param>
        /// <param name="tb_LuckDraw_ExtensionUserService"></param>
        /// <param name="tb_LuckDraw_ExtensionUser_itemService"></param>
        /// <param name="tb_LuckDraw_cityService"></param>
        /// <param name="tb_LuckDraw_PrizeService"></param>
        /// <param name="tb_LuckDraw_itemService"></param>
        /// <param name="tb_RedPacket_MIDService"></param>
        /// <param name="tb_xiyun_notifyService"></param>
        /// <param name="tb_RedPacketService"></param>
        public LuckDrawController(Itb_LuckDrawService tb_LuckDrawService,
            Itb_LuckDraw_ExtensionUserService tb_LuckDraw_ExtensionUserService,
            Itb_LuckDraw_ExtensionUser_itemService tb_LuckDraw_ExtensionUser_itemService,
            Itb_LuckDraw_cityService tb_LuckDraw_cityService,
            Itb_LuckDraw_PrizeService tb_LuckDraw_PrizeService,
            Itb_LuckDraw_itemService tb_LuckDraw_itemService,
            Itb_RedPacket_MIDService tb_RedPacket_MIDService,
            Itb_xiyun_notifyService tb_xiyun_notifyService,
            Itb_RedPacketService tb_RedPacketService)
        {
            _tb_LuckDrawService = tb_LuckDrawService;
            _tb_LuckDraw_ExtensionUserService = tb_LuckDraw_ExtensionUserService;
            _tb_LuckDraw_ExtensionUser_itemService = tb_LuckDraw_ExtensionUser_itemService;
            _tb_LuckDraw_cityService = tb_LuckDraw_cityService;
            _tb_LuckDraw_PrizeService = tb_LuckDraw_PrizeService;
            _tb_LuckDraw_itemService = tb_LuckDraw_itemService;
            _tb_RedPacket_MIDService = tb_RedPacket_MIDService;
            _tb_xiyun_notifyService = tb_xiyun_notifyService;
            _tb_RedPacketService = tb_RedPacketService;
            aliRed = new AliRedPacketHelper("RedPacketsUserInfo2");
        }
        /// <summary>
        /// 【抽奖】添加城市aliuserid
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddLuckDrawCityUser([FromBody] tb_luckdraw_city obj)
        {
            try
            {
                bool isOk = _tb_LuckDraw_ExtensionUserService.Any(x => x.ali_user_id == obj.ali_user_id);
                if (!isOk)
                {
                    _tb_LuckDraw_cityService.Insert(obj);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
                });
            }
            catch (Exception e)
            {
                log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }
        /// <summary>
        /// 【抽奖活动】添加推广员（头头）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddLuckDrawExtensionUser([FromBody] tb_luckdraw_extensionuser obj)
        {
            try
            {
                bool isOk = _tb_LuckDraw_ExtensionUserService.Any(x => x.ali_user_id == obj.ali_user_id && x.luckDraw_id == obj.luckDraw_id);
                if (!isOk)
                {
                    _tb_LuckDraw_ExtensionUserService.Insert(obj);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
                });
            }
            catch (Exception e)
            {
                log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }
        /// <summary>
        /// 【抽奖】给推广员添加下线
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddLuckDrawExtensionItemUser([FromBody] JObject obj)
        {
            try
            {
                int luckDraw_id = Convert.ToInt32(obj["luckDraw_id"]);
                string aliuserid = obj["ali_user_id"].ToString();
                string aliuseridroot = obj["aliuseridroot"].ToString();
                var data = _tb_LuckDraw_ExtensionUserService.FindByClause(x => x.ali_user_id == aliuseridroot && x.luckDraw_id == luckDraw_id);
                bool isOK = _tb_LuckDraw_ExtensionUser_itemService.Any(x => x.ali_user_id == aliuserid && x.extensionUserid == data.id);
                if (!isOK)
                {
                    tb_luckdraw_extensionuser_item model = new tb_luckdraw_extensionuser_item();
                    model.ali_user_id = aliuserid;
                    model.extensionUserid = data.id;
                    _tb_LuckDraw_ExtensionUser_itemService.Insert(model);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
                });
            }
            catch (Exception e)
            {
                log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }

        /// <summary>
        /// 【抽奖】添加抽奖活动
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddLuckDraw([FromBody] tb_luckdraw obj)
        {
            try
            {
                _tb_LuckDrawService.Insert(obj);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
                });
            }
            catch (Exception e)
            {
                log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }

        /// <summary>
        /// 【抽奖】添加抽奖奖品
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddLuckDrawPrize([FromBody] tb_luckdraw_prize obj)
        {
            try
            {
                var prizeid = _tb_LuckDraw_PrizeService.Insert(obj);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
                });
            }
            catch (Exception e)
            {
                log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }
        /// <summary>
        /// 【抽奖】添加抽奖流水
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddLuckDraw_item([FromBody] tb_luckdraw_item obj)
        {
            try
            {
                var Prize = _tb_LuckDraw_PrizeService.FindByClause(x => x.id == obj.prizeid);
                if (Prize.prizeTypes==3)
                {
                    obj.isConvert = false;
                }
                else
                {
                    obj.isConvert = true;
                }
                _tb_LuckDraw_itemService.Insert(obj);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
                });
            }
            catch (Exception e)
            {
                log.Error(e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }
        /// <summary>
        /// 【抽奖】查看抽奖活动通过id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetLuckDrawInfo(int id)
        {
            try
            {
                
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = _tb_LuckDrawService.FindByClause(x=>x.id==id)
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
        /// 【抽奖】查看抽奖明细（单个人）
        /// </summary>
        /// <param name="luckDraw_id"></param>
        /// <param name="ali_user_id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetLuckDraw_itemToaliuserid(int luckDraw_id, string ali_user_id)
        {
            try
            {
                var dataitem = _tb_LuckDraw_itemService.FindListByClause(x => x.tb_LuckDraw_id == luckDraw_id && x.ali_user_id == ali_user_id, t => t.id, SqlSugar.OrderByType.Asc);
                List<LuckDraw_PrizeModel> list = new List<LuckDraw_PrizeModel>();
                var prize = _tb_LuckDraw_PrizeService.FindListByClause(x => x.tb_LuckDraw_id == luckDraw_id, t => t.id, SqlSugar.OrderByType.Asc);
                foreach (var item in dataitem)
                {
                    LuckDraw_PrizeModel prize1 = new LuckDraw_PrizeModel();
                    tb_luckdraw_prize model = new tb_luckdraw_prize();
                    model = prize.Where(x => x.id == item.prizeid).ToList()[0];
                    prize1.isConvert = Convert.ToBoolean(item.isConvert);
                    prize1.Prize = model;
                    prize1.Prizeid = item.id.ToString();
                    list.Add(prize1);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    prizeList = list
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
        /// 【抽奖】修改领取物品状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LuckDraw_itemisConvert([FromBody] JObject obj)
        {
            try
            {
                int itemid = Convert.ToInt32(obj["itemid"]);
                var model = _tb_LuckDraw_itemService.FindByClause(x => x.id == itemid);
                model.isConvert = true;
                _tb_LuckDraw_itemService.Update(model);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
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
        /// 【抽奖】查看抽奖活动流水
        /// </summary>
        /// <param name="luckDrawid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetLuckDraw_item(string luckDrawid)
        {
            try
            {
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = _tb_LuckDraw_itemService.FindListByClause(x => x.tb_LuckDraw_id == Convert.ToInt32(luckDrawid), t => t.id, SqlSugar.OrderByType.Asc)
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
        /// 【抽奖】给出活动奖品组，抽奖次数
        /// </summary>
        /// <param name="luckDrawid"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LuckDrawInit(int luckDrawid,string UserId)
        {
            try
            {
                int count = 0;
                var luckdata = _tb_LuckDrawService.FindByClause(x => x.id == luckDrawid);
                var redno = _tb_RedPacketService.FindByClause(x => x.id == luckdata.tb_RedPacket_id);
                count = Convert.ToInt32(luckdata.luckDrawCount);
                var data = _tb_LuckDraw_PrizeService.FindListByClause(x => x.tb_LuckDraw_id == luckDrawid && x.isDel == false,t=>t.id,SqlSugar.OrderByType.Asc) as List<tb_luckdraw_prize>;
                var items = data.Select(x => x.prizeName).ToList();
                //判断消费次数是否能够抽奖
                var midlist = _tb_RedPacket_MIDService.FindListByClause(x => x.redpacket_id == luckdata.tb_RedPacket_id, x => x.id, SqlSugar.OrderByType.Asc) as List<tb_redpacket_mid>;
                string xiyunMCode = "";
                for (int i = 0; i < midlist.Count; i++)
                {
                    xiyunMCode += midlist[i].mid + ",";
                }
                xiyunMCode = xiyunMCode.Substring(0, xiyunMCode.Length - 1);
                xiyunMCode = xiyunMCode.Replace(",", "','");
                //查询当前支付次数    发布时要加上-------------------------
                int xiyun_count = _tb_xiyun_notifyService.CountToUserid2(UserId, xiyunMCode);//用户当前支付次数
                if (xiyun_count >= luckdata.consumptionCount)
                {
                    string dtime = DateTime.Now.ToString("yyyy-MM-dd");
                    DateTime stime = Convert.ToDateTime(dtime + " 00:00:00");
                    DateTime etime = Convert.ToDateTime(dtime + " 23:59:59");
                    var luckDrawitems = _tb_LuckDraw_itemService.FindListByClause(x => x.ali_user_id == UserId && x.pay_time >= stime && x.pay_time <= etime, t => t.pay_time, SqlSugar.OrderByType.Asc) as List<tb_luckdraw_item>;
                    if (xiyun_count>= luckdata.luckDrawCount)
                    {
                        count = Convert.ToInt32(luckdata.luckDrawCount);
                        count -= luckDrawitems.Count;
                    }
                    else
                    {
                        count = xiyun_count;
                        count -= luckDrawitems.Count;
                    }


                    //if (xiyun_count >= luckDrawitems.Count)
                    //{
                    //    //if (luckDrawitems != null)
                    //    count = xiyun_count - luckDrawitems.Count;
                    //    count = count >= luckdata.luckDrawCount ? Convert.ToInt32(luckdata.luckDrawCount) : count;
                    //    //log.Info("xiyun_count="+ xiyun_count + "luckDrawitems=" +luckDrawitems.Count);
                    //}
                    //else
                    //{
                    //    count = -1;
                    //}
                   
                }
                else
                {
                    count = -1;
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    PrizeItem = items,
                    count = count,
                    prizeInfo = data,
                    CrowdNo = redno.crowd_no
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
        /// 【抽奖】抽出奖品
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="luckDrawid"></param>
        /// <param name="isSW"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LuckDrawStart(string UserId, int luckDrawid,int isSW)
        {
            try
            {
                int count = 0;
                var luckdata = _tb_LuckDrawService.FindByClause(x => x.id == luckDrawid);
                //判断消费次数是否能够抽奖
                var midlist = _tb_RedPacket_MIDService.FindListByClause(x => x.redpacket_id == luckdata.tb_RedPacket_id, x => x.id, SqlSugar.OrderByType.Asc) as List<tb_redpacket_mid>;
                string xiyunMCode = "";
                for (int i = 0; i < midlist.Count; i++)
                {
                    xiyunMCode += midlist[i].mid + ",";
                }
                xiyunMCode = xiyunMCode.Substring(0, xiyunMCode.Length - 1);
                xiyunMCode = xiyunMCode.Replace(",", "','");
                //查询当前支付次数    发布时要加上-------------------------
                int xiyun_count = _tb_xiyun_notifyService.CountToUserid(UserId, xiyunMCode);//用户当前支付次数
                int redCount = 0;
                string dtime = DateTime.Now.ToString("yyyy-MM-dd");
                DateTime stime = Convert.ToDateTime(dtime + " 00:00:00");
                DateTime etime = Convert.ToDateTime(dtime + " 23:59:59");
                var luckDrawitems = _tb_LuckDraw_itemService.FindListByClause(x => x.ali_user_id == UserId && x.pay_time >= stime && x.pay_time <= etime, t => t.pay_time, SqlSugar.OrderByType.Asc) as List<tb_luckdraw_item>;
                if (xiyun_count >= luckdata.consumptionCount)
                {
                    count = Convert.ToInt32(luckdata.luckDrawCount);
                    redCount = luckDrawitems.Count;
                    if (xiyun_count >= luckdata.luckDrawCount)
                    {
                        count = Convert.ToInt32(luckdata.luckDrawCount);
                        count -= luckDrawitems.Count;
                    }
                    else
                    {
                        count = xiyun_count;
                        count -= luckDrawitems.Count;
                    }
                    //if (xiyun_count >= luckDrawitems.Count)
                    //{
                    //    count = Convert.ToInt32(luckdata.luckDrawCount);
                    //    if (luckDrawitems != null)
                    //    {
                    //        count = xiyun_count - luckDrawitems.Count;
                    //        redCount = luckDrawitems.Count;
                    //    }
                    //    count = count > luckdata.luckDrawCount ? Convert.ToInt32(luckdata.luckDrawCount) : count;
                    //}
                    //else
                    //{
                    //    count = -1;
                    //}

                }
                else
                {
                    count = -1;
                }
                //判断抽奖次数是否达到上限

                var data = _tb_LuckDraw_PrizeService.FindListByClause(x => x.tb_LuckDraw_id == luckDrawid && x.isDel == false, t => t.id, SqlSugar.OrderByType.Asc) as List<tb_luckdraw_prize>;
                var items = data.Select(x => x.prizeName).ToList();
                var redinfo = _tb_RedPacketService.FindByClause(x => x.id == luckdata.tb_RedPacket_id);
                int getPrizeIndex = 0;
                Random rd = new Random();
                int num = rd.Next(0, 100);
                if (isSW!=0)//有实物为0.不然没实物
                {
                    items.RemoveAt(items.Count - 1);
                }
                if (num >= 90)
                {
                    getPrizeIndex = rd.Next(2, items.Count);
                    try
                    {
                        string CrowdNo = redinfo.crowd_no;
                        string NBUserId = "";
                        string outbizno = DateTime.Now.ToString("yyyyMMddHHmmssffff") + new Random().Next(1000).ToString().PadLeft(4, '0');
                        AlipayMarketingCampaignCashTriggerModel mm = new AlipayMarketingCampaignCashTriggerModel()
                        {
                            OutBizNo = outbizno,
                            CrowdNo = CrowdNo,
                            UserId = NBUserId
                        };
                        var res = aliRed.TriggerRedPack(mm);
                    }
                    catch (Exception ex)
                    {

                        log.Error(ex);
                    }
                }
                else
                {
                    int total_num = Convert.ToInt32(redinfo.total_num);
                    var data2 = data.Where(x => x.prizeTypes == 1).ToList();
                    int total = luckDrawitems.Where(x => x.prizeid == data2[0].id).ToList().Count;
                    if (total >= total_num)
                    {
                        getPrizeIndex = rd.Next(2, items.Count);
                    }
                    else
                    {
                        getPrizeIndex = 1;
                    }
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    count = count,
                    getPrizeIndex = getPrizeIndex,
                    prizeServiceItems = data
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
        /// 【抽奖】统计下线人数，消费笔数
        /// </summary>
        /// <param name="luckDraw_id"></param>
        /// <param name="ali_user_id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStatistics(int luckDraw_id,string ali_user_id)
        {
            try
            {
                var cityuser = _tb_LuckDraw_cityService.FindByClause(x => x.ali_user_id == ali_user_id);
                if (cityuser!=null)
                {
                    var a = GetLuckDrawTimes(ali_user_id, luckDraw_id);

                    return Json(new
                    {
                        JStype = 0,
                        code = JsonReturnMsg.SuccessCode,
                        extensionUserCount = a.Item2,
                        count = a.Item1
                    });
                }

                var extensionUser = _tb_LuckDraw_ExtensionUserService.FindByClause(x => x.luckDraw_id == luckDraw_id && x.ali_user_id == ali_user_id);
                var extensionUser_item = _tb_LuckDraw_ExtensionUser_itemService.FindListByClause(x => x.extensionUserid == extensionUser.id, t => t.id, SqlSugar.OrderByType.Asc) as List<tb_luckdraw_extensionuser_item>;
                int luckdrawitem = 0;
                foreach (var item2 in extensionUser_item)
                {
                    luckdrawitem += _tb_LuckDraw_itemService.Count(x => x.ali_user_id == item2.ali_user_id && x.tb_LuckDraw_id == luckDraw_id);
                }


                //var luckdraw = _tb_LuckDrawService.FindByClause(x => x.id == luckDraw_id);
                //var extensionUser = _tb_LuckDraw_ExtensionUserService.FindByClause(x => x.ali_user_id == ali_user_id && x.luckDraw_id == luckDraw_id);
                //var extensionUser_item = _tb_LuckDraw_ExtensionUser_itemService.FindListByClause(x => x.extensionUserid == extensionUser.id, t => t.id, SqlSugar.OrderByType.Asc) as List<tb_luckdraw_extensionuser_item>;
                //var midlist = _tb_RedPacket_MIDService.FindListByClause(x => x.redpacket_id == luckdraw.tb_RedPacket_id, x => x.id, SqlSugar.OrderByType.Asc) as List<tb_redpacket_mid>;
                //string xiyunMCode = "";
                //for (int i = 0; i < midlist.Count; i++)
                //{
                //    xiyunMCode += midlist[i].mid + ",";
                //}
                //xiyunMCode = xiyunMCode.Substring(0, xiyunMCode.Length - 1);
                //xiyunMCode = xiyunMCode.Replace(",", "','");
                ////查询当前支付次数    发布时要加上-------------------------
                //int xiyun_count = 0;
                //foreach (var item in extensionUser_item)
                //{
                //    xiyun_count += _tb_xiyun_notifyService.CountToUserid(item.ali_user_id, xiyunMCode);//用户当前支付次数
                //}

                return Json(new
                {
                    JStype = 1,
                    code = JsonReturnMsg.SuccessCode,
                    extensionUser_itemcount = extensionUser_item.Count,
                    count = luckdrawitem
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
        /// 【抽奖】统计下线人数，消费笔数
        /// </summary>
        /// <param name="luckDraw_id"></param>
        /// <param name="ali_user_id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStatistics2(int luckDraw_id, string ali_user_id)
        {
            try
            {
                var a = GetLuckDrawTimes(ali_user_id, luckDraw_id);
                
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    extensionUserCount = a.Item2,
                    count = a.Item1
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
       
        private static (int,List<LuckDrawCount>) GetLuckDrawTimes(string aliuserid,int tb_LuckDraw_id)
        {
            List<LuckDrawCount> list = new List<LuckDrawCount>();
            int count = 0;
            var cityuser = _tb_LuckDraw_cityService.FindByClause(x => x.ali_user_id == aliuserid);
            var extensionUser = _tb_LuckDraw_ExtensionUserService.FindListByClause(x => x.luckDraw_id == tb_LuckDraw_id&&x.luckDraw_city_id== cityuser.id, t => t.id, SqlSugar.OrderByType.Asc);
            foreach (var item in extensionUser)
            {
                LuckDrawCount tot = new LuckDrawCount();
                var extensionUser_item = _tb_LuckDraw_ExtensionUser_itemService.FindListByClause(x => x.extensionUserid == item.id, t => t.id, SqlSugar.OrderByType.Asc);
                tot.aliuserid = item.ali_user_id;
                int luckdrawitem = 0;
                foreach (var item2 in extensionUser_item)
                {
                    luckdrawitem +=  _tb_LuckDraw_itemService.Count(x => x.ali_user_id == item2.ali_user_id && x.tb_LuckDraw_id == tb_LuckDraw_id);
                }
                tot.count = luckdrawitem;
                count += luckdrawitem;
                list.Add(tot);
            }
            return (count,list);
        }
       
    }
}