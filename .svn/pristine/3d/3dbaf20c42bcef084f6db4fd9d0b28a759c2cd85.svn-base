using DbModel;
using Infrastructure;
using Infrastructure.Service;
using IService;
using Models.ViewModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Service
{
    public class banding_dormitoryService : GenericService<BandingDormitory>, Ibanding_dormitoryService
    {
        public IEnumerable<BandingDormitory> FindUnBandingDormitory(int pageIndex, int pageSize, ref int total,
           string school_code = "", string floor_no = "", string room_no = "", string studentIdentity = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var queryAble = db.Queryable<tb_school_user>("it")
                    .In(it => it.user_id, db.Queryable<tb_school_user_room>("userroom").Where("it.user_id = user_id")
                    .WhereIF(!string.IsNullOrEmpty(school_code), schoolroom => schoolroom.school_code == SqlFunc.ToInt32(school_code))
                    .WhereIF(!string.IsNullOrEmpty(floor_no), schoolroom => schoolroom.floor_no == SqlFunc.ToInt32(floor_no))
                    .WhereIF(!string.IsNullOrEmpty(room_no), schoolroom => schoolroom.room_no == SqlFunc.ToInt32(room_no))
                    .Select(q => q.user_id))
                    .Select(schooluser => schooluser.user_id);
                //SqlSugar对not in支持的不够，只好分两次来查询
                var array = queryAble.ToList();
                //先查询出 not in 哪些值，然后再执行not in
                var pageResult = db.Queryable<tb_school_user>()
                    .Where(it => !array.Contains(it.user_id))
                    .WhereIF(!string.IsNullOrEmpty(studentIdentity), schooluser =>
                            (SqlFunc.ToString(schooluser.student_id) == studentIdentity || schooluser.user_name == studentIdentity))
                    .WhereIF(!string.IsNullOrEmpty(school_code), schooluser => schooluser.school_id == school_code)
                    .Select(schooluser =>
                        new BandingDormitory
                        {
                            user_id = SqlFunc.ToString(schooluser.user_id),
                            user_name = schooluser.user_name,
                            student_id = SqlFunc.ToString(schooluser.student_id),
                        })
                    .ToPageList(pageIndex, pageSize, ref total);

                var sql = queryAble.ToSql();
                return pageResult;
            }
        }

        public IEnumerable<BandingDormitory> FindBandingDormitory(int pageIndex, int pageSize, ref int total,
           string school_code = "", string floor_no = "", string room_no = "", string studentIdentity = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var pageResult = db.Queryable<tb_school_user_room, tb_school_user, tb_building_room_config, tb_building_room_config>(
                    (userroom, schooluser, floorconfig, roomconfig) =>
                    new object[] {
                        JoinType.Inner,userroom.user_id == SqlFunc.ToString(schooluser.user_id) &&
                                       userroom.school_code == SqlFunc.ToInt32(schooluser.school_id),
                        JoinType.Inner,userroom.floor_no == floorconfig.id,
                        JoinType.Inner,userroom.room_no == roomconfig.id
                    })
                    .WhereIF(!string.IsNullOrEmpty(school_code),
                            (userroom, schooluser) => userroom.school_code == SqlFunc.ToInt32(school_code))
                    .WhereIF(!string.IsNullOrEmpty(floor_no),
                            (userroom, schooluser) => userroom.floor_no == SqlFunc.ToInt32(floor_no))
                    .WhereIF(!string.IsNullOrEmpty(room_no),
                            (userroom, schooluser) => userroom.room_no == SqlFunc.ToInt32(room_no))
                    .WhereIF(!string.IsNullOrEmpty(studentIdentity), (userRoom, schooluser) =>
                            (schooluser.student_id == studentIdentity || schooluser.user_name == studentIdentity))
                    .OrderBy((userroom, schooluser, floorconfig, roomconfig) => schooluser.student_id, OrderByType.Desc)
                    .Select((userroom, schooluser, floorconfig, roomconfig) =>
                     new BandingDormitory
                     {
                         id = SqlFunc.ToInt32(userroom.id),
                         user_id = userroom.user_id,
                         user_name = schooluser.user_name,
                         student_id = schooluser.student_id,
                         floor_name = floorconfig.building_room_no,
                         room_name = roomconfig.building_room_no,
                         access_code = SqlFunc.ToString(roomconfig.id)
                     }).ToPageList(pageIndex, pageSize, ref total);
                return pageResult;
            }
        }
    }
}