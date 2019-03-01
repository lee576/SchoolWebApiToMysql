﻿using DbModel;
using Infrastructure;
using Infrastructure.Service;
using IService;
using Models.ViewModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Service
{
    public class tb_payment_ARService : GenericService<tb_payment_ar>, Itb_payment_ARService
    {
        public static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        public IEnumerable<tb_payment_ar> ConstraintARIDandPassPort(List<tb_payment_ar> list)
        {
            //using (var db = DbFactory.GetSqlSugarClient())
            //{

            //    db.Ado.CommandTimeOut = 30000;//设置超时时间
            //    //List<tb_payment_ar> list = new List<tb_payment_ar>();
            //    try
            //    {
            //        //db.BeginTran();//开启事务
            //        //特别说明：在事务中，默认情况下是使用锁的，也就是说在当前事务没有结束前，其他的任何查询都需要等待
            //        //ReadCommitted：在正在读取数据时保持共享锁，以避免脏读，但是在事务结束之前可以更改数据，从而导致不可重复的读取或幻像数据。
            //        db.Ado.BeginTran(System.Data.IsolationLevel.ReadCommitted); //重载指定事务的级别
            //        //特别说明：在事务操作中，对于自增长列的表，插入成功，又回滚的会占据一次自增长值
            //        foreach (var item in list)
            //        {
            //            var data = db.Insertable(item).ExecuteReturnEntity();
            //            //Console.WriteLine(id1);
            //            //throw new Exception("事务执行异常");
            //            //提交事务
            //            db.Ado.CommitTran();
            //        }
            //        return list;
            //    }
            //    catch (Exception ex)
            //    {
            //        db.Ado.RollbackTran();//回滚
            //        return null;
            //        //throw ex;
            //    }
            //}
            try
            {
                Itb_payment_ARService _tb_payment_ARService = new tb_payment_ARService();
                foreach (var item in list)
                {
                    bool isOK = _tb_payment_ARService.Any(x => x.ARID == item.ARID && x.passport == item.passport && x.schoolcode == item.schoolcode);
                    if (isOK)//判断是否有重复数据
                    {
                        return null;
                    }
                    //加入数据
                    _tb_payment_ARService.Insert(item);
                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool AddPayment_ARInfo(tb_payment_ar payment)
        {
            try
            {
                Itb_payment_ARService _tb_payment_ARService = new tb_payment_ARService();

                bool isOK = _tb_payment_ARService.Any(x => x.ARID == payment.ARID && x.passport == payment.passport && x.schoolcode == payment.schoolcode);
                if (!isOK)//判断是否有重复数据
                {
                    _tb_payment_ARService.Insert(payment);
                }
                //加入数据
                return isOK;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 通过学生ID获取需要的缴费信息（若火缴费小程序使用（非缴费大厅））
        /// </summary>
        /// <param name="student_id"></param>
        /// <returns></returns>
        public List<CampusPay> FindCampusPayListByStudentId(string schoolcode, string student_id)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var pageResult = db.Queryable<tb_payment_ar, tb_school_user, tb_school_class>(
                                    (pa, su, sc) => new object[]{
                                        JoinType.Inner,pa.st_name == su.user_name && pa.passport == su.passport,
                                        JoinType.Inner,sc.id==su.class_id
                                    })
                                    .Where((pa, su, sc) =>
                                        su.student_id.ToString() == student_id.ToString()
                                        && pa.schoolcode.ToString() == schoolcode
                                        && pa.JSstatus == 0
                                    )
                                    .Select((pa, su, sc) => new CampusPay
                                    {
                                        id = pa.id,
                                        schoolcode = pa.schoolcode,
                                        ARID = pa.ARID,
                                        name = pa.name,
                                        amount = pa.amount,
                                        JSstatus = pa.JSstatus,
                                        status = pa.status,
                                        AR_account = pa.AR_account,
                                        star_date = pa.star_date,
                                        end_date = pa.end_date,
                                        st_name = pa.st_name,
                                        passport = pa.passport,
                                        fact_amount = pa.fact_amount,
                                        ClassName = sc.name,
                                        //Professional = sc.class_info,
                                        Professional = su.department,
                                        StartTime = pa.star_date,
                                        EndTime = pa.end_date
                                    });
                return pageResult.ToList();
            }
        }

        public List<tb_payment_ar> FindListByStudentId(string student_id)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var pageResult = db.Queryable<tb_payment_ar, tb_school_user>(
                                        (pa, su) =>
                                        new object[] {
                                            JoinType.Inner, pa.passport==su.passport && pa.schoolcode==su.school_id
                                    })
                                    .Where((pa, su) =>
                                        su.student_id == student_id
                                        && pa.JSstatus == 0
                                    )
                                    .Select((pa, su) => new tb_payment_ar
                                    {
                                        id = pa.id,
                                        schoolcode = pa.schoolcode,
                                        ARID = pa.ARID,
                                        name = pa.name,
                                        amount = pa.amount,
                                        JSstatus = pa.JSstatus,
                                        status = pa.status,
                                        AR_account = pa.AR_account,
                                        star_date = pa.star_date,
                                        end_date = pa.end_date,
                                        st_name = pa.st_name,
                                        passport = pa.passport,
                                        fact_amount = pa.fact_amount
                                    });
                return pageResult.ToList();
            }
        }
        /// <summary>
        /// 创建应缴费用的订单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="alipayOrderNo">阿里订单号</param>
        /// <returns></returns>
        public (List<string> paymentList, List<string> arList, List<payment_Orders> orderList) CreateOrder(tb_payment_ar model, string alipayOrderNo, string schoolCode)
        {
            var paymentList = new List<string>();
            var arList = new List<string>();
            var orderList = new List<payment_Orders>();
            if (model == null)
                return (paymentList, arList, orderList);

            #region
            using (var db = DbFactory.GetSqlSugarClient())
            {
                try
                {
                    db.Ado.BeginTran();
                    //操作
                    tb_payment_ar_record modelArRecord = new tb_payment_ar_record
                    {
                        ar_id = model.id,
                        out_trade_no = alipayOrderNo,
                        payer_passport = model.passport,
                        pay_amount = decimal.Parse(model.amount == null ? "0" : model.amount.ToString()),
                        refund_amount = 0,
                        status = 0,
                        pay_time = DateTime.Now
                    };
                    tb_payment_ar_recordService tparservice = new tb_payment_ar_recordService();
                    tb_payment_alipay_recordService tpalipayservice = new tb_payment_alipay_recordService();
                    int tb_payment_ar_record_id = Convert.ToInt32(tparservice.Insert(modelArRecord));
                    tb_payment_alipay_record modelalipay = new tb_payment_alipay_record();
                    modelalipay.alipay_order = alipayOrderNo;
                    modelalipay.order = alipayOrderNo;
                    modelalipay.type = 0;
                    modelalipay.create_time = DateTime.Now;
                    modelalipay.schoolcode = schoolCode;
                    tpalipayservice.Insert(modelalipay);
                    paymentList.Add(modelArRecord.id.ToString());
                    //tr.Commit();
                    db.Ado.CommitTran();
                    tb_payment_accountsService accountsService = new tb_payment_accountsService();
                    var data = accountsService.FindListByClause(x => x.schoolcode == schoolCode, t => t.id, OrderByType.Asc) as List<tb_payment_accounts>;
                    if (data.Count() > 0)
                    {
                        payment_Orders modelOrders = new payment_Orders()
                        {
                            PrivateKey = data[0].private_key,
                            OrderName = model.name,
                            Price = model.amount.ToString(),
                            AlipayPublicKey = data[0].alipay_public_key,
                            AppId = data[0].appid,
                            OrderNo = alipayOrderNo
                        };
                        orderList.Add(modelOrders);
                    }

                    return (paymentList, arList, orderList);
                }
                catch (Exception ex)
                {
                    log.Error("事务回滚:" + ex);
                    db.Ado.RollbackTran();
                    throw ex;
                }
            }

            //using (SqlConnection sqlcon = new SqlConnection(DbFactory.GetSqlSugarClient().Ado.Connection.ConnectionString))
            //{
            //    SqlTransaction tr = null;
            //    try
            //    {
            //        sqlcon.Open();
            //        tr = sqlcon.BeginTransaction();
            //        tb_payment_ar_record modelArRecord = new tb_payment_ar_record
            //        {
            //            ar_id = model.id,
            //            out_trade_no = alipayOrderNo,
            //            payer_passport = model.passport,
            //            pay_amount = decimal.Parse(model.amount == null ? "0" : model.amount.ToString()),
            //            refund_amount = 0,
            //            status = 0,
            //            pay_time = DateTime.Now
            //        };
            //        tb_payment_ar_recordService tparservice = new tb_payment_ar_recordService();
            //        tb_payment_alipay_recordService tpalipayservice = new tb_payment_alipay_recordService();
            //        int tb_payment_ar_record_id = Convert.ToInt32(tparservice.Insert(modelArRecord));
            //        tb_payment_alipay_record modelalipay = new tb_payment_alipay_record();
            //        modelalipay.alipay_order = alipayOrderNo;
            //        modelalipay.order = alipayOrderNo;
            //        modelalipay.type = 0;
            //        modelalipay.create_time = DateTime.Now;
            //        modelalipay.schoolcode = schoolCode;
            //        tpalipayservice.Insert(modelalipay);
            //        paymentList.Add(modelArRecord.id.ToString());
            //        tr.Commit();
            //        tb_payment_accountsService accountsService = new tb_payment_accountsService();
            //        var data = accountsService.FindListByClause(x => x.schoolcode == schoolCode, t => t.id, OrderByType.Asc) as List<tb_payment_accounts>;
            //        if (data.Count()>0)
            //        {
            //            payment_Orders modelOrders = new payment_Orders()
            //            {
            //                PrivateKey = data[0].private_key,
            //                OrderName = model.name,
            //                Price = model.amount.ToString(),
            //                AlipayPublicKey = data[0].alipay_public_key,
            //                AppId = data[0].appid,
            //                OrderNo = alipayOrderNo
            //            };
            //            orderList.Add(modelOrders);
            //        }
            //        return (paymentList, arList, orderList);
            //    }
            //    catch (Exception ex)
            //    {
            //        tr.Rollback();
            //        log.Error("事务回滚:" + ex);
            //        return (null, null, null);
            //    }
            //    finally
            //    {
            //        sqlcon.Close();
            //    }
            //}
            #endregion
        }
        #region CreateOrder
        //public (List<string> paymentList, List<string> arList, List<payment_Orders> orderList) CreateOrder(tb_payment_ar model, string alipayOrderNo, string schoolCode)
        //{
        //    var paymentList = new List<string>();
        //    var arList = new List<string>();
        //    var orderList = new List<payment_Orders>();
        //    if (model == null)
        //        return (paymentList, arList, orderList);
        //    #region
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(@" INSERT INTO dbo.tb_payment_ar_record
        //                            (
        //                                ar_id,
        //                                /*trade_no,*/
        //                                out_trade_no,
        //                                payer_passport,
        //                                pay_amount,
        //                                refund_amount,
        //                                status,
        //                                pay_time
        //                            ) ");
        //    sb.Append(@" values
        //                            (
        //                                @ar_id,
        //                                /*@trade_no,*/
        //                                @out_trade_no,
        //                                @payer_passport,
        //                                @pay_amount,
        //                                @refund_amount,
        //                                @status,
        //                                getdate()
        //                            );select @@IDENTITY ");
        //    StringBuilder sb1 = new StringBuilder();
        //    sb1.Append(@" SELECT id,
        //                    ar_id,
        //                    trade_no,
        //                    out_trade_no,
        //                    payer_passport,
        //                    pay_amount,
        //                    refund_amount,
        //                    status,
        //                    pay_time FROM tb_payment_ar_record ");
        //    sb1.Append(@" where out_trade_no=@out_trade_no ");

        //    StringBuilder sb2 = new StringBuilder();
        //    sb2.Append(@" INSERT tb_payment_alipay_record
        //                            (
        //                                alipay_order,
        //                                [order],
        //                                [type],
        //                                create_time
        //                            )");
        //    sb2.Append(@"           VALUES
        //                            (   
        //                             @alipay_order,
        //                                @order,
        //                                @type,
        //                                @create_time
        //                             )");
        //    StringBuilder sb3 = new StringBuilder();
        //    sb3.Append(@" SELECT id,
        //                           schoolcode,
        //                           pid,
        //                           appid,
        //                           name,
        //                           private_key,
        //                           publickey,
        //                           alipay_public_key FROM tb_payment_accounts 
        //                            WHERE schoolcode=@schoolcode ");
        //    tb_payment_ar_record modelArRecord = new tb_payment_ar_record
        //    {
        //        ar_id = model.id,
        //        out_trade_no = alipayOrderNo,
        //        payer_passport = model.passport,
        //        pay_amount = decimal.Parse(model.amount == null ? "0" : model.amount.ToString()),
        //        refund_amount = 0,
        //        status = 0,
        //        pay_time = DateTime.Now
        //    };
        //    using (SqlConnection sqlcon = new SqlConnection(DbFactory.GetSqlSugarClient().Ado.Connection.ConnectionString))
        //    {
        //        SqlTransaction tr = null;
        //        try
        //        {
        //            sqlcon.Open();
        //            tr = sqlcon.BeginTransaction();
        //            SqlCommand sqlcmd = new SqlCommand(sb.ToString(), sqlcon);
        //            sqlcmd.Parameters.Add(new SqlParameter("@ar_id", modelArRecord.ar_id));
        //            //sqlcmd.Parameters.Add(new SqlParameter("@trade_no", modelArRecord.trade_no));
        //            sqlcmd.Parameters.Add(new SqlParameter("@out_trade_no", modelArRecord.out_trade_no));
        //            sqlcmd.Parameters.Add(new SqlParameter("@payer_passport", modelArRecord.payer_passport));
        //            sqlcmd.Parameters.Add(new SqlParameter("@pay_amount", modelArRecord.pay_amount));
        //            sqlcmd.Parameters.Add(new SqlParameter("@refund_amount", modelArRecord.refund_amount));
        //            sqlcmd.Parameters.Add(new SqlParameter("@status", modelArRecord.status));
        //            sqlcmd.Transaction = tr;
        //            modelArRecord.id = Convert.ToInt32(sqlcmd.ExecuteScalar());

        //            //System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter(sb1.ToString(),sqlcon);
        //            //sda.SelectCommand.Parameters.Add(new SqlParameter("@out_trade_no",modelArRecord.out_trade_no));
        //            //sda.SelectCommand.Transaction = tr;
        //            //System.Data.DataTable dt = new System.Data.DataTable();
        //            //sda.Fill(dt);

        //            SqlCommand sqlcmd1 = new SqlCommand(sb2.ToString(), sqlcon);
        //            sqlcmd1.Parameters.Add(new SqlParameter("@alipay_order", alipayOrderNo));
        //            sqlcmd1.Parameters.Add(new SqlParameter("@order", alipayOrderNo));
        //            sqlcmd1.Parameters.Add(new SqlParameter("@type", "0"));
        //            sqlcmd1.Parameters.Add(new SqlParameter("@create_time", DateTime.Now.ToString()));
        //            sqlcmd1.Transaction = tr;
        //            sqlcmd1.ExecuteNonQuery();
        //            //需要反查ID
        //            tb_payment_alipay_record model1 = new tb_payment_alipay_record()
        //            {
        //                alipay_order = alipayOrderNo,
        //                order = alipayOrderNo,
        //                type = byte.Parse("0"),
        //                create_time = DateTime.Now,
        //                schoolcode = string.Empty,
        //            };
        //            paymentList.Add(modelArRecord.id.ToString());

        //            tr.Commit();
        //            System.Data.SqlClient.SqlDataAdapter sda1 = new System.Data.SqlClient.SqlDataAdapter(sb3.ToString(), sqlcon);
        //            //sda1.SelectCommand.Parameters.Add(new SqlParameter("@id", modelArRecord.id));
        //            sda1.SelectCommand.Parameters.Add(new SqlParameter("@schoolcode", schoolCode));
        //            System.Data.DataTable dt1 = new System.Data.DataTable();
        //            sda1.Fill(dt1);
        //            if (dt1.Rows.Count > 0)
        //            {
        //                payment_Orders modelOrders = new payment_Orders()
        //                {
        //                    PrivateKey = dt1.Rows[0]["private_key"].ToString(),
        //                    OrderName = model.name,
        //                    Price = model.amount.ToString(),
        //                    AlipayPublicKey = dt1.Rows[0]["alipay_public_key"].ToString(),
        //                    AppId = dt1.Rows[0]["appid"].ToString(),
        //                    OrderNo = alipayOrderNo
        //                };
        //                orderList.Add(modelOrders);
        //            }
        //            return (paymentList, arList, orderList);
        //        }
        //        catch (Exception ex)
        //        {
        //            tr.Rollback();
        //            log.Error("事务回滚:" + ex);
        //            return (null, null, null);
        //        }
        //        finally
        //        {
        //            sqlcon.Close();
        //        }
        //    }
        //    #endregion
        //}
        #endregion
        public (tb_payment_ar modelAr, tb_payment_accounts modelTpa, tb_payment_ar_record modelPar) GetListByTradeNo(string TradeNo)
        {
            tb_payment_ar modelAr = null;
            tb_payment_accounts modelTpa = null;
            tb_payment_ar_record modelPar = null;
            using (var db = DbFactory.GetSqlSugarClient())
            {
                List<tb_payment_ar_record> lstPar = db.Queryable<tb_payment_ar_record>()
                                    .Where(x => x.out_trade_no.Equals(TradeNo)).ToList();
                if (lstPar.Count > 0)
                    modelPar = lstPar[0];
                if (modelPar != null)
                {
                    List<tb_payment_ar> lstAr = db.Queryable<tb_payment_ar>()
                                        .Where(x => x.id.Equals(modelPar.ar_id)).ToList();
                    modelAr = lstAr[0];
                    List<tb_payment_accounts> lstTpa = db.Queryable<tb_payment_accounts>()
                                    .Where(x => x.schoolcode.Equals(modelAr.schoolcode)).ToList();
                    if (lstTpa.Count > 0)
                        modelTpa = lstTpa[0];
                }
                else
                {
                    List<tb_payment_record> lst = db.Queryable<tb_payment_record>()
                                                    .Where(x => x.out_order_no.Equals(TradeNo)).ToList();
                    if (lst.Count > 0)
                    {
                        tb_school_user modelSchoolUser = db.Queryable<tb_school_user>()
                                                        .Single(x => x.student_id == lst[0].student_id);
                        tb_school_info modelSchoolInfo = db.Queryable<tb_school_info>()
                                                        .Single(x => x.School_Code == modelSchoolUser.school_id);
                        if (modelSchoolInfo != null)
                        {
                            modelTpa = db.Queryable<tb_payment_accounts>()
                                                        .Single(x => x.private_key.Equals(modelSchoolInfo.private_key)
                                                                    && x.alipay_public_key.Equals(modelSchoolInfo.alipay_public_key));
                        }
                    }
                }
                return (modelAr, modelTpa, modelPar);
            }
        }
        public bool PayOk2(string TradeNo)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                tb_payment_ar_record modelPar = null;
                try
                {
                    db.Ado.BeginTran(System.Data.IsolationLevel.ReadCommitted);
                    List<tb_payment_ar_record> lstPar = db.Queryable<tb_payment_ar_record>()
                                    .Where(x => x.out_trade_no.Equals(TradeNo)).ToList();
                    if (lstPar.Count > 0)
                        modelPar = lstPar[0];
                    if (modelPar != null)
                    {

                        tb_payment_ar_recordService par = new tb_payment_ar_recordService();
                        par.UpdateColumnsByConditon(x => new tb_payment_ar_record { status = 1, pay_time = SqlFunc.GetDate() }, x => x.out_trade_no == TradeNo);
                        tb_payment_ARService p_ar = new tb_payment_ARService();
                        tb_payment_ar model = p_ar.FindById(modelPar.ar_id);
                        model.status = 1;
                        model.JSstatus = 1;
                        model.fact_amount = model.amount;
                        p_ar.Update(model);
                    }
                    else
                    {
                        tb_payment_recordService prs = new tb_payment_recordService();
                        prs.UpdateColumnsByConditon(x => new tb_payment_record { status = 1, pay_time = SqlFunc.GetDate() }, x => x.out_order_no == TradeNo);
                    }

                    db.Ado.CommitTran();
                    return true;
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    return false;
                }
            }
        }
        public bool PayOk(string TradeNo)
        {
            //tb_payment_ar modelAr = null;
            //tb_payment_ar_record modelPar = null;
            StringBuilder sb = new StringBuilder();
            sb.Append(@" SELECT id,
                               ar_id,
                               trade_no,
                               out_trade_no,
                               payer_passport,
                               pay_amount,
                               refund_amount,
                               status,
                               pay_time FROM dbo.tb_payment_ar_record ");
            sb.Append(@" where out_trade_no=@out_trade_no ");

            StringBuilder sb3 = new StringBuilder();
            sb3.Append(" update tb_payment_record ");
            sb3.Append(" set status=1,refund_amount=pay_amount,pay_time=@pay_time ");
            sb3.Append(" where out_order_no=@out_order_no ");

            StringBuilder sb1 = new StringBuilder();
            sb1.Append(@" update dbo.tb_payment_ar_record ");
            sb1.Append(@" set pay_time=@pay_time
                                ,status=1 ");
            sb1.Append(" where out_trade_no=@out_trade_no ");

            StringBuilder sb2 = new StringBuilder();
            sb2.Append(" update tb_payment_ar ");
            sb2.Append(" set status=1,JSstatus=1,fact_amount=amount ");
            sb2.Append(" where id=@id ");

            using (SqlConnection sqlcon = new SqlConnection(DbFactory.GetSqlSugarClient().Ado.Connection.ConnectionString))
            {
                SqlTransaction tr = null;
                try
                {
                    sqlcon.Open();
                    tr = sqlcon.BeginTransaction();
                    System.Data.SqlClient.SqlDataAdapter sda1 = new System.Data.SqlClient.SqlDataAdapter(sb.ToString(), sqlcon);
                    sda1.SelectCommand.Parameters.Add(new SqlParameter("@out_trade_no", TradeNo));
                    sda1.SelectCommand.Transaction = tr;
                    System.Data.DataTable dt = new System.Data.DataTable();
                    sda1.Fill(dt);

                    SqlCommand sqlcmd = new SqlCommand(sb1.ToString(), sqlcon);
                    //sqlcmd.Parameters.Add(new SqlParameter("@pay_time", dt.Rows[0]["pay_time"].ToString()));
                    //sqlcmd.Parameters.Add(new SqlParameter("@out_trade_no", dt.Rows[0]["out_trade_no"].ToString()));

                    SqlCommand sqlcmd2 = new SqlCommand(sb3.ToString(), sqlcon);
                    sqlcmd2.Parameters.Add(new SqlParameter("@out_order_no", TradeNo));
                    sqlcmd2.Parameters.Add(new SqlParameter("@pay_time", DateTime.Now.ToString()));
                    sqlcmd2.Transaction = tr;
                    sqlcmd2.ExecuteNonQuery();

                    if (dt.Rows.Count > 0)
                    {
                        sqlcmd.Parameters.Add(new SqlParameter("@pay_time", DateTime.Now.ToString()));
                        sqlcmd.Parameters.Add(new SqlParameter("@out_trade_no", TradeNo));
                        sqlcmd.Transaction = tr;
                        sqlcmd.ExecuteNonQuery();

                        SqlCommand sqlcmd1 = new SqlCommand(sb2.ToString(), sqlcon);
                        sqlcmd1.Parameters.Add(new SqlParameter("@id", dt.Rows[0]["ar_id"].ToString()));
                        sqlcmd1.Transaction = tr;
                        sqlcmd1.ExecuteNonQuery();
                    }
                    tr.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    log.Error("事务回滚:" + ex);
                    return false;
                }
                finally
                {
                    sqlcon.Close();
                }
            }

            //using (var db = DbFactory.GetSqlSugarClient())
            //{
            //    try
            //    {
            //        db.Ado.BeginTran(System.Data.IsolationLevel.ReadCommitted);
            //        List<tb_payment_ar_record> lstPar = db.Queryable<tb_payment_ar_record>()
            //                        .Where(x => x.out_trade_no.Equals(TradeNo)).ToList();
            //        if (lstPar.Count > 0)
            //            modelPar = lstPar[0];
            //        if (modelPar != null)
            //        {
            //            //modelPar.status = 1;
            //            //modelPar.pay_time = DateTime.Now;
            //            //db.Updateable(modelPar).ExecuteCommand();
            //            StringBuilder sb = new StringBuilder();
            //            sb.Append(@" update dbo.tb_payment_ar_record ");
            //            sb.Append(@" set pay_time=@pay_time,status=@status ");
            //            sb.Append(" where out_trade_no=@out_trade_no ");
            //            SugarParameter[] pa = new SugarParameter[3];
            //            pa[0] = new SugarParameter("@pay_time", modelPar.pay_time);
            //            pa[1] = new SugarParameter("@status", modelPar.status);
            //            pa[2] = new SugarParameter("@out_trade_no", modelPar.out_trade_no);
            //            db.Ado.ExecuteCommand(sb.ToString(), pa);


            //            List<tb_payment_ar> lstAr = db.Queryable<tb_payment_ar>()
            //                                .Where(x => x.id.Equals(modelPar.ar_id)).ToList();
            //            modelAr = lstAr[0];
            //            modelAr.status = 1;
            //            modelAr.JSstatus = 1;
            //            modelAr.fact_amount = modelAr.amount;
            //            db.Updateable(modelAr).ExecuteCommand();
            //        }
            //        db.Ado.CommitTran();
            //        return true;
            //    }
            //    catch (Exception ex)
            //    {
            //        db.Ado.RollbackTran();
            //        return false;
            //    }
            //}
        }

        public FeePayable GetFeePayableBySchoolCode(string schoolcode)
        {
            FeePayable modelReturn = null;
            using (var db = DbFactory.GetSqlSugarClient())
            {
                modelReturn = db.Queryable<tb_payment_ar>()
                                .Where(x => x.schoolcode == schoolcode)
                                .Select(x => new FeePayable()
                                {
                                    YJJE = SqlFunc.IsNull(SqlFunc.AggregateSum(SqlFunc.ToDecimal(x.amount)), 0),
                                    SJJE = SqlFunc.IsNull(SqlFunc.AggregateSum(SqlFunc.ToDecimal(x.fact_amount)), 0),
                                    SJBS = SqlFunc.IsNull(SqlFunc.AggregateSum(SqlFunc.ToDecimal(x.JSstatus)), 0),
                                    YJBS = SqlFunc.IsNull(SqlFunc.AggregateCount(SqlFunc.ToDecimal(x.ARID)), 0)
                                }).First();
                modelReturn.YJXM = db.Queryable<tb_payment_ar>()
                                    .Where(x => x.schoolcode == schoolcode && x.status == 0).ToList().Count();
            }
            return modelReturn;
        }
        /// <summary>
        /// 缴费大厅概览数据
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public PaymentARAccount GetPaymentARAccount(string schoolcode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var modelReturn = db.Queryable<tb_payment_ar>()
                                .Where(x => x.schoolcode == schoolcode)
                                .Select(x => new PaymentARAccount
                                {
                                    YJJE = SqlFunc.IsNull(SqlFunc.AggregateSum(SqlFunc.ToDecimal(x.amount)), 0),
                                    SJJE = SqlFunc.IsNull(SqlFunc.AggregateSum(SqlFunc.ToDecimal(x.fact_amount)), 0),
                                    SJRS = SqlFunc.IsNull(SqlFunc.AggregateSum(SqlFunc.ToDecimal(x.JSstatus)), 0),
                                    YJRS = SqlFunc.IsNull(SqlFunc.AggregateCount(x.amount), 0)
                                }).First();
                return modelReturn;
            }

        }
        public int GetPaymentARAccount_YJXM(string schoolcode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                string sql = @"select count(a.arid) YJXM 
                    from 
                    (select arid from tb_payment_AR where  schoolcode = '" + schoolcode + "' and status=0 group by arid) a";
                var count = db.Ado.SqlQuery<int>(sql).FirstOrDefault();
                return count;
            }
        }
        /// <summary>
        /// 获取Payment_record 的pay_amount 总额和总行数
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public (decimal pay_amount, int count) GetPayment_recordAmountCount(string schoolcode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                string tt = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
                DateTime etime = Convert.ToDateTime(tt);
                var pay_amount = db.Queryable<tb_payment_record, tb_payment_item>((pr, pi) => new object[]{
                                        JoinType.Inner,pr.payment_id == pi.id
                                    })
                                 .Where((pr, pi) => pi.schoolcode == schoolcode
                                 && pr.pay_time >= DateTime.Now.Date && pr.pay_time <= etime)
                                 .Select((pr, pi) => new
                                 {
                                     amount = SqlFunc.IsNull(SqlFunc.AggregateSum(SqlFunc.ToDecimal(pr.pay_amount)), 0),
                                     count = SqlFunc.IsNull(SqlFunc.AggregateCount(pr.pay_amount), 0),
                                 }).First();
                int count = db.Queryable<tb_payment_ar>()
                                    .Where(x => x.schoolcode == schoolcode && x.status == 1).ToList().Count();
                return (pay_amount.amount, pay_amount.count);
            }
        }
        public List<WeekPayment_item> GetWeekPayment_item(string schoolcode, string stime, string etime)
        {
            DateTime ED = Convert.ToDateTime(etime);
            DateTime SD = Convert.ToDateTime(stime);
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var pay_amount = db.Queryable<tb_payment_record, tb_payment_item>((pr, pi) => new object[]{
                                        JoinType.Inner,pr.payment_id == pi.id
                                    })
                                 .Where((pr, pi) => pi.schoolcode == schoolcode
                                 //&& pr.pay_time >= SD && pr.pay_time <= ED
                                 && pr.status == 1)
                                 .Select((pr, pi) => new WeekPayment_item
                                 {
                                     pay_amount = SqlFunc.IsNull(SqlFunc.AggregateSum(SqlFunc.ToDecimal(pr.pay_amount)), 0),
                                     pay_time = SqlFunc.Substring(pr.pay_time, 0, 10),
                                 }).GroupBy(pr => SqlFunc.Substring(pr.pay_time, 0, 10)).ToList();

                return pay_amount;
            }
        }
        public List<Payment_ARList> Getpayment_ARList(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode, string status = "", string selectinfo = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var pay_amount = db.Queryable<tb_payment_ar>()
                                 .WhereIF(!string.IsNullOrWhiteSpace(selectinfo), x => x.ARID == selectinfo || x.name == selectinfo)
                                 .WhereIF(!string.IsNullOrWhiteSpace(status), x => x.status == SqlFunc.ToInt32(status))
                                 .Where(x => x.schoolcode == schoolCode)
                                 .Select(x => new Payment_ARList
                                 {
                                     ARID = x.ARID,
                                     name = x.name,
                                     arcount = SqlFunc.AggregateCount(x.id),
                                     fact_count = SqlFunc.AggregateSum(SqlFunc.ToInt32(x.JSstatus)),
                                     amount = SqlFunc.AggregateSum(SqlFunc.ToDecimal(x.amount)),
                                     fact_amount = SqlFunc.AggregateSum(SqlFunc.ToInt32(x.fact_amount)),
                                     status = SqlFunc.ToInt32(x.status),
                                     star_date = SqlFunc.ToDate(x.star_date)
                                 })//ARID, name,status, star_date
                                 .GroupBy(@"ARID,
	                                    `name`,
	                                    `status`,
	                                    star_date
	                                   ")
                                 .ToPageList(pageIndex, pageSize, ref total);
                return pay_amount;
            }
        }
        public List<Payment_ARToUserList> Getpayment_ARListToUser(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode, string selectinfo = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var pay_amount = db.Queryable<tb_payment_ar, tb_school_user, tb_payment_ar_record>((pa, su, par) => new object[]{
                                        JoinType.Left,pa.passport == su.passport,
                                        JoinType.Left,pa.passport == par.payer_passport&&pa.id == par.ar_id&&par.status == 1,
                                    })
                                .WhereIF(!string.IsNullOrWhiteSpace(selectinfo), (pa, su, par) => su.user_name == selectinfo || su.student_id == selectinfo || su.passport == selectinfo)
                                .Where((pa, su, par) => pa.schoolcode == schoolCode)
                                .Select((pa, su, par) => new Payment_ARToUserList
                                {
                                    ARID = pa.ARID,
                                    name = pa.name,
                                    username = su.user_name,
                                    studentid = su.student_id,
                                    amount = SqlFunc.ToDecimal(pa.amount),
                                    fact_amount = SqlFunc.ToDecimal(pa.fact_amount),
                                    pay_time = SqlFunc.ToDate(par.pay_time),
                                    passport = su.passport
                                })
                                .ToPageList(pageIndex, pageSize, ref total);

                return pay_amount;
            }
        }

        public List<Payment_ARList> Getpayment_ARDetailed(string schoolCode, string ARID)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var pay_amount = db.Queryable<tb_payment_ar>()
                                 .Where(x => x.schoolcode == schoolCode && x.ARID == ARID)
                                 .Select(x => new Payment_ARList
                                 {
                                     ARID = x.ARID,
                                     name = x.name,
                                     arcount = SqlFunc.AggregateCount(x.id),
                                     fact_count = SqlFunc.AggregateSum(SqlFunc.ToInt32(x.JSstatus)),
                                     amount = SqlFunc.AggregateSum(SqlFunc.ToDecimal(x.amount)),
                                     fact_amount = SqlFunc.AggregateSum(SqlFunc.ToInt32(x.fact_amount)),
                                     status = SqlFunc.ToInt32(x.status),
                                     star_date = SqlFunc.ToDate(x.star_date)
                                 })//ARID, name,status, star_date
                                 .GroupBy(@"ARID,
	                                    `name`,
	                                    `status`,
	                                    star_date
	                                   ")
                                .ToList();
                return pay_amount;//--st_name passport amount fact_amount pay_time
            }
        }
        public List<Payment_ARisPayment> GetPayment_ARisPayment(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode, string ARID = "", string JSstatus = "", string name = "", string stime = "", string etime = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var pay_amount = db.Queryable<tb_payment_ar, tb_payment_ar_record, tb_school_user>((pa, par, pr) => new object[]{
                                        JoinType.Inner,pa.id == par.ar_id,
                                        JoinType.Left,pa.passport == pr.passport
                                    })
                                .WhereIF(!string.IsNullOrWhiteSpace(ARID), (pa, par, pr) => pa.ARID == ARID)
                                .WhereIF(!string.IsNullOrWhiteSpace(JSstatus), (pa, par, pr) => pa.JSstatus == SqlFunc.ToInt32(JSstatus) && par.status == SqlFunc.ToInt32(JSstatus))
                                .WhereIF(!string.IsNullOrWhiteSpace(name), (pa, par, pr) => pa.st_name.Contains(name) || pa.passport.Contains(name) || pr.cell.Contains(name) || pr.student_id.Contains(name) || par.out_trade_no.Contains(name))
                                .Where((pa, par, pr) => pa.schoolcode == schoolCode)
                                .Select((pa, par, pr) => new Payment_ARisPayment
                                {
                                    passport = pa.passport,
                                    st_name = pa.st_name,
                                    amount = SqlFunc.ToDecimal(pa.amount),
                                    fact_amount = SqlFunc.ToDecimal(pa.fact_amount),
                                    pay_time = SqlFunc.ToDate(par.pay_time),
                                    out_order_no = par.out_trade_no,
                                    name = pa.name,
                                    stundetid = pr.student_id,
                                    phone = pr.cell,
                                    status = par.status
                                })
                                .ToPageList(pageIndex, pageSize, ref total);

                return pay_amount;
            }
        }
        public List<Payment_ARisPayment> GetPayment_ARisPayment2(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode, string ARID = "", string JSstatus = "", string name = "", string stime = "", string etime = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var pay_amount = db.Queryable<tb_payment_ar, tb_school_user>((pa, pr) => new object[]{
                                        JoinType.Left,pa.passport == pr.passport
                                    })
                                .WhereIF(!string.IsNullOrWhiteSpace(ARID), (pa, pr) => pa.ARID == ARID)
                                //.WhereIF(!string.IsNullOrWhiteSpace(JSstatus), (pa, pr) => pa.JSstatus != SqlFunc.ToInt32(JSstatus))
                                .WhereIF(!string.IsNullOrWhiteSpace(name), (pa, pr) => pa.st_name.Contains(name) || pa.passport.Contains(name) || pr.cell.Contains(name) || pr.student_id.Contains(name))
                                .Where((pa, pr) => pa.schoolcode == schoolCode && pa.JSstatus ==0 )
                                .Select((pa, pr) => new Payment_ARisPayment
                                {
                                    passport = pa.passport,
                                    st_name = pa.st_name,
                                    amount = SqlFunc.ToDecimal(pa.amount),
                                    fact_amount = SqlFunc.ToDecimal(pa.fact_amount),
                                    //pay_time = SqlFunc.ToDate(par.pay_time),
                                    //out_order_no = par.out_trade_no,
                                    name = pa.name,
                                    stundetid = pr.student_id,
                                    phone = pr.cell,
                                    status = 0
                                })
                                .ToPageList(pageIndex, pageSize, ref total);

                return pay_amount;
            }
        }
    }
}