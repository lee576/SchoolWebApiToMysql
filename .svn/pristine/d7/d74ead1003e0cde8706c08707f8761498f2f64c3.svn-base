using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using SqlSugar;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_xiyun_notifyService : IServiceBase<tb_xiyun_notify>
    {
        /// <summary>
        /// 根据userid查看当天交易次数
        /// </summary>
        /// <returns></returns>
        int CountToUserid(string userid,string Mids);
        int CountToUserid2(string userid, string Mids);
        List<string> getMIDlist();
        /// <summary>
        /// 喜云使用mid次数
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="mids">可以是多个用‘，’隔开</param>
        /// <returns></returns>
        List<xiyunMidUse_CountModel> GetxiyunMidCountInfo(string stime, string etime, string mids);
        int GetxiyunGetCardStudentPayInfo(string stime, string etime,List<tb_school_user> schooluserlist,string YYMM);

        /// <summary>
        /// 禧云交易流水推送的消息通知
        /// </summary>
        /// <param name="xiyun_notify"></param>
        /// <param name="cashier_trade_order"></param>
        void Notify(tb_xiyun_notify xiyun_notify, tb_cashier_trade_order cashier_trade_order);
    }
}