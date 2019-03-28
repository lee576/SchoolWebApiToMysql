﻿using DbModel;
using Infrastructure;
using Infrastructure.Service;
using IService;
using Models.ViewModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Service
{
    public class tb_school_user_gradeService : GenericService<tb_school_user_grade>, Itb_school_user_gradeService
    {
        #region 获取成绩单管理列表页的数据（分页及条件查询）
        /// <summary>
        /// 获取成绩单管理列表页的数据（分页及条件查询）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="schoolCode"></param>
        /// <param name="discipline"></param>
        /// <param name="student_id"></param>
        /// <param name="user_name"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IEnumerable<SchoolUserGradeModel> FindSchoolUserGradeList(int pageIndex, int pageSize, ref int total, string schoolCode,
            string discipline = "", string student_idOrName = "", string startTime = "", string endTime = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var list = db.Queryable<tb_school_user_grade, tb_school_user, tb_school_info>((sug, su, si) => new object[] {
                  JoinType.Inner,sug.student_id==su.student_id,
                  JoinType.Inner,sug.school_code==si.School_Code  })
                   .Where((sug, su, si) => sug.school_code == schoolCode)
                   .WhereIF(!string.IsNullOrEmpty(discipline), (sug, su, si) => sug.discipline == discipline)
                   .WhereIF(!string.IsNullOrEmpty(student_idOrName), (sug, su, si) => sug.student_id.Contains(student_idOrName) || su.user_name.Contains(student_idOrName))
                   .WhereIF(!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime), (sug, su, si) => sug.examinationTime >= SqlFunc.ToDate(startTime) && sug.examinationTime <= SqlFunc.ToDate(endTime).AddDays(1))
                   .OrderBy((sug, su, si) => sug.grade, OrderByType.Desc)
              .Select((sug, su, si) => new SchoolUserGradeModel
              {
                  student_id = sug.student_id,
                  user_name = su.user_name,
                  discipline = sug.discipline,
                  examinationTime = sug.examinationTime,
                  examinationName = sug.examinationName,
                  term = sug.term,
                  grade = sug.grade,
                  isQualified = sug.isQualified
              })
              .ToPageList(pageIndex, pageSize, ref total);
                return list;

            }
        }
        #endregion

        #region 根据学校编号获取学科列表
        /// <summary>
        /// 根据学校编号获取学科列表
        /// </summary>
        /// <param name="schoolCode"></param>
        /// <returns></returns>
        public IEnumerable<SchoolDisciplineViewModel> FindDisciplineBySchoolCode(string schoolCode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                return db.Queryable<tb_school_user_grade>().Where(a => a.school_code == schoolCode).Select(a => new SchoolDisciplineViewModel
                {
                    discipline = a.discipline,
                })
              .ToList();
            }
        }

        #endregion

        #region 获取成绩单管理列表页数据（用于导出）
        /// <summary>
        /// 导出成绩单管理列表页数据
        /// </summary>
        /// <param name="schoolCode"></param>
        /// <param name="discipline"></param>
        /// <param name="student_idOrName"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>

        public IEnumerable<SchoolUserGradeModel> InduceSchoolUserGradeLIst(string schoolCode, string discipline = "", string student_idOrName = "", string startTime = "", string endTime = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var list = db.Queryable<tb_school_user_grade, tb_school_user, tb_school_info>((sug, su, si) => new object[] {
                  JoinType.Inner,sug.student_id==su.student_id,
                  JoinType.Inner,sug.school_code==si.School_Code  })
                   .Where((sug, su, si) => sug.school_code == schoolCode)
                   .WhereIF(!string.IsNullOrEmpty(discipline), (sug, su, si) => sug.discipline == discipline)
                   .WhereIF(!string.IsNullOrEmpty(student_idOrName), (sug, su, si) => sug.student_id.Contains(student_idOrName) || su.user_name.Contains(student_idOrName))
                   .WhereIF(!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime), (sug, su, si) => sug.examinationTime >= SqlFunc.ToDate(startTime) && sug.examinationTime <= SqlFunc.ToDate(endTime).AddDays(1))
                   .OrderBy((sug, su, si) => sug.grade, OrderByType.Desc)
              .Select((sug, su, si) => new SchoolUserGradeModel
              {
                  student_id = sug.student_id,
                  user_name = su.user_name,
                  discipline = sug.discipline,
                  examinationTime = sug.examinationTime,
                  examinationName = sug.examinationName,
                  term = sug.term,
                  grade = sug.grade,
                  isQualified = sug.isQualified
              }).ToList();
                return list;

            }
        }
        #endregion

        #region 根据ali_user_id或学期查询学生成绩单
        /// <summary>
        /// 根据ali_user_id或学期查询学生成绩单
        /// </summary>
        /// <param name="student_id"></param>
        /// <param name="school_id"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public IEnumerable<StudentGradeListModel> SelectSchoolUserGradeLIst(string student_id, string school_id, string term = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var list = db.Queryable<tb_school_user_grade, tb_school_user>((sug, su) => new object[] {
                  JoinType.Inner,sug.student_id==su.student_id, })
                  .Where((sug, su) => su.student_id == student_id)
                  .Where((sug, su) => sug.school_code == school_id)
                  .WhereIF(!string.IsNullOrEmpty(term), (sug, su) => sug.term == term)
                  .OrderBy((sug, su) => sug.grade, OrderByType.Desc)
                 .Select((sug, su) => new StudentGradeListModel
                 {
                     discipline = sug.discipline,
                     grade = sug.grade.ToString(),
                     isQualified = sug.isQualified,
                 })
                    .ToList();
                return list;
            }
        }
        #endregion

        public List<SchoolUserGradeModel> GetUserGradeList(int pageIndex, int pageSize, ref int totalRecordNum, string schoolcode, string discipline = "", string class_id = "", string isQualified = "", string selectinfo = "", string term = "")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var list = db.Queryable<tb_school_user_grade, tb_school_user>((sug, su) => new object[] {
                  JoinType.Inner,sug.student_id==su.student_id, })
                  .WhereIF(!string.IsNullOrEmpty(discipline), (sug, su) => sug.discipline == discipline)
                  .WhereIF(!string.IsNullOrEmpty(class_id), (sug, su) => su.department_id == SqlFunc.ToInt32(class_id))
                  .WhereIF(!string.IsNullOrEmpty(isQualified), (sug, su) => sug.isQualified == (isQualified.Equals("合格") ? true : false))
                  .WhereIF(!string.IsNullOrEmpty(selectinfo), (sug, su) => su.student_id == selectinfo || su.user_name == selectinfo)
                  .WhereIF(!string.IsNullOrEmpty(term), (sug, su) => sug.term == term)
                  .Where((sug, su) => sug.school_code == schoolcode)
                  .OrderBy((sug, su) => sug.grade, OrderByType.Desc)
                 .Select((sug, su) => new SchoolUserGradeModel
                 {
                     grade_id = sug.grade_id,
                     student_id = sug.student_id,
                     user_name = su.user_name,
                     discipline = sug.discipline,
                     examinationTime = sug.examinationTime,
                     examinationName = sug.examinationName,
                     term = sug.term,
                     grade = sug.grade,
                     isQualified = sug.isQualified
                 })
                .ToPageList(pageIndex, pageSize, ref totalRecordNum);
                return list;
            }
        }
        public List<tb_school_user_grade> GetGradeAnalysis(string schoolcode, string discipline = "", string term = "", string classid = "")
        {
            string Yclassid = "";
            string Xclassid = "";
            string Bclassid = "";
            if (!string.IsNullOrWhiteSpace(classid))
            {
                var sp = classid.Split(',');
                if (sp.Length == 1)
                {
                    return null;
                }
                if (sp.Length == 2)
                {
                    Yclassid = sp[1];
                }
                if (sp.Length == 3)
                {
                    Xclassid = sp[2];
                }
                if (sp.Length == 4)
                {
                    Bclassid = sp[3];
                }
            }
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var list = db.Queryable<tb_school_user_grade, tb_school_user, tb_school_classinfo>((sug, su, sc) => new object[] {
                  JoinType.Inner,sug.student_id==su.student_id,
                  JoinType.Inner,su.department_id == sc.ID
                })
                .WhereIF(!string.IsNullOrEmpty(discipline), (sug, su, sc) => sug.discipline == discipline)
                .WhereIF(!string.IsNullOrEmpty(term), (sug, su, sc) => sug.term == term)
                .WhereIF(!string.IsNullOrEmpty(Yclassid), (sug, su, sc) => sc.BranchID == SqlFunc.ToInt32(Yclassid))
                .WhereIF(!string.IsNullOrEmpty(Xclassid), (sug, su, sc) => sc.DepartmentID == SqlFunc.ToInt32(Xclassid))
                .WhereIF(!string.IsNullOrEmpty(Bclassid), (sug, su, sc) => sc.ID == SqlFunc.ToInt32(Bclassid))
                .Where((sug, su, sc) => sug.school_code == schoolcode)
                .OrderBy((sug, su, sc) => sug.grade, OrderByType.Desc)
                .Select((sug, su, sc) => sug).ToList();
                return list;
            }
        }

    }
}