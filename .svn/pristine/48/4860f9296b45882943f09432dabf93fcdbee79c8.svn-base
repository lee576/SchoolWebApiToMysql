using DbModel;
using IService;
using Infrastructure.Service;
using System.Collections.Generic;
using Infrastructure;
using SqlSugar;
using Models.ViewModels;

namespace Service
{
    public class tb_school_departmentService : GenericService<tb_school_department>,Itb_school_departmentService
    {
        public DataTable GetDepartmentInfo(string schoolcode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                //var list3 = db.Queryable<tb_school_department>().GroupBy(it => new { it.name}).Select(it => new {it.name }).ToList();
                //return list3;
                var dt = db.Ado.GetDataTable("select department from tb_school_user where school_id='"+ schoolcode + "' GROUP BY department");
                return dt;
            }
           
        }
        public DataTable selbypid(string schoolcode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var dt = db.Ado.GetDataTable("select * from tb_school_department where schoolcode = '" + schoolcode + "' and p_id = 0 ");
                return dt;
            }
        }
        public DataTable selbyname(string schoolcode, string name)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var dt = db.Ado.GetDataTable("select * from tb_school_department where schoolcode = '" + schoolcode + "' and name = '" + name + "'");
                return dt;
            }
        }
        public int ADD(string schoolcode, string name, int pid, int level)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                tb_school_departmentService sv = new tb_school_departmentService();
                tb_school_department deparment = new tb_school_department()
                {
                    name = name,
                    schoolcode = schoolcode,
                    p_id = pid,
                    treeLevel = level
                };
                //int depid = (int)sv.Insert(deparment);
                //if (pid == 0)
                //{
                //    return depid;
                //}
                //tb_school_userService us = new tb_school_userService();
                //var t10 = db.Updateable<tb_school_user>().UpdateColumns(it => new tb_school_user() { department_id = depid}).Where(x => x.school_id == schoolcode && x.department.Contains(name)).ExecuteCommand();
                ////var data = us.FindListByClause(x => x.school_id == schoolcode && x.department.Contains(name), t => t.user_id, OrderByType.Asc) as List<tb_school_user>;
                ////db.Updateable(data).ReSetValue(it => it.department_id == (it.Name + 1)).ExecuteCommand();
                ////data.ForEach(p => p.department_id=depid);
                ////var t7 = db.Updateable(data).ExecuteCommand();
                return (int)sv.Insert(deparment);
               
            }
            
        }
    }
}