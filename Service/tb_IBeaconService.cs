using System.Collections.Generic;
using DbModel;
using Infrastructure;
using IService;
using Infrastructure.Service;
namespace Service
{
    public class tb_IBeaconService : GenericService<tb_ibeacon>,Itb_IBeaconService
    {
        public List<string> GetIBeaconList(string schoolCode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                return db.Queryable<tb_ibeacon>()
                    .Where(t => t.schoolCode == schoolCode)
                    .Select(t => t.uuid).ToList();
            }
        }
        public List<tb_ibeacon> GetIBeaconInfoToPageList(int pageIndex, int pageSize,ref int total,string schoolCode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var tb = db.Queryable<tb_ibeacon>()
                    .Where(t => t.schoolCode == schoolCode)
                    .Select<tb_ibeacon>()
                  .ToPageList(pageIndex, pageSize, ref total);
                return tb;
            }
        }
    }
}