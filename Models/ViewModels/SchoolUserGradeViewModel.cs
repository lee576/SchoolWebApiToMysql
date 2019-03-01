using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class SchoolUserGradeViewModel
    {
        /// <summary>
        /// 学生姓名 
        /// </summary>
        public string student_Name { get; set; }

        /// <summary>
        /// 学生编号
        /// </summary>
        public string student_Id { get; set; }
    }
  

    public class StudentGradeListModel
    {
        /// <summary>
        /// 分数
        /// </summary>
        public string grade { get; set; }

        /// <summary>
        /// 学科
        /// </summary>
        public string discipline { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public bool? isQualified { get; set; }
    }
}
