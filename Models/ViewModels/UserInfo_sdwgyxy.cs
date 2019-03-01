using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    /// <summary>
    /// 山东外国语学院学员信息模型
    /// </summary>
    public class UserInfo_sdwgyxy_respond
    {
        public string code { get; set; }
        public List<UserInfo_sdwgyxy> data { get; set; }
    }
    public class UserInfo_sdwgyxy
    {
        public string user_name { get; set; }
        public string pass_port { get; set; }
        public string depart_ment { get; set; }
        public string student_id { get; set; }
        public string validity_time { get; set; }
        public string class_id { get; set; }
        /// <summary>
        /// 是否多重身份
        /// </summary>
        public string isMultipleIdentities { get; set; }
    }
}
