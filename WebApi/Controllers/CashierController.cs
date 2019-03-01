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
    /// 收银台接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class CashierController : Controller
    {

        private Itb_cashier_trade_orderService _tb_cashier_trade_orderService;

        private Itb_cashier_deviceService _tb_cashier_deviceService;

        private Itb_cashier_stallService _tb_cashier_stallService;

        private Itb_cashier_dining_hallService _Itb_cashier_dining_hallService;

        private Itb_payment_accountsService _Itb_payment_accountsService;

        private Itb_school_InfoService _Itb_school_InfoService;
        /// <summary>
        /// 
        /// </summary>
        public CashierController(Itb_cashier_trade_orderService tb_cashier_trade_orderService, Itb_cashier_deviceService tb_cashier_deviceService, Itb_cashier_stallService tb_cashier_stallService, Itb_cashier_dining_hallService tb_cashier_dining_hallService, Itb_payment_accountsService Itb_payment_accountsService, Itb_school_InfoService Itb_school_InfoService)
        {
            _tb_cashier_trade_orderService = tb_cashier_trade_orderService;
            _tb_cashier_deviceService = tb_cashier_deviceService;
            _tb_cashier_stallService = tb_cashier_stallService;
            _Itb_cashier_dining_hallService = tb_cashier_dining_hallService;
            _Itb_payment_accountsService = Itb_payment_accountsService;
            _Itb_school_InfoService = Itb_school_InfoService;
        }

        /// <summary>
        /// 收银台概收款统计
        /// </summary>
        /// <param name="school_id"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCashierOrder(string school_id, string st, string et)
        {
            Getcashier_trade_orderSum obj = _tb_cashier_trade_orderService.Getcashier_trade_order(school_id, st, et);
            return Json(new
            {
                code = "10000",
                msg = "查询成功",
                pays = obj.pays,
                ordercount = obj.ordercount,
                array_alldays = obj.array_alldays
            });
        }

        /// <summary>
        /// 数据概览
        /// </summary>
        /// <param name="school_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCashierOrderCount(string school_id)
        {
            string st = DateTime.Now.ToString("yyyy/MM/dd");
            string et = DateTime.Now.ToString("yyyy/MM/dd");
            Cashier_trade_orderSum obj = _tb_cashier_trade_orderService.Getcashier_trade_orderCount(school_id, st, et);

            string lst = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            string let = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            Cashier_trade_orderSum obj1 = _tb_cashier_trade_orderService.Getcashier_trade_orderCount(school_id, lst, let);


            int count = _tb_cashier_deviceService.FindListByClause(p => p.schoolcode == school_id, p => p.id, SqlSugar.OrderByType.Asc).Count();
            return Json(new
            {
                deviceCount = count,
                totalMoney = obj.paysum,
                yesterdayMoney = obj1.paysum,
                code = "10000",
                msg = "查询成功",

            });
        }

        /// <summary>
        /// 获取档口
        /// </summary>
        /// <param name="school_id"></param>
        /// <param name="diningid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStall(string school_id,string diningid)
        {
            var stall= _tb_cashier_stallService.GetStall(school_id, diningid);
        
            return Json(new
            {
                code = "10000",
                msg = "查询成功",
                data= stall

            });

        }

        /// <summary>
        /// 获取食堂
        /// </summary>
        /// <param name="school_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDinningHall(string school_id)
        {
            var list= _Itb_cashier_dining_hallService.FindListByClause(p => p.schoolcode == school_id, p => p.id, SqlSugar.OrderByType.Asc);
            return Json(new
            {
                code = "10000",
                msg = "查询成功",
                data = list

            });

        }

        /// <summary>
        /// 交易流水查询
        /// </summary>
        /// <param name="dining_hall"></param>
        /// <param name="stall"></param>
        /// <param name="order"></param>
        /// <param name="user_code"></param>
        /// <param name="machine"></param>
        /// <param name="tid"></param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="school_id"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFlowing(string dining_hall, string stall, string order, string user_code, string machine, string tid, string stime, string etime, string school_id,int iDisplayStart,int iDisplayLength)
        {

            int pageStart = iDisplayStart;
            int pageSize = iDisplayLength;
           
            int totalRecordNum = default(int);
            var list = _tb_cashier_trade_orderService.GetFlowing(dining_hall, stall, order, user_code, machine, tid, stime, etime, school_id, pageStart, pageSize,ref totalRecordNum);
            totalRecordNum = list.Count();
            return Json(new {
                code = "10000",
                msg = "查询成功",
                iTotalRecords = totalRecordNum,
                data = list
            });


        }

        /// <summary>
        /// 下载交易流水
        /// </summary>
        /// <param name="dining_hall"></param>
        /// <param name="stall"></param>
        /// <param name="order"></param>
        /// <param name="user_code"></param>
        /// <param name="machine"></param>
        /// <param name="tid"></param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="school_id"></param>
        [HttpGet]
        public ActionResult GetFlowingExcel(string dining_hall, string stall, string order, string user_code, string machine, string tid, string stime, string etime, string school_id)
        {

            var list = _tb_cashier_trade_orderService.GetFlowingExcel(dining_hall, stall, order, user_code, machine, tid, stime, etime, school_id);
            return Json(new
            {
                code = "10000",
                msg = "下载成功",
                data = list
            });


        }

        /// <summary>
        /// 获取收款账号
        /// </summary>
        /// <param name="school_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPayment_accounts(string school_id)
        {
           var list= _Itb_payment_accountsService.FindListByClause(p => p.schoolcode == school_id, p => p.id, SqlSugar.OrderByType.Asc);
            return Json(new
            {
                code = "10000",
                msg = "查询成功",
                data = list
            });
        }

        /// <summary>
        /// 对账单下载
        /// </summary>
        /// <param name="timeType"></param>
        /// <param name="datetime"></param>
        /// <param name="school_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetBillExcel(string timeType, string datetime, string school_id,string payment_accounts)
        {
            var list = _tb_cashier_trade_orderService.GetBillExcel(timeType, datetime, school_id);
            return Json(new
            {
                code = "10000",
                msg = "下载成功",
                data = list
            });


        }

        /// <summary>
        /// 资金管理汇总
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTotalt(string stime, string etime, string school_id)
        {

            var list = _tb_cashier_trade_orderService.getTotalt(stime, etime, school_id);

            
            return Json(new
            {
                code = "10000",
                msg = "查询成功",
                data = list
            });

        }


        /// <summary>
        /// 资金管理详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetorderDetail(string dining_hall, string stall, string stime, string etime, string school_id, int iDisplayStart, int iDisplayLength)

        {
            int pageStart = iDisplayStart;
            int pageSize = iDisplayLength;
          
            int totalRecordNum = default(int);

            var list = _tb_cashier_trade_orderService.getCapitalList(dining_hall, stall, stime, etime, school_id, pageStart, pageSize, ref totalRecordNum);
            return Json(new
            {
                code = "10000",
                msg = "查询成功",
                iTotalRecords = totalRecordNum,
                data = list
            });
        }

        /// <summary>
        /// 获取档口树
        /// </summary>
        /// <param name="school_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDininglist(string school_id)
        {
            var top1 = _Itb_school_InfoService.FindByClause(p => p.School_Code == school_id);
            var to2 = _Itb_cashier_dining_hallService.FindListByClause(p => p.schoolcode == school_id, p => p.id, SqlSugar.OrderByType.Asc);
            var to3 = _tb_cashier_stallService.GetStallAll(school_id);

            DepartmentTree deptree = new DepartmentTree();
            deptree.id = 0;
            deptree.name = top1.School_name;
            deptree.label = top1.School_name;
            deptree.schoolcode = school_id;
            deptree.treeLever = 0;
            foreach (var item2 in to2)
            {
                DepartmentTree deptree2 = new DepartmentTree();
                deptree2.id = item2.id;
                deptree2.label = item2.name;
                deptree2.name = item2.name;
                deptree2.schoolcode = item2.schoolcode;
                deptree2.treeLever = 1;
                deptree.children.Add(deptree2);
                var top3v = to3.Where(p => p.dining_tall == item2.id);
                foreach (var item in top3v)
                {
                    DepartmentTree deptree3 = new DepartmentTree();
                    deptree3.id = item.id;
                    deptree3.label = item.name;
                    deptree3.name = item.name;
                    deptree3.schoolcode = item2.schoolcode;
                    deptree3.treeLever = 2;
                    deptree2.children.Add(deptree3);
                }
            }

            return Json(new
            {
                code = "10000",
                msg = "查询成功",
                data = deptree,
                defaultProps = new { children = "children", label = "label" },
            });

        }


        /// <summary>
        /// 获取食堂设备
        /// </summary>
        /// <param name="queryparm">查询名字</param>
        /// <param name="sn">编号</param>
        /// <param name="hallid">食堂id</param>
        /// <param name="tallid">档口id</param>
        /// <param name="school_id"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Getdevice(string queryparm, string sn, string hallid, string tallid, string school_id, int iDisplayStart, int iDisplayLength)
        {
            int pageStart = iDisplayStart;
            int pageSize = iDisplayLength;
            int pageIndex = (pageStart / pageSize) + 1;
            int totalRecordNum = default(int);
            var list= _tb_cashier_deviceService.Getinfo(queryparm, hallid, tallid, sn,  school_id, pageIndex, pageSize, ref totalRecordNum);
            return Json(new
            {
                code = "10000",
                msg = "查询成功",
                iTotalRecords = totalRecordNum,
                aaData = list
            });

        }

       

        /// <summary>
        /// 删除食堂设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DelDeldevice(int id)
        {
            object[] ids = { id };
           if(_tb_cashier_deviceService.DeleteByIds(ids))
            {
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.DeleteSuccess
                });

            }
           else
            {
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.DeleteFail
                });

            }
        }

        /// <summary>
        /// 删除档口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Del_stall(int id)
        {

            object[] ids = { id };
            if (_tb_cashier_stallService.DeleteByIds(ids))
            {
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.DeleteSuccess
                });

            }
            else
            {
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.DeleteFail
                });

            }
        }


        /// <summary>
        /// 删除食堂
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Del_dining_hall(int id)
        {

            object[] ids = { id };
            if (_Itb_cashier_dining_hallService.DeleteByIds(ids))
            {
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.DeleteSuccess
                });

            }
            else
            {
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.DeleteFail
                });

            }
        }





        /// <summary>
        /// 新增食堂
        /// </summary>
        /// <param name="name"></param>
        /// <param name="introduction"></param>
        /// <param name="school_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add_dining_hall(string name,string introduction, string school_id)
        {
            tb_cashier_dining_hall hall = new tb_cashier_dining_hall();
            hall.name = name;
            hall.introduction = introduction;
            hall.schoolcode = school_id;
            long id = _Itb_cashier_dining_hallService.Insert(hall);
            if ( id> 0)
            {
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
                    id = id

                });
            }
            else
            {
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail

                });
            }
        }



        /// <summary>
        /// 新增档口
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="dining_hall_id"></param>
        /// <param name="school_card_rate"></param>
        /// <param name="student_card_rate"></param>
        /// <param name="other_rate"></param>
        /// <param name="refund"></param>
        /// <param name="payee_account"></param>
        /// <param name="subsidy"></param>
        /// <param name="refundpassword"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add_stall(string name,int type, int dining_hall_id,int school_card_rate, int student_card_rate,int other_rate,int refund,int payee_account,int subsidy,string refundpassword)
        {
            tb_cashier_stall tall = new tb_cashier_stall();
            tall.name = name;
            tall.type = type;
            tall.dining_tall = dining_hall_id;
            tall.school_card_rate = school_card_rate;
            tall.student_card_rate = student_card_rate;
            tall.subsidy = subsidy;
            tall.refund_password = refundpassword;
            long id = _tb_cashier_stallService.Insert(tall);
            if (id > 0)
            {
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
                    id = id
                });

            }
            else
            {
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail

                });
            }

        }


      /// <summary>
      /// 新增设备
      /// </summary>
      /// <param name="sn"></param>
      /// <param name="brand"></param>
      /// <param name="stallid"></param>
      /// <param name="schoolcode"></param>
      /// <returns></returns>
        [HttpGet]
        public ActionResult AddDevice(string sn,int brand,int stallid,string schoolcode)
        {
            tb_cashier_device device = new tb_cashier_device();
            device.brand = brand;
            device.schoolcode = schoolcode;
            device.sn = sn;
            device.stall = stallid;
            long id = _tb_cashier_deviceService.Insert(device);
            if (id > 0)
            {

                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
                    id=id

                });
            }
            else
            {
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail

                });
            }


        }

     /// <summary>
     /// 修改设备
     /// </summary>
     /// <param name="id"></param>
     /// <param name="sn"></param>
     /// <param name="brand"></param>
     /// <param name="stallid"></param>
     /// <returns></returns>
        [HttpGet]
        public ActionResult Edit_Device(int id,string sn, int brand, int stallid)
        {
            var device= _tb_cashier_deviceService.FindByClause(p => p.id == id);
            device.sn = sn;
            device.brand = brand;
            device.stall = stallid;
            if(_tb_cashier_deviceService.Update(device))
            {
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess

                });
            }
            else
            {

                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UploadFail

                });
            }

        }


        /// <summary>
        /// 修改档口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="dining_hall_id"></param>
        /// <param name="school_card_rate"></param>
        /// <param name="student_card_rate"></param>
        /// <param name="other_rate"></param>
        /// <param name="refund"></param>
        /// <param name="payee_account"></param>
        /// <param name="subsidy"></param>
        /// <param name="refundpassword"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit_stall(int id,string name, int type, int dining_hall_id, int school_card_rate, int student_card_rate, int other_rate, int refund, int payee_account, int subsidy, string refundpassword)
        {
            tb_cashier_stall tall = _tb_cashier_stallService.FindByClause(p => p.id == id);
            tall.name = name;
            tall.type = type;
            tall.dining_tall = dining_hall_id;
            tall.school_card_rate = school_card_rate;
            tall.student_card_rate = student_card_rate;
            tall.subsidy = subsidy;
            tall.refund_password = refundpassword;
            if (_tb_cashier_stallService.Update(tall))
            {
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess

                });

            }
            else
            {
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UploadFail

                });

            }

        }

        /// <summary>
        /// 修改食堂
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="introduction"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit_dining_hall(int id,string name, string introduction)
        {
            var hall = _Itb_cashier_dining_hallService.FindByClause(p => p.id == id);
            hall.name = name;
            hall.introduction = introduction;
            if(_Itb_cashier_dining_hallService.Update(hall))
            {
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess

                });

            }
            else
            {
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.UploadFail

                });

            }

        }

        /// <summary>
        /// 获取食堂信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get_dining_hall(int id)
        {
            var hall = _Itb_cashier_dining_hallService.FindByClause(p => p.id == id);
            if(hall!=null)
            {
                return Json(new
                {
                    code = "10000",
                    msg = "查询成功",
                    data=hall

                });

            }
            else
            {
                return Json(new
                {
                    code = "00000",
                    msg = "查询失败"

                });

            }

        }


        /// <summary>
        /// 获取档口信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get_stall(int id)
        {
            var stall = _tb_cashier_stallService.FindByClause(p=>p.id==id);
            if (stall != null)
            {
                return Json(new
                {
                    code = "10000",
                    msg = "查询成功",
                    data = stall

                });

            }
            else
            {
                return Json(new
                {
                    code = "00000",
                    msg = "查询失败"

                });

            }


        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get_decive(int id)
        {
            var decive = _tb_cashier_deviceService.FindByClause(p => p.id == id);
            if (decive != null)
            {
                return Json(new
                {
                    code = "10000",
                    msg = "查询成功",
                    data = decive

                });

            }
            else
            {
                return Json(new
                {
                    code = "00000",
                    msg = "查询失败"

                });

            }


        }





























    }
}