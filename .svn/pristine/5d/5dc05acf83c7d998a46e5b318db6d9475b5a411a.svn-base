using DbModel;
using IService;
using Infrastructure.Service;
using System.Collections.Generic;
using Infrastructure;
using System.Linq;
using System;
using System.Data;
using Models.ViewModels;

namespace Service
{
    public class tb_entrance_recordService : GenericService<tb_entrance_record>,Itb_entrance_recordService
    {
        public int GetListInfo(string schoolcode ,string stime, string etime)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                string sql = $"SELECT count(*) FROM tb_school_user AS a INNER JOIN dbo.tb_entrance_record AS b ON a.user_id = b.user_id WHERE convert(varchar(10), b.open_time, 120) >= '"+stime+"' AND convert(varchar(10), b.open_time, 120) <= '"+etime+"' And b.entrance_status = '进' AND a.school_id = "+schoolcode+"";
                return db.Ado.SqlQuery<int>(sql).FirstOrDefault();
            }
        }
        public int Get_LibraryCount(string schoolcode,string devicetype)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                DateTime day = DateTime.Now;
                string sday = day.ToString("yyyy-MM-dd");
                //string sql = $"SELECT b.user_id FROM tb_school_user AS a INNER JOIN dbo.tb_entrance_record AS b ON a.user_id = b.user_id WHERE convert(varchar(10),b.open_time,120) ='" + sday + "' And b.entrance_status='进' and a.school_id="+schoolcode+"";
                string sql = @"SELECT b.user_id  FROM tb_school_user AS a INNER JOIN dbo.tb_entrance_record AS b ON a.user_id = b.user_id inner join tb_school_device as c on b.device_id = c.device_id WHERE convert(varchar(10),b.open_time,120) = '" + sday + "' And b.entrance_status = '进' and a.school_id = '"+schoolcode+ "' and device_state = "+ devicetype + "";
                return db.Ado.SqlQuery<string>(sql).Distinct().ToList().Count;
            }
        }
        public List<LibraryRanking> Get_LibraryRanking(string stime, string etime, string schoolcode,string devicetype)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                //string sql = $"select * from (SELECT t.user_name,COUNT(*)AS times FROM (SELECT a.user_name FROM tb_school_user AS a INNER JOIN dbo.tb_entrance_record AS b ON a.user_id = b.user_id WHERE convert(varchar(10), b.open_time, 120) >= '"+ stime + "' AND convert(varchar(10), b.open_time, 120) <= '"+etime+"' And b.entrance_status = '进' AND a.school_id = "+schoolcode+") t GROUP BY t.user_name) as tt order by tt.times desc";
                string sql = @"select * from (SELECT t.user_name,COUNT(*)AS times FROM ( 
                            SELECT a.user_name FROM tb_school_user AS a INNER JOIN dbo.tb_entrance_record AS b ON a.user_id = b.user_id inner join tb_school_device as c on b.device_id = c.device_id 
                            WHERE convert(varchar(10), b.open_time, 120) >= '" + stime + "' AND convert(varchar(10), b.open_time, 120) <= '"+etime+"' And b.entrance_status = '进' AND a.school_id = "+schoolcode+" and c.device_state='"+ devicetype + "') t GROUP BY t.user_name) as tt order by tt.times desc";
                List<LibraryRanking> data = db.Ado.SqlQuery<LibraryRanking>(sql);
                return data;
            }
        }
        public string GetdataRate(string schoolcode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                string sql = $"select count(*) from tb_school_user where school_id = "+ schoolcode + " and card_state = 1";
                var countcall = db.Ado.SqlQuery<int>(sql).FirstOrDefault();
                sql = $"select count(*) from tb_school_user as a inner join (SELECT * FROM dbo.tb_entrance_record WHERE id  IN(SELECT MIN(id) FROM dbo.tb_entrance_record GROUP BY user_id)) t on a.user_id = t.user_id where a.school_id = "+ schoolcode + "";
                var lkcount = db.Ado.SqlQuery<int>(sql).FirstOrDefault();
                string dRate = countcall + "-" + lkcount;
                return dRate;
            }
        }
            //select count(*) from tb_school_user where school_id = '10027' and card_state = 1

        //SELECT count(*) FROM dbo.tb_entrance_record WHERE id  IN (SELECT MIN(id) FROM dbo.tb_entrance_record GROUP BY user_id)
    }

}