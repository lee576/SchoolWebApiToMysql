﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class Payment_ARInfo
    {
        public Payment_ARInfo()
        {


        }
        /// <summary>
        /// 学号
        /// </summary>
        public string studentid { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 身份号
        /// </summary>
        public string passport { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string ARID { get; set; }
        /// <summary>
        /// 应缴项目名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 应缴金额
        /// </summary>
        public decimal? amount { get; set; }
        /// <summary>
        /// 实缴金额
        /// </summary>
        public decimal? fact_amount { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public string pay_time { get; set; }

    }
    /// <summary>
    /// 应缴款项列表模型
    /// </summary>
    public class Payment_ARList
    {
        public string ARID { get; set; }
        public string name { get; set; }
        public int arcount { get; set; }
        public int fact_count { get; set; }
        public decimal amount { get; set; }
        public decimal fact_amount { get; set; }
        public int @status { get; set; }
        public DateTime star_date { get; set; }
    }
    /// <summary>
    /// 应缴款项根据学员信息获取列表模型
    /// </summary>
    public class Payment_ARToUserList
    {
        public string passport { get; set; }
        public string ARID { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string studentid { get; set; }
        public decimal amount { get; set; }
        public decimal fact_amount { get; set; }
        public DateTime? pay_time { get; set; }
    }
    /// <summary>
    /// 应缴款项是否缴费
    /// </summary>
    public class Payment_ARisPayment
    {//st_name passport amount fact_amount pay_time
        public int id { get; set; }
        public string st_name { get; set; }
        public string passport { get; set; }
        public decimal amount { get; set; }
        public decimal fact_amount { get; set; }
        public DateTime pay_time { get; set; }
        public string out_order_no { get; set; }
        public string name { get; set; }
        public string stundetid { get; set; }
        public string phone { get; set; }
        public int status { get; set; }
        public string pid { get; set; }
    }
    public class PaymentARExcel
    {
       
        public string trade_no { get; set; }
        public string ARID { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string studentid { get; set; }
        public string passport { get; set; }
        public decimal? amount { get; set; }
        public decimal? fact_amount { get; set; }
        public DateTime? pay_time { get; set; }
        public string class_name { get; set; }
    }
}
