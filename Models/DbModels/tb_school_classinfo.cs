using System;
using System.Linq;
using System.Text;

namespace DbModel
{
    ///<summary>
    ///
    ///</summary>
    public partial class tb_school_classinfo
    {
           public tb_school_classinfo(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int ID {get;set;}

           /// <summary>
           /// Desc:学校编号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string schoolcode {get;set;}

           /// <summary>
           /// Desc:分院ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? BranchID {get;set;}

           /// <summary>
           /// Desc:系ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? DepartmentID {get;set;}

           /// <summary>
           /// Desc:班级对应部门表id
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? department_classID {get;set;}

    }
}
