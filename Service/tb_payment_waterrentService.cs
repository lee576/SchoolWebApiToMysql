using DbModel;
using IService;
using Infrastructure.Service;
using System.Collections.Generic;
using Models.ViewModels;
using Infrastructure;
using SqlSugar;
using System;

namespace Service
{

    public class tb_payment_waterrentService : GenericService<tb_payment_waterrent>,Itb_payment_waterrentService
    {
        /// <summary>
        /// 获取水费已付款订单信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="schoolcode"></param>
        /// <param name="total"></param>
        /// <param name="ordernumber"></param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        public IEnumerable<tb_payment_waterrent> GetWaterInfo(int pageIndex, int pageSize, int schoolcode, ref int total,
         string ordernumber = "", string stime = "", string etime = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
              
                if (!string.IsNullOrWhiteSpace(stime) && !string.IsNullOrWhiteSpace(etime))
                {
                    stime = stime + " 00:00:00";
                    etime = etime + " 23:59:59";
                 
                }

                var result= db.Queryable<tb_payment_waterrent>()
                    .Where(p=>p.deptId==schoolcode)
                    .WhereIF(!string.IsNullOrEmpty(ordernumber), p => p.orderId == ordernumber)
                    .WhereIF(!string.IsNullOrEmpty(stime) && !string.IsNullOrEmpty(etime),p => p.posDataTime >= SqlFunc.ToDate(stime) && p.posDataTime <= SqlFunc.ToDate(etime))
                    .OrderBy(a => a.posDataTime, OrderByType.Desc).ToPageList(pageIndex,pageSize,ref total);


                return result;
            }
        }

        /// <summary>
        /// 返回订单总金额和总比数
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public List<WaterrentCount> GetSumOrderandSumPay(int schoolcode)
        {

            using (var db = DbFactory.GetSqlSugarClient())
            {
                var list = db.Queryable<tb_payment_waterrent>();
                int allcount = list.Count();
                int scsuess= list.Where(p => p.deptId == schoolcode && p.orderState == true).Count();
                int noscsuess = allcount- scsuess;
                 

                return list.Where(p => p.deptId == schoolcode&&p.orderState==true).Select(p => new WaterrentCount { wcount = allcount, scussecount=scsuess,noscussecount= noscsuess, sumprice = SqlFunc.AggregateSum(SqlFunc.ToDouble(p.posPay)) }).GroupBy("deptId").ToList() ; 

            }

        }







    }
}