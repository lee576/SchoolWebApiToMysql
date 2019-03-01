using DbModel;
using IService;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;
using Infrastructure;

namespace Service
{
    public class sh_areaService : GenericService<sh_area>,Ish_areaService
    {
        //select * from sh_area where pid=0  order by [first] collate Chinese_PRC_CI_AS
        public List<AreaInfoList> GetAreaInfo()
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                List<AreaInfoList> lal = new List<AreaInfoList>();
                string[] zm = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                //var dt = db.Ado.SqlQuery<sh_area>("select * from sh_area where pid=0  order by [first] collate Chinese_PRC_CI_AS");
                var dt = db.Ado.SqlQuery<sh_area>("select * from sh_area where pid = 0 ORDER BY CONVERT(first USING GBK) ASC");
                
                int index = 0;
                foreach (var item in zm)
                {
                    AreaInfoList al = new AreaInfoList();
                    List<AreaInfo> las = new List<AreaInfo>();
                    foreach (var item2 in dt)
                    {
                        AreaInfo info = new AreaInfo();
                        if (item==item2.first)
                        {
                            index++;
                            info.id = "v" + index;
                            info.cityName = item2.name;
                            info.type = "0";
                            las.Add(info);
                        }
                    }
                    if (las.Count==0)
                    {
                        continue;
                    }
                    al.letter = item;
                    al.show = true;
                    al.data=las;
                    lal.Add(al);
                }
                return lal;
            }
        }
       
    }
}