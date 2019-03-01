using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RestSharp;
using Models.ViewModels;
using SchoolWebApi.Utility;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 中卡淋浴系统接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class BathController : Controller
    {
        /// <summary>
        /// 初始化日志对象
        /// </summary>
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        //支付成功回调小程序通知地址
        private static string URL = "";
        //提供给支付宝的异步回调地址
        private string Zfburl = "";

        private readonly Itb_school_userService _schoolUser;
        private readonly Itb_school_departmentService _schoolDepartment;
        private readonly Itb_payment_waterrentService _Itb_payment_waterrentService;
        private readonly Itb_appaccounts_itemService _tb_appaccounts_itemService;
        private readonly Itb_payment_accountsService _tb_payment_accountsService;
        private readonly string _aesSecretKey = "test_lxt_00000000000000000000000";
        private readonly string _md5SecretKey = "test_000";
        private readonly string _serial = "1520502968836";
        private readonly int successCode = 1;
        private readonly int failCode = -1;
        
        

        //服务ID常量
        private long _appId = 5;

        /// <summary>
        /// 
        /// </summary>
        public BathController(Itb_school_userService schoolUser, Itb_school_departmentService schoolDepartment, Itb_payment_waterrentService Itb_payment_waterrentService, Itb_appaccounts_itemService Itb_appaccounts_itemService, Itb_payment_accountsService Itb_payment_accountsService)
        {
            _schoolUser = schoolUser;
            _schoolDepartment = schoolDepartment;
            _Itb_payment_waterrentService = Itb_payment_waterrentService;
            _tb_appaccounts_itemService = Itb_appaccounts_itemService;
            _tb_payment_accountsService = Itb_payment_accountsService;
            //设置appid
            AppHelper helper = new AppHelper("RuohuoWater");
            _appId =Convert.ToInt64(helper.CAppId);
            Zfburl = helper.Zfburl;
        }

        /// <summary>
        /// 不合法的访问请求Json
        /// </summary>
        /// <returns></returns>
        private JsonResult InvalidJsonResult()
        {
            return Json(new
            {
                code = failCode,
                serial = _serial,
                data = "",
                message = @"不合法的访问请求"
            });
        }


        /// <summary>
        /// 更新未完成订单状态
        /// </summary>
        private void UpdatePaywater()
        {
            
            #region 处理订单表中未完成状态的订单数据

            var dt = _Itb_payment_waterrentService.FindListByClause(x => x.posDataTime <= DateTime.Now && x.posDataTime >= DateTime.Now.AddDays(-5) && x.orderState == false, t => t.id, SqlSugar.OrderByType.Asc);
            if (dt.LongCount() > 0)
            {

                foreach (var item in dt)
                {
                    string ordernumber = item.orderId;
                    int appaccounts_id = Convert.ToInt32(item.appaccounts_id);
                    var appaccount = _tb_appaccounts_itemService.FindByClause(x => x.id == appaccounts_id);
                    var payment_accounts = _tb_payment_accountsService.FindByClause(x => x.id == appaccount.accounts_id);
                    bool isOk = AliPayHelper.GetOrdersStatus2(ordernumber, payment_accounts.appid, payment_accounts.private_key, payment_accounts.alipay_public_key);
                    if (isOk)
                    {
                        item.orderState = true;
                        _Itb_payment_waterrentService.Update(item);
                    }
                }
            }
            #endregion

        }

        /// <summary>
        /// 检查签名是否正确
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool CheckSign(string sign, string code)
        {
            var _sign = MD5Helper.MD5Encrypt32(_md5SecretKey + code);
            return String.Equals(_sign, sign, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 返回Sign和Code 测试示例: appId=10000#?aliUserId=2088702258167392
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SignCode(string code)
        {
            var aesCode = AESHelper.Encrypt(code, _aesSecretKey);
            var sign = MD5Helper.MD5Encrypt32(_md5SecretKey + aesCode);
            return Json(new { Sign = sign, Code = aesCode });
        }

        /// <summary>
        /// 检测令牌是否合法的公共方法
        /// </summary>
        /// <param name="sign">签名</param>
        /// <param name="code">请求参数</param>
        /// <param name="appid">服务ID常量</param>
        /// <param name="serviceAction">处理委托</param>
        /// <returns></returns>
        private ActionResult CommonService<T>(string sign, string code, long appid, Func<T, ActionResult> serviceAction) where T : new()
        {
            try
            {
                if (CheckSign(sign, code) && Equals(appid, _appId))
                {
                    var _code = AESHelper.Decrypt(code, _aesSecretKey);
                    var codeParams = Regex.Split(_code, @"#\?", RegexOptions.IgnoreCase);
                    var model = new T();
                    foreach (string param in codeParams)
                    {
                        if (param.Contains("="))
                        {
                            var paramsArray = param.Split(new[] { '=' });
                            if (paramsArray.Length == 2)
                            {
                                Type t = model.GetType();
                                var prop = t.GetProperty(paramsArray[0]);
                                prop.SetValue(model,
                                    Convert.ChangeType(paramsArray[1], prop.PropertyType));
                            }
                        }
                    }
                    return serviceAction(model);
                }
                return InvalidJsonResult();
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return InvalidJsonResult();
            }
        }

        /// <summary>
        /// 中卡淋浴系统--获取人员信息接口
        /// </summary>
        /// <param name="sign">签名</param>
        /// <param name="code">请求参数,测试示例: appId=10000#?aliUserId=2088702258167392</param>
        /// <param name="appid">服务ID常量 1000000000000</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BathPosService(string sign, string code, long appid) => CommonService<BathPosServiceModel>(
            sign, code, appid, model =>
            {
                var userInfo =
                    _schoolUser.FindByClause(t =>
                        t.ali_user_id == model.aliUserId);

                if (userInfo != null)
                {
                    //var departmentName = userInfo?.department?.Substring(userInfo.department.LastIndexOf('/'),
                    //    userInfo.department.Length - userInfo.department.LastIndexOf('/'));
                    //var departmentInfo =
                    //    _schoolDepartment.FindByClause(t => t.schoolcode == model.appId && t.name == departmentName);

                    return Json(new
                    {
                        code = 0,
                        serial = _serial,
                        data = new
                        {
                            //用户ID
                            userId = userInfo.user_id,
                            //用户名称
                            userName = userInfo.user_name,
                            //用户手机号
                            cellPhone = userInfo.cell,
                            //学号
                            studentNumber = userInfo.student_id,
                            //支付宝用户ID
                            aliUserId = userInfo.ali_user_id,
                            //学校或部门ID
                            deptId = userInfo.school_id
                        },
                        message = @""
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = failCode,
                        serial = _serial,
                        data = "",
                        message = @"该用户不存在"
                    });

                }
              
            });

        /// <summary>
        /// 未支付订单查询
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="code"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserFindOrderService(string sign, string code, long appid) =>
            CommonService<UserFindOrderServiceModel>(sign, code, appid, model =>
            {

              

                //向支付宝更新本地未支付订单状态
                //   UpdatePaywater();
                var  dt = _Itb_payment_waterrentService.FindListByClause(x => x.posDataTime <= DateTime.Now && x.posDataTime >= DateTime.Now.AddDays(-3) && x.orderState == false&& x.appId == model.appId && x.userId == model.userId && x.deptId == model.deptId &&x.remarks== "WAIT_BUYER_PAY", p => p.id, SqlSugar.OrderByType.Desc);

                List<veiw_PaymentWaterChilder> listw = new List<veiw_PaymentWaterChilder>();

                if (dt.LongCount() > 0)
                {

                    foreach (var item in dt)
                    {
                        //创建子类进行复制对象
                        veiw_PaymentWaterChilder v1 = new veiw_PaymentWaterChilder();
                        v1.aliUserId = item.aliUserId;
                        v1.appaccounts_id = item.appaccounts_id;
                        v1.appId = item.appId;
                        v1.deptId = item.deptId;
                        v1.id = item.id;
                        v1.orderId = item.orderId;
                        v1.orderState = item.orderState;
                        v1.posDataTime = item.posDataTime;
                        v1.posPay = item.posPay;
                        v1.remarks = item.remarks;
                        v1.userId = item.userId;
                        

                        string ordernumber = item.orderId;
                        int appaccounts_id = Convert.ToInt32(item.appaccounts_id);
                        var appaccount = _tb_appaccounts_itemService.FindByClause(x => x.id == appaccounts_id);
                        var payment_accounts = _tb_payment_accountsService.FindByClause(x => x.id == appaccount.accounts_id);
                        string msg = AliPayHelper.GetOrdersStatus(ordernumber, payment_accounts.appid, payment_accounts.private_key, payment_accounts.alipay_public_key);
                      
                        if (((JObject)JsonConvert.DeserializeObject(msg))["alipay_trade_query_response"]["code"].ToString() == "10000")
                        {
                            alipay_trade_query_response req1 = JsonConvert.DeserializeObject<alipay_trade_query_response>(((JObject)JsonConvert.DeserializeObject(msg))["alipay_trade_query_response"].ToString());

                            v1.vmodel = req1;
                            item.remarks = req1.trade_status;
                            //如果交易成功
                            if (req1.trade_status == "TRADE_SUCCESS")
                            {
                                item.orderState = true;
                            }
                            _Itb_payment_waterrentService.Update(item);
                        }
                        listw.Add(v1);
                    }
                }
               //得到更新后的数据 订单成功但未支付
                dt = dt.Where(x => x.orderState == false&&x.remarks== "WAIT_BUYER_PAY");

                var _list1 = (from p in listw
                              select new { orderId = p.orderId, posPay=p.posPay*100, payTradeNo= p.vmodel.trade_no }).ToList();
                //如果有未支付数据
                if (_list1 .Count>0)
                {
                    return Json(new 
                    {
                        code = 0,
                        serial = _serial,
                        data = _list1,
                        message = @""
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = -2,
                        serial = _serial,
                        data ="",
                        message = @"没有需要支付的订单"
                    });

                }


                
            });

        /// <summary>
        /// 上传消费订单接口
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="code">请求参数,测试示例: appId=10000#?userId=123#?deptId=123#?orderId=123#?posPay=100#?machineId=123#?posDataTime=2018-11-11 11:11:11#?posLitr=1.1#?notifyUrl=http://xxx</param>
        /// <param name="appid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpOrderService(string sign, string code, long appid) => CommonService<UpOrderServiceModel>(
            sign, code, appid, model =>
            {

                int schoolcode = Convert.ToInt32(model.deptId);
                
              
                string aliUserID = _schoolUser.FindById(model.userId).ali_user_id;

                double pay_amount = model.posPay*0.01;//应缴金额 传来的是分
                DateTime pay_time = DateTime.Now;
                bool pay_status = false;
                string ordernumber = model.orderId;
               
                var appaccount = _tb_appaccounts_itemService.FindByClause(x => Convert.ToInt32(x.schoolcode) == schoolcode && x.typename == "水费");

               
           
                var account = _tb_payment_accountsService.FindByClause(x => x.id == appaccount.accounts_id);
                string resp = AliPayHelper.AliCreateOrders(account.appid, account.private_key, account.alipay_public_key, aliUserID, "缴纳水费", pay_amount.ToString(), ordernumber, Zfburl);
                //设置回调地址
                URL = model.notifyUrl;

                Log.Info("发给支付宝地址" + Zfburl + "请求地址" + URL);
              
                if (((JObject)JsonConvert.DeserializeObject(resp))["alipay_trade_create_response"]["code"].ToString() == "10000")
                {
                    
                   string out_no= ((JObject)JsonConvert.DeserializeObject(resp))["alipay_trade_create_response"]["out_trade_no"].ToString();

                    tb_payment_waterrent e1= _Itb_payment_waterrentService.FindByClause(p => p.orderId == out_no);
                    if (e1== null)
                    {
                        //生成支付宝订单
                        tb_payment_waterrent ele = new tb_payment_waterrent();
                        ele.orderId = ordernumber;
                        ele.posDataTime = pay_time;
                        ele.posPay = pay_amount;
                        ele.aliUserId = aliUserID;
                        ele.orderState = pay_status;
                        ele.deptId = schoolcode;
                        ele.userId = model.userId;
                        ele.appaccounts_id = appaccount.id;
                        ele.appId = model.appId;
                        ele.remarks = "WAIT_BUYER_PAY";
                        // var app = new AppHelper(appName);
                        _Itb_payment_waterrentService.Insert(ele);
                    }

                    return Json(new
                    {
                        code = 0,
                        serial = _serial,
                        data = new
                        {
                            //学生id
                            userId = model.userId,
                            //消费订单ID
                            orderId = ordernumber,
                            //消费金额
                            posPay = model.posPay,
                            //可以调起支付的信息
                            payTradeNo = ((JObject)JsonConvert.DeserializeObject(resp))["alipay_trade_create_response"]["trade_no"].ToString()
                        },
                        message = @"订单已生成"
                    });
                }
                else
                {
                    if (((JObject)JsonConvert.DeserializeObject(resp))["alipay_trade_create_response"]["code"].ToString() == "40004")
                    {
                        if (((JObject)JsonConvert.DeserializeObject(resp))["alipay_trade_create_response"]["sub_code"].ToString()== "ACQ.TRADE_HAS_SUCCESS")
                        {
                            return Json(new
                            {
                                code = -2,
                                serial = _serial,
                                data = "",
                                message = @"上传成功，并且订单已支付成功"
                            });

                        }
                    }
                    

                    return Json(new
                    {
                        code = failCode,
                        serial = _serial,
                        data = "",
                        message = ((JObject)JsonConvert.DeserializeObject(resp))["alipay_trade_create_response"]["sub_msg"].ToString()
                    });

                }


               
            });

        /// <summary>
        /// 订单状态查询接口
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="code"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FindOrderStateService(string sign, string code, long appid) =>
            CommonService<FindOrderStateServiceModel>(sign, code, appid, model =>
            {
                //订单编号
                string ordernumber = model.orderId;

                //在订单表中找到该订单实例
                var Orderstate = _Itb_payment_waterrentService.FindByClause(p => p.orderId == ordernumber);

                int appaccounts_id = Convert.ToInt32(Orderstate.appaccounts_id);
                var appaccount = _tb_appaccounts_itemService.FindByClause(x => x.id == appaccounts_id);
                var payment_accounts = _tb_payment_accountsService.FindByClause(x => x.id == appaccount.accounts_id);
                //到支付宝检查状态
                bool isOk = AliPayHelper.GetOrdersStatus2(ordernumber, payment_accounts.appid, payment_accounts.private_key, payment_accounts.alipay_public_key);

                //如果查找到就更新状态
                if (isOk)
                {
                    Orderstate.orderState = true;
                    _Itb_payment_waterrentService.Update(Orderstate);
                }

                return Json(new
                {
                    code = successCode,
                    serial = _serial,
                    data = new
                    {
                        //消费订单ID
                        orderId = Orderstate.orderId,
                        //0 支付成功 1未支付
                        state =Convert.ToInt32(isOk)
                    },
                    message =(bool)Orderstate.orderState ? @"支付成功" : @"未支付"
                });
            });

        /// <summary>
        /// 异步通知订单状态接口notifyUrlService(回调地址中卡公司提供)
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="code"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NotifyUrlService(string sign, string code, long appid) =>
            CommonService<NotifyUrlServiceModel>(sign, code, appid, model =>
            {
                var a = model.appId;
                return Json(new
                {
                    code = successCode,
                    serial = _serial,
                    data = new
                    {
                        //消费订单ID
                        orderId = default(string),
                        //操作成功 返回值只能是1
                        success = 1
                    },
                    message = @"操作成功"
                });
            });


        /// <summary>
        /// 支付宝付款成功回调函数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult NotifyUrl()
        {
            string status = Request.Form["trade_status"].ToString();
            string orderId = Request.Form["out_trade_no"].ToString();
            Log.Info(status + "_" + orderId + "回调地址:" + URL);
            tb_payment_waterrent water = _Itb_payment_waterrentService.FindByClause(p => p.orderId == orderId);



            water.remarks = status;


            if (status == "TRADE_SUCCESS")
            {
                //更新订单状态

                water.orderState = true;

                _Itb_payment_waterrentService.Update(water);

                //组合参数

            }

            string code = "appId="+ water.appId+ "#?userId="+ water.userId+"#?deptId="+ water.deptId+ "#?orderId="+ water.orderId + "#?posPay="+Convert.ToInt32(water.posPay*100)+ "#?orderState="+Convert.ToInt32(water.orderState);


            var aesCode = AESHelper.Encrypt(code, _aesSecretKey);
            var sign = MD5Helper.MD5Encrypt32(_md5SecretKey + aesCode);
            Log.Info("____" + URL + "?sign=" + sign + "&code=" + aesCode + "&appId=" + _appId);
            //发给小程序支付通知
            var client = new RestClient(URL + "?sign=" + sign + "&code=" + aesCode + "&appId=" + _appId);

            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.Parameters.Clear();



            var response = client.Execute(request);
            var content = response.Content;
            return Json("success");

        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="ordernumber"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetWaterInfo(int code, int iDisplayStart, int iDisplayLength, string ordernumber = "", string startTime = "", string endTime = "")
        {

            try
            {
                int pageStart = iDisplayStart;
                int pageSize = iDisplayLength;
                int pageIndex = (pageStart / pageSize) + 1;
                int totalRecordNum = default(int);
               
               var data = _Itb_payment_waterrentService.GetWaterInfo(pageIndex, pageSize, code, ref totalRecordNum, ordernumber, startTime, endTime);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    iTotalRecords = totalRecordNum,
                    iTotalDisplayRecords = totalRecordNum,
                    aaData = data
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误:" + ex);
                return new JsonResult(new {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail,
                    data = ex.ToString() });
            }


        }

        /// <summary>
        /// 获取订单总额和订单数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetWaterCount(int code)
        {
            var count = _Itb_payment_waterrentService.GetSumOrderandSumPay(code).FirstOrDefault();
       


            if (count!=null)
            {
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    wcount=count.wcount,
                    scussecount=count.scussecount,
                    noscussecount=count.noscussecount,
                    sumprice =count.sumprice
                });

            }
            else
            {

                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    wcount = 0,
                    scussecount = 0,
                    noscussecount = 0,
                    sumprice = 0
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
                var tb = _tb_appaccounts_itemService.FindListByClause(x => x.schoolcode == schoolcode && x.typename == "水费", t => t.id, SqlSugar.OrderByType.Asc);
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
                Log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }

    }
}
