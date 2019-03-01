using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;
using System.Data;

namespace IService
{
    public interface Itb_entrance_recordService : IServiceBase<tb_entrance_record>
    {
        int GetListInfo(string schoolcode,string stime,string etime);
        int Get_LibraryCount(string schoolcode,string devicetype);

        List<LibraryRanking> Get_LibraryRanking(string stime, string etime, string schoolcode,string devicetype);
        string GetdataRate(string schoolcode);
    }
}