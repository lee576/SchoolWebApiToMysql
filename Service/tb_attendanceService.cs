﻿using System;
using System.Collections.Generic;
using DbModel;
using Infrastructure;
using IService;
using Infrastructure.Service;
using Models.ViewModels;
using SqlSugar;

namespace Service
{
    public class tb_attendanceService : GenericService<tb_attendance>, Itb_attendanceService
    {
        public IEnumerable<veiw_attendance> UnSignIned(long teamId)
        {
            var startTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            var endTime = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var result = db.Queryable<tb_team_user, tb_school_user>((teamUser, schooluser) =>
                        new object[]
                        {
                            JoinType.Inner, teamUser.joinUserid == schooluser.student_id
                        })
                    .Where((teamUser, schooluser) =>
                        SqlFunc.Subqueryable<tb_attendance>()
                            .Where(attendance =>
                                attendance.joinUserid == teamUser.joinUserid &&
                                attendance.attendanceTime >= SqlFunc.ToDate(startTime) &&
                                attendance.attendanceTime <= SqlFunc.ToDate(endTime)).NotAny() &&
                        teamUser.teamID == teamId)
                    .OrderBy((teamUser, schooluser) => schooluser.student_id, OrderByType.Desc)
                    .Select((teamUser, schooluser) =>
                        new veiw_attendance
                        {
                            id = teamUser.id,
                            student_id = schooluser.student_id,
                            user_name = schooluser.user_name
                        })
                    .ToList();
                return result;
            }
        }

        public IEnumerable<veiw_attendance> SignIned(long teamId)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var startTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                var endTime = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                var result = db.Queryable<tb_attendance, tb_school_user>((attendance, schooluser) =>
                        new object[]
                        {
                            JoinType.Inner, attendance.joinUserid == schooluser.student_id
                        })
                    .Where((attendance, schooluser) =>
                        attendance.teamID == teamId && 
                        attendance.attendanceTime >= SqlFunc.ToDate(startTime) &&
                        attendance.attendanceTime <= SqlFunc.ToDate(endTime))
                    .OrderBy((attendance, schooluser) => attendance.attendanceTime, OrderByType.Desc)
                    .Select((attendance, schooluser) =>
                        new veiw_attendance
                        {
                            id = attendance.id,
                            student_id = schooluser.student_id,
                            user_name = schooluser.user_name,
                            attendanceTime = attendance.attendanceTime,
                            attendanceType = attendance.attendanceType
                        })
                    .ToList();
                return result;
            }
        }
        /// <summary>
        /// 根据code获得学校签到信息
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public IEnumerable<Attendance> GetAttendanceInfoToSchoolCode(int pageIndex, int pageSize, ref int total, string schoolcode,
            string nameorid="",string teamNameorTeamiD="",string stime="",string etime="")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                //db.Ado.SerializerDateFormat
               
                var result = db.Queryable<tb_attendance, tb_team, tb_school_user>((a, t,u) =>
                       new object[]
                       {
                            JoinType.Inner, a.teamID == t.id,
                            JoinType.Inner, a.joinUserid == u.student_id
                       })
                  .Where((a,t,u)=>t.schoolCode==schoolcode)
                  .WhereIF(!string.IsNullOrEmpty(nameorid), (a, t, u) => a.joinUserid == nameorid || u.user_name == nameorid)
                  .WhereIF(!string.IsNullOrEmpty(teamNameorTeamiD), (a, t, u) => a.teamID.ToString() == teamNameorTeamiD || t.teamName == teamNameorTeamiD)
                  .WhereIF(!string.IsNullOrEmpty(stime)&& !string.IsNullOrEmpty(etime), (a, t, u) => a.attendanceTime >= SqlFunc.ToDate(stime) && a.attendanceTime <= SqlFunc.ToDate(etime))
                  .OrderBy(a => a.attendanceTime, OrderByType.Desc)
                    
                   .Select((a, t,u) =>
                       new Attendance
                       {
                           id = a.id.ToString(),
                           teamID = a.teamID.ToString(),
                           joinUserid = a.joinUserid.ToString(),
                           attendanceTime = a.attendanceTime,
                           attendanceType = a.attendanceType.ToString(),
                           teamName = t.teamName,
                           user_name = u.user_name

                       })
                    .ToPageList(pageIndex, pageSize, ref total);
                return result;
            }
        }
        public IEnumerable<Attendance> GetAttendanceInfo(string schoolcode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                //db.Ado.SerializerDateFormat
                var result = db.Ado.SqlQuery<Attendance>(@"select a.*,t.teamName,u.[user_name] from tb_attendance a inner join tb_team t on a.teamID = t.id 

                              inner join tb_school_user u on a.joinUserid = u.student_id

                              where u.school_id = @code",new {code = schoolcode });
                return result;
            }
        }
        public IEnumerable<Attendance> GetAttendanceInfo2(string schoolcode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                //db.Ado.SerializerDateFormat
                var result = db.Ado.SqlQuery<Attendance>(@"SELECT
	                                a.id,
	                                a.teamID,
	                                a.joinUserid,
	                                a.attendanceTime,
	                                CASE a.attendanceType
                                WHEN 0 THEN
	                                '签到'
                                ELSE
	                                '签退'
                                END AS attendanceType,
                                 t.teamName,
                                 u.user_name
                                FROM
	                                tb_attendance a
                                INNER JOIN tb_team t ON a.teamID = t.id
                                INNER JOIN tb_school_user u ON a.joinUserid = u.student_id
                                WHERE
	                                u.school_id = '10045';

                                SELECT
	                                *
                                FROM
	                                tb_attendance;

                                SELECT
	                                *
                                FROM
	                                tb_school_user
                                WHERE
	                                student_id = @code", new { code = schoolcode });
                return result;
            }
        }
    }
}