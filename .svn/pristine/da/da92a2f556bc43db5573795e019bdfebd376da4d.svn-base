using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_userinfoService : IServiceBase<tb_userinfo>
    {
        List<tb_userinfo_view> GetUserList(int pageIndex, int pageSize, ref int intTotalRecords, string schoolCode, string userNameOrLoginuserOrRole);
        bool IsExistUser(string loginuser, string schoolcode);
    }
}