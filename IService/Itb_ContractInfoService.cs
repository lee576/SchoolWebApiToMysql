using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IService
{
    public interface Itb_ContractInfoService : IServiceBase<tb_ContractInfo>
    {
        #region 获取签约管理列表页数据（分页）
        /// <summary>
        /// 获取签约管理列表页数据（分页）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="schoolCode_Name"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        IEnumerable<ContractInfoViewModel> FindContracInfoList(int pageIndex, int pageSize, ref int total,
            string schoolCode_Name = "", string startTime = "");
        #endregion

        #region 获取学校的卡的领取未领取数据
        /// <summary>
        /// 获取学校的卡的领取未领取数据
        /// </summary>
        /// <param name="school_Code"></param>
        /// <returns></returns>
        SchoolCardInfoViewModel FindSchoolCardInfoBySchoolCode(string school_Code);
        #endregion

        #region 获取学校流水数据
        /// <summary>
        /// 获取学校流水数据
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="merchantCode"></param>
        /// <returns></returns>
        IEnumerable<TransactionFlowingViewModel> FindTransactionFlowingInfo(string stime, string merchantCode);
        #endregion
        
        #region 根据签约人获取学校领卡及流水数据
        /// <summary>
        /// 根据签约人获取学校领卡及流水数据
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="merchantCode"></param>
        /// <returns></returns>
        SchoolCardFlowingViewModel FindSchoolCardFlowingInfo(string ali_user_Id, string stime);
        #endregion

    }
}
