using System;
using System.Collections.Generic;
using System.Linq;
using DbModel;
using Infrastructure;
using IService;
using Infrastructure.Service;
using SqlSugar;

namespace Service
{
    public class tb_teamService : GenericService<tb_team>,Itb_teamService
    {
        /// <summary>
        /// 查找该用户参与的群组
        /// </summary>
        /// <param name="schoolCode"></param>
        /// <param name="joinUserId"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public IEnumerable<tb_team> JoinedTeams(string schoolCode, string joinUserId,DateTime now)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                //人人可参与,没有固定参与人员,发起的临时签到
                var unTemporaryQuery1 = db.Queryable<tb_team>()
                    .Where(team => team.isAddJoin == 0 && team.isTemporary == 0 && team.schoolCode == schoolCode);

                //人人可参与,没有固定参与人员,发起的非临时签到
                var unTemporaryQuery2 = db.Queryable<tb_team>()
                    .Where(team => team.isAddJoin == 0 && team.isTemporary == 1 && team.schoolCode == schoolCode);

                //用户参与的群组,临时签到
                var temporaryQuery1 = db.Queryable<tb_team, tb_team_user>(
                        (team, teamUser) =>
                            new object[]
                            {
                                JoinType.Inner, team.id == teamUser.teamID
                            })
                    .Where((team, teamUser) => teamUser.joinUserid == joinUserId && team.isTemporary == 1 && team.isAddJoin == 1 && team.schoolCode == schoolCode &&
                          SqlFunc.ToDate(team.endTime) > now)
                    .Select((team, teamUser) => team);

                //用户参与的群组,非临时签到
                var temporaryQuery2 = db.Queryable<tb_team, tb_team_user>(
                        (team, teamUser) =>
                            new object[]
                            {
                                JoinType.Inner, team.id == teamUser.teamID
                            })
                    .Where((team, teamUser) => teamUser.joinUserid == joinUserId && team.isTemporary == 0 && team.isAddJoin == 1 && team.schoolCode == schoolCode)
                    .Select((team, teamUser) => team);

                //Union All查询合并非临时群组与临时群组
                var list = db.UnionAll(unTemporaryQuery1, unTemporaryQuery2, temporaryQuery1, temporaryQuery2).ToList();
                return list.Distinct().OrderBy(t => t.isAddJoin).ThenBy(t => t.isTemporary).ThenBy(t => t.startTime).ToList();
            }
        }

        /// <summary>
        /// 查找该用户发起的签到群组
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public IEnumerable<tb_team> LaunchTeams(string userid, DateTime now)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                //非临时群组
                var teamsIsUnTemporary = db.Queryable<tb_team>().Where(t => t.userid == userid && t.isTemporary == 0);
                //过滤掉过期的临时签到群组
                var teamsIsTemporary = db.Queryable<tb_team>().Where(t => t.userid == userid && t.isTemporary == 1 &&
                                                   SqlFunc.ToDate(t.endTime) > now);
                //Union All查询合并非临时群组与临时群组
                var list = db.UnionAll(teamsIsUnTemporary, teamsIsTemporary).ToList();
                return list.OrderBy(t => t.isTemporary).ThenBy(t => t.startTime);
            }
        }
    }
}