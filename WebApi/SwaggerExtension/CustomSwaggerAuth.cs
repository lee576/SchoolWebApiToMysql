using SchoolWebApi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolWebApi.SwaggerExtension
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomSwaggerAuth
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomSwaggerAuth() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        public CustomSwaggerAuth(string userName, string userPwd)
        {
            UserName = userName;
            UserPwd = userPwd;
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserPwd { get; set; }
        //加密字符串
        /// <summary>
        /// 
        /// </summary>
        public string AuthStr => Sha256Helper.HMACSHA256(UserName + UserPwd);
    }
}
