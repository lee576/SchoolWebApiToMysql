using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;
namespace IService
{
    public interface Ish_areaService : IServiceBase<sh_area>
    {
        List<AreaInfoList> GetAreaInfo();
    }
}