using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_school_InfoService : IServiceBase<tb_school_info>
    {
        IEnumerable<tb_school_info> GetSchoolName(string areaName);

        #region 获取校园卡的领取及未领取数据
        /// <summary>
        /// 获取校园卡的领取及未领取数据
        /// </summary>
        /// <param name="school_Code"></param>
        /// <returns></returns>
        SchoolCardInfoViewModel FindSchoolCardInfoBySchoolCode(string school_Code);
        #endregion

        #region 获取校园卡增长数据
        /// <summary>
        /// 获取校园卡增长数据
        /// </summary>
        /// <param name="school_Code"></param>
        /// <returns></returns>
        List<SchoolCardGrowthViewModel> FindSchoolCardGrowth(string school_Code);
        #endregion

      
    }
}