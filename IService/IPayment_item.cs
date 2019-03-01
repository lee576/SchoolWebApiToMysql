﻿using DbModel;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IService
{
    public interface IPaymentItemService
    {
        IEnumerable<Payment_ARInfo> GetPaymentItemInfo(string schoolcode, string startDateTime);
        IEnumerable<Payment_item> GetPaymentItemToStudentId(string schoolcode,string studentid, string time);
        List<Paymentlist> GetPaymentlist(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode, string jfName = "");
        List<Paymentlist> GetPayment_item(string schoolCode, int id);
        List<tb_payment_record> Getpayment_itemInfoList(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode, int id, string stime = "", string etime = "", string selectinfo = "");
        string GetPaymentInfoSearch(int sEcho, int pageIndex, int pageSize, ref int total, string schoolCode, string selectinfo = "",
            string sTime = "", string eTime = "", string tb_payment_sub_adminItem = "", string classID = "", string status = "");
        List<PaymentRecord2> GetPaymentInfoSearchToExcel(string schoolCode, string selectinfo = "",
            string sTime = "", string eTime = "", string tb_payment_sub_adminItem = "", string classID = "", string status = "");
        PaymentItmeTotle GetPaymentInfoTotle(string schoolCode, string selectinfo = "",
           string sTime = "", string eTime = "", string tb_payment_sub_adminItem = "", string classID = "", string status = "");
        List<PayRecord> GetPayRecord(string schoolcode, string studentid, string time);


    }
}
