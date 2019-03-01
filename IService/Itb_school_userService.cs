using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_school_userService : IServiceBase<tb_school_user>
    {
        bool SynchronizationInfo(List<tb_school_user> list);
        bool AddUserInfoToSDYGYXY(tb_school_user user);
        List<tb_school_user> GetSchoolUserInfo(string schoolcode, string depamentid = "", string card_state = "", string selectinfo = "");
        #region 获取校园卡人员管理信息
        /// <summary>
        /// 获取校园卡人员管理信息
        /// </summary>
        /// <param name="userNameOrId"></param>
        /// <param name="branchId"></param>
        /// <param name="departmentId"></param>
        /// <param name="department_classId"></param>
        /// <param name="classId"></param>
        /// <param name="card_state"></param>
        /// <returns></returns>
        List<SchoolStudentViewModel> FindSchoolStudentInfo(string school_Code, int pageIndex, int pageSize, ref int total, string userNameOrId = "", string branchId = "", string departmentId = "",
            string department_classId = "", string classId = "", string card_state = "" );
        #endregion

        #region 导出校园卡人员管理信息
        /// <summary>
        /// 导出校园卡人员管理信息
        /// </summary>
        /// <param name="userNameOrId"></param>
        /// <param name="branchId"></param>
        /// <param name="departmentId"></param>
        /// <param name="department_classId"></param>
        /// <param name="classId"></param>
        /// <param name="card_state"></param>
        /// <returns></returns>
        List<InduceSchoolUserViewModel> InduceSchoolUserInfo(string school_Code, string userNameOrId = "", string branchId = "", string departmentId = "",
            string department_classId = "", string classId = "", string card_state = "");
        #endregion
    }
}