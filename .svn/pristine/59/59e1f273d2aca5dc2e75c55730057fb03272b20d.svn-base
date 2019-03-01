using System.Collections.Generic;
using DbModel;
using Infrastructure.Service;
using Models.ViewModels;

namespace IService
{
    public interface Itb_payment_itemService : IServiceBase<tb_payment_item>
    {
        List<view_payment_item> FindListBySchoolCode(string school_code);
        (List<searchPayment>, int) searchPayment(int start, int end, string schoolcode, string student, string name, string status, string sdate, string edate);
       
    }
}