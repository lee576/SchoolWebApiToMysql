using DbModel;
using Infrastructure.Service;
using System.Collections.Generic;
using Models.ViewModels;

namespace IService
{
    public interface Itb_payment_waterrentService : IServiceBase<tb_payment_waterrent>
    {
       IEnumerable<tb_payment_waterrent> GetWaterInfo(int pageIndex, int pageSize, int schoolcode, ref int total,
     string ordernumber = "", string stime = "", string etime = "");

        List<WaterrentCount> GetSumOrderandSumPay(int schoolcode);
    }
}