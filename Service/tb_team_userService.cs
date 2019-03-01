using System.Collections.Generic;
using DbModel;
using Infrastructure;
using IService;
using Infrastructure.Service;
using SqlSugar;

namespace Service
{
    public class tb_team_userService : GenericService<tb_team_user>,Itb_team_userService
    {
        public IEnumerable<tb_school_user> FindTeamUser(string teamId)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var result = db.Queryable<tb_team_user, tb_school_user>(
                    (teamUser, schooluser) =>
                        new object[]
                        {
                            JoinType.Inner, teamUser.joinUserid == schooluser.student_id
                        })
                    .Where((teamUser, schooluser) => teamUser.teamID == SqlFunc.ToInt64(teamId))
                    .Select((teamUser, schooluser) =>
                    schooluser).ToList();
                return result;
            }
        }
    }
}