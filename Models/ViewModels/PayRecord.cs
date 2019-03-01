using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    /// <summary>
    /// 支付记录应缴的和常规的记录
    /// </summary>
    public class PayRecord
    {
        public string out_order_no { get; set; }
        public string name { get; set; }
        public decimal receipt_amount { get; set; }
        public DateTime pay_time { get; set; }
        public string student_id { get; set; }
        public string passport { get; set; }
    }
}
