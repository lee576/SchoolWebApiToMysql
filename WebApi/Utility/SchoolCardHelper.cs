using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel;
using IService;
using Infrastructure.Service;
using SqlSugar;
using Models.ViewModels;
using System.Text;
using Infrastructure;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api;
using  Newtonsoft.Json;
using Service;
using System.IO;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class SchoolCardHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schoolid"></param>
        /// <param name="card_add_id"></param>
        /// <returns></returns>
        public static card_template_model GetSschoolCardInfo(string schoolid,int card_add_id)
        {
            Itb_school_card_templateService _tb_school_card_templateService = new tb_school_card_templateService();

            var cardmodel = _tb_school_card_templateService.FindByClause(p => p.School_ID == schoolid && p.Card_add_ID == card_add_id);
            card_template_model m = new card_template_model();
            m.template_id = cardmodel.template_id;
            m.request_id = cardmodel.request_id;
            m.card_spec_tag = cardmodel.card_spec_tag;
            m.biz_no_prefix = cardmodel.biz_no_prefix;
            m.write_off_type = cardmodel.write_off_type;
            m.card_show_name = cardmodel.card_show_name;
            m.logo_id = cardmodel.logo_id;
            m.background_id = cardmodel.background_id;
            m.bg_color = cardmodel.bg_color;
            m.front_text_list_enable = true;
            m.front_image_enable = bool.Parse(cardmodel.front_image_enable);
            m.column_info_layout = cardmodel.column_info_layout;
            m.column_info_list = JsonConvert.DeserializeObject<card_template_model.column_info_model[]>(cardmodel.T_column_info_list);
            card_template_model.field_rule_model fm = JsonConvert.DeserializeObject<card_template_model.field_rule_model[]>(cardmodel.T_field_rule_list)[0];
            m.field_name = fm.field_name;
            m.rule_name = fm.rule_name;
            m.rule_value = fm.rule_value;
            m.card_action_list = JsonConvert.DeserializeObject<card_template_model.card_action_model[]>(cardmodel.T_card_action_list);


            return m;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public static string ModifySchoolCard(card_template_model m, string schoolcode)
        {
            AlipayMarketingCardTemplateModifyModel mm = new AlipayMarketingCardTemplateModifyModel();
            mm.RequestId = DateTime.Now.Ticks.ToString();// "2016072600000000000000009";
            mm.TemplateId = m.template_id;// "OUT_MEMBER_CARD";
            mm.CardSpecTag = m.card_spec_tag;// "COLLEGE_CARD";
            //1) 静态码 qrcode: 二维码，扫码得商户开卡传入的external_card_no
            //||barcode: 条形码，扫码得商户开卡传入的external_card_no
            //||text: 当前不再推荐使用，text的展示效果目前等价于barcode+qrcode，同时出现条形码和二维码
            mm.WriteOffType = m.write_off_type;// "dbarcode";
            //list.shop_ids = new string[] { "2015122900077000000002409504" };//会员卡上架门店id（支付宝门店id），既发放会员卡的商家门店id 
            //list.service_label_list = new string[] { "2015122900077000000002409504" };//服务Code HUABEI_FUWU：花呗服务（只有需要花呗服务时，才需要加入该标识）

            // template_style_info 
            TemplateStyleInfoDTO tsi = new TemplateStyleInfoDTO();
            tsi.CardShowName = m.card_show_name;// "校园卡";
            tsi.LogoId = m.logo_id;// "Lzx8irpNQ0e-tG1xLVrwBgAAACMAAQED";
            tsi.BackgroundId = m.background_id;// "bwVHeNvpSX60-WhcwoRSZQAAACMAAQED";
            tsi.BgColor = m.bg_color;// "rgb(55,112,179)";

            tsi.FrontTextListEnable = m.front_text_list_enable;//m.front_text_list_enable;// 
            tsi.FrontImageEnable = m.front_image_enable;// true;
            tsi.ColumnInfoLayout = m.column_info_layout;//list grid;

            mm.TemplateStyleInfo = tsi;

            //column_info_list 
            List<TemplateColumnInfoDTO> column_info_list = new List<TemplateColumnInfoDTO>();
            for (int i = 0, l = m.column_info_list.Length; i < l; i++)
            {
                var ci = new TemplateColumnInfoDTO();
                var cim = m.column_info_list[i];
                ci.Code = cim.code;// "T2018WH001";
                ci.OperateType = cim.operate_type ?? "openWeb";//"openWeb";
                ci.Title = cim.title;//"会员";
                ci.Value = cim.value;//"会员专享权益GO";
                ci.IconId = cim.IconId;
                ci.GroupTitle = cim.GroupTitle;
                ci.Tag = cim.Tag;
                MoreInfoDTO more_info = new MoreInfoDTO();
                more_info.Title = cim.more_info_title;//"会员专享权益";
                more_info.Url = cim.more_info_url;//
                ci.MoreInfo = more_info;
                column_info_list.Add(ci);
            }
            mm.ColumnInfoList = column_info_list;

            //field_rule_list
            List<TemplateFieldRuleDTO> field_rule_list = new List<TemplateFieldRuleDTO>();
            var fr = new TemplateFieldRuleDTO();
            fr.FieldName = m.field_name;// "Balance";
            fr.RuleName = m.rule_name;// "ASSIGN_FROM_REQUEST";
            fr.RuleValue = m.rule_value;// "Balance";
            field_rule_list.Add(fr);
            mm.FieldRuleList = field_rule_list;

            //card_action_list
            List<TemplateActionInfoDTO> card_action_list = new List<TemplateActionInfoDTO>();
            for (int i = 0, l = m.card_action_list.Length; i < l; i++)
            {
                var ca = new TemplateActionInfoDTO();
                var cam = m.card_action_list[i];
                ca.Code = cam.code;//"TO_TRADE_IN";
                ca.Text = cam.text;//"去支付";
                ca.Url = cam.url;//"alipays://platformapi/startapp?appId=20000056&source=2017092808978966";
                card_action_list.Add(ca);
            }
            mm.CardActionList = card_action_list;

            var response = card_template.modify(mm, schoolcode);

            if (response.Code == "10000")
            {
                Itb_school_card_templateService server = new tb_school_card_templateService();
                var card = server.FindByClause(p => p.template_id ==m.template_id);
          
                card.request_id = m.request_id;
                card.biz_no_prefix = m.biz_no_prefix;
                card.write_off_type = m.write_off_type;
                card.card_show_name = m.card_show_name;
                card.logo_id = m.logo_id;
                card.background_id = m.background_id;
                card.bg_color = m.bg_color;
                card.front_text_list_enable = m.front_text_list_enable + "";
                card.front_image_enable = m.front_image_enable + "";
                card.T_column_info_list = JsonConvert.SerializeObject(m.column_info_list);
                card.T_field_rule_list = JsonConvert.SerializeObject(new[] { new { m.field_name, m.rule_name, m.rule_value } });
                card.T_card_action_list = JsonConvert.SerializeObject(m.card_action_list);
                card.card_spec_tag = m.card_spec_tag;
                card.column_info_layout = m.column_info_layout;
              

                server.Update(card);

                
            }
            else
            {
                return response.Code + ":" + response.SubMsg;
            }

            return response.Body;
        }

        /// <summary>
        /// 会员卡开卡赋值
        /// </summary>
        /// <returns>支付宝返回的会员卡开卡结果</returns>
        public static string  OpenSchoolCard(alipay_marketing_card_open_Model.gotojsonbean gt, string access_token, string schoolcode)
        {

            alipay_marketing_card_open_Model.jsonbean list = new alipay_marketing_card_open_Model.jsonbean();
            list.out_serial_no = DateTime.Now.Ticks.ToString();// "201801080000002";                     //外部商户流水号（商户需要确保唯一性控制，类似request_id唯一请求标识） 
            list.card_template_id = gt.card_template_id;//"20180117000000000746252000300239";                  //支付宝分配的卡模板Id（卡模板创建接口返回的模板ID） 
            // card_user_info 
            alipay_marketing_card_open_Model.TCard_user_info card_user_info = new alipay_marketing_card_open_Model.TCard_user_info();
            card_user_info.user_uni_id = gt.user_uni_id;// "2088002443001736";           //用户唯一标识, 根据user_id_type类型来定 （目前暂支持支付宝userId） 支付宝userId说明：支付宝用户号是以2088开头的16位纯数字组成 
            card_user_info.user_uni_id_type = "UID";                    //ID类型：UID， 即传值UID即可 
            list.card_user_info = card_user_info;
            list.open_card_channel = "college_card_isv";
            list.open_card_channel_id = CashierConfig.PID;
            //card_ext_info
            alipay_marketing_card_open_Model.TCard_ext_info card_ext_info = new alipay_marketing_card_open_Model.TCard_ext_info();
            card_ext_info.external_card_no = gt.external_card_no;// "EXT0001";                 //商户外部会员卡卡号 说明： 1、会员卡开卡接口，如果卡类型为外部会员卡，请求中则必须提供该参数； 2、更新、查询、删除等接口，请求中则不需要提供该参数值； 
            card_ext_info.open_date = gt.open_date;//"2018-01-16 15:07:46";            //会员卡开卡时间，格式为yyyy-MM-dd HH:mm:ss 
            card_ext_info.valid_date = gt.valid_date;//"2030-02-20 21:20:46";           //会员卡有效期 
            alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list front_text_list = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list front_text_list1 = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list front_text_list2 = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list front_text_list3 = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            card_ext_info.front_text_list = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list[1];

            front_text_list = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            front_text_list.label = "姓名";
            front_text_list.value = gt.name;

            //front_text_list1 = new Model.alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            //front_text_list1.label = "性别";
            //front_text_list1.value = gt.gende.Substring(0,1) =="M"?"男":"女";//"2018000100001";// 

            front_text_list2 = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            front_text_list2.label = "学号";
            front_text_list2.value = gt.external_card_no;//"2018000100001";// 

            front_text_list3 = new alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list();
            front_text_list3.label = "班级";
            front_text_list3.value = gt.dept.Substring(gt.dept.LastIndexOf("/") + 1);//"钢琴";

            List<alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list> ft = new List<alipay_marketing_card_open_Model.TCard_ext_info.TFront_text_list>();
            ft.Add(front_text_list);
            ft.Add(front_text_list1);
            ft.Add(front_text_list2);
            ft.Add(front_text_list3);
            card_ext_info.front_text_list = ft.ToArray();

            list.card_ext_info = card_ext_info;
            // member_ext_info 
            alipay_marketing_card_open_Model.TMember_ext_info member_ext_info = new alipay_marketing_card_open_Model.TMember_ext_info();
            member_ext_info.name = gt.name;//"LT";                                  //String可选64姓名 李洋 
            member_ext_info.gende = gt.gende;//"MALE";                             //String可选32性别（男：MALE；女：FEMALE） MALE 
            member_ext_info.birth = gt.birth;//"2018-01-10";                         //String可选32生日 yyyy-MM-dd 2016-06-27 
            member_ext_info.cell = gt.cell;//"13000000000";                       //String可选32手机号 13000000000  
            list.member_ext_info = member_ext_info;

           

       
          
            IAopClient client = CashierConfig.getDefaultAopClient(schoolcode);
            AlipayMarketingCardOpenRequest request = new AlipayMarketingCardOpenRequest();
            request.BizContent = JsonConvert.SerializeObject(list);
      

            AlipayMarketingCardOpenResponse response = client.Execute(request, access_token);
            return response.Body;


        }


        /// <summary>
        /// 会员卡查询
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public static string SchoolCardQuery(alipay_marketing_card_query_Model jsonObject, string schoolcode)
        {
            IAopClient client = CashierConfig.getDefaultAopClient(schoolcode);
            AlipayMarketingCardQueryRequest request = new AlipayMarketingCardQueryRequest();
            request.BizContent =JsonConvert.SerializeObject(jsonObject);
            AlipayMarketingCardQueryResponse response = client.Execute(request);
            return response.Body;
        }







        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file"></param>
        /// <param name="imgtype"></param>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public static string UploadImage(string name, string file, string imgtype, string schoolcode)
        {
          
            ZhifubaoLogin zfbLogin = CashierConfig.dataZFBlogin(schoolcode);
            IAopClient client = CashierConfig.LoginDAL(zfbLogin);
            AlipayOfflineMaterialImageUploadRequest request = new AlipayOfflineMaterialImageUploadRequest();
            request.ImageType = imgtype;
            request.ImageName = name;
            Aop.Api.Util.FileItem ImageContent = new Aop.Api.Util.FileItem(file);
            request.ImageContent = ImageContent;
            AlipayOfflineMaterialImageUploadResponse response = client.Execute(request);
            
            return response.Body;
        }


       


        /// <summary>
        /// 创建校园卡
        /// </summary>
        /// <param name="info"></param>
        /// <param name="name"></param>
        /// <param name="layout"></param>
        /// <param name="schoolid"></param>
        /// <returns></returns>
        public static Dictionary<string, string> CreateSchoolCard(string info,string name,string layout,string schoolid)
        {
            #region 初始化模板
            card_template_model m = new card_template_model()
            {

                benefit_info = info,
                card_show_name = name,
                column_info_list = new[] {
                                new card_template_model.column_info_model()
                                {
                                    code = "T2018WH10",
                                    operate_type = "openWeb",
                                    title = "缴费大厅",
                                    value = "缴费",
                                    more_info_title = "",
                                    more_info_url = "http://www.newxiaoyuan.com/entrance/payment/jspay.ashx?schoolcode=10027&url=http%3A%2F%2Fwww.newxiaoyuan.com%2Fentrance%2Fpayment%2Fm%2Fpayment_list.aspx",
                                    GroupTitle = "在线缴费",
                                    IconId = "c-DtfgivRlaNL5VVzrkVKQAAACMAAQED",
                                    Tag = null
                                },
                                new card_template_model.column_info_model()
                                {
                                    code = "T2018WH10",
                                    operate_type = "openWeb",
                                    title = "我的账单",
                                    value = "交易记录",
                                    more_info_title = "",
                                    more_info_url = "alipays://platformapi/startapp?appId=20000168",
                                    GroupTitle = "在线缴费",
                                    IconId = "bNf9zl9lTjOgwUJqTPCT_AAAACMAAQED",
                                    Tag = null
                                }
                            },
                card_action_list = new[]
                            {
                                new card_template_model.card_action_model()
                                {
                                    code = "TO_CLOCK_IN",
                                    text = "付款码",
                                    url = "alipays://platformapi/startapp?appId=20000056"
                                },
                                new card_template_model.card_action_model()
                                {
                                    code = "TO_CLOCK_IN",
                                    text = "校园码",
                                    url = "alipays://platformapi/startapp?appId=2018081361049285"
                                }
                            },
                column_info_layout = layout//添加模版
            };
            #endregion

            Dictionary<string, string> dic = new Dictionary<string, string>();

            Aop.Api.Domain.AlipayMarketingCardTemplateCreateModel cm = new Aop.Api.Domain.AlipayMarketingCardTemplateCreateModel();
            cm.RequestId = m.request_id;// "2016072600000000000000009";
            cm.CardType = m.card_type;// "OUT_MEMBER_CARD";
            cm.CardSpecTag = m.card_spec_tag;// "COLLEGE_CARD";
            //卡号前缀
            //list.biz_no_prefix = "sh";
            //卡号随机后缀
            cm.BizNoSuffixLen = m.biz_no_suffix_len;// "12";
            //1) 静态码 qrcode: 二维码，扫码得商户开卡传入的external_card_no
            //||barcode: 条形码，扫码得商户开卡传入的external_card_no
            //||text: 当前不再推荐使用，text的展示效果目前等价于barcode+qrcode，同时出现条形码和二维码
            cm.WriteOffType = m.write_off_type;// "dbarcode";
            //会员卡上架门店id（支付宝门店id），既发放会员卡的商家门店id 
            //list.shop_ids = new string[] { "2015122900077000000002409504" };
            //服务Code HUABEI_FUWU：花呗服务（只有需要花呗服务时，才需要加入该标识）
            //list.service_label_list = new string[] { "2015122900077000000002409504" };

            // template_style_info 
            Aop.Api.Domain.TemplateStyleInfoDTO tsi = new Aop.Api.Domain.TemplateStyleInfoDTO();
            tsi.CardShowName = m.card_show_name;// "校园卡";
            tsi.LogoId = m.logo_id;// "Lzx8irpNQ0e-tG1xLVrwBgAAACMAAQED";
            tsi.BackgroundId = m.background_id;// "bwVHeNvpSX60-WhcwoRSZQAAACMAAQED";
            tsi.BgColor = m.bg_color;// "rgb(55,112,179)";
            tsi.FrontTextListEnable = true;//m.front_text_list_enable;
            tsi.FrontImageEnable = m.front_image_enable;// true;
            tsi.ColumnInfoLayout = m.column_info_layout;
            cm.TemplateStyleInfo = tsi;

            List<Aop.Api.Domain.TemplateColumnInfoDTO> column_info_list = new List<Aop.Api.Domain.TemplateColumnInfoDTO>();
            for (int i = 0, l = m.column_info_list.Length; i < l; i++)
            {
                var ci = new Aop.Api.Domain.TemplateColumnInfoDTO();
                var mci = m.column_info_list[i];
                //标准栏位：行为由支付宝统一定，同时已经分配标准Code 
                //BALANCE：会员卡余额 
                //POINT：积分 
                //LEVEL：等级 
                //TELEPHONE：联系方式 
                //自定义栏位：行为由商户定义，自定义Code码（只要无重复） 
                ci.Code = mci.code;// "T2018WH001";
                                   //1、openNative：打开二级页面，展现 more中descs
                                   //2、openWeb：打开URL
                                   //3、staticinfo：静态信息 注意： 不填则默认staticinfo； 标准code尽量使用staticinfo，例如TELEPHONE商家电话栏位就只支持staticinfo； 

                ci.OperateType = mci.operate_type ?? "openWeb";//"openWeb"; 
                ci.Title = mci.title;//"会员";
                ci.Value = mci.value;//"会员专享权益GO";
                ci.IconId = mci.IconId;
                ci.GroupTitle = mci.GroupTitle;
                ci.Tag = mci.Tag;
                Aop.Api.Domain.MoreInfoDTO more_info = new Aop.Api.Domain.MoreInfoDTO();
                more_info.Title = mci.more_info_title;//"会员专享权益";
                more_info.Url = mci.more_info_url;//"https://www.baidu.com";
                //more_info.Params = "{}";
                //more_info.descs = new string[] { "test" };
                ci.MoreInfo = more_info;
                column_info_list.Add(ci);
            }
            cm.ColumnInfoList = column_info_list;

            //field_rule_list
            List<Aop.Api.Domain.TemplateFieldRuleDTO> field_rule_list = new List<Aop.Api.Domain.TemplateFieldRuleDTO>();
            var fr = new Aop.Api.Domain.TemplateFieldRuleDTO();
            fr.FieldName = m.field_name;// "Balance";
            fr.RuleName = m.rule_name;// "ASSIGN_FROM_REQUEST";
            fr.RuleValue = m.rule_value;// "Balance";
            field_rule_list.Add(fr);
            cm.FieldRuleList = field_rule_list;

            //card_action_list
            List<Aop.Api.Domain.TemplateActionInfoDTO> card_action_list = new List<Aop.Api.Domain.TemplateActionInfoDTO>();
            for (int i = 0, l = m.card_action_list.Length; i < l; i++)
            {
                var ca = new Aop.Api.Domain.TemplateActionInfoDTO();
                var cam = m.card_action_list[i];
                ca.Code = cam.code;//"TO_TRADE_IN";
                ca.Text = cam.text;//"去支付";
                ca.Url = cam.url;//"alipays://platformapi/startapp?appId=20000056&source=2017092808978966";
                card_action_list.Add(ca);
            }
            cm.CardActionList = card_action_list;
            var response = card_template.create(cm, schoolid);
            if (response.Code == "10000")
            {
                Itb_school_card_templateService server = new tb_school_card_templateService();
                int cardid = 1;
                var tp = server.FindListByClause(p => p.School_ID == schoolid, p => p.ID, OrderByType.Desc).FirstOrDefault();
                if (tp != null)
                    cardid = (int)tp.Card_add_ID + 1;
                


                tb_school_card_template card = new tb_school_card_template();
                card.School_ID = schoolid;
                card.Card_add_ID = cardid;
                card.request_id = m.request_id;
                card.template_id = response.TemplateId;
                card.card_type = m.card_type;
                card.biz_no_prefix = m.biz_no_prefix;
                card.biz_no_suffix_len = m.biz_no_suffix_len;
                card.write_off_type = m.write_off_type;
                card.card_show_name = m.card_show_name;
                card.logo_id = m.logo_id;
                card.background_id = m.background_id;
                card.bg_color = m.bg_color;
                card.front_text_list_enable = m.front_text_list_enable+"";
                card.front_image_enable = m.front_image_enable+"";
                card.T_column_info_list =JsonConvert.SerializeObject(m.column_info_list);
                card.T_field_rule_list = JsonConvert.SerializeObject(new[] { new { m.field_name, m.rule_name, m.rule_value } });
                card.T_card_action_list = JsonConvert.SerializeObject(m.card_action_list);
                card.service_label_list = JsonConvert.SerializeObject(cm.ServiceLabelList);
                card.card_spec_tag = m.card_spec_tag;
                card.column_info_layout = m.column_info_layout;


                long cid=server.Insert(card);
                dic.Add(cid.ToString(), response.Body);
            }

           return dic;


        }


    }

    /// <summary>
    /// 
    /// </summary>
    public static class card_template
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public static AlipayMarketingCardTemplateCreateResponse create(AlipayMarketingCardTemplateCreateModel m, string schoolcode)
        {
            IAopClient client = CashierConfig.getDefaultAopClient(schoolcode);

            AlipayMarketingCardTemplateCreateRequest request = new AlipayMarketingCardTemplateCreateRequest();
            request.SetBizModel(m);
            //request.SetNotifyUrl("http://" + HttpContext.Current.Request.Url.Host + "/notify_url.aspx");

            AlipayMarketingCardTemplateCreateResponse response = client.Execute(request);

            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public static AlipayMarketingCardTemplateModifyResponse modify(AlipayMarketingCardTemplateModifyModel m, string schoolcode)
        {
            IAopClient client = CashierConfig.getDefaultAopClient(schoolcode);

            AlipayMarketingCardTemplateModifyRequest request = new AlipayMarketingCardTemplateModifyRequest();
            request.SetBizModel(m);

            AlipayMarketingCardTemplateModifyResponse response = client.Execute(request);

            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public static AlipayMarketingCardDeleteResponse Delete(AlipayMarketingCardDeleteModel m, string schoolcode)
        {
            IAopClient client = CashierConfig.getDefaultAopClient(schoolcode);

            AlipayMarketingCardDeleteRequest request = new AlipayMarketingCardDeleteRequest();
            request.SetBizModel(m);

            AlipayMarketingCardDeleteResponse response = client.Execute(request);

            return response;
        }
    }

}
