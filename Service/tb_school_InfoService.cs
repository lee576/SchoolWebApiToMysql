using DbModel;
using IService;
using Infrastructure.Service;
using System.Collections.Generic;
using Infrastructure;
using Models.ViewModels;
using System;
using SqlSugar;

namespace Service
{
    public class tb_school_InfoService : GenericService<tb_school_info>, Itb_school_InfoService
    {
        #region 获取校园卡增长数据
        /// <summary>
        /// 获取校园卡增长数据
        /// </summary>
        /// <param name="school_Code"></param>
        /// <returns></returns>
        public List<SchoolCardGrowthViewModel> FindSchoolCardGrowth(string school_Code)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                DateTime now = DateTime.Now;
                List<string> dtList = new List<string>();
                string SD = "";
                string ED = "";
                // DateTime ED ;
                for (int i = 0; i < 7; ++i)
                {
                    dtList.Add(now.AddDays(-1 * i).ToString("yyyy-MM-dd"));
                    if (i == 6)
                        SD = now.AddDays(-i).ToString("yyyy-MM-dd 00:00:00");
                    if (i == 0)
                        ED = now.AddDays(-i).ToString("yyyy-MM-dd 23:59:59");
                }
                var info = db.Queryable<tb_school_user>()
                    .Where(it => it.school_id == school_Code)
                    .Where(it => it.create_time >= SqlFunc.ToDate(SD) && it.create_time <= SqlFunc.ToDate(ED))
                    .GroupBy(it => it.class_id)
                    .GroupBy(it => it.create_time)
                    .Select(it => new SchoolCardGrowthViewModel()
                    {
                        class_id = it.class_id,
                        create_time = it.create_time,
                        count = SqlFunc.AggregateCount(it.user_id),
                    })
                    .ToList();
                return info;
            }
        }
        #endregion

        #region 获取校园卡的领取及未领取数据
        /// <summary>
        /// 获取校园卡的领取及未领取数据
        /// </summary>
        /// <param name="school_Code"></param>
        /// <returns></returns>
        public SchoolCardInfoViewModel FindSchoolCardInfoBySchoolCode(string school_Code)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                int registered_count = db.Queryable<tb_school_user>().Where(it => it.card_state == 1 && it.school_id == school_Code).ToList().Count;
                int unregistered_count = db.Queryable<tb_school_user>().Where(it => it.card_state == 0 && it.school_id == school_Code).ToList().Count;
                int card_count = db.Queryable<tb_school_user>().Where(it => it.school_id == school_Code).ToList().Count;
                SchoolCardInfoViewModel infoViewModel = new SchoolCardInfoViewModel();
                infoViewModel.unRegisteredCcount = unregistered_count;//未领卡
                infoViewModel.RegisteredCcount = registered_count;//领卡
                infoViewModel.cardCount = card_count;//发卡总数
                return infoViewModel;
            }
        }
        #endregion

       
        public IEnumerable<tb_school_info> GetSchoolName(string areaName)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var dt = db.Ado.SqlQuery<tb_school_info>("select * from tb_school_info where province like '%" + areaName + "%'");
                return dt;
            }
        }
    }
}