﻿using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_payment_ARService : IServiceBase<tb_payment_ar>
    {
        IEnumerable<tb_payment_ar> ConstraintARIDandPassPort(List<tb_payment_ar>list);
        bool AddPayment_ARInfo(tb_payment_ar payment);
        /// <summary>
        /// 通过学生ID获取缴费信息
        /// </summary>
        /// <param name="student_id"></param>
        /// <returns></returns>
        List<tb_payment_ar> FindListByStudentId(string student_id);
        /// <summary>
        /// 创建应缴费用的订单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="alipayOrderNo">阿里订单号</param>
        /// <returns></returns>
        (List<string> paymentList, List<string> arList, List<payment_Orders> orderList) CreateOrder(tb_payment_ar model,string alipayOrderNo,string schoolCode);
        (tb_payment_ar modelAr, tb_payment_accounts modelTpa, tb_payment_ar_record modelPar) GetListByTradeNo(string TradeNo);
        bool PayOk2(string TradeNo);
        bool PayOk(string TradeNo);
        /// <summary>
        /// 通过学生ID获取需要的缴费信息（若火缴费小程序使用（非缴费大厅））
        /// </summary>
        /// <param name="studentCode"></param>
        /// <param name="student_id"></param>
        /// <returns></returns>
        List<CampusPay> FindCampusPayListByStudentId(string schoolcode, string student_id);
        /// <summary>
        /// 通过学校Code获取应缴金额、实缴金额、应缴笔数、实缴笔数、应缴项目
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        FeePayable GetFeePayableBySchoolCode(string schoolcode);
        /// <summary>
        /// 获取Payment_record 的pay_amount 总额和总行数
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        (decimal pay_amount, int count) GetPayment_recordAmountCount(string schoolcode);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        PaymentARAccount GetPaymentARAccount(string schoolcode);
        /// <summary>
        /// 柱形图 一周数据显示
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        List<WeekPayment_item> GetWeekPayment_item(string schoolcode,string stime,string etime);

        List<Payment_ARList> Getpayment_ARList(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode,string status="", string selectinfo = "");
        List<Payment_ARToUserList> Getpayment_ARListToUser(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode, string selectinfo = "");
        List<Payment_ARList> Getpayment_ARDetailed(string schoolCode, string ARID);
        List<Payment_ARisPayment> GetPayment_ARisPayment(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode, string ARID = "", string JSstatus = "", string name = "", string stime = "", string etime = "");
        List<Payment_ARisPayment> GetPayment_ARisPayment2(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode, string ARID = "", string JSstatus = "", string name = "", string stime = "", string etime = "");
        int GetPaymentARAccount_YJXM(string schoolcode);

    }
}