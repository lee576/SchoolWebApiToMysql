﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SchoolWebApi.Utility;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 电表管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class ElectricitybillsController : Controller
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private Itb_ammeterService _tb_ammeterService;
        private Itb_Building_Room_ConfigService _tb_Building_Room_ConfigService;
        private Itb_payment_electricitybillsService _tb_payment_electricitybillsService;
        private Itb_school_userService _tb_school_userService;
        private Itb_payment_accountsService _tb_payment_accountsService;
        private Itb_appaccounts_itemService _tb_appaccounts_itemService;
        private Itb_School_User_RoomService _tb_School_User_RoomService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_ammeterService"></param>
        /// <param name="tb_Building_Room_ConfigService"></param>
        /// <param name="tb_payment_electricitybillsService"></param>
        /// <param name="tb_school_userService"></param>
        /// <param name="tb_payment_accountsService"></param>
        /// <param name="tb_appaccounts_itemService"></param>
        /// <param name="tb_School_User_RoomService"></param>
        public ElectricitybillsController(Itb_ammeterService tb_ammeterService,
            Itb_Building_Room_ConfigService tb_Building_Room_ConfigService,
            Itb_payment_electricitybillsService tb_payment_electricitybillsService,
            Itb_school_userService tb_school_userService,
            Itb_payment_accountsService tb_payment_accountsService,
            Itb_appaccounts_itemService tb_appaccounts_itemService,
            Itb_School_User_RoomService tb_School_User_RoomService
            )
        {
            _tb_ammeterService = tb_ammeterService;
            _tb_Building_Room_ConfigService = tb_Building_Room_ConfigService;
            _tb_payment_electricitybillsService = tb_payment_electricitybillsService;
            _tb_school_userService = tb_school_userService;
            _tb_payment_accountsService = tb_payment_accountsService;
            _tb_appaccounts_itemService = tb_appaccounts_itemService;
            _tb_School_User_RoomService = tb_School_User_RoomService;
        }
        /// <summary>
        /// 通过宿舍id获取电表信息
        /// </summary>
        /// <param name="roomid">宿舍ID</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAmmeterInfoToRoomID(int roomid)
        {
            try
            {
                var data = _tb_ammeterService.FindByClause(x => x.room_id == roomid);
                if (data == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail
                    });
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = data
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 添加电表房间绑定关系
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAmmeter([FromBody] JObject obj)
        {
            try
            {

                int room_id = Convert.ToInt32(obj["room_id"].ToString());
                string meterAddr = obj["meterAddr"].ToString();
                var tb = _tb_Building_Room_ConfigService.FindByClause(x => x.id == room_id);
                var ammeterdate = _tb_ammeterService.CheckAmmeterInfo(tb.school_id.ToString(), meterAddr).ToList();
                if (ammeterdate.Count != 0)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.AddFail
                    });
                }
                tb_ammeter ammeter = new tb_ammeter();
                ammeter.room_id = room_id;
                ammeter.MeterAddr = meterAddr;
                _tb_ammeterService.Insert(ammeter);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }
        /// <summary>
        /// 修改电表房间绑定关系
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateAmmeter([FromBody] JObject obj)
        {
            try
            {
                int room_id = Convert.ToInt32(obj["room_id"].ToString());
                string meterAddr = obj["meterAddr"].ToString();
                int id = Convert.ToInt32(obj["id"].ToString());
                var tb = _tb_Building_Room_ConfigService.FindByClause(x => x.id == room_id);
                var ammeterdate = _tb_ammeterService.CheckAmmeterInfo(tb.school_id.ToString(), meterAddr).ToList();
                if (ammeterdate.Count != 0)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.UpdateFail
                    });
                }
                tb_ammeter ammeter = new tb_ammeter();
                ammeter.room_id = room_id;
                ammeter.MeterAddr = meterAddr;
                ammeter.id = id;
                _tb_ammeterService.Update(ammeter);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UpdateFail
                });
            }
        }
        /// <summary>
        /// 删除电表房间绑定关系
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAmmeter([FromBody] JObject obj)
        {
            try
            {
                int id = Convert.ToInt32(obj["id"].ToString());
                var tb = _tb_ammeterService.FindByClause(x => x.id == id);
                if (tb == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.DeleteFail
                    });
                }
                _tb_ammeterService.Delete(tb);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }

        /// <summary>
        /// 湖南交通工程学院宿舍信息导入【外部接口导入】【湖南交通工程学院】
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="schoolName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Addtb_Building_Room_Config_School10005(int schoolcode, string schoolName)
        {
            try
            {
                var client = new RestClient("http://59.51.42.150:8003/api/Users/GetUserAll");
                var request = new RestRequest { Method = Method.POST };
                request.AddHeader("Accept", "application/x-www-form-urlencoded");
                request.Parameters.Clear();
                string body = "";//参数
                request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);
                var response = client.Execute(request);
                var content = response.Content; // 返回的网页内容
                var data = JsonConvert.DeserializeObject<Return_RoomInfo>(content);
                //查询根目录是否存在
                bool isOK = _tb_Building_Room_ConfigService.Any(x => x.school_id == schoolcode);
                long tb_Building_Room_Config_ID = 0;
                tb_building_room_config config = new tb_building_room_config();
                long rootparent = 0;
                long jsparent = 0;
                bool jsok = true;
                long mmparent = 0;
                bool mmok = true;

                if (!isOK)
                {
                    //添加根目录
                    config.school_id = schoolcode;
                    config.parent_id = 0;
                    config.ispublic = 0;
                    config.building_room_no = schoolName;
                    tb_Building_Room_Config_ID = _tb_Building_Room_ConfigService.Insert(config);
                    rootparent = tb_Building_Room_Config_ID;
                }
                for (int i = 15; i <= 26; i++)
                {
                    config.school_id = schoolcode;
                    config.parent_id = rootparent;
                    config.ispublic = 0;
                    config.building_room_no = i + "栋";
                    tb_Building_Room_Config_ID = _tb_Building_Room_ConfigService.Insert(config);
                }
                LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(600);
                TaskFactory factory = new TaskFactory(lcts);
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < data.DataList.Count; i++)
                {
                    if (data.DataList[i].Address.Contains("教师"))
                    {
                        if (jsok)
                        {
                            jsok = false;
                            config.school_id = schoolcode;
                            config.parent_id = rootparent;
                            config.ispublic = 0;
                            config.building_room_no = "教师公寓";
                            tb_Building_Room_Config_ID = _tb_Building_Room_ConfigService.Insert(config);
                            jsparent = tb_Building_Room_Config_ID;
                        }
                        config.school_id = schoolcode;
                        config.parent_id = jsparent;
                        config.ispublic = 0;
                        config.building_room_no = data.DataList[i].Address;
                        config.building_room_num = data.DataList[i].UserCode;
                        tb_Building_Room_Config_ID = _tb_Building_Room_ConfigService.Insert(config);
                        //jsparent = tb_Building_Room_Config_ID;
                        GetMeterAddrInfo(data.DataList[i].UserCode, Convert.ToInt32(tb_Building_Room_Config_ID));
                    }
                    else if (data.DataList[i].Address.Contains("门面"))
                    {
                        if (mmok)
                        {
                            mmok = false;
                            config.school_id = schoolcode;
                            config.parent_id = rootparent;
                            config.ispublic = 0;
                            config.building_room_no = "门面";
                            tb_Building_Room_Config_ID = _tb_Building_Room_ConfigService.Insert(config);
                            mmparent = tb_Building_Room_Config_ID;
                        }
                        config.school_id = schoolcode;
                        config.parent_id = mmparent;
                        config.ispublic = 0;
                        config.building_room_no = data.DataList[i].Address;
                        config.building_room_num = data.DataList[i].UserCode;
                        tb_Building_Room_Config_ID = _tb_Building_Room_ConfigService.Insert(config);
                        //GetMeterAddrInfo(data.DataList[i].UserCode, Convert.ToInt32(tb_Building_Room_Config_ID));
                    }
                    else if (!data.DataList[i].Address.Contains("门面") && data.DataList[i].Address.Contains("栋"))
                    {
                        long pare = 0;
                        if (data.DataList[i].Doorplate.Split('-')[0].Equals("15"))
                            pare = rootparent + 1;
                        if (data.DataList[i].Doorplate.Split('-')[0].Equals("16"))
                            pare = rootparent + 2;
                        if (data.DataList[i].Doorplate.Split('-')[0].Equals("17"))
                            pare = rootparent + 3;
                        if (data.DataList[i].Doorplate.Split('-')[0].Equals("18"))
                            pare = rootparent + 4;
                        if (data.DataList[i].Doorplate.Split('-')[0].Equals("19"))
                            pare = rootparent + 5;
                        if (data.DataList[i].Doorplate.Split('-')[0].Equals("20"))
                            pare = rootparent + 6;
                        if (data.DataList[i].Doorplate.Split('-')[0].Equals("21"))
                            pare = rootparent + 7;
                        if (data.DataList[i].Doorplate.Split('-')[0].Equals("22"))
                            pare = rootparent + 8;
                        if (data.DataList[i].Doorplate.Split('-')[0].Equals("23"))
                            pare = rootparent + 9;
                        if (data.DataList[i].Doorplate.Split('-')[0].Equals("24"))
                            pare = rootparent + 10;
                        if (data.DataList[i].Doorplate.Split('-')[0].Equals("25"))
                            pare = rootparent + 11;
                        if (data.DataList[i].Doorplate.Split('-')[0].Equals("26"))
                            pare = rootparent + 12;
                        config.school_id = schoolcode;
                        config.parent_id = pare;
                        config.ispublic = 0;
                        config.building_room_no = data.DataList[i].Address;
                        config.building_room_num = data.DataList[i].UserCode;
                        var dd = _tb_Building_Room_ConfigService.FindByClause(x => x.school_id == schoolcode && x.building_room_num == data.DataList[i].UserCode);
                        if (dd == null)
                        {
                            tasks.Add(factory.StartNew(s =>
                            {
                                try
                                {
                                    tb_Building_Room_Config_ID = _tb_Building_Room_ConfigService.Insert(config);
                                }
                                catch (Exception)
                                {

                                    throw;
                                }

                            }, i));
                        }
                    }
                }

                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 获取电表信息湖南交通工程学院宿舍信息导入【外部接口导入】 【湖南交通工程学院】
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="room_id"></param>
        private void GetMeterAddrInfo(string usercode, int room_id)
        {
            try
            {
                var client = new RestClient("http://59.51.42.150:8003/api/Users/GetUserInfo");
                var request = new RestRequest();
                request.Method = Method.POST;
                request.AddHeader("Accept", "application/x-www-form-urlencoded");
                request.Parameters.Clear();
                string body = "UserCode=" + usercode;//参数
                                                     //request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);
                request.AddJsonBody(new { UserCode = usercode });
                var response = client.Execute(request);
                var content = response.Content; // 返回的网页内容
                var data = JsonConvert.DeserializeObject<Return_MeterAddrInfo>(content);
                string meterAddr = data.DataList.MeterAddr;
                tb_ammeter ammeter = new tb_ammeter();
                ammeter.MeterAddr = meterAddr;
                ammeter.room_id = room_id;
                _tb_ammeterService.Insert(ammeter);

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 添加充值记录【外部接口导入】【湖南交通工程学院】
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddElectricitybillsInfo(int schoolcode)
        {
            try
            {
                var tb = _tb_Building_Room_ConfigService.FindAll().Where(x => x.school_id == schoolcode && x.building_room_num != null);
                if (tb == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        msg = JsonReturnMsg.UpdateSuccess
                    });
                }
                var client = new RestClient("http://59.51.42.150:8003/api/Users/GetPayList");
                var request = new RestRequest();
                LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(600);
                TaskFactory factory = new TaskFactory(lcts);
                List<Task> tasks = new List<Task>();
                foreach (var item in tb)
                {
                    request.Method = Method.POST;
                    request.AddHeader("Accept", "application/x-www-form-urlencoded");
                    request.Parameters.Clear();
                    request.AddJsonBody(new { UserCode = item.building_room_num });
                    var data = JsonConvert.DeserializeObject<GetPayList>(client.Execute(request).Content); // 返回的网页内容 GetPayList
                    tb_payment_electricitybills model = new tb_payment_electricitybills();
                    if (data.DataList.Count > 0)
                    {
                        for (int i = 0; i < data.DataList.Count; i++)
                        {
                            var dd = _tb_payment_electricitybillsService.FindByClause(x => x.ordernumber == data.DataList[i].PayID);
                            if (dd == null)
                            {
                                model.ordernumber = data.DataList[i].PayID;
                                model.pay_amount = data.DataList[i].FareMoney;
                                model.pay_status = true;
                                model.pay_time = data.DataList[i].PayDate;
                                model.room_id = Convert.ToInt32(item.id);
                                model.schoolcode = schoolcode.ToString();
                                tasks.Add(factory.StartNew(s =>
                                {
                                    _tb_payment_electricitybillsService.Insert(model);
                                }, i));

                            }
                        }
                    }
                }
                Task.WaitAll(tasks.ToArray());
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess
                });
            }
            catch (Exception e)
            {
                log.Error("错误:" + e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UpdateFail
                });
            }
        }


        /// <summary>
        /// 添加充值记录【外部接口导入】【湖南交通工程学院】
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult YLGetUserAll(int schoolcode)
        {
            try
            {
                var client = new RestClient("http://59.51.42.150:8003/api/Users/GetUserAll");
                var request = new RestRequest();
                request.Method = Method.POST;
                request.AddHeader("Accept", "application/x-www-form-urlencoded");
                request.Parameters.Clear();
                string body = "";//参数
                request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);
                var response = client.Execute(request);
                var content = response.Content; // 返回的网页内容
                var data = JsonConvert.DeserializeObject<Return_RoomInfo>(content);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
                    data = data
                });
            }
            catch (Exception e)
            {
                log.Error("错误:" + e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UpdateFail
                });
            }
        }

        /// <summary>
        /// 添加电表记录【外部接口导入】【湖南交通工程学院】
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddAmmeter(int schoolcode)
        {
            try
            {
                var tb = _tb_Building_Room_ConfigService.FindAll().Where(x => x.school_id == schoolcode && x.building_room_num != null);
                if (tb == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        msg = JsonReturnMsg.UpdateSuccess
                    });
                }

                LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(600);
                TaskFactory factory = new TaskFactory(lcts);
                List<Task> tasks = new List<Task>();
                List<tb_building_room_config> tt = tb as List<tb_building_room_config>;
                int i = 0;
                foreach (var item in tb)
                {
                    tasks.Add(factory.StartNew(s =>
                    {
                        GetMeterAddrInfo(item.building_room_num, Convert.ToInt32(item.id));
                    }, i));
                    i++;
                }
                Task.WaitAll(tasks.ToArray());
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess
                });
            }
            catch (Exception e)
            {
                log.Error("错误:" + e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UpdateFail
                });
            }
        }
        /// <summary>
        /// 获取楼层目录
        /// </summary>
        /// <param name="schoolCode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetRoomRootInfo(int schoolCode)
        {
            try
            {
                var dt = _tb_Building_Room_ConfigService.FindByClause(x => x.school_id == schoolCode && x.parent_id == 0);
                if (dt == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail
                    });
                }
                var dt2 = _tb_Building_Room_ConfigService.FindAll().Where(x => x.parent_id == dt.id);

                List<object> list = new List<object>();
                foreach (var item in dt2)
                {
                    var data = new { id = item.id, name = item.building_room_no };
                    list.Add(data);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    dormArray = list
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }

        /// <summary>
        /// 获取楼层房间号
        /// </summary>
        /// <param name="schoolCode"></param>
        /// <param name="rootid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetRoomNumInfo(int schoolCode, int rootid)
        {
            try
            {
                var dt = _tb_Building_Room_ConfigService.FindAll().Where(x => x.parent_id == rootid);
                if (dt == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail
                    });
                }
                List<object> list = new List<object>();
                foreach (var item in dt)
                {
                    var data = new { id = item.id, name = item.building_room_no, roomnumber = item.building_room_num };
                    list.Add(data);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    dormArray = list
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 获取支付宝订单支付状态
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="appName"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetOrdersStatus(string out_trade_no, string appName)
        {
            try
            {
                string schoolcode = _tb_payment_electricitybillsService.FindByClause(x => x.ordernumber == out_trade_no).schoolcode;
                var item = _tb_appaccounts_itemService.FindByClause(x => x.schoolcode == schoolcode && x.typename == "电费");
                var account = _tb_payment_accountsService.FindByClause(x => x.id == item.accounts_id);
                string res = AliPayHelper.GetOrdersStatus(out_trade_no, account.appid, account.private_key, account.alipay_public_key);
                return Json(new
                {
                    data = res
                });
            }
            catch (Exception e)
            {
                log.Error("错误:" + e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = "获取支付宝订单状态失败"
                });
            }
        }
        /// <summary>
        /// 自动获取房间信息
        /// </summary>
        /// <param name="ali_user_id"></param>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetRoomInfoToSchoolcodeAndali_user_id(string ali_user_id, string schoolcode)
        {
            try
            {
                var studentinfo = _tb_school_userService.FindByClause(x => x.school_id == schoolcode && x.ali_user_id == ali_user_id);

                var roominfo2 = _tb_School_User_RoomService.FindByClause(x => x.school_code.ToString() == schoolcode && Convert.ToInt32(x.user_id) == studentinfo.user_id);

                var roominfo = _tb_Building_Room_ConfigService.FindByClause(x => x.id == roominfo2.room_no);

                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = roominfo
                });
            }
            catch (Exception e)
            {
                log.Error("错误:" + e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 查询电费缴费信息通过房间id
        /// </summary>
        /// <param name="room_id"></param>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetElectricitybillsInfoToroomID(int room_id, string schoolcode)
        {
            try
            {
                var dt = _tb_payment_electricitybillsService.FindListByClause(x => x.room_id == room_id && x.pay_status == true && x.schoolcode.Trim() == schoolcode,
                    t => t.pay_time, SqlSugar.OrderByType.Desc);
                if (dt.LongCount() > 0)
                {
                    List<tb_payment_electricitybills> list = new List<tb_payment_electricitybills>();
                    foreach (var item in dt)
                    {
                        list.Add(item);
                    }
                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        msg = JsonReturnMsg.GetSuccess,
                        data = list
                    });
                }
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
            catch (Exception e)
            {
                log.Error("错误:" + e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UpdateAddElectricitybills(string out_trade_no)
        {
            try
            {
                var dt = _tb_payment_electricitybillsService.FindByClause(x => x.ordernumber == out_trade_no);
                if (dt == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.UpdateFail
                    });
                }
                dt.pay_time = DateTime.Now;
                dt.pay_status = true;

                _tb_payment_electricitybillsService.Update(dt);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess
                });
            }
            catch (Exception e)
            {
                log.Error("错误:" + e);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UpdateFail
                });
            }
        }
        /// <summary>
        /// 添加电费数据 参数schoolcode building_room_num pay_amount
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddElectricitybills([FromBody] JObject obj)
        {
            try
            {
                int schoolcode = Convert.ToInt32(obj["schoolcode"].ToString());
                string building_room_num = obj["building_room_num"].ToString();
                string appName = obj["appName"].ToString();
                string aliUserID = obj["aliUserID"].ToString();

                decimal pay_amount = Convert.ToDecimal(obj["pay_amount"]);//应缴金额
                DateTime pay_time = DateTime.Now;
                bool pay_status = false;
                string ordernumber = DateTime.Now.ToString("yyyyMMddHHmmssffff") + new Random().Next(1000).ToString().PadLeft(4, '0');
                var dt = _tb_Building_Room_ConfigService.FindByClause(x => x.school_id == schoolcode && x.building_room_num == building_room_num);
                if (dt == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.AddFail
                    });
                }
                long room_id = dt.id;
                //生成支付宝订单
                tb_payment_electricitybills ele = new tb_payment_electricitybills();
                ele.ordernumber = ordernumber;
                ele.room_id = Convert.ToInt32(room_id);
                ele.pay_amount = pay_amount;
                ele.pay_status = pay_status;
                ele.schoolcode = schoolcode.ToString();
                var appaccount = _tb_appaccounts_itemService.FindByClause(x => Convert.ToInt32(x.schoolcode) == schoolcode && x.typename == "电费");
                ele.appaccounts_id = appaccount.id;
                _tb_payment_electricitybillsService.Insert(ele);
                var app = new AppHelper(appName);
                var item = _tb_appaccounts_itemService.FindByClause(x => Convert.ToInt32(x.schoolcode) == schoolcode && x.typename == "电费");
                var account = _tb_payment_accountsService.FindByClause(x => x.id == item.accounts_id);
                string resp = AliPayHelper.AliCreateOrders(account.appid, account.private_key, account.alipay_public_key, aliUserID, "缴纳电费", pay_amount.ToString(), ordernumber);
                JObject jo = ((JObject)JsonConvert.DeserializeObject(
                           ((JObject)JsonConvert.DeserializeObject(resp))["alipay_trade_create_response"]
                           .ToString()));
                if (((JObject)JsonConvert.DeserializeObject(resp))["alipay_trade_create_response"]["code"].ToString() == "10000")
                {
                    return new JsonResult(new
                    {
                        return_code = 10000,
                        return_msg = "订单支付成功",
                        data = (JObject)JsonConvert.DeserializeObject(resp)
                    });
                }

                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }
        /// <summary>
        /// 获取缴纳电费历史记录
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="code"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="room_id"></param>
        /// <param name="ordernumber"></param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetElectricitybillsInfoToSchoolCode(int sEcho, int code, int iDisplayStart, int iDisplayLength,
            string room_id = "", string ordernumber = "", string stime = "", string etime = "")
        {
            try
            {
                int pageStart = iDisplayStart;
                int pageSize = iDisplayLength;
                int pageIndex = (pageStart / pageSize) + 1;
                int totalRecordNum = default(int);
                if (room_id == "0")
                {
                    room_id = "";
                }
                var data = _tb_payment_electricitybillsService.GetelectricitybillsInfo(pageIndex, pageSize, ref totalRecordNum, code, room_id, ordernumber, stime, etime);
                var data2 = _tb_payment_electricitybillsService.FindListByClause(x => x.schoolcode == SqlSugar.SqlFunc.ToString(code), t => t.id, SqlSugar.OrderByType.Asc);
                PaymentItmeTotle pt = new PaymentItmeTotle();
                pt.sj_count = data2.Where(x => x.pay_status == true).ToList().Count();
                pt.pay_amountTotle = data2.Where(x => x.pay_status == true).Sum(x => x.pay_amount);
                pt.dj_count = data2.Where(x => x.pay_status != true).ToList().Count();
                pt.item_count = data2.Count();
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    sEcho = sEcho,
                    iTotalRecords = totalRecordNum,
                    iTotalDisplayRecords = totalRecordNum,
                    aaData = data,
                    totle = pt
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return new JsonResult(new { data = ex.ToString() });
            }
        }
        /// <summary>
        /// 获取实时数据【外部接口】【湖南交通工程学院】
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetElectInfo([FromBody] JObject obj)
        {
            try
            {
                var usercode = obj["UserCode"].ToString();
                var client = new RestClient("http://59.51.42.150:8003/api/Users/GetUserInfo");
                var request = new RestRequest();
                request.Method = Method.POST;
                request.AddHeader("Accept", "application/x-www-form-urlencoded");
                request.Parameters.Clear();
                string body = "UserCode=" + usercode;//参数
                                                     //request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);
                request.AddJsonBody(new { UserCode = usercode });
                var response = client.Execute(request);
                var content = response.Content; // 返回的网页内容
                //var data = JsonConvert.DeserializeObject<Return_MeterAddrInfo>(content);
                var dd = JsonConvert.DeserializeObject<ElectrInfo>(content);
                return Json(dd);
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UpdateFail
                });
            }
        }
        /// <summary>
        /// 获取实时数据【外部接口】【湖南交通工程学院】
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PayElectInfoStatus([FromBody] JObject obj)
        {
            try
            {
                var UserCode = obj["UserCode"].ToString();
                var FactMoney = obj["FactMoney"].ToString();
                var PayDate = obj["PayDate"].ToString();
                var TransId = obj["TransId"].ToString();
                var PayWay = obj["PayWay"].ToString();
                var client = new RestClient("http://59.51.42.150:8003/api/Pay/RemotePay");
                var request = new RestRequest();
                request.Method = Method.POST;
                request.AddHeader("Accept", "application/x-www-form-urlencoded");
                request.Parameters.Clear();
                request.AddJsonBody(new { UserCode = UserCode, FactMoney = FactMoney, PayDate = PayDate, TransId = TransId, PayWay = PayWay });
                var response = client.Execute(request);
                var content = response.Content; // 返回的网页内容
                //var data = JsonConvert.DeserializeObject<Return_MeterAddrInfo>(content);
                var dd = JsonConvert.DeserializeObject<ReturnPayInfo>(content);
                if (dd.ResultCode != 1)
                {
                    var info = _tb_payment_electricitybillsService.FindByClause(x => x.ordernumber == TransId);
                    info.pay_status = false;
                    _tb_payment_electricitybillsService.Update(info);
                }
                return Json(dd);
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UpdateFail
                });
            }
        }
        /// <summary>
        /// 获取学校收款账号
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPayAccountsInfo(string schoolcode)
        {
            try
            {
                var tb = _tb_payment_accountsService.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, SqlSugar.OrderByType.Asc);
                if (tb == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail
                    });
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = tb
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 获取学校收款账号id
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPayAccountsIndex(string schoolcode)
        {
            try
            {
                var tb = _tb_appaccounts_itemService.FindListByClause(x => x.schoolcode == schoolcode && x.typename == "电费", t => t.id, SqlSugar.OrderByType.Asc);
                if (tb == null)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.GetFail
                    });
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = tb
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }

        /// <summary>
        /// 修改房间名通过第三方接口
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateRoomInfoTo10005()
        {
            try
            {
                var client = new RestClient("http://59.51.42.150:8003/api/Users/GetUserAll");
                var request = new RestRequest { Method = Method.POST };
                request.AddHeader("Accept", "application/x-www-form-urlencoded");
                request.Parameters.Clear();
                string body = "";//参数
                request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);
                var response = client.Execute(request);
                var content = response.Content; // 返回的网页内容
                var data = JsonConvert.DeserializeObject<Return_RoomInfo>(content);
                var roomdata = _tb_Building_Room_ConfigService.FindListByClause(x => x.school_id == 10005 && !SqlSugar.SqlFunc.IsNullOrEmpty(x.building_room_num), t => t.id, SqlSugar.OrderByType.Asc);
                foreach (var item in roomdata)
                {
                    var a = data.DataList.Where(x => x.UserCode == item.building_room_num).ToList();
                    if (a.Count!=0)
                    {
                        item.building_room_no = a[0].Doorplate + "";
                        _tb_Building_Room_ConfigService.Update(item);
                    }
                }
                var roomdata2 = _tb_Building_Room_ConfigService.FindListByClause(x => x.school_id == 10005 && !SqlSugar.SqlFunc.IsNullOrEmpty(x.building_room_num), t => t.id, SqlSugar.OrderByType.Asc);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = roomdata2,
                    count = data.DataList.Count
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 添加房间信息通过第三方接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRoomInfoTo10005()
        {
            try
            {
                var client = new RestClient("http://59.51.42.150:8003/api/Users/GetUserAll");
                var request = new RestRequest { Method = Method.POST };
                request.AddHeader("Accept", "application/x-www-form-urlencoded");
                request.Parameters.Clear();
                string body = "";//参数
                request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);
                var response = client.Execute(request);
                var content = response.Content; // 返回的网页内容
                var data = JsonConvert.DeserializeObject<Return_RoomInfo>(content);
                var roomdata = _tb_Building_Room_ConfigService.FindListByClause(x => x.school_id == 10005, t => t.id, SqlSugar.OrderByType.Asc);
                List<tb_building_room_config> list = new List<tb_building_room_config>();
                foreach (var item in data.DataList)
                {
                    
                    if (roomdata.Where(x => x.building_room_num == item.UserCode).ToList().Count==0)
                    {
                        tb_building_room_config model = new tb_building_room_config();
                        model.school_id = 10005;
                        if (item.Doorplate.Contains('-'))
                        {
                            var roomModel = roomdata.Where(x => x.building_room_no == item.Doorplate.Split('-')[0] + "栋").ToList();
                            model.parent_id = roomModel[0].id;
                        }
                        else
                        {
                            try
                            {
                                if (item.Doorplate.Contains('栋'))
                                {
                                    var roomModel = roomdata.Where(x => x.building_room_no == item.Doorplate.Split('栋')[0] + "栋").ToList();
                                    model.parent_id = roomModel[0].id;
                                }
                               
                            }
                            catch (Exception)
                            {

                            }
                            try
                            {
                                if (item.UserCode.Contains("JS"))
                                {
                                    model.parent_id = 747;
                                }
                            }
                            catch (Exception)
                            {

                            }
                            try
                            {
                                if (item.Doorplate.Contains("门面"))
                                {
                                    model.parent_id = 1461;
                                }
                            }
                            catch (Exception)
                            {

                            }
                            
                        }
                        model.ispublic = 0;
                        model.building_room_num = item.UserCode;
                        model.building_room_no = item.Doorplate;
                        if (model.ispublic==null)
                        {
                            continue;
                        }
                        list.Add(model);
                    }
                }
                _tb_Building_Room_ConfigService.Insert(list);
                var roomdata2 = _tb_Building_Room_ConfigService.FindListByClause(x => x.school_id == 10005 && !SqlSugar.SqlFunc.IsNullOrEmpty(x.building_room_num), t => t.id, SqlSugar.OrderByType.Asc);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = roomdata2,
                    count = data.DataList.Count
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        /// <summary>
        /// 添加电表数据10005
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddAmmeterTo10005()
        {
            try
            {
                var roomdata = _tb_Building_Room_ConfigService.FindListByClause(x => x.school_id == 10005, t => t.id, SqlSugar.OrderByType.Asc);
                var ammeterList = _tb_ammeterService.FindAll();
                foreach (var item in roomdata)
                {
                    if (string.IsNullOrWhiteSpace(item.building_room_num)||ammeterList.Any(x=>x.room_id==item.id))
                    {
                        continue;
                    }
                    var client = new RestClient("http://59.51.42.150:8003/api/Users/GetUserInfo");
                    var request = new RestRequest();
                    request.Method = Method.POST;
                    request.AddHeader("Accept", "application/x-www-form-urlencoded");
                    request.Parameters.Clear();
                    string body = "UserCode=" + item.building_room_num;//参数
                                                         //request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);
                    request.AddJsonBody(new { UserCode = item.building_room_num });
                    var response = client.Execute(request);
                    var content = response.Content; // 返回的网页内容
                    var data = JsonConvert.DeserializeObject<Return_MeterAddrInfo>(content);
                    string meterAddr = data.DataList.MeterAddr;
                    
                    tb_ammeter ammeter = new tb_ammeter();
                    ammeter.MeterAddr = meterAddr;
                    ammeter.room_id = Convert.ToInt32(item.id);
                    _tb_ammeterService.Insert(ammeter);
                }
                
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail
                });
            }
        }
    }
}
/// <summary>
/// 
/// </summary>
public class Return_RoomInfo
{
    /// <summary>
    /// 
    /// </summary>
    public Return_RoomInfo()
    {
        DataList = new List<RoomInfo>();
    }
    /// <summary>
    /// 
    /// </summary>
    public int resultCode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<RoomInfo> DataList { get; set; }
}
/// <summary>
/// 
/// </summary>
public class RoomInfo
{
    /// <summary>
    /// 
    /// </summary>
    public string UserCode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Doorplate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Address { get; set; }
}
/// <summary>
/// 
/// </summary>
public class Return_MeterAddrInfo
{
    /// <summary>
    /// 
    /// </summary>
    public Return_MeterAddrInfo()
    {
        DataList = new MeterAddrInfo();
    }
    /// <summary>
    /// 
    /// </summary>
    public int resultCode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public MeterAddrInfo DataList { get; set; }
}
/// <summary>
/// 
/// </summary>
public class MeterAddrInfo
{
    /// <summary>
    /// 
    /// </summary>
    public decimal Remnant { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public decimal UseNumber { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public decimal DayUsedFare { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Valvestate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string MeterAddr { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public decimal MonthUsedFare { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public decimal ReadNumber { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string ReadTime { get; set; }
}
/// <summary>
/// 
/// </summary>
public class GetPayList
{
    /// <summary>
    /// 
    /// </summary>
    public GetPayList()
    {
        DataList = new List<PayListInfo>();
    }
    /// <summary>
    /// 
    /// </summary>
    public int ResultCode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<PayListInfo> DataList { get; set; }
    //public string sign { get; set; }
}
/// <summary>
/// 
/// </summary>
public class PayListInfo
{
    /// <summary>
    /// 
    /// </summary>
    public string PayID { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime PayDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public decimal FareMoney { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public decimal BuyInt { get; set; }
}
/// <summary>
/// 
/// </summary>
public class ElectrInfo
{
    /// <summary>
    /// 
    /// </summary>
    public ElectrInfo()
    {
        DataList = new ElectrInfo2();
    }
    //{"ResultCode":1,"Message":"操作成功","DataList":{"Remnant":162.00,"UseNumber":1.12,"DayUsedFare":0.67200000,"Valvestate":1,"MeterAddr":"401306290101","MonthUsedFare":90.99600000,"ReadNumber":1324.68,"Price":0.600000,"ReadTime":"2018/10/28 18:13:30"}}
    /// <summary>
    /// 
    /// </summary>
    public int ResultCode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public ElectrInfo2 DataList { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string sign { get; set; }
}
/// <summary>
/// 
/// </summary>
public class ElectrInfo2
{
    /// <summary>
    /// 
    /// </summary>
    public decimal DayUsedFare { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string MeterAddr { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public decimal MonthUsedFare { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public decimal ReadNumber { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime ReadTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public decimal Remnant { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public decimal UseNumber { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Valvestate { get; set; }
}
/// <summary>
/// 
/// </summary>
public class ReturnPayInfo
{
    /// <summary>
    /// 
    /// </summary>
    public int ResultCode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Message { get; set; }
}