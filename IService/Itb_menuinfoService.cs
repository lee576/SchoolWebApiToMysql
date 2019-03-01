using DbModel;
using Infrastructure.Service;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_menuinfoService : IServiceBase<tb_menuinfo>
    {
        List<tb_menuinfo> GetMenu();
        List<tb_menuinfo> GetMenus();
    }
}