using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class searchPayment
    {
        public int rn{ get; set; }
        public string order_no{ get; set; }
        public string out_order_no{ get; set; }
        public string name{ get; set; }
        public string total_amount{ get; set; }
        public string pay_amount{ get; set; }
        public string receipt_amount{ get; set; }
        public string refund_amount{ get; set; }
        public string pay_time{ get; set; }
        public string status{ get; set; }
        public string student_id{ get; set; }
        public string passport{ get; set; }
        public string pay_name{ get; set; }
        public string phone { get; set; }
    }
}
