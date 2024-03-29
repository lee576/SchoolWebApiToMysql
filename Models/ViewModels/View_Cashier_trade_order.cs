﻿using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

using System.Linq;

using Models;



namespace Models.ViewModels
{
    class View_Cashier_trade_order
    {
    }



    public class Getcashier_trade_orderSum
    {

        public List<double> pays { get; set; }
        public List<int> ordercount { get; set; }
        public List<string> array_alldays { get; set; }


    }


    /// <summary>
    /// 设备订单统计
    /// </summary>
    public class Cashier_trade_orderSum
    {
        public string days { get; set; }

        public double paysum { get; set; }

        public int orderCount { get; set; }

    }
    /// <summary>
    /// 交易流水
    /// </summary>
    public class Cashier_trade_Flowing
    {
        public int id { get; set; } 

        public string order { get; set; }

        public string user_code { get; set; }
        public string name { get; set; }

        public string shop { get; set; }

        public string stall { get; set; }

        public string machine { get; set; }

       public double paid { get; set; }

        public double refund { get; set; }

        public int status { get; set; }

        public string datetime { get; set; }

        public string payer_account { get; set; }

        public double pay_amount { get; set; }

    }

    /// <summary>
    /// 下载交易明细
    /// </summary>
    public class Cashier_trade_FlowingExcel
    {
        public string alipay_order { get; set; }

        public string order { get; set; }

        public string create_time { get; set; }
        public string finish_time { get; set; }

        public string status { get; set; }

        public string dining_name { get; set; }

        public string stall_name { get; set; }

        public string sn { get; set; }

        public string payer_account { get; set; }

        public string user_code { get; set; }

        public double pay_amount { get; set; }

        public double paid { get; set; }

        public double refund { get; set; }

        public string name { get; set; }

    }

    /// <summary>
    /// 对账单下载
    /// </summary>
    public class Cashier_trade_Bill
    {
        public string alipay_order { get; set; }

        public string order { get; set; }

        public string type { get; set; }
        public string trade_name { get; set; }

        public string create_time { get; set; }

        public string finish_time { get; set; }

        public string stall { get; set; }

        public string name { get; set; }

        public string operators { get; set; }

        public string terminal_number { get; set; }

        public string payer_account { get; set; }

        public double pay_amount { get; set; }

        public double paid { get; set; }

        public double refund { get; set; }

        public double alipay_red { get; set; }

        public double collection_treasure { get; set; }

        public double alipay_discount { get; set; }

        public double ticket_money { get; set; }

        public string ticket_name { get; set; }

        public double merchant_red_consumption { get; set; }

        public double card_consumption { get; set; }

        public string refund_batch_number { get; set; }

        public double service_charge { get; set; }

        public double shares_profit { get; set; }

        public string remark { get; set; }

    }

    /// <summary>
    /// 档口下拉
    /// </summary>
    public class Stall
    {
       public int id { get; set; }
        public string name { get; set; }

    }

    /// <summary>
    /// 资金管理统计
    /// </summary>
    public class Cashier_trade_Totalt
    {
        public double totalReundMoney { get; set; }

        public double totalPayAmount { get; set; }

        public double totalMoney { get; set; }

        public double totalReund { get; set; }

        public int totalOrder { get; set; }

    }

    /// <summary>
    /// 资金管理详情
    /// </summary>
    public class Cashier_trade_detil
    {
        public string shop { get; set; }

        public string stall { get; set; }

        public int totalNum { get; set; }
        
        public double totalBoard { get; set; }

        public double totalOrderPrice { get; set; }

        public double totalPrice { get; set; }


        public double totalPayment { get; set; }

        public double totalRefund { get; set; }

        public double totalRefundCount { get; set; }

        public double totalFavorable { get; set; }
    }


    public class Cashier_SN
    {
        public string trade_name { get; set; }
        public string terminal_number { get; set; }

    }

    /// <summary>
    /// 食堂管理中的设备列表
    /// </summary>
    public class Cashier_device
    {
      
        public int dining_hall_id { get; set; }

        public int stall_id { get; set; }

        public int deviceid { get; set; }

        public string diningName { get; set; }

        public string stallName { get; set; }

        public string SN { get; set; }

        public int brand { get; set; }
    }

}
