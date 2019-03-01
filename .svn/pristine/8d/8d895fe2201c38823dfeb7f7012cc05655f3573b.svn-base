using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class SchoolCardInfoViewModel
    {
        public int unRegisteredCcount { get; set; }
        public int RegisteredCcount { get; set; }
        public int cardCount { get; set; }
        //public string TransactionFlowing { get; set; }
        //public string Commission { get; set; }
        //佣金返回—领卡返佣
        public double? CardCommission { get; set; }
    }

    public class TransactionFlowingViewModel
    {
        public double receivableAmountSum { get; set; }
        public int TransactionCount { get; set; }
    }

    public class SchoolDetailsInfoViewModel
    {
        public SchoolCardInfoViewModel schoolCardInfo { get; set; }
        public IEnumerable<TransactionFlowingViewModel> TransactionFlowing { get; set; }
    }

    public class SchoolCardFlowingViewModel
    {
        /// <summary>
        /// 签约学校总数
        /// </summary>
        public int SignCount { get; set; }
        /// <summary>
        /// 未领卡总数
        /// </summary>
        public int unRegisteredCcount { get; set; }
        /// <summary>
        /// 领卡总数
        /// </summary>
        public int RegisteredCcount { get; set; }
        /// <summary>
        /// 交易总金额
        /// </summary>
        public double TransactionAmountSum { get; set; }
        /// <summary>
        /// 水电费
        /// </summary>
        public double electricity { get; set; }
        public int electricityCount { get; set; }

        public double water { get; set; }

        public int waterCount { get; set; }
        /// <summary>
        /// 缴费大厅
        /// </summary>
        public double payMent { get; set; }
        public int payMentCount { get; set; }
        /// <summary>
        /// 食堂
        /// </summary>
        public double canteen { get; set; }
        /// <summary>
        /// 线下交易笔数（食堂）
        /// </summary>
        public int canteenCount { get; set; }

        /// <summary>
        /// 领卡返佣
        /// </summary>
        public double? CardCommission { get; set; }
        /// <summary>
        /// 领卡返佣
        /// </summary>
        public double? PayMentCommission { get; set; }

        /// <summary>
        /// 交易总量
        /// </summary>
        public int TransactionCount { get; set; }
       

        /// <summary>
        /// 线上交易笔数
        /// </summary>
        public int OnlineTradingCount { get; set; }

    }

    public class ContractInfo
    {
        public string schoolCode { get; set; }
        public string ali_user_id { get; set; }
    }


    public class SchoolCardList
    {
        public string card_add_id { get; set; }
        public string card_show_name { get; set; }

        public string background_url { get; set; }
        public int card_count { get; set; }

    }


    public class CardTypes
    {
        public string card_show_name { get; set; }
        public int ID { get; set; }
    }

    public class StudentSchoolCard
    {
        public string user_name { get; set; }
        public string template_id { get; set; }
        public string card_show_name { get; set; }

        public string student_id { get; set; }

        public string card_validity { get; set; }

        public string school_user_id { get; set; }

        public string department { get; set; }

        public string school_id { get; set; }

        public string welcome_flg { get; set; }
    }


    public class alipay_marketing_card_query_Model
    {
        public string target_card_no { get; set; }
        public string target_card_no_type { get; set; }
    }




}
