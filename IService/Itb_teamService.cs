using System;
using System.Collections.Generic;
using DbModel;
using Infrastructure.Service;
namespace IService
{
    public interface Itb_teamService : IServiceBase<tb_team>
    {
        /// <summary>
        /// 参与的群组
        /// </summary>
        /// <param name="schoolCode"></param>
        /// <param name="joinUserId"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        IEnumerable<tb_team> JoinedTeams(string schoolCode, string joinUserId, DateTime now);

        /// <summary>
        /// 发起的群组
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        IEnumerable<tb_team> LaunchTeams(string userid, DateTime now);
    }
}