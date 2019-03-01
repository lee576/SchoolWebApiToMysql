using DbModel;
using IService;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;
using Infrastructure;
using SqlSugar;

namespace Service
{
    public class tb_payment_accountsService : GenericService<tb_payment_accounts>,Itb_payment_accountsService
    {
        public List<tb_payment_accountsAddaccountstatusModel> GetPayMentAccounts(string schoolcode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var pay_amount = db.Queryable<tb_payment_accounts, tb_payment_item>((pa, pi) => new object[]{
                                        JoinType.Left,SqlFunc.ToString(pa.id) == pi.account,
                                    })
                                .Where((pa, pi) => pa.schoolcode == schoolcode)
                                .Select((pa, pi) => new tb_payment_accountsAddaccountstatusModel
                                {
                                    accountstatus = SqlFunc.AggregateCount(pi.id),
                                    alipay_public_key = pa.alipay_public_key,
                                    appid = pa.appid,
                                    id = pa.id,
                                    name = pa.name,
                                    pid = pa.pid,
                                    private_key = pa.private_key,
                                    publickey = pa.publickey,
                                    schoolcode = schoolcode

                                })
                                .GroupBy(@"pi.account,pa.id,pa.schoolcode,pa.pid,pa.appid,pa.name,pa.alipay_public_key,pa.private_key,pa.publickey")
                                .ToList();

                return pay_amount;
            }
        }
        public List<tb_payment_accountsAddaccountstatusModel> GetPayMentAccountsPageList(int sEcho, int pageIndex, int pageSize, ref int totalRecordNum,string schoolcode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {

                var data = db.SqlQueryable<tb_payment_accountsAddaccountstatusModel>(@"SELECT
	                                                            pa.*, COALESCE (pi.count, 0) accountstatus
                                                            FROM
	                                                            tb_payment_accounts pa
                                                            LEFT JOIN (
	                                                            SELECT
		                                                            account,
		                                                            count(id) count
	                                                            FROM
		                                                            tb_payment_item
	                                                            GROUP BY
		                                                            account
                                                            ) pi ON pa.id = pi.account
                                                            WHERE
	                                                            pa.schoolcode = '"+ schoolcode + "'").ToPageList(pageIndex, pageSize, ref totalRecordNum); ;

                return data;
            }
        }
    }
}