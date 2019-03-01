using DbModel;
using IService;
using Infrastructure.Service;
using System.Collections.Generic;
using Infrastructure;

namespace Service
{
    public class tb_ammeterService : GenericService<tb_ammeter>,Itb_ammeterService
    {
        public IEnumerable<tb_ammeter> CheckAmmeterInfo(string schoolcode, string meterAddr)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var dt = db.Ado.SqlQuery<tb_ammeter>("select a.* from tb_ammeter a inner join tb_building_room_config b on a.room_id=b.id where b.school_id = " + schoolcode + "and a.MeterAddr='"+meterAddr+"'");
                return dt;
            }
        }
    }
}