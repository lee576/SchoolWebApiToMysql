using DbModel;
using Infrastructure.Service;
using Models.ViewModels;

namespace IService
{
    public interface Itb_payment_ar_recordService : IServiceBase<tb_payment_ar_record>
    {
        payOk GetPayOk(string out_trade_no);
    }
}