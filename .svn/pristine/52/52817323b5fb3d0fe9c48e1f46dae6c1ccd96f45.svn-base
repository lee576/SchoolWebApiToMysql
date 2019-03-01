using DbModel;
using IService;
using Infrastructure.Service;
using Infrastructure;
using System.Collections.Generic;

namespace Service
{
    public class tb_menuinfoService : GenericService<tb_menuinfo>,Itb_menuinfoService
    {
        /// <summary>
        /// 获取根节点菜单
        /// </summary>
        /// <returns></returns>
        public List<tb_menuinfo> GetMenu()
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                return db.Queryable<tb_menuinfo>().Where(t => t.p_id == 0).OrderBy(t => t.index).ToList();
            }
        }

        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <returns></returns>
        public List<tb_menuinfo> GetMenus()
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                return db.Queryable<tb_menuinfo>().Where(t => t.p_id > 0).OrderBy(t => t.index).ToList();
            }
        }
    }
}