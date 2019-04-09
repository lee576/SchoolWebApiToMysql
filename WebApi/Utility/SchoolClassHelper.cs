﻿using DbModel;
using Service;
using SqlSugar;
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
                departmentName += model.p_id + "/";
                departmentName += model.id;
            }
            if (level == 3)
            {
                var class_id = cs.FindById(classid);
                departmentName += class_id.BranchID + "/" + class_id.DepartmentID + "/" + class_id.ID;
            }
            return departmentName;
        }
        public static (string, string) GetClassinfoToidAndLevel2(string schoolcode, int classid, int level)
        {
            tb_school_userService su = new tb_school_userService();
            tb_school_departmentService dstree = new tb_school_departmentService();
            tb_school_classinfoService cs = new tb_school_classinfoService();
            tb_school_departmentinfoService ds = new tb_school_departmentinfoService();
            string departmentid = dstree.FindByClause(x => x.schoolcode == schoolcode && x.p_id == 0).id + "/";
            string departmentName = dstree.FindByClause(x => x.schoolcode == schoolcode && x.p_id == 0).name + "/";
            if (level == 1)
            {
                departmentid += dstree.FindByClause(x => x.id == classid && x.treeLevel == level).id + "/";
                departmentName += dstree.FindByClause(x => x.id == classid && x.treeLevel == level).name;
            }
            if (level == 2)
            {
                var model = dstree.FindByClause(x => x.id == classid && x.treeLevel == level);
                departmentid += model.p_id + "/";
                departmentid += model.id;
                departmentName += model.name + "/";
                departmentName += model.name;
            }
            if (level == 3)
            {
                var class_id = cs.FindById(classid);
                departmentid += class_id.BranchID + "/" + class_id.DepartmentID + "/" + class_id.ID;
                departmentName += dstree.FindByClause(x => x.id == class_id.BranchID).name + "/" + dstree.FindByClause(x => x.id == class_id.DepartmentID).name + "/" + dstree.FindByClause(x => x.id == class_id.department_classID).name;
            }
            return (departmentid, departmentName);
        }
        public static (List<string>, List<string>, List<string>, List<string>) GetDepartmentinfo(string schoolcode)
        {
            tb_school_departmentService dstree = new tb_school_departmentService();
            var data = dstree.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, SqlSugar.OrderByType.Asc);
            if (data.Count() == 0)
            {
                return (null, null, null, null);
            }
            string departmentid = dstree.FindByClause(x => x.schoolcode == schoolcode && x.p_id == 0).id + "/";
            string departmentName = dstree.FindByClause(x => x.schoolcode == schoolcode && x.p_id == 0).name + "/";
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            List<string> list4 = new List<string>();
            foreach (var item in data)
            {
                if (item.treeLevel == 0)
                {
                    list.Add(item.name);
                    continue;
                }
                if (item.treeLevel == 1)
                {
                    var xiao = data.Where(x => x.id == item.p_id).ToList()[0];
                    list2.Add(xiao.name + "/" + item.name);
                    continue;
                }
                if (item.treeLevel == 2)
                {
                    var yuan = data.Where(x => x.id == item.p_id).ToList()[0];
                    var xiao = data.Where(x => x.id == yuan.p_id).ToList()[0];
                    list3.Add(xiao.name + "/" + yuan.name + "/" + item.name);
                    continue;
                }
                if (item.treeLevel == 3)
                {
                    var xi = data.Where(x => x.id == item.p_id).ToList()[0];
                    var yuan = data.Where(x => x.id == xi.p_id).ToList()[0];
                    var xiao = data.Where(x => x.id == yuan.p_id).ToList()[0];
                    list4.Add(xiao.name + "/" + yuan.name + "/" + xi.name + "/" + (item.isType == false ? item.name + "|" : item.name));
                    continue;
                }
            }
            list = list.Distinct().ToList();
            list2 = list2.Distinct().ToList();
            list3 = list3.Distinct().ToList();
            list4 = list4.Distinct().ToList();
            return (list, list2, list3, list4);
        }
        public static int GetDepartmentPid(string schoolcode, string department,
            List<tb_school_department>data,
            List<tb_school_classinfo> _classinfoAll,
            List<tb_school_departmentinfo> _depinfoAll
            )
        {
            tb_school_departmentService dstree = new tb_school_departmentService();
            tb_school_classinfoService _tb_school_classinfoService = new tb_school_classinfoService();
            tb_school_departmentinfoService _tb_school_departmentinfoService = new tb_school_departmentinfoService();
           
            string departmentid = dstree.FindByClause(x => x.schoolcode == schoolcode && x.p_id == 0).id + "/";
            string departmentName = dstree.FindByClause(x => x.schoolcode == schoolcode && x.p_id == 0).name + "/";
            int returnint = 0;
            var data2 = data.Where(x => x.treeLevel == 3).ToList();
            foreach (var item in data2)
            {

                var xi = data.Where(x => x.id == item.p_id).ToList()[0];
                var yuan = data.Where(x => x.id == xi.p_id).ToList()[0];
                var xiao = data.Where(x => x.id == yuan.p_id).ToList()[0];
                var depart2 = xiao.name + "/" + yuan.name + "/" + xi.name + "/" + (item.isType == false ? item.name + "|" : item.name);
                if (depart2.Equals(department))
                {
                    if (department.Contains('|'))
                    {
                        returnint = _classinfoAll.Where(x => x.department_classID == item.id).Select(t => t.ID).ToList()[0];
                        break;
                    }
                    else
                    {
                        returnint = _depinfoAll.Where(x => x.department_treeID == item.id).Select(t => t.ID).ToList()[0];
                        break;
                    }
                }
            }
            return returnint;
        }

    }
}
