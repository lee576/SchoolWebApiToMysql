﻿using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_cashier_trade_orderService : IServiceBase<tb_cashier_trade_order>
    {
        Getcashier_trade_orderSum Getcashier_trade_order(string schoolcode, string stime, string etime);
        Cashier_trade_orderSum Getcashier_trade_orderCount(string schoolid, string stime, string etime);

        List<Cashier_trade_Flowing> GetFlowing(string dining_hall, string stall, string order, string user_code, string machine, string tid, string stime, string etime, string schoolcode, int pageIndex, int pageSize, ref int totalRecordNum);

        List<Cashier_trade_FlowingExcel> GetFlowingExcel(string dining_hall, string stall, string order, string user_code, string machine, string tid, string stime, string etime, string schoolcode);
        List<Cashier_trade_Bill> GetBillExcel(string timeType, string datetime, string schoolcode);
        List<Cashier_trade_Totalt> getTotalt(string stime, string etime, string schoolcode, string dining_hall, string stall);

        List<Cashier_trade_detil> getCapitalList(string dining_hall, string stall, string stime, string etime, string schoolcode, int pageIndex, int pageSize, ref int totalRecordNum);

        List<Cashier_trade_detil> getCapitalListexcel(string dining_hall, string stall, string stime, string etime, string schoolcode);

        List<Cashier_SN> GetSN(string code);
    }
}