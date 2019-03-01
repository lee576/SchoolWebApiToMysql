﻿using DbModel;
using Infrastructure;
using Infrastructure.Service;
using IService;
using Models.ViewModels;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class PaymentItemService : GenericService<Payment_ARInfo>, IPaymentItemService
    {
        public IEnumerable<Payment_ARInfo> GetPaymentItemInfo(string schoolcode, string startDateTime)
        {
            string endTime = Convert.ToDateTime(startDateTime).AddDays(1).ToString("yyyy-MM-dd");
            //string sql = @"select ROW_NUMBER() over(order by t.id desc)rn,t.order_no,t.out_order_no,tpi.name,
            //                t.total_amount, t.pay_amount, t.receipt_amount, t.refund_amount,CONVERT(varchar(100),t.pay_time,23) as pay_time,t.status,t.student_id,t.passport,t.pay_name
            //                from tb_payment_record t,tb_payment_item tpi where t.payment_id = tpi.id   and pay_time between '" + startDateTime + "' and '" + endTime + "'  and tpi.schoolcode = '" + schoolcode + "' ";
            string sql = @"SELECT t.order_no,t.out_order_no,tpi. NAME,t.total_amount,t.pay_amount,t.receipt_amount,t.refund_amount,date(t.pay_time) AS pay_time,t. STATUS,t.student_id,t.passport,t.pay_name
                           FROM tb_payment_record t,tb_payment_item tpi WHERE t.payment_id = tpi.id AND pay_time BETWEEN '" + startDateTime + "' AND '" + endTime + "' AND tpi.schoolcode = '" + schoolcode + "'";
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var dt = db.Ado.SqlQuery<Payment_ARInfo>(sql);
                return dt;
            }
        }
        public List<PayRecord> GetPayRecord(string schoolcode,string studentid,string time)
        {
            string sql = string.Format(@"
                            SELECT
	                            *
                            FROM
	                            (
		                            SELECT
			                            t.out_order_no,
			                            tpi.`NAME`,
			                            t.receipt_amount,
			                            t.pay_time AS pay_time,
			                            t.student_id,
			                            su.passport
		                            FROM
			                            tb_payment_record t,
			                            tb_payment_item tpi,
			                            tb_school_user su
		                            WHERE
			                            t.payment_id = tpi.id
		                            AND su.student_id = t.student_id
		                            AND tpi.schoolcode = '{0}'
	                            ) tt
                            WHERE
	                            DATE_FORMAT(tt.pay_time, '%Y-%m') = '{1}'
                            AND tt.student_id = '{2}'
                            union all
                            SELECT
	                            *
                            FROM
	                            (
		                            SELECT
			                            t.out_trade_no AS out_order_no,
			                            tpi.`NAME`,
			                            t.pay_amount AS receipt_amount,
			                            t.pay_time AS pay_time,
			                            su.student_id,
                            su.passport
		                            FROM
			                            tb_payment_ar tpi,
			                            tb_payment_ar_record t,
			                            tb_school_user su
		                            WHERE
			                            t.ar_id = tpi.id
		                            AND su.passport = t.payer_passport
		                            AND tpi.schoolcode = '{0}'
	                            ) tt
                            WHERE
	                            DATE_FORMAT(tt.pay_time, '%Y-%m') = '{1}'
                            AND tt.student_id = '{2}';", schoolcode, time, studentid);
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var Result = db.Ado.SqlQuery<PayRecord>(sql);
                return Result;
            }
        }
        public IEnumerable<Payment_item> GetPaymentItemToStudentId(string schoolcode, string studentid, string time)
        {
            //            string sql = string.Format(@"select * from
            //(select ROW_NUMBER() over(order by t.id desc)rn,t.order_no,t.out_order_no,tpi.name,
            //t.total_amount, t.pay_amount, t.receipt_amount, t.refund_amount,CONVERT(varchar(100),t.pay_time,23) as pay_time,t.status,t.student_id,t.passport,t.pay_name,t.phone,tpi.icon 
            //from tb_payment_record t,tb_payment_item tpi where t.payment_id = tpi.id   and tpi.schoolcode = '{0}' )tt
            //where DATENAME(yy, tt.pay_time) = '{1}' and tt.student_id='{2}'", schoolcode, time, studentid);
            string sql = string.Format(@"SELECT
	                            *
                            FROM
	                            (
		                            SELECT
			                            t.order_no,
			                            t.out_order_no,
			                            tpi.`NAME`,
			                            t.total_amount,
			                            t.pay_amount,
			                            t.receipt_amount,
			                            t.refund_amount,
			
			                            date(t.pay_time)as pay_time,
			
			                            t.`STATUS`,
			                            t.student_id,
			                            t.passport,
			                            t.pay_name,
			                            t.phone,
			                            tpi.icon
		                            FROM
			                            tb_payment_record t,
			                            tb_payment_item tpi
		                            WHERE
			                            t.payment_id = tpi.id
		                            AND tpi.schoolcode = '{0}'
	                            ) tt
                            WHERE
                            DATE_FORMAT(tt.pay_time,'%Y-%m')= '{1}'
	
                            AND tt.student_id = '{2}'", schoolcode, time, studentid);
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var dt = db.Ado.SqlQuery<Payment_item>(sql);
                return dt;
            }
        }
        public List<Paymentlist> GetPaymentlist(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode, string jfName = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                string where = "";
                if (!string.IsNullOrEmpty(jfName))
                {
                    where = " and t.name like '" + jfName + "%'";
                }
                string sql = @"SELECT
	                t.id,
	                t.`NAME`,
	                t.is_external,
	                t.type,
	                t.target,
	                t.fixed,
	                t.money,
	                t.introduction,
	                t.icon,
	                t.`GROUP`, t.method,
	                t.can_set_count,
	                t.date_to AS dateto,
	                t.date_from AS date_from,
	                t.nessary_info,
                    t.status as isstop,
	                tpt.`NAME` AS tname,
	                ta.`NAME` accountname,
	                ta.pid account,
	                CASE
                WHEN date_to < now() THEN
	                1
                ELSE
	                0
                END STATUS,
                 CASE
                WHEN pr.count IS NULL THEN
	                0
                ELSE
	                pr.count
                END count,
                 t.notify_link,
                 t.notify_key,
                 t.notify_msg,
                 t.remark,
                 t.`LIMIT`,
                 t.class_id,
                 t.status as paystatus
                FROM
	                tb_payment_item t
                LEFT JOIN tb_payment_accounts ta ON t.account = ta.id
                LEFT JOIN tb_payment_type tpt ON t.type = tpt.id
                LEFT JOIN (
	                SELECT
		                count(payment_id) count,
		                payment_id
	                FROM
		                tb_payment_record
	                WHERE
		                STATUS = 1
	                GROUP BY
		                payment_id
                ) pr ON t.id = pr.payment_id
                WHERE
	                is_external =0
                and t.schoolcode='" + schoolCode + "'" + where;
                var dt = db.Ado.SqlQuery<Paymentlist>(sql) as List<Paymentlist>;
                total = dt.Count();
                var data = dt.Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
                return data;
                //return db.Queryable<tb_payment_item, tb_payment_accounts, tb_payment_type, tb_payment_record>(
                //    (t, ta, tpt, pr) =>
                //    new object[] {
                //         JoinType.Left,t.account == SqlFunc.ToString(ta.id),
                //        JoinType.Left,t.type==tpt.id,
                //        JoinType.Left,t.id == pr.payment_id
                //    }
                //)
                //.WhereIF(!string.IsNullOrWhiteSpace(jfName), (t, ta, tpt, pr) => t.name.Contains(jfName))
                //.Where((t, ta, tpt, pr) => t.schoolcode == schoolCode && t.is_external == 0 && pr.status == 1)
                //.Select((t, ta, tpt, pr) => new Paymentlist
                //{
                //    id = SqlFunc.ToString(t.id),
                //    name = t.name,
                //    is_external = SqlFunc.ToString(t.is_external),
                //    type = SqlFunc.ToString(t.type),
                //    target = SqlFunc.ToString(t.target),
                //    @fixed = SqlFunc.ToString(t.@fixed),
                //    money = SqlFunc.ToString(t.money),
                //    introduction = t.introduction,
                //    icon = SqlFunc.ToString(t.icon),
                //    group = SqlFunc.ToString(t.group),
                //    method = SqlFunc.ToString(t.method),
                //    can_set_count = SqlFunc.ToString(t.can_set_count),
                //    dateto = t.date_to,
                //    date_from = t.date_from,
                //    nessary_info = t.nessary_info,
                //    tname = tpt.name,
                //    accountname = ta.name,
                //    account = ta.pid,
                //    status = SqlFunc.IIF(t.date_to < DateTime.Now.Date, "1", "0"),
                //    count = SqlFunc.AggregateCount(t.id),
                //    notify_link = t.notify_link,
                //    notify_key = t.notify_key,
                //    notify_msg = t.notify_msg,
                //    remark = t.remark,
                //    limit = SqlFunc.ToString(t.limit)
                //})
                //.GroupBy(@"id,
	               //     name,
	               //     is_external,
	               //     type,
	               //     target,
	               //     fixed,
	               //     money,
	               //     introduction,
	               //     icon,
	               //     `group`,
	               //     method,
	               //     can_set_count,
	               //     dateto,
	               //     date_from,
	               //     nessary_info,
	               //     tname,
	               //     accountname,
	               //     account,
	               //     `status`,
	               //     count,
	               //     notify_link,
	               //     notify_key,
	               //     notify_msg,
	               //     remark,
	               //     `limit`")
                //.ToPageList(pageIndex, pageSize, ref total);
            }
        }
        public List<Paymentlist> GetPayment_item(string schoolCode, int id)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                return db.Queryable<tb_payment_item, tb_payment_accounts, tb_payment_type, tb_payment_record>(
                    (t, ta, tpt, pr) =>
                    new object[] {
                         JoinType.Left,t.account == SqlFunc.ToString(ta.id),
                        JoinType.Left,t.type==tpt.id,
                        JoinType.Left,t.id == pr.payment_id
                    }
                )
                .Where((t, ta, tpt, pr) => t.schoolcode == schoolCode && t.id == id && pr.status == 1)
                .Select((t, ta, tpt, pr) => new Paymentlist
                {
                    id = SqlFunc.ToString(t.id),
                    name = t.name,
                    is_external = SqlFunc.ToString(t.is_external),
                    type = SqlFunc.ToString(t.type),
                    target = SqlFunc.ToString(t.target),
                    @fixed = SqlFunc.ToString(t.@fixed),
                    money = SqlFunc.ToString(t.money),
                    introduction = t.introduction,
                    icon = SqlFunc.ToString(t.icon),
                    group = SqlFunc.ToString(t.group),
                    method = SqlFunc.ToString(t.method),
                    can_set_count = SqlFunc.ToString(t.can_set_count),
                    dateto = t.date_to,
                    date_from = t.date_from,
                    nessary_info = t.nessary_info,
                    tname = tpt.name,
                    accountname = ta.name,
                    account = ta.pid,
                    status = SqlFunc.IIF(t.date_to < DateTime.Now.Date, "1", "0"),
                    count = SqlFunc.AggregateCount(t.id),
                    notify_link = t.notify_link,
                    notify_key = t.notify_key,
                    notify_msg = t.notify_msg,
                    remark = t.remark,
                    limit = SqlFunc.ToString(t.limit)
                })
                .GroupBy(@"id,
	                    name,
	                    is_external,
	                    type,
	                    target,
	                    fixed,
	                    money,
	                    introduction,
	                    icon,
	                    `group`,
	                    method,
	                    can_set_count,
	                    dateto,
	                    date_from,
	                    nessary_info,
	                    tname,
	                    accountname,
	                    account,
	                    `status`,
	                    count,
	                    notify_link,
	                    notify_key,
	                    notify_msg,
	                    remark,
	                    `limit`")
                .ToList();
            }
        }
        public List<tb_payment_record> Getpayment_itemInfoList(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode, int id, string stime = "", string etime = "", string selectinfo = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                DateTime Stime = new DateTime();
                DateTime Etime = new DateTime();

                if (!string.IsNullOrWhiteSpace(stime) && !string.IsNullOrWhiteSpace(etime))
                {
                    Stime = Convert.ToDateTime(stime + " 00:00:00");
                    Etime = Convert.ToDateTime(etime + " 23:59:59");
                }
                
                return db.Queryable<tb_payment_item, tb_payment_record>(
                    (pi, pr) =>
                    new object[] {
                        JoinType.Left,pi.id == pr.payment_id
                    }
                )
                .WhereIF(!string.IsNullOrWhiteSpace(stime) && !string.IsNullOrWhiteSpace(etime), (pi, pr) => pr.pay_time >= Stime && pr.pay_time <= Etime)
                .WhereIF(!string.IsNullOrWhiteSpace(selectinfo), (pi, pr) => pr.out_order_no == selectinfo || pr.phone == selectinfo || pr.pay_name == selectinfo)
                .Where((pi, pr) => pr.status == 1 && pr.payment_id == id && pi.schoolcode == schoolCode)
                .Select((pi, pr) => pr)
                .ToPageList(pageIndex, pageSize, ref total);

            }
        }
        public string GetPaymentInfoSearch(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode, string selectinfo = "",
            string sTime = "", string eTime = "", string tb_payment_sub_adminItem = "", string classID = "",string status="")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                if (!string.IsNullOrWhiteSpace(sTime) && !string.IsNullOrWhiteSpace(eTime))
                {
                    sTime += " 00:00:00";
                    eTime += " 23:59:59";
                }
                return JsonConvert.SerializeObject(
                    db.Queryable<tb_payment_item, tb_payment_record, tb_school_user>(
                    (pi, pr, su) =>
                    new object[] {
                        JoinType.Left,pi.id == pr.payment_id,
                        JoinType.Left,pr.passport == su.passport
                    }
                    )
                    .WhereIF(!string.IsNullOrWhiteSpace(selectinfo), (pi, pr, su) => pr.student_id == selectinfo || pr.out_order_no == selectinfo || pr.pay_name == selectinfo)
                    .WhereIF(!string.IsNullOrWhiteSpace(sTime) && !string.IsNullOrWhiteSpace(eTime), (pi, pr, su) => pr.pay_time >= SqlFunc.ToDate(sTime) && pr.pay_time <= SqlFunc.ToDate(eTime))
                    .WhereIF(!string.IsNullOrWhiteSpace(tb_payment_sub_adminItem), (pi, pr, su) => pr.payment_id == SqlFunc.ToInt32(tb_payment_sub_adminItem))
                    .WhereIF(!string.IsNullOrWhiteSpace(classID), (pi, pr, su) => su.department_id == SqlFunc.ToInt32(classID))
                    .WhereIF(!string.IsNullOrWhiteSpace(status), (pi, pr, su) => pr.status == SqlFunc.ToInt32(status))
                    .Where((pi, pr, su) => pi.schoolcode == schoolCode)
                    .Select((pi, pr, su) => new
                    {
                        order_no = pr.order_no,
                        out_order_no = pr.out_order_no,
                        name = pi.name,
                        total_amount = pr.total_amount,
                        pay_amount = pr.pay_amount,
                        receipt_amount = pr.receipt_amount,
                        refund_amount = pr.refund_amount,
                        pay_time = pr.pay_time,
                        status = pr.status,
                        student_id = pr.student_id,
                        passport = pr.passport,
                        pay_name = pr.pay_name,
                        phone = pr.phone
                    })
                    .ToPageList(pageIndex, pageSize, ref total)
                );

            }
        }
        public List<PaymentRecord2> GetPaymentInfoSearchToExcel(string schoolCode, string selectinfo = "",
            string sTime = "", string eTime = "", string tb_payment_sub_adminItem = "", string classID = "",string status = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                if (!string.IsNullOrWhiteSpace(sTime) && !string.IsNullOrWhiteSpace(eTime))
                {
                    sTime += " 00:00:00";
                    eTime += " 23:59:59";
                }
                return db.Queryable<tb_payment_item, tb_payment_record, tb_school_user>(
                    (pi, pr, su) =>
                    new object[] {
                        JoinType.Left,pi.id == pr.payment_id,
                        JoinType.Left,pr.passport == su.passport
                    }
                    )
                    .WhereIF(!string.IsNullOrWhiteSpace(selectinfo), (pi, pr, su) => pr.student_id == selectinfo || pr.out_order_no == selectinfo || pr.pay_name == selectinfo)
                    .WhereIF(!string.IsNullOrWhiteSpace(sTime) && !string.IsNullOrWhiteSpace(eTime), (pi, pr, su) => pr.pay_time >= SqlFunc.ToDate(sTime) && pr.pay_time <= SqlFunc.ToDate(eTime))
                    .WhereIF(!string.IsNullOrWhiteSpace(tb_payment_sub_adminItem), (pi, pr, su) => pr.payment_id == SqlFunc.ToInt32(tb_payment_sub_adminItem))
                    .WhereIF(!string.IsNullOrWhiteSpace(classID), (pi, pr, su) => su.department_id == SqlFunc.ToInt32(classID))
                    .WhereIF(!string.IsNullOrWhiteSpace(status), (pi, pr, su) => pr.status == SqlFunc.ToInt32(status))
                    .Where((pi, pr, su) => pi.schoolcode == schoolCode)
                    .Select((pi, pr, su) => new PaymentRecord2
                    {
                        order_no = pr.order_no,
                        out_order_no = pr.out_order_no,
                        name = pi.name,
                        total_amount = pr.total_amount,
                        pay_amount = pr.pay_amount,
                        receipt_amount = pr.receipt_amount,
                        refund_amount = pr.refund_amount,
                        pay_time = pr.pay_time,
                        status = SqlFunc.IIF(pr.status==1, "'已缴费'", "'未缴费'"),
                        student_id = pr.student_id,
                        passport = pr.passport,
                        pay_name = pr.pay_name,
                        phone = pr.phone
                        
                    }).ToList();
            }
        }
        public PaymentItmeTotle GetPaymentInfoTotle(string schoolCode, string selectinfo = "",
    string sTime = "", string eTime = "", string tb_payment_sub_adminItem = "", string classID = "",string status = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                if (!string.IsNullOrWhiteSpace(sTime) && !string.IsNullOrWhiteSpace(eTime))
                {
                    sTime += " 00:00:00";
                    eTime += " 23:59:59";
                }
                //return JsonConvert.SerializeObject(
                var data = db.Queryable<tb_payment_item, tb_payment_record, tb_school_user>(
                (pi, pr, su) =>
                new object[] {
                        JoinType.Left,pi.id == pr.payment_id,
                        JoinType.Left,pr.passport == su.passport
                }
                )
                .WhereIF(!string.IsNullOrWhiteSpace(selectinfo), (pi, pr, su) => pr.student_id == selectinfo || pr.order_no == selectinfo || pr.pay_name == selectinfo)
                .WhereIF(!string.IsNullOrWhiteSpace(sTime) && !string.IsNullOrWhiteSpace(eTime), (pi, pr, su) => pr.pay_time >= SqlFunc.ToDate(sTime) && pr.pay_time <= SqlFunc.ToDate(eTime))
                .WhereIF(!string.IsNullOrWhiteSpace(tb_payment_sub_adminItem), (pi, pr, su) => pr.payment_id == SqlFunc.ToInt32(tb_payment_sub_adminItem))
                .WhereIF(!string.IsNullOrWhiteSpace(classID), (pi, pr, su) => su.department_id == SqlFunc.ToInt32(classID))
                 //.WhereIF(!string.IsNullOrWhiteSpace(status), (pi, pr, su) => pr.status == SqlFunc.ToInt32(status))
                 //.Where((pi, pr, su) => pi.schoolcode == schoolCode && pr.status == 1)
                 .Where((pi, pr, su) => pi.schoolcode == schoolCode)
                .Select((pi, pr, su) => pr

                ).ToList();
                //);
                PaymentItmeTotle pt = new PaymentItmeTotle();
                pt.sj_count = data.Where(x => x.status == 1).ToList().Count();
                pt.pay_amountTotle = data.Where(x => x.status == 1).Sum(x => x.pay_amount);
                pt.dj_count = data.Where(x => x.status != 1).ToList().Count();
                pt.item_count = data.Count();
                return pt;
            }
        }
    }
}