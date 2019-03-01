using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Infrastructure
{
    public enum OpResultType
    {
        /// <summary>
        /// 操作没有引发任何变化，提交取消。
        /// </summary>
        [Description("操作没有引发任何变化，提交取消。")]
        NoChanged = 0,

        /// <summary>
        /// 操作成功。
        /// </summary>
        [Description("操作成功。")]
        Success = 1,

        /// <summary>
        /// 操作引发错误。
        /// </summary>
        [Description("操作引发错误。")]
        Error = 2,

        /// <summary>
        /// 指定参数的数据不存在。
        /// </summary>
        [Description("指定参数的数据不存在。")]
        QueryNull = 3,

        /// <summary>
        /// 输入信息验证失败。
        /// </summary>
        [Description("输入信息验证失败。")]
        ValidError = 4,

        /// <summary>
        /// 登录失效。
        /// </summary>
        [Description("登录失效。")]
        LoginInvalid = 5,

        /// <summary>
        /// 身份认证信息错误。
        /// </summary>
        [Description("身份认证信息错误。")]
        AuthInvalid = 6,

        /// <summary>
        /// 未登录。
        /// </summary>
        [Description("未登录。")]
        NotLoggedIn = 7
    }
}
