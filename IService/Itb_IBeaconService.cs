using System.Collections.Generic;
using DbModel;
using Infrastructure.Service;
namespace IService
{
    public interface Itb_IBeaconService : IServiceBase<tb_ibeacon>
    {
        List<string> GetIBeaconList(string schoolCode);
        List<tb_ibeacon> GetIBeaconInfoToPageList(int pageIndex, int pageSize, ref int total, string schoolCode);
    }
}