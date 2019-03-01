﻿using DbModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class view_payment_item//: tb_payment_item
    {
        public int id { get; set; }
        public string name { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string iconImg { get; set; }
        public int type { get; set; }
        public string appid { get; set; }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public decimal money { get; set; }
        public int class_id { get; set; }
    }
}