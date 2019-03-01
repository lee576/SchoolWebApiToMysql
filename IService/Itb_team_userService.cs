using System.Collections.Generic;
using DbModel;
using Infrastructure.Service;
namespace IService
{
    public interface Itb_team_userService : IServiceBase<tb_team_user>
    {
        IEnumerable<tb_school_user> FindTeamUser(string teamId);
    }
}