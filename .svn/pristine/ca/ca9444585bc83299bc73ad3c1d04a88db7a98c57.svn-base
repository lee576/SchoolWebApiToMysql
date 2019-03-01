using DbModel;
using IService;
using Infrastructure.Service;
using System.Collections.Generic;
using Infrastructure;
using System;
using System.Linq.Expressions;
using SqlSugar;

namespace Service
{
    public class tb_school_deviceService : GenericService<tb_school_device>, Itb_school_deviceService
    {

        public List<tb_school_device> GetList(int pageIndex, int pageSize, string schoolCode, string deviceCode,
                                                string deviceName, ref int intTotalRecords)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                return db.Queryable<tb_school_device, tb_entrance_record, tb_school_user>(
                    (sd, er, su) =>
                    new object[] {
                        JoinType.Left,er.device_id==sd.device_id,
                        JoinType.Left,er.user_id==su.user_id
                    }
                )

                .WhereIF(!string.IsNullOrWhiteSpace(deviceCode), (sd, er, su) => sd.device_id.Contains(deviceCode))
                .WhereIF(!string.IsNullOrWhiteSpace(deviceName), (sd, er, su) => sd.device_name.Contains(deviceName))
                .GroupBy((sd, er, su) => sd.id)
                .GroupBy((sd, er, su) => sd.school_id)
                .GroupBy((sd, er, su) => sd.device_id)
                .GroupBy((sd, er, su) => sd.device_name)
                .GroupBy((sd, er, su) => sd.device_state)
                .GroupBy((sd, er, su) => sd.shop_id)
                .Having((sd, er, su) => sd.school_id == schoolCode)
                .ToPageList(pageIndex, pageSize, ref intTotalRecords);
            }
        }
    }
}