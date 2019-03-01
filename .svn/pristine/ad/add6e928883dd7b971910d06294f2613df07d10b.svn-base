using DbModel;
using IService;
using Infrastructure.Service;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using SqlSugar;
using Infrastructure;

namespace Service
{
    public class tb_Building_Room_ConfigService : GenericService<tb_building_room_config>,Itb_Building_Room_ConfigService
    {
        public IEnumerable<tb_building_room_config> FindPublicArea(int pageIndex, int pageSize, ref int total,
            Expression<Func<tb_building_room_config, bool>> expression, 
            string school_code = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var page = db.Queryable<tb_building_room_config>().Where(expression).OrderBy(t => t.building_room_no,OrderByType.Asc).ToPageList(pageIndex, pageSize, ref total);
                return page;
            }
        }
    }
}