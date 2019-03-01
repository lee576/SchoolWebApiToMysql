using DbModel;
using Infrastructure;
using Infrastructure.Service;
using IService;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class TransactionRecordService : GenericService<TransactionRecord>, ITransactionRecordService
    {
        public IEnumerable<TransactionRecord> GetTransactionRecord(string schoolcode, string startDateTime)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                //select xiyunMcode from tb_school_info where school_code='10019' 
                //select * from tb_xiyun_notify xn where xn.merchantCode in('101726','101939') and tradeFinishedTime between '2018-09-14' and '2018-09-15'
                var xiyunMcode = db.Ado.SqlQuery<tb_school_info>("select xiyunMcode from tb_school_info where school_code='"+schoolcode+"' ");
                if (xiyunMcode.Count == 0)
                {
                    return null;
                }
                string strxiyunMcode = "";
                foreach (var item in xiyunMcode)
                {
                    strxiyunMcode += " '" + item.xiyunMCode + "',";
                }
                strxiyunMcode = strxiyunMcode.Substring(0, strxiyunMcode.Length - 1);
                string[] sp = startDateTime.Split('-');
                string tbname = "tb_xiyun_notify" + sp[0] + sp[1];
                string endTime = Convert.ToDateTime(startDateTime).AddDays(1).ToString("yyyy-MM-dd");
                string sql = @"select * from "+ tbname + " xn where xn.merchantCode in(" + strxiyunMcode+") and tradeFinishedTime between '"+ startDateTime + "' and '"+ endTime + "'";
                var data = db.Ado.SqlQuery<TransactionRecord>(sql);
                foreach (var item in data)
                {
                    item.shop = "食堂";
                    if (item.consumerCode.Equals(""))
                    {
                        item.consumerCode = "尚未领卡";
                    }
                    if (item.consumerName.Equals(""))
                    {
                        item.consumerName = "尚未领卡";
                    }
                }
                return data;
            }
        }
    }
}
