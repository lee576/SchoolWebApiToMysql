using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class SchoolClassHelper
    {
        /// <summary>
        /// 通过班级id获取联动值
        /// </summary>
        /// <param name="schoolcode">学校code</param>
        /// <param name="classid">班级id</param>
        /// <param name="type">班级类型 1学生 2老师</param>
        /// <returns></returns>
        public static string GetClassinfoToid(string schoolcode, int classid, string type)
        {
            tb_school_userService su = new tb_school_userService();
            tb_school_departmentService dstree = new tb_school_departmentService();
            tb_school_classinfoService cs = new tb_school_classinfoService();
            tb_school_departmentinfoService ds = new tb_school_departmentinfoService();
            var model = su.FindByClause(x => x.school_id == schoolcode && x.department_id == classid);
            string departmentName = dstree.FindByClause(x => x.schoolcode == schoolcode && x.p_id == 0).id + "/";
            if (type == "1")
            {
                var cstree = cs.FindListByClause(x => x.ID == model.department_id, t => t.ID, SqlSugar.OrderByType.Asc).ToList()[0];
                departmentName += dstree.FindListByClause(x => x.schoolcode == schoolcode && x.id == cstree.BranchID, t => t.id, SqlSugar.OrderByType.Asc).ToList()[0].id + "/";
                departmentName += dstree.FindListByClause(x => x.schoolcode == schoolcode && x.id == cstree.DepartmentID, t => t.id, SqlSugar.OrderByType.Asc).ToList()[0].id + "/";
                departmentName += cstree.ID;
            }
            else
            {
                var dstree2 = ds.FindListByClause(x => x.ID == model.department_id, t => t.ID, SqlSugar.OrderByType.Asc).ToList()[0];
                departmentName += dstree.FindListByClause(x => x.schoolcode == schoolcode && x.id == dstree2.BranchID, t => t.id, SqlSugar.OrderByType.Asc).ToList()[0].id + "/";
                departmentName += dstree.FindListByClause(x => x.schoolcode == schoolcode && x.id == dstree2.departmentID, t => t.id, SqlSugar.OrderByType.Asc).ToList()[0].id + "/";
                departmentName += dstree2.ID;
            }
            return departmentName;
        }
        public static string GetClassinfoToidAndLevel(string schoolcode, int classid, int level)
        {
            tb_school_userService su = new tb_school_userService();
            tb_school_departmentService dstree = new tb_school_departmentService();
            tb_school_classinfoService cs = new tb_school_classinfoService();
            tb_school_departmentinfoService ds = new tb_school_departmentinfoService();
            string departmentName = "";
            if (level == 1)
            {
                departmentName += dstree.FindByClause(x => x.id == classid && x.treeLevel == level).id;
            }
            if (level == 2)
            {
                var model = dstree.FindByClause(x => x.id == classid && x.treeLevel == level);
                departmentName += dstree.FindByClause(x => x.p_id == model.id).id + "/";
                departmentName += model.id;
            }
            if (level == 3)
            {
                var class_id = cs.FindById(classid);
                departmentName += class_id.BranchID + "/" + class_id.DepartmentID + "/" + class_id.ID;
            }
            return departmentName;
        }
    }
}
