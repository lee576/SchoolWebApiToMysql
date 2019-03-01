﻿using DbModel;
using IService;
using Infrastructure.Service;
using Infrastructure;
using System;
using System.Linq;
using System.Collections.Generic;
using Models.ViewModels;
using SqlSugar;

namespace Service
{
    public class tb_xiyun_notifyService : GenericService<tb_xiyun_notify>, Itb_xiyun_notifyService
    {
        public int CountToUserid(string userid, string Mids)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                string YYYYMM = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMM");
                string sql = $"select count(*) from tb_xiyun_notify{YYYYMM} where thirdUserId = '{userid}' and amount >= 0.1 and CONVERT(nvarchar(12),tradeFinishedTime,112) = CONVERT(nvarchar(12),GETDATE(),112) and merchantCode in ('" + Mids + "')";
                //string sql = $"select count(*) from tb_xiyun_notify where thirdUserId = '{userid}' and amount >= 1.5 and CONVERT(nvarchar(12),tradeFinishedTime,112) = CONVERT(nvarchar(12),GETDATE(),112) and merchantCode in ('" + Mids + "')";
                var count = db.Ado.SqlQuery<int>(sql).FirstOrDefault();
                return count;
            }
        }
        public int CountToUserid2(string userid, string Mids)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                string YYYYMM = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMM");
                string sql = $"select count(*) from tb_xiyun_notify{YYYYMM} where thirdUserId = '{userid}' and amount >= 0.1 and CONVERT(nvarchar(12),tradeFinishedTime,112) = CONVERT(nvarchar(12),GETDATE(),112) and merchantCode in ('" + Mids + "')";
                //string sql = $"select count(*) from tb_xiyun_notify where thirdUserId = '{userid}' and amount >= 1.5 and CONVERT(nvarchar(12),tradeFinishedTime,112) = CONVERT(nvarchar(12),GETDATE(),112) and merchantCode in ('" + Mids + "')";
                var count = db.Ado.SqlQuery<int>(sql).FirstOrDefault();
                return count;
            }
        }
        public List<xiyunMidUse_CountModel> GetxiyunMidCountInfo(string stime, string etime, string mids)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                List<xiyunMidUse_CountModel> list = new List<xiyunMidUse_CountModel>();
                string YYYYMM = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMM");
                string sTime = Convert.ToDateTime(stime).ToString();
                string eTime = Convert.ToDateTime(etime).ToString();
                if (mids.Contains(','))
                {
                    foreach (var item in mids.Split(','))
                    {
                        xiyunMidUse_CountModel model = new xiyunMidUse_CountModel();
                        model.mid = item;
                        string sql = $"select count(*) from tb_xiyun_notify{YYYYMM} where  tradeFinishedTime >= '" + sTime + "' and tradeFinishedTime <= '" + eTime + "' and merchantCode = '" + item + "'";
                        var count = db.Ado.SqlQuery<int>(sql).FirstOrDefault();
                        model.count = count;
                        list.Add(model);
                    }
                }
                else
                {
                    xiyunMidUse_CountModel model = new xiyunMidUse_CountModel();
                    model.mid = mids;
                    string sql = $"select count(*) from tb_xiyun_notify{YYYYMM} where  tradeFinishedTime >= '" + sTime + "' and tradeFinishedTime <= '" + eTime + "' and merchantCode = '" + mids + "'";
                    var count = db.Ado.SqlQuery<int>(sql).FirstOrDefault();
                    model.count = count;
                    list.Add(model);
                }
                return list;
            }
        }
        public int GetxiyunGetCardStudentPayInfo(string stime, string etime, List<tb_school_user> schooluserlist, string YYMM)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                string aliuserids = "";
                foreach (var item in schooluserlist)
                {
                    aliuserids += item.ali_user_id + ",";
                }
                aliuserids = aliuserids.Replace(",", "','");
                string sql = $"select count(*) from tb_xiyun_notify{YYMM} where  tradeFinishedTime >= '" + stime + "' and tradeFinishedTime <= '" + etime + "' and thirdUserId in ('" + aliuserids + "')";
                var count = db.Ado.SqlQuery<int>(sql).FirstOrDefault();
                return count;
            }
        }
        public List<string> getMIDlist()
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                string sql = $"SELECT DISTINCT merchantCode FROM tb_xiyun_notify201809";
                var dt = db.Ado.GetDataTable(sql);
                List<string> list = new List<string>();
                foreach (SqlSugar.DataRow item in dt.Rows)
                {
                    string i = item["merchantCode"].ToString();
                    list.Add(i);
                }
                string sql2 = $"SELECT DISTINCT merchantCode FROM tb_xiyun_notify201810";
                var dt2 = db.Ado.GetDataTable(sql2);
                foreach (SqlSugar.DataRow item in dt2.Rows)
                {
                    string i = item["merchantCode"].ToString();
                    list.Add(i);
                }
                string sql3 = $"SELECT DISTINCT merchantCode FROM tb_xiyun_notify201811";
                var dt3 = db.Ado.GetDataTable(sql3);
                foreach (SqlSugar.DataRow item in dt3.Rows)
                {
                    string i = item["merchantCode"].ToString();
                    list.Add(i);
                }
                list = list.Distinct().ToList();
                return list;
            }
        }

        /// <summary>
        /// 禧云交易流水推送的消息通知
        /// </summary>
        /// <param name="xiyun_notify"></param>
        /// <param name="cashier_trade_order"></param>
        public void Notify(tb_xiyun_notify xiyun_notify, tb_cashier_trade_order cashier_trade_order)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                try
                {
                    db.Ado.BeginTran();
                    db.Insertable<tb_xiyun_notify>(xiyun_notify).ExecuteCommand();
                    db.Insertable<tb_cashier_trade_order>(cashier_trade_order).ExecuteCommand();
                    db.Ado.CommitTran();
                }
                catch
                {
                    db.Ado.RollbackTran();
                }
            }
        }
    }
}