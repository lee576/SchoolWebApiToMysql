﻿using DbModel;
using Infrastructure;
using Infrastructure.Service;
using IService;
using Models.ViewModels;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static System.String;

namespace Service
{
    public class tb_cashier_trade_orderService : GenericService<tb_cashier_trade_order>,Itb_cashier_trade_orderService
    {
        public Getcashier_trade_orderSum Getcashier_trade_order(string schoolcode, string stime, string etime)
        {


           
            Getcashier_trade_orderSum obj = new Getcashier_trade_orderSum();

            if (!string.IsNullOrWhiteSpace(stime) && !string.IsNullOrWhiteSpace(etime))
            {
                //stime = stime + " 00:00:00";
                //etime = etime + " 23:59:59";
                DateTime st = Convert.ToDateTime(stime);
                DateTime et = Convert.ToDateTime(etime);
                string sql = "";

                List<double> pays = new List<double>();
                List<int> ordercount = new List<int>();
                List<string> array_alldays = new List<string>();




                //if (st.Month == et.Month)
                //{

                //string tablename = "tb_cashier_trade_order" + st.ToString("yyyyMM");

                //sql = @"select DATE_FORMAT(finish_time,'%Y-%m-%d') days,SUM(paid)-sum(refund) paysum,count(*) orderCount  from " + tablename + " where finish_time >= '" + stime + "' and finish_time <= '" + etime + "' and status = 1  and terminal_number in(select t.sn from tb_cashier_device t where t.schoolcode='" + schoolcode + "') group by DATE_FORMAT(finish_time,'%Y-%m-%d')";

                sql = "select * from tb_cashier_sumcount where days>='"+ stime + "' and days<='"+ etime + "' and school_id='"+ schoolcode + "'";

                    using (var db = DbFactory.GetSqlSugarClient())
                    {
                        var dr = db.Ado.SqlQuery<Cashier_trade_orderSum>(sql);


                    for (DateTime dt = et; dt >= st; dt = dt.AddDays(-1))
                        {
                            array_alldays.Add(dt.ToString("yyyy/MM/dd"));
                            Cashier_trade_orderSum dc = dr.Where(p => Convert.ToDateTime(p.days).ToString("yyyy/MM/dd") == dt.ToString("yyyy/MM/dd")).FirstOrDefault();

                            if (dc != null)
                            {

                                pays.Add(dc.paysum);
                                ordercount.Add(dc.orderCount);
                            }
                            else
                            {
                                pays.Add(0);
                                ordercount.Add(0);
                            }




                        }


                    }

               // }
                //else
                //{
                //    using (var db = DbFactory.GetSqlSugarClient())
                //    {



                //        //得到最后一个月份中的第一天
                //        DateTime first = et.AddDays(1 - et.Day).Date;
                //        //得出第一个月份中的最后一天 
                //        DateTime last = st.AddDays(1 - st.Day).Date.AddMonths(1).AddSeconds(-1);







                //        string tablename = "tb_cashier_trade_order" + et.ToString("yyyyMM");
                //        //查询后一个月中第一天到最后一天数据
                //        sql = @"select DATE_FORMAT(finish_time,'%Y-%m-%d') days,SUM(paid)-sum(refund) paysum,count(*) orderCount  from tb_cashier_trade_order where finish_time >= '" + first + "' and finish_time <= '" + etime + "' and status = 1  and terminal_number in(select t.sn from tb_cashier_device t where t.schoolcode='" + schoolcode + "') group by DATE_FORMAT(finish_time,'%Y-%m-%d')";

                //        var dr = db.Ado.SqlQuery<Cashier_trade_orderSum>(sql);


                //        //先循环后一个月的数据
                //        for (DateTime dt = et; dt >= first; dt = dt.AddDays(-1))
                //        {
                //            array_alldays.Add(dt.ToString("yyyy/MM/dd"));
                //            Cashier_trade_orderSum dc = dr.Where(p => Convert.ToDateTime(p.days).ToString("yyyy/MM/dd") == dt.ToString("yyyy/MM/dd")).FirstOrDefault();

                //            if (dc != null)
                //            {

                //                pays.Add(dc.paysum);
                //                ordercount.Add(dc.orderCount);
                //            }
                //            else
                //            {
                //                pays.Add(0);
                //                ordercount.Add(0);
                //            }
                //        }


                //        tablename = "tb_cashier_trade_order" + st.ToString("yyyyMM");
                //        //查询后一个月中第一天到最后一天数据
                //        sql = @"select DATE_FORMAT(finish_time,'%Y-%m-%d') days,SUM(paid)-sum(refund) paysum,count(*) orderCount  from tb_cashier_trade_order where finish_time >= '" + stime + "' and finish_time <= '" + last + "' and status = 1  and terminal_number in(select t.sn from tb_cashier_device t where t.schoolcode='" + schoolcode + "') group by DATE_FORMAT(finish_time,'%Y-%m-%d')";

                //        dr = db.Ado.SqlQuery<Cashier_trade_orderSum>(sql);


                //        //先循环后一个月的数据
                //        for (DateTime dt = last; dt >= st; dt = dt.AddDays(-1))
                //        {
                //            array_alldays.Add(dt.ToString("yyyy/MM/dd"));
                //            Cashier_trade_orderSum dc = dr.Where(p => Convert.ToDateTime(p.days).ToString("yyyy/MM/dd") == dt.ToString("yyyy/MM/dd")).FirstOrDefault();

                //            if (dc != null)
                //            {

                //                pays.Add(dc.paysum);
                //                ordercount.Add(dc.orderCount);
                //            }
                //            else
                //            {
                //                pays.Add(0);
                //                ordercount.Add(0);
                //            }
                //        }
                //    }

                //}





                obj.array_alldays = array_alldays;
                    obj.ordercount = ordercount;
                    obj.pays = pays;

                }
            
              return obj; 

        }


        public Cashier_trade_orderSum Getcashier_trade_orderCount(string schoolid, string stime, string etime)
        {
            //stime = stime + " 00:00:00";
            //etime = etime + " 23:59:59";
            //DateTime st = Convert.ToDateTime(stime);
            //DateTime et = Convert.ToDateTime(etime);
            string sql = "";
            // string tablename = "tb_cashier_trade_order" + st.ToString("yyyyMM");
            ////如果在同一个月份当中
            //if (st.Month == et.Month)
            //{

            //    sql = @"select '1' days,SUM(paid)-sum(refund) paysum,0 orderCount  from tb_cashier_trade_order where finish_time >= '" + stime + "' and finish_time <= '" + etime + "' and status = 1  and terminal_number in(select t.sn from tb_cashier_device t where t.schoolcode='" + schoolid + "')";

            sql = "select * from tb_cashier_sumcount where days='"+stime+ "' and school_id='"+ schoolid + "'";
            using (var db = DbFactory.GetSqlSugarClient())
                {
                    return db.Ado.SqlQuery<Cashier_trade_orderSum>(sql).FirstOrDefault();
                }
            //}
            //else
            //{
            //    using (var db = DbFactory.GetSqlSugarClient())
            //    {
            //        //得到最后一个月份中的第一天
            //        DateTime first = et.AddDays(1 - et.Day).Date;
            //    //得出第一个月份中的最后一天 
            //    DateTime last = st.AddDays(1 - st.Day).Date.AddMonths(1).AddSeconds(-1);


            //     sql = @"select '1' days,SUM(paid)-sum(refund) paysum,0 orderCount  from tb_cashier_trade_order where finish_time >= '" + first + "' and finish_time <= '" + etime + "' and status = 1  and terminal_number in(select t.sn from tb_cashier_device t where t.schoolcode='" + schoolid + "')";
               
            //        var v1= db.Ado.SqlQuery<Cashier_trade_orderSum>(sql).FirstOrDefault();


            //        sql = @"select '1' days,SUM(paid)-sum(refund) paysum,0 orderCount  from tb_cashier_trade_order where finish_time >= '" + first + "' and finish_time <= '" + et + "' and status = 1  and terminal_number in(select t.sn from tb_cashier_device t where t.schoolcode='" + schoolid + "')";


            //    }


            //}
           

           

           


        }


        /// <summary>
        /// 获取交易流水
        /// </summary>
        /// <param name="dining_hall"></param>
        /// <param name="stall"></param>
        /// <param name="order"></param>
        /// <param name="user_code"></param>
        /// <param name="machine"></param>
        /// <param name="tid"></param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="schoolcode"></param>
        public List<Cashier_trade_Flowing> GetFlowing(string dining_hall,string stall,string order,string user_code,string machine,string tid,string stime,string etime,string schoolcode,int pageIndex, int pageSize, ref int totalRecordNum)
        {

            string where1 = " ";
            string where2 = " ";
            string where3 = " ";
            string where4 = " ";
            string where5 = " ";

            if (!IsNullOrWhiteSpace(dining_hall) && dining_hall!= "0") where1 += $" and a.id ="+ dining_hall;

            if (!IsNullOrWhiteSpace(stall) && stall != "0") where2 += $" and b.id = "+stall;

            if (!IsNullOrWhiteSpace(order)) where3 += $" and t.order = '"+order+"'";

            if (!IsNullOrWhiteSpace(user_code)) where4 += $" where student_id = '"+user_code+"' ";

            if (!IsNullOrWhiteSpace(machine)) where5 += $" and c.sn = '"+machine+"'";

            

           

      
            stime = stime + " 00:00:00";
            etime = etime + " 23:59:59";
            DateTime st = Convert.ToDateTime(stime);
            DateTime et = Convert.ToDateTime(etime);



            string sql1 = @"SELECT g.order,c.student_id user_code,c.user_name  name,b.stname as shop,b.dkname as stall, g.terminal_number as machine, g.pay_amount, g.refund, g.status, g.finish_time as datetime from (select t.order, t.terminal_number,t.pay_amount, t.refund, t.status, t.finish_time,t.payer_account  
  from tb_cashier_trade_order t where finish_time >= '" + stime + "' and finish_time <= '"+ etime + "'"+ where3 + ") g INNER JOIN (select  a.name stname,b.name dkname,c.sn from tb_cashier_dining_hall a INNER JOIN tb_cashier_stall b on a.id=b.dining_tall INNER JOIN tb_cashier_device c  on b.id=c.stall  where  a.schoolcode='"+schoolcode+"'"+ where1+ where2+ where5 + " ) b on g.terminal_number=b.sn left  JOIN (select student_id,user_name,ali_user_id from  tb_school_user where school_id = '"+schoolcode+"' and card_state = 1 ) c  on g.payer_account=c.ali_user_id  "+ where4;




            string sql2 = @"SELECT COUNT(*)  from (select t.terminal_number,t.payer_account 
  from tb_cashier_trade_order t where finish_time >= '" + stime + "' and finish_time <= '" + etime + "'" + where3 + ") g INNER JOIN (select  a.name stname,b.name dkname,c.sn from tb_cashier_dining_hall a INNER JOIN tb_cashier_stall b on a.id=b.dining_tall INNER JOIN tb_cashier_device c  on b.id=c.stall  where  a.schoolcode='" + schoolcode + "'" + where1 + where2 + where5 + " ) b on g.terminal_number=b.sn left  JOIN (select student_id,user_name,ali_user_id from  tb_school_user where school_id = '" + schoolcode + "' and card_state = 1 ) c  on g.payer_account=c.ali_user_id "+ where4;




            using (var db = DbFactory.GetSqlSugarClient())
            {
               

                if (pageSize != -1) 
                {
                    sql1 += $"   limit  " + pageIndex + "," + pageSize;
                    var dr = db.Ado.SqlQuery<Cashier_trade_Flowing>(sql1);
                    return dr;
                }
                else
                {
                    totalRecordNum = db.Ado.GetInt(sql2);
                    return null;
                }
            }

        }



        public List<Cashier_trade_FlowingExcel> GetFlowingExcel(string dining_hall, string stall, string order, string user_code, string machine, string tid, string stime, string etime, string schoolcode)
        {

            string where1 = " ";
            string where2 = " ";
            string where3 = " ";
            string where4 = " ";
            string where5 = " ";

            if (!IsNullOrWhiteSpace(dining_hall) && dining_hall != "0") where1 += $" and a.id =" + dining_hall;

            if (!IsNullOrWhiteSpace(stall) && stall != "0") where2 += $" and b.id = " + stall;

            if (!IsNullOrWhiteSpace(order)) where3 += $" and t.order = '" + order + "'";

            if (!IsNullOrWhiteSpace(user_code)) where4 += $" where student_id = '" + user_code + "' ";

            if (!IsNullOrWhiteSpace(machine)) where5 += $" and c.sn = '" + machine + "'";




            stime = stime + " 00:00:00";
            etime = etime + " 23:59:59";
            DateTime st = Convert.ToDateTime(stime);
            DateTime et = Convert.ToDateTime(etime);




            string sql1 = @"SELECT g.order,c.student_id user_code,c.user_name  name,b.stname as dining_name,b.dkname as stall_name, g.terminal_number as sn, g.paid, g.refund, g.status,g.create_time,g.finish_time,g.alipay_order,g.create_time,payer_account,pay_amount  from (select t.order,t.alipay_order,t.create_time, t.terminal_number,t.paid, t.refund, t.status, t.finish_time,t.payer_account,t.pay_amount 
  from tb_cashier_trade_order t where finish_time >= '" + stime + "' and finish_time <= '" + etime + "'" + where3 + ") g INNER JOIN (select  a.name stname,b.name dkname,c.sn from tb_cashier_dining_hall a INNER JOIN tb_cashier_stall b on a.id=b.dining_tall INNER JOIN tb_cashier_device c  on b.id=c.stall  where  a.schoolcode='" + schoolcode + "'" + where1 + where2 + where5 + " ) b on g.terminal_number=b.sn left  JOIN (select student_id,user_name,ali_user_id from  tb_school_user where school_id = '" + schoolcode + "' and card_state = 1 ) c  on g.payer_account=c.ali_user_id "+ where4;



            using (var db = DbFactory.GetSqlSugarClient())
            {
                var dr = db.Ado.SqlQuery<Cashier_trade_FlowingExcel>(sql1);
                return dr;
            }

        }


        /// <summary>
        /// 下载对账单
        /// </summary>
        /// <param name="timeType"></param>
        /// <param name="datetime"></param>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public List<Cashier_trade_Bill> GetBillExcel(string timeType, string datetime,string schoolcode)
        {
            string where = " ";


            if (!IsNullOrWhiteSpace(timeType))
            {
                //按自然日
                if (timeType == "0")
                {
                    string stime = datetime + " 00:00:00";
                    string etime = datetime + " 23:59:59";
                    DateTime st = Convert.ToDateTime(stime);
                    DateTime et = Convert.ToDateTime(etime);

                    where += $" and finish_time >= '" + st + "' and finish_time <= '" + et + "'";

                }
                else
                {
                    if (!IsNullOrWhiteSpace(datetime))
                    {
                        string stime = datetime + "-01 00:00:00";

                        DateTime st = Convert.ToDateTime(stime);
                        DateTime et = st.AddDays(1 - st.Day).Date.AddMonths(1).AddSeconds(-1);

                        where += $" and finish_time >= '" + st + "' and finish_time <= '" + et + "'";

                    }
                }



            }




            string tablename = "tb_cashier_trade_order";
            string sql = $"select t.alipay_order,t.order,t.type,t.trade_name,t.create_time,t.finish_time,t.stall,cs.name,t.operator as operators,t.terminal_number,payer_account,pay_amount,paid,t.refund,alipay_red,collection_treasure,alipay_discount,merchant_discount,ticket_money,ticket_name,merchant_red_consumption,card_consumption,refund_batch_number,service_charge,shares_profit,remark from {tablename} t , tb_cashier_dining_hall dh, tb_cashier_stall cs, tb_cashier_device cd where cd.stall = cs.id and t.terminal_number = cd.sn and dh.id = cs.dining_tall and cd.schoolcode='{schoolcode}' {where}";
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var dr = db.Ado.SqlQuery<Cashier_trade_Bill>(sql);
                return dr;
            }




        }




        /// <summary>
        /// 资金管理  总数统计
        /// </summary>
        /// <returns></returns>
        public List<Cashier_trade_Totalt> getTotalt(string stime, string etime, string schoolcode,string dining_hall,string stall)
        {
            string where = "1=1";

            if (!IsNullOrWhiteSpace(dining_hall) && dining_hall != "0") where += $" and dh.id =" + dining_hall;

            if (!IsNullOrWhiteSpace(stall) && stall != "0") where += $" and cd.stall = " + stall;

            stime = stime + " 00:00:00";
            etime = etime + " 23:59:59";
            DateTime st = Convert.ToDateTime(stime);
            DateTime et = Convert.ToDateTime(etime);
            string sql = "";
           
            string  stablename = "tb_cashier_trade_order";
            where += $" and finish_time >= '" + st + "' and finish_time <= '" + et + "'  AND terminal_number<>'' ";
            sql = $@"select COUNT(t.order) as totalOrder,COALESCE(SUM(t.pay_amount),0) as totalPayAmount,COALESCE(SUM(t.paid),0)-COALESCE(SUM(t.refund),0)  totalMoney,(select COUNT(0) from {stablename} cto, tb_cashier_device cd where status=1 and cto.terminal_number = cd.sn and cd.schoolcode='{schoolcode}' and type = 1 and {where})totalReund, COALESCE(SUM(t.refund),0) totalReundMoney from {stablename} t,tb_cashier_dining_hall dh, tb_cashier_stall cs, tb_cashier_device cd where t.type = 0 and cd.stall = cs.id and t.terminal_number = cd.sn and dh.id = cs.dining_tall and t.status=1 and cd.schoolcode='{schoolcode}' and {where}";




            using (var db = DbFactory.GetSqlSugarClient())
            {
                var dr = db.Ado.SqlQuery<Cashier_trade_Totalt>(sql);
                return dr;
            }
         
        }


        /// <summary>
        /// 资金管理列表
        /// </summary>
        /// <returns></returns>
        public List<Cashier_trade_detil> getCapitalList(string dining_hall,string stall, string stime, string etime, string schoolcode, int pageIndex,int pageSize,ref int totalRecordNum)
        {
            string where = "";

            if (!IsNullOrWhiteSpace(dining_hall) && dining_hall != "0") where += $" and dh.id =" + dining_hall;

            if (!IsNullOrWhiteSpace(stall) && stall != "0") where += $" and cd.stall = " + stall;

            stime = stime + " 00:00:00";
            etime = etime + " 23:59:59";
            DateTime st = Convert.ToDateTime(stime);
            DateTime et = Convert.ToDateTime(etime);

            string stablename = "";
           
            string SQL = "";
           
                stablename = "tb_cashier_trade_order" ;

            where += $" and finish_time >= '" + st + "' and finish_time <= '" + et + "' AND terminal_number<>'' ";


           

           SQL = $@"select dh.name as shop,cs.name as stall,COUNT(0) as totalNum,SUM(t.pay_amount) as totalOrderPrice,SUM(t.paid) as totalPayment,COALESCE((select SUM(ct.refund) from {stablename} ct where ct.stall = cs.id and ct.shop = dh.id and dh.schoolcode='{schoolcode}' and ct.type = 1 {where}),0) totalRefund,(select COUNT(0) from {stablename} ct where ct.stall = cs.id and ct.shop = dh.id and dh.schoolcode='{schoolcode}' and ct.type = 1 {where})totalRefundCount,SUM(pay_amount) -COALESCE((select SUM(ct.refund) from {stablename} ct where ct.stall = cs.id and ct.shop = dh.id and dh.schoolcode='{schoolcode}' and ct.type = 1 {where}),0) totalPrice,0.00 as totalBoard,sum(paid - pay_amount) as totalFavorable from {stablename} t , tb_cashier_dining_hall dh, tb_cashier_stall cs, tb_cashier_device cd where t.type = 0 and cd.stall = cs.id and t.terminal_number = cd.sn and dh.id = cs.dining_tall and t.status=1 and dh.schoolcode='{schoolcode}' {where} group by cs.id,dh.id,dh.name,cs.name,dh.schoolcode";

            
          

            using (var db = DbFactory.GetSqlSugarClient())
            {
               


                if (pageSize != -1)
                {
                    string pagewhere = @"   limit  " + pageIndex  + "," + pageSize;
                   
                   
                    SQL += pagewhere;
                    var dr = db.Ado.SqlQuery<Cashier_trade_detil>(SQL);
                    return dr;
                }
                else
                {

                    string SQL2 = $@"select count(*) from  (select 1 from {stablename} t , tb_cashier_dining_hall dh, tb_cashier_stall cs, tb_cashier_device cd where t.type = 0 and cd.stall = cs.id and t.terminal_number = cd.sn and dh.id = cs.dining_tall and t.status=1 and dh.schoolcode='{schoolcode}' {where} group by cs.id,dh.id,dh.name,cs.name,dh.schoolcode) a ";
                    totalRecordNum = db.Ado.GetInt(SQL2);
                    return null;
                }
                  
               
              
            }

        }



        /// <summary>
        /// 导出资金管理列表
        /// </summary>
        /// <returns></returns>
        public List<Cashier_trade_detil> getCapitalListexcel(string dining_hall, string stall, string stime, string etime, string schoolcode)
        {
            string where = "";

            if (!IsNullOrWhiteSpace(dining_hall) && dining_hall != "0") where += $" and dh.id =" + dining_hall;

            if (!IsNullOrWhiteSpace(stall) && stall != "0") where += $" and cd.stall = " + stall;

            stime = stime + " 00:00:00";
            etime = etime + " 23:59:59";
            DateTime st = Convert.ToDateTime(stime);
            DateTime et = Convert.ToDateTime(etime);

            string stablename = "";

            string SQL = "";

            stablename = "tb_cashier_trade_order";

            where += $" and finish_time >= '" + st + "' and finish_time <= '" + et + "' AND terminal_number<>'' ";




            SQL = $@"select dh.name as shop,cs.name as stall,COUNT(0) as totalNum,SUM(t.pay_amount) as totalOrderPrice,SUM(t.paid) as totalPayment,COALESCE((select SUM(ct.refund) from {stablename} ct where ct.stall = cs.id and ct.shop = dh.id and dh.schoolcode='{schoolcode}' and ct.type = 1 {where}),0) totalRefund,(select COUNT(0) from {stablename} ct where ct.stall = cs.id and ct.shop = dh.id and dh.schoolcode='{schoolcode}' and ct.type = 1 {where})totalRefundCount,SUM(pay_amount) -COALESCE((select SUM(ct.refund) from {stablename} ct where ct.stall = cs.id and ct.shop = dh.id and dh.schoolcode='{schoolcode}' and ct.type = 1 {where}),0) totalPrice,0.00 as totalBoard,sum(paid - pay_amount) as totalFavorable from {stablename} t , tb_cashier_dining_hall dh, tb_cashier_stall cs, tb_cashier_device cd where t.type = 0 and cd.stall = cs.id and t.terminal_number = cd.sn and dh.id = cs.dining_tall and t.status=1 and dh.schoolcode='{schoolcode}' {where} group by cs.id,dh.id,dh.name,cs.name,dh.schoolcode";




            using (var db = DbFactory.GetSqlSugarClient())
            {

                    var dr = db.Ado.SqlQuery<Cashier_trade_detil>(SQL);
                    return dr;
            }

        }

        /// <summary>
        /// 获取该学校的SN
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<Cashier_SN> GetSN(string code)
        {
            string sql = "select trade_name,terminal_number from tb_cashier_trade_order where finish_time>='2019-03-27 00:00:00' and finish_time<='2019-03-27 23:59:59' and remark='103417' and terminal_number<>'' group by trade_name,terminal_number ";

            using (var db = DbFactory.GetSqlSugarClient())
            {

                var dr = db.Ado.SqlQuery<Cashier_SN>(sql);
                return dr;
            }
        }








    }



   

}