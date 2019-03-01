using DbModel;
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
                stime = stime + " 00:00:00";
                etime = etime + " 23:59:59";
                DateTime st = Convert.ToDateTime(stime);
                DateTime et = Convert.ToDateTime(etime);
                string sql = "";

                List<double> pays = new List<double>();
                List<int> ordercount = new List<int>();
                List<string> array_alldays = new List<string>();

                //如果在同一个月份当中
                if (st.Month == et.Month)
                {
                    string tablename = "tb_cashier_trade_order";

                sql = @"select DATE_FORMAT(create_time,'%Y-%m-%d') days,COALESCE(SUM(t.paid)-sum(t.refund),0) paysum,count(*) orderCount from 
                    " + tablename + " t ,tb_cashier_device cd where t.status = 1 and cd.sn = t.terminal_number and cd.schoolcode='" + schoolcode + "' and  create_time >= '" + st + "' and create_time <= '" + et + "' group by DATE_FORMAT(create_time,'%Y-%m-%d')";


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
            }

            //else
            //{
            //    var db = DbFactory.GetSqlSugarClient();
            //    //得到最后一个月份中的第一天
            //    DateTime first = et.AddDays(1 - et.Day).Date;
            //    //得出第一个月份中的最后一天 
            //    DateTime last = st.AddDays(1 - st.Day).Date.AddMonths(1).AddSeconds(-1);







            //    string tablename = "tb_cashier_trade_order" + et.ToString("yyyyMM");
            //    //查询后一个月中第一天到最后一天数据
            //    sql = @"select DATE_FORMAT(create_time,'%Y-%m-%d') days,COALESCE(SUM(t.paid)-sum(t.refund),0) paysum,count(*) orderCount from 
            //            " + tablename + " t ,tb_cashier_device cd where t.status = 1 and cd.sn = t.terminal_number and cd.schoolcode='" + schoolcode + "' and  create_time >= '" + first + "' and create_time <= '" + et + "' group by DATE_FORMAT(create_time,'%Y-%m-%d')";
            //    var dr = db.Ado.SqlQuery<Cashier_trade_orderSum>(sql);


            //    //先循环后一个月的数据
            //    for (DateTime dt = et; dt >= first; dt = dt.AddDays(-1))
            //    {
            //        array_alldays.Add(dt.ToString("yyyy/MM/dd"));
            //        pays.Add(dr.Where(p => p.days.ToString("yyyy/MM/dd") == dt.ToString("yyyy/MM/dd")).FirstOrDefault().paysum);
            //        ordercount.Add(dr.Where(p => p.days.ToString("yyyy/MM/dd") == dt.ToString("yyyy/MM/dd")).FirstOrDefault().orderCount);
            //    }


            //    tablename = "tb_cashier_trade_order" + st.ToString("yyyyMM");
            //    //查询后一个月中第一天到最后一天数据
            //    sql = @"select DATE_FORMAT(create_time,'%Y-%m-%d') days,COALESCE(SUM(t.paid)-sum(t.refund),0) paysum,count(*) orderCount from 
            //            " + tablename + " t ,tb_cashier_device cd where t.status = 1 and cd.sn = t.terminal_number and cd.schoolcode='" + schoolcode + "' and  create_time >= '" + first + "' and create_time <= '" + et + "' group by DATE_FORMAT(create_time,'%Y-%m-%d')";
            //    dr = db.Ado.SqlQuery<Cashier_trade_orderSum>(sql);


            //    //先循环后一个月的数据
            //    for (DateTime dt = last; dt >= st; dt = dt.AddDays(-1))
            //    {
            //        array_alldays.Add(dt.ToString("yyyy/MM/dd"));
            //        pays.Add(dr.Where(p => p.days.ToString("yyyy/MM/dd") == dt.ToString("yyyy/MM/dd")).FirstOrDefault().paysum);
            //        ordercount.Add(dr.Where(p => p.days.ToString("yyyy/MM/dd") == dt.ToString("yyyy/MM/dd")).FirstOrDefault().orderCount);
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
            stime = stime + " 00:00:00";
            etime = etime + " 23:59:59";
            DateTime st = Convert.ToDateTime(stime);
            DateTime et = Convert.ToDateTime(etime);
            string tablename = "tb_cashier_trade_order";
            string sql = @"select '1' days,COALESCE(SUM(t.paid)-sum(t.refund),0) paysum,0 orderCount from 
                    " + tablename + " t ,tb_cashier_device cd where t.status = 1 and cd.sn = t.terminal_number and cd.schoolcode='" + schoolid + "' and  create_time >= '" + st + "' and create_time <= '" + et + "'";

            using (var db = DbFactory.GetSqlSugarClient())
            {
               return db.Ado.SqlQuery<Cashier_trade_orderSum>(sql).FirstOrDefault();
            }

           


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

            string where = " ";

            if (!IsNullOrWhiteSpace(dining_hall) && dining_hall!= "0") where += $" and dh.id ="+ dining_hall;

            if (!IsNullOrWhiteSpace(stall) && stall != "0") where += $" and cd.stall = "+stall;

            if (!IsNullOrWhiteSpace(order)) where += $" and order = '"+order+"'";

            if (!IsNullOrWhiteSpace(user_code)) where += $" and su.student_id = '"+user_code+"'";

            if (!IsNullOrWhiteSpace(machine)) where += $" and cd.sn = '"+machine+"'";

            if (!IsNullOrWhiteSpace(tid)) where += $" and t.id = '"+tid+"'";

           

            string stablename = "tb_cashier_trade_order" ;
            stime = stime + " 00:00:00";
            etime = etime + " 23:59:59";
            DateTime st = Convert.ToDateTime(stime);
            DateTime et = Convert.ToDateTime(etime);


            where += $" and DATE_FORMAT(t.create_time,'%Y-%m-%d') >= '" + st + "' and DATE_FORMAT(t.create_time,'%Y-%m-%d') <= '" + et + "'";

            

            string  sql = Join(" ",
            $"select t.id,t.order,su.student_id user_code,su.user_name  name,dh.name as shop,cs.name as stall, t.terminal_number as machine, t.paid, t.refund, t.status, DATE_FORMAT(t.create_time,'%Y-%m-%d') as datetime,payer_account,pay_amount from {stablename} t left join tb_school_user su on su.ali_user_id = payer_account and school_id = '"+ schoolcode + "' and card_state = 1 LEFT JOIN tb_cashier_dining_hall dh on su.school_id = dh.schoolcode LEFT JOIN  tb_cashier_stall cs on cs.dining_tall = dh.id LEFT JOIN tb_cashier_device cd  on cd.stall = cs.id ",
            $"{where} ");

            string sql2 = Join(" ",
          $"select count(*) from {stablename} t left join tb_school_user su on su.ali_user_id = payer_account and school_id = '" + schoolcode + "' and card_state = 1 LEFT JOIN tb_cashier_dining_hall dh on su.school_id = dh.schoolcode LEFT JOIN  tb_cashier_stall cs on cs.dining_tall = dh.id LEFT JOIN tb_cashier_device cd  on cd.stall = cs.id ",
          $"{where} ");


            using (var db = DbFactory.GetSqlSugarClient())
            {
                totalRecordNum = db.Ado.GetInt(sql2);

                if (pageIndex != -1) where += $"   limit  " + pageIndex + "," + pageSize;
                var dr = db.Ado.SqlQuery<Cashier_trade_Flowing>(sql);
                return dr;
            }

        }



        public List<Cashier_trade_FlowingExcel> GetFlowingExcel(string dining_hall, string stall, string order, string user_code, string machine, string tid, string stime, string etime, string schoolcode)
        {

            string where = " ";

            if (!IsNullOrWhiteSpace(dining_hall) && dining_hall != "0") where += $" and dh.id =" + dining_hall;

            if (!IsNullOrWhiteSpace(stall) && stall != "0") where += $" and cd.stall = " + stall;

            if (!IsNullOrWhiteSpace(order)) where += $" and order = '" + order + "'";

            if (!IsNullOrWhiteSpace(user_code)) where += $" and su.student_id = '" + user_code + "'";

            if (!IsNullOrWhiteSpace(machine)) where += $" and cd.sn = '" + machine + "'";

            if (!IsNullOrWhiteSpace(tid)) where += $" and t.id = '" + tid + "'";



            string stablename = "tb_cashier_trade_order";
            stime = stime + " 00:00:00";
            etime = etime + " 23:59:59";
            DateTime st = Convert.ToDateTime(stime);
            DateTime et = Convert.ToDateTime(etime);


           

            
            stablename = "tb_cashier_trade_order";
            where += $" and DATE_FORMAT(t.create_time,'%Y-%m-%d') >= '" + st + "' and DATE_FORMAT(t.create_time,'%Y-%m-%d') <= '" + et + "'";
            string sql = $@"select t.alipay_order,t.order,t.create_time,t.finish_time,status,dh.name as dining_name,cs.name as stall_name,cd.sn,payer_account,user_code,t.name,pay_amount,t.paid,t.refund from {stablename} t , tb_cashier_dining_hall dh, tb_cashier_stall cs, tb_cashier_device cd where cd.stall = cs.id and t.terminal_number = cd.sn and dh.id = cs.dining_tall and dh.schoolcode='{schoolcode}' {where} order by t.create_time";
            

           
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var dr = db.Ado.SqlQuery<Cashier_trade_FlowingExcel>(sql);
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

                    where += $" and create_time >= '" + st + "' and create_time <= '" + et + "'";

                }
                else
                {
                    if (!IsNullOrWhiteSpace(datetime))
                    {
                        string stime = datetime + "-01 00:00:00";

                        DateTime st = Convert.ToDateTime(stime);
                        DateTime et = st.AddDays(1 - st.Day).Date.AddMonths(1).AddSeconds(-1);

                        where += $" and create_time >= '" + st + "' and create_time <= '" + et + "'";

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
        public List<Cashier_trade_Totalt> getTotalt(string stime, string etime, string schoolcode)
        {
            string where = "1=1";



            stime = stime + " 00:00:00";
            etime = etime + " 23:59:59";
            DateTime st = Convert.ToDateTime(stime);
            DateTime et = Convert.ToDateTime(etime);
            string sql = "";
           
            string  stablename = "tb_cashier_trade_order";
            where += $" and create_time >= '" + st + "' and create_time <= '" + et + "'";
            sql = $@"select COUNT(t.order) as totalOrder,COALESCE(SUM(t.pay_amount),0) as totalPayAmount,COALESCE(SUM(t.paid),0)-COALESCE(SUM(refund),0)  totalMoney,(select COUNT(0) from {stablename} cto, tb_cashier_device cd where status=1 and cto.terminal_number = cd.sn and cd.schoolcode='{schoolcode}' and type = 1 and {where})totalReund, COALESCE(SUM(refund),0) totalReundMoney from {stablename} t, tb_cashier_device cd  where status=1 and t.terminal_number = cd.sn and cd.schoolcode='{schoolcode}' and {where}";




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

            where += $" and create_time >= '" + st + "' and create_time <= '" + et + "'";


           

           SQL = $@"select dh.name as shop,cs.name as stall,COUNT(0) as totalNum,SUM(t.pay_amount) as totalOrderPrice,SUM(t.paid) as totalPayment,COALESCE((select SUM(ct.refund) from {stablename} ct where ct.stall = cs.id and ct.shop = dh.id and dh.schoolcode='{schoolcode}' and ct.type = 1 {where}),0) totalRefund,(select COUNT(0) from {stablename} ct where ct.stall = cs.id and ct.shop = dh.id and dh.schoolcode='{schoolcode}' and ct.type = 1 {where})totalRefundCount,SUM(pay_amount) -COALESCE((select SUM(ct.refund) from {stablename} ct where ct.stall = cs.id and ct.shop = dh.id and dh.schoolcode='{schoolcode}' and ct.type = 1 {where}),0) totalPrice,0.00 as totalBoard,sum(paid - pay_amount) as totalFavorable from {stablename} t , tb_cashier_dining_hall dh, tb_cashier_stall cs, tb_cashier_device cd where t.type = 0 and cd.stall = cs.id and t.terminal_number = cd.sn and dh.id = cs.dining_tall and t.status=1 and dh.schoolcode='{schoolcode}' {where} group by cs.id,dh.id,dh.name,cs.name,dh.schoolcode";

            
          

            using (var db = DbFactory.GetSqlSugarClient())
            {
               
                    
               
                if (pageIndex != -1)
                {
                    string pagewhere = @"   limit  " + pageIndex  + "," + pageSize;
                    string SQL2 = $@"select count(*) from  (select 1 from {stablename} t , tb_cashier_dining_hall dh, tb_cashier_stall cs, tb_cashier_device cd where t.type = 0 and cd.stall = cs.id and t.terminal_number = cd.sn and dh.id = cs.dining_tall and t.status=1 and dh.schoolcode='{schoolcode}' {where} group by cs.id,dh.id,dh.name,cs.name,dh.schoolcode) a ";
                    totalRecordNum = db.Ado.GetInt(SQL2);
                    SQL += pagewhere;
                }
                  
                var dr = db.Ado.SqlQuery<Cashier_trade_detil>(SQL);
                return dr;
            }

        }


        





    }



   

}