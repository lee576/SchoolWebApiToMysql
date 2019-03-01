using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class AliPayAPIModel
    {
        public AliPayAPIModel()
        {
            product_code = "QUICK_MSECURITY_PAY";
            timeout_express = "90m";
            out_trade_no = DateTime.Now.ToString("yyyyMMddHHmmssffff") + new Random().Next(1000).ToString().PadLeft(4, '0');
            extend_params = new extend_params();
        }
        /// <summary>
        /// 【可选】对一笔交易的具体描述信息。如果是多种商品，请将商品描述字符串累加传给body。
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 【必选】商品的标题/交易标题/订单标题/订单关键字等。
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// 【必选】(已赋值)商户网站唯一订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 【可选】(已赋值)该笔订单允许的最晚付款时间，逾期将关闭交易。取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。 该参数数值不接受小数点， 如 1.5h，可转换为 90m。注：若为空，则默认为15d。
        /// </summary>
        public string timeout_express { get; set; }
        /// <summary>
        /// 【必填】订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]
        /// </summary>
        public string total_amount { get; set; }
        /// <summary>
        /// 【必填】(为固定值已赋值)销售产品码，商家和支付宝签约的产品码，为固定值QUICK_MSECURITY_PAY
        /// </summary>
        public string product_code { get; set; }
        /// <summary>
        /// 【可选】商品主类型：0—虚拟类商品，1—实物类商品 注：虚拟类商品不支持使用花呗渠道
        /// </summary>
        //public string goods_type { get; set; }
        /// <summary>
        /// 【可选】公用回传参数，如果请求时传递了该参数，则返回给商户时会回传该参数。支付宝会在异步通知时将该参数原样返回。本参数必须进行UrlEncode之后才可以发送给支付宝
        /// </summary>
        //public string passback_params { get; set; }
        /// <summary>
        /// 【可选】优惠参数注：仅与支付宝协商后可用
        /// </summary>
        //public string promo_params { get; set; }
        /// <summary>
        /// 【可选】业务扩展参数
        /// </summary>
        public extend_params extend_params { get; set; }
        /// <summary>
        /// 【可选】可用渠道，用户只能在指定渠道范围内支付当有多个渠道时用“,”分隔 注：与disable_pay_channels互斥
        /// </summary>
        //public string enable_pay_channels { get; set; }
        /// <summary>
        /// 【可选】禁用渠道，用户不可用指定渠道支付当有多个渠道时用“,”分隔注：与enable_pay_channels互斥
        /// </summary>
        //public string disable_pay_channels { get; set; }
        /// <summary>
        /// 【可选】商户门店编号。该参数用于请求参数中以区分各门店，非必传项。
        /// </summary>
        //public string store_id { get; set; }
    }
    /// <summary>
    /// 业务扩展参数
    /// </summary>
    public class extend_params
    {
        public extend_params()
        {
            sys_service_provider_id = "2088131404015935";
        }
        /// <summary>
        /// 【可选】系统商编号 该参数作为系统商返佣数据提取的依据，请填写系统商签约协议的PID
        /// </summary>
        public string sys_service_provider_id { get; set; }
        /// <summary>
        /// 【可选】是否发起实名校验 T：发起 F：不发起
        /// </summary>
        //public string needBuyerRealnamed { get; set; }
        /// <summary>
        /// 【可选】账务备注 注：该字段显示在离线账单的账务备注中
        /// </summary>
        //public string TRANS_MEMO { get; set; }
        /// <summary>
        /// 【可选】花呗分期数（目前仅支持3、6、12）
        /// </summary>
        //public string hb_fq_num { get; set; }
        /// <summary>
        /// 【可选】卖家承担收费比例，商家承担手续费传入100，用户承担手续费传入0，仅支持传入100、0两种，其他比例暂不支持
        /// </summary>
        //public string hb_fq_seller_percent { get; set; }
    }
    //public class alipay_trade_app_pay
    //{
    //    public alipay_trade_app_pay()
    //    {
    //        timeout_express = "90m";
    //        out_trade_no = DateTime.Now.ToString("yyyyMMddHHmmssffff") + new Random().Next(1000).ToString().PadLeft(4, '0');
    //        extend_params = new extend_params();
    //    }
    //    /// <summary>
    //    /// 【可选】该笔订单允许的最晚付款时间，逾期将关闭交易。取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。 该参数数值不接受小数点， 如 1.5h，可转换为 90m。
    //    /// </summary>
    //    public string timeout_express { get; set; }
    //    /// <summary>
    //    /// 【可选】订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]
    //    /// </summary>
    //    public string total_amount { get; set; }
    //    /// <summary>
    //    /// 【可选】收款支付宝用户ID。 如果该值为空，则默认为商户签约账号对应的支付宝用户ID
    //    /// </summary>
    //    public string seller_id { get; set; }
    //    /// <summary>
    //    /// 【可选】销售产品码，商家和支付宝签约的产品码
    //    /// </summary>
    //    public string product_code { get; set; }
    //    /// <summary>
    //    /// 【可选】对一笔交易的具体描述信息。如果是多种商品，请将商品描述字符串累加传给body。
    //    /// </summary>
    //    public string body { get; set; }
    //    /// <summary>
    //    /// 【可选】商品的标题/交易标题/订单标题/订单关键字等。
    //    /// </summary>
    //    public string subject { get; set; }
    //    /// <summary>
    //    /// 【可选】(不赋值默认自动生成)商户网站唯一订单号
    //    /// </summary>
    //    public string out_trade_no { get; set; }
    //    /// <summary>
    //    /// 【可选】绝对超时时间，格式为yyyy-MM-dd HH:mm。
    //    /// </summary>
    //    public string time_expire { get; set; }
    //    /// <summary>
    //    /// 【可选】商品主类型 :0-虚拟类商品,1-实物类商品
    //    /// </summary>
    //    public string goods_type { get; set; }
    //    /// <summary>
    //    /// 优惠参数 注：仅与支付宝协商后可用
    //    /// </summary>
    //    public string promo_params { get; set; }
    //    /// <summary>
    //    /// 【可选】公用回传参数，如果请求时传递了该参数，则返回给商户时会回传该参数。支付宝只会在同步返回（包括跳转回商户网站）和异步通知时将该参数原样返回。本参数必须进行UrlEncode之后才可以发送给支付宝。
    //    /// </summary>
    //    public string passback_params { get; set; }
    //    /// <summary>
    //    /// 【可选】描述分账信息，json格式，详见分账参数说明
    //    /// </summary>
    //    public royalty_info royalty_info { get; set; }
    //    /// <summary>
    //    /// 【必选】分账明细的信息，可以描述多条分账指令，json数组。
    //    /// </summary>
    //    public royalty_detail_infos royalty_detail_infos { get; set; }
    //    /// <summary>
    //    /// 【可选】业务扩展参数
    //    /// </summary>
    //    public extend_params extend_params { get; set; }
    //    /// <summary>
    //    /// 【可选】间连受理商户信息体，当前只对特殊银行机构特定场景下使用此字段
    //    /// </summary>
    //    public sub_merchant sub_merchant { get; set; }
    //    /// <summary>
    //    /// 【可选】可用渠道，用户只能在指定渠道范围内支付 当有多个渠道时用“,”分隔注，与disable_pay_channels互斥
    //    /// </summary>
    //    public string enable_pay_channels { get; set; }
    //    /// <summary>
    //    /// 【可选】商户门店编号
    //    /// </summary>
    //    public string store_id { get; set; }
    //    /// <summary>
    //    /// 指定渠道，目前仅支持传入pcredit 若由于用户原因渠道不可用，用户可选择是否用其他渠道支付。 注：该参数不可与花呗分期参数同时传入
    //    /// </summary>
    //    public string specified_channel { get; set; }
    //    /// <summary>
    //    /// 禁用渠道，用户不可用指定渠道支付 当有多个渠道时用“,”分隔注，与enable_pay_channels互斥
    //    /// </summary>
    //    public string disable_pay_channels { get; set; }
    //    /// <summary>
    //    /// 【可选】描述结算信息，json格式，详见结算参数说明
    //    /// </summary>
    //    public settle_info settle_info { get; set; }
    //    /// <summary>
    //    /// 【可选】开票信息
    //    /// </summary>
    //    public invoice_info invoice_info { get; set; }
    //    /// <summary>
    //    /// 【可选】外部指定买家
    //    /// </summary>
    //    public ext_user_info ext_user_info { get; set; }
    //    /// <summary>
    //    /// 商户传入业务信息，具体值要和支付宝约定，应用于安全，营销等参数直传场景，格式为json格式
    //    /// </summary>
    //    public string business_params { get; set; }
    //}
    ///// <summary>
    ///// 外部指定买家
    ///// </summary>
    //public class ext_user_info
    //{
    //    /// <summary>
    //    /// 姓名 注： need_check_info=T时该参数才有效
    //    /// </summary>
    //    public string name { get; set; }
    //    /// <summary>
    //    /// 手机号 注：该参数暂不校验
    //    /// </summary>
    //    public string mobile { get; set; }
    //    /// <summary>
    //    /// 身份证：IDENTITY_CARD、护照：PASSPORT、军官证：OFFICER_CARD、士兵证：SOLDIER_CARD、户口本：HOKOU等。如有其它类型需要支持，请与蚂蚁金服工作人员联系。 注： need_check_info=T时该参数才有效
    //    /// </summary>
    //    public string cert_type { get; set; }
    //    /// <summary>
    //    /// 证件号 注：need_check_info=T时该参数才有效
    //    /// </summary>
    //    public string cert_no { get; set; }
    //    /// <summary>
    //    /// 允许的最小买家年龄，买家年龄必须大于等于所传数值 注： 1.need_check_info=T时该参数才有效 2. min_age为整数，必须大于等于0
    //    /// </summary>
    //    public string min_age { get; set; }
    //    /// <summary>
    //    /// 是否强制校验付款人身份信息 T:强制校验，F：不强制
    //    /// </summary>
    //    public string fix_buyer { get; set; }
    //    /// <summary>
    //    /// 是否强制校验身份信息 T:强制校验，F：不强制
    //    /// </summary>
    //    public string need_check_info { get; set; }

    //}
    //public class invoice_info
    //{
    //    public invoice_info()
    //    {
    //        key_info = new key_info();
    //    }
    //    /// <summary>
    //    /// 【必填】开票关键信息
    //    /// </summary>
    //    public key_info key_info { get; set; }
    //}
    ///// <summary>
    ///// 开票关键信息
    ///// </summary>
    //public class key_info
    //{
    //    /// <summary>
    //    /// 【必填】该交易是否支持开票
    //    /// </summary>
    //    public bool is_support_invoice { get; set; }
    //    /// <summary>
    //    /// 【必填】开票商户名称：商户品牌简称|商户门店简称
    //    /// </summary>
    //    public string invoice_merchant_name { get; set; }
    //    /// <summary>
    //    /// 【必填】税号
    //    /// </summary>
    //    public string tax_num { get; set; }
    //    /// <summary>
    //    /// 【必填】开票内容
    //    /// </summary>
    //    public string details { get; set; }
    //}
    ///// <summary>
    ///// 描述结算信息，json格式，详见结算参数说明
    ///// </summary>
    //public class settle_info
    //{
    //   public settle_info()
    //    {
    //        settle_detail_infos = new settle_detail_infos();
    //    }
    //    /// <summary>
    //    /// 【必填】结算详细信息，json数组，目前只支持一条。
    //    /// </summary>
    //    public settle_detail_infos settle_detail_infos { get; set; }

    //}
    ///// <summary>
    ///// 【必填】结算详细信息，json数组，目前只支持一条。
    ///// </summary>
    //public class settle_detail_infos
    //{
    //    /// <summary>
    //    /// 【必填】结算收款方的账户类型。 cardSerialNo：结算收款方的银行卡编号。 目前只支持cardSerialNo账户类型
    //    /// </summary>
    //    public string trans_in_type { get; set; }
    //    /// <summary>
    //    /// 【必填】结算收款方。当结算收款方类型是cardSerialNo时，本参数为用户在支付宝绑定的卡编号
    //    /// </summary>
    //    public string trans_in { get; set; }
    //    /// <summary>
    //    /// 【可选】结算汇总维度，按照这个维度汇总成批次结算，由商户指定。 目前需要和结算收款方账户类型为cardSerialNo配合使用
    //    /// </summary>
    //    public string summary_dimension { get; set; }
    //    /// <summary>
    //    /// 【必填】结算的金额，单位为元。目前必须和交易金额相同
    //    /// </summary>
    //    public string amount { get; set; }
    //}
    ///// <summary>
    ///// 间连受理商户信息体，当前只对特殊银行机构特定场景下使用此字段
    ///// </summary>
    //public class sub_merchant
    //{
    //    /// <summary>
    //    /// 【必填】间连受理商户的支付宝商户编号，通过间连商户入驻后得到。间连业务下必传，并且需要按规范传递受理商户编号。
    //    /// </summary>
    //    public string merchant_id { get; set; }
    //    /// <summary>
    //    /// 【可选】商户id类型 alipay: 支付宝分配的间连商户编号, merchant: 商户端的间连商户编号
    //    /// </summary>
    //    public string merchant_type { get; set; }
    //}
    ///// <summary>
    ///// 描述分账信息，json格式，详见分账参数说明
    ///// </summary>
    //public class royalty_info
    //{
    //    /// <summary>
    //    /// 【可选】分账类型 卖家的分账类型，目前只支持传入ROYALTY（普通分账类型）。
    //    /// </summary>
    //    public string royalty_type { get; set; }
    //}
    ///// <summary>
    ///// 分账明细的信息，可以描述多条分账指令，json数组。
    ///// </summary>
    //public class royalty_detail_infos
    //{
    //    /// <summary>
    //    /// 【可选】分账序列号，表示分账执行的顺序，必须为正整数
    //    /// </summary>
    //    public int serial_no { get; set; }
    //    /// <summary>
    //    /// 【可选】接受分账金额的账户类型： userId：支付宝账号对应的支付宝唯一用户号。 bankIndex：分账到银行账户的银行编号。目前暂时只支持分账到一个银行编号。 storeId：分账到门店对应的银行卡编号。 默认值为userId。	
    //    /// </summary>
    //    public string trans_in_type { get; set; }
    //    /// <summary>
    //    /// 【必填】分账批次号 分账批次号。 目前需要和转入账号类型为bankIndex配合使用。
    //    /// </summary>
    //    public string batch_no { get; set; }
    //    /// <summary>
    //    /// 【可选】商户分账的外部关联号，用于关联到每一笔分账信息，商户需保证其唯一性。 如果为空，该值则默认为“商户网站唯一订单号+分账序列号”
    //    /// </summary>
    //    public string out_relation_id { get; set; }
    //    /// <summary>
    //    /// 【必填】要分账的账户类型。 目前只支持userId：支付宝账号对应的支付宝唯一用户号。 默认值为userId。
    //    /// </summary>
    //    public string trans_out_type { get; set; }
    //    /// <summary>
    //    /// 【必填】如果转出账号类型为userId，本参数为要分账的支付宝账号对应的支付宝唯一用户号。以2088开头的纯16位数字。
    //    /// </summary>
    //    public string trans_out { get; set; }
    //    /// <summary>
    //    /// 【必填】如果转入账号类型为userId，本参数为接受分账金额的支付宝账号对应的支付宝唯一用户号。以2088开头的纯16位数字。 如果转入账号类型为bankIndex，本参数为28位的银行编号（商户和支付宝签约时确定）。 如果转入账号类型为storeId，本参数为商户的门店ID。
    //    /// </summary>
    //    public string trans_in { get; set; }
    //    /// <summary>
    //    /// 【必填】分账的金额，单位为元
    //    /// </summary>
    //    public int amount { get; set; }
    //    /// <summary>
    //    /// 【可选】分账描述信息
    //    /// </summary>
    //    public string desc { get; set; }
    //    /// <summary>
    //    /// 【可选】分账的比例，值为20代表按20%的比例分账
    //    /// </summary>
    //    public string amount_percentage { get; set; }
    //}

}
