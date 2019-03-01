using DbModel;
using Infrastructure.Service;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IService
{
    public interface Itb_Building_Room_ConfigService : IServiceBase<tb_building_room_config>
    {
        IEnumerable<tb_building_room_config> FindPublicArea(int pageIndex, int pageSize, ref int total,
                    Expression<Func<tb_building_room_config, bool>> expression,
                    string school_code = "");
    }
}