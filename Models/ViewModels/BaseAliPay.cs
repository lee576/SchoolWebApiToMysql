using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    /// <summary>
    /// 公共响应参数
    /// </summary>
    public class BaseAliPay
    {
        /// <summary>
        /// 网关返回码,详见文档
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 网关返回码描述,详见文档
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 业务返回码，参见具体的API接口文档
        /// </summary>
        public string sub_code { get; set; }
        /// <summary>
        /// 业务返回码描述，参见具体的API接口文档
        /// </summary>
        public string sub_msg { get; set; }
        /// <summary>
        /// 签名,详见文档
        /// </summary>
        public string sign { get; set; }
    }
}
