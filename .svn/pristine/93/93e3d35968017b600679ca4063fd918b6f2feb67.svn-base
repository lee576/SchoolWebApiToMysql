using DbModel;
using IService;
using Infrastructure.Service;
using System.Collections.Generic;
using Models.ViewModels;
using Infrastructure;
using SqlSugar;
namespace Service
{
    public class tb_cashier_deviceService : GenericService<tb_cashier_device>,Itb_cashier_deviceService
    {
        public List<Cashier_device> Getinfo(string queryparm, string hallid, string tallid, string sn, string school_id, int iDisplayStart, int iDisplayLength, ref int total)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var result = db.Queryable<tb_cashier_device, tb_cashier_stall, tb_cashier_dining_hall>((d, a, t) =>
                     new object[]
                     {
                            JoinType.Left,d.stall==a.id,
                            JoinType.Left,a.dining_tall==t.id
                     }).Where((d, a, t) => t.schoolcode == school_id)
                     .WhereIF(!string.IsNullOrEmpty(hallid), (d, a, t) => t.id ==SqlFunc.ToInt32(hallid))
                     .WhereIF(!string.IsNullOrEmpty(tallid), (d, a, t) =>d.stall == SqlFunc.ToInt32(tallid))
                     .WhereIF(!string.IsNullOrEmpty(queryparm), (d, a, t) =>t.name.Contains(queryparm) || a.name.Contains(queryparm))
                     .Select((d, a, t) => new Cashier_device { dining_hall_id=t.id, stall_id=a.id, deviceid=d.id, diningName=t.name, stallName=a.name,SN=d.sn }).ToPageList(iDisplayStart, iDisplayLength, ref total);
                
                return result;
            }
        }
    }
}