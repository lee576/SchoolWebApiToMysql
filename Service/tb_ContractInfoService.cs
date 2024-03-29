﻿using DbModel;
using Infrastructure;
using Infrastructure.Service;
using IService;
using Models.ViewModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public partial class tb_ContractInfoService : GenericService<tb_ContractInfo>, Itb_ContractInfoService
    {

        #region 签约管理列表数据
        /// <summary>
        /// 签约管理列表数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="schoolCode_Name"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public IEnumerable<ContractInfoViewModel> FindContracInfoList(int pageIndex, int pageSize, ref int total, string schoolCode_Name = "", string startTime = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var list = db.Queryable<tb_ContractInfo, tb_school_info>((c, si) => new object[] {
                  JoinType.Inner,c.school_code==si.School_Code  })
                   .WhereIF(!string.IsNullOrEmpty(schoolCode_Name), (c, si) => si.School_name.Contains(schoolCode_Name) || c.contractName.Contains(schoolCode_Name))
                   .WhereIF(!string.IsNullOrEmpty(startTime), (c, si) => SqlFunc.StartsWith(SqlFunc.ToString(c.contractTime), startTime))
                   .OrderBy((c, si) => c.contractTime, OrderByType.Desc)
              .Select((c, si) => new ContractInfoViewModel
              {
                  contractId = c.contractId,
                  contractName = c.contractName,
                  schoolName = si.School_name,
                  contractTime = c.contractTime,
              })
              .ToPageList(pageIndex, pageSize, ref total);
                return list;
            }
        }

        #endregion

        #region 获取学校的卡的领取未领取数据
        /// <summary>
        /// 获取学校的卡的领取未领取数据
        /// </summary>
        /// <param name="school_Code"></param>
        /// <returns></returns>
        public SchoolCardInfoViewModel FindSchoolCardInfoBySchoolCode(string school_Code)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                int registered_count = db.Queryable<tb_school_user>().Where(it => it.card_state == 1 && it.school_id == school_Code).ToList().Count;
                int unregistered_count = db.Queryable<tb_school_user>().Where(it => it.card_state == 0 && it.school_id == school_Code).ToList().Count;
                SchoolCardInfoViewModel infoViewModel = new SchoolCardInfoViewModel();
                infoViewModel.unRegisteredCcount = unregistered_count;
                infoViewModel.RegisteredCcount = registered_count;
                //领卡返佣公式 2 * X （支付宝给的变动值）（X=5）*领卡人数（查询月）
                infoViewModel.CardCommission = 2 * 5 * registered_count;
                //.缴费大厅返佣公式 2 * X * 线上缴费人数（缴费多少笔都算1笔，X=2）（查询月）
                return infoViewModel;
            }
        }

        #endregion

        #region 根据学校merchantCode获取学校流水数据
        public IEnumerable<TransactionFlowingViewModel> FindTransactionFlowingInfo(string stime, string merchantCode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                List<TransactionFlowingViewModel> info = new List<TransactionFlowingViewModel>();
                string YYYYMM = DateTime.Parse(stime).ToString("yyyyMM");
                string sTime = DateTime.Parse(stime).ToString("yyyy-MM");

                string sql = $"SELECT COUNT(ID) AS ID, ISNULL(SUM(receivableAmount),0)  AS receivableAmount " +
                    $"FROM tb_xiyun_notify where  CONVERT(varchar(100),tradeFinishedTime, 23) like '" + sTime + '%' + "' and merchantCode = '" + merchantCode + "'";
                var list = db.Ado.GetDataTable(sql);
                foreach (SqlSugar.DataRow item in list.Rows)
                {
                    TransactionFlowingViewModel model = new TransactionFlowingViewModel();
                    model.TransactionCount = int.Parse(item["ID"].ToString());
                    model.receivableAmountSum = double.Parse(item["receivableAmount"].ToString());
                    info.Add(model);
                }
                return info;
            }
        }
        #endregion

        #region 根据签约人获取学校领卡及流水数据（当前月）
        /// <summary>
        /// 根据签约人获取学校领卡及流水数据（当前月）
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public SchoolCardFlowingViewModel FindSchoolCardFlowingInfo(string ali_user_Id, string stime)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                stime = stime + "-01 00:00:00";
                DateTime st = Convert.ToDateTime(stime);

                //得出第一个月份中的最后一天 
                DateTime et = st.AddDays(1 - st.Day).Date.AddMonths(1).AddSeconds(-1);

      
                

                SchoolCardFlowingViewModel info = new SchoolCardFlowingViewModel();

                string where = " and contractTime >= '" + st + "' and contractTime<='" + et + "'";

              
                string sqlcount = "select * from tb_contractinfo where  ali_user_Id='" + ali_user_Id + "'";

                string sqlSchoolCode = $"SELECT DISTINCT (school_code)AS school_code FROM tb_ContractInfo WHERE ali_user_Id='" + ali_user_Id + "'";

               
               

                if (stime!="")
                {
                    sqlcount += where;
                    sqlSchoolCode += where;
                }
                 

                var listcount= db.Ado.SqlQuery<ContractInfo>(sqlcount);
                info.SignCount = listcount.Count;
           
              
               


                var SchoolCodelist = db.Ado.GetDataTable(sqlSchoolCode);
                List<string> cInfoList = new List<string>();
                foreach (SqlSugar.DataRow item in SchoolCodelist.Rows)
                {
                    string schoolCode = item["school_code"].ToString();
                    cInfoList.Add(schoolCode);
                }
                foreach (var item in cInfoList)
                {
                    string school_Code = item;
                    string sqlCollarcardInfo = $"SELECT COUNT(*) FROM tb_school_user WHERE card_state=1  AND school_id = '" + school_Code + "'";
                    string sqlUnCollarcardInfo = $"SELECT COUNT(*) FROM tb_school_user WHERE card_state=0  AND school_id = '" + school_Code + "'";
                    string stsql = $"SELECT COUNT(DISTINCT x.merchantCode) AS ID ,  ISNULL(SUM(x.receivableAmount),0) AS receivableAmount" +
                     $" FROM tb_xiyun_notify x INNER JOIN dbo.tb_school_info si ON x.merchantCode=si.xiyunMCode " +
                     $"WHERE  si.School_Code ='" + school_Code + "'";

                    if (stime!="")
                    {
                        sqlCollarcardInfo += " and collarcard_time>='"+st+"' and collarcard_time<='"+et+"'";
                        sqlUnCollarcardInfo+= " and collarcard_time>='" + st + "' and collarcard_time<='" + et + "'";
                        stsql += " x.tradeFinishedTime>='"+ st + "' and x.tradeFinishedTime<='"+et+"'";

                    }

                    var CollarcardInfoCount = db.Ado.SqlQuery<int>(sqlCollarcardInfo);//领卡人数
                    info.RegisteredCcount += CollarcardInfoCount[0];

                
                    var UnCollarcardInfoCount = db.Ado.SqlQuery<int>(sqlUnCollarcardInfo);
                    info.unRegisteredCcount += UnCollarcardInfoCount[0];//未领卡人数

                 
                    var list = db.Ado.GetDataTable(stsql);
                    foreach (SqlSugar.DataRow citem in list.Rows)
                    {
                        info.canteenCount += int.Parse(citem["ID"].ToString());//食堂交易总数
                        info.canteen += double.Parse(citem["receivableAmount"].ToString());//食堂交易总金融
                    }

                    string sqlWater = $"SELECT  COUNT(DISTINCT pw.userId) AS id , ISNULL(SUM(pw.posPay),0) AS posPay" +
                                  $" FROM tb_payment_waterrent pw INNER JOIN tb_school_user su ON pw.userId = su.user_id" +
                                  $" WHERE CONVERT(varchar(100),pw.posDataTime, 23) like '" + stime + '%' + "' AND su.school_id = '" + school_Code + "'";
                    var waterlist = db.Ado.GetDataTable(sqlWater);
                    foreach (SqlSugar.DataRow aitem in waterlist.Rows)
                    {
                        info.waterCount += int.Parse(aitem["id"].ToString());//水费交易总数（线下交易总笔数）
                        info.water += double.Parse(aitem["posPay"].ToString());//水费总金融
                    }
                    string sqlelectricity = $" SELECT COUNT(DISTINCT schoolcode) AS id ,ISNULL(SUM(pay_amount),0) AS payAmount FROM tb_payment_electricitybills " +
                                    $"WHERE CONVERT(varchar(100),pay_time, 23) like '" + stime + '%' + "' AND schoolcode = '" + school_Code + "'";
                    var electricitylist = db.Ado.GetDataTable(sqlelectricity);
                    foreach (SqlSugar.DataRow eitem in electricitylist.Rows)
                    {
                        info.electricityCount += int.Parse(eitem["id"].ToString());//电费交易总数
                        info.electricity += double.Parse(eitem["payAmount"].ToString());//电费总金融
                    }
                    string sqlpayment = $"SELECT COUNT(DISTINCT tpi.schoolcode) AS id,ISNULL(SUM(t.receipt_amount),0) AS receiptAmount FROM tb_payment_record t  " +
                        $"INNER JOIN tb_payment_item tpi ON t.payment_id = tpi.id  " +
                        $"WHERE CONVERT(varchar(100),t.pay_time, 23) like '" + stime + '%' + "' AND tpi.schoolcode = '" + school_Code + "'";
                    var paymentlist = db.Ado.GetDataTable(sqlpayment);
                    foreach (SqlSugar.DataRow ritem in paymentlist.Rows)
                    {
                        info.payMentCount += int.Parse(ritem["id"].ToString());//缴费大厅交易总数
                        info.payMent += double.Parse(ritem["receiptAmount"].ToString());//缴费大厅交易总金融
                    }
                }

                info.TransactionCount = info.waterCount + info.electricityCount + info.payMentCount + info.canteenCount;//交易总数
                info.TransactionAmountSum = info.water + info.electricity + info.payMent + info.canteen;
                //交易总金额
                //当前月总体佣金获取信息
                //①领卡佣金 公式 2 * X （支付宝给的变动值）（X=5）*领卡人数（查询月）
                info.CardCommission = 2 * 5 * info.RegisteredCcount;
                //②缴费大厅 2 * X * 线上缴费人数（缴费多少笔都算1笔，X=2）（查询月）
                info.PayMentCommission = 2 * 2 * (info.waterCount + info.electricityCount + info.payMentCount);
                info.OnlineTradingCount = info.waterCount + info.electricityCount + info.payMentCount;
                return info;
            }
        }
        #endregion

    }
}
