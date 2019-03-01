using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_payment_electricitybillsService : IServiceBase<tb_payment_electricitybills>
    {
        IEnumerable<Electricitybills> GetelectricitybillsInfo(int pageIndex, int pageSize, ref int total, int schoolcode,
        string room_id = "", string ordernumber = "", string stime = "", string etime = "");
        void EditOutOrdersStatus(string SelectedDay);
    }
}