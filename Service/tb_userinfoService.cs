using DbModel;
using Infrastructure;
using Infrastructure.Service;
using IService;
using Models.ViewModels;
using SqlSugar;
using System.Collections.Generic;

namespace Service
{
    public class tb_userinfoService : GenericService<tb_userinfo>, Itb_userinfoService
    {
        public List<tb_userinfo_view> GetUserList(int pageIndex, int pageSize, ref int intTotalRecords, string schoolCode, string userNameOrLoginuserOrRole)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                return db.Queryable<tb_userinfo>()
                .Where(t => t.loginuser != "admin" && t.schoolcode == schoolCode)
                .WhereIF((!string.IsNullOrEmpty(userNameOrLoginuserOrRole)
                          && userNameOrLoginuserOrRole != "普通用户"
                          && userNameOrLoginuserOrRole != "食堂经理"
                          && userNameOrLoginuserOrRole != "财务"), 
                          t => t.userName.Contains(userNameOrLoginuserOrRole) || t.loginuser.Contains(userNameOrLoginuserOrRole))
                .WhereIF(userNameOrLoginuserOrRole == "普通用户", t => t.roletype == 0)
                .WhereIF(userNameOrLoginuserOrRole == "食堂经理", t => t.roletype == 1)
                .WhereIF(userNameOrLoginuserOrRole == "财务", t => t.roletype == 2)
                .Select(t => new tb_userinfo_view
                {
                    id = t.id,
                    userName = t.userName,
                    loginuser = t.loginuser,
                    role = SqlFunc.IF(t.roletype == 0)
                              .Return("普通用户")
                              .ElseIF(t.roletype == 1)
                              .Return("食堂经理")
                              .ElseIF(t.roletype == 2)
                              .Return("财务")
                              .End("")
                })
                .ToPageList(pageIndex, pageSize, ref intTotalRecords);
            }
        }

        public bool IsExistUser(string loginuser, string schoolcode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                return db.Queryable<tb_userinfo>().Any(t => t.loginuser == loginuser && t.schoolcode == schoolcode);
            }
        }
    }
}