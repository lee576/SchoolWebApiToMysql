using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class alipay_marketing_card_open_Model
    {
        public class TMember_ext_info
        {
            public string birth { get; set; }	/*2016-06-27*/
            public string name { get; set; }	/*李洋*/
            public string cell { get; set; }	/*13000000000*/
            public string gende { get; set; }	/*MALE*/
        }
        public class TCard_ext_info
        {
            public class TFront_text_list
            {
                public string value { get; set; }	/*金融贸易*/
                public string label { get; set; }	/*专业*/
            }
            public class TMdcode_info
            {
                public string code_value { get; set; }	/*1KFCDY0002*/
                public string expire_time { get; set; }	/*2017-06-0916:25:53*/
                public string code_status { get; set; }	/*SUCCESS*/
                public int time_stamp { get; set; }	/*1496996459*/
            }
            public TFront_text_list[] front_text_list { get; set; }
            public string point { get; set; }	/*88*/
            public string balance { get; set; }	/*124.89*/
            public string level { get; set; }	/*VIP1*/
            public string open_date { get; set; }	/*2014-02-2021:20:46*/
            public TMdcode_info mdcode_info { get; set; }	/*TMdcode_info*/
            public string valid_date { get; set; }	/*2020-02-2021:20:46*/
            public string external_card_no { get; set; }	/*EXT0001*/
            public string front_image_id { get; set; }	/*9fxnkgt0QFmqKAl5V2BqxQAAACMAAQED*/
        }
        public class TCard_user_info
        {
            public string user_uni_id_type { get; set; }	/*UID*/
            public string user_uni_id { get; set; }	/*2088302463082075*/
        }
        //本地支付宝JSON
        public class jsonbean
        {
            public String open_card_channel_id { get; set; }	/*2088123123123123*/
            public TMember_ext_info member_ext_info { get; set; }	/*TMember_ext_info*/
            public String out_serial_no { get; set; }	/*201606270000001*/
            public TCard_ext_info card_ext_info { get; set; }	/*TCard_ext_info*/
            public String card_template_id { get; set; }	/*201606270000001*/
            public String open_card_channel { get; set; }	/*20161534000000000008863*/
            public TCard_user_info card_user_info { get; set; }	/*TCard_user_info*/
        }
        //接收JSON
        public class gotojsonbean
        {
            public String out_serial_no { get; set; } // = DateTime.Now.Ticks.ToString();// "201801080000002";                     //外部商户流水号（商户需要确保唯一性控制，类似request_id唯一请求标识） 
            public String card_template_id { get; set; } // = "20180117000000000746252000300239";                  //支付宝分配的卡模板Id（卡模板创建接口返回的模板ID） 
            public String user_uni_id { get; set; } //= "2088002443001736";           //用户唯一标识, 根据user_id_type类型来定 （目前暂支持支付宝userId） 支付宝userId说明：支付宝用户号是以2088开头的16位纯数字组成 
            public String user_uni_id_type { get; set; } // = "UID";                    //ID类型：UID， 即传值UID即可 
            public String external_card_no { get; set; } //= "EXT0001";                 //商户外部会员卡卡号 说明： 1、会员卡开卡接口，如果卡类型为外部会员卡，请求中则必须提供该参数； 2、更新、查询、删除等接口，请求中则不需要提供该参数值； 
            public String open_date { get; set; } //= "2018-01-16 15:07:46";            //会员卡开卡时间，格式为yyyy-MM-dd HH:mm:ss 
            public String valid_date { get; set; } //= "2030-02-20 21:20:46";           //会员卡有效期 
            public String name { get; set; } //="LT";                                  //String可选64姓名 李洋 
            public String gende { get; set; } //= "MALE";                             //String可选32性别（男：MALE；女：FEMALE） MALE 
            public String birth { get; set; } //="2018-01-10";                         //String可选32生日 yyyy-MM-dd 2016-06-27 
            public String cell { get; set; } //= "13000000000";                       //String可选32手机号 13000000000 
            public String dept { get; set; } //= "13000000000";    
        }
        public class CardActivateformQuery
        {
            public string biz_type { get; set; }
            public string template_id { get; set; }
            public string request_id { get; set; }
        }
    }
}
