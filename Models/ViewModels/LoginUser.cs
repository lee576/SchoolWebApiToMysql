using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginUser
    {
        /// <summary>
        /// 学校编码
        /// </summary>           
        public string schoolcode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>           
        public string userName { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>           
        public string loginuser { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>           
        public string password { get; set; }

        /// <summary>
        /// 角色
        /// </summary>           
        public int? roletype { get; set; }

        /// <summary>
        /// 食堂
        /// </summary>           
        public string dining_talls { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>           
        public string menus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>           
        public string remark { get; set; }
    }
}
