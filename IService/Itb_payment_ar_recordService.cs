﻿using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_payment_ar_recordService : IServiceBase<tb_payment_ar_record>
    {
        payOk GetPayOk(string out_trade_no);
        List<tb_payment_ar_record> GetPaymentArRecordList(string schoolcode, string arid);
    }
}