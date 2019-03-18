﻿using DbModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class SchoolCord
    {
        public SchoolCord()
        {
            Province = "";
            schoolInfo = new List<tb_school_info>();
        }
        public string Province { get; set; }
        public List<tb_school_info> schoolInfo { get; set; }
    }
    public class AreaInfoList
    {
        public string letter { get; set; }
        public bool show { get; set; }
        public List<AreaInfo> data { get; set; }
    }
    public class AreaInfo
    {
        public string id { get; set; }
        public string cityName { get; set; }
        public string type { get; set; }
        public string schooltype { get; set; }
        public string schoolcode { get; set; }
    }
    public class SchoolUser
    {
        public string template_id { get; set; }
        public string card_show_name { get; set; }
        public string student_id { get; set; }
        public string card_validity { get; set; }
        public string school_id { get; set; }
        public string department { get; set; }
        public string user_id { get; set; }
    }

    /// <summary>
    /// 学生卡类型
    /// </summary>
    public class card_type_list
    {
        public string school_id { get; set; }
        public string card_type_name { get; set; }
        public string card_type_id { get; set; }
    }

    /// <summary>
    /// 校园卡分类
    /// </summary>
    public class SchoolPid
    {
        public string school_id { get; set; }

        public string pid { get; set; }
    }
    
    /// <summary>
    /// 获取会员卡领卡投放链接
    /// </summary>
    public class alipay_marketing_card_activateurl_apply
    {
        /// <summary>
        /// 【必选】会员卡模板id。使用会员卡模板创建接口(alipay.marketing.card.template.create)返回的结果
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 【可选】扩展信息，会员领卡完成后将此参数原样带回商户页面。
        /// </summary>
        public string out_string { get; set; }
        /// <summary>
        /// 【必选】会员卡开卡表单提交后回调地址。 1.该地址不可带参数，如需回传参数，可设置out_string入参。
        /// </summary>
        public string callback { get; set; }
        /// <summary>
        /// 【可选】需要关注的生活号AppId。若需要在领卡页面展示“关注生活号”提示，需开通生活号并绑定会员卡。生活号快速接入详见：https://doc.open.alipay.com/docs/doc.htm?treeId=193&articleId=105933&docType=1
        /// </summary>
        public string follow_app_id { get; set; }
    }
    /// <summary>
    /// 会员卡开卡表单模板配置
    /// </summary>
    public class alipay_marketing_card_formtemplate_set
    {
        public alipay_marketing_card_formtemplate_set()
        {
            fields = new fields();
        }
        /// <summary>
        /// 【必选】会员卡模板id。使用会员卡模板创建接口(alipay.marketing.card.template.create)返回的结果
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 【可选】会员卡开卡时的表单字段配置信息，可定义多个通用表单字段，最大不超过20个。
        /// </summary>
        public fields fields { get; set; } 
    }
    /// <summary>
    /// 【可选】会员卡开卡时的表单字段配置信息，可定义多个通用表单字段，最大不超过20个。
    /// </summary>
    public class fields
    {
        //表单必填字段配置，common_fields属性定义一个表单字段数组，字段有效值如下列表所示： 
        //OPEN_FORM_FIELD_MOBILE -- 手机号
        //OPEN_FORM_FIELD_GENDER -- 性别
        //OPEN_FORM_FIELD_NAME -- 姓名
        //OPEN_FORM_FIELD_BIRTHDAY -- 生日（不含年份，如：01-01） 
        //OPEN_FORM_FIELD_BIRTHDAY_WITH_YEAR -- 生日（含年份，如：1988-01-01） 
        //OPEN_FORM_FIELD_IDCARD -- 身份证
        //OPEN_FORM_FIELD_CERT_TYPE -- 证件类型
        //OPEN_FORM_FIELD_CERT_NO -- 证件号
        //OPEN_FORM_FIELD_EMAIL -- 邮箱
        //OPEN_FORM_FIELD_ADDRESS -- 地址
        //OPEN_FORM_FIELD_CITY -- 城市
        //OPEN_FORM_FIELD_IS_STUDENT -- 是否学生认证
        //OPEN_FORM_FIELD_MEMBER_GRADE -- 会员等级

        //注： 
        //1. 会员等级、是否学生认证字段，如果获取不到该项数据时，表单页面不做展示。 
        //2. 身份证字段和证件号、证件类型字段不可同时配置。 
        //3. 如果身份证字段不能满足业务需求，可通过配置证件类型+证件号字段来获取其他证件类型的支持，目前支持的证件类型有：身份证、护照、港澳居民通行证、台湾居民通行证
        /// <summary>
        /// 【可选】
        /// </summary>
        public string required { get; set; }
        //表单可选字段配置，common_fields属性定义一个表单字段数组，表单字段有效值列表与required字段有效值列表相同。 
        //可选字段配置中不能含有必须字段配置的有效值。
        /// <summary>
        /// 【可选】
        /// </summary>
        public string optional { get; set; }
    }
    public class alipay_marketing_card_activateform_query
    {
        /// <summary>
        /// 【必选】开放表单信息查询业务类型，可选类型如下： MEMBER_CARD -- 会员卡开卡
        /// </summary>
        public string biz_type { get; set; }
        /// <summary>
        /// 【必选】会员卡模板id。使用会员卡模板创建接口(alipay.marketing.card.template.create)返回的结果
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 【必选】查询用户表单提交信息的请求id，在用户授权表单确认提交后跳转商户页面url时返回此参数。
        /// </summary>
        public string request_id { get; set; }
    }


    public class WaterrentCount
    {
        public int wcount { get; set; }

        public double sumprice { get; set; }

        public int scussecount { get; set; }

        public int noscussecount { get; set; }

    }

    /// <summary>
    /// 卡模板
    /// </summary>
    public class CardTemplateformer
    {
        public int id { get; set; }

        public string Logo_id { get; set; }

        public string background_id { get; set; }

        public string T_column_info_list { get; set; }

        public string T_card_action_list { get; set; }

        public string column_info_layout { get; set; }

        public string schoolid { get; set; }

        public int baseinfoshow { get; set; }

        public string alipay_url { get; set; }

        public string background_url { get; set; }
    }
   

}
