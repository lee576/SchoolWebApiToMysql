using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class Payment_item
    {
        public int rn { get; set; }
        public string order_no { get; set; }
        public string out_order_no { get; set; }
        public string name { get; set; }
        public decimal total_amount { get; set; }
        public decimal papay_amount { get; set; }
        public decimal receipt_amount { get; set; }
        public decimal refund_amount { get; set; }
        public string pay_time { get; set; }
        public int status { get; set; }
        public string student_id { get; set; }
        public string passport { get; set; }
        public string icon { get; set; }

    }
    public class Paymentlist
    {
        public string id { get; set; }
        public string name { get; set; }
        public string is_external { get; set; }
        public string type { get; set; }
        public string target { get; set; }
        public string @fixed { get; set; }
        public string money { get; set; }
        public string introduction { get; set; }
        public string icon { get; set; }
        public string group { get; set; }
        public string method { get; set; }
        public string can_set_count { get; set; }
        public DateTime dateto { get; set; }
        public DateTime date_from { get; set; }
        public string nessary_info { get; set; }
        public string tname { get; set; }
        public string accountname { get; set; }
        public string account { get; set; }
        public string status { get; set; }
        public int count { get; set; }
        public string notify_link { get; set; }
        public string notify_key { get; set; }
        public string notify_msg { get; set; }
        public string remark { get; set; }
        public string limit { get; set; }
        public int class_id { get; set; }
        public string paystatus { get; set; }
        public string isstop { get; set; }
    }

    public class PaymentItmeTotle
    {
        public int sj_count { get; set; }
        public decimal? pay_amountTotle { get; set; }
        public int dj_count { get; set; }
        public int item_count { get; set; }
    }
}
