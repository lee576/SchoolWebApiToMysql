﻿using DbModel;
using IService;
using Infrastructure.Service;
using Infrastructure;
using SqlSugar;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text;
using Models.ViewModels;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Service
{
    public class tb_payment_recordService : GenericService<tb_payment_record>, Itb_payment_recordService
    {
        public (List<string> paymentList, List<string> arList, List<payment_Orders> orderList) createOrder(string payment_id, string ar_id, string alipayOrderNo, string money, string payer_id,
                                                                                string student_id, string payer_name, string passport, string phone)
        {
            List<string> paymentList = new List<string>();
            List<string> arList = new List<string>();
            List<payment_Orders> orderList = new List<payment_Orders>();
            if (string.IsNullOrWhiteSpace(payment_id) && string.IsNullOrWhiteSpace(ar_id))
                return (paymentList, arList, orderList);

            StringBuilder sb1 = new StringBuilder();
            sb1.Append(@" SELECT id,
                                count FROM tb_payment_record_count ");
            sb1.Append(@" WHERE id=@id ");

            StringBuilder sb2 = new StringBuilder();


            using (var db = DbFactory.GetSqlSugarClient())
            {
                try
                {
                    db.Ado.BeginTran(System.Data.IsolationLevel.ReadCommitted);
                    string orderNo;
                    int count = 1;
                    StringBuilder sb = new StringBuilder();
                    if (!string.IsNullOrWhiteSpace(payment_id))
                    {
                        string[] paymentIds = payment_id.Split(',');
                        foreach (int id in paymentIds.Select(x => int.Parse(x)))
                        {
                            sb.Clear();
                            List<tb_payment_record_count> lst = db.Queryable<tb_payment_record_count>()
                                            .Where(x => x.id.Equals(id)).ToList();
                            if (lst.Count <= 0)
                            {
                                tb_payment_record_count modelInsert = new tb_payment_record_count()
                                {
                                    id = id,
                                    count = 1
                                };
                                db.Insertable<tb_payment_record_count>(modelInsert).ExecuteCommand();
                                count = 1;
                            }
                            else
                            {
                                lst[0].count += 1;
                                db.Updateable(lst[0]).ExecuteCommand();
                                count += 1;
                            }
                            orderNo = DateTime.Now.ToString("yyyyMMddHHmmssffff") + id + count;
                            int intRowCount = 0;
                            if (paymentIds.Length > 0)
                            {
                                //根据传入的payment_id获取价格
                                List<tb_payment_item> lstPayment = db.Queryable<tb_payment_item>().Where(x => x.id.Equals(id)).ToList();

                                tb_payment_item modelEdit = db.Queryable<tb_payment_item>().Single(x => x.id.Equals(id));

                                //获取订单信息
                                #region 保留订单的名称等信息
                                List<payment_Orders> lstOrders = db.Queryable<tb_payment_item, tb_payment_accounts>(
                                    (pi, pa1) =>
                                    new object[] {
                                        JoinType.Inner,pi.schoolcode==pa1.schoolcode && pi.account==pa1.id.ToString()
                                })
                                .Where((pi, pa1) => pi.id == modelEdit.id)
                                .Select((pi, pa1) => new payment_Orders()
                                {
                                    PrivateKey = pa1.private_key,
                                    OrderName = pi.name,
                                    Price = pi.money.ToString(),
                                    AlipayPublicKey = pa1.alipay_public_key,
                                    AppId = pa1.appid
                                }).ToList();
                                List<payment_Orders> lstOrdersOld = orderList.Where(x => x.PrivateKey == lstOrders[0].PrivateKey).ToList();
                                //收款账号相同的同一次付款使用同一个订单号
                                lstOrders[0].OrderNo = alipayOrderNo;
                                //orderNo = lstOrders[0].OrderNo;
                                orderList.Add(lstOrders[0]);
                                #endregion

                                db.Insertable<tb_payment_record>(new tb_payment_record
                                {
                                    order_no = orderNo,
                                    out_order_no = alipayOrderNo,
                                    payment_id = id,
                                    total_amount = lstPayment[0].money,
                                    receipt_amount = lstPayment[0].money,
                                    pay_amount = lstPayment[0].money,
                                    refund_amount = 0,
                                    payer_id = payer_id,
                                    pay_time = DateTime.Now,
                                    status = 0,
                                    student_id = student_id,
                                    pay_name = payer_name,
                                    passport = passport,
                                    phone = phone
                                }).ExecuteCommand();
                            }
                            else
                            {
                                //tb_payment_item
                                tb_payment_item modelEdit = db.Queryable<tb_payment_item>().Single(x => x.id.Equals(id));

                                //获取订单信息
                                #region 保留订单的名称等信息
                                List<payment_Orders> lstOrders = db.Queryable<tb_payment_item, tb_payment_accounts>(
                                    (pi, pa) =>
                                    new object[] {
                                        JoinType.Inner,pi.schoolcode==pa.schoolcode
                                })
                                .Where((pi, pa) => pi.id == modelEdit.id)
                                .Select((pi, pa) => new payment_Orders()
                                {
                                    PrivateKey = pa.private_key,
                                    OrderName = pi.name,
                                    Price = pi.money.ToString(),
                                    AlipayPublicKey = pa.alipay_public_key
                                }).ToList();
                                List<payment_Orders> lstOrdersOld = orderList.Where(x => x.PrivateKey == lstOrders[0].PrivateKey).ToList();
                                //收款账号相同的同一次付款使用同一个订单号
                                //if (lstOrdersOld.Count() <= 0)
                                //    lstOrders[0].OrderNo = orderNo;
                                //else
                                //    lstOrders[0].OrderNo = lstOrdersOld[0].OrderNo;
                                lstOrders[0].OrderNo = alipayOrderNo;
                                orderList.Add(lstOrders[0]);
                                //orderNo = lstOrders[0].OrderNo;
                                #endregion

                                tb_payment_record modelInsert = new tb_payment_record()
                                {
                                    out_order_no = alipayOrderNo,
                                    order_no = orderNo,
                                    payment_id = id,
                                    total_amount = modelEdit.money,
                                    receipt_amount = modelEdit.money,
                                    pay_amount = modelEdit.money,
                                    refund_amount = 0,
                                    payer_id = payer_id,
                                    pay_time = DateTime.Now,
                                    status = 0,
                                    student_id = student_id,
                                    pay_name = payer_name,
                                    passport = passport,
                                    phone = phone
                                };
                                intRowCount = db.Updateable(modelEdit).ExecuteCommand();
                            }
                            tb_payment_alipay_record model = new tb_payment_alipay_record()
                            {
                                alipay_order = alipayOrderNo,
                                order = orderNo,
                                type = byte.Parse("0"),
                                create_time = DateTime.Now,
                                schoolcode = student_id,
                            };
                            db.Insertable(model).ExecuteCommand();
                            paymentList.Add(id.ToString());
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(ar_id))
                    {
                        foreach (var id in ar_id.Split(',').Select(x => int.Parse(x)))
                        {
                            sb.Clear();
                            List<tb_payment_record_count> lst = db.Queryable<tb_payment_record_count>()
                                            .Where(x => x.id.Equals(id)).ToList();
                            if (lst.Count <= 0)
                            {
                                tb_payment_record_count modelInsert = new tb_payment_record_count()
                                {
                                    id = id,
                                    count = 1
                                };
                                db.Insertable<tb_payment_record_count>(modelInsert).ExecuteCommand();
                                count = 1;
                            }
                            else
                            {
                                lst[0].count += 1;
                                db.Updateable(lst[0]).ExecuteCommand();
                                count += 1;
                            }

                            orderNo = DateTime.Now.ToString("yyyyMMddHHmmssffff") + id + count;
                            tb_payment_ar_record modelInsertRecord = new tb_payment_ar_record()
                            {
                                ar_id = int.Parse(ar_id),
                                out_trade_no = alipayOrderNo,
                                payer_passport = passport,
                                pay_amount = decimal.Parse(money),
                                status = byte.Parse("0"),
                                pay_time = DateTime.Now
                            };
                            db.Insertable(modelInsertRecord).ExecuteCommand();

                            tb_payment_alipay_record model = new tb_payment_alipay_record()
                            {
                                alipay_order = alipayOrderNo,
                                order = orderNo,
                                type = byte.Parse("0"),
                                create_time = DateTime.Now,
                                schoolcode = student_id,
                            };
                            db.Insertable(model).ExecuteCommand();
                            arList.Add(id.ToString());
                        }
                    }
                    db.Ado.CommitTran();
                    return (paymentList, arList, orderList);
                }
                catch (Exception)
                {
                    db.Ado.RollbackTran();
                    return (null, null, null);
                }
            }
        }

        public int FindRecordByPayId(string payment_id, string passport)
        {
            int intResult = 0;
            string[] PaymentId = payment_id.Split(',');
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var pageResult = db.Queryable<tb_payment_record, tb_payment_item>(
                                        (pr, pi) =>
                                        new object[] {
                                            JoinType.Left,pr.payment_id == pi.id
                                    })
                                    .Where((pr, pi) =>
                                        pr.status == 1
                                        && PaymentId.Contains(pr.payment_id.ToString())
                                        && pr.passport == passport
                                    )
                                    .GroupBy((pr, pi) =>
                                        new { pr.payment_id, pi.limit }
                                    )
                                    .Select((pr, pi) => new { pr.payment_id, pi.limit });
                var pageList = pageResult.ToList();
                intResult = pageList.Count;
                intResult = pageList.Count(x => x.limit != 0 && x.limit <= intResult);
            }
            return intResult;
        }
        /// <summary>
        /// 处理订单表中未完成状态的订单数据
        /// </summary>
        /// <returns></returns>
        public void EditOutOrdersStatus(string SelectedDay)
        {
            //string strSQL = " select * from tb_payment_record where status=0 ";
            StringBuilder sbPaymentRecord = new StringBuilder();
            sbPaymentRecord.Append(@" select id,
                                order_no,
                                out_order_no,
                                payment_id,
                                total_amount,
                                receipt_amount,
                                pay_amount,
                                refund_amount,
                                payer_id,
                                pay_time,
                                status,
                                student_id,
                                pay_name,
                                passport,
                                phone ");
            sbPaymentRecord.Append(@" from tb_payment_record ");
            sbPaymentRecord.Append(@" where status=0 ");
            sbPaymentRecord.Append(@" and pay_time> CAST(CONVERT(varchar(100), DATEADD(DD,-" + SelectedDay.ToString() + ",getdate()), 23) AS DATETIME) ");
            StringBuilder sbSchool = new StringBuilder();
            sbSchool.Append(@" SELECT si.app_id,si.private_key,si.alipay_public_key,su.passport
                                   FROM dbo.tb_school_user su
                                   INNER JOIN dbo.tb_school_info si ON si.School_Code=su.school_id 
                                    where su.passport=@passport ");

            StringBuilder sbEditPaymentRecord = new StringBuilder();
            sbEditPaymentRecord.Append(" update tb_payment_record ");
            sbEditPaymentRecord.Append(" set status=@status ");
            sbEditPaymentRecord.Append(" where id=@id ");

            StringBuilder sbEditPaymentAlipayRecord = new StringBuilder();
            sbEditPaymentAlipayRecord.Append(" UPDATE tb_payment_alipay_record ");
            sbEditPaymentAlipayRecord.Append(" SET type=@Type ");
            sbEditPaymentAlipayRecord.Append(" WHERE alipay_order=@Order ");

            StringBuilder sbEditPaymentArRecord = new StringBuilder();
            sbEditPaymentArRecord.Append(" update tb_payment_ar_record ");
            sbEditPaymentArRecord.Append(" set status=@status ");
            sbEditPaymentArRecord.Append(" where out_trade_no in ( ");
            sbEditPaymentArRecord.Append(" SELECT out_trade_no FROM tb_payment_alipay_record ");
            sbEditPaymentArRecord.Append(" where create_time> CAST(CONVERT(varchar(100), DATEADD(DD,-" + SelectedDay.ToString() + " ,getdate()), 23) AS DATETIME) ");
            sbEditPaymentArRecord.Append(" and type=@status1 ");
            sbEditPaymentArRecord.Append(" ) ");

            StringBuilder sbEditPaymentAR = new StringBuilder();
            sbEditPaymentAR.Append(" update tb_payment_ar ");
            sbEditPaymentAR.Append(" set JSstatus=@status ");
            sbEditPaymentAR.Append(" where id IN( ");
            sbEditPaymentAR.Append(" SELECT ar_id FROM tb_payment_ar_record ");
            sbEditPaymentAR.Append(" WHERE pay_time>=CAST(CONVERT(varchar(100), DATEADD(DD,-" + SelectedDay.ToString() + " ,getdate()), 23) AS DATETIME) ");
            sbEditPaymentAR.Append(" AND status=@status1 ");
            sbEditPaymentAR.Append(" ) ");

            using (var db = DbFactory.GetSqlSugarClient())
            {
                try
                {
                    db.Ado.BeginTran(System.Data.IsolationLevel.ReadCommitted);
                    ////获取学校的AppId,私钥、公钥、身份证号等
                    DataTable dtPaymentRecord = db.Ado.GetDataTable(sbPaymentRecord.ToString());
                    if (dtPaymentRecord.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtPaymentRecord.Rows)
                        {
                            //获取学校的AppId,私钥、公钥、身份证号等
                            DataTable dt1 = db.Ado.GetDataTable(sbSchool.ToString(), new SugarParameter("@passport", item["passport"].ToString()));
                            if (dt1.Rows.Count > 0)
                            {
                                if (GetOrdersStatus(item["out_order_no"].ToString()
                                                        , dt1.Rows[0]["app_id"].ToString()
                                                        , dt1.Rows[0]["private_key"].ToString()
                                                        , dt1.Rows[0]["alipay_public_key"].ToString()))
                                {
                                    //已经支付成功,需要修改状态
                                    #region tb_payment_record
                                    db.Ado.ExecuteCommand(sbEditPaymentRecord.ToString(), new SugarParameter("@status", 1), new SugarParameter("@id", item["id"].ToString()));
                                    #endregion
                                    #region tb_payment_alipay_record
                                    db.Ado.ExecuteCommand(sbEditPaymentAlipayRecord.ToString(), new SugarParameter("@Type", 1), new SugarParameter("@Order", item["out_order_no"].ToString()));
                                    #endregion
                                    #region tb_payment_ar_record
                                    db.Ado.ExecuteCommand(sbEditPaymentArRecord.ToString(), new SugarParameter("@status", 1), new SugarParameter("@status1", 1));
                                    #endregion
                                    #region tb_payment_ar
                                    db.Ado.ExecuteCommand(sbEditPaymentAR.ToString(), new SugarParameter("@status", 1), new SugarParameter("@status1", 1));
                                    #endregion
                                }
                            }
                        }
                    }
                    db.Ado.CommitTran();
                }
                catch (Exception)
                {
                    db.Ado.RollbackTran();
                }
                db.Ado.Close();
            }
        }

        private bool GetOrdersStatus(string out_trade_no,string appId,string privateKey,string alipayPublicKey)
        {
            //通过应缴费用缴费ID获取订单号、appid、私钥、公钥、
            string Url = "https://openapi.alipay.com/gateway.do";
            IAopClient client = new DefaultAopClient(Url, appId, privateKey, "json", "1.0", "RSA2", alipayPublicKey);
            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
            request.BizContent = "{" +
            "\"out_trade_no\":\"" + out_trade_no + "\"" +
            "  }";
            AlipayTradeQueryResponse response = client.Execute(request);

            if (((JObject)JsonConvert.DeserializeObject(response.Body))["alipay_trade_query_response"]["trade_status"].ToString().ToUpper() == "TRADE_SUCCESS")
                return true;
            return false;
        }

        public List<PaymentRecord> GetPaymentRecord(int start, int end, string schoolcode,string payment_id,string department, string star_date, string end_date,out int intCount)
        {
            List<PaymentRecord> lst = new List<PaymentRecord>();
            string[] strDepartment = department.Split('/');
            intCount = 0;
            using (var db = DbFactory.GetSqlSugarClient())
            {
                lst = db.Queryable<tb_payment_record, tb_payment_item, tb_school_user>(
                    (a, b, c) => new object[] {
                        JoinType.Inner,a.payment_id == b.id,
                        JoinType.Inner,a.passport == c.passport,
                        JoinType.Inner,b.schoolcode == c.school_id
                    })
                    .WhereIF(string.IsNullOrWhiteSpace(payment_id), (a, b, c) => a.payment_id.ToString() == payment_id)
                    .WhereIF(string.IsNullOrWhiteSpace(department), (a, b, c) => strDepartment.Contains(c.department))
                    .Where((a, b, c) => b.schoolcode == schoolcode && a.status == 1)
                    .Where((a, b, c) => a.pay_time >= DateTime.Parse(star_date + " 00:00:00") && a.pay_time <= DateTime.Parse(end_date + " 23:59:59")).Select((a,b,c)=>new PaymentRecord {
                        name=b.name,
                        pay_amount=a.pay_amount,
                        student_id=a.student_id,
                        pay_name=a.pay_name,
                        phone=a.phone,
                        pay_time=a.pay_time,
                        order_no=a.order_no
                    }).ToList();
                intCount = lst.Count;
                for (int i = start; i < end; i++)
                {
                    if (lst[i] != null)
                        lst[i].rn = i + 1;
                }
            }
            return lst;
        }
    }
}