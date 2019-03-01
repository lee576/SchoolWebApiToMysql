using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 缴费大厅
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class PayMallController : Controller
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private readonly Itb_payment_itemService _tb_payment_itemService;
        private readonly Itb_payment_recordService _tb_payment_recordService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_payment_itemService"></param>
        /// <param name="tb_payment_recordService"></param>
        public PayMallController(Itb_payment_itemService tb_payment_itemService, Itb_payment_recordService tb_payment_recordService)
        {
            _tb_payment_itemService = tb_payment_itemService;
            _tb_payment_recordService = tb_payment_recordService;
        }

        /// <summary>
        /// 通过校园编码获取收费项目
        /// </summary>
        /// <param name="schoolCode"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetPaymentItem(string schoolCode)
        {
            List<tb_payment_item> lst = _tb_payment_itemService.FindListByClause(x => x.schoolcode == schoolCode,x=>x.id) as List<tb_payment_item>;
            return new JsonResult(new {
                return_code = 0,
                return_msg = "查询成功",
                data = lst
            });
        }

        /// <summary>
        /// 通过查询条件查询收费项目
        /// </summary>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="schoolcode"></param>
        /// <param name="student"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="sdate"></param>
        /// <param name="edate"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPaymentItemSearch(string iDisplayStart,string iDisplayLength,string schoolcode,string student,string name,string status,string sdate,string edate)
        {
            int start = int.Parse(iDisplayStart);
            start++;
            int end = int.Parse(iDisplayLength);
            end += start - 1;
            int count = 0;
            if (!string.IsNullOrWhiteSpace(sdate))
                sdate = sdate + " 00:00:00";
            if (!string.IsNullOrWhiteSpace(edate))
                edate = edate + " 23:59:59";
            var rt=_tb_payment_itemService.searchPayment(start, end, schoolcode, student, name, status,sdate, edate);
            return Json(new {
                return_code = 0,
                return_msg = "查询成功",
                iTotalRecords =rt.Item2,
                iTotalDisplayRecords = count,
                aaData = rt.Item1
            });
        }
        /// <summary>
        /// 未完待续
        /// </summary>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="star_date"></param>
        /// <param name="end_date"></param>
        /// <param name="schoolcode"></param>
        /// <param name="payment_id"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPaymentRecord(string iDisplayStart,string iDisplayLength,string star_date,string end_date,string schoolcode,string payment_id,string department)
        {
            int start = int.Parse(iDisplayStart.ToString());
            start++;
            int end = int.Parse(iDisplayLength.ToString());
            end += start - 1;
            int count = 0;
            //SELECT* FROM tb_payment_record a
            //    INNER JOIN tb_payment_item b ON a.payment_id = b.id
            //    INNER JOIN tb_school_user c ON b.schoolcode = c.school_id  AND a.passport = c.passport
            //    WHERE b.schoolcode = '10000'
            //    AND a.status = 1
            //    AND pay_time between '2011-12-26' and  '2018-12-28'
            //    AND payment_id in (2)and c.department like '%外语系%'
            List<PaymentRecord> lst = _tb_payment_recordService.GetPaymentRecord(start,end,schoolcode, payment_id, department, star_date, end_date,out count);

            return Json(new {
                return_code = 0,
                return_msg = "查询成功",
                iTotalRecords = lst.Count,
                iTotalDisplayRecords = count,
                aaData = lst
            });
        }
    }
}