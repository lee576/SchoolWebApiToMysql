using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class alipay_trade_create_Result
    {
        public AliPayTradeCreateResult alipay_trade_create_response { get; set; }
        public string sign { get; set; }
    }

    public class AliPayTradeCreateResult : BaseAliPay
    {
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string trade_no { get; set; }
    }
}
