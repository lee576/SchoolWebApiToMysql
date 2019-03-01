using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_payment_recordService : IServiceBase<tb_payment_record>
    {
        int FindRecordByPayId(string payment_id, string passport);
        (List<string> paymentList, List<string> arList,List<payment_Orders> orderList) createOrder(string payment_id, string ar_id, string alipayOrderNo, string money, string payer_id, 
                                                                        string student_id, string payer_name, string passport, string phone);
        /// <summary>
        /// 处理订单表中未完成状态的订单数据
        /// </summary>
        /// <param name="SelectedDay"></param>
        void EditOutOrdersStatus(string SelectedDay);
        /// <summary>
        /// 对账单下载
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="schoolcode"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<PaymentRecord> GetPaymentRecord(int start, int end, string schoolcode, string payment_id,string department, string star_date, string end_date,out int intCount);
    }
}