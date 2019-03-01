using DbModel;
using IService;
using Infrastructure.Service;
using System.Collections.Generic;
using Models.ViewModels;
using Infrastructure;
using SqlSugar;

namespace Service
{
    public class tb_payment_electricitybillsService : GenericService<tb_payment_electricitybills>,Itb_payment_electricitybillsService
    {
        public IEnumerable<Electricitybills>GetelectricitybillsInfo(int pageIndex, int pageSize, ref int total, int schoolcode,
        string room_id = "", string ordernumber = "", string stime = "", string etime = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                //db.Ado.SerializerDateFormat

                var result = db.Queryable<tb_payment_electricitybills, tb_building_room_config>(
                    (a, t) =>new object[]
                        {
                            JoinType.Inner, a.room_id == t.id,
                        })
                  .Where((a, t) => t.school_id == schoolcode && a.pay_status == true)
                  .WhereIF(!string.IsNullOrEmpty(room_id), (a, t) => a.room_id.ToString() == room_id)
                  .WhereIF(!string.IsNullOrEmpty(ordernumber), (a, t) => a.ordernumber == ordernumber)
                  .WhereIF(!string.IsNullOrEmpty(stime) && !string.IsNullOrEmpty(etime), (a, t) => a.pay_time >= SqlFunc.ToDate(stime) && a.pay_time <= SqlFunc.ToDate(etime))
                  .OrderBy(a => a.pay_time, OrderByType.Desc)

                   .Select((a, t) =>
                       new Electricitybills
                       {
                           id = a.id,
                           ordernumber = a.ordernumber,
                           pay_amount = a.pay_amount,
                           pay_status = a.pay_status,
                           pay_time = a.pay_time,
                           room_id = a.room_id,
                           room_name = t.building_room_no,
                           schoolcode = t.school_id.ToString()
                       })
                    .ToPageList(pageIndex, pageSize, ref total);

                return result;
            }
        }
        public void EditOutOrdersStatus(string SelectedDay)
        {

        }
    }
}