using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class payment_Orders
    {
        /// <summary>
        /// 私钥（商家的PrivateKey）
        /// </summary>
        public string PrivateKey { get; set; }
        /// <summary>
        /// 订单名称
        /// </summary>
        public string OrderName { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 商家对应的支付宝公钥
        /// </summary>
        public string AlipayPublicKey { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 配套的AppId
        /// </summary>
        public string AppId { get; set; }
    }
}
