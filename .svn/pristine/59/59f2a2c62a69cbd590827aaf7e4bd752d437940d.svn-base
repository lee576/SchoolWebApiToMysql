using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_payment_accountsService : IServiceBase<tb_payment_accounts>
    {
        List<tb_payment_accountsAddaccountstatusModel> GetPayMentAccounts(string schoolcode);
        List<tb_payment_accountsAddaccountstatusModel> GetPayMentAccountsPageList(int sEcho, int pageIndex, int pageSize, ref int totalRecordNum, string schoolcode);
    }
}