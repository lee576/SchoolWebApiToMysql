using DbModel;
using Infrastructure;
using Infrastructure.Service;
using IService;
using Models.ViewModels;
using SqlSugar;
using System.Collections.Generic;

namespace Service
{
    public class view_entrance_recordService : GenericService<view_entrance_Record>, Iview_entrance_recordService
    {
        public IEnumerable<view_entrance_Record> FindAll(string deviceId = "", string studentIdentity = "",
            string startTime = "", string endTime = "", string schoolCode = "", string stuffType = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var pageResult = db.Queryable<tb_entrance_record, tb_school_user, tb_school_device, tb_school_info>(
                               (entrance, schooluser, device, school) =>
                               new object[] {
                                 JoinType.Inner,entrance.user_id == schooluser.user_id,
                                 JoinType.Left,entrance.device_id == device.device_id,
                                 JoinType.Inner,schooluser.school_id == school.School_Code
                               })
                             .WhereIF(!string.IsNullOrEmpty(stuffType) && stuffType != "0",
                                     (entrance, schooluser, device, school) =>
                                                schooluser.class_id == SqlFunc.ToInt32(stuffType))
                             .WhereIF(!string.IsNullOrEmpty(deviceId) && deviceId != "0",
                                     (entrance, schooluser, device, school) =>
                                                entrance.device_id == deviceId)
                             .WhereIF(!string.IsNullOrEmpty(studentIdentity),
                                     (entrance, schooluser, device, school) =>
                                                (SqlFunc.ToString(schooluser.student_id) == studentIdentity || schooluser.user_name == studentIdentity))
                             .WhereIF((!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime)),
                                     (entrance, schooluser, device, school) =>
                                                (entrance.open_time >= SqlFunc.ToDate(startTime) && entrance.open_time <= SqlFunc.ToDate(endTime)))
                             .WhereIF(!string.IsNullOrEmpty(schoolCode),
                                     (entrance, schooluser, device, school) => schooluser.school_id == schoolCode && school.School_Code == schoolCode)
                             .OrderBy((entrance, schooluser, device, school) => entrance.open_time, OrderByType.Desc)
                             .Select((entrance, schooluser, device, school) =>
                                new view_entrance_Record
                                {
                                    student_id = schooluser.student_id,
                                    user_realname = schooluser.user_name,
                                    device_id = entrance.device_id,
                                    device_name = device.device_name,
                                    school_name = school.School_name,
                                    open_time = entrance.open_time,
                                    entrance_status = entrance.entrance_status
                                })
                             .ToList();
                return pageResult;
            }
        }

        public IEnumerable<view_entrance_Record> FindAll(int pageIndex, int pageSize, ref int total,
            string deviceId = "", string studentIdentity = "",
            string startTime = "", string endTime = "", string schoolCode = "", string stuffType = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var pageResult = db.Queryable<tb_entrance_record, tb_school_user, tb_school_device, tb_school_info>(
                        (entrance, schooluser, device, school) =>
                            new object[]
                            {
                                JoinType.Inner, entrance.user_id == schooluser.user_id,
                                JoinType.Left, entrance.device_id == device.device_id,
                                JoinType.Inner, schooluser.school_id == school.School_Code
                            })
                    .WhereIF(!string.IsNullOrEmpty(stuffType) && stuffType != "0",
                        (entrance, schooluser, device, school) =>
                            schooluser.class_id == SqlFunc.ToInt32(stuffType))
                    .WhereIF(!string.IsNullOrEmpty(deviceId) && deviceId != "0",
                        (entrance, schooluser, device, school) =>
                            entrance.device_id == deviceId)
                    .WhereIF(!string.IsNullOrEmpty(studentIdentity),
                        (entrance, schooluser, device, school) =>
                            (SqlFunc.ToString(schooluser.student_id) == studentIdentity ||
                             schooluser.user_name == studentIdentity))
                    .WhereIF((!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime)),
                        (entrance, schooluser, device, school) =>
                            (entrance.open_time >= SqlFunc.ToDate(startTime) &&
                             entrance.open_time <= SqlFunc.ToDate(endTime)))
                    .WhereIF(!string.IsNullOrEmpty(schoolCode),
                        (entrance, schooluser, device, school) =>
                            schooluser.school_id == schoolCode && school.School_Code == schoolCode)
                    .OrderBy((entrance, schooluser, device, school) => entrance.open_time, OrderByType.Desc)
                    .Select((entrance, schooluser, device, school) =>
                        new view_entrance_Record
                        {
                            student_id = schooluser.student_id,
                            user_realname = schooluser.user_name,
                            device_id = entrance.device_id,
                            device_name = device.device_name,
                            school_name = school.School_name,
                            open_time = entrance.open_time,
                            entrance_status = entrance.entrance_status
                        })
                    .ToPageList(pageIndex, pageSize, ref total);
                return pageResult;
            }
        }
    }
}
