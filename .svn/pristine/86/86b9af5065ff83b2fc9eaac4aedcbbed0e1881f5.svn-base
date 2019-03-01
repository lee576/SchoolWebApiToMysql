using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class SchoolAuthToISV
    {
        
    }
    public class alipay_open_auth_token_app_response_info
    {
        public alipay_open_auth_token_app_response_info()
        {
            alipay_open_auth_token_app_response = new alipay_open_auth_token_app_response();
        }
        public alipay_open_auth_token_app_response alipay_open_auth_token_app_response { get; set; }
        public string sign { get; set; }
    }
    public class alipay_open_auth_token_app_response
    {
        public alipay_open_auth_token_app_response()
        {
            tokens = new List<alipay_open_auth_token_app_response_tokens>();
        }
        public List<alipay_open_auth_token_app_response_tokens> tokens { get; set; }
        public string code { get; set; }
        public string msg { get; set; }
    }
    public class alipay_open_auth_token_app_response_tokens
    {
        //"tokens":[{"app_auth_token":"201810BB69050373fa024bdeb0fe1e1a44273B46",
        //"app_refresh_token":"201810BB932fbb8070fb4bc88040b7dce6917X46","auth_app_id":"2018101261653314",
        //"expires_in":31536000,"re_expires_in":32140800,"user_id":"2088802940887460"}]}
        /// <summary>
        /// 通过该令牌来帮助商户发起请求，完成业务
        /// </summary>
        public string app_auth_token { get; set; }
        public string app_refresh_token { get; set; }
        /// <summary>
        /// 授权商户的AppId（如果有服务窗，则为服务窗的AppId）
        /// </summary>
        public string auth_app_id { get; set; }
        public string expires_in { get; set; }
        public string re_expires_in { get; set; }
        /// <summary>
        /// 授权者的PID
        /// </summary>
        public string user_id { get; set; }
    }
}
