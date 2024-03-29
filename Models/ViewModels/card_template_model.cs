﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.ViewModels
{
    //接收JSON
    public class card_template_model
    {
        /// <summary>请求ID，由开发者生成并保证唯一性 </summary>
        public string request_id { get; set; }//= "2016072600000000000000009";

        /// <summary>卡类型为固定枚举类型，可选类型如下： OUT_MEMBER_CARD：外部权益卡 </summary>
        public string card_type { get; set; }// = "OUT_MEMBER_CARD";

        /// <summary>卡特定标签，只供特定业务使用，通常接入无需关注 (COLLEGE_CARD校园卡)</summary>
        public string card_spec_tag { get; set; }//= "COLLEGE_CARD";

        public string template_id { get; set; }

        /// <summary>卡号前缀</summary>
        public string biz_no_prefix { get; set; }//= "sh";

        /// <summary>卡号随机后缀</summary>
        public string biz_no_suffix_len { get; set; }//= "12";

        public string write_off_type { get; set; }//= "dbarcode";

        /// <summary>
        /// <para>1) 静态码 qrcode: 二维码，扫码得商户开卡传入的external_card_no</para>
        /// <para>||barcode: 条形码，扫码得商户开卡传入的external_card_no</para>
        /// <para>||text: 当前不再推荐使用，text的展示效果目前等价于barcode+qrcode，同时出现条形码和二维码</para>
        /// <para>list.shop_ids = new string[] { "2015122900077000000002409504" };//会员卡上架门店id（支付宝门店id），既发放会员卡的商家门店id </para>
        /// <para>list.service_label_list = new string[] { "2015122900077000000002409504" };//服务Code HUABEI_FUWU：花呗服务（只有需要花呗服务时，才需要加入该标识）</para>
        /// </summary>
        public string card_show_name { get; set; }//= "校园卡";

        public string logo_id { get; set; }//= "Lzx8irpNQ0e-tG1xLVrwBgAAACMAAQED";

        public string background_id { get; set; }//= "bwVHeNvpSX60-WhcwoRSZQAAACMAAQED";

        public string bg_color { get; set; }//= "rgb(55,112,179)";

        public bool front_text_list_enable { get; set; }//= true;

        public string benefit_info { get; set; }

        public bool front_image_enable { get; set; }//= true;

        public string field_name { get; set; }//="Balance";

        public string rule_name { get; set; }//="ASSIGN_FROM_REQUEST";

        public string rule_value { get; set; }//= "Balance";

        public string column_info_layout { get; set; }//="list" or "grid";

        public column_info_model[] column_info_list { get; set; }

        public card_action_model[] card_action_list { get; set; }

        public card_template_model()
        {
            request_id = DateTime.Now.Ticks.ToString();
            card_type = "OUT_MEMBER_CARD";
            card_spec_tag = "COLLEGE_CARD";
            biz_no_suffix_len = "12";
            write_off_type = "none";
            logo_id = "gxsTZfoEQfuTj5yXOhl6eQAAACMAAQED";
            background_id = "ryfVpjprRzGP4LKnZ7DwigAAACMAAQED";
            bg_color = "rgb(55,112,179)";
            field_name = "Balance";
            rule_name = "ASSIGN_FROM_REQUEST";
            rule_value = "Balance";
        }

        public class field_rule_model
        {
            public string field_name { get; set; }//="Balance";

            public string rule_name { get; set; }//="ASSIGN_FROM_REQUEST";

            public string rule_value { get; set; }//= "Balance";
        }

        public class column_info_model
        {
            /// <summary>
            /// 标准栏位：行为由支付宝统一定，同时已经分配标准Code 
            /// <para>BALANCE：会员卡余额</para>
            /// <para>POINT：积分</para>
            /// <para>LEVEL：等级</para>
            /// <para>TELEPHONE：联系方式</para>
            /// <para>自定义栏位：行为由商户定义，自定义Code码（只要无重复）</para>
            /// </summary>
            public string code { get; set; }// = "T2018WH001";

            /// <summary>
            /// <para>1、openNative：打开二级页面，展现 more中descs</para>
            /// <para>2、openWeb：打开URL</para>
            /// <para>3、staticinfo：静态信息</para>
            /// <para>注意： 不填则默认staticinfo； 标准code尽量使用staticinfo，例如TELEPHONE商家电话栏位就只支持staticinfo； </para>
            /// </summary>
            public string operate_type { get; set; }// = "openWeb";

            public string title { get; set; }// = "会员";

            /// <summary>
            /// 卡包详情页面，卡栏位右边展现的值
            /// <para>TELEPHONE栏位的商家联系电话号码由此value字段传入 </para>
            /// </summary>
            public string value { get; set; }// = "会员专享权益GO";

            /// <summary>二级页面标题 </summary>
            public string more_info_title { get; set; }// = "会员专享权益";

            public string more_info_url { get; set; }//= "https://www.baidu.com";

            //public string more_info_descs { get; set; }//= new string[] { "test" };

            public string IconId { get; set; }

            public string GroupTitle { get; set; }

            public string Tag { get; set; }
        }

        public class card_action_model
        {
            /// <summary>行动点业务CODE，商户自定义 TO_CLOCK_IN </summary>
            public string code { get; set; }//= "TO_CLOCK_IN";

            /// <summary>6 行动点展示文案 打卡</summary>
            public string text { get; set; }//= "刷门禁";

            /// <summary>行动点跳转链接 https://merchant.ali.com/ee/clock_in </summary>
            public string url { get; set; }//= "https://www.baidu.com";
        }
    }

    public class card_template_deletemodel
    {
        public string out_serial_no { get; set; }
        public string target_card_no { get; set; }
        public string target_card_no_type { get; set; }
        public string reason_code { get; set; }
        public string ext_info { get; set; }
    }
    public class ext_info
    {
        public string new_card_no { get; set; }
        public string donee_user_id { get; set; }
    }
}